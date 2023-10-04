using System;
using JetBrains.Threading;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Feito {
    [RequireComponent(typeof(Button))]
    public class SimpleRouterButton : MonoBehaviour {
        [SerializeField] private Route route;
        [SerializeField] private Button button;

        [Inject]
        private void Inject(ISimpleRouter simpleRouter) {
            _simpleRouter = simpleRouter;
        }

        private ISimpleRouter _simpleRouter;

        private void OnEnable() {
            button.Bind(gameObject.GetActiveLifetime().Lifetime, Move);
        }

        private void Move() {
            switch (route) {
                case Route.MainMenu:
                    _simpleRouter.MainMenu().NoAwait();
                    break;
                case Route.Game:
                    _simpleRouter.Game().NoAwait();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private enum Route {
            MainMenu,
            Game
        }
    }
}