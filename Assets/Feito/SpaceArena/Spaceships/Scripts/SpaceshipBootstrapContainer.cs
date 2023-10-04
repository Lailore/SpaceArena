using Collections.Pooled;

namespace Feito.SpaceArena {
    public class SpaceshipBootstrapContainer : BootstrapContainer {
        public override PooledList<IBootstrap> Get() {
            return new PooledList<IBootstrap> {
                Instantiator.Instantiate<Engine.Bootstrap>(),
                Instantiator.Instantiate<Wheel.Bootstrap>()
            };
        }
    }
}