using UnityEngine;
using Zenject;

namespace Feito {
    public class BootstrapInstaller : MonoInstaller {
        [SerializeField] private BootstrapContainer[] containers;

        public override void InstallBindings() {
            Container.Bind<BootstrapContainer[]>().FromInstance(containers);
            Container.BindInterfacesTo<BootstrapExecutor>().AsSingle();
            Container.BindInterfacesTo<BootstrapRunner>().AsSingle();
        }
    }
}