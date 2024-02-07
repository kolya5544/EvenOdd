using EmbedIO;
using EmbedIO.WebApi;
using System;
using System.Threading;

namespace EvenOdd
{
    class Program
    {
        static void Main(string[] args)
        {
            var ws = CreateWebServer("http://127.0.0.1:6418/");
            ws.RunAsync();
            Console.WriteLine("=[ Server is up! ]=");

            while (true)
            {
                Thread.Sleep(5000);
            }
        }

        private static WebServer CreateWebServer(string url)
        {
            var server = new WebServer(o => o
                    .WithUrlPrefix(url)
                    .WithMode(HttpListenerMode.EmbedIO))
                .WithWebApi("/", Controller.AsJSON, m => m
                    .WithController<Controller>());

            return server;
        }
    }
}
