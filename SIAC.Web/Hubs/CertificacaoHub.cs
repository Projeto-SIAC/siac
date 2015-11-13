using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace SIAC.Hubs
{
    public class CertificacaoHub : Hub
    {
        public void Realizar(string codAvaliacao)
        {
            Groups.Add(Context.ConnectionId, codAvaliacao);
        }

        public void Liberar(string codAvaliacao, bool flag)
        {
            if (flag)
            {
                Clients.Group(codAvaliacao).liberar(codAvaliacao);
            }
            else
            {
                Clients.Group(codAvaliacao).bloquear(codAvaliacao);
            }
        }
    }
}