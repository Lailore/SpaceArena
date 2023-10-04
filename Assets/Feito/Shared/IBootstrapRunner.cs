using System.Threading.Tasks;

namespace Feito {
    public interface IBootstrapRunner {
        Task Initialization { get; }
    }
}