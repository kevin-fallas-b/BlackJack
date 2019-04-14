using System;
using System.Net;
using System.Net.Sockets;


namespace Controlador
{
    class Program
    {
        
        static void Main(string[] args)
        {
            Int32 puerto = 13000;
            IPAddress direccionLocal = IPAddress.Parse("127.0.0.1");
            //socket por donde me van a entrar datos
            TcpListener server = null;
            try
            {
                server = new TcpListener(direccionLocal, puerto);
                server.Start();

                //buffer donde se me almacena los mensajes recibidos en el socket
                Byte[] bytes = new Byte[1024];
                String menRecibido = null;

                //entrar en un loop infinito donde espera conexiones
                while (true)
                {
                    Console.WriteLine("Esperando conexion.");
                    //cliente es igual a lo que le entra al socket
                    TcpClient cliente = server.AcceptTcpClient();
                    Console.WriteLine("Conexion establecida.");
                    menRecibido = null;

                    NetworkStream stream = cliente.GetStream();

                    int i = 0;
                    while ((i = stream.Read(bytes, 0, bytes.Length)) !=0 )
                    {
                        // Translate data bytes to a ASCII string.
                        menRecibido = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
                        Console.WriteLine("Received: {0}", menRecibido);

                        // Process the data sent by the client.
                        menRecibido = menRecibido.ToUpper();

                        byte[] msg = System.Text.Encoding.ASCII.GetBytes("Mensaje Recibido, decia: "+menRecibido);

                        // Send back a response.
                        stream.Write(msg, 0, msg.Length);
                        Console.WriteLine("Se envio mensaje de afirmacion.");
                    }
                    //cerrar conexion
                    cliente.Close();
                    Console.WriteLine("Finalizo conexion con el cliente.");
                }
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }
            finally
            {
                //cerrar el servidor  para que no se caiga toda la madre
                server.Stop();
            }
        }
    }
}
