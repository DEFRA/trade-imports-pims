namespace Defra.Imports.Infrastructure.DependencyInjection
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.ExceptionServices;

    /// <summary>Stores application registrations and creates isolated execution scopes.</summary>
    public sealed class PluginContainer : IDisposable
    {
        private readonly object gate = new object();
        private readonly Dictionary<Type, Type> registrations;
        private readonly Dictionary<Type, object> instances = new Dictionary<Type, object>();
        private readonly List<IDisposable> ownedInstances = new List<IDisposable>();
        private readonly bool executionScope;
        private bool frozen;
        private bool disposed;

        /// <summary>Initializes a new instance of the <see cref="PluginContainer"/> class as a root.</summary>
        public PluginContainer()
            : this(new Dictionary<Type, Type>(), false)
        {
        }

        private PluginContainer(Dictionary<Type, Type> registrations, bool executionScope)
        {
            this.registrations = registrations;
            this.executionScope = executionScope;
        }

        /// <summary>Registers a transient implementation for an application contract.</summary>
        /// <typeparam name="TInterface">The service contract.</typeparam>
        /// <typeparam name="TImplementation">The concrete implementation.</typeparam>
        public void Register<TInterface, TImplementation>()
            where TImplementation : TInterface
        {
            this.RegisterType(typeof(TInterface), typeof(TImplementation));
        }

        /// <summary>Registers a concrete transient service.</summary>
        /// <typeparam name="TImplementation">The concrete service type.</typeparam>
        public void Register<TImplementation>()
        {
            this.RegisterType(typeof(TImplementation), typeof(TImplementation));
        }

        /// <summary>Registers a borrowed runtime service, which the scope will not dispose.</summary>
        /// <typeparam name="TService">The runtime contract to expose.</typeparam>
        /// <param name="instance">The current execution's service instance.</param>
        public void Register<TService>(TService instance)
            where TService : class
        {
            if (instance == null)
            {
                throw new ArgumentNullException(nameof(instance));
            }

            lock (this.gate)
            {
                this.ThrowIfDisposed();
                if (!this.executionScope)
                {
                    throw new InvalidOperationException("Runtime instances must be registered in an execution scope.");
                }

                this.instances.Add(typeof(TService), instance);
            }
        }

        /// <summary>Freezes root registrations and copies their metadata, never runtime instances.</summary>
        /// <returns>An independent scope owned by the caller.</returns>
        public PluginContainer CreateExecutionScope()
        {
            lock (this.gate)
            {
                this.ThrowIfDisposed();
                if (this.executionScope)
                {
                    throw new InvalidOperationException("Create execution scopes from the root container only.");
                }

                this.frozen = true;
                return new PluginContainer(new Dictionary<Type, Type>(this.registrations), true);
            }
        }

        /// <summary>Constructs a transient object graph using exactly one public constructor per type.</summary>
        /// <typeparam name="T">The registered service contract.</typeparam>
        /// <returns>A resolved service; created disposables belong to this container or scope.</returns>
        public T Resolve<T>()
        {
            lock (this.gate)
            {
                this.ThrowIfDisposed();
                return (T)this.Resolve(typeof(T), new HashSet<Type>());
            }
        }

        /// <summary>Disposes created services in reverse construction order, but never borrowed services.</summary>
        public void Dispose()
        {
            lock (this.gate)
            {
                if (this.disposed)
                {
                    return;
                }

                this.disposed = true;
                var failures = new List<Exception>();
                for (int index = this.ownedInstances.Count - 1; index >= 0; index--)
                {
                    try
                    {
                        this.ownedInstances[index].Dispose();
                    }
                    catch (Exception exception)
                    {
                        failures.Add(exception);
                    }
                }

                this.ownedInstances.Clear();
                this.instances.Clear();
                this.registrations.Clear();
                if (failures.Count != 0)
                {
                    throw new AggregateException("Disposing container-owned services failed.", failures);
                }
            }
        }

        private void RegisterType(Type serviceType, Type implementationType)
        {
            lock (this.gate)
            {
                this.ThrowIfDisposed();
                if (this.frozen)
                {
                    throw new InvalidOperationException("Root registrations cannot change after an execution scope has been created.");
                }

                if (!implementationType.IsClass || implementationType.IsAbstract || implementationType.ContainsGenericParameters)
                {
                    throw new ArgumentException("An implementation must be a closed, concrete class.", nameof(implementationType));
                }

                this.registrations.Add(serviceType, implementationType);
            }
        }

        private object Resolve(Type serviceType, HashSet<Type> path)
        {
            object instance;
            if (this.instances.TryGetValue(serviceType, out instance))
            {
                return instance;
            }

            Type implementationType;
            if (!this.registrations.TryGetValue(serviceType, out implementationType))
            {
                throw new InvalidOperationException("No registration exists for " + serviceType.FullName + ".");
            }

            if (!path.Add(serviceType))
            {
                throw new InvalidOperationException("A circular dependency was detected for " + serviceType.FullName + ".");
            }

            try
            {
                var constructors = implementationType.GetConstructors();
                if (constructors.Length != 1)
                {
                    throw new InvalidOperationException(implementationType.FullName + " must have exactly one public constructor.");
                }

                var arguments = constructors[0].GetParameters()
                    .Select(parameter => this.Resolve(parameter.ParameterType, path)).ToArray();
                try
                {
                    instance = constructors[0].Invoke(arguments);
                }
                catch (TargetInvocationException exception) when (exception.InnerException != null)
                {
                    ExceptionDispatchInfo.Capture(exception.InnerException).Throw();
                    throw;
                }

                var disposable = instance as IDisposable;
                if (disposable != null)
                {
                    this.ownedInstances.Add(disposable);
                }

                return instance;
            }
            finally
            {
                path.Remove(serviceType);
            }
        }

        private void ThrowIfDisposed()
        {
            if (this.disposed)
            {
                throw new ObjectDisposedException(nameof(PluginContainer));
            }
        }
    }
}
