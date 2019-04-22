using Autenticacion.Manager;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.DirectoryServices;
using System.Collections.Generic;

namespace Controlador
{
    class Program
    {
        
        static void Main(string[] args)
        {
            string Opcion = "0";
            List<Partida> partidas = new List<Partida>();

            while(Convert.ToInt32(Opcion) != 3)
            {
                Console.Clear();
                Console.WriteLine("Digite 1 para crear una partida nueva.");
                Console.WriteLine("Digite 2 para terminar las partidas y salir.");
                Console.WriteLine("Digite 3 para salir sin terminar las partidas.");
                Opcion = Console.ReadLine();
                switch (Convert.ToInt32(Opcion))
                {
                    case 1:
                        Partida part = new Partida();
                        break;
                    case 3:
                        Console.WriteLine("salida.");
                        break;
                }
            }
        }
    }
}
