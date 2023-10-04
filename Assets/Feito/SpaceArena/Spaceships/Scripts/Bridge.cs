using System;
using System.Threading.Tasks;
using JetBrains.Collections.Viewable;
using JetBrains.Lifetimes;
using UnityEngine;

namespace Feito.SpaceArena {
    public class Bridge : IBridge {
        private readonly ICollisionSensor2D _collisionSensor;
        private readonly IEngine _engine;
        private readonly IRoot _root;
        private readonly IWheel _wheel;

        public Bridge(IEngine engine, IWheel wheel, IRoot root, ICollisionSensor2D collisionSensor) {
            _engine = engine;
            _wheel = wheel;
            _root = root;
            _collisionSensor = collisionSensor;
        }

        public ISource<Collision2D> CollisionDetected => _collisionSensor.Enter;

        public async Task SetInputs(Lifetime lifetime, IReadonlyProperty<float> engineInput,
            IReadonlyProperty<float> wheelInput) {
            var engineControl = await _engine.Run(lifetime, engineInput);
            if (engineControl.IsFailed) {
                throw new Exception();
            }

            var wheelControl = await _wheel.Run(lifetime, wheelInput);
            if (wheelControl.IsFailed) {
                throw new Exception();
            }
        }

        public Task Teleport(Vector2 position) {
            _root.Teleport(position);
            return Task.CompletedTask;
        }

        public Task OnRent(Lifetime lifetime) {
            return _root.Enable(lifetime);
        }
    }
}