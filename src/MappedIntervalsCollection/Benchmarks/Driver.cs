using System;
using System.Collections.Generic;
using BenchmarkDotNet.Running;
using Contract;

namespace Console.Benchmarks
{
    internal sealed class Driver
    {
        private readonly ILogger _logger;
        private readonly IReadOnlyCollection<SandboxPlugin> _plugins;

        public Driver(ILogger logger, IReadOnlyCollection<SandboxPlugin> plugins)
        {
            _logger = logger;
            _plugins = plugins;
        }

        public void Run()
        {
            if (_plugins.Count == 0)
            {
                return;
            }

            try
            {
                foreach (var p in _plugins)
                {
                    _logger.Info(FormattableString.Invariant($"* Registered {p.Name} factory."));
                    CollectionFactories.RegisterFactory(new CollectionFactory(p));
                }
                _logger.Info("Running benchmarks...");
                BenchmarkRunner.Run<SinglePutScenarios<ValueCrate<int>>>();
            }
            finally
            {
                CollectionFactories.Cleanup();
            }
        }
    }
}