namespace Defra.Imports.UnitTests.Infrastructure
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Defra.Imports.Infrastructure.DependencyInjection;
    using Xunit;

    public sealed class PluginContainerTests
    {
        [Fact]
        public void Resolve_RegisteredGraph_InjectsConstructorsAndCreatesTransients()
        {
            using (var root = new PluginContainer())
            {
                root.Register<IDependency, Dependency>();
                root.Register<Consumer>();
                var first = root.Resolve<Consumer>();
                var second = root.Resolve<Consumer>();
                Assert.IsType<Dependency>(first.Dependency);
                Assert.NotSame(first, second);
                Assert.NotSame(first.Dependency, second.Dependency);
            }
        }

        [Fact]
        public void CreateExecutionScope_ConcurrentExecutions_IsolatesBorrowedServices()
        {
            using (var root = new PluginContainer())
            {
                root.Register<IDependency, Dependency>();
                root.Register<Consumer>();
                Parallel.For(0, 30, index =>
                {
                    using (var scope = root.CreateExecutionScope())
                    {
                        var dependency = new Dependency();
                        scope.Register<IDependency>(dependency);
                        Assert.Same(dependency, scope.Resolve<Consumer>().Dependency);
                    }
                });
                Assert.IsType<Dependency>(root.Resolve<Consumer>().Dependency);
                Assert.Throws<InvalidOperationException>(() => root.Register<Consumer>());
            }
        }

        [Fact]
        public void Register_InvalidOrDuplicateRegistration_FailsExplicitly()
        {
            using (var root = new PluginContainer())
            {
                Assert.Throws<ArgumentException>(() => root.Register<IDependency>());
                Assert.Throws<InvalidOperationException>(() => root.Register<IDependency>(new Dependency()));
                root.Register<Consumer>();
                Assert.Throws<ArgumentException>(() => root.Register<Consumer>());
                Assert.Throws<InvalidOperationException>(() => root.Resolve<Consumer>());
                using (var scope = root.CreateExecutionScope())
                {
                    Assert.Throws<ArgumentNullException>(() => scope.Register<IDependency>(null));
                    scope.Register<IDependency>(new Dependency());
                    Assert.Throws<ArgumentException>(() => scope.Register<IDependency>(new Dependency()));
                    Assert.Throws<InvalidOperationException>(() => scope.CreateExecutionScope());
                }
            }
        }

        [Fact]
        public void Resolve_InvalidConstructorGraphs_ReportCause()
        {
            using (var root = new PluginContainer())
            {
                root.Register<Cycle>();
                root.Register<Ambiguous>();
                root.Register<NoPublicConstructor>();
                root.Register<Throwing>();
                Assert.Contains("circular", Assert.Throws<InvalidOperationException>(() => root.Resolve<Cycle>()).Message);
                Assert.Contains("exactly one", Assert.Throws<InvalidOperationException>(() => root.Resolve<Ambiguous>()).Message);
                Assert.Contains("exactly one", Assert.Throws<InvalidOperationException>(() => root.Resolve<NoPublicConstructor>()).Message);
                Assert.Equal("constructor failed", Assert.Throws<ApplicationException>(() => root.Resolve<Throwing>()).Message);
            }
        }

        [Fact]
        public void Dispose_OwnedAndBorrowedServices_DisposesOnlyOwnedInReverseOrder()
        {
            var order = new List<string>();
            var borrowed = new Borrowed();
            var root = new PluginContainer();
            root.Register<OwnedChild>();
            root.Register<OwnedParent>();
            var scope = root.CreateExecutionScope();
            scope.Register(order);
            scope.Register(borrowed);
            scope.Resolve<OwnedParent>();
            scope.Dispose();
            scope.Dispose();
            Assert.Equal(new[] { "parent", "child" }, order);
            Assert.False(borrowed.Disposed);
            Assert.Throws<ObjectDisposedException>(() => scope.Resolve<OwnedParent>());
            Assert.Throws<ObjectDisposedException>(() => scope.Register<Dependency>());
            Assert.Throws<ObjectDisposedException>(() => scope.Register(borrowed));
            root.Dispose();
            Assert.Throws<ObjectDisposedException>(() => root.CreateExecutionScope());
        }

        [Fact]
        public void Dispose_FailingDisposables_ContinuesCleanupAndReportsFailures()
        {
            using (var root = new PluginContainer())
            {
                root.Register<FailingDisposable>();
                var scope = root.CreateExecutionScope();
                var first = scope.Resolve<FailingDisposable>();
                var second = scope.Resolve<FailingDisposable>();
                var exception = Assert.Throws<AggregateException>(() => scope.Dispose());
                Assert.Equal(2, exception.InnerExceptions.Count);
                Assert.True(first.Disposed);
                Assert.True(second.Disposed);
            }
        }

        [Fact]
        public void Dispose_RootAndSiblingScope_DoNotInvalidateOtherScope()
        {
            var root = new PluginContainer();
            root.Register<Dependency>();
            var first = root.CreateExecutionScope();
            var second = root.CreateExecutionScope();
            root.Dispose();
            first.Dispose();
            using (second)
            {
                Assert.IsType<Dependency>(second.Resolve<Dependency>());
                second.Register<Consumer>();
                var dependency = new Dependency();
                second.Register<IDependency>(dependency);
                Assert.Same(dependency, second.Resolve<Consumer>().Dependency);
            }
        }

        [Fact]
        public void Resolve_ConstructorFails_ScopeStillDisposesConstructedDependencies()
        {
            var order = new List<string>();
            using (var root = new PluginContainer())
            {
                root.Register<OwnedChild>();
                root.Register<FailingParent>();
                using (var scope = root.CreateExecutionScope())
                {
                    scope.Register(order);
                    Assert.Throws<ApplicationException>(() => scope.Resolve<FailingParent>());
                    Assert.Empty(order);
                }

                Assert.Equal(new[] { "child" }, order);
            }
        }

        public interface IDependency
        {
        }

        public sealed class Dependency : IDependency
        {
        }

        public sealed class Consumer
        {
            public Consumer(IDependency dependency)
            {
                this.Dependency = dependency;
            }

            public IDependency Dependency { get; }
        }

        public sealed class Cycle
        {
            public Cycle(Cycle cycle)
            {
            }
        }

        public sealed class Ambiguous
        {
            public Ambiguous()
            {
            }

            public Ambiguous(IDependency dependency)
            {
            }
        }

        public sealed class NoPublicConstructor
        {
            private NoPublicConstructor()
            {
            }
        }

        public sealed class Throwing
        {
            public Throwing()
            {
                throw new ApplicationException("constructor failed");
            }
        }

        public sealed class Borrowed : IDisposable
        {
            public bool Disposed { get; private set; }

            public void Dispose()
            {
                this.Disposed = true;
            }
        }

        public sealed class OwnedChild : IDisposable
        {
            private readonly List<string> order;

            public OwnedChild(List<string> order)
            {
                this.order = order;
            }

            public void Dispose()
            {
                this.order.Add("child");
            }
        }

        public sealed class OwnedParent : IDisposable
        {
            private readonly List<string> order;

            public OwnedParent(OwnedChild child, List<string> order, Borrowed borrowed)
            {
                this.order = order;
            }

            public void Dispose()
            {
                this.order.Add("parent");
            }
        }

        public sealed class FailingDisposable : IDisposable
        {
            public bool Disposed { get; private set; }

            public void Dispose()
            {
                this.Disposed = true;
                throw new InvalidOperationException("cleanup failed");
            }
        }

        public sealed class FailingParent
        {
            public FailingParent(OwnedChild child)
            {
                throw new ApplicationException("parent constructor failed");
            }
        }
    }
}
