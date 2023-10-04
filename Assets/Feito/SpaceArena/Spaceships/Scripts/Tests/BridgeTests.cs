using System.Collections;
using JetBrains.Lifetimes;
using NSubstitute;
using NUnit.Framework;
using Redcode.Awaiting;
using UnityEngine;
using UnityEngine.TestTools;
using Zenject;

namespace Feito.SpaceArena.Spaceships.Tests {
    [TestFixture]
    public class BridgeTests : ZenjectUnitTestFixture {
        private IEngine _engine;
        private IWheel _wheel;
        private IRoot _root;
        private ICollisionSensor2D _collisionSensor;

        [UnityTest]
        public IEnumerator Teleport() {
            var bridge = Container.Instantiate<Bridge>();
            var position = new Vector2(Random.Range(-1, 1), Random.Range(-1, 1));
            
            yield return bridge.Teleport(position).AsEnumerator();
            
            _root.Received().Teleport(position);
        }
        
        [UnityTest]
        public IEnumerator Activations() {
            var bridge = Container.Instantiate<Bridge>();
            var lifetime = Lifetime.Eternal;
            
            
            yield return bridge.OnRent(lifetime).AsEnumerator();
            
            _root.Received().Enable(lifetime);
        }

        public override void Setup() {
            base.Setup();

            _engine = Substitute.For<IEngine>();
            _wheel = Substitute.For<IWheel>();
            _root = Substitute.For<IRoot>();
            _collisionSensor = Substitute.For<ICollisionSensor2D>();

            Container.Bind<IEngine>().FromInstance(_engine);
            Container.Bind<IWheel>().FromInstance(_wheel);
            Container.Bind<IRoot>().FromInstance(_root);
            Container.Bind<ICollisionSensor2D>().FromInstance(_collisionSensor);
        }
    }
}