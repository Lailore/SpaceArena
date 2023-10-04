using System.Threading.Tasks;

namespace Feito {
    public interface ISimpleRouter {
        Task MainMenu();
        Task Game();
        Task GameOver();
    }
}