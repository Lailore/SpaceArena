using System.Threading.Tasks;
using JetBrains.Collections.Viewable;
using JetBrains.Lifetimes;
using JetBrains.Threading;
using Redcode.Awaiting;
using UnityEngine;

namespace Feito.SpaceArena {
    public class Spawner {
        private readonly Lifetime _appLifeTime;
        private readonly IPool<IBridge> _pool;
        private readonly IScorer _scorer;

        public Spawner(Lifetime appLifeTime, IPool<IBridge> pool, IScorer scorer) {
            _pool = pool;
            _appLifeTime = appLifeTime;
            _scorer = scorer;
        }

        private void Start() {
            _scorer.Reset();
            Loop().NoAwait();
        }

        private async Task Loop() {
            var difficultyLevel = 16;
            while (_appLifeTime.IsAlive) {
                difficultyLevel *= difficultyLevel;
                await _appLifeTime.UsingNestedAsync(async lifetime => {
                    await SpawnEnemies(lifetime, difficultyLevel);
                });
                await new WaitForSeconds(3);
            }
        }

        private async Task SpawnEnemies(Lifetime lifetime, int difficultyLevel) {
            for (var i = 0; i < difficultyLevel; i++) {
                lifetime.ThrowIfNotAlive();
                await lifetime.ExecuteAsync(async () => {
                    var bridge = await _pool.Rent(lifetime);
                    await bridge.SetInputs(lifetime,
                        new ViewableProperty<float>(Random.Range(0.5f, 1f)),
                        new ViewableProperty<float>(Random.Range(-1f, 1f))
                    );

                    await bridge.Teleport(new Vector2(Random.Range(-15, 15), Random.Range(-15, 15)));
                    await _scorer.Increment();
                });
                
                await new WaitForSeconds(Random.Range(0.1f, 2f));
            }
        }

        public class Bootstrap : IBootstrap {
            private readonly IBootstrapExecutor _bootstrapExecutor;
            private readonly Spawner _spawner;

            public Bootstrap(Spawner spawner, IBootstrapExecutor bootstrapExecutor) {
                _spawner = spawner;
                _bootstrapExecutor = bootstrapExecutor;
            }

            public Task Run() {
                _bootstrapExecutor.ExecuteAfterInitialization(() => {
                    _spawner.Start();
                    return Task.CompletedTask;
                }).NoAwait();

                return Task.CompletedTask;
            }
        }
    }
}