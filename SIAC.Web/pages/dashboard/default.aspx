<%@ Page Title="" Language="C#" MasterPageFile="~/templates/template.Master" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="SIAC.Web.views.dashboard._default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h3 class="uppercase">Dashboard</h3>
    <div class="ui grid stackable">
        <div class="stackable doubling two columns centered row dash-menu">
            <div class="column aligned center">
                <a href="#">
                    <div class="ui icon header">
                        <i class="file text outline icon blue"></i>
                        <div class="content">
                            Realizar uma auto-avaliação
                        </div>
                    </div>
                </a>
            </div>
            <div class="column aligned center">
                <a href="#">
                    <div class="ui icon header">
                        <i class="copy icon blue"></i>
                        <div class="content">
                            Visualizar avaliações agendadas
                        </div>
                    </div>
                </a>
            </div>
            <div class="column aligned center">
                <a href="#">
                    <div class="ui icon header">
                        <i class="wait icon blue"></i>
                        <div class="content">
                            Visualizar meu histórico
                        </div>
                    </div>
                </a>
            </div>
            <div class="column aligned center">
                <a href="#">
                    <div class="ui icon header">
                        <i class="user icon blue"></i>
                        <div class="content">
                            Visualizar meu perfil
                        </div>
                    </div>
                </a>
            </div>
        </div>
    </div>
</asp:Content>
