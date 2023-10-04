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
    public class Engine : MonoBehaviour, IEngine {
        private static readonly ProfilerMarker MoveProfilerMarker = new("Feito.SpaceArena.Engine.Loop");

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

            return Result.Fail("Can't start controlling engine");
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

        //Тут можно было всё отправлять в общую систему, которая будет без переключения контекста "двигать" все двигатели,
        //но в ТЗ указанно, что лучше без ECS подхода. В любом случае это точка оптимизации, если объектов будет много.
        private void OnEnabling(Lifetime lifetime, IReadonlyProperty<float> strength) {
            gameObject.GetActiveLifetime().IsActive
                .View(lifetime, (lf, isActive) => Loop(lf, isActive, strength).NoAwait());
        }

        private async Task Loop(Lifetime lifetime, bool isActive, IReadonlyProperty<float> strength) {
            await new WaitForFixedUpdate();
            if (isActive) {
                var rbTransform = rb.transform;
                while (lifetime.IsAlive) {
                    using (MoveProfilerMarker.Auto()) {
                        rb.AddForce(strength.Value * settings.moveForce * rbTransform.up);
                        rb.velocity = Vector2.ClampMagnitude(rb.velocity, settings.maxSpeed);
                    }

                    await new WaitForFixedUpdate();
                }
            }
        }

        public class Bootstrap : IBootstrap {
            private readonly Engine _engine;

            public Bootstrap(Engine engine) {
                _engine = engine;
            }

            public Task Run() {
                return _engine.Initialize();
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
            public float moveForce;
            public float maxSpeed;
        }
    }
}