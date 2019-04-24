using System;
using System.Net.Sockets;
using System.Windows;
using System.Net;


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
            Control.Conexion.setPantInicio(this);
        }

        private void BotIngresar_Click(object sender, RoutedEventArgs e)
        {
            if (Control.Conexion.Conectar(tbdireccionIP.GetLineText(0)))
            {
                Control.pantUsuario.ShowDialog();
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
            Control.Conexion.TerminarConexion();
        }

        private void BotAcercaDe_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
