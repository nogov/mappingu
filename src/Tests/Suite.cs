using System.Collections.Generic;
using Contract;
using NUnitLite;

namespace Tests
{
    public sealed class Suite
    {
        private readonly IReadOnlyCollection<SandboxPlugin> _plugins;

        public Suite(IReadOnlyCollection<SandboxPlugin> plugins)
        {
            _plugins = plugins;
        }

        public void Run()
        {
            var failures = new AutoRun().Execute(new string[0]);
        }
    }
}
