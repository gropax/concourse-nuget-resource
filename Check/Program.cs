using System;
using System.Threading.Tasks;

namespace Check
{
    class Program
    {
        static async Task Main(string[] args)
        {
            await new Command(Console.In, Console.Error, Console.Out).Run();
        }
    }
}
