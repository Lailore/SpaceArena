using JetBrains.Collections.Viewable;
using UnityEngine;

namespace Feito {
    public interface ICollisionSensor2D {
        ISource<Collision2D> Enter { get; }
    }
}