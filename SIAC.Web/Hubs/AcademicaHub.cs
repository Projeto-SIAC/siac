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
        public void ProfessorConectou(string codAvaliacao, string usrMatricula)
        {
            Groups.Add(Context.ConnectionId, "PRF" + codAvaliacao);
        }

        public void AlunoConectou(string codAvaliacao,string usrMatricula,string usrNome)
        {
            Groups.Add(Context.ConnectionId, "AVA"+codAvaliacao+"ALN"+usrMatricula);

            Clients.Group("PRF" + codAvaliacao).addAluno(usrMatricula,usrNome);
        }

        public void RequererAval(string codAvaliacao,string usrMatricula)
        {
            Clients.Group("AVA" + codAvaliacao + "ALN" + usrMatricula).enviarAval(codAvaliacao);
        }

        public void AvalEnviada(string codAvaliacao,string alnMatricula,string alnNome, string imgAval)
        {
            Clients.Group("PRF" + codAvaliacao).receberAval(alnMatricula,alnNome, imgAval);
        }
    }
}