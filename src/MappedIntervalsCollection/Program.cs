using System.Linq;
using MappedIntervalsCollection;

namespace Console
{
    internal sealed class Program
    {
        static void Main(string[] args)
        {
            var logger = new ConsoleLogger();
            var plugins = MefDlyaBednyx.GetPlugins(logger).ToArray();

            var tests = new Tests.Suite(logger, plugins);
            tests.Run();

            var benchmarks = new Benchmarks(plugins);
            benchmarks.Run();

            System.Console.ReadKey();
        }
    }
}
