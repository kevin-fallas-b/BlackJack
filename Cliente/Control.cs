using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Windows;
using System.Threading;

namespace Cliente
{
    class Control
    {
        private static Control conexion = null;
        private static PantInicio pantinicio = null;
        private static PantUsuario pantusuario = null;
        private static PantJuego pantjuego = null;
        private TcpClient cliente = new TcpClient();
        private TcpListener server = null;
        private string direcIp = null;
        private int puerto = 13000;
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
                if (direcIp == null)
                {
                    direcIp = direccionIp;
                }
                IniciarServidor();
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
                //ocupo pedirle al servidor que guarde mi informacion.
                conectado = false;
                EnviarMensaje("X");
                cliente.Close();
            }
        }

        public void EnviarMensaje(string mensaje)
        {
            
            if (conectado)
            {
                //pasar el mensaje de string a bytes
                Byte[] data = System.Text.Encoding.ASCII.GetBytes(mensaje);
                NetworkStream stream = cliente.GetStream();
                stream.Write(data, 0, data.Length);              
            }
            else
            {
                Console.WriteLine("No se puede enviar mensaje. No existe conexion.");
            }
        }

        public void setPantInicio(PantInicio pant)
        {
            pantinicio = pant;
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

        private void IniciarServidor()
        {
            try
            {
                server = TcpListener.Create(puerto);
                server.Start();
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
            while (true)
            {
                Console.WriteLine("Esperando conexion.");
                //cliente es igual a lo que le entra al socket
                TcpClient clienteNuevo = server.AcceptTcpClient();
                Console.WriteLine("Conexion establecida.");
                //manda a tratar ese cliente nuevo en un hilo
                Thread t = new Thread(new ParameterizedThreadStart(LidiarConexion));
                t.Start(clienteNuevo);
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
            while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
            {
                // Translate data bytes to a ASCII string.
                menRecibido = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
                Console.WriteLine("llego en mensaje: " + menRecibido);
                char accion = menRecibido.ElementAt(0);
                switch (accion)
                {
                    case 'A':
                        //Caso de apuesta
                        //A:posicionjugador/cantidad^
                        //aqui es hacer la animacion en el lugar que me entro
                        int pos = int.Parse(BuscarEnString(menRecibido, ":", "/"));
                        int cant = int.Parse(BuscarEnString(menRecibido, "/", "^"));
                        pantjuego.animacionApostar(pos, cant);
                        break;
                    case 'N':
                        //caso de nombre de jugador y posicion en la mesa
                        //N:pos/nombre^
                        break;
                    case 'R':
                        //caso de registro exitoso
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            pantUsuario.MensajePopUp("Registro possible. Por favor dirigase al sector de Log In e inicia sesion.")
                            
                        });
                        break;
                    case 'H':
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            pantJuego.iniciarTiempoApuestas();
                        });
                        break;
                    case 'L':
                        //caso de login exitoso
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            pantJuego.Show();
                            pantUsuario.Close();
                            //Conectar(direcIp);
                        });
                        break;
                    case 'P':
                        //caso de pedir carta
                        //P:posicionJugador/carta^
                        //hacer la animacion de poner la carta que llego en el jugador que indica 
                        int posJug = int.Parse(BuscarEnString(menRecibido, ":", "/"));
                        string carta = BuscarEnString(menRecibido, "/", "^");
                        //aqui hacer pantJuego.repartirCarta(pos, carta);
                        break;
                    case 'Q':
                        //caso de quedarse
                        break;
                    case 'T':
                        //caso de terminar turno
                        //aqui es deshabilitar los botones de apostar y pedir cartas, mientras juega la casa
                        //pantjuego.deshabilitarBotones();
                        break;
                    case 'E':
                        //caso de Error
                        MessageBox.Show("Se ha producido un error: " + menRecibido.Remove(0,2));
                        
                        break;
                    case 'X':
                        //caso de cerrar conexion
                        //La partida por la razon que sea ha finalizado.
                        TerminarConexion();
                        break;
                    case 'Z':
                        //caso de poner cartas en mesa
                        string[] cartasAponer = menRecibido.Split('^');
                        break;
                }
            }
            //cerrar conexion
            cliente.Close();
        }
    }  
}
