using System.Threading.Tasks;
using JetBrains.Collections.Viewable;
using JetBrains.Lifetimes;
using JetBrains.Threading;
using Redcode.Awaiting;
using UnityEngine;

namespace Feito.SpaceArena {
    public class PlayerController {
        private readonly ISimpleRouter _simpleRouter;

        public PlayerController(ISimpleRouter simpleRouter) {
            _simpleRouter = simpleRouter;
        }

        private (IReadonlyProperty<float> engineInput, IReadonlyProperty<float> wheelInput) GetInputs(
            Lifetime lifetime) {
            var engineInput = new ViewableProperty<float>();
            var wheelInput = new ViewableProperty<float>();
            Loop(lifetime, engineInput, wheelInput).NoAwait();
            return (engineInput, wheelInput);
        }

        private async Task Loop(Lifetime lifetime, ViewableProperty<float> engineInput,
            ViewableProperty<float> wheelInput) {
            await new WaitForFixedUpdate();
            while (lifetime.IsAlive) {
                engineInput.Value = Input.GetAxis("Vertical");
                wheelInput.Value = -Input.GetAxis("Horizontal");
                await new WaitForFixedUpdate();
            }
        }

        private Task FinishGame() {
            return _simpleRouter.GameOver();
        }

        public class Bootstrap : IBootstrap {
            private readonly Lifetime _appLifetime;
            private readonly PlayerController _playerController;
            private readonly IPool<IBridge> _spaceshipsPool;

            public Bootstrap(Lifetime appLifetime, PlayerController playerController, IPool<IBridge> spaceshipsPool) {
                _appLifetime = appLifetime;
                _playerController = playerController;
                _spaceshipsPool = spaceshipsPool;
            }

            public async Task Run() {
                var (engineInput, wheelInput) = _playerController.GetInputs(_appLifetime);
                var bridge = await _spaceshipsPool.Rent(_appLifetime);
                await bridge.SetInputs(_appLifetime, engineInput, wheelInput);

                bridge.CollisionDetected.Advise(_appLifetime, _ => _playerController.FinishGame().NoAwait());
            }
        }
    }
}