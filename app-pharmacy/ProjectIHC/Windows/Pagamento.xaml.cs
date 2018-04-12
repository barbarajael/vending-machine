using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;     //para o timer funcionar corretamente
using System.Xml;

namespace ProjectIHC
{
    public partial class Pagamento : Window
    {
        private Boolean fullscreen = true; // The application will start in fullscreen mode
        DispatcherTimer timer = null;   //para o timer funcionar corretamente
        private Boolean SubCategory;
        private string category;
        private string ID;

        public Pagamento(string mID, string categoryName, Boolean subcategory)
        {
            InitializeComponent();
            GetMedicamento(mID);
            SubCategory = subcategory;
            category = categoryName;
            ID = mID;
        }

        private void GetMedicamento(string id)
        {
            XmlDocument xDoc = new XmlDocument();
            // Load Xml
            xDoc.Load(@"..\..\ihc_dummy_data.xml");

            XmlNodeList nodes = xDoc.SelectNodes("//Medicine[@id='" + id + "']");

            MedName.Content = nodes.Item(0).ChildNodes[0].InnerText;
            //MedDesc.Content = nodes.Item(0).ChildNodes[1].InnerText;
            string url = nodes.Item(0).ChildNodes[2].InnerText;
            BitmapImage bi3 = new BitmapImage();
            bi3.BeginInit();
            bi3.UriSource = new Uri(url, UriKind.Relative);
            bi3.EndInit();
            MedImage.Source = bi3;
            MedPrice.Content = nodes.Item(0).ChildNodes[3].InnerText + " €";
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

        //funçao do botao undo
        private void Button_Click_Undo(object sender, RoutedEventArgs e)
        {
            new Medicamento(ID, category, SubCategory).Show();
            //depois de abrir uma nova janela, a atual deve ser fechada (mas nao abruptamente; dai o timer)
            StartTimer();
        }

        //funçao do botao help
        private void Button_Click_Help(object sender, RoutedEventArgs e)
        {
            string msg = "Agora poderá comprar o seu medicamento.\n\nSe pretender número de contribuinte na fatura, insira-o carregando nos botões numéricos. Se se enganar, basta clicar no botão Apagar.\n\nEscolha o seu método de pagamento: dinheiro, multibanco ou NFC. Para tal, deve carregar num dos botões que se encontram na base desta página.\n\nSe se enganou e não é este o medicamento que pretende, basta carregar no botão Desfazer (canto superior esquerdo na barra desta janela) ou no botão Início (botão central superior na barra desta janela).";
            MessageBoxResult result = MessageBox.Show(msg, "Ajuda", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        //funçao do botao home
        private void Button_Click_Home(object sender, RoutedEventArgs e)
        {
            Window win = new MainWindow();
            win.Show();
            //depois de abrir uma nova janela, a atual deve ser fechada (mas nao abruptamente; dai o timer)
            StartTimer();
        }

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

        private void Button_Click_End(object sender, RoutedEventArgs e)
        {
            if (NIF.Text.Length > 0 && NIF.Text.Length < 9)
            {
                MessageBox.Show("Insira um número de contribuinte válido!", "Pagamento", MessageBoxButton.OK);
            }
            else
            {
                MessageBox.Show("Pagamento efectuado!\n\nObrigado, volte sempre!", "Pagamento", MessageBoxButton.OK);
                new MainWindow().Show();
                StartTimer();
            }
        }


        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void keyboard_Click(object sender, RoutedEventArgs e)
        {
            string button = (string)(sender as Button).Content;
            if (NIF.Text.Length < 9)
            {
                switch (button)
                {
                    case "0":
                        NIF.Text += '0';
                        break;
                    case "1":
                        NIF.Text += '1';
                        break;
                    case "2":
                        NIF.Text += '2';
                        break;
                    case "3":
                        NIF.Text += '3';
                        break;
                    case "4":
                        NIF.Text += '4';
                        break;
                    case "5":
                        NIF.Text += '5';
                        break;
                    case "6":
                        NIF.Text += '6';
                        break;
                    case "7":
                        NIF.Text += '7';
                        break;
                    case "8":
                        NIF.Text += '8';
                        break;
                    case "9":
                        NIF.Text += '9';
                        break;
                }
            }
            if (button.Equals("Apagar"))
            {
                NIF.Text = "";
            }
        }
    }
}
