using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controlador
{
    class Carta
    {
        private string nombre;
        private int valor;

        public Carta(string nom,string val)
        {
            nombre = nom;
            if(val == "J" || val == "Q" || val == "K")
            {
                valor = 10;
            }
            else
            {
                if (val == "A")
                {
                    valor = 1;
                }
                else
                {
                    valor = int.Parse(val);
                }

            }
            
            
        }
        public void setNombre(string nomb)
        {
            nombre = nomb;
        }

        public string getNombre()
        {
            return nombre;
        }
        public void setValor(int val)
        {
            valor = val;
        }
        public int getValor()
        {
            return valor;
        }
    }
}
