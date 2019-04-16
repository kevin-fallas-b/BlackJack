﻿using Autenticacion.Manager;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.DirectoryServices;

namespace Controlador
{
    class Program
    {
        private static bool ServidorEjecutando = false;
        private static Int32 puerto = 13000;
        private static TcpListener server = null;


        static void Main(string[] args)
        {
            IniciarServidor();
            AutenticationManager a = new AutenticationManager();
            DirectoryEntry de = a.GetDirectoryEntry();
            while (true)
            {

            }
        }

        private static void IniciarServidor()
        { 
            try
            {
                server = TcpListener.Create(puerto);
                server.Start();
                ServidorEjecutando = true;
                Thread esperador = new Thread(new ThreadStart(EsperarConexion));
                esperador.IsBackground = true;
                esperador.Start();
                
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }
            
        }

        private static void EsperarConexion()
        {
            if (ServidorEjecutando)
            {
                //entrar en un loop infinito donde espera conexiones
                while (true)
                {
                    Console.WriteLine("Esperando conexion.");
                    //cliente es igual a lo que le entra al socket
                    TcpClient clienteNuevo = server.AcceptTcpClient();
                    Console.WriteLine("Conexion establecida.");
                    Thread t = new Thread(new ParameterizedThreadStart(LidiarConexion));
                    t.Start(clienteNuevo);
                }
            }
        }

        private static void LidiarConexion(Object obj)
        {
            //buffer donde se me almacena los mensajes recibidos en el socket
            Byte[] bytes = new Byte[1024];
            String menRecibido = null;
            TcpClient cliente = (TcpClient)obj;
            menRecibido = null;
            NetworkStream stream = cliente.GetStream();
            int i = 0;
            Console.WriteLine(((IPEndPoint)cliente.Client.RemoteEndPoint).Address.ToString());
            while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
            {
                // Translate data bytes to a ASCII string.
                menRecibido = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
                Console.WriteLine("Recibido mensaje de {0}", menRecibido);

                // Process the data sent by the client.
                menRecibido = menRecibido.ToUpper();

                byte[] msg = System.Text.Encoding.ASCII.GetBytes("Mensaje Recibido, decia: " + menRecibido);

                // Send back a response.
                stream.Write(msg, 0, msg.Length);
                Console.WriteLine("Se envio mensaje de afirmacion.");
            }
            //cerrar conexion
            cliente.Close();
            Console.WriteLine("Finalizo conexion con el cliente.");

        }
        private static bool ValidarJugador(string nom,string cont)
        {
            AutenticationManager aut = new AutenticationManager();
            return aut.ValidarUsuario(nom,cont);   
        }
    }

}
