using System;
using System.Net.Sockets;
using System.Windows;
using System.Net;
using System.Windows.Input;

namespace Cliente
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    
    public partial class PantInicio : Window
    {
        public PantInicio()
        {
            InitializeComponent();
            //la siguiente linea es porque WPF exige que se inicie desde un xaml. Por lo que el singleton nunca tendria acceso a pantinicio, entonces lo que hago
            // es que inicio la aplicacion en pantInicio y mando pantInicio al singleton.
            Control.Conexion.setPantInicio(this);
        }

        private void BotIngresar_Click(object sender, RoutedEventArgs e)
        {
            if (Control.Conexion.Conectar(tbdireccionIP.GetLineText(0)))
            {
                Control.pantUsuario.ShowDialog();
            }
        }

        private void tbdireccionIP_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (Control.Conexion.Conectar(tbdireccionIP.GetLineText(0)))
                {
                    Control.pantUsuario.ShowDialog();
                }
            }
        }


        private void BotLimpiar_Click(object sender, RoutedEventArgs e)
        {
            if(tbdireccionIP.GetLineText(0) == "Direccion IP")
            {
                tbdireccionIP.Clear();
            }
            
        }

        private void CleanIpText(object sender, RoutedEventArgs e)
        {
            tbdireccionIP.Clear();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }

        private void BotAcercaDe_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
