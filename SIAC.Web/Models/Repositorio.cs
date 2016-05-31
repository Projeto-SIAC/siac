using System.Web;

namespace SIAC.Models
{
    public class Repositorio
    {
        public static Contexto GetInstance()
        {
            if (Sistema.AlertarMudanca.Contains(Helpers.Sessao.UsuarioMatricula))
            {
                Commit();
                Dispose();
                Sistema.AlertarMudanca.Remove(Helpers.Sessao.UsuarioMatricula);
            }
            Contexto contexto = Helpers.Sessao.Retornar("dbSIACEntities") as Contexto;
            if (contexto == null)
            {
                Helpers.Sessao.Inserir("dbSIACEntities", new Contexto());
            }
            return (Contexto)Helpers.Sessao.Retornar("dbSIACEntities") ?? new Contexto();
        }

        public static void Commit()
        {
            Contexto contexto = Helpers.Sessao.Retornar("dbSIACEntities") as Contexto;
            if (contexto != null && contexto.ChangeTracker.HasChanges())
            {
                ((Contexto)Helpers.Sessao.Retornar("dbSIACEntities")).SaveChanges();
            }
        }

        public static void Dispose()
        {
            Contexto contexto = Helpers.Sessao.Retornar("dbSIACEntities") as Contexto;
            if (contexto != null)
            {
                ((Contexto)Helpers.Sessao.Retornar("dbSIACEntities")).Dispose();
                HttpContext.Current.Session["dbSIACEntities"] = null;
                HttpContext.Current.Session.Remove("dbSIACEntities");
            }
        }
    }
}