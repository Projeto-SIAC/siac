namespace SIAC.Models
{
    public partial class Pessoa
    {
        private static dbSIACEntities contexto => Repositorio.GetInstance();

        public static int Inserir(Pessoa pessoa)
        {
            contexto.Pessoa.Add(pessoa);
            contexto.SaveChanges();
            return pessoa.CodPessoa;
        }
    }
}