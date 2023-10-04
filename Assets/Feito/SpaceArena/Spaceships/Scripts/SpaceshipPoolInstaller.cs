using System;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Feito.SpaceArena {
    [RequireComponent(typeof(SpaceshipPoolBootstrapContainer))]
    public class SpaceshipPoolInstaller : MonoInstaller {
        [SerializeField] private SpaceshipFactory.Settings settings;

        public override void InstallBindings() {
            Container.BindInstance(settings);
            Container.BindInterfacesAndSelfTo<SpaceshipFactory>().AsSingle();
            Container.Bind<Func<Task<IBridge>>>().FromMethod(() => Container.Resolve<SpaceshipFactory>().Create);
            Container.Bind<IPool<IBridge>>().To<Pool<IBridge>>().AsSingle();
        }
    }
}