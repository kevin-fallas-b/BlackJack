using System;
using System.Net.Sockets;
using System.Windows;


namespace Cliente
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    
    public partial class PantInicio : Window
    {
        System.Net.Sockets.TcpClient cliente = new System.Net.Sockets.TcpClient();
        Int32 puerto = 13000;
        string direccionIp = null;
        Boolean conectado = false;
        
        public PantInicio()
        {
            InitializeComponent();
        }

        private void Conectar()
        {
            try
            {
                
                cliente = new TcpClient(direccionIp, puerto);
                conectado = true;
                PantUsuario pantUsuario = new PantUsuario();
                pantUsuario.ShowDialog();
            }
            catch (SocketException e)
            {
                MessageBox.Show("No se encontro un servidor con dicha direccion IP");
                Console.WriteLine("Se cayo esta pecha\nSocketException: {0}", e);
            }
        }
        private void TerminarConexion()
        {
            conectado = false;
            cliente.Close();
        }

        private void EnviarMensajeAControlador(string mensaje)
        {
            if (conectado)
            {
                //pasar el mensaje de string a bytes
                Byte[] data = System.Text.Encoding.ASCII.GetBytes(mensaje);
                NetworkStream stream = cliente.GetStream();
                stream.Write(data, 0, data.Length);
                //listo el envio, ahora recibir respuesta del servidor
                data = new Byte[256];
                String respuesta = String.Empty;
                Int32 bytes = stream.Read(data, 0, data.Length);
                respuesta = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
                Console.WriteLine("Recibido: {0}", respuesta);
            }
            else
            {
                Console.WriteLine("No se puede enviar mensaje. No existe conexion.");
            }
        }
        private void BotIngresar_Click(object sender, RoutedEventArgs e)
        {
            Conectar();
        }

        private void BotLimpiar_Click(object sender, RoutedEventArgs e)
        {
            tbdireccionIP.Clear();
        }

        private void BotFinConex_Click(object sender, RoutedEventArgs e)
        {
            TerminarConexion();
        }

        private void BotEnviarMen_Click(object sender, RoutedEventArgs e)
        {
            EnviarMensajeAControlador(tbMensaje.GetLineText(0));
        }

        private void CleanIpText(object sender, RoutedEventArgs e)
        {
            direccionIp = tbdireccionIP.GetLineText(0);
            tbdireccionIP.Clear();
        }


        private void FillIPText(object sender, RoutedEventArgs e)
        {
            tbdireccionIP.Text = "Dirrecion IP";
        }
    }
}
