﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SIAC.Models;
using Newtonsoft.Json;

namespace SIAC.ViewModels
{
    public class SimuladoProvaViewModel
    {
        public Simulado Simulado { get; set; }
        public SimProva Prova { get; set; } = new SimProva();
        public List<Disciplina> Disciplinas { get; set; }
    }

    public class SimuladoSalasViewModel
    {
        public Simulado Simulado { get; set; }
        public List<Campus> Campi { get; set; } = new List<Campus>();
        public List<Bloco> Blocos { get; set; } = new List<Bloco>();
        public List<Sala> Salas { get; set; } = new List<Sala>();

        public string BlocosEmJson => JsonConvert.SerializeObject(Blocos.Select(b => new
        {
            Campus = b.Campus.CodComposto,
            CodBloco = b.CodBloco,
            Descricao = b.Descricao,
            Sigla = b.Sigla,
            RefLocal = b.RefLocal,
            Observacao = b.Observacao
        }));

        public string SalasEmJson => JsonConvert.SerializeObject(Salas.Select(s => new
        {
            CodBloco = s.Bloco.CodBloco,
            CodSala = s.CodSala,
            Descricao = s.Descricao,
            Sigla = s.Sigla,
            RefLocal = s.RefLocal,
            Observacao = s.Observacao,
            Capacidade = s.Capacidade
        }));
    }

    public class SimuladoRespostasViewModel
    {
        public Simulado Simulado { get; set; }
        public List<SimProva> Provas { get; set; }
        public List<SimCandidato> Candidatos { get; set; }
    }

    public class SimuladoRespostasCandidatoViewModel
    {
        public Simulado Simulado { get; set; }
        public Candidato Candidato { get; set; }
        public SimProva Prova { get; set; }
        public List<SimProvaQuestao> Questoes { get; set; }
        public List<SimCandidatoQuestao> Respostas { get; set; }
    }
}