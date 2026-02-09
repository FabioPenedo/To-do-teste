# To-Do API

API RESTful para gerenciamento de tarefas (To-Do List) desenvolvida em .NET 10 com autenticação JWT, PostgreSQL e Docker.

## 📋 Sobre o Projeto

Este projeto é uma API completa de gerenciamento de tarefas que permite aos usuários criar, ler, atualizar e deletar tarefas, com sistema de autenticação e autorização usando JWT (JSON Web Tokens) e Refresh Tokens.

## 🚀 Tecnologias

- **.NET 10**
- **Entity Framework Core 9.0.8**
- **PostgreSQL** (Npgsql.EntityFrameworkCore.PostgreSQL)
- **JWT Authentication** (Microsoft.AspNetCore.Authentication.JwtBearer)
- **BCrypt** para hash de senhas
- **Serilog** para logging
- **Swagger/OpenAPI** para documentação
- **Docker** e **Docker Compose** para containerização

## 📦 Pré-requisitos

### Opção 1: Executar com Docker (Recomendado)
- [Docker](https://www.docker.com/get-started) instalado
- [Docker Compose](https://docs.docker.com/compose/install/) instalado

### Opção 2: Executar localmente
- [.NET 10 SDK](https://dotnet.microsoft.com/download) instalado
- [PostgreSQL](https://www.postgresql.org/download/) instalado e rodando

## 🔧 Configuração

### 1. Clone o repositório

```bash
git clone https://github.com/FabioPenedo/To-do-teste.git
cd To-do-teste
```

### 2. Configuração do Banco de Dados

O projeto está configurado para conectar ao PostgreSQL. As configurações padrão estão em `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=todo_db;Username=postgres;Password=123456"
  }
}
```

**⚠️ Importante:** Para ambientes de produção, altere a senha padrão e use variáveis de ambiente ou User Secrets.

## 🐳 Executando com Docker

A maneira mais fácil de executar o projeto é usando Docker Compose, que irá subir tanto a API quanto o banco de dados PostgreSQL:

```bash
docker-compose up -d
```
ou 

```bash
iniciando pelo start do visual studio
```

Isso irá:
- Criar e iniciar o container do PostgreSQL na porta 5432
- Criar e iniciar o container da API nas portas 8080 (HTTP) e 8081 (HTTPS)
- Configurar a rede entre os containers
- Persistir os dados do PostgreSQL em um volume

### Verificar logs dos containers

```bash
# Ver logs da API
docker logs todoteste-api -f

# Ver logs do PostgreSQL
docker logs todoteste-postgres -f
```

### Parar os containers

```bash
docker-compose down
```

### Parar e remover volumes (apaga dados do banco)

```bash
docker-compose down -v
```

## 💻 Executando Localmente (sem Docker)

### 1. Executar as Migrations

No diretório do projeto, execute:

```bash
cd To-do-teste
dotnet ef database update
```

### 2. Atualizar a string de conexão (se necessário)

Edite o arquivo `To-do-teste/appsettings.json` com suas credenciais do PostgreSQL.


Isso irá criar todas as tabelas necessárias no banco de dados.

### 4. Executar a aplicação

```bash
dotnet run
```

Ou para desenvolvimento com hot reload:

```bash
dotnet watch run
```

A API estará disponível em:
- HTTP: `http://localhost:5298`
- HTTPS: `https://localhost:7131`

## 📚 Documentação da API

Após iniciar a aplicação, acesse a documentação Swagger:

- **Com Docker**: `https://localhost:8081/index.html` ou `http://localhost:8080/index.html`
- **Localmente**: `https://localhost:7131/index.html` ou `http://localhost:5298/index.html`

## 🔐 Autenticação

A API utiliza JWT (JSON Web Tokens) para autenticação. O fluxo é:

1. **Registrar usuário**: `POST /api/auth/register`
2. **Login**: `POST /api/auth/login` - retorna um Access Token (válido por 15 minutos) e um Refresh Token (válido por 7 dias)
3. **Usar o token**: Inclua o token no header `Authorization: Bearer {seu-token}`
4. **Renovar token**: Ele pega do cockie o Refresh Token para obter novos tokens


## 📁 Estrutura do Projeto

```
To-do-teste/
├── src/
│   ├── Controllers/       # Controllers da API
│   ├── Services/          # Lógica de negócio
│   ├── Repositories/      # Acesso a dados
│   ├── Entities/          # Entidades do banco de dados
│   ├── DTOs/              # Data Transfer Objects
│   ├── Interfaces/        # Interfaces e contratos
│   ├── Middlewares/       # Middlewares customizados
│   ├── Data/              # Configuração do DbContext
│   ├── Configurations/    # Configurações da aplicação
│   └── Logging/           # Configurações de log
├── Migrations/            # Migrations do EF Core
├── logs/                  # Arquivos de log
├── Dockerfile             # Configuração Docker da API
├── appsettings.json       # Configurações da aplicação
└── Program.cs             # Ponto de entrada da aplicação
```

## 🛠️ Comandos Úteis

### Entity Framework Core

```bash
# Criar nova migration
dotnet ef migrations add NomeDaMigration

# Aplicar migrations
dotnet ef database update

# Reverter última migration
dotnet ef migrations remove

# Ver informações do banco
dotnet ef dbcontext info
```

### Docker

```bash
# Rebuild dos containers
docker-compose up --build

# Ver containers rodando
docker ps

# Entrar no container da API
docker exec -it todoteste-api bash

# Entrar no container do PostgreSQL
docker exec -it todoteste-postgres psql -U postgres -d todo_db
```

## 📝 Logs

Os logs são salvos em:
- Console (desenvolvimento)
- Arquivo: `logs/todo-teste-{data}.log`
- Retenção: 30 dias

## 🔒 Segurança

- Senhas são hashadas usando BCrypt
- Tokens JWT com expiração configurável
- Refresh Tokens armazenados no banco de dados
- CORS configurado
- Validação de dados de entrada

## 👤 Front-end

GitHub: [@Front-end](https://github.com/FabioPenedo/to-do-app)

## 👤 Autor

**Fabio Penedo**

- GitHub: [@FabioPenedo](https://github.com/FabioPenedo)