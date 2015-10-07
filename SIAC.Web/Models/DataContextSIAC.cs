using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIAC.Web.Models
{ 
    public class DataContextSIAC
    {        
        public static dbSIACEntities GetInstance()
        {
            dbSIACEntities contexto = HttpContext.Current.Session["dbSIACEntities"] as dbSIACEntities;
            if (contexto == null)
            {
                HttpContext.Current.Session["dbSIACEntities"] = new dbSIACEntities();
            }
            return (dbSIACEntities)HttpContext.Current.Session["dbSIACEntities"];
        }
    }
}