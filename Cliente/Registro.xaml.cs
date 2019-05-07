using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Cliente
{
    /// <summary>
    /// Interaction logic for Registro.xaml
    /// </summary>
    public partial class Registro : Page
    {
        String usuario = null;
        String contrasenna = null;
        String ccontrasenna = null;

        public Registro()
        {
            InitializeComponent();
        }

        private void BotRegis_Click(object sender, RoutedEventArgs e)
        {
            usuario = txtUsuario.GetLineText(0);
            contrasenna = txtContrasenna.GetLineText(0);
            ccontrasenna = txtConfContrasenna.GetLineText(0);
            if (contrasenna != ccontrasenna)
            {
                MessageBox.Show("Las contraseñas no coinciden");
            }
            else
            {
                if (validarpass(txtContrasenna.GetLineText(0)))
                {
                    string mensaje = "R:" + usuario + "/" + contrasenna + "^";
                    Control.Conexion.EnviarMensaje(mensaje);
                }
            }
        }

        private bool validarpass(string password)
        {
            bool letra = false;
            bool mayuscula = false;
            bool minuscula = false;
            bool numero = false;
            bool simbolo = false;

            char[] caracter = password.ToCharArray();

            if (caracter.Length < 7)
            {
                MessageBox.Show("Tamaño incorrecto, debe tener como minimo 7 caracteres.");
                return false;
            }

            for (int i = 0; i < caracter.Length; i++)
            {
                if (char.IsLetter(caracter[i]))
                {
                    letra = true;
                    if (char.IsLower(caracter[i]))
                    {
                        minuscula = true;
                    }
                    else if (char.IsUpper(caracter[i]))
                    {
                        mayuscula = true;
                    }
                }
                else if (char.IsNumber(caracter[i]))
                {
                    numero = true;
                }
                else if (char.IsSymbol(caracter[i]) || char.IsPunctuation(caracter[i]))
                {
                    simbolo = true;
                }
            }

            string respuesta = "La contraseña aun debe contener:";
            bool error = false;

            if (letra == false)
            {
                error = true;
                respuesta += " letras,";
            }
            if (mayuscula == false)
            {
                error = true;
                respuesta += " mayusculas,";
            }
            if (minuscula == false)
            {
                error = true;
                respuesta += " minusculas,";
            }
            if (numero == false)
            {
                error = true;
                respuesta += " numeros,";
            }
            if (simbolo == false)
            {
                error = true;
                respuesta += " simbolos,";
            }

            if (!error)
            {
                return true;
            }
            else
            {
                respuesta = respuesta.Remove(respuesta.Length - 1);
                respuesta += ".";
                MessageBox.Show(respuesta);
                return false;
            }
        }

        private void txtConfContrasenna_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                usuario = txtUsuario.GetLineText(0);
                contrasenna = txtContrasenna.GetLineText(0);
                ccontrasenna = txtConfContrasenna.GetLineText(0);
                if (contrasenna != ccontrasenna)
                {
                    MessageBox.Show("Las contraseñas no coinciden");
                }
                else
                {
                    if (validarpass(txtContrasenna.GetLineText(0)))
                    {
                        MessageBox.Show("Resgistro posible");
                        string mensaje = "R:" + usuario + "/" + contrasenna + "^";
                        Control.Conexion.EnviarMensaje(mensaje);
                    }
                }
            }
        }
        private void TxtUsuario_GotFocus(object sender, RoutedEventArgs e)
        {
            if (txtUsuario.GetLineText(0) == "Usuario")
            {
                txtUsuario.Clear();
            }
        }

        private void TxtContrasenna_GotFocus(object sender, RoutedEventArgs e)
        {
            if (txtContrasenna.GetLineText(0) == "Contraseña")
            {
                txtContrasenna.Clear();
            }
        }

        private void TxtConfContrasenna_GotFocus(object sender, RoutedEventArgs e)
        {
            if (txtConfContrasenna.GetLineText(0) == "Confirmar Contraseña")
            {
                txtConfContrasenna.Clear();
            }
        }

    }
}
