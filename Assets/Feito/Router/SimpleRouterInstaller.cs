using UnityEngine;
using Zenject;

namespace Feito {
    public class SimpleRouterInstaller : MonoInstaller {
        [SerializeField] private SimpleRouter.Settings settings;

        public override void InstallBindings() {
            Container.BindInstance(settings);
            Container.BindInterfacesAndSelfTo<SimpleRouter>().AsSingle();
        }
    }
}