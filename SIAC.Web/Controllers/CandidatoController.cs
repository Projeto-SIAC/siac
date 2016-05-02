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
        public ActionResult Index() => View();

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
            Candidato = Sessao.Candidato,
            Paises = Municipio.ListarPaisesOrdenadamente(),
            Estados = Municipio.ListarEstadosOrdenadamente(),
            Municipios = Municipio.ListarOrdenadamente()
        });

        [HttpPost]
        [CandidatoFilter]
        public ActionResult Perfil(CandidatoPerfilViewModel model)
        {
            // TODO
            return View(model);
        }
    }
}