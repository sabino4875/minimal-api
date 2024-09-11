# Minimal API

Este é um fork do projeto do bootcamp XP Inc.

## Estrutura do Projeto

- **Api**: Contém a implementação da API.
- **Test**: Contém os testes para a API.
- **minimal-api.sln**: Solução do projeto.

## Api
### Estrutura do Projeto

- **Application**: Rotinas de validação e lógica de aplicação.
  - **Profiles**: Rotinas de mapeamento das entidades de transferência de dados e as entidades do domínio.
  - **Validation**: Rotinas para validação das entidades de transferência de dados.
---
- **Authentication**: Rotinas de autenticação utilizando o token JWT.
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
- **Migrations**: Arquivos de migração do banco de dados.
- **Properties**: Propriedades do projeto.
- **Serialization**: Configurações de serialização.
- **logs**: Arquivos de log.

## Arquivos Principais

- **Program.cs**: Arquivo principal de configuração e inicialização da API.
- **mininal-api.csproj**: Arquivo de configuração do projeto.
- **settings.example.json**: Exemplo de arquivo de configuração.

## Como Executar

1. Clone o repositório:
   ```bash
   git clone https://github.com/sabino4875/minimal-api.git

2. Abra o projeto no VSCode ou no Visual Studio
   
## Contribuições
Sinta-se à vontade para fazer um fork do projeto e enviar pull requests.

## Licença
Este projeto está licenciado sob a licença MIT.