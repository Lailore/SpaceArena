using System.Threading.Tasks;
using JetBrains.Lifetimes;

namespace Feito {
    public interface IPool<TItem> where TItem : IPoolable {
        Task<TItem> Rent(Lifetime lifetime);
    }
}