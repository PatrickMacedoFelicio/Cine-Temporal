**Trabalho feito por:**

Patrick Macêdo Felicio

Lucas Freire Sêmeler

Samuel Felipe Cardoso Leite


🎞️ Cine Temporal

Aplicação ASP.NET Core MVC para catálogo, gerenciamento e enriquecimento de filmes com dados do TMDb e clima por geolocalização.

✨ Funcionalidades

CRUD completo de filmes com sinopse, elenco, nota, idioma e poster.

Importação automática de dados via TMDb (busca, populares, lançamentos, detalhes).

Previsão do tempo por cidade/latitude/longitude usando API Open-Meteo.

Poster e informações enriquecidas diretamente da TMDb.

Cache inteligente (5–10 min) para reduzir consumo de API.

Exportação do catálogo para Excel (ClosedXML).

Modais dinâmicos (detalhes e exclusão) carregados via AJAX.

Banco SQLite integrado, criado automaticamente.

Layout responsivo + paleta inspirada no Letterboxd.

Favicon customizável (inclusive com emoji).

🏛️ Arquitetura
Controllers/      Lógica de interface e orquestração
Repositories/     Acesso ao banco via EF Core
Services/         TMDb, Clima, Exportação, Log
DTOs/             Mapeamento das respostas das APIs
ViewModels/       Modelos para exibição nas Views
Views/            Interface Razor + modais
wwwroot/          CSS, JS, imagens, favicon
Data/             DbContext (SQLite)

🔧 Tecnologias

ASP.NET Core MVC 9

Entity Framework Core 9 + SQLite

HttpClient + APIs externas

IMemoryCache

ClosedXML (exportação Excel)

Bootstrap 5

Logging integrado

⚙️ Configuração
1. Restaurar dependências
dotnet restore

2. Registrar sua TMDb API Key (User Secrets)
dotnet user-secrets init
dotnet user-secrets set "TMDb:ApiKey" "SUA_CHAVE_AQUI"

3. Executar o projeto
dotnet run


O arquivo filmes.db será criado automaticamente.

📚 Como usar
Catálogo TMDb

Buscar um filme (ex: "Batman")

Importar para a base local com um clique

Gerenciar filmes

Criar, editar e excluir

Inserir latitude/longitude para habilitar o clima

Poster, elenco e nota podem ser preenchidos de forma automática via TMDb

Modais dinâmicos

Detalhes: sinopse, elenco, nota, duração, clima e poster

Exclusão: modal de confirmação

Exportar catálogo

Geração de planilha Excel via menu de exportação

🌤️ Integração com clima

Caso o filme possua latitude e longitude, a aplicação exibe um bloco com a previsão diária utilizando a API Open-Meteo.

📁 Estrutura de diretórios (simplificada)
Sistema-Cine/
 ├── Controllers/
 ├── Services/
 ├── Repositories/
 ├── ViewModels/
 ├── Models/
 ├── DTOs/
 ├── Views/
 ├── wwwroot/
 ├── Data/
 └── Program.cs

🚀 Roadmap sugerido

Paginação do catálogo interno

Favoritos e perfis de usuário

Tema dark avançado

Dashboard com estatísticas de filmes

Histórico de requisições TMDb
