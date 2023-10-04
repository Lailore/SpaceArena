using System.Threading.Tasks;
using JetBrains.Collections.Viewable;
using JetBrains.Lifetimes;
using UnityEngine;

namespace Feito.SpaceArena {
    public interface IBridge : IPoolable {
        ISource<Collision2D> CollisionDetected { get; }
        Task SetInputs(Lifetime lifetime, IReadonlyProperty<float> engineInput, IReadonlyProperty<float> wheelInput);
        Task Teleport(Vector2 position);
    }
}