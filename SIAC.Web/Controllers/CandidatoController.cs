using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using SIAC.ViewModels;
using SIAC.Helpers;
using SIAC.Models;
using SIAC.Filters;

namespace SIAC.Controllers
{
    public class CandidatoController : Controller
    {
        // GET: simulado/candidato
        [CandidatoFilter]
        public ActionResult Index() => View(new CandidatoIndexViewModel() {
            Inscritos = Simulado.ListarNaoEncerradoOrdenadamente().Where(s=>s.CandidatoInscrito(Sessao.Candidato.CodCandidato)).ToList(),
            Passados = Simulado.ListarEncerradoOrdenadamente().Where(s => s.CandidatoInscrito(Sessao.Candidato.CodCandidato)).ToList()
        });

        // GET: simulado/candidato/acessar
        public ActionResult Acessar()
        {
            if (Sessao.Candidato != null)
                return RedirectToAction("");
            return View(new CandidatoAcessarViewModel());
        }

        // POST: simulado/candidato/acessar
        [HttpPost]
        public ActionResult Acessar(CandidatoAcessarViewModel model)
        {
            if (!StringExt.IsNullOrWhiteSpace(model.Cpf, model.Senha))
            {
                Candidato c = Candidato.Autenticar(model.Cpf, model.Senha);
                if (c != null)
                {
                    Sessao.Inserir("SimuladoCandidato", c);
                    if (Request.QueryString["continuar"] != null)
                        return Redirect(Request.QueryString["continuar"].ToString());
                    return RedirectToAction("");
                }
            }

            model.Mensagem = "Não foi encontrado nenhum cadastro para as credenciais informadas.";
            return View(model);
        }

        // GET: simulado/candidato/cadastrar
        public ActionResult Cadastrar() => View(new CandidatoCadastrarViewModel());

        // POST: simulado/candidato/cadastrar
        [HttpPost]
        public ActionResult Cadastrar(CandidatoCadastrarViewModel model)
        {
            if (!StringExt.IsNullOrWhiteSpace(model.Nome, model.Cpf, model.Email, model.Senha, model.SenhaConfirmacao))
            {
                if (model.SenhaConfirmacao == model.Senha)
                {
                    if (model.Email.Contains("@"))
                    {
                        model.Cpf = Formate.DeCPF(model.Cpf);
                        if (model.Cpf.Length == 11 && Valida.CPF(model.Cpf))
                        {
                            Candidato c = new Candidato()
                            {
                                Nome = model.Nome,
                                Cpf = model.Cpf,
                                Email = model.Email,
                                Senha = Criptografia.RetornarHash(model.Senha)
                            };

                            Candidato.Inserir(c);
                            Sessao.Inserir("SimuladoCandidato", c);
                            return RedirectToAction("Perfil");
                        }
                        else
                        {
                            model.Mensagem = "Informe um CPF válido.";
                        }
                    }
                    else
                    {
                        model.Mensagem = "Informe um email válido.";
                    }
                }
                else
                {
                    model.Mensagem = "Senha de Confirmação diferente da Senha informada.";
                }
            }
            else {
                model.Mensagem = "Todos os campos são obrigatórios.";
            }
            return View(model);
        }

        // GET: simulado/candidato/desconectar
        public ActionResult Desconectar()
        {
            Sessao.Remover("SimuladoCandidato");
            return RedirectToAction("");
        }

        // GET: simulado/candidato/perfil
        [CandidatoFilter]
        public ActionResult Perfil() => View(new CandidatoPerfilViewModel()
        {
            Mensagem = (string)TempData["Mensagem"],
            Paises = Municipio.ListarPaisesOrdenadamente(),
            Estados = Municipio.ListarEstadosOrdenadamente(),
            Municipios = Municipio.ListarOrdenadamente()
        });

        [HttpPost]
        [CandidatoFilter]
        public ActionResult Perfil(string codigo, CandidatoPerfilViewModel model)
        {
            if (codigo == Sessao.Candidato.Cpf)
            {
                if (ModelState.IsValid)
                {
                    Candidato c = Candidato.ListarPorCPF(Sessao.Candidato.Cpf);

                    if (!string.IsNullOrWhiteSpace(model.Nome))
                    {
                        c.Nome = model.Nome;
                    }

                    if (!string.IsNullOrWhiteSpace(model.Email))
                    {
                        c.Email = model.Email;
                    }

                    if (model.RgDtExpedicao != null && !string.IsNullOrWhiteSpace(model.RgOrgao) && model.RgNumero > 0)
                    {
                        c.RgDtExpedicao = model.RgDtExpedicao;
                        c.RgNumero = model.RgNumero;
                        c.RgOrgao = model.RgOrgao;
                    }

                    if (model.DtNascimento != null)
                    {
                        c.DtNascimento = model.DtNascimento;
                    }

                    if (model.Sexo != null)
                    {
                        c.Sexo = model.Sexo;
                    }

                    if (!string.IsNullOrWhiteSpace(model.TelefoneCelular) || !string.IsNullOrWhiteSpace(model.TelefoneFixo))
                    {
                        c.TelefoneCelular = model.TelefoneCelular;
                        c.TelefoneFixo = model.TelefoneFixo;
                    }

                    c.Municipio = Municipio.ListarPorCodigo(model.Pais, model.Estado, model.Municipio);

                    c.FlagAdventista = model.Adventista;

                    c.FlagNecessidadeEspecial = model.NecessidadeEspecial;

                    if (model.NecessidadeEspecial)
                    {
                        c.DescricaoNecessidadeEspecial = model.DescricaoNecessidadeEspecial;
                    }
                    else
                    {
                        c.DescricaoNecessidadeEspecial = null;
                    }

                    Repositorio.Commit();
                }
                else
                {
                    //Lembrete.AdicionarNotificacao("Erro nas informações inseridas", Lembrete.NEGATIVO, 5);
                }
            }

            if (!String.IsNullOrWhiteSpace(Request.QueryString["inscricao"]))
            {
                return RedirectToAction("Confirmar", "Inscricao", new { codigo = Request.QueryString["inscricao"] });
            }

            return RedirectToAction("Perfil", "Candidato", new
            {
                codigo = ""
            });
        }

        // POST: simulado/candidato/alterarsenha
        [HttpPost]
        public ActionResult AlterarSenha(string senhaAtual, string senhaNova, string senhaConfirmacao)
        {
            string mensagem = "Ocorreu um erro ao tentar alterar a senha.";
            if (!StringExt.IsNullOrWhiteSpace(senhaAtual, senhaNova, senhaConfirmacao))
            {
                Candidato c = Sessao.Candidato;
                string hashSenhaAtual = Criptografia.RetornarHash(senhaAtual);
                if (hashSenhaAtual == c.Senha)
                {
                    if (senhaNova == senhaConfirmacao)
                    {
                        string hashSenhaNova = Criptografia.RetornarHash(senhaNova);
                        c.Senha = hashSenhaNova;
                        Repositorio.Commit();
                        mensagem = "Senha alterada com sucesso.";
                    }
                    else
                    {
                        mensagem = "A confirmação da senha deve ser igual a senha nova.";
                    }
                }
                else
                {
                    mensagem = "A senha atual informada está incorreta.";
                }
            }
            else
            {
                mensagem = "Todos os campos são necessários para alterar a senha.";
            }
            TempData["Mensagem"] = mensagem;
            return RedirectToAction("Perfil");
        }
    }
}