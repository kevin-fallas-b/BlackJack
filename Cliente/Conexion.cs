using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Windows;

namespace Cliente
{
    class Conexion
    {
        private static Conexion instancia = null;
        private TcpClient cliente = new TcpClient();
        private Int32 puerto = 13000;
        private bool conectado = false;

        private Conexion()
        {

        }
        
        public static Conexion Instance
        {
            get
            {
                if (instancia == null)
                {
                    instancia = new Conexion();
                }
                return instancia;
            }
        }

        public bool Conectar(string direccionIp)
        {
            try
            {

                cliente = new TcpClient(direccionIp, puerto);
                conectado = true;
                PantUsuario pantUsuario = new PantUsuario();
                pantUsuario.ShowDialog();
            }
            catch (SocketException e)
            {
                MessageBox.Show("No se encontro un servidor con dicha direccion IP");
                Console.WriteLine("Se cayo esta pecha\nSocketException: {0}", e);
            }
            return true;
        }

        public bool TerminarConexion()
        {
            conectado = false;
            cliente.Close();
            return true;
        }

        public string EnviarMensaje(string mensaje)
        {
            String respuesta = "hola";
            if (conectado)
            {
                //pasar el mensaje de string a bytes
                Byte[] data = System.Text.Encoding.ASCII.GetBytes(mensaje);
                NetworkStream stream = cliente.GetStream();
                stream.Write(data, 0, data.Length);
                //listo el envio, ahora recibir respuesta del servidor
                data = new Byte[256];
                respuesta = String.Empty;
                Int32 bytes = stream.Read(data, 0, data.Length);
                respuesta = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
                Console.WriteLine("Recibido: {0}", respuesta);
            }
            else
            {
                Console.WriteLine("No se puede enviar mensaje. No existe conexion.");
            }
            return respuesta;
        }
    }
}
