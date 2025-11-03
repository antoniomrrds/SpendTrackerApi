# 💰 SpendTrack API

<div align="center">

![.NET](https://img.shields.io/badge/.NET-9.0-512BD4?style=for-the-badge&logo=dotnet)
![MySQL](https://img.shields.io/badge/MySQL-8.0-4479A1?style=for-the-badge&logo=mysql&logoColor=white)
![Docker](https://img.shields.io/badge/Docker-2CA5E0?style=for-the-badge&logo=docker&logoColor=white)
![Tests](https://img.shields.io/badge/Tests-xUnit-512BD4?style=for-the-badge)
![License](https://img.shields.io/badge/License-MIT-yellow.svg?style=for-the-badge)

## ✨ Vertical Slice Architecture

> 🏗️ API moderna construída com **Vertical Slice Architecture**, **princípios SOLID** e **organização por features**.

Esta versão implementa uma arquitetura completamente desacoplada e escalável, seguindo as melhores práticas de desenvolvimento:

- 🎯 **Vertical Slice Architecture**: Organização por features, onde cada funcionalidade é independente e coesa
- 📁 **Feature-Based Organization**: Código organizado por casos de uso, não por camadas técnicas
- 🧪 **Testes Automatizados**: Cobertura completa com testes unitários, de integração e E2E
- 🔐 **Validação Robusta**: FluentValidation com mensagens localizadas
- 📦 **Domain-Driven Design**: Modelagem rica de domínio com Value Objects e Entities
- ⚡ **Alta Coesão, Baixo Acoplamento**: Cada feature contém tudo que precisa para funcionar

---

</div>

## 📖 Sobre o Projeto

A **SpendTrack API** é uma solução completa para gerenciamento de gastos pessoais, desenvolvida com ASP.NET Core 9.0. A API oferece funcionalidades para criar, gerenciar categorias de gastos e registrar despesas de forma organizada e eficiente, seguindo os princípios de Vertical Slice Architecture.

### ✨ Características Principais

- 🎯 **Vertical Slice Architecture**: Cada feature é independente, contendo controller, use case, validação e DTOs
- 📁 **Organização por Features**: Código agrupado por funcionalidade, não por tipo técnico
- ✅ **Validação Robusta**: FluentValidation com mensagens localizadas em pt-BR
- 📊 **Documentação Automática**: OpenAPI/Swagger integrado com Scalar UI
- 🌐 **Localização**: Configuração completa para cultura pt-BR
- 🔄 **Entity Framework Core**: ORM moderno com suporte a MySQL e SQLite
- 🧪 **Testes Automatizados**: Cobertura com xUnit, testes unitários, de integração e E2E
- 🔒 **Result Pattern**: Tratamento de erros tipado e seguro
- 📦 **Shared Kernel**: Código compartilhado entre agregados do domínio
- 🚀 **Alta Manutenibilidade**: Fácil de entender, modificar e escalar

## 🚀 Tecnologias Utilizadas

### Core
- **Framework**: .NET 9.0
- **Linguagem**: C# 13 (Latest)
- **Banco de Dados**: MySQL 8.0 (Produção) / SQLite (Testes)
- **ORM**: Entity Framework Core 9.0

### Bibliotecas
- **Validação**: FluentValidation 12.0
- **Documentação**: OpenAPI + Scalar UI 2.8
- **Testes**: xUnit v3, NSubstitute, Shouldly, Bogus
- **Análise**: SonarAnalyzer

### DevOps
- **Containerização**: Docker + Docker Compose
- **Test Containers**: Testcontainers.MySql para testes de integração
- **Cobertura**: Coverlet

## 📋 Pré-requisitos

- [.NET 9.0 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- [Docker](https://www.docker.com/) (para MySQL e testes de integração)
- [MySQL 8.0](https://dev.mysql.com/downloads/) (opcional, pode usar Docker)

## 🛠️ Instalação e Configuração

### 1. Clone o Repositório

```bash
git clone https://gitlab.com/antonio-tech/spendtrackapi.git
cd spendtrackapi
```

### 2. Inicie o Banco de Dados MySQL

```bash
# Iniciar container MySQL
docker compose up -d mysql
```

### 3. Configure o Banco de Dados

```bash
# Aplicar migrações
cd src/WebApi
dotnet ef database update
```

### 4. Execute o Projeto

```bash
# A partir da raiz do projeto
dotnet run --project src/WebApi/WebApi.csproj
```

### 5. Execute os Testes

```bash
# Todos os testes
dotnet test

# Com cobertura
dotnet test --collect:"XPlat Code Coverage"

# Apenas testes unitários
dotnet test --filter "Category=Unit"

# Apenas testes de integração
dotnet test --filter "Category=Integration"
```

## 📚 Documentação da API

Após executar o projeto, acesse a documentação interativa:

- **Scalar UI**: `https://localhost:5001/scalar/v1` ou `http://localhost:5000/scalar/v1`
- **OpenAPI Schema**: `https://localhost:5001/openapi/v1.json`

> 💡 A documentação está disponível apenas em ambiente de desenvolvimento

## 🔗 Endpoints Principais

### 📂 Categorias

| Método | Endpoint | Descrição |
|--------|----------|----------|
| `GET` | `/api/categories` | Lista todas as categorias |
| `GET` | `/api/categories/{id}` | Busca categoria por ID |
| `POST` | `/api/categories` | Cria nova categoria |
| `PUT` | `/api/categories/{id}` | Atualiza categoria |
| `DELETE` | `/api/categories/{id}` | Remove categoria |

### 💳 Despesas

| Método | Endpoint | Descrição |
|--------|----------|----------|
| `GET` | `/api/expenses` | Lista todas as despesas |
| `GET` | `/api/expenses/{id}` | Busca despesa por ID |
| `GET` | `/api/expenses/total` | Retorna total de despesas |
| `POST` | `/api/expenses` | Cria nova despesa |
| `PUT` | `/api/expenses/{id}` | Atualiza despesa |
| `DELETE` | `/api/expenses/{id}` | Remove despesa |

## 📊 Exemplos de Uso

### Criar Categoria

```json
POST /api/categories
{
  "name": "Alimentação",
  "description": "Gastos com comida e bebida"
}
```

### Criar Despesa

```json
POST /api/expenses
{
  "description": "Almoço no restaurante",
  "value": 45.90,
  "date": "2024-01-15T12:00:00Z",
  "categoryId": 1
}
```

### Resposta de Despesa

```json
{
  "id": 1,
  "description": "Almoço no restaurante",
  "value": 45.90,
  "date": "2024-01-15T12:00:00Z",
  "categoryId": 1,
  "category": {
    "id": 1,
    "name": "Alimentação",
    "description": "Gastos com comida e bebida"
  }
}
```

## 🏗️ Estrutura do Projeto

```
SpendTrackerApi/
├── src/
│   ├── WebApi/                    # 🎯 Monolito Modular - Tudo em um projeto
│   │   │
│   │   ├── Domain/               # Camada de domínio
│   │   │   ├── Categories/       # Agregado de categorias
│   │   │   ├── Expenses/         # Agregado de despesas
│   │   │   ├── Errors/           # Erros de domínio
│   │   │   ├── Extensions/       # Extensões do domínio
│   │   │   ├── Resources/        # Recursos de localização
│   │   │   └── Validation/       # Validadores do domínio
│   │   │
│   │   ├── Features/             # 🎯 VERTICAL SLICES
│   │   │   │
│   │   │   ├── Categories/       # Feature de Categorias
│   │   │   │   ├── Common/       # Base controller, DTOs, Repository
│   │   │   │   ├── Create/       # Criar categoria (Controller + UseCase + Validator)
│   │   │   │   ├── GetAll/       # Listar categorias (UseCase)
│   │   │   │   └── GetById/      # Buscar por ID (Controller + UseCase)
│   │   │   │
│   │   │   └── Expense/          # Feature de Despesas
│   │   │       └── ...           # Slices de despesas
│   │   │
│   │   ├── Infrastructure/       # Camada de infraestrutura
│   │   │   └── Persistence/
│   │   │       ├── Data/             # DbContext
│   │   │       ├── EntityConfigurations/ # Configurações EF Core
│   │   │       ├── Migrations/       # Migrações do banco
│   │   │       └── Repositories/     # Implementações de repositórios
│   │   │
│   │   ├── Common/               # Código compartilhado
│   │   │   └── Web/
│   │   │       ├── Constants/    # Constantes da API
│   │   │       ├── Controllers/  # Base controllers
│   │   │       ├── Exceptions/   # Exception handlers
│   │   │       ├── Extensions/   # Extensões da API
│   │   │       ├── Factories/    # Factories de resposta
│   │   │       ├── Filters/      # Action filters
│   │   │       ├── Helper/       # Classes auxiliares
│   │   │       ├── ModelBinders/ # Model binders customizados
│   │   │       └── Responses/    # Modelos de resposta
│   │   │
│   │   ├── Properties/           # Configurações do projeto
│   │   ├── Program.cs            # Ponto de entrada
│   │   ├── appsettings.json      # Configurações
│   │   └── WebApi.csproj         # Arquivo do projeto
│   │
│   └── SharedKernel/              # Código compartilhado (Result Pattern, etc)
│       └── Abstractions/
│           └── Data/             # Interfaces de repositório
│
├── tests/
│   ├── WebApi.Tests/             # Testes unitários e de integração
│   └── TestUtilities/            # Utilitários para testes
│
├── docker-compose.yml            # Orquestração MySQL
├── Directory.Packages.props      # Gerenciamento centralizado de pacotes
├── Directory.Build.props         # Propriedades compartilhadas do build
└── SpendTracker.slnx             # Solution do projeto
```

### 🎯 Sobre a Arquitetura

**Monolito Modular com Vertical Slice Architecture:**

- 📦 **Um único projeto**: Tudo em `WebApi.csproj` para simplificar desenvolvimento
- 🎯 **Organização por Features**: Código agrupado por funcionalidade em `Features/`
- 📁 **Cada feature contém**: Controller, UseCase, Validator, DTOs e Repository
- 🏗️ **Domínio próprio**: Entidades e regras de negócio em `Domain/`
- 🔧 **Infraestrutura integrada**: Persistência em `Infrastructure/`

**Vantagens desta abordagem:**
- ✅ **Simplicidade**: Menos projetos, menos complexidade
- ✅ **Velocidade**: Builds rápidos, desenvolvimento ágil
- ✅ **Modularidade**: Features isoladas e fáceis de entender
- ✅ **Manutenibilidade**: Tudo relacionado a uma feature em um lugar
- ✅ **Evolução**: Pode ser quebrado em microserviços depois se necessário

## 🔧 Comandos Úteis

### Build e Execução

```bash
# Restaurar dependências
dotnet restore

# Build do projeto
dotnet build

# Executar API
dotnet run --project src/WebApi/WebApi.csproj

# Watch mode (hot reload)
dotnet watch --project src/WebApi/WebApi.csproj
```

### Testes

```bash
# Executar todos os testes
dotnet test

# Executar com cobertura
dotnet test --collect:"XPlat Code Coverage"

# Gerar relatório de cobertura (após instalar reportgenerator)
dotnet tool install --global dotnet-reportgenerator-globaltool
reportgenerator -reports:**/coverage.cobertura.xml -targetdir:coveragereport -reporttypes:Html

# Executar testes por categoria
dotnet test --filter "Category=Unit"
dotnet test --filter "Category=Integration"
dotnet test --filter "Category=E2E"
```

### Entity Framework

```bash
# Navegar para o diretório da WebApi
cd src/WebApi

# Criar nova migração
dotnet ef migrations add NomeDaMigracao

# Aplicar migrações
dotnet ef database update

# Remover última migração
dotnet ef migrations remove

# Listar migrações
dotnet ef migrations list
```

### Docker

```bash
# Iniciar apenas MySQL
docker compose up -d mysql

# Ver logs do MySQL
docker compose logs -f mysql

# Parar containers
docker compose down

# Parar e remover volumes
docker compose down -v
```

## 🧪 Testes

O projeto possui uma cobertura completa de testes em múltiplas camadas:

### Tipos de Testes

- **Testes Unitários**: Validam a lógica de domínio e aplicação de forma isolada
- **Testes de Integração**: Verificam a integração com banco de dados usando Testcontainers
- **Testes E2E**: Testam fluxos completos da API usando WebApplicationFactory

### Ferramentas

- **xUnit v3**: Framework de testes
- **NSubstitute**: Mocking e stubs
- **Shouldly**: Asserções fluentes
- **Bogus**: Geração de dados fake
- **Testcontainers**: Containers MySQL para testes de integração
- **Coverlet**: Cobertura de código

## 🤝 Contribuindo

1. Fork o projeto
2. Crie uma branch para sua feature (`git checkout -b feature/MinhaFeature`)
3. Faça commit das mudanças (`git commit -m 'feat: adicionar alguma feature'`)
4. Adicione testes para suas alterações
5. Push para a branch (`git push origin feature/MinhaFeature`)
6. Abra um Pull Request

### 📝 Padrões de Commit

Este projeto usa [Conventional Commits](https://www.conventionalcommits.org/):

- `feat`: Nova funcionalidade
- `fix`: Correção de bug
- `docs`: Documentação
- `style`: Formatação, ponto e vírgula, etc
- `refactor`: Refatoração de código
- `test`: Testes
- `chore`: Tarefas de build, configuração, etc
- `perf`: Melhorias de performance

## 🔐 Variáveis de Ambiente

| Variável | Descrição | Padrão |
|----------|-----------|--------|
| `ASPNETCORE_ENVIRONMENT` | Ambiente da aplicação | `Development` |
| `ConnectionStrings__DefaultConnection` | String de conexão MySQL | `Server=localhost;Port=3306;Database=spendtracker_db;Uid=spendtracker_user;Pwd=spendtracker_pass;` |

### Configuração Local

Crie um arquivo `appsettings.Development.json` em `src/WebApi/` com suas configurações locais (não versionado):

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Port=3306;Database=spendtracker_db;Uid=root;Pwd=rootpassword;"
  }
}
```

## 📄 Licença

Este projeto está sob a licença MIT. Veja o arquivo [LICENSE](LICENSE) para mais detalhes.

## 👨‍💻 Autor

**Antonio Tech**
- GitHub: [@antonio-tech](https://github.com/antoniomrrds)

## 📞 Suporte

Se você tiver alguma dúvida ou problema:

- 🐛 [Reportar um bug](https://github.com/antoniomrrds/spendtrackapi/-/issues/new)
- 💡 [Solicitar uma feature](https://github.com/antoniomrrds/spendtrackapi/-/issues/new)
- 📧 Entrar em contato via GitLab

---

<div align="center">

**⭐ Se este projeto te ajudou, considere dar uma estrela!**

Feito com ❤️ e ☕ por [Antonio Tech](https://github.com/antoniomrrds)

</div>
