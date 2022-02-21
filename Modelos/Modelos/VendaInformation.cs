using System;
using System.Collections.Generic;
using System.Text;

namespace Loja.Modelos
{
    public class VendaInformation
    {
        public int Codigo { get; private set; }
        public DateTime Data { get; private set; }
        public int Quantidade { get; private set; }
        public bool Faturado { get; private set; }
        public int CodigoCliente { get; private set; }
        public int CodigoProduto { get; private set; }
        public string NomeCliente { get; private set; }
    }
}
