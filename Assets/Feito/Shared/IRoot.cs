using System.Threading.Tasks;
using JetBrains.Lifetimes;
using UnityEngine;

namespace Feito {
    public interface IRoot {
        Task Enable(Lifetime lifetime);
        void Teleport(Vector2 position);
    }
}