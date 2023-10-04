using UnityEngine;
using Zenject;

namespace Feito {
    public class Bootstrap : MonoBehaviour {
        [Inject]
        public void Inject(ISimpleRouter simpleRouter, IBootstrapExecutor bootstrapExecutor) {
            _simpleRouter = simpleRouter;
            _bootstrapExecutor = bootstrapExecutor;
        }

        private IBootstrapExecutor _bootstrapExecutor;
        private ISimpleRouter _simpleRouter;

        private void Start() {
            _bootstrapExecutor.ExecuteAfterInitialization(_simpleRouter.MainMenu);
        }
    }
}