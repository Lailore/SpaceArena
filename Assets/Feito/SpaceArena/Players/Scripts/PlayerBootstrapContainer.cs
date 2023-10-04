using Collections.Pooled;

namespace Feito.SpaceArena {
    public class PlayerBootstrapContainer : BootstrapContainer {
        public override PooledList<IBootstrap> Get() {
            return new PooledList<IBootstrap> {
                Instantiator.Instantiate<PlayerController.Bootstrap>()
            };
        }
    }
}