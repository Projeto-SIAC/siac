/*
This file is part of SIAC.

Copyright (C) 2016 Felipe Mateus Freire Pontes <felipemfpontes@gmail.com>
Copyright (C) 2016 Francisco Bento da Silva Júnior <francisco.bento.jr@hotmail.com>

SIAC is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details. 
*/
using SIAC.Helpers;
using SIAC.Models;
using System;
using System.Linq;
using System.Web.Mvc;

namespace SIAC.Controllers
{
    [Filters.AutenticacaoFilter(Categorias = new[] { Categoria.SUPERUSUARIO, Categoria.ALUNO, Categoria.PROFESSOR, Categoria.COLABORADOR })]
    public class PerfilController : Controller
    {
        // GET: perfil
        public ActionResult Index()
        {
            return View(Sistema.UsuarioAtivo[Sessao.UsuarioMatricula].Usuario);
        }

        // GET: perfil/estatisticas
        [OutputCache(CacheProfile = "PorUsuario")]
        public ActionResult Estatisticas()
        {
            return PartialView("_Estatisticas", Sistema.UsuarioAtivo[Sessao.UsuarioMatricula].Usuario);
        }

        // POST: perfil/enviaropiniao
        [HttpPost]
        public void EnviarOpiniao(string opiniao)
        {
            if (!String.IsNullOrWhiteSpace(opiniao))
            {
                Usuario usuario = Sistema.UsuarioAtivo[Sessao.UsuarioMatricula].Usuario;
                usuario.UsuarioOpiniao.Add(new UsuarioOpiniao
                {
                    CodOrdem = usuario.UsuarioOpiniao.Count > 0 ? usuario.UsuarioOpiniao.Max(o => o.CodOrdem) + 1 : 1,
                    DtEnvio = DateTime.Now,
                    Opiniao = opiniao.Trim()
                });
                Repositorio.GetInstance().SaveChanges(false);
            }
        }

        // POST: perfil/alterarsenha
        [HttpPost]
        public ActionResult AlterarSenha(string senhaAtual, string senhaNova, string senhaConfirmacao)
        {
            string lembrete = Lembrete.NEGATIVO;
            string mensagem = "Ocorreu um erro ao tentar alterar a senha.";

            if (!StringExt.IsNullOrWhiteSpace(senhaAtual, senhaNova, senhaConfirmacao))
            {
                Usuario usuario = Sistema.UsuarioAtivo[Sessao.UsuarioMatricula].Usuario;

                if (Criptografia.ChecarSenha(senhaAtual, usuario.Senha))
                {
                    if (senhaNova == senhaConfirmacao)
                    {
                        usuario.Senha = Criptografia.RetornarHash(senhaNova);
                        Repositorio.Commit();

                        lembrete = Lembrete.POSITIVO;
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

            Lembrete.AdicionarNotificacao(mensagem, lembrete);
            return RedirectToAction("Index");
        }
    }
}