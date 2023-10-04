using JetBrains.Lifetimes;
using Zenject;

namespace Feito {
    public class LifetimeInstaller : MonoInstaller {
        public override void InstallBindings() {
            Container.Bind<Lifetime>().FromInstance(gameObject.GetLifetime());
        }
    }
}