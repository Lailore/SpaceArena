using System;
using System.Threading.Tasks;
using FluentResults;
using JetBrains.Collections.Viewable;
using JetBrains.Lifetimes;
using JetBrains.Threading;
using Redcode.Awaiting;
using Stateless;
using Unity.Profiling;
using UnityEngine;

namespace Feito.SpaceArena {
    public class Wheel : MonoBehaviour, IWheel {
        private static readonly ProfilerMarker TorqueProfilerMarker = new("Feito.SpaceArena.Wheel.Loop");
        [SerializeField] private Rigidbody2D rb;
        [SerializeField] private Settings settings;

        private readonly StateMachine<State, Trigger> _machine = new(State.Invalid);

        private StateMachine<State, Trigger>.TriggerWithParameters<Lifetime, IReadonlyProperty<float>>
            _enableTrigger;

        private bool CanEnable => _machine.CanFire(Trigger.Enable);

        public async Task<Result> Run(Lifetime lifetime, IReadonlyProperty<float> strength) {
            if (CanEnable) {
                await _machine.FireAsync(_enableTrigger, lifetime, strength);
                return Result.Ok();
            }

            return Result.Fail("Can't start controlling wheel");
        }

        private Task Initialize() {
            _enableTrigger = _machine.SetTriggerParameters<Lifetime, IReadonlyProperty<float>>(Trigger.Enable);

            _machine.Configure(State.Invalid)
                .Permit(Trigger.Initialize, State.Initializating)
                .OnActivate(() => _machine.Fire(Trigger.Initialize));

            _machine.Configure(State.Initializating)
                .Permit(Trigger.Ready, State.Ready)
                .OnEntry(() => _machine.Fire(Trigger.Ready));

            _machine.Configure(State.Ready)
                .Permit(Trigger.Enable, State.Enabled);

            _machine.Configure(State.Enabled)
                .PermitReentry(Trigger.Enable)
                .OnEntryFrom(_enableTrigger, OnEnabling);

            return _machine.ActivateAsync();
        }

        //Тут можно было всё отправлять в общую систему, которая будет без переключения контекста "крутить" все корабли,
        //но в ТЗ указанно, что лучше без ECS подхода. В любом случае это точка оптимизации, если объектов будет много.
        private void OnEnabling(Lifetime lifetime, IReadonlyProperty<float> strength) {
            gameObject.GetActiveLifetime().IsActive.View(lifetime, (lf, isActive) => Loop(lf, isActive, strength).NoAwait());
        }

        private async Task Loop(Lifetime lifetime, bool isActive, IReadonlyProperty<float> strength) {
            if (isActive) {
                await new WaitForFixedUpdate();
                while (lifetime.IsAlive) {
                    using (TorqueProfilerMarker.Auto()) {
                        rb.AddTorque(strength.Value * settings.torqueForce);
                        rb.angularVelocity = Math.Clamp(rb.angularVelocity, -settings.maxTorque, settings.maxTorque);
                    }
                    await new WaitForFixedUpdate();
                }
            }
        }

        public class Bootstrap : IBootstrap {
            private readonly Wheel _wheel;

            public Bootstrap(Wheel wheel) {
                _wheel = wheel;
            }

            public Task Run() {
                return _wheel.Initialize();
            }
        }

        private enum State {
            Invalid,
            Initializating,
            Ready,
            Enabled
        }

        private enum Trigger {
            Initialize,
            Ready,
            Enable
        }

        [Serializable]
        public class Settings {
            public float torqueForce;
            public float maxTorque;
        }
    }
}