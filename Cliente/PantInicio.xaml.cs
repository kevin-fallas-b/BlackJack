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

        private Conexion conexion = Conexion.Instance;
        public PantInicio()
        {
            InitializeComponent();
        }

        private void BotIngresar_Click(object sender, RoutedEventArgs e)
        {
            conexion.Conectar(tbdireccionIP.GetLineText(0));
        }

        private void BotLimpiar_Click(object sender, RoutedEventArgs e)
        {
            tbdireccionIP.Clear();
        }

        private void BotFinConex_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void BotEnviarMen_Click(object sender, RoutedEventArgs e)
        {
            //EnviarMensajeAControlador(tbMensaje.GetLineText(0));
        }

        private void CleanIpText(object sender, RoutedEventArgs e)
        {
            tbdireccionIP.Clear();
        }


        private void FillIPText(object sender, RoutedEventArgs e)
        {
           
        }
    }
}
