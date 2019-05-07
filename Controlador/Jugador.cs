using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controlador
{
    class Jugador
    {
        private string usuario;
        private string ip;
        private bool conectado;
        private int plata;
        private List<Carta> cartasEnMano = new List<Carta>();

        public Jugador(string nombre, string ipp, int plataa)
        {
            usuario = nombre;
            ip = ipp;
            conectado = false;
            plata = plataa;
        }
        public Jugador(string i)
        {
            string ip = i;
            conectado = true;
        }
        public void setUsuario(string u)
        {
            usuario = u;
        }
        public void setIp(string i)
        {
            ip = i;
        }
        public void setConectado(bool c)
        {
            conectado = c;
        }
        public string getUsuario()
        {
            return usuario;
        }
        public string getIp()
        {
            return ip;
        }
        public bool isConectado()
        {
            return conectado;
        }
        public int getPlata()
        {
            return plata;
        }
        public void setPlata(int pla)
        {
            plata = pla;
        }
        public void setCartas(List<Carta> cartas)
        {
            cartasEnMano = cartas;
        }
        public void setCartas(Carta car)
        {
            cartasEnMano.Add(car);
        }
        public List<Carta> getCartasEnMano()
        {
            return cartasEnMano;
        }
    }
}
