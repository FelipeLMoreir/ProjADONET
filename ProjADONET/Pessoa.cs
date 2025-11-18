using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjADONET
{
    internal class Pessoa
    {
        public int Id { get; private set; }
        public string Nome { get; private set; }
        public string CPF { get; private set; }
        public DateOnly DataNascimento { get; private set; }
        public List<Telefone> Telefones { get; private set; } = new List<Telefone>();

        public Pessoa(string nome, string cpf, DateOnly dataNascimento)
        {
            Nome = nome;
            CPF = cpf;
            DataNascimento = dataNascimento;
        }

        public void SetId(int id)
        {
            Id = id;
        }

        public override string ToString()
        {
            return $"Id: {Id}\nNome: {Nome}\nCPF: {CPF}\nData de Nascimento: {DataNascimento}";
        }
    }
}
