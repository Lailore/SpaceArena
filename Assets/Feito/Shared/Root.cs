using System.Threading.Tasks;
using JetBrains.Lifetimes;
using UnityEngine;

namespace Feito {
    public class Root : MonoBehaviour, IRoot {
        public Task Enable(Lifetime lifetime) {
            gameObject.SetActive(true);
            var gameObjectLifetime = gameObject.GetLifetime();
            lifetime.OnTermination(() => {
                if (gameObjectLifetime.IsAlive) {
                    gameObject.SetActive(false);
                }
            });
            return Task.CompletedTask;
        }

        public void Teleport(Vector2 position) {
            transform.position = position;
        }
    }
}