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
        private List<Jugador> enEspera;
        private List<Jugador> enMesa;
        private bool[] terminoTurno;  
        private static string[] letters = new string[] { "C", "T", "P", "D" };
        private static string[] values = new string[] { "2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K", "A" };
        private static Carta[] baraja = new Carta[52];
        private static int contBaraja = 0;//cont que me dice que carta sacar de la baraja
        private static List<Carta> CartasCasa = new List<Carta>();

        public Partida()
        {
            enMesa = new List<Jugador>();
            enMesa.Clear();
            enEspera = new List<Jugador>();
            enEspera.Clear();
            CrearBaraja();
            RevolverBaraja();
        }

        public Partida(IPAddress direccIP, int puertoPar)
        {
            enMesa = new List<Jugador>();
            enEspera = new List<Jugador>();
        }

       
        public void TerminarPartidaDesdeControlador()
        {

        }

        public bool agregarJugador(Jugador jug)
        {
            if (enMesa.Count() >= 7)
            {
                enEspera.Add(jug);
                return false;
            }
            else
            {
                enMesa.Add(jug);
                return true;
            }
        }

        public int getCantJugadores()
        {
            return enMesa.Count();
        }

        public string getCarta()
        {
            string carta = null;

            return carta;
        }

        public bool RemoverJugador(string ip)
        {
            bool eliminado = false;
            for(int i = 0; i < enMesa.Count(); i++)
            {
                if(ip == enMesa.ElementAt(i).getIp())
                {
                    eliminado = true;
                    enMesa.RemoveAt(i);
                    break;
                }
            }          
            if (eliminado)
            {
                enMesa.Add(enEspera.ElementAt(0));
                Console.WriteLine("se agrego a la mesa el jugador " + enEspera.ElementAt(0).getUsuario());
                enEspera.RemoveAt(0);
            }

            return eliminado;
        }

        public string PedirCarta(string ipJug)
        {
            //generar carta
            string NombCar = "";
            for(int i = 0; i < enMesa.Count(); i++)
            {
                if(ipJug == enMesa.ElementAt(i).getIp())
                {
                    enMesa.ElementAt(i).setCartas(baraja[contBaraja]);
                    NombCar = (i+"/"+baraja[contBaraja].getNombre());
                    contBaraja++;
                    break;
                }
            }
            return NombCar;
        }

        public int getPosJugador(string ip)
        {
            for(int i = 0; i < enMesa.Count(); i++)
            {
                if(ip == enMesa.ElementAt(i).getIp())
                {
                    return i;
                }
            }
            return -1;
        }

        private void CrearBaraja()
        {
            int contC = 0;
            for(int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 13; j++)
                {
                    baraja[contC] = new Carta((letters[i] + values[j]), values[j]);
                    contC++;
                }
            }
        }

        private void RevolverBaraja()
        {
            new Random().Shuffle(baraja);
        }

        public List<Jugador> getJugadoresMesa()
        {
            return enMesa;
        }

        public string RepartirCartas(int[] readyPlayer)
        {
            string cartas = "";
            for(int i=0;i < 7; i++)
            {
                if (readyPlayer[i]==1)
                {
                    enMesa.ElementAt(i).setCartas(baraja[contBaraja]);
                    cartas += ("Z:" +i+"/"+baraja[contBaraja].getNombre()+"%^");
                    contBaraja++;
                }
            }
            CartasCasa.Add(baraja[contBaraja]);
            cartas += ("Z:7" +"/" + baraja[contBaraja].getNombre()+"%^");
            contBaraja++;
            return cartas;
        }

        public int getPuntosCliente(string ipJug)
        {
            int puntos = 0;
            for(int i = 0; i < enMesa.Count(); i++)
            {
                if(ipJug == enMesa.ElementAt(i).getIp())
                {
                    for(int k = 0; k < enMesa.ElementAt(i).getCartasEnMano().Count(); k++)
                    {
                        puntos += enMesa.ElementAt(i).getCartasEnMano().ElementAt(k).getValor();
                    }
                }
            }
            return puntos;
        }

        public int getPuntosCasa()
        {
            int puntos = 0;
            for(int i = 0; i < CartasCasa.Count(); i++)
            {
                puntos += CartasCasa.ElementAt(i).getValor();
            }
            return puntos;
        }

        public string CasaPedirCarta()
        {
            CartasCasa.Add(baraja[contBaraja]);
            string carta = baraja[contBaraja].getNombre();
            contBaraja++;
            return carta;
        }

        public void LimpiarManoCasa()
        {
            CartasCasa = new List<Carta>();
            for(int i = 0; i < enMesa.Count(); i++)
            {
                enMesa.ElementAt(i).setCartas(new List<Carta>());
            }
        }
    }
}

static class RandomExtensions
{
    public static void Shuffle<T>(this Random rng, T[] array)
    {
        
        int n = array.Length;
        while (n > 1)
        {
            int k = rng.Next(n--);
            T temp = array[n];
            array[n] = array[k];
            array[k] = temp;
        }
    }
}