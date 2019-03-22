using System.Collections.Generic;
using System.Linq;
using Contract;
using MappedIntervalsCollection;

namespace Console
{
    internal sealed class Program
    {
        public static void Main(string[] args)
        {
            var logger = new ConsoleLogger();
            var plugins = FilterIfNeeded(MefDlyaBednyx.GetPlugins(logger), args).ToArray();

            var tests = new Tests.Suite(logger, plugins);
            tests.Run();

            var benchmarks = new Benchmarks.Driver(logger, plugins);
            benchmarks.Run();

            System.Console.ReadKey();
        }

        private static IEnumerable<SandboxPlugin> FilterIfNeeded(IEnumerable<SandboxPlugin> plugins, string[] names)
        {
            if (names.Length == 0)
            {
                return plugins;
            }

            var needles = new HashSet<string>(names);
            return plugins.Where(p => needles.Contains(p.Name));
        }
    }
}
