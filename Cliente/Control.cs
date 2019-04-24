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
    class Control
    {
        private static Control conexion = null;
        private static PantInicio pantinicio = null;
        private static PantUsuario pantusuario = null;
        private static PantJuego pantjuego = null;
        private TcpClient cliente = new TcpClient();
        private Int32 puerto = 13000;
        private bool conectado = false;

        private Control()
        {
        }

        public static Control Conexion
        {
            get
            {
                if (conexion == null)
                {
                    conexion = new Control();
                }
                return conexion;
            }
        }
        
        public static PantInicio pantInicio
        {
            get
            {
                if (pantinicio == null)
                {
                    pantinicio = new PantInicio();
                }
                return pantinicio;
            }
        }

        public static PantUsuario pantUsuario
        {
            get
            {
                if (pantusuario == null)
                {
                    pantusuario = new PantUsuario();
                }
                return pantusuario;
            }
        }
        public static PantJuego pantJuego
        {
            get
            {
                if (pantjuego == null)
                {
                    pantjuego = new PantJuego();
                }
                return pantjuego;
            }
        }
        public bool Conectar(string direccionIp)
        {
            try
            {

                cliente = new TcpClient(direccionIp, puerto);
                conectado = true;
                return true;
            }
            catch (SocketException e)
            {
                MessageBox.Show("No se encontro un servidor con dicha direccion IP");
                Console.WriteLine("SocketException: {0}", e);
                return false;
            }
            
        }

        public void TerminarConexion()
        {
            if (conectado)
            {
                conectado = false;
                cliente.Close();
            }
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

        public void setPantInicio(PantInicio pant)
        {
            pantinicio = pant;
        }
    }
}
