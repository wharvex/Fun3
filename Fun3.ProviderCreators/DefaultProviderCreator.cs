using Fun3.Lexers;
using Fun3.TokenServices;
using Microsoft.Extensions.DependencyInjection;

namespace Fun3.ProviderCreators;

public class DefaultProviderCreator
{
    public void ConfigureProvider(string[] lines)
    {
        var services = new ServiceCollection();

        services.AddSingleton<ILexer>(_ => new DefaultLexer(lines));
        services.AddSingleton<ITokenService, DefaultTokenService>();

        ProviderContainer.Provider = services.BuildServiceProvider();
    }
}
