using System.Web;

namespace SIAC.Models
{
    public class Repositorio
    {
        public static Contexto GetInstance()
        {
            Contexto contexto = null;
            try
            {
                if (Sistema.AlertarMudanca.Contains(Helpers.Sessao.UsuarioMatricula))
                {
                    Commit();
                    Restart();
                    Sistema.AlertarMudanca.Remove(Helpers.Sessao.UsuarioMatricula);
                }
                contexto = Helpers.Sessao.Retornar("dbSIACEntities") as Contexto;
            }
            catch
            {
                Restart();
            }
            finally
            {
                if (contexto == null)
                {
                    Helpers.Sessao.Inserir("dbSIACEntities", new Contexto());
                }
            }
            return (Contexto)Helpers.Sessao.Retornar("dbSIACEntities") ?? new Contexto();
        }

        public static void Restart()
        {
            Dispose();
            Helpers.Sessao.Inserir("dbSIACEntities", new Contexto());
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
                HttpContextManager.Current.Session["dbSIACEntities"] = null;
                HttpContextManager.Current.Session.Remove("dbSIACEntities");
            }
        }
    }
}