using System.Threading.Tasks;
using JetBrains.Lifetimes;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Feito {
    public static class AddressablesLifeTimedLoadAssetAsyncExtension {
        public static Task<T> LoadAssetAsync<T>(this AssetReferenceT<T> reference, Lifetime lifetime) where T : Object {
            var handle = Addressables.LoadAssetAsync<T>(reference);
            lifetime.OnTermination(() => Addressables.Release(handle));
            return handle.Task;
        }
    }
}