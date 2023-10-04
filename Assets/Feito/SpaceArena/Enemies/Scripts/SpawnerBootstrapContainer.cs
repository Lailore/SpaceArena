using Collections.Pooled;

namespace Feito.SpaceArena {
    public class SpawnerBootstrapContainer : BootstrapContainer {
        public override PooledList<IBootstrap> Get() {
            return new PooledList<IBootstrap> {
                Instantiator.Instantiate<Spawner.Bootstrap>()
            };
        }
    }
}