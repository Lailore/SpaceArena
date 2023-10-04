using System.Threading.Tasks;

namespace Feito.SpaceArena {
    public interface ISpaceshipFactory {
        Task<IBridge> Create();
    }
}