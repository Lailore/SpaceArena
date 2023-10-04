using JetBrains.Collections.Viewable;
using JetBrains.Lifetimes;
using TMPro;

namespace Feito {
    public static class TextMeshProBindExtension {
        public static void Bind<T>(this TMP_Text container, Lifetime lifetime, ISource<T> source) {
            source.Advise(lifetime, value => container.text = value.ToString());
        }
    }
}