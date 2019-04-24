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
using System.Windows.Shapes;

namespace Cliente
{
    /// <summary>
    /// Interaction logic for PantUsuario.xaml
    /// </summary>
    public partial class PantUsuario : Window
    {
        public PantUsuario()
        {
            InitializeComponent();
            main.Content = new Login();
        }

        private void Registro_Click(object sender, RoutedEventArgs e)
        {
            main.Content = new Registro();
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            main.Content = new Login();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }

        private void Cancelar_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
