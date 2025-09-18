using System.Reflection;
using Mapster;

namespace SpendTrackApi.Mapping.Extensions
{
    public static class TypeAdapterConfigExtensions
    {
        public static void RegisterMappings(this TypeAdapterConfig config)
        {
            IEnumerable<IRegister> registers = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(t => typeof(IRegister).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract)
                .Select(Activator.CreateInstance)
                .Cast<IRegister>();

            foreach (var register in registers)
            {
                register.Register(config);
            }
        }
    }
}
