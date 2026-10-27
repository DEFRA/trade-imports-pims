namespace Defra.Imports.UnitTests.Infrastructure.DependencyInjection
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Defra.Imports.Infrastructure.DependencyInjection;
    using Defra.Imports.UnitTests.Support;
    using Xunit;

    public sealed class PluginContainerTests : IDisposable
    {
        private readonly PluginContainer root = new PluginContainer();

        private interface IDependency
        {
        }

        public void Dispose()
        {
            this.root.Dispose();
        }

        [Fact]
        public void Resolve_RegisteredInterface_ReturnsImplementationWithInjectedDependencies()
        {
            this.root.Register<IDependency, Dependency>();
            this.root.Register<Consumer>();

            var consumer = this.root.Resolve<Consumer>();

            Assert.IsType<Dependency>(consumer.Dependency);
        }

        [Fact]
        public void Resolve_CalledTwice_ReturnsNewInstanceEachTime()
        {
            this.root.Register<IDependency, Dependency>();
            this.root.Register<Consumer>();

            var first = this.root.Resolve<Consumer>();
            var second = this.root.Resolve<Consumer>();

            Assert.NotSame(first, second);
            Assert.NotSame(first.Dependency, second.Dependency);
        }

        [Fact]
        public void Resolve_UnregisteredService_ThrowsInvalidOperationException()
        {
            var exception = Assert.Throws<InvalidOperationException>(() => this.root.Resolve<Consumer>());

            Assert.Contains(typeof(Consumer).FullName, exception.Message);
        }

        [Fact]
        public void Resolve_CircularDependency_ThrowsInvalidOperationException()
        {
            this.root.Register<Cycle>();

            var exception = Assert.Throws<InvalidOperationException>(() => this.root.Resolve<Cycle>());

            Assert.Contains("circular", exception.Message);
        }

        [Fact]
        public void Resolve_MultiplePublicConstructors_ThrowsInvalidOperationException()
        {
            this.root.Register<Ambiguous>();

            var exception = Assert.Throws<InvalidOperationException>(() => this.root.Resolve<Ambiguous>());

            Assert.Contains("exactly one", exception.Message);
        }

        [Fact]
        public void Resolve_NoPublicConstructor_ThrowsInvalidOperationException()
        {
            this.root.Register<NoPublicConstructor>();

            var exception = Assert.Throws<InvalidOperationException>(() => this.root.Resolve<NoPublicConstructor>());

            Assert.Contains("exactly one", exception.Message);
        }

        [Fact]
        public void Resolve_ConstructorThrows_PropagatesOriginalException()
        {
            this.root.Register<Throwing>();

            var exception = Assert.Throws<SimulatedFailureException>(() => this.root.Resolve<Throwing>());

            Assert.Equal("constructor failed", exception.Message);
        }

        [Fact]
        public void Resolve_BorrowedInstanceRegisteredInScope_ReturnsThatInstance()
        {
            var borrowed = new Dependency();
            using (var scope = this.root.CreateExecutionScope())
            {
                scope.Register<IDependency>(borrowed);

                var resolved = scope.Resolve<IDependency>();

                Assert.Same(borrowed, resolved);
            }
        }

        [Fact]
        public void Resolve_DisposedScope_ThrowsObjectDisposedException()
        {
            var scope = this.root.CreateExecutionScope();
            scope.Dispose();

            Assert.Throws<ObjectDisposedException>(() => scope.Resolve<Dependency>());
        }

        [Fact]
        public void Register_ImplementationIsAnInterface_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() => this.root.Register<IDependency>());
        }

        [Fact]
        public void Register_ServiceAlreadyRegistered_ThrowsArgumentException()
        {
            this.root.Register<Consumer>();

            Assert.Throws<ArgumentException>(() => this.root.Register<Consumer>());
        }

        [Fact]
        public void Register_AfterExecutionScopeCreated_ThrowsInvalidOperationException()
        {
            using (this.root.CreateExecutionScope())
            {
                Assert.Throws<InvalidOperationException>(() => this.root.Register<Consumer>());
            }
        }

        [Fact]
        public void Register_InstanceOnRoot_ThrowsInvalidOperationException()
        {
            Assert.Throws<InvalidOperationException>(() => this.root.Register<IDependency>(new Dependency()));
        }

        [Fact]
        public void Register_NullInstanceInScope_ThrowsArgumentNullException()
        {
            using (var scope = this.root.CreateExecutionScope())
            {
                Assert.Throws<ArgumentNullException>(() => scope.Register<IDependency>(null));
            }
        }

        [Fact]
        public void Register_InstanceAlreadyRegisteredInScope_ThrowsArgumentException()
        {
            using (var scope = this.root.CreateExecutionScope())
            {
                scope.Register<IDependency>(new Dependency());

                Assert.Throws<ArgumentException>(() => scope.Register<IDependency>(new Dependency()));
            }
        }

        [Fact]
        public void Register_DisposedScope_ThrowsObjectDisposedException()
        {
            var scope = this.root.CreateExecutionScope();
            scope.Dispose();

            Assert.Throws<ObjectDisposedException>(() => scope.Register<Dependency>());
        }

        [Fact]
        public void CreateExecutionScope_FromScope_ThrowsInvalidOperationException()
        {
            using (var scope = this.root.CreateExecutionScope())
            {
                Assert.Throws<InvalidOperationException>(() => scope.CreateExecutionScope());
            }
        }

        [Fact]
        public void CreateExecutionScope_DisposedRoot_ThrowsObjectDisposedException()
        {
            this.root.Dispose();

            Assert.Throws<ObjectDisposedException>(() => this.root.CreateExecutionScope());
        }

        [Fact]
        public void CreateExecutionScope_ConcurrentScopes_IsolateBorrowedInstances()
        {
            const int scopeCount = 30;
            this.root.Register<Consumer>();
            var isolated = new ConcurrentBag<bool>();

            Parallel.For(0, scopeCount, index =>
            {
                using (var scope = this.root.CreateExecutionScope())
                {
                    var borrowed = new Dependency();
                    scope.Register<IDependency>(borrowed);

                    isolated.Add(ReferenceEquals(borrowed, scope.Resolve<Consumer>().Dependency));
                }
            });

            Assert.Equal(scopeCount, isolated.Count);
            Assert.All(isolated, value => Assert.True(value));
        }

        [Fact]
        public void CreateExecutionScope_BorrowedInstanceInOneScope_IsNotVisibleToRoot()
        {
            this.root.Register<Consumer>();
            using (var scope = this.root.CreateExecutionScope())
            {
                scope.Register<IDependency>(new Dependency());
            }

            Assert.Throws<InvalidOperationException>(() => this.root.Resolve<Consumer>());
        }

        [Fact]
        public void Dispose_OwnedServices_DisposesInReverseConstructionOrder()
        {
            var log = new DisposalLog();
            this.root.Register<OwnedChild>();
            this.root.Register<OwnedParent>();
            var scope = this.root.CreateExecutionScope();
            scope.Register(log);
            scope.Resolve<OwnedParent>();

            scope.Dispose();

            Assert.Equal(new[] { "parent", "child" }, log.Entries);
        }

        [Fact]
        public void Dispose_CalledTwice_DisposesServicesOnce()
        {
            var log = new DisposalLog();
            this.root.Register<OwnedChild>();
            var scope = this.root.CreateExecutionScope();
            scope.Register(log);
            scope.Resolve<OwnedChild>();

            scope.Dispose();
            scope.Dispose();

            Assert.Equal(new[] { "child" }, log.Entries);
        }

        [Fact]
        public void Dispose_BorrowedInstance_DoesNotDisposeIt()
        {
            var borrowed = new BorrowedDisposable();
            var scope = this.root.CreateExecutionScope();
            scope.Register(borrowed);

            scope.Dispose();

            Assert.False(borrowed.Disposed);
        }

        [Fact]
        public void Dispose_OwnedServiceFails_DisposesRemainingServicesAndThrowsAggregateException()
        {
            this.root.Register<FailingDisposable>();
            var scope = this.root.CreateExecutionScope();
            var first = scope.Resolve<FailingDisposable>();
            var second = scope.Resolve<FailingDisposable>();

            var exception = Assert.Throws<AggregateException>(() => scope.Dispose());

            Assert.Equal(2, exception.InnerExceptions.Count);
            Assert.True(first.Disposed);
            Assert.True(second.Disposed);
        }

        [Fact]
        public void Dispose_ConstructorFailsAfterDependencyCreated_StillDisposesTheDependency()
        {
            var log = new DisposalLog();
            this.root.Register<OwnedChild>();
            this.root.Register<FailingParent>();
            var scope = this.root.CreateExecutionScope();
            scope.Register(log);
            Assert.Throws<SimulatedFailureException>(() => scope.Resolve<FailingParent>());

            scope.Dispose();

            Assert.Equal(new[] { "child" }, log.Entries);
        }

        [Fact]
        public void Dispose_Root_DoesNotInvalidateExistingScope()
        {
            this.root.Register<Consumer>();
            using (var scope = this.root.CreateExecutionScope())
            {
                var borrowed = new Dependency();
                scope.Register<IDependency>(borrowed);

                this.root.Dispose();

                Assert.Same(borrowed, scope.Resolve<Consumer>().Dependency);
            }
        }

        [Fact]
        public void Dispose_SiblingScope_DoesNotInvalidateOtherScope()
        {
            this.root.Register<Dependency>();
            var first = this.root.CreateExecutionScope();
            using (var second = this.root.CreateExecutionScope())
            {
                first.Dispose();

                Assert.IsType<Dependency>(second.Resolve<Dependency>());
            }
        }

        private sealed class Dependency : IDependency
        {
        }

        private sealed class Consumer
        {
            public Consumer(IDependency dependency)
            {
                this.Dependency = dependency;
            }

            public IDependency Dependency { get; }
        }

        private sealed class Cycle
        {
            public Cycle(Cycle cycle)
            {
                this.Inner = cycle;
            }

            public Cycle Inner { get; }
        }

        private sealed class Ambiguous
        {
            public Ambiguous()
            {
            }

            public Ambiguous(IDependency dependency)
            {
                this.Dependency = dependency;
            }

            public IDependency Dependency { get; }
        }

        private sealed class NoPublicConstructor
        {
            private NoPublicConstructor()
            {
            }
        }

        private sealed class Throwing
        {
            public Throwing()
            {
                throw new SimulatedFailureException("constructor failed");
            }
        }

        private sealed class DisposalLog
        {
            private readonly List<string> entries = new List<string>();

            public IReadOnlyList<string> Entries => this.entries.ToList();

            public void Add(string entry)
            {
                this.entries.Add(entry);
            }
        }

        private sealed class OwnedChild : IDisposable
        {
            private readonly DisposalLog log;

            public OwnedChild(DisposalLog log)
            {
                this.log = log;
            }

            public void Dispose()
            {
                this.log.Add("child");
            }
        }

        private sealed class OwnedParent : IDisposable
        {
            private readonly DisposalLog log;

            public OwnedParent(OwnedChild child, DisposalLog log)
            {
                this.Child = child;
                this.log = log;
            }

            public OwnedChild Child { get; }

            public void Dispose()
            {
                this.log.Add("parent");
            }
        }

        private sealed class FailingParent
        {
            public FailingParent(OwnedChild child)
            {
                this.Child = child;
                throw new SimulatedFailureException("parent constructor failed");
            }

            public OwnedChild Child { get; }
        }

        private sealed class BorrowedDisposable : IDisposable
        {
            public bool Disposed { get; private set; }

            public void Dispose()
            {
                this.Disposed = true;
            }
        }

        private sealed class FailingDisposable : IDisposable
        {
            public bool Disposed { get; private set; }

            public void Dispose()
            {
                this.Disposed = true;
                throw new SimulatedFailureException("cleanup failed");
            }
        }
    }
}
