# Minimal API

# Objetivo: 
Este é um fork do projeto do bootcamp XP Inc, contendo o projeto com a implementação de uma api mínima, e um projeto contendo os testes da api.

## Estrutura do Projeto

- **Api**: Contém a implementação da API.
- **Test**: Contém os testes para a API.
- **minimal-api.sln**: Solução do projeto.

## Api

Contém uma implementação de uma API mínima, baseada no desafio proposto pela Digital Innovation One.

### Estrutura do Projeto

- **Application**: Rotinas de validação e lógica de aplicação.
  - **Profiles**: Rotinas de mapeamento das entidades de transferência de dados e as entidades do domínio.
  - **Validation**: Rotinas para validação das entidades de transferência de dados.
---
- **Authentication**: Rotinas de autenticação utilizando o token JWT.
---
- **Converters**: Conversores json utilizados no projeto.
---
- **Dominio**: Classes de domínio da aplicação.
  - **DTOs**: Objetos de Transferência de Dados utilizados na API.
  - **Entidades**: Classes que representam as entidades do domínio.
  - **Enums**: Enumerações utilizadas no projeto.
  - **Extensions**: Métodos de extensão para facilitar operações comuns.
  - **Filter**: Filtros aplicados nas requisições.
  - **Helpers**: Classes auxiliares para diversas funcionalidades.
  - **Interfaces**: Interfaces que definem contratos para implementação.
  - **ModelViews**: Modelos de visualização utilizados nas respostas da API.
  - **Serviços**: Implementações dos serviços da aplicação.
  - **Settings**: Configurações da aplicação.
---
- **EndPoints**: Definição dos endpoints da API.
---
- **Infraestrutura**: Configurações de acesso a dados, mapeamento das tabelas.
  - **Configurations**: Ajustes nas rotinas de teste.
  - **Db**: Configurações relacionadas ao banco de dados.
  - **InitialData**: Dados iniciais para a aplicação. 
---
- **Middlewares**: Middlewares utilizados na API.
---
- **Migrations**: Arquivos de migração do banco de dados.
---
- **Properties**: Propriedades de execução do projeto.
---
- **Serialization**: Configurações de serialização.

### Arquivos Principais

- **Program.cs**: Arquivo principal de configuração e inicialização da API.
- **mininal-api.csproj**: Arquivo de configuração do projeto.
- **settings.example.json**: Exemplo de arquivo de configuração.

# Test

Contém uma implementação dos testes de uma API mínima, baseada no desafio proposto pela Digital Innovation One.

## Estrutura do Projeto
- **Domain**: Contém as rotinas de teste do domínio da aplicação.
- **Helpers**: Contém rotinas auxiliares para os testes.
- **Mocks**: Contém a implementação dos serviços para acesso a dados.
- **Requests**: Contém as rotinas de teste para as requisições.


### Arquivos Principais

- **GlobalUsings.cs**: Arquivo de usings globais.
- **Test.csproj**: Arquivo de configuração do projeto de teste.
- **settings.example.json**: Arquivo de configuração de exemplo.


## Como Executar

1. Clone o repositório:
   ```bash
   git clone https://github.com/sabino4875/minimal-api.git

2. Abra o projeto no VSCode ou no Visual Studio
3. Renomeie o arquivo settings.example.json para settings.json em ambos os projetos.
4. Edite o arquivo e informe os dados de conexão com o banco de dados MySql. No projeto de teste, adicione **_test** no final do nome do banco.
5. Informe os dados de configuração para geração do token JWT. 
- Exemplo de configuração
  ```bash
  "ConnectionStrings": {
    "Host": "127.0.0.1",
    "Database": "minimalApiDb",
    "UserName": "root",
    "Password": "@pass!word$",
    "Port": "3310"
  },
  "Jwt": {
    "Issuer": "https://www.minimal-api.com",
    "Audience": "urn:minimal-api.api",
    "Secret": "5DB94C64892FB2B6385C485111F07EDAEDADA57BF3A7D7AC578C122890679DFA740FE9A3480D4F747F93BC9F5A02059E22D9CDEB67A129B31F41791BECA0EB4902A7D610A7641C35EC1F2C3BD24EBBDBAED471C7F3390835D5B13C2E15DEB54F934B0DD241ED117FCA37FEA18D0C6B90C532E7645FDD62405A88D716666AEE9E",
    "Seconds": 86400
  } 

## Referência bibliográfica
- [Markdown Syntax](https://www.markdownguide.org/basic-syntax/)
- [Associação de parâmetros em aplicativos de API mínima](https://learn.microsoft.com/pt-br/aspnet/core/fundamentals/minimal-apis/parameter-binding?view=aspnetcore-8.0)
- [Customize the behavior of AuthorizationMiddleware](https://learn.microsoft.com/en-us/aspnet/core/security/authorization/customizingauthorizationmiddlewareresponse?view=aspnetcore-8.0)
- [Generated Values](https://learn.microsoft.com/en-us/ef/core/modeling/generated-properties?tabs=fluent-api)
- [Data Seeding](https://learn.microsoft.com/en-us/ef/core/modeling/data-seeding#model-seed-data)
- [Unit testing C# with MSTest and .NET](https://learn.microsoft.com/en-us/dotnet/core/testing/unit-testing-with-mstest)
- [Integration tests in ASP.NET Core](https://learn.microsoft.com/en-us/aspnet/core/test/integration-tests?view=aspnetcore-8.0)
- [Policy-based authorization in ASP.NET Core](https://learn.microsoft.com/en-us/aspnet/core/security/authorization/policies?view=aspnetcore-8.0)

## Contribuições
Sinta-se à vontade para fazer um fork do projeto e enviar pull requests.

## Licença

[Termos de uso](https://app.dio.me/terms/)

# Código de Conduta

## Introdução

Este Código de Conduta estabelece as expectativas de comportamento para todos os colaboradores, parceiros e stakeholders envolvidos com nossa organização. Nosso objetivo é promover um ambiente de trabalho respeitoso, inclusivo e profissional.

## Valores e Princípios

- **Integridade**: Agir com honestidade e transparência em todas as interações.
- **Respeito**: Tratar todos com dignidade e respeito, independentemente de suas diferenças.
- **Responsabilidade**: Assumir a responsabilidade por nossas ações e decisões.
- **Excelência**: Buscar a excelência em todas as nossas atividades.

## Comportamento Esperado

- **Profissionalismo**: Manter uma postura profissional em todas as situações.
- **Colaboração**: Trabalhar de forma colaborativa e apoiar os colegas.
- **Confidencialidade**: Proteger informações confidenciais e sensíveis.
- **Cumprimento de Prazos**: Cumprir prazos e compromissos acordados.

## Conduta Inaceitável

- **Discriminação**: Não toleramos qualquer forma de discriminação ou assédio.
- **Conflitos de Interesse**: Evitar situações que possam gerar conflitos de interesse.
- **Uso Indevido de Recursos**: Não utilizar recursos da organização para fins pessoais.
- **Comportamento Antiético**: Qualquer comportamento que viole nossos valores e princípios.

## Conformidade Legal

Todos os colaboradores devem cumprir as leis e regulamentações aplicáveis, bem como as políticas internas da organização.

## Denúncias e Consequências

Qualquer violação deste Código de Conduta deve ser reportada imediatamente. A organização investigará todas as denúncias e tomará as medidas apropriadas, que podem incluir ações disciplinares.

## Revisão e Atualização

Este Código de Conduta será revisado periodicamente para garantir que continue relevante e eficaz.

## Contato

Para dúvidas ou mais informações sobre este Código de Conduta, entre em contato com o departamento de Recursos Humanos.

---

**Última atualização:** [11/09/2024]

### Criado utilizando inteligência artificial e revisado por humano.
