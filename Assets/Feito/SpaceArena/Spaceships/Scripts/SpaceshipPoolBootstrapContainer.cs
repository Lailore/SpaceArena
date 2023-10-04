using Collections.Pooled;

namespace Feito.SpaceArena {
    public class SpaceshipPoolBootstrapContainer : BootstrapContainer {
        public override PooledList<IBootstrap> Get() {
            return new PooledList<IBootstrap> {
                Instantiator.Instantiate<SpaceshipFactory.Bootstrap>()
            };
        }
    }
}