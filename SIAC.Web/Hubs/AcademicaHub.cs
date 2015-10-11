using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace SIAC.Web.Hubs
{
    public class AcademicaHub : Hub
    {
        public void Realizar(string codAvaliacao)
        {
            Groups.Add(Context.ConnectionId, codAvaliacao);
        }

        public void Liberar(string codAvaliacao)
        {
            Clients.Group(codAvaliacao).liberar(codAvaliacao);
        }
    }
}