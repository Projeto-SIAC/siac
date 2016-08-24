using SIAC.Filters;
using SIAC.Helpers;
using SIAC.Models;
using SIAC.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace SIAC.Controllers
{
    public class CandidatoController : Controller
    {
        // GET: simulado/candidato
        [CandidatoFilter]
        public ActionResult Index() => View(new CandidatoIndexViewModel()
        {
            Inscritos = Simulado.ListarNaoEncerradoOrdenadamente().Where(s => s.CandidatoInscrito(Sessao.Candidato.CodCandidato)).ToList(),
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
            if (Sessao.Candidato != null)
                return RedirectToAction("");

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
        public ActionResult Cadastrar() =>
            Sessao.Candidato != null ? (ActionResult)RedirectToAction("") : (ActionResult)View(new CandidatoCadastrarViewModel());

        // POST: simulado/candidato/cadastrar
        [HttpPost]
        public ActionResult Cadastrar(CandidatoCadastrarViewModel model)
        {
            if (!StringExt.IsNullOrWhiteSpace(model.Nome, model.Cpf, model.Email, model.Senha, model.SenhaConfirmacao))
            {
                if (model.SenhaConfirmacao == model.Senha)
                {
                    if (Valida.Email(model.Email))
                    {
                        model.Cpf = Formate.DeCPF(model.Cpf);
                        if (Candidato.ListarPorCPF(model.Cpf) == null)
                        {
                            if (model.Cpf.Length == 11 && Valida.CPF(model.Cpf))
                            {
                                var c = new Candidato()
                                {
                                    Nome = model.Nome,
                                    Cpf = model.Cpf,
                                    Email = model.Email,
                                    Senha = Criptografia.RetornarHash(model.Senha)
                                };

                                Candidato.Inserir(c);
                                Sessao.Inserir("SimuladoCandidato", c);

                                EnviarEmail.Cadastro(c.Email, c.Nome);

                                return RedirectToAction("Perfil");
                            }
                            else
                            {
                                model.Mensagem = "Informe um CPF válido.";
                            }
                        }
                        else
                        {
                            model.Mensagem = "Este CPF já está cadastrado.";
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
            Repositorio.Commit();
            Repositorio.Dispose();
            return RedirectToAction("");
        }

        // GET: simulado/candidato/perfil
        [CandidatoFilter]
        public ActionResult Perfil() => View(new CandidatoPerfilViewModel()
        {
            Mensagem = (String)TempData["Mensagem"],
            Paises = Municipio.ListarPaisesOrdenadamente(),
            Estados = Municipio.ListarEstadosOrdenadamente(),
            Municipios = Municipio.ListarOrdenadamente()
        });

        // POST: simulado/candidato/perfil
        [HttpPost]
        [CandidatoFilter]
        public ActionResult Perfil(string codigo, CandidatoPerfilViewModel model)
        {
            if (codigo == Sessao.Candidato.Cpf)
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

                    if (model.DtNascimento.HasValue)
                    {
                        c.DtNascimento = model.DtNascimento.Value;
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

                    if (model.Municipio > 0)
                    {
                        c.Municipio = Municipio.ListarPorCodigo(model.Pais, model.Estado, model.Municipio);
                    }

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

            if (!String.IsNullOrWhiteSpace(Request.QueryString["inscricao"]))
            {
                return RedirectToAction("Confirmar", "Inscricao", new { codigo = Request.QueryString["inscricao"] });
            }

            return RedirectToAction("Perfil", "Candidato", new
            {
                codigo = ""
            });
        }

        // POST: simulado/candidato/atualizarsenha
        [HttpPost]
        [CandidatoFilter]
        public ActionResult AtualizarSenha(string senhaAtual, string senhaNova, string senhaConfirmacao)
        {
            string mensagem = "Ocorreu um erro ao tentar alterar a senha.";
            if (!StringExt.IsNullOrWhiteSpace(senhaAtual, senhaNova, senhaConfirmacao))
            {
                Candidato c = Sessao.Candidato;
                if (Criptografia.ChecarSenha(senhaAtual, c.Senha))
                {
                    if (senhaNova == senhaConfirmacao)
                    {
                        c.Senha = Criptografia.RetornarHash(senhaNova);
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

        // GET: simulado/candidato/inscricoes
        [CandidatoFilter]
        public ActionResult Inscricoes(string codigo)
        {
            if (!String.IsNullOrEmpty(codigo) && codigo.ToLower().StartsWith("simul"))
            {
                Simulado s = Simulado.ListarPorCodigo(codigo);
                if (s != null && s.CandidatoInscrito(Sessao.Candidato.CodCandidato))
                {
                    return View(s.SimCandidato.First(sc => sc.CodCandidato == Sessao.Candidato.CodCandidato));
                }
            }
            else
            {
                var model = new CandidatoInscricoesViewModel();
                int pagina = String.IsNullOrEmpty(codigo) ? 1 : 0;
                int qtePorPagina = CandidatoInscricoesViewModel.QtePorPagina;
                if (pagina == 0)
                {
                    int.TryParse(codigo, out pagina);
                }

                if (pagina > 0)
                {
                    List<Simulado> lista = Sessao.Candidato.SimCandidato
                        .Select(sc => sc.Simulado)
                        .Distinct()
                        .OrderByDescending(d => d.PrimeiroDiaRealizacao?.DtRealizacao)
                        .ToList();
                    model.Simulados = lista.Skip(qtePorPagina * pagina - qtePorPagina).Take(qtePorPagina).ToList();
                    model.TemProxima = lista.Count > qtePorPagina;
                }

                return View("ListaInscricoes", model);
            }

            return RedirectToAction("Index");
        }

        // GET: simulado/candidato/cartaoinscricao
        [CandidatoFilter]
        public ActionResult CartaoInscricao(string codigo)
        {
            if (!String.IsNullOrWhiteSpace(codigo))
            {
                Simulado s = Simulado.ListarPorCodigo(codigo);
                if (s != null && s.FlagInscricaoEncerrado && s.CandidatoInscrito(Sessao.Candidato.CodCandidato))
                {
                    return View(s.SimCandidato.First(sc => sc.CodCandidato == Sessao.Candidato.CodCandidato));
                }
            }
            return null;
        }

        // GET: simulado/candidato/esqueceusenha
        public ActionResult EsqueceuSenha() => View(new CandidatoEsqueceuSenhaViewModel
        {
            Mensagem = (String)TempData["EsqueceuSenhaMensagem"]
        });

        // POST: simulado/candidato/esqueceusenha
        [HttpPost]
        public ActionResult EsqueceuSenha(CandidatoEsqueceuSenhaViewModel model)
        {
            if (!StringExt.IsNullOrWhiteSpace(model.Cpf, model.Email) && Valida.CPF(model.Cpf) && Valida.Email(model.Email))
            {
                Candidato c = Candidato.ListarPorCPF(Formate.DeCPF(model.Cpf));

                if (c != null && c.Email.ToLower() == model.Email.ToLower())
                {
                    string token = Candidato.GerarTokenParaAlterarSenha(c);
                    string url = Url.Action("AlterarSenha", "Candidato", new { codigo = token }, Request.Url.Scheme);
                    EnviarEmail.SolicitarSenha(c.Email, c.Nome, url);
                    TempData["EsqueceuSenhaMensagem"] = $"Um email com instruções foi enviado para {c.Email}.";
                    return RedirectToAction("EsqueceuSenha");
                }
                else
                {
                    model.Mensagem = "Não foi encontrado nenhum candidato para os dados informados.";
                }
            }
            else
            {
                model.Mensagem = "Todos os campos devem serem preenchidos com valores válidos.";
            }
            return View(model);
        }

        // GET: simulado/candidato/alterarsenha/{token}
        public ActionResult AlterarSenha(string codigo)
        {
            var model = new CandidatoAlterarSenhaViewModel();
            Candidato candidato = Candidato.LerTokenParaAlterarSenha(codigo);

            if (candidato != null)
            {
                model.Cpf = candidato.Cpf;
                model.Email = candidato.Email;
            }        
            else
            {
                model.Mensagem = "Seu acesso está inválido. Solicite novamente em <b>Tenho Cadastro > Esqueci minha senha</b>.";
            }

            return View(model);
        }

        // POST: simulado/candidato/alterarsenha/{token}
        [HttpPost]
        public ActionResult AlterarSenha(string codigo, CandidatoAlterarSenhaViewModel model)
        {
            string token = Uri.UnescapeDataString(codigo);
            Candidato candidado = Candidato.LerTokenParaAlterarSenha(Criptografia.Base64Decode(token));
            if (candidado != null)
            {
                if (!StringExt.IsNullOrWhiteSpace(model.Cpf, model.Email, model.Senha, model.Confirmacao)
                    && Valida.CPF(model.Cpf)
                    && Valida.Email(model.Email)
                    && model.Senha == model.Confirmacao)
                {
                    using (var contexto = new Contexto())
                    {
                        string cpf = Formate.DeCPF(model.Cpf);
                        Candidato c = contexto.Candidato.FirstOrDefault(cand => cand.Cpf == cpf);

                        if (c != null && c.Email.ToLower() == model.Email.ToLower())
                        {
                            if (c.Cpf == candidado.Cpf && c.Email.ToLower() == candidado.Email.ToLower())
                            {
                                c.Senha = Criptografia.RetornarHash(model.Senha);
                                c.AlterarSenha = null;
                                model.Mensagem = "Senha alterada com sucesso.";
                                model.Ok = true;
                                contexto.SaveChanges();
                                return View(model);
                            }
                        }
                        else
                        {
                            model.Mensagem = "Não foi encontrado nenhum candidato para os dados informados.";
                        }
                    }
                }
                else
                {
                    model.Mensagem = "Todos os campos devem serem preenchidos com valores válidos.";
                }

                return View(model);
            }
            return RedirectToAction("Index");
        }
    }
}