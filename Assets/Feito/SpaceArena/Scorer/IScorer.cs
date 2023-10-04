using System;
using System.Threading.Tasks;
using JetBrains.Collections.Viewable;

namespace Feito.SpaceArena {
    public interface IScorer {
        IReadonlyProperty<int> Maximum { get; }
        IReadonlyProperty<int> Current { get; }
        public Task Increment();
        void Reset();
    }
}