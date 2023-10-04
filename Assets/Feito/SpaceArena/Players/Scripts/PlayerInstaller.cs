using Zenject;

namespace Feito.SpaceArena {
    public class PlayerInstaller : MonoInstaller {
        public override void InstallBindings() {
            Container.BindInterfacesAndSelfTo<PlayerController>().AsSingle();
        }
    }
}