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

            _logger.Info(FormattableString.Invariant($"Going to run tests for {_plugins.Count} plugin (s)."));
            foreach (var p in _plugins)
            {
                _logger.Info(FormattableString.Invariant($"* Running {p.Name} tests..."));
                var failures = new AutoRun(p.GetType().Assembly).Execute(new string[0]);
                _logger.Info(FormattableString.Invariant($"Failures: {failures}."));
            }
        }
    }
}
