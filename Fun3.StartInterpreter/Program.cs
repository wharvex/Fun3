using Fun3.ProviderCreators;
using Microsoft.Extensions.DependencyInjection;

namespace Fun3.StartInterpreter
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var lines = File.ReadAllLines(args[0]);
            var pc = new DefaultProviderCreator();
            pc.ConfigureProvider(lines);
            var lexer = ProviderContainer.Provider.GetRequiredService<ILexer>();
            lexer.Lex();
        }
    }
}
