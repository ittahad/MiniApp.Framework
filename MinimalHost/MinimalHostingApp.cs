
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using MiniApp.Core;
using System.Reflection;

namespace MinimalHost
{
    public class MinimalHostingApp
    {
        public IHost Host { get; set; }
        private readonly MinimalHostOptions _options;

        public MinimalHostingApp(MinimalHostOptions options = null)
        {
            _options = options == null ? new MinimalWebAppOptions()
            {
                CommandLineArgs = new string[] { },
            } : options;
        }

        public void Run()
        {
            Host.Run();
        }

        public static IEnumerable<Type> GetAllDescendantsOf(
            Assembly assembly,
            Type genericTypeDefinition)
        {
            IEnumerable<Type> GetAllAscendants(Type t)
            {
                var current = t;

                while (current.BaseType != typeof(object))
                {
                    yield return current.BaseType;
                    current = current.BaseType;
                }
            }

            if (assembly == null)
                throw new ArgumentNullException(nameof(assembly));

            if (genericTypeDefinition == null)
                throw new ArgumentNullException(nameof(genericTypeDefinition));

            if (!genericTypeDefinition.IsGenericTypeDefinition)
                throw new ArgumentException(
                    "Specified type is not a valid generic type definition.",
                    nameof(genericTypeDefinition));

            return assembly.GetTypes()
                           .Where(t => GetAllAscendants(t).Any(d =>
                               d.IsGenericType &&
                               d.GetGenericTypeDefinition()
                                .Equals(genericTypeDefinition)));
        }
    }
}