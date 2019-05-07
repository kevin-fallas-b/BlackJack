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
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Page
    {
        String usuario = null;
        String contrasenna = null;

        public Login()
        {
            InitializeComponent();
        }

        private void BotLogin_Click(object sender, RoutedEventArgs e)
        {
            if ((txtUsuario.GetLineText(0) != "") && (txtContrasenna.Password != ""))
            {
                usuario = txtUsuario.GetLineText(0);
                contrasenna = txtContrasenna.Password;
                Control.Conexion.EnviarMensaje("L:" + usuario + "/" + contrasenna + "^");
            }
            else
            {
                MessageBox.Show("Debe de completar los espacios.");
            }
        }

        private void TxtUsuario_GotFocus(object sender, RoutedEventArgs e)
        {
            if(txtUsuario.GetLineText(0) == "Usuario")
            {
                txtUsuario.Clear();
            }
        }

        private void TxtContrasenna_GotFocus(object sender, RoutedEventArgs e)
        {

        }

        private void txtContrasenna_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if ((txtUsuario.GetLineText(0) != "") && (txtContrasenna.Password != ""))
                {
                    usuario = txtUsuario.GetLineText(0);
                    contrasenna = txtContrasenna.Password;
                    Control.Conexion.EnviarMensaje("L:" + usuario + "/" + contrasenna + "^");
                }
                else
                {
                    MessageBox.Show("Debe de completar los espacios.");
                }
            }
        }
    }
}
