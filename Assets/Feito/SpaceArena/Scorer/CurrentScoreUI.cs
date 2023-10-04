using TMPro;
using UnityEngine;
using Zenject;

namespace Feito.SpaceArena {
    public class CurrentScoreUI : MonoBehaviour {
        [SerializeField] private TMP_Text label;

        [Inject]
        private void Inject(IScorer scorer) {
            _scorer = scorer;
        }

        private IScorer _scorer;

        private void OnEnable() {
            label.Bind(gameObject.GetActiveLifetime().Lifetime, _scorer.Current);
        }
    }
}