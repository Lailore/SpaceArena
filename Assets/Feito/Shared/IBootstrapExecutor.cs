using System;
using System.Threading.Tasks;

namespace Feito {
    public interface IBootstrapExecutor {
        Task ExecuteAfterInitialization(Func<Task> executor);
        Task<TValue> ExecuteAfterInitialization<TValue>(Func<Task<TValue>> executor);
    }
}