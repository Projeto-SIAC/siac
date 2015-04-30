<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="SIAC.Web._default" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml" lang="pt-br">
<head>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <link href="third_party/semantic/semantic.min.css" rel="stylesheet" />
    <link href="assets/css/style.css" rel="stylesheet" />
    <script src="assets/js/jquery.min.js"></script>
    <script src="third_party/semantic/semantic.min.js"></script>
    <title>SIAC: Sistema Interativo de Avaliação do Conhecimento</title>.
     
    <script>
        $('document').ready(function () {
            $('.ui.modal').modal();
            $('.second.modal').modal('attach events', '.first.modal .button');
        });
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <article class="ui coupled first modal basic">
            <i class="close icon"></i>
            <header class="header">Quem é você?</header>
            <section class="content">
                <div class="4 ui buttons icon icon fluid inverted large">
                    <button class="ui button inverted orange">
                        <i class="student icon"></i>
                        Estudante
                    </button>
                    <button class="ui button inverted green">
                        <i class="doctor icon"></i>
                        Professor
                    </button>
                    <button class="ui button inverted red">
                        <i class="user icon"></i>
                        Funcionário
                    </button>
                    <button class="ui button inverted purple">
                        <i class="spy icon"></i>
                        Convidado
                    </button>
                </div>
            </section>
        </article>
        <article class="ui modal coupled second small">
            <i class="close icon"></i>
            <header class="header">
                Quais são as suas credenciais?
            </header>
            <section class="content">
                <div class="ui form large">
                    <div class="required field">
                        <label>Matrícula</label>
                        <div class="ui icon input">
                            <asp:TextBox ID="txtMatricula" runat="server" placeholder="Matrícula"></asp:TextBox>
                            <i class="user icon"></i>
                        </div>
                    </div>
                    <div class="required field">
                        <label>Senha</label>
                        <div class="ui icon input">
                            <asp:TextBox ID="txtSenha" runat="server" TextMode="Password" placeholder="Senha"></asp:TextBox>
                            <i class="lock icon"></i>
                        </div>
                    </div>
                </div>
            </section>
            <section class="actions">
                <button class="ui button">Cancelar</button>
                <a class="ui button primary icon labeled" href="pages/"><i class="icon sign in"></i>Acessar</a>
            </section>
        </article>

        <div class="ui page grid stackable">
            <div class="row"></div>
            <div class="row">
                <header class="column">
                    <section class="ui large menu secondary pointing">
                        <div class="header item">
                            <i class="icon coffee"></i>SIAC
                        </div>
                        <nav class="right menu">
                            <div class="item hidden-xs">
                                <asp:Label ID="lblData" runat="server" Text=""></asp:Label>
                            </div>
                            <div class="item">
                                <button class="ui button" onclick="history.back();">
                                    Voltar
                                </button>
                            </div>
                        </nav>
                    </section>
                </header>
            </div>
            <div class="three column row centered ">
                <article class="column aligned center" style="top: 5rem">
                    <header class="ui header">
                        <h1>Bem vindo ao SIAC</h1>
                    </header>
                    <p class="">
                        Lorem ipsum dolor sit amet, consectetur adipiscing elit. 
                        Donec bibendum faucibus mi, vehicula fringilla lacus eleifend in. 
                        Proin malesuada libero vel velit ultrices mollis at eget orci. 
                    </p>
                    <footer>
                        <a class="ui button huge primary circular" onclick="$('.ui.first.modal').modal('show');">Acessar</a>
                    </footer>
                </article>
            </div>
        </div>
    </form>
</body>
</html>

