# [SIAC](http://siac-stage.apphb.com)

> Sistema Interativo de Avaliação de Conhecimento.

Possibilitar autoavaliações em qualquer assunto ministrado no IFRN por todos que fazem a nossa instituição, além de aplicar as autoavaliações no Colégio Floriano Cavalcanti, após a conclusão e aplicação no IFRN. A ideia é permitir aos professores do IFRN elaborar e criar questões de suas disciplinas. A outra função é o de permitir a administração elaborar, gerar e divulgar resultados das avaliações institucionais de forma contínua.

## Primeiros Passos

Estas instruções são para você conseguir uma cópia do projeto e ajudar no desenvolvimento. 

### Requisitos

Antes de tudo, você precisa instalar estas ferramentas:

- [VisualStudio 2015](//www.visualstudio.com)
- [Node.js](//nodejs.org)
- [Git](//git-scm.com)

Após instalar as ferramentas anteriores, certifique de ter os comandos __npm__ e __git__ na sua linha comando, caso não estejam é necessários adicionar os caminhos para os mesmos no _path_ da sua máquina.

### Instalando

Clone o repositório localmente através de uma interface gráfica como [GitHub Desktop](//desktop.github.com/), ou através da linha de comando:

```bash
git clone https://github.com/chicobentojr/simulado.git
```

Com o __VisualStudio 2015__ aberto, abra o projeto clonado e realize um _build_. Isso acarretará na recuperação das bibliotecas pelo __Bower__ e __NuGet__.

Após isso, você pode apertar <kbd>F5</kbd> ou <kbd>Ctrl</kbd> + <kbd>F5</kbd> para visualizar o sistema no navegador.

### Publicando

O __VisualStudio__ possui uma guia próprio para ajudar na publicação do sistema. Entretanto, o mesmo não envia as bibliotecas do __Bower__, para isso você precisa executar o seguinte comando na pasta dos arquivos publicados:

```bash
npm install
```

Esse comando é responsável por instalar o Bower, caso necessário, e sicronizar as bibliotecas necessárias. Após isso, o sistema está pronto para ser utilizado.

### Autores

* [Prof. Reginaldo Falcão](//diatinf.ifrn.edu.br/doku.php?id=pessoal:docente:efetivo:regis)
* [Felipe Pontes](//github.com/felipemfp)
* [Francisco Bento](//github.com/chicobentojr)

#### Contribuidores

* Deyvison Soares
* João Paulo
* Bruno Medeiros
