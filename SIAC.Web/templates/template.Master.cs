using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SIAC.Web.templates
{
    public partial class template : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["sair"]!=null)
            {
                Response.Redirect("~/");
            }
            lblData.Text = DateTime.Today.ToLongDateString();
            SetActiveItemMenu(Request.Url.ToString());
        }

        private void SetActiveItemMenu(string url)
        {
            if (url.Contains("/dashboard"))
            {
                hlkDashboard.CssClass += " active";
                return;
            }
            else if (url.Contains("/historico"))
            {
                hlkHistorico.CssClass += " active";
                return;
            }
            else if (url.Contains("/perfil"))
            {
                hlkPerfil.CssClass += " active";
                return;
            }
            else if (url.Contains("/av-institucional"))
            {
                hlkAvInstitucional.CssClass += " active";
                return;
            }
            
        }
    }
}