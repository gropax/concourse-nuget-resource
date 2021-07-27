using System;
using System.Threading.Tasks;

namespace In
{
    class Program
    {
        static async Task Main(string[] args)
        {
            await new Command(Console.In, Console.Error, Console.Out, args[0]).Run();
        }
    }
}
