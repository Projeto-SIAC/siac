using System;
using System.Linq;

namespace SIAC.Models
{
    public partial class AvalQuesPessoaResposta
    {
        private static Contexto contexto => Repositorio.GetInstance();

        public static void SalvarResposta(Avaliacao avaliacao, Questao questao, PessoaFisica pessoa, string resposta, string comentario)
        {
            if (avaliacao != null && questao != null && pessoa != null && !String.IsNullOrWhiteSpace(resposta))
            {
                bool sobreposicao = true;

                AvalQuesPessoaResposta corrente = contexto.AvalQuesPessoaResposta
                    .FirstOrDefault(r =>
                        r.Ano == avaliacao.Ano
                        && r.Semestre == avaliacao.Semestre
                        && r.CodTipoAvaliacao == avaliacao.CodTipoAvaliacao
                        && r.NumIdentificador == avaliacao.NumIdentificador
                        && r.CodQuestao == questao.CodQuestao
                        && r.CodPessoaFisica == pessoa.CodPessoa);

                if (corrente == null)
                {
                    sobreposicao = false;
                    corrente = new AvalQuesPessoaResposta()
                    {
                        AvalTemaQuestao = avaliacao.AvaliacaoTema
                            .Where(at => at.AvalTemaQuestao.FirstOrDefault(atq => atq.CodQuestao == questao.CodQuestao) != null)
                            .Select(at => at.AvalTemaQuestao.FirstOrDefault(atq => atq.CodQuestao == questao.CodQuestao))
                            .FirstOrDefault(),
                        PessoaFisica = pessoa
                    };
                }

                switch (questao.CodTipoQuestao)
                {
                    case TipoQuestao.OBJETIVA:
                        corrente.RespAlternativa = int.Parse(resposta);
                        break;

                    case TipoQuestao.DISCURSIVA:
                        corrente.RespDiscursiva = resposta;
                        break;

                    default:
                        break;
                }

                corrente.RespComentario = !String.IsNullOrWhiteSpace(comentario) ? comentario : null;

                if (sobreposicao == false)
                {
                    contexto.AvalQuesPessoaResposta.Add(corrente);
                }

                contexto.SaveChanges();
            }
        }
    }
}