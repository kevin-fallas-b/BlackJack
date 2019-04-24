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
                MessageBox.Show("Resgistro posible");
                string mensaje = "R:" + usuario + "/" + contrasenna + "^";
                Control.Conexion.EnviarMensaje(mensaje);
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
            if(txtConfContrasenna.GetLineText(0)== "Confirmar Contraseña")
            {
                txtConfContrasenna.Clear();
            }
        }
    }
}
