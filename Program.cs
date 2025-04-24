using System;
using System.Threading.Tasks;

class Program
{
    static async Task Main()
    {
        await HomeNetNavTest.Run();
        await OpenAIChatTest.Run();
    }
}
