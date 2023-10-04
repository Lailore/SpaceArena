using System;
using JetBrains.Lifetimes;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Feito {
    public static class ButtonBindExtension {
        public static void Bind(this Button button, Lifetime lifetime, Action handler) {
            var unityAction = new UnityAction(handler);
            button.onClick.AddListener(unityAction);
            lifetime.OnTermination(() => button.onClick.RemoveListener(unityAction));
        }
    }
}