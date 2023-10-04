using Zenject;

namespace Feito.SpaceArena {
    public class SpaceshipInstaller : MonoInstaller {
        public override void InstallBindings() {
            Container.BindInterfacesTo<Bridge>().AsSingle();
        }
    }
}