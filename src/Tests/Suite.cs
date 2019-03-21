using System;
using System.Collections.Generic;
using Contract;
using NUnitLite;

namespace Tests
{
    public sealed class Suite
    {
        private readonly ILogger _logger;
        private readonly IReadOnlyCollection<SandboxPlugin> _plugins;

        public Suite(ILogger logger, IReadOnlyCollection<SandboxPlugin> plugins)
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
                    CollectionFactories.RegisterFactory(() => p.CreateCollection<Crate>());
                }
                _logger.Info("Running tests...");
                var failures = new AutoRun().Execute(new string[0]);
                _logger.Info(FormattableString.Invariant($"Failures: {failures}."));

                if (failures > 0)
                {
                    throw new InvalidOperationException("Please fix!");
                }
            }
            finally 
            {
                CollectionFactories.Cleanup();
            }
        }
    }
}
