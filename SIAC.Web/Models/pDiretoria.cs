using System.Collections.Generic;
using System.Linq;

namespace SIAC.Models
{
    public partial class Diretoria
    {
        public string CodComposto => $"{CodInstituicao}.{CodCampus}.{CodDiretoria}";

        private static dbSIACEntities contexto => Repositorio.GetInstance();

        public static void Inserir(Diretoria diretoria)
        {
            //Realizando um "IDENTITY Manual"
            Campus campus = diretoria.Campus;
            List<Diretoria> diretorias = contexto.Diretoria.Where(d => d.CodInstituicao == campus.CodInstituicao 
                                                                    && d.CodCampus == campus.CodCampus).ToList();
            int id = diretorias.Count > 0 ? diretorias.Max(d => d.CodDiretoria) + 1 : 1;

            diretoria.CodDiretoria = id;
            contexto.Diretoria.Add(diretoria);
            contexto.SaveChanges();
        }

        public static List<Diretoria> ListarOrdenadamente() => contexto.Diretoria.OrderBy(d => d.Sigla).ToList();

        public static Diretoria ListarPorCodigo(string codComposto)
        {
            string[] codigos = codComposto.Split('.');
            int codInstituicao = int.Parse(codigos[0]);
            int codCampus = int.Parse(codigos[1]);
            int codDiretoria = int.Parse(codigos[2]);

            return contexto.Diretoria.FirstOrDefault(d => d.CodInstituicao == codInstituicao
                                                       && d.CodCampus == codCampus
                                                       && d.CodDiretoria == codDiretoria);
        }
    }
}