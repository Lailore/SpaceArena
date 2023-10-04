using System;
using System.Threading.Tasks;
using JetBrains.Lifetimes;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

namespace Feito.SpaceArena {
    public class SpaceshipFactory {
        private readonly IInstantiator _instantiator;
        private GameObject _prefab;

        public SpaceshipFactory(IInstantiator instantiator) {
            _instantiator = instantiator;
        }

        public Task<IBridge> Create() {
            var context = _instantiator.InstantiatePrefabForComponent<GameObjectContext>(_prefab);
            var bootstrapExecutor = context.Container.Resolve<IBootstrapExecutor>();
            return bootstrapExecutor.ExecuteAfterInitialization(() =>
                Task.FromResult(context.Container.Resolve<IBridge>())
            );
        }

        private void Initialize(GameObject prefab) {
            _prefab = prefab;
        }

        public class Bootstrap : IBootstrap {
            private readonly Lifetime _appLifetime;
            private readonly SpaceshipFactory _factory;
            private readonly Settings _settings;

            public Bootstrap(Lifetime appLifetime, Settings settings, SpaceshipFactory factory) {
                _appLifetime = appLifetime;
                _settings = settings;
                _factory = factory;
            }

            public async Task Run() {
                var prefab = await _settings.reference.LoadAssetAsync(_appLifetime);
                _factory.Initialize(prefab);
            }
        }

        [Serializable]
        public class Settings {
            public AssetReferenceGameObject reference;
        }
    }
}