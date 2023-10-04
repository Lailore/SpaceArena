using System;
using System.Threading.Tasks;
using UnityEngine.AddressableAssets;

namespace Feito {
    public class SimpleRouter : ISimpleRouter {
        private readonly Settings _settings;

        private SimpleRouter(Settings settings) {
            _settings = settings;
        }

        public Task MainMenu() {
            return Addressables.LoadSceneAsync(_settings.mainMenu).Task;
        }

        public Task Game() {
            return Addressables.LoadSceneAsync(_settings.game).Task;
        }

        public Task GameOver() {
            return Addressables.LoadSceneAsync(_settings.gameOver).Task;
        }

        [Serializable]
        public class Settings {
            public AssetReference mainMenu;
            public AssetReference game;
            public AssetReference gameOver;
        }
    }
}