using System.Reflection;
using System.Text.RegularExpressions;
using Fun3.Lexers;
using Microsoft.Extensions.DependencyInjection;

namespace Fun3.StartInterpreter
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
            var lines = File.ReadAllLines(args[0]);

            var pattern = @"\w+";
            var rg = new Regex(pattern);

            var match = rg.Match(lines[0]);
            Console.WriteLine(match.Success ? match.Index : "not found");
        }
    }
}
