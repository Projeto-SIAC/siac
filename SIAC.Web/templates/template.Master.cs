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
            if (url.Contains("dashboard"))
            {
                hlkDashboard.CssClass += " active";
                hlkDashboard2.CssClass += " active";
                lblPrimeiroNo.Text = "Dashboard";
                return;
            }
            else if (url.Contains("historico"))
            {
                hlkHistorico.CssClass += " active";
                hlkHistorico2.CssClass += " active";
                lblPrimeiroNo.Text = "Histórico";
                return;
            }
            else if (url.Contains("perfil"))
            {
                hlkPerfil.CssClass += " active";
                hlkPerfil2.CssClass += " active";
                lblPrimeiroNo.Text = "Perfil";
                return;
            }
            else if (url.Contains("av-institucional"))
            {
                hlkAvInstitucional.CssClass += " active";
                hlkAvInstitucional2.CssClass += " active";
                lblPrimeiroNo.Text = "Avaliação Institucional";
                return;
            }            
        }
    }
}