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