using JetBrains.Collections.Viewable;
using UnityEngine;

namespace Feito {
    public class CollisionSensor2D : MonoBehaviour, ICollisionSensor2D {
        private readonly Signal<Collision2D> _enter = new();

        private void OnCollisionEnter2D(Collision2D other) {
            _enter.Fire(other);
        }

        public ISource<Collision2D> Enter => _enter;
    }
}