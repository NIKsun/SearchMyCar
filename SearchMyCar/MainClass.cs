using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace SearchMyCar
{
    class MainClass
    {
        

        static void Main(string[] args)
        {
            if (args.Length < 2)
            {
                Console.WriteLine("Not enough arguments\n");
                return;
            }
            Console.WriteLine("{0}, {1}\n", args[0], args[1]);
            HandlerRequest hr= new HandlerRequest(args[0],args[1]);
            hr.start();
        }
    }
}
