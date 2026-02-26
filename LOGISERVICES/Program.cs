using LOGISERVICES;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace LOGISERVICES
{
    static class Program
    {
        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        /// 

        //#if DEBUG
        
                static void Main()
                
         {
                    var service = new Service1();
                    service.DebugStart();
                }
        
        //#else
/*
        static void Main()
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new Service1()
            };
            ServiceBase.Run(ServicesToRun);
        }
        ///*
        //#endif
        //*/
    }
}



//using LOGISERVICES;
//using System;
//using System.Linq;
//using System.ServiceProcess;

//static class Program
//{
//    static void Main(string[] args)
//    {
//        // Si se ejecuta desde consola (con argumento --console)
//        if (Environment.UserInteractive || args.Contains("--console"))
//        {
//            RunAsConsole(args);
//        }
//        else // Ejecución normal como servicio
//        {
//            ServiceBase[] ServicesToRun;
//            ServicesToRun = new ServiceBase[]
//            {
//                new Service1()
//            };
//            ServiceBase.Run(ServicesToRun);
//        }
//    }

//    private static void RunAsConsole(string[] args)
//    {
//        var service = new Service1();

//        Console.WriteLine("Presiona:");
//        Console.WriteLine("  'S' para iniciar el servicio");
//        Console.WriteLine("  'T' para detener el servicio");
//        Console.WriteLine("  'Q' para salir");
//        Console.WriteLine();

//        var running = true;
//        while (running)
//        {
//            var key = Console.ReadKey(true).Key;

//            switch (key)
//            {
//                case ConsoleKey.S:
//                    Console.WriteLine("Iniciando servicio...");
//                    service.Start(args);
//                    Console.WriteLine("Servicio en ejecución");
//                    break;

//                case ConsoleKey.T:
//                    Console.WriteLine("Deteniendo servicio...");
//                    service.Stop();
//                    Console.WriteLine("Servicio detenido");
//                    break;

//                case ConsoleKey.Q:
//                    Console.WriteLine("Saliendo...");
//                    service.Stop();
//                    running = false;
//                    break;
//            }
//        }
//    }
//}


