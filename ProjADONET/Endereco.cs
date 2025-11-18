using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjADONET
{
    public class Endereco
    {
        public int Id { get; private set; }
        public string Logradouro { get; private set; }
        public string Numero { get; private set; }
        public string Complemento { get; private set; }
        public string Bairro { get; private set; }
        public string Cidade { get; private set; }
        public string Estado { get; private set; }
        public string Cep { get; private set; }
        public int PessoaId { get; private set; }

        public Endereco(string logradouro, string numero, string complemento, string bairro, 
            string cidade, string estado, string cep, int pessoaId)
        {
            Logradouro = logradouro;
            Numero = numero;
            Complemento = complemento;
            Bairro = bairro;
            Cidade = cidade;
            Estado = estado;
            Cep = cep;
            PessoaId = pessoaId;
        }

        public void SetId(int id)
        {
            Id = id;
        }

        public override string ToString()
        {
            return $"Rua:{Logradouro}\nNúmero:{Numero}\nComplemento:{Complemento}\n" +
                $"Bairro:{Bairro}\nCidade:{Cidade}\nEstado:{Estado}CEP:{Cep}";
        }
    }
}
