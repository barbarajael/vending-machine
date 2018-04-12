using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Media.Imaging;

namespace ProjectIHC.Models
{

    class MedicamentoModel 
    {
        public string ID { get; set; }
        public string Nome { get; set; }
        public string Preco { get; set; }
        public string Descricao { get; set; }
        public string Imagem { get; set; }

        public MedicamentoModel() { }

        public MedicamentoModel(string id, string nome, string preco, string descricao, string imagem)
        {
            ID = id;
            Nome = nome;
            Preco = preco;
            Descricao = descricao;
            Imagem = imagem;
        }

        public MedicamentoModel(string id, string nome, string preco, string descricao)
        {
            ID = id;
            Nome = nome;
            Preco = preco;
            Descricao = descricao;
            Imagem = null;
        }
    }
}
