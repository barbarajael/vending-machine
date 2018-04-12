using ProjectIHC.Models;
using ProjectIHC.Windows;
using System;
using System.Collections;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Xml;
using System.ComponentModel;

namespace ProjectIHC
{
    public partial class MainWindow : Window
    {
        private Boolean fullscreen = true; // The application will start in fullscreen mode
        DispatcherTimer timer = null;   //para o timer funcionar corretamente

        public MainWindow()
        {
            InitializeComponent();

            //poe os medicamentos da lista por ordem alfabetica
            ListBoxMeds.Items.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));
        }

        //funçao de definiçao de atalhos de teclas
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
                case "analgesicos":
                    win = new TiposAnalgesicos();
                    win.Show();
                    //depois de abrir uma nova janela, a atual deve ser fechada (mas nao abruptamente; dai o timer)
                    StartTimer();
                    break;

                case "antiinflamatorios":
                    win = new ListaMedicamentos("Antiinflamatórios", false);
                    win.Show();
                    //depois de abrir uma nova janela, a atual deve ser fechada (mas nao abruptamente; dai o timer)
                    StartTimer();

                    break;

                case "azia":
                    win = new ListaMedicamentos("Azia/Indigestão", false);
                    win.Show();
                    //depois de abrir uma nova janela, a atual deve ser fechada (mas nao abruptamente; dai o timer)
                    StartTimer();
                    break;

                case "calmantes":
                    win = new ListaMedicamentos("Calmantes", false);
                    win.Show();
                    //depois de abrir uma nova janela, a atual deve ser fechada (mas nao abruptamente; dai o timer)
                    StartTimer();
                    break;

                case "fraldas":
                    win = new ListaMedicamentos("Fraldas", false);
                    win.Show();
                    //depois de abrir uma nova janela, a atual deve ser fechada (mas nao abruptamente; dai o timer)
                    StartTimer();
                    break;

                case "gripe":
                    win = new ListaMedicamentos("Gripe/Tosse", false);
                    win.Show();
                    //depois de abrir uma nova janela, a atual deve ser fechada (mas nao abruptamente; dai o timer)
                    StartTimer();
                    break;

                case "pensos":
                    win = new ListaMedicamentos("Pensos", false);
                    win.Show();
                    //depois de abrir uma nova janela, a atual deve ser fechada (mas nao abruptamente; dai o timer)
                    StartTimer();
                    break;

                case "preservativos":
                    win = new ListaMedicamentos("Preservativos", false);
                    win.Show();
                    //depois de abrir uma nova janela, a atual deve ser fechada (mas nao abruptamente; dai o timer)
                    StartTimer();
                    break;
            }
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


        //funçao do botao help
        private void Button_Click_Help(object sender, RoutedEventArgs e)
        {
            string msg = "Bem vindo!\nNeste quiosque estão à venda 45 medicamentos não sujeitos a receita médica.\n\nSe procura um medicamento que resolva o seu problema, mas não tem nenhum em mente ou não se recorda do nome, escolha a categoria que se encaixa mais na sua situação. A tabela de categorias encontra-se nesta página à esquerda.\nPara selecionar uma categoria basta carregar no botão correspondente.\n\nSe já sabe o nome do medicamento que pretende adquirir, pode procurá-lo na lista que se encontra nesta página à direita. A lista está ordenada alfabeticamente.\nPara selecionar um medicamento basta carregar em cima do retângulo que contém o seu nome e a imagem da sua caixa.\n\nObrigado!";
            MessageBoxResult result = MessageBox.Show(msg, "Ajuda", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        //abre numa nova janela o medicamento selecionado na lista de todos os medicamentos
        private void MedSelected_Click(object sender, RoutedEventArgs e)
        {
            Button target = (Button)sender;
            XmlAttribute att = (XmlAttribute)target.Tag;

            Window win = new Medicamento(att.Value, "", false);
            win.Show();
            //depois de abrir uma nova janela, a atual deve ser fechada (mas nao abruptamente; dai o timer)
            StartTimer();
        }

    }
}