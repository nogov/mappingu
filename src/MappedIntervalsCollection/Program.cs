using System.Linq;
using MappedIntervalsCollection;

namespace Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var plugins = MefDlyaBednyx.GetPlugins().ToArray();

            var tests = new Tests.Suite(plugins);
            tests.Run();

            var benchmarks = new Benchmarks(plugins);
            benchmarks.Run();

            System.Console.ReadKey();
        }
    }
}
