using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;

namespace Funky.Bootstrapper
{
    public static class Program
    {
        public static async Task Main(string[] args)
            => await CreateHostBuilder(args)
                .RunConsoleAsync()
                .ConfigureAwait(false);

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .ConfigureServices((_, __) =>
            {
                // var dir = Path.GetFullPath("../../../../Funky.Tests/bin/Debug/netcoreapp3.1");

                // foreach (var typeOption in hostContext.Configuration
                //     .GetSection("types")
                //     .Get<TypeDefinitionOptions[]>())
                // {
                //     var typeDef = new TypeDefinition(typeOption.Assembly, typeOption.TypeName);
                //     var loader = new Loader(dir);
                //     var startupType = loader.Load(new TypeDefinition(typeOption.Assembly, "Funky.Tests.Startup"));
                //     var type = loader.Load(typeDef);

                //     var startup = Activator.CreateInstance(startupType);
                //     startupType.GetMethod("ConfigureServices").Invoke(startup, new object[] { services });

                //     services.AddTransient(typeof(IFunk), type);
                // }

                // services.Configure<BootstrapperServiceOptions>(hostContext.Configuration.GetSection("svc"));
                // services.AddHostedService<BootstrapperService>();
            });
    }
}
