using System.Threading.Tasks;
using JetBrains.Lifetimes;

namespace Feito {
    public interface IPoolable {
        public Task OnRent(Lifetime lifetime);
    }
}