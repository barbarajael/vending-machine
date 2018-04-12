using ProjectIHC.Windows;
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
using System.Windows.Threading;

namespace ProjectIHC
{
    public partial class TiposAnalgesicos : Window
    {
        private Boolean fullscreen = true; // The application will start in fullscreen mode
        DispatcherTimer timer = null;   //para o timer funcionar corretamente

        public TiposAnalgesicos()
        {
            InitializeComponent();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F11)
            {
                // Switch fullscreen mode or normal mode
                if (!fullscreen)
                {
                    WindowStyle = WindowStyle.None;
                    WindowState = WindowState.Maximized;
                    ResizeMode = ResizeMode.NoResize;
                    fullscreen = true;
                }
                else
                {
                    WindowStyle = WindowStyle.SingleBorderWindow;
                    WindowState = WindowState.Normal;
                    ResizeMode = ResizeMode.NoResize;
                    fullscreen = false;
                }
            }
            else if (e.Key == Key.Q)
            {
                // Close the application
                Close();
            }
        }

        //funçao para determinar açao dos botoes das categorias
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string tag = (string)((Button)sender).Tag;
            Window win;

            switch (tag)
            {
                case "cabeca":
                    win = new ListaMedicamentos("Dores de cabeça", true);
                    win.Show();
                    //depois de abrir uma nova janela, a atual deve ser fechada (mas nao abruptamente; dai o timer)
                    StartTimer();
                    break;

                case "ouvidos":
                    win = new ListaMedicamentos("Dores de ouvidos", true);
                    win.Show();
                    //depois de abrir uma nova janela, a atual deve ser fechada (mas nao abruptamente; dai o timer)
                    StartTimer();
                    break;

                case "costas":
                    win = new ListaMedicamentos("Dores de costas", true);
                    win.Show();
                    //depois de abrir uma nova janela, a atual deve ser fechada (mas nao abruptamente; dai o timer)
                    StartTimer();
                    break;

                case "joelhos":
                    win = new ListaMedicamentos("Dores nos joelhos", true);
                    win.Show();
                    //depois de abrir uma nova janela, a atual deve ser fechada (mas nao abruptamente; dai o timer)
                    StartTimer();
                    break;

                case "ossos":
                    win = new ListaMedicamentos("Dores de ossos", true);
                    win.Show();
                    //depois de abrir uma nova janela, a atual deve ser fechada (mas nao abruptamente; dai o timer)
                    StartTimer();
                    break;

                case "menstruais":
                    win = new ListaMedicamentos("Dores menstruais", true);
                    win.Show();
                    //depois de abrir uma nova janela, a atual deve ser fechada (mas nao abruptamente; dai o timer)
                    StartTimer();
                    break;

                case "queimaduras":
                    win = new ListaMedicamentos("Queimaduras/Feridas", true);
                    win.Show();
                    //depois de abrir uma nova janela, a atual deve ser fechada (mas nao abruptamente; dai o timer)
                    StartTimer();
                    break;
            }
        }

        //funçao do botao help
        private void Button_Click_Help(object sender, RoutedEventArgs e)
        {
            string msg = "Chegou à secção dos Analgésicos.\nEscolha a opção que mais se enquadra no seu tipo de dor. Para selecionar a opção basta carregar no botão correspondente.";
            MessageBoxResult result = MessageBox.Show(msg, "Ajuda", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        //funçao do botao undo
        private void Button_Click_Undo(object sender, RoutedEventArgs e)
        {
            Window win = new MainWindow();
            win.Show();
            //depois de abrir uma nova janela, a atual deve ser fechada (mas nao abruptamente; dai o timer)
            StartTimer();
        }

        //funçao do botao home
        private void Button_Click_Home(object sender, RoutedEventArgs e)
        {
            Window win = new MainWindow();
            win.Show();
            //depois de abrir uma nova janela, a atual deve ser fechada (mas nao abruptamente; dai o timer)
            StartTimer();
        }

        ////////////////////////////////////
        private void StartTimer()
        {
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(0.5);
            timer.Tick += new EventHandler(timer_Elapsed);
            timer.Start();
        }

        private void timer_Elapsed(object sender, EventArgs e)
        {
            timer.Stop();
            this.Close();
        }
        ////////////////////////////////////
    }
}
