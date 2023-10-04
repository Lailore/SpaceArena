using System;
using System.Threading.Tasks;

namespace Feito {
    public class BootstrapExecutor : IBootstrapExecutor {
        private readonly IBootstrapRunner _bootstrapRunner;

        public BootstrapExecutor(IBootstrapRunner bootstrapRunner) {
            _bootstrapRunner = bootstrapRunner;
        }

        public async Task ExecuteAfterInitialization(Func<Task> executor) {
            await _bootstrapRunner.Initialization;
            await executor();
        }

        public async Task<TValue> ExecuteAfterInitialization<TValue>(Func<Task<TValue>> executor) {
            await _bootstrapRunner.Initialization;
            return await executor();
        }
    }
}