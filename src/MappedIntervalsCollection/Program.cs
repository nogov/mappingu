using System.Linq;
using MappedIntervalsCollection;

namespace Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var plugins = MefDlyaBednyx.GetPlugins().ToArray();
            var benchmarks = new Benchmarks(plugins);
            benchmarks.Run();

            System.Console.ReadKey();
        }
    }
}
