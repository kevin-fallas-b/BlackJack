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

    }
}
