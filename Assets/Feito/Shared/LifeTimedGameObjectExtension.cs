using JetBrains.Lifetimes;
using UnityEngine;

namespace Feito {
    public static class LifetimedGameObjectExtension {
        public static Lifetime GetLifetime(this GameObject gameObject) {
            var component = gameObject.GetComponent<Component>() ?? gameObject.AddComponent<Component>();
            return component.Lifetime;
        }

        public class Component : MonoBehaviour {
            private readonly LifetimeDefinition _definition = new();
            public Lifetime Lifetime => _definition.Lifetime;

            private void OnDestroy() {
                _definition.Terminate();
            }
        }
    }
}