using System;
using System.Threading.Tasks;
using JetBrains.Collections.Viewable;
using JetBrains.Lifetimes;
using UnityEngine;

namespace Feito.SpaceArena {
    public class Scorer : IScorer {
        private const string RecordPlayPrefsKey = "Record";
        private readonly Lifetime _appLifetime;
        private readonly ViewableProperty<int> _current = new();
        private readonly ViewableProperty<int> _maximum = new();

        public Scorer(Lifetime appLifetime) {
            _appLifetime = appLifetime;
        }

        public IReadonlyProperty<int> Maximum => _maximum;
        public IReadonlyProperty<int> Current => _current;

        public Task Increment() {
            _current.Value += 1;
            return Task.CompletedTask;
        }

        public void Reset() {
            _current.Value = 0;
        }

        private Task Initialize() {
            _maximum.Value = PlayerPrefs.GetInt(RecordPlayPrefsKey);
            _current.Advise(_appLifetime, OnCurrentScoreChanged);
            return Task.CompletedTask;
        }

        private void OnCurrentScoreChanged(int value) {
            if (_maximum.Value < value) {
                _maximum.Value = value;
                PlayerPrefs.SetInt(RecordPlayPrefsKey, value);
            }
        }

        public class Bootstrap : IBootstrap {
            private readonly Scorer _scorer;

            public Bootstrap(Scorer scorer) {
                _scorer = scorer;
            }

            public Task Run() {
                return _scorer.Initialize();
            }
        }
    }
}