﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIAC.Web.Models
{
    public class Repositorio
    {
        public static dbSIACEntities GetInstance()
        {
            if (Sistema.AlertarMudanca.Contains(Helpers.Sessao.UsuarioMatricula))
            {
                Commit();
                Dispose();
                Sistema.AlertarMudanca.Remove(Helpers.Sessao.UsuarioMatricula);
            }
            dbSIACEntities contexto = Helpers.Sessao.Retornar("dbSIACEntities") as dbSIACEntities;
            if (contexto == null)
            {
                Helpers.Sessao.Inserir("dbSIACEntities", new dbSIACEntities());
            }
            return (dbSIACEntities)Helpers.Sessao.Retornar("dbSIACEntities");
        }

        public static void Commit()
        {
            dbSIACEntities contexto = Helpers.Sessao.Retornar("dbSIACEntities") as dbSIACEntities;
            if (contexto != null && contexto.ChangeTracker.HasChanges())
            {
                ((dbSIACEntities)Helpers.Sessao.Retornar("dbSIACEntities")).SaveChanges();
            }
        }

        public static void Dispose()
        {
            dbSIACEntities contexto = Helpers.Sessao.Retornar("dbSIACEntities") as dbSIACEntities;
            if (contexto != null)
            {
                ((dbSIACEntities)Helpers.Sessao.Retornar("dbSIACEntities")).Dispose();
                HttpContext.Current.Session["dbSIACEntities"] = null;
                HttpContext.Current.Session.Remove("dbSIACEntities");
            }
        }
    }
}