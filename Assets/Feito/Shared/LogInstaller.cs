using JetBrains.Diagnostics;
using JetBrains.Diagnostics.Internal;
using Zenject;

namespace Feito {
    public class LogInstaller : MonoInstaller {
        public override void InstallBindings() {
            Log.UsingLogFactory(new SingletonLogFactory(new UnityLogger()));
        }
    }
}