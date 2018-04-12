using ProjectIHC.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace ProjectIHC.Windows
{
    public partial class ListaMedicamentos : Window
    {
        private Boolean fullscreen = true; // The application will start in fullscreen mode
        DispatcherTimer timer = null;   //para o timer funcionar corretamente
        private Boolean SubCategory;
        private string category;
        private List<MedicamentoModel> list;

        public ListaMedicamentos(string categoryName, Boolean subcategory)
        {
            InitializeComponent();
            SubCategory = subcategory;
            category = categoryName;

            XmlNodeList nodes = GetNodesFromXML();
            ImageListbox.ItemsSource = GetListOfMedicines(nodes);

            ObservableCollection<string> list = new ObservableCollection<string>();
            list.Add("Todos");
            list.Add("Comprimidos");
            list.Add("Saquetas");
            list.Add("Gel");
            list.Add("Pomada");
            list.Add("Supositorio");
            list.Add("Pastilha");
            list.Add("Xarope");
            list.Add("Gotas");
            list.Add("Efervescente");
            ComboBoxToma.ItemsSource = list;

            ObservableCollection<string> filtro = new ObservableCollection<string>();
            filtro.Add("Preço");
            filtro.Add("Nome");
            ComboBoxOrdenacao.ItemsSource = filtro;

        }

        private XmlNodeList GetNodesFromXML()
        {
            XmlDocument xDoc = new XmlDocument();
            // Load Xml
            xDoc.Load(@"..\..\ihc_dummy_data.xml");

            if (SubCategory)
            {
                return xDoc.SelectNodes("//Medicine[@subcategory='" + category + "']");
            }

            return xDoc.SelectNodes("//Category[@name='" + category + "']/Medicine");
        }

        private List<MedicamentoModel> GetListOfMedicines(XmlNodeList nodes)
        {
            list = new List<MedicamentoModel>();

            for (int i = 0; i < nodes.Count; i++)
            {
                string name = nodes.Item(i).ChildNodes[0].InnerText;
                string desc = nodes.Item(i).ChildNodes[1].InnerText;
                string img = nodes.Item(i).ChildNodes[2].InnerText;
                string price = nodes.Item(i).ChildNodes[3].InnerText;
                string id = nodes.Item(i).Attributes["id"].Value;
                Console.WriteLine("IMG: " + img);
                list.Add(new MedicamentoModel(id, name, price, desc, img));
            }
            return list;
        }

        private List<MedicamentoModel> SearchGenericosListOfMedicines(XmlNodeList nodes)
        {
            list = new List<MedicamentoModel>();

            for (int i = 0; i < nodes.Count; i++)
            {
                string name = nodes.Item(i).ChildNodes[0].InnerText;
                string desc = nodes.Item(i).ChildNodes[1].InnerText;
                string img = nodes.Item(i).ChildNodes[2].InnerText;
                string price = nodes.Item(i).ChildNodes[3].InnerText;
                string id = nodes.Item(i).Attributes["id"].Value;

                string generico = nodes.Item(i).ChildNodes[5].InnerText;
                if (generico.Equals("true"))
                {
                    list.Add(new MedicamentoModel(id, name, price, desc, img));
                }
            }
            return list;
        }

        private List<MedicamentoModel> SearchFiltersListOfMedicines(XmlNodeList nodes, string filtro)
        {
            list = new List<MedicamentoModel>();

            for (int i = 0; i < nodes.Count; i++)
            {
                string name = nodes.Item(i).ChildNodes[0].InnerText;
                string desc = nodes.Item(i).ChildNodes[1].InnerText;
                string img = nodes.Item(i).ChildNodes[2].InnerText;
                string price = nodes.Item(i).ChildNodes[3].InnerText;
                string id = nodes.Item(i).Attributes["id"].Value;

                string generico = nodes.Item(i).ChildNodes[4].InnerText;
                if (generico.Equals(filtro))
                {
                    list.Add(new MedicamentoModel(id, name, price, desc, img));
                }
            }
            return list;
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
            string msg = "Nesta página estão listados os medicamentos pertencentes à categoria que escolheu anteriormente.\n\nPode escolher ver só os Genéricos desta categoria, carregando no botão a verde ao lado da palavra “Genéricos:” .\nPode também escolher ver os medicamentos pelo meio de toma (comprimidos, saquetas, gel, pomada, supositório, pastilha, xarope, gotas, efervescente).\nPode ainda ordenar os medicamentos da lista alfabeticamente ou por preço.\n\nPara selecionar um medicamento basta carregar em cima do retângulo que contém o seu nome e a imagem da sua caixa.";
            MessageBoxResult result = MessageBox.Show(msg, "Ajuda", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        //funçao do botao undo
        private void Button_Click_Undo(object sender, RoutedEventArgs e)
        {
            Window win;

            if (SubCategory)
            {
                win = new TiposAnalgesicos();
            }

            else
            {
                win = new MainWindow();
            }

            win.Show();
            // depois de abrir uma nova janela, a atual deve ser fechada (mas nao abruptamente; dai o timer)
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            string id = (string)btn.Tag;
            Window win = new Medicamento(id, category, SubCategory);
            win.Show();
            //depois de abrir uma nova janela, a atual deve ser fechada (mas nao abruptamente; dai o timer)
            StartTimer();
        }

        private void ComboBoxToma_Selected(object sender, RoutedEventArgs e)
        {
            ComboBox combo = sender as ComboBox;
            string selected = (string)combo.SelectedValue;
            if (selected.Equals("Todos"))
            {
                ImageListbox.ItemsSource = GetListOfMedicines(GetNodesFromXML());
            }
            else
            {
                ImageListbox.ItemsSource = SearchFiltersListOfMedicines(GetNodesFromXML(), selected);
            }
        }

        private void ComboBoxOrdenacao_Selected(object sender, RoutedEventArgs e)
        {
            List<MedicamentoModel> list = GetListOfMedicines(GetNodesFromXML());
            ComboBox combo = sender as ComboBox;
            string selected = (string)combo.SelectedValue;
            if (selected.Equals("Preço"))
            {
                var results = list.OrderBy(x => x.Preco).ToList();
                ImageListbox.ItemsSource = results;
            }
            else
            {
                var results = list.OrderBy(x => x.Nome).ToList();
                ImageListbox.ItemsSource = results;
            }
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            // CheckBox dos genericos
            ImageListbox.ItemsSource = SearchGenericosListOfMedicines(GetNodesFromXML());
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            // CheckBox dos genericos
            ImageListbox.ItemsSource = GetListOfMedicines(GetNodesFromXML());
        }

    }
}
