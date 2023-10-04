using Zenject;

namespace Feito.SpaceArena {
    public class EnemiesInstaller : MonoInstaller {
        public override void InstallBindings() {
            Container.BindInterfacesAndSelfTo<Spawner>().AsSingle();
        }
    }
}