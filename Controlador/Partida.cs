using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;

namespace Controlador
{
    class Partida
    {
        private bool ServidorEjecutando = false;
        private Int32 puerto = 13000;
        private IPAddress DireccIP = null;
        private TcpListener server = null;
        
        public Partida()
        {
            IniciarServidor();
        }

        public Partida(IPAddress direccIP, int puertoPar)
        {
            DireccIP = direccIP;
            puerto = puertoPar;
            IniciarServidor();
        }

        private void IniciarServidor()
        {
            try
            {
                if (DireccIP == null) {
                    server = TcpListener.Create(puerto);
                }
                else
                {
                    server = new TcpListener(DireccIP, puerto);
                }
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

        private void EsperarConexion()
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

        private void LidiarConexion(Object obj)
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
                char accion = menRecibido.ElementAt(0);
                Console.WriteLine("Accion: " + accion);
                switch (accion)
                {
                    case 'R':
                        string usuario = BuscarEnString(menRecibido, ":", "/");
                        string contra = BuscarEnString(menRecibido, "/", "^");
                        RegistrarEnServidor(usuario, contra);
                        break;
                    case 'L':
                        break;
                    case 'P':
                        break;
                    case 'Q':
                        break;
                    case 'T':
                        break;
                    case 'A':
                        break;
                    case 'E':
                        break;
                }
            }
            
            //cerrar conexion
            cliente.Close();
            Console.WriteLine("Finalizo conexion con el cliente.");

        }

        private void RegistrarEnServidor(string usuario, string contra)
        {
            Console.WriteLine("Se va a intentar crear usuario en servidor");
            PrincipalContext context = new PrincipalContext(ContextType.Domain, "UNA", "administrador", "Una123");
            try
            {
                UserPrincipal usuarioNuevo = new UserPrincipal(context);
                usuarioNuevo.SamAccountName = usuario;
                usuarioNuevo.SetPassword(contra);
                usuarioNuevo.Enabled = true;
                usuarioNuevo.Save();
                Console.WriteLine("Guardado");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al crear usuario. Exception: " + ex);
            }
        }
        private string BuscarEnString(string strSource, string strStart, string strEnd)
        {
            int Start, End;
            if (strSource.Contains(strStart) && strSource.Contains(strEnd))
            {
                Start = strSource.IndexOf(strStart, 0) + strStart.Length;
                End = strSource.IndexOf(strEnd, Start);
                return strSource.Substring(Start, End - Start);
            }
            else
            {
                return "";
            }
        }
    }
}
