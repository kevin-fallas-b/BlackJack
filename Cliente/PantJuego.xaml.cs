using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Animation;

namespace Cliente
{
    /// <summary>
    /// Interaction logic for PantJuego.xaml
    /// </summary>
    public partial class PantJuego : Window
    {
        
        string[] letters = new string[] { "C", "T", "P", "D" };
        string[] values = new string[] { "2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K", "A" };


        int[] positionsX = new int[] { -750, -670, -510, -270, -30, 130, 200 };
        int[] positionsY = new int[] { 120, 240, 330, 350, 330, 240, 110 };

        public PantJuego()
        {
            InitializeComponent();
            //aqui ocupa llegar por parametro mi nombre y demas datos de usuario
        }

        public static void MoveTo(Image target, double newX, double newY)
        {
            Vector offset = VisualTreeHelper.GetOffset(target);
            var top = offset.Y;
            var left = offset.X;
            TranslateTransform trans = new TranslateTransform();
            target.RenderTransform = trans;
            DoubleAnimation anim1 = new DoubleAnimation(0, newY - top, TimeSpan.FromSeconds(2));
            DoubleAnimation anim2 = new DoubleAnimation(0, newX - left, TimeSpan.FromSeconds(2));
            trans.BeginAnimation(TranslateTransform.YProperty, anim1);
            trans.BeginAnimation(TranslateTransform.XProperty, anim2);
        }

        private void PedirCarta(object sender, RoutedEventArgs e)
        {

            repartirCartas();
        }

        private void repartirCartas()
        {
            int x = 0;

            for (int j = 0; j < 7; j++)
            {
                Random rnd = new Random();
                int first = rnd.Next(0, 4);
                int second = rnd.Next(0, 13);

                Image myImage3 = new Image();
                myImage3.Source = new BitmapImage(new Uri(System.Environment.CurrentDirectory + "../../../Resources/" + letters[first] + values[second] + ".jpg"));

                myImage3.MaxWidth = 55;
                myImage3.MaxHeight = 80;
                myImage3.MinWidth = 55;
                myImage3.MinHeight = 80;
                myImage3.Stretch = Stretch.Fill;
                myImage3.Margin = new Thickness(530, -480, 0, 0);


                Ventana.Children.Add(myImage3);

                MoveTo(myImage3, positionsX[j] + x, positionsY[j]);
                System.Threading.Thread.Sleep(1);

            }
            x += 20;
            for (int j = 0; j < 7; j++)
            {
                Random rnd = new Random();
                int first = rnd.Next(0, 4);
                int second = rnd.Next(0, 13);

                Image myImage3 = new Image();
                myImage3.Source = new BitmapImage(new Uri(System.Environment.CurrentDirectory + "../../../Resources/" + letters[first] + values[second] + ".jpg"));

                myImage3.MaxWidth = 55;
                myImage3.MaxHeight = 80;
                myImage3.MinWidth = 55;
                myImage3.MinHeight = 80;
                myImage3.Stretch = Stretch.Fill;
                myImage3.Margin = new Thickness(530, -480, 0, 0);


                Ventana.Children.Add(myImage3);

                MoveTo(myImage3, positionsX[j] + x, positionsY[j]);
                System.Threading.Thread.Sleep(1);

            }
        }
    }
}
