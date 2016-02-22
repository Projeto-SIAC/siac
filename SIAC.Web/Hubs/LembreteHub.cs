using Microsoft.AspNet.SignalR;
using SIAC.Helpers;
using SIAC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIAC.Hubs
{
    public class LembreteHub : Hub
    {
        public void RecuperarNotificacoes(string matricula)
        {
            List<Dictionary<string, string>> notificacoes = new List<Dictionary<string, string>>();
            notificacoes.AddRange(Sistema.Notificacoes[matricula]);
            Sistema.Notificacoes[matricula].Clear();
            Clients.Client(Context.ConnectionId).receberNotificacoes(notificacoes);
        }
    }
}