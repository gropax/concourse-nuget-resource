using Resource;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading;
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
