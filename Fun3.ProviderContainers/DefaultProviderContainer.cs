using Fun3.Lexers;
using Fun3.TokenServices;
using Microsoft.Extensions.DependencyInjection;

namespace Fun3.ProviderContainers;

public class DefaultProviderContainer : IProviderContainer
{
    public static void ConfigureProvider(string[] lines)
    {
        var services = new ServiceCollection();

        services.AddSingleton<ILexer>(_ => new DefaultLexer(lines, new DefaultTokenService()));

        Provider = services.BuildServiceProvider();
    }

    public static IServiceProvider Provider { get; private set; }
}
