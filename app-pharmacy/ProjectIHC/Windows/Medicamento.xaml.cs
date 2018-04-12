using ProjectIHC.Models;
using System;
using System.IO;
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
using System.Xml;
using System.Xml.Linq;
using Microsoft.Win32;
using System.Windows.Xps.Packaging;
using ProjectIHC.Windows;
using System.Reflection;


namespace ProjectIHC
{
    public partial class Medicamento : Window
    {
        private Boolean fullscreen = true; // The application will start in fullscreen mode
        private DispatcherTimer timer = null;   //para o timer funcionar corretamente
        private Boolean SubCategory;
        private string category;
        private string ID;

        public Medicamento(string mID, string categoryName, Boolean subcategory)
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

            MedTitle.Content = nodes.Item(0).ChildNodes[0].InnerText;
            string UrlFolheto = nodes.Item(0).ChildNodes[1].InnerText;
            string image = nodes.Item(0).ChildNodes[2].InnerText;
            MedImage.Source = SetImageView(image);
            MedPrice.Content = nodes.Item(0).ChildNodes[3].InnerText;


            // link do pdf do site do infarmed xD
            //DocumentViewer.Navigate("http://www.infarmed.pt/infomed/download_ficheiro.php?med_id=639&tipo_doc=fi");
            DocumentViewer.Navigate(System.IO.Path.GetFullPath(UrlFolheto));
        }

        private BitmapImage SetImageView(string url)
        {
            BitmapImage bi3 = new BitmapImage();
            bi3.BeginInit();
            bi3.UriSource = new Uri(url, UriKind.Relative);
            bi3.EndInit();
            return bi3;
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


        //funçao do botao help
        private void Button_Click_Help(object sender, RoutedEventArgs e)
        {
            string msg = "Selecionou um medicamento.\n\nPode consultar o folheto informativo deste medicamento do lado direito desta página. Leia atentamente as instruções e precauções, em especial se não conhecer o medicamento.\nSe tiver dúvidas quanto ao modo de atuação deste medicamento e se a farmácia ainda se encontrar aberta, não hesite em pedir ajuda ao seu farmacêutico.\n\nPode proceder à compra deste medicamento por carregar no botão que se encontra na parte esquerda inferior desta página.";
            MessageBoxResult result = MessageBox.Show(msg, "Ajuda", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        //funçao do botao undo
        private void Button_Click_Undo(object sender, RoutedEventArgs e)
        {
            Window win;

            if (category.Equals(""))
            {
                // o medicamento vem da home
                win = new MainWindow();
            }

            else
            {
                // o medicamento vem da lista de medicamentos
                win = new ListaMedicamentos(category, SubCategory);
            }

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


        //funçao do botao comprar
        private void Button_Click_Buy(object sender, RoutedEventArgs e)
        {
            Window win = new Pagamento(ID, category, SubCategory);
            win.Show();
            //depois de abrir uma nova janela, a atual deve ser fechada (mas nao abruptamente; dai o timer)
            StartTimer();
        }
    }
}
