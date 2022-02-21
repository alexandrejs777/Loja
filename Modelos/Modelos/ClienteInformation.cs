using System;

namespace Loja.Modelos
{
    public class ClienteInformation
    {
        public int Codigo { get; set; }
        public string Nome { get; private set; }

        public string Email { get; private set; }

        public string Telefone { get; private set; }
    }
}
