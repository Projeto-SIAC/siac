namespace SIAC.Models
{
    public partial class Pessoa
    {
        public const string FISICA = "F";
        public const string JURIDICA = "J";

        private static Contexto contexto => Repositorio.GetInstance();

        public static int Inserir(Pessoa pessoa)
        {
            contexto.Pessoa.Add(pessoa);
            contexto.SaveChanges();
            return pessoa.CodPessoa;
        }
    }
}