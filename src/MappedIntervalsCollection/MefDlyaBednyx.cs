using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Contract;

namespace MappedIntervalsCollection
{
    internal static class MefDlyaBednyx
    {
        public static IEnumerable<SandboxPlugin> GetPlugins(ILogger logger)
        {
            var executingDir = Path.GetDirectoryName(new Uri(Assembly.GetEntryAssembly().CodeBase).AbsolutePath);
            var pluginsDir = Path.Combine(executingDir, "plugins");

            foreach (var assemblyFileName in Directory.EnumerateFiles(pluginsDir, "*.dll"))
            {
                var assembly = Assembly.LoadFile(assemblyFileName);
                var types = assembly.GetTypes().Where(type => typeof(SandboxPlugin).IsAssignableFrom(type));
                foreach (var type in types)
                {
                    yield return (SandboxPlugin)Activator.CreateInstance(type, logger);
                }
            }
        }
    }
}