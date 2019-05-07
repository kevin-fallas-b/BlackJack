using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Animation;
using System.Collections.Generic;
using System.Windows.Input;
using System.Threading;
using System.Windows.Threading;

namespace Cliente
{
    /// <summary>
    /// Interaction logic for PantJuego.xaml
    /// </summary>
    public partial class PantJuego : Window
    {
        static int cont = 30;

        static int contJ = 0;

        static string[] letters = new string[] { "C", "T", "P", "D" };
        static string[] values = new string[] { "2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K", "A" };
        static string[] cartas = new string[52];
        
        static bool[] readyPlayers = new bool[] { false, false, false, false, false, false, false };
        
        static int[] conts = new int[] { 0, 0, 0, 0, 0, 0, 0 };

        static string[] cartasR = new string[16];
        
        static int[] positionsX = new int[] { 200, 130,-30 , -270, -510,-670 , -750 };
        static int[] positionsY = new int[] { 110, 240, 330, 350, 330, 240, 120 };
        
        //apuestas
        static List<Image> apuestasImg = new List<Image>();
        
        static int turno = 1;
        //static int[] posXA = new int[] { 340, 300, 210, -10, -240, -310, -375 };
        //static int[] posYA = new int[] { -140, -40, 30, 40, 30, -50, -140 };
        static int[] posXA = new int[] { 340, 280, 210, -10, -240, -310, -350 };
        static int[] posYA = new int[] { -140, -50, 30, 50, 30, -60, -140 };

        
            DispatcherTimer t ;
       

       // DispatcherTimer tj = new DispatcherTimer();
        static int turnoJ = -1;
        static bool partidaTermina = false;



        public PantJuego()
        {

            InitializeComponent();
            Application.Current.Dispatcher.Invoke((Action)delegate{
                DeshabilitarBotones();
                lblApostar.Content = "Tiempo para apostar: ";
            });

            //int contC=0;
            /*for(int i = 0; i < 4; i++)
            {
                for(int j = 0; j < 13; j++)
                {
                    cartas[contC] = letters[i] + values[j];
                    contC++;
                }
            }*/

            /* for(int i = 0; i < 14; i++)
             {
                 Random rnd = new Random();
                 int second = rnd.Next(0, 13);
                 cartasR[i] = cartas[second];
                 Thread.Sleep(1);
             }*/

            //aqui ocupa llegar por parametro mi nombre y demas datos de usuario



        }
        
        public void iniciarTiempoApuestas()
        {
            
                t = new DispatcherTimer();
                t.Interval = TimeSpan.FromSeconds(1);
                t.Tick += tiempoApuestas;
                t.Start();
        }
        

        public void empiezaPartida()
        {
            lblApostar.Content = "Tiempo para jugar: ";
            //turnoJ = jugadorSiguiente();
            /*tj.Interval = TimeSpan.FromSeconds(1);
            tj.Tick += tiempoJugador;
            tj.Start();*/

        }
        
        public int jugadorSiguiente()
        {
            for (int i = turnoJ + 1; i < 7; i++)
            {
                if (readyPlayers[i])
                {
                    lblJugador.Content = "jugador" + (i+1);
                    return i;
                }
            }

            return -1;
        }

        /*public void tiempoJugador(object sender, EventArgs e)
        {
            if (contJ == 0)
            {
                t.Stop();

                if (!partidaTermina)
                {
                    turnoJ = jugadorSiguiente();
                    contJ = 7;
                    tj.Start();
                }
                else
                {
                    pos.Text = "Termino.";
                }
            }
            else
            {
                contJ--;
                lblTimer.Content = "" + contJ;
            }
        }
        */
        public void DeshabilitarBotones()
        {
            btnApostar.IsEnabled = false;
            btnPedir.IsEnabled = false;
        }
        public void HabilitaBotones()
        {
            btnApostar.IsEnabled = true;
            btnPedir.IsEnabled = true;
        }

        public void tiempoApuestas(object sender, EventArgs e)
        {
            if (cont == 0)
            {
                t.Stop();
                DeshabilitarBotones();
                lblApostar.Content = "Tiempo de Jugador: ";
                cont = 30;

                Control.Conexion.EnviarMensaje("T");
                //repartirCartas();
            }
            else
            {
                HabilitaBotones();
                cont--;
                lblTimer.Content = "   : " + cont;
            }
        }

        public Image newImage(String carta)
        {
            

            Image myImage3 = new Image();
            myImage3.Source = new BitmapImage(new Uri(System.Environment.CurrentDirectory + "../../../Resources/" + carta + ".jpg"));

            myImage3.MaxWidth = 55;
            myImage3.MaxHeight = 80;
            myImage3.MinWidth = 55;
            myImage3.MinHeight = 80;
            myImage3.Stretch = Stretch.Fill;
            myImage3.Margin = new Thickness(530, -480, 0, 0);

            Ventana.Children.Add(myImage3);

            return myImage3;
        }

        public void Repartir(Image target, int i,int x)
        {
            Vector offset = VisualTreeHelper.GetOffset(target);
            var top = offset.Y;
            var left = offset.X;
            TranslateTransform trans = new TranslateTransform();
            target.RenderTransform = trans;
            DoubleAnimation anim1 = new DoubleAnimation(0, positionsY[i] - top, TimeSpan.FromSeconds(0.5));
            DoubleAnimation anim2 = new DoubleAnimation(0, (positionsX[i]+conts[i]*30) - left, TimeSpan.FromSeconds(0.5));
            conts[i] += 1;
            if (i < 7)
            {
                anim2.Completed += (s, e) =>
                {
                    i += 1;
                    while (i < 7)
                    {
                        if (readyPlayers[i])
                        {
                            break;
                        }
                        i++;
                    }
                    if (i < 7)
                    {
                        Repartir(newImage(cartasR[i]), i,x);
                    }
                    else if(x==1)
                    {
                        x += 1;
                        i = 0;
                        while (i < 7)
                        {
                            if (readyPlayers[i])
                            {
                                break;
                            }
                            i++;
                        }
                        if (i < 7)
                        {
                            Repartir(newImage(cartasR[i+7]), i, x);
                        }
                    }
                    else
                    {
                        empiezaPartida();
                    }
                };
            }
            trans.BeginAnimation(TranslateTransform.YProperty, anim1);
            trans.BeginAnimation(TranslateTransform.XProperty, anim2);
        }

        private void PedirCarta(object sender, RoutedEventArgs e)
        {
            Image target = newImage(cartas[5]);
            Vector offset = VisualTreeHelper.GetOffset(target);
            var top = offset.Y;
            var left = offset.X;
            TranslateTransform trans = new TranslateTransform();
            target.RenderTransform = trans;
            DoubleAnimation anim1 = new DoubleAnimation(0, positionsY[turnoJ] - top, TimeSpan.FromSeconds(0.5));
            DoubleAnimation anim2 = new DoubleAnimation(0, (positionsX[turnoJ] + conts[turnoJ] * 30) - left, TimeSpan.FromSeconds(0.5));
            conts[turnoJ] += 1;
          
            trans.BeginAnimation(TranslateTransform.YProperty, anim1);
            trans.BeginAnimation(TranslateTransform.XProperty, anim2);
        }
        public void nombreJugador(int pos,string nombre)
        {
            if (pos == 1) lblJugador7.Content = nombre;
            if (pos == 2) lblJugador6.Content = nombre;
            if (pos == 3) lblJugador5.Content = nombre;
            if (pos == 4) lblJugador4.Content = nombre;
            if (pos == 5) lblJugador3.Content = nombre;
            if (pos == 6) lblJugador2.Content = nombre;
            if (pos == 7) lblJugador1.Content = nombre;
        }
        private void Apostar(object sender, RoutedEventArgs e)
        {
            /*//define la posicion donde se colocaran las apuestas segun el turno del jugador
            turno = Convert.ToInt32(pos.GetLineText(0));
            turno -= 1;
            readyPlayers[turno] = true;*/
            int monto = Convert.ToInt32(monto_apostar.GetLineText(0));

            Control.Conexion.EnviarMensaje("A:"+monto+"/");
            Console.WriteLine("A:" + monto + "/");


        }
        
        
        public void animacionApostar(int pos, int monto)
        {
            int quinientos, cien, cincuenta, cinco, uno;
            cincuenta = 0;

            quinientos = monto / 500;
            monto = monto - (monto / 500) * 500;

            cien = monto / 100;
            monto = monto - (monto / 100) * 100;

            cincuenta = monto / 50;
            monto = monto - (monto / 50) * 50;
            //para que no sea una sola ficha de 50
            cincuenta += (cien / 3) * 2;
            cien = cien - (cien / 3);
            //
            cinco = monto / 5;
            monto = monto - (monto / 5) * 5;
            uno = monto / 1;
            Application.Current.Dispatcher.Invoke((Action)delegate{
                for (int i = 0; i < quinientos; i++)//quinientos
                {
                    Image img = new Image();
                    img.Source = new BitmapImage(new Uri(System.Environment.CurrentDirectory + "../../../Resources/A100.png"));

                    img.MaxWidth = 30;
                    img.MaxHeight = 30;

                    Ventana.Children.Add(img);

                    TranslateTransform trans = new TranslateTransform();
                    img.RenderTransform = trans;

                    //DoubleAnimation anim1 = new DoubleAnimation(-60, -60, TimeSpan.FromSeconds(0.8));
                    //DoubleAnimation anim2 = new DoubleAnimation(0, -(i * 3), TimeSpan.FromSeconds(0.8));
                    DoubleAnimation anim1 = new DoubleAnimation(posXA[pos]-60, posXA[pos]-60, TimeSpan.FromSeconds(1));
                    DoubleAnimation anim2 = new DoubleAnimation(posYA[pos], (posYA[pos] - 3) - (i * 2), TimeSpan.FromSeconds(1));

                    trans.BeginAnimation(TranslateTransform.XProperty, anim1);
                    trans.BeginAnimation(TranslateTransform.YProperty, anim2);

                    apuestasImg.Add(img);
                }
                    for (int i = 0; i < cien; i++)//cien
                {
                    Image img = new Image();
                    img.Source = new BitmapImage(new Uri(System.Environment.CurrentDirectory + "../../../Resources/A100.png"));

                    img.MaxWidth = 30;
                    img.MaxHeight = 30;

                    Ventana.Children.Add(img);

                    TranslateTransform trans = new TranslateTransform();
                    img.RenderTransform = trans;

                    //DoubleAnimation anim1 = new DoubleAnimation(-60, -60, TimeSpan.FromSeconds(0.8));
                    //DoubleAnimation anim2 = new DoubleAnimation(0, -(i * 3), TimeSpan.FromSeconds(0.8));
                    DoubleAnimation anim1 = new DoubleAnimation(posXA[pos], posXA[pos], TimeSpan.FromSeconds(1));
                    DoubleAnimation anim2 = new DoubleAnimation(posYA[pos], (posYA[pos] - 3) - (i * 2), TimeSpan.FromSeconds(1));

                    trans.BeginAnimation(TranslateTransform.XProperty, anim1);
                    trans.BeginAnimation(TranslateTransform.YProperty, anim2);

                    apuestasImg.Add(img);
                }
                for (int i = 0; i < cincuenta; i++)//cincuenta
                {
                    Image img = new Image();
                    img.Source = new BitmapImage(new Uri(System.Environment.CurrentDirectory + "../../../Resources/A50.png"));

                    img.MaxWidth = 30;
                    img.MaxHeight = 30;

                    Ventana.Children.Add(img);

                    TranslateTransform trans = new TranslateTransform();
                    img.RenderTransform = trans;

                    //DoubleAnimation anim1 = new DoubleAnimation(-30, -30, TimeSpan.FromSeconds(0.8));
                    //DoubleAnimation anim2 = new DoubleAnimation(0, -(i * 3), TimeSpan.FromSeconds(0.8));
                    DoubleAnimation anim1 = new DoubleAnimation(posXA[pos] - 30, posXA[pos] - 30, TimeSpan.FromSeconds(1));
                    DoubleAnimation anim2 = new DoubleAnimation(posYA[pos], (posYA[pos] - 3) - (i * 2), TimeSpan.FromSeconds(1));

                    trans.BeginAnimation(TranslateTransform.XProperty, anim1);
                    trans.BeginAnimation(TranslateTransform.YProperty, anim2);

                    apuestasImg.Add(img);
                }

                for (int i = 0; i < cinco; i++)//cinco
                {
                    Image img = new Image();
                    img.Source = new BitmapImage(new Uri(System.Environment.CurrentDirectory + "../../../Resources/A5.png"));

                    img.MaxWidth = 30;
                    img.MaxHeight = 30;

                    Ventana.Children.Add(img);

                    TranslateTransform trans = new TranslateTransform();
                    img.RenderTransform = trans;
                    //DoubleAnimation anim1 = new DoubleAnimation(0, 0), TimeSpan.FromSeconds(0.05));
                    //DoubleAnimation anim2 = new DoubleAnimation(0, -(i * 3), TimeSpan.FromSeconds(0.8));
                    DoubleAnimation anim1 = new DoubleAnimation(posXA[pos] + 30, posXA[pos] + 30, TimeSpan.FromSeconds(1));
                    DoubleAnimation anim2 = new DoubleAnimation(posYA[pos], (posYA[pos] - 3) - (i * 2), TimeSpan.FromSeconds(1));

                    trans.BeginAnimation(TranslateTransform.XProperty, anim1);
                    trans.BeginAnimation(TranslateTransform.YProperty, anim2);

                    apuestasImg.Add(img);
                }

                for (int i = 0; i < uno; i++)//uno
                {
                    Image img = new Image();
                    img.Source = new BitmapImage(new Uri(System.Environment.CurrentDirectory + "../../../Resources/A1.png"));

                    img.MaxWidth = 30;
                    img.MaxHeight = 30;

                    Ventana.Children.Add(img);

                    TranslateTransform trans = new TranslateTransform();
                    img.RenderTransform = trans;
                    //DoubleAnimation anim1 = new DoubleAnimation(30, 30, TimeSpan.FromSeconds(0.08));
                    //DoubleAnimation anim2 = new DoubleAnimation(0, -(i * 3), TimeSpan.FromSeconds(0.8));
                    DoubleAnimation anim1 = new DoubleAnimation(posXA[pos] + 60, posXA[pos] + 60, TimeSpan.FromSeconds(1));
                    DoubleAnimation anim2 = new DoubleAnimation(posYA[pos], (posYA[pos] - 3) - (i * 2), TimeSpan.FromSeconds(1));

                    trans.BeginAnimation(TranslateTransform.XProperty, anim1);
                    trans.BeginAnimation(TranslateTransform.YProperty, anim2);

                    apuestasImg.Add(img);
                }
            });

        }
        
        private void repartirCartas()
        {
            int x = 0;
            while (x < 7)
            {
                if (readyPlayers[x])
                {
                    break;
                }
                x++;
            }
            if (x < 7)
            {
                Repartir(newImage(cartasR[x]), x,1);
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Control.Conexion.TerminarConexion();
        }

        private void Terminar(object sender, RoutedEventArgs e)//elimina las imagenes de las apuestas
        {
            foreach (Image img in apuestasImg)
            {
                Ventana.Children.Remove(img);
            }
        }

       

    }
}