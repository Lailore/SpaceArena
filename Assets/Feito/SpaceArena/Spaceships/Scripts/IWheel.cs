using System.Threading.Tasks;
using FluentResults;
using JetBrains.Collections.Viewable;
using JetBrains.Lifetimes;

namespace Feito.SpaceArena {
    public interface IWheel {
        Task<Result> Run(Lifetime lifetime, IReadonlyProperty<float> strength);
    }
}