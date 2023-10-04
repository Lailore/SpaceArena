using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using JetBrains.Lifetimes;

namespace Feito {
    public class Pool<TItem> : IPool<TItem> where TItem : IPoolable {
        private readonly Func<Task<TItem>> _factory;
        private readonly ConcurrentQueue<TItem> _items = new();

        protected Pool(Func<Task<TItem>> factory) {
            _factory = factory;
        }

        public async Task<TItem> Rent(Lifetime lifetime) {
            if (!_items.TryDequeue(out var item)) {
                item = await _factory.Invoke();
            }

            await item.OnRent(lifetime);
            lifetime.OnTermination(() => {
                _items.Enqueue(item);
            });
            
            return item;
        }
    }
}