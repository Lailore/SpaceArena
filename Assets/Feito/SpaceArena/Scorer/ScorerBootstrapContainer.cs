using Collections.Pooled;

namespace Feito.SpaceArena {
    public class ScorerBootstrapContainer : BootstrapContainer {
        public override PooledList<IBootstrap> Get() {
            return new PooledList<IBootstrap> {
                Instantiator.Instantiate<Scorer.Bootstrap>()
            };
        }
    }
}