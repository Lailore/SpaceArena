using Collections.Pooled;
using UnityEngine;
using Zenject;

namespace Feito {
    [RequireComponent(typeof(BootstrapInstaller))]
    public abstract class BootstrapContainer : MonoBehaviour {
        [Inject]
        private void Inject(IInstantiator instantiator) {
            Instantiator = instantiator;
        }

        protected IInstantiator Instantiator;

        public abstract PooledList<IBootstrap> Get();
    }
}