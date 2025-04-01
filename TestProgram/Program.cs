// Presentation/Program.cs
using System;
using Logic;

namespace Presentation
{
    class Program
    {
        static void Main(string[] args)
        {
            var service = new MessageService();
            string message = service.GetHelloMessage();
            Console.WriteLine(message);
        }
    }
}