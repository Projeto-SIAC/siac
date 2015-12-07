using System.Collections.Generic;
using System.Linq;

namespace SIAC.Models
{
    public partial class Campus
    {
        public string CodComposto => $"{CodInstituicao}.{CodCampus}";

        private static dbSIACEntities contexto => Repositorio.GetInstance();

        public static List<Campus> ListarOrdenadamente() => contexto.Campus.OrderBy(c => c.Sigla).ToList();

        public static void Inserir(Campus campus)
        {
            //Realizando um "IDENTITY Manual"
            Instituicao instituicao = campus.Instituicao;
            List<Campus> campi = contexto.Campus.Where(c => c.CodInstituicao == instituicao.CodInstituicao).ToList();
            int id = campi.Count > 0 ? campi.Max(c => c.CodCampus) + 1 : 1;

            campus.CodCampus = id;

            contexto.Campus.Add(campus);
            contexto.SaveChanges();
        }

        public static Campus ListarPorCodigo(string codComposto)
        {
            string[] codigos = codComposto.Split('.');
            int codInstituicao = int.Parse(codigos[0]);
            int codCampus = int.Parse(codigos[1]);

            return contexto.Campus.FirstOrDefault(c => c.CodInstituicao == codInstituicao
                                                    && c.CodCampus == codCampus);
        }
    }
}