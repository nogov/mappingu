using System;
using System.Collections.Generic;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;
using BenchmarkDotNet.Validators;
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
                    CollectionBag.Register(new CollectionDescription(p));
                }
                _logger.Info("Running benchmarks...");
                BenchmarkRunner.Run<SinglePutScenarios<ValueCrate<int>>>();
            }
            finally
            {
                CollectionBag.Cleanup();
            }
        }
    }
}