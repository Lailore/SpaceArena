using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace Feito {
    public class BootstrapRunner : IInitializable, IBootstrapRunner {
        private readonly BootstrapContainer[] _containers;
        private readonly TaskCompletionSource<int> _tcs = new();

        public BootstrapRunner(BootstrapContainer[] containers) {
            _containers = containers;
        }

        public Task Initialization => _tcs.Task;

        public async void Initialize() {
            foreach (var container in _containers) {
                using var bootstraps = container.Get();
                await Task.WhenAll(bootstraps.Select(x => x.Run()));
            }

            _tcs.SetResult(0);
        }
    }
}