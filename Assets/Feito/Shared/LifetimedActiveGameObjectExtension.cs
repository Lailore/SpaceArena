using JetBrains.Collections.Viewable;
using JetBrains.Lifetimes;
using UnityEngine;

namespace Feito {
    public static class LifetimedActiveGameObjectExtension {
        public static Component GetActiveLifetime(this GameObject gameObject) {
            return gameObject.GetComponent<Component>() ?? gameObject.AddComponent<Component>();
        }

        public class Component : MonoBehaviour {
            private readonly ViewableProperty<bool> _isActive = new(false);
            private LifetimeDefinition _definition;

            public IReadonlyProperty<bool> IsActive => _isActive;
            public Lifetime Lifetime => _definition.Lifetime;

            private void Awake() {
                _definition = new LifetimeDefinition();
                _definition.Terminate();
            }

            private void OnEnable() {
                _definition = gameObject.GetLifetime().CreateNested();
                _isActive.Value = true;
            }

            private void OnDisable() {
                _definition.Terminate();
                _isActive.Value = false;
            }
        }
    }
}