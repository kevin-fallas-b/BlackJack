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
            usuario = txtUsuario.GetLineText(0);
            contrasenna = txtContrasenna.GetLineText(0);
        }

       
    }
}
