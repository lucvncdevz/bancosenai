using BancoSENAIAPI.Models;

namespace BancoSENAIAPI.DB
{
    public class Banco
    {
        public static List<Carteira> _carteira = new List<Carteira>
        {
            new Carteira
            {
                NumeroCarteira = 101,
                NomeCarteira = "Agro",
                ApetiteCarteira = 10000
            },

            new Carteira
            {
                NumeroCarteira = 102,
                NomeCarteira = "Tech",
                ApetiteCarteira = 10000
            },

            new Carteira
            {
                NumeroCarteira = 103,
                NomeCarteira = "Pop",
                ApetiteCarteira = 10000
            }
        };


        public static List<Agencia> _agencias = new List<Agencia>
        {
            new Agencia
            {
                NumeroAgencia = 1001,
                Cidade = "Aracaju",
                SiglaEstado = "SE"
            },

            new Agencia
            {
                NumeroAgencia = 2002,
                Cidade = "São Paulo",
                SiglaEstado = "SP"
            },

            new Agencia
            {
                NumeroAgencia = 3003,
                Cidade = "Salvador",
                SiglaEstado = "BA"
            }
        };

        public static List<Client> _cliente = new List<Client>
        {
            new Client
            {
                CodigoClient = 1,
                NomeClient = "Felipe",
                CPF = "00900900900",
                Sexo = "M",
                Enderco = "Rua A",
                Cidade = "Aracaju",
                Estado = "SE",
                SaldoTotal = 1000,
                NumeroAgencia = 1001
            },

            new Client
            {
                CodigoClient = 2,
                NomeClient = "Caio",
                CPF = "00800800800",
                Sexo = "M",
                Enderco = "Rua B",
                Cidade = "Aracaju",
                Estado = "SE",
                SaldoTotal = 1000,
                NumeroAgencia = 2002
            },

            new Client
            {
                CodigoClient = 3,
                NomeClient = "Paulo",
                CPF = "00700700700",
                Sexo = "M",
                Enderco = "Rua C",
                Cidade = "Aracaju",
                Estado = "SE",
                SaldoTotal = 1000,
                NumeroAgencia = 3003
            }
        };

        public static List<DocumentoMetadados> _document =
            new List<DocumentoMetadados>();

    }
}
