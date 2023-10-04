using Zenject;

namespace Feito.SpaceArena {
    public class ScorerInstaller : MonoInstaller {
        public override void InstallBindings() {
            Container.BindInterfacesAndSelfTo<Scorer>().AsSingle();
        }
    }
}