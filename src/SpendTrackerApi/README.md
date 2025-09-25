# 💰 SpendTrack API

<div align="center">

![.NET](https://img.shields.io/badge/.NET-9.0-512BD4?style=for-the-badge&logo=dotnet)
![SQLite](https://img.shields.io/badge/SQLite-07405E?style=for-the-badge&logo=sqlite&logoColor=white)
![Docker](https://img.shields.io/badge/Docker-2CA5E0?style=for-the-badge&logo=docker&logoColor=white)
![License](https://img.shields.io/badge/License-MIT-yellow.svg?style=for-the-badge)


## 🧪 Versão 1 - Arquitetura Acoplada

> ⚠️ Esta é a **primeira versão** da SpendTrack API e foi construída com arquitetura **acoplada** por escolha intencional.

Nesta fase inicial, o objetivo é praticar e validar as funcionalidades básicas de uma API REST, com uma estrutura simples e direta.
A aplicação ainda não segue um padrão totalmente desacoplado, como DDD ou Onion Architecture, mas mantém organização e boas práticas mínimas (como validações e separação em camadas).

Isso permite uma evolução mais natural para versões futuras com aplicação de princípios SOLID, SRP, DIP e outros padrões de arquitetura.

🔧 O projeto continuará evoluindo com:

Implementação de testes automatizados

Refatorações progressivas

Adoção de um estilo arquitetural mais desacoplado e escalável
---




</div>

## 📖 Sobre o Projeto

A **SpendTrack API** é uma solução completa para gerenciamento de gastos pessoais, desenvolvida com ASP.NET Core 9.0. A API oferece funcionalidades para criar, gerenciar categorias de gastos e registrar despesas de forma organizada e eficiente.

### ✨ Características Principais

- ✅ **Validação Robusta**: Implementação de validações usando FluentValidation
- 🗺️ **Mapeamento Automático**: Uso do Mapster para conversão entre DTOs e entidades
- 📊 **Documentação Automática**: OpenAPI/Swagger integrado com Scalar UI
- 🌐 **Localização**: Configuração para cultura pt-BR
- 🔄 **Entity Framework**: ORM moderno com SQLite
- 🎯 **URLs Consistentes**: URLs em minúsculas para melhor compatibilidade

## 🚀 Tecnologias Utilizadas

- **Framework**: .NET 9.0
- **Linguagem**: C# (Latest version)
- **Banco de Dados**: SQLite
- **ORM**: Entity Framework Core 9.0
- **Validação**: FluentValidation 12.0
- **Mapeamento**: Mapster 7.4
- **Documentação**: OpenAPI + Scalar UI
- **Containerização**: Docker
- **Commit**: Husky + Commitizen + Commitlint

## 📋 Pré-requisitos

- [.NET 9.0 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- [Docker](https://www.docker.com/) (opcional)
- [Node.js](https://nodejs.org/) (para commits convencionais)
- [PNPM](https://pnpm.io/) (gerenciador de pacotes Node.js)

## 🛠️ Instalação e Configuração

### 1. Clone o Repositório

```bash
git clone https://gitlab.com/antonio-tech/spendtrackapi.git
cd spendtrackapi
```

### 2. Instale as Dependências Node.js

```bash
pnpm install
```

### 3. Configure o Banco de Dados

```bash
# Aplicar migrações
dotnet ef database update
```

### 4. Execute o Projeto

#### Localmente
```bash
dotnet run
```

#### Com Docker
```bash
docker compose up --build
```

## 📚 Documentação da API

Após executar o projeto, acesse a documentação interativa:

- **Scalar UI**: `https://localhost:5001/scalar/v1` (Desenvolvimento)
- **OpenAPI Schema**: `https://localhost:5001/openapi/v1.json`

## 🔗 Endpoints Principais

### 📂 Categorias

| Método | Endpoint | Descrição |
|--------|----------|----------|
| `GET` | `/api/category` | Lista todas as categorias |
| `GET` | `/api/category/{id}` | Busca categoria por ID |
| `POST` | `/api/category` | Cria nova categoria |
| `PUT` | `/api/category/{id}` | Atualiza categoria |
| `DELETE` | `/api/category/{id}` | Remove categoria |

### 💳 Despesas

| Método | Endpoint | Descrição |
|--------|----------|----------|
| `GET` | `/api/expense` | Lista todas as despesas |
| `GET` | `/api/expense/{id}` | Busca despesa por ID |
| `GET` | `/api/expense/total` | Retorna total de despesas |
| `POST` | `/api/expense` | Cria nova despesa |
| `PUT` | `/api/expense/{id}` | Atualiza despesa |
| `DELETE` | `/api/expense/{id}` | Remove despesa |

## 📊 Exemplos de Uso

### Criar Categoria

```json
POST /api/category
{
  "name": "Alimentação",
  "description": "Gastos com comida e bebida"
}
```

### Criar Despesa

```json
POST /api/expense
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
SpendTrackApi/
├── Controllers/           # Controllers da API
│   ├── Category/         # Endpoints de categorias
│   └── Expense/          # Endpoints de despesas
├── Data/                 # Contexto do Entity Framework
├── Extensions/           # Extensões personalizadas
├── Mapping/              # Configurações do Mapster
├── Migrations/           # Migrações do banco de dados
├── Models/               # Entidades do domínio
├── Properties/           # Configurações do projeto
├── Dockerfile           # Configuração do Docker
├── compose.yaml         # Docker Compose
└── Program.cs           # Ponto de entrada da aplicação
```

## 🔧 Comandos Úteis

### Entity Framework

```bash
# Criar nova migração
dotnet ef migrations add NomeDaMigracao

# Aplicar migrações
dotnet ef database update

# Remover última migração
dotnet ef migrations remove
```

### Docker

```bash
# Build da imagem
docker build -t spendtrackapi .

# Executar container
docker run -p 5000:8080 spendtrackapi

# Docker Compose
docker compose up --build
docker compose down
```

### Commits Convencionais

```bash
# Usar commitizen para commits padronizados
pnpm commit

# Ou usar git commit normalmente (será validado pelo commitlint)
git commit -m "feat: adicionar endpoint de categorias"
```

## 🤝 Contribuindo

1. Fork o projeto
2. Crie uma branch para sua feature (`git checkout -b feature/MinhaFeature`)
3. Faça commit das mudanças (`pnpm commit` ou `git commit -m 'feat: adicionar alguma feature'`)
4. Push para a branch (`git push origin feature/MinhaFeature`)
5. Abra um Pull Request

### 📝 Padrões de Commit

Este projeto usa [Conventional Commits](https://www.conventionalcommits.org/) com [Gitmojis](https://gitmoji.dev/):

- `feat`: Nova funcionalidade
- `fix`: Correção de bug
- `docs`: Documentação
- `style`: Formatação, ponto e vírgula, etc
- `refactor`: Refatoração de código
- `test`: Testes
- `chore`: Tarefas de build, configuração, etc

## 🔐 Variáveis de Ambiente

| Variável | Descrição | Padrão |
|----------|-----------|--------|
| `ASPNETCORE_ENVIRONMENT` | Ambiente da aplicação | `Development` |
| `ConnectionStrings__DefaultConnection` | String de conexão do banco | SQLite local |

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
