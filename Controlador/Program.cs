using Autenticacion.Manager;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.DirectoryServices;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.Linq;

namespace Controlador
{
    class Program
    {
        private static bool ServidorEjecutando = false;
        private static Int32 puerto = 13000;
        private static TcpListener server = null;
        private static Partida partida = null;
        private static List<TcpClient> ConexionesRegistradas = new List<TcpClient>();
        private static PrincipalContext context = null;
        private static int jugadoresTurnoActivo = 0;
        private static bool TurnoEnJuego = false;
        private static bool ApuestaHecha = false;
        private static int[] ReadyPlayer = new int[] { 0, 0, 0, 0, 0, 0, 0 };//0 = no aposto, 1 = aposto, 2= se paso de verga
        private static int contDeAnimacionesPorHacer = 0;
        private static int[] apuestasEnEjecucion = new int[] { 0, 0, 0, 0, 0, 0, 0 };
        private static bool calculandoGanadores = false;
        private static bool resetReady = false;

        static void Main(string[] args)
        {
            string Opcion = "0";  
            try
            {
                context = new PrincipalContext(ContextType.Domain, "UNA", "administrador", "Una123");
                LoginUsuarioServidor("felipe", "Una-123");
                Console.WriteLine("Conexion con Servidor Windows 2012 lograda.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("no se pudo conectar con windows server.  EX: " + ex);
            }
            Console.WriteLine("Digite 1 para crear una partida.");
            Console.WriteLine("Digite 2 para terminar la partida y salir.");
            Console.WriteLine("Digite 3 para salir sin terminar la partida.");
            while (Convert.ToInt32(Opcion) != 3)
            {
                Opcion = Console.ReadLine();
                switch (Convert.ToInt32(Opcion))
                {
                    case 1:
                        if (partida == null) {
                            partida = new Partida();
                            IniciarServidor();
                        }
                        else
                        {
                            Console.WriteLine("Ya existe una partida en ejecucion.");
                        }
                        break;
                    case 2:
                        Console.WriteLine("Finalizando Conexiones.");
                        if( partida!= null)
                        {
                            Broadcast("X");
                        }
                        break;
                    case 3:
                        Console.WriteLine("salida. Esperando que finalice una partida.");
                        break;
                }
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
                    ConexionesRegistradas.Add(clienteNuevo);
                    string ipCliente = ((IPEndPoint)clienteNuevo.Client.RemoteEndPoint).Address.ToString();
                    Console.WriteLine("Conexion establecida con IP: "+ipCliente);
                    //manda a tratar ese cliente nuevo en un hilo
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
            string ipCliente = ((IPEndPoint)cliente.Client.RemoteEndPoint).Address.ToString();
            menRecibido = null;
            NetworkStream stream = cliente.GetStream();
            int i = 0;
            while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
            {
                // Translate data bytes to a ASCII string.
                menRecibido = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
                char accion = menRecibido.ElementAt(0);
                switch (accion)
                {
                    case 'R':
                        //caso de registro
                        Console.WriteLine(ipCliente + "   INTENTA REGISTRO");
                        string usuarioReg = BuscarEnString(menRecibido, ":", "/");
                        string contraReg = BuscarEnString(menRecibido, "/", "^");
                        if (RegistrarEnServidor(usuarioReg, contraReg))
                        {
                            EnviarMensajeACliente(ipCliente, "R");
                        }
                        else
                        {
                            EnviarMensajeACliente(ipCliente, "E:No se pudo completar el registro.");
                        }
                        break;
                    case 'L':
                        //caso de login
                        Console.WriteLine(ipCliente + "   INTENTA LOGIN");
                        string usuario = BuscarEnString(menRecibido, ":", "/");
                        string contra = BuscarEnString(menRecibido, "/", "^");
                        if (LoginUsuarioServidor(usuario, contra)==true)
                        {
                            //se valido el login
                            Console.WriteLine(ipCliente + "  LOGIN VALIDO CON USUARIO:  " + usuario);
                            UserPrincipal user = UserPrincipal.FindByIdentity(context, IdentityType.SamAccountName, usuario);
                            int plata = 100;
                            if (user != null)//revisa cuanta plata tiene el jugador
                            {
                                plata += int.Parse(user.Description);
                                Jugador jug = new Jugador(usuario, ipCliente, plata);
                                if (partida.agregarJugador(jug))
                                {
                                    PonerNombresEnMesa();
                                }
                                if (!TurnoEnJuego)
                                {
                                    jugadoresTurnoActivo++;
                                }
                                EnviarMensajeACliente(ipCliente, "L");
                                if (partida.getCantJugadores() == 1)
                                {
                                    //significa que acaba de hacer login el primer jugador, iniciar la vara
                                    EnviarMensajeACliente(ipCliente, "H");
                                    TurnoEnJuego = true;
                                }
                            }
                            else
                            {
                                EnviarMensajeACliente(ipCliente, "E:No se pudo completar el login.");
                            }
                            
                        }
                        else
                        {
                            EnviarMensajeACliente(ipCliente, "E:No se pudo completar el login.");
                        }
                        break;
                    case 'P':
                        //caso de pedir carta
                        //pide carta y a la vez manda un broadcast para que todos lo agreguen en pantalla
                        string resp = partida.PedirCarta(ipCliente);
                        string posJugg = resp.ElementAt(0).ToString();
                        Broadcast("P:"+resp+"^");
                        //verificar que no se haya pasado de 21 
                        if (partida.getPuntosCliente(ipCliente) > 21)
                        {
                            //el cliente perdio
                            ReadyPlayer[int.Parse(posJugg)] = 2;
                            EnviarMensajeACliente(ipCliente,"U:Usted ha perdido. Se paso de cartas.^");
                        }
                        break;
                    case 'Q':
                        // de reset jugada
                        if (resetReady)
                        {
                            //broadcast limpiar mesa
                            ApuestaHecha = false;
                            resetJugada();
                            resetReady = false;
                        }
                        
                        break;
                    case 'T':
                        //caso de terminar turno
                        //avisar a partida que el jugador termino turno, si el vector de termino turno tiene puros true, ejecutar
                        Console.WriteLine(ipCliente + "   ANUNCIO FIN DE TIEMPO");
                        jugadoresTurnoActivo--;
                        if (jugadoresTurnoActivo == 0)
                        {
                            if (ApuestaHecha) {
                                string cartas1 = partida.RepartirCartas(ReadyPlayer);
                                Broadcast(cartas1 + partida.RepartirCartas(ReadyPlayer));
                                Console.WriteLine("Casa intenta jugar");
                                //falta asignar turnos antes de la casa jugar
                                //JugarCasa();
                            }
                            jugadoresTurnoActivo = partida.getCantJugadores();
                            //resetJugada();
                            if (!ApuestaHecha)
                            {
                                Broadcast("H");
                            }
                        }
                        break;
                    case 'A':
                        //caso de Apostar
                        if (contDeAnimacionesPorHacer < 0)
                        {
                            contDeAnimacionesPorHacer = 0;
                        }
                        contDeAnimacionesPorHacer++;
                        string cant = BuscarEnString(menRecibido, ":", "/");
                        Console.WriteLine("Se va apostar un monto de:  " + cant);
                        //apuesta es un metodo que me regresa la posicion en mesa del jugador y se encarga de hacer la apuesta en la partida
                        int pos = Apuesta(ipCliente, int.Parse(cant));
                        ReadyPlayer[pos] = 1;
                        Console.WriteLine("se va a poner la apuesta en la pos:  " + pos);
                        Broadcast("A:"+pos+"/"+int.Parse(cant)+"^");
                        ApuestaHecha = true;
                        break;
                    case 'X':
                        //caso de cerrar conexion
                        if (partida.RemoverJugador(ipCliente))
                        {
                            //anunciar a jugadores que se fue un jugador

                        }
                        break;
                    case 'F':
                        Console.WriteLine(ipCliente + "  Anuncio FIN DE TURNO");
                        //caso donde ocupo asignar turno
                        contDeAnimacionesPorHacer--;
                        if (contDeAnimacionesPorHacer <= 0)
                        {
                            //buscar en ready player primer jugador en true, asignarle turno y pasarlo a false
                            for (int o = 0; o < ReadyPlayer.Count(); o++)
                            {
                                if (ReadyPlayer[o] == 1)
                                {
                                    Broadcast("G:" + o + "^");
                                    EnviarMensajeACliente(partida.getJugadoresMesa().ElementAt(o).getIp(), "W:SU TURNO HA LLEGADO");
                                    ReadyPlayer[o] = 0;
                                    break;
                                }
                                if (o == ReadyPlayer.Count()-1)
                                { 
                                    JugarCasa();
                                }
                            }
                        }
                        break;
                    case 'M':
                        Console.WriteLine("INGRESO M");
                        //caso de termino animaciones agregar carta a mesa
                        if (!calculandoGanadores)
                        {
                            calcGanadores();
                            calculandoGanadores = true;
                        }
                        
                        break;
                        
                }
            }

        }

        private static bool RegistrarEnServidor(string usuario, string contra)
        {
            try
            {
                UserPrincipal usuarioNuevo = new UserPrincipal(context);
                usuarioNuevo.SamAccountName = usuario;
                usuarioNuevo.SetPassword(contra);
                usuarioNuevo.Description = "10000";
                usuarioNuevo.Enabled = true;
                usuarioNuevo.Save();
                Console.WriteLine("Registro Posible. Usuario nombre: "+usuario);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al crear usuario. Exception: " + ex);
                return false;
            }
        }

        private static bool LoginUsuarioServidor(string usuario, string contra)
        {
            return context.ValidateCredentials(usuario, contra);
        }

        private static bool ActualizarUsuarioEnServidor(string usuario, int plata)
        {
            bool valido = false;
            UserPrincipal user = UserPrincipal.FindByIdentity(context, IdentityType.SamAccountName, usuario);
            if (user != null)
            {
                user.Description = plata.ToString();
                user.Save();
                user.Dispose();
                valido = true;
                //or maybe you need user.UserPrincipalName;
            }
            else
            {
                Console.WriteLine("nel pastel. No se encontro usuario para actualizar.");
            }
            context.Dispose();
            return valido;
        }

        private static string BuscarEnString(string strSource, string strStart, string strEnd)
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

        private static void EnviarMensajeACliente(string IPcliente, string mensaje)
        {
            Console.WriteLine("se va a intentar enviar mensaje a cliente con IP: " + IPcliente);
            Console.WriteLine("Mensaje contiene: " + mensaje);
            for(int i = 0; i < ConexionesRegistradas.Count; i++)
            {
                if(IPcliente == ((IPEndPoint)ConexionesRegistradas.ElementAt(i).Client.RemoteEndPoint).Address.ToString())
                {
                    Console.WriteLine("Se encontro el cliente");
                    //se ubico cliente, enviar mensaje
                    //pasar el mensaje de string a bytes
                    TcpClient cliente = new TcpClient(IPcliente.Remove(0,7), puerto);
                    Byte[] data = System.Text.Encoding.ASCII.GetBytes(mensaje);
                    NetworkStream stream = cliente.GetStream();
                    stream.Write(data, 0, data.Length);
                    Console.WriteLine("Se envio el mensaje");
                    cliente.Close();
                    break;
                }
            }
        }

        private static int Apuesta(string ip, int cant)
        {
            int pos = partida.getPosJugador(ip);
            apuestasEnEjecucion[pos] = cant;
            return pos;
        }

        private static void Broadcast(string mensaje)
        {
            for(int i = 0; i < ConexionesRegistradas.Count(); i++)
            {
                EnviarMensajeACliente(((IPEndPoint)ConexionesRegistradas.ElementAt(i).Client.RemoteEndPoint).Address.ToString(), mensaje);
            }
        }

        private static void JugarCasa()
        {
            if (partida.getPuntosCasa() >= 17)
            {
                //brincar a la oyta varra, seguir a evaluar jugadores a ver quienes ganarin
                Broadcast("K");
            }
            else
            {


                string cartas = "";

                while (partida.getPuntosCasa() < 17)
                {
                    cartas += ("K:7/" + partida.CasaPedirCarta() + "%^");
                    Console.WriteLine("CASA PIDIO CARTA");
                }
                Broadcast(cartas);
            }
        }

        private static void PonerNombresEnMesa()
        {
            List<Jugador> jugadoresEnMesa = partida.getJugadoresMesa();
            for (int i = 0; i < partida.getCantJugadores(); i++)
            {
                string posEnMesa = partida.getPosJugador(jugadoresEnMesa.ElementAt(i).getIp()).ToString();
                string usuario = jugadoresEnMesa.ElementAt(i).getUsuario();
                Broadcast("N:" + posEnMesa + "/" + usuario + "^");
            }
        }

        private static void resetJugada()
        {
            for(int i = 0; i < 7; i++)
            {
                apuestasEnEjecucion[i] = 0;
            }
            Broadcast("V");
            TurnoEnJuego = false;
            contDeAnimacionesPorHacer = 0;
            partida.LimpiarManoCasa();
            calculandoGanadores = false;
            if (!ApuestaHecha)
            {
                Broadcast("H");
            }
        }

        private static void calcGanadores()
        {
            Console.WriteLine("se va intentar calcular ganadores");
            string mensaje = "";
            if (partida.getPuntosCasa() > 21)
            {
                Console.WriteLine("SE DETECTO QUE lA CASA TIENE EXCESO DE PUNTOS ");
                for(int i = 0; i < partida.getJugadoresMesa().Count(); i++)
                {
                    if (partida.getPuntosCliente(partida.getJugadoresMesa().ElementAt(i).getIp()) <= 21)
                    {
                        Console.WriteLine("JUGADOR GANO PORQUE TIENE MENOS DE 21 Y LA CASA SE PASO" + i);
                        mensaje += "O:" + i + "/1%^";
                    }
                }
            }
            else
            {
                for (int i = 0; i < 7; i++)
                {
                    Console.WriteLine("EL JUGADOR EN LA POS: " + i + " APOSTO: " + apuestasEnEjecucion[i] + "  CANT DE CARTAS QUE TIENE ESE JUGADOR:  ");// + partida.getPuntosCliente(partida.getJugadoresMesa().ElementAt(i).getIp()));
                    if (apuestasEnEjecucion[i] != 0)
                    {

                        if (partida.getPuntosCliente(partida.getJugadoresMesa().ElementAt(i).getIp()) > 21)
                        {
                            Console.WriteLine("SE DETECTO QUE PERDIO JUGADOR EXCESO DE CARTAS EN POS " + i);
                            mensaje += "O:" + i + "/2%^";
                            continue;//perdio
                        }

                        if (partida.getPuntosCasa() > partida.getPuntosCliente(partida.getJugadoresMesa().ElementAt(i).getIp()))
                        {
                            //caso de que perdio
                            partida.getJugadoresMesa().ElementAt(i).setPlata(partida.getJugadoresMesa().ElementAt(i).getPlata() - apuestasEnEjecucion[i]);
                            Console.WriteLine("SE DETECTO QUE PERDIO JUGADOR EN POS " + i);
                            mensaje += "O:" + i + "/2%^";
                        }
                        else
                        {
                            if (partida.getPuntosCasa() < partida.getPuntosCliente(partida.getJugadoresMesa().ElementAt(i).getIp()))
                            {
                                //caso de que gano
                                partida.getJugadoresMesa().ElementAt(i).setPlata(partida.getJugadoresMesa().ElementAt(i).getPlata() + apuestasEnEjecucion[i]);
                                mensaje += "O:" + i + "/1%^";
                                Console.WriteLine("SE DETECTO QUE GANO JUGADOR EN POS " + i);
                            }
                            else
                            {
                                mensaje += "O:" + i + "/3%^";
                                Console.WriteLine("SE DETECTO QUE EMPATO JUGADOR EN POS " + i);
                            }
                        }
                    }
                }
            }
            Broadcast(mensaje);
            resetReady = true;
            calculandoGanadores = false;
        }
    }
}
