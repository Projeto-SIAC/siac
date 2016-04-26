using SIAC.Helpers;
using SIAC.Models;
using SIAC.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SIAC.Controllers
{
    [Filters.AutenticacaoFilter(Categorias = new[] { Categoria.COLABORADOR, Categoria.PROFESSOR })]
    public class GerenciaController : Controller
    {
        // GET: gerencia
        public ActionResult Index() => View();

        // GET: gerencia/dados
        [Filters.AutenticacaoFilter(Categorias = new[] { Categoria.COLABORADOR })]
        public ActionResult Dados() => View();

        // GET: gerencia/blocos
        [Filters.AutenticacaoFilter(Categorias = new[] { Categoria.COLABORADOR })]
        public ActionResult Blocos()
        {
            GerenciaBlocosViewModel viewModel = new GerenciaBlocosViewModel();
            viewModel.Campi = Campus.ListarOrdenadamente();
            viewModel.Blocos = Bloco.ListarOrdenadamente();

            return View(viewModel);
        }

        // POST: gerencia/novobloco
        [HttpPost]
        [Filters.AutenticacaoFilter(Categorias = new[] { Categoria.COLABORADOR })]
        public ActionResult NovoBloco(FormCollection form)
        {
            string lembrete = Lembrete.NEGATIVO;
            string mensagem = "Ocorreu um erro ao tentar cadastrar um novo bloco.";

            if (form.HasKeys())
            {
                string campusCodComposto = form["ddlCampus"].Trim();
                string descricao = form["txtDescricao"].Trim();
                string sigla = form["txtSigla"].Trim();
                string refLocal = form["txtRefLocal"].Trim();
                string observacao = form["txtObservacao"].Trim();
                if (!StringExt.IsNullOrWhiteSpace(campusCodComposto, descricao))
                {
                    Bloco bloco = new Bloco();
                    bloco.Campus = Campus.ListarPorCodigo(campusCodComposto);
                    bloco.Descricao = descricao;
                    bloco.Sigla = String.IsNullOrWhiteSpace(sigla) ? null : sigla;
                    bloco.RefLocal = String.IsNullOrWhiteSpace(refLocal) ? null : refLocal;
                    bloco.Observacao = String.IsNullOrWhiteSpace(observacao) ? null : observacao;

                    Bloco.Inserir(bloco);

                    lembrete = Lembrete.POSITIVO;
                    mensagem = $"Novo bloco \"{bloco.Descricao}\" cadastrado com sucesso.";
                }
                else
                {
                    mensagem = "É necessário Campus e Descrição para cadastrar um novo bloco.";
                }
            }

            Lembrete.AdicionarNotificacao(mensagem, lembrete);
            return RedirectToAction("Blocos");
        }

        // POST: gerencia/editarbloco
        [HttpPost]
        [Filters.AutenticacaoFilter(Categorias = new[] { Categoria.COLABORADOR })]
        public ActionResult CarregarBloco(int bloco)
        {
            GerenciaEditarBlocoViewModel viewModel = new GerenciaEditarBlocoViewModel();
            viewModel.Campi = Campus.ListarOrdenadamente();
            viewModel.Bloco = Bloco.ListarPorCodigo(bloco);

            return PartialView("_CarregarBloco", viewModel);
        }

        // POST: gerencia/editarbloco
        [HttpPost]
        [Filters.AutenticacaoFilter(Categorias = new[] { Categoria.COLABORADOR })]
        public ActionResult EditarBloco(int codigo, FormCollection form)
        {
            string lembrete = Lembrete.NEGATIVO;
            string mensagem = "Ocorreu um erro ao tentar cadastrar um novo bloco.";

            Bloco bloco = Bloco.ListarPorCodigo(codigo);

            if (bloco != null && form.HasKeys())
            {
                string campusCodComposto = form["ddlCampus"].Trim();
                string descricao = form["txtDescricao"].Trim();
                string sigla = form["txtSigla"].Trim();
                string refLocal = form["txtRefLocal"].Trim();
                string observacao = form["txtObservacao"].Trim();

                if (!StringExt.IsNullOrWhiteSpace(campusCodComposto, descricao))
                {
                    bloco.Campus = Campus.ListarPorCodigo(campusCodComposto);
                    bloco.Descricao = descricao;
                    bloco.Sigla = String.IsNullOrWhiteSpace(sigla) ? null : sigla;
                    bloco.RefLocal = String.IsNullOrWhiteSpace(refLocal) ? null : refLocal;
                    bloco.Observacao = String.IsNullOrWhiteSpace(observacao) ? null : observacao;

                    Repositorio.GetInstance().SaveChanges();

                    lembrete = Lembrete.POSITIVO;
                    mensagem = $"Novo bloco \"{bloco.Descricao}\" editado com sucesso.";
                }
                else
                {
                    mensagem = "É necessário Campus e Descrição para editar um bloco.";
                }
            }

            Lembrete.AdicionarNotificacao(mensagem, lembrete);
            return RedirectToAction("Blocos");
        }

        // POST: gerencia/excluirbloco
        [HttpPost]
        [Filters.AutenticacaoFilter(Categorias = new[] { Categoria.COLABORADOR })]
        public void ExcluirBloco(int codigo)
        {
            string lembrete = Lembrete.NEGATIVO;
            string mensagem = "Ocorreu um erro ao tentar cadastrar um novo bloco.";

            Bloco bloco = Bloco.ListarPorCodigo(codigo);

            if (bloco != null)
            {
                if (bloco.Sala.Count == 0)
                {
                    Repositorio.GetInstance().Bloco.Remove(bloco);
                    Repositorio.GetInstance().SaveChanges();

                    lembrete = Lembrete.POSITIVO;
                    mensagem = $"Bloco \"{bloco.Descricao}\" excluído com sucesso.";
                }
                else
                {
                    lembrete = Lembrete.NEGATIVO;
                    mensagem = $"É necessário excluir primeiro as salas do Bloco \"{bloco.Descricao}\". Este bloco contém {bloco.Sala.Count} salas relacionadas.";
                }                
            }

            Lembrete.AdicionarNotificacao(mensagem, lembrete);
        }

        // GET: gerencia/salas
        [Filters.AutenticacaoFilter(Categorias = new[] { Categoria.COLABORADOR })]
        public ActionResult Salas() => View(new ViewModels.GerenciaSalasViewModel());
    }
}