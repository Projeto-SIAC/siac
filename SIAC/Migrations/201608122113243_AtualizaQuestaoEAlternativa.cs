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
namespace SIAC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AtualizaQuestaoEAlternativa : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Alternativa", "Enunciado", c => c.String(nullable: false));
            AlterColumn("dbo.Questao", "Enunciado", c => c.String(nullable: false));
            AlterColumn("dbo.Questao", "ChaveDeResposta", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Questao", "ChaveDeResposta", c => c.String(unicode: false, storeType: "text"));
            AlterColumn("dbo.Questao", "Enunciado", c => c.String(nullable: false, maxLength: 250));
            AlterColumn("dbo.Alternativa", "Enunciado", c => c.String(nullable: false, maxLength: 250));
        }
    }
}
