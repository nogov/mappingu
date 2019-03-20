using System.Collections.Generic;
using BenchmarkDotNet.Running;
using Contract;

namespace Console
{
    internal sealed class Benchmarks
    {
        private readonly IEnumerable<SandboxPlugin> _plugins;

        public Benchmarks(IEnumerable<SandboxPlugin> plugins)
        {
            _plugins = plugins;
        }

        public void Run()
        {
            BenchmarkRunner.Run<Benchmarks>();
        }
    }
}