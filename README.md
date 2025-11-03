# FIAP Cloud Games - API REST

## Descrição do Projeto

O **FIAP Cloud Games (FCG)** é uma plataforma de venda de jogos digitais e gestão de servidores para partidas online. Esta é a **Fase 1** do projeto, que consiste em uma API REST desenvolvida em **.NET 8** para gerenciamento de usuários, jogos, biblioteca de jogos adquiridos e promoções.

## Tecnologias Utilizadas

- **.NET 8** - Framework principal
- **ASP.NET Core Web API** - Framework para construção da API REST
- **Entity Framework Core 8** - ORM para acesso ao banco de dados
- **SQL Server** - Banco de dados relacional
- **JWT (JSON Web Tokens)** - Autenticação e autorização
- **BCrypt.Net** - Hash de senhas
- **Swagger/OpenAPI** - Documentação interativa da API
- **xUnit** - Framework de testes unitários
- **Moq** - Biblioteca para criação de mocks
- **FluentAssertions** - Biblioteca para assertions mais legíveis

## Arquitetura

O projeto segue os princípios de **Domain-Driven Design (DDD)** e está organizado em camadas:

```
FiapCloudGames/
├── FiapCloudGames.API/              # Camada de apresentação (Controllers, Middleware)
├── FiapCloudGames.Domain/           # Camada de domínio (Entidades, Validadores)
├── FiapCloudGames.Application/      # Camada de aplicação (Services, DTOs)
├── FiapCloudGames.Infrastructure/   # Camada de infraestrutura (EF Core, Repositórios)
└── FiapCloudGames.Tests/            # Testes unitários
```

## Funcionalidades Implementadas

### Autenticação e Autorização
- ✅ Registro de usuários com validação de e-mail e senha segura
- ✅ Login com geração de token JWT
- ✅ Dois níveis de acesso: **Usuário** e **Administrador**

### Gerenciamento de Usuários
- ✅ Obter dados do usuário autenticado
- ✅ Atualizar dados do usuário autenticado
- ✅ Listar todos os usuários (Admin)
- ✅ Obter usuário por ID (Admin)
- ✅ Deletar usuário (Admin)

### Gerenciamento de Jogos
- ✅ Listar todos os jogos ativos
- ✅ Obter jogo por ID
- ✅ Cadastrar novo jogo (Admin)
- ✅ Atualizar jogo (Admin)
- ✅ Desativar jogo (Admin)
- ✅ Cálculo automático de preço com promoção ativa

### Biblioteca do Usuário
- ✅ Obter biblioteca de jogos do usuário
- ✅ Adicionar jogo à biblioteca (compra)
- ✅ Verificar se usuário possui determinado jogo

### Gerenciamento de Promoções
- ✅ Listar promoções ativas
- ✅ Obter promoção por ID
- ✅ Criar promoção (Admin)
- ✅ Atualizar promoção (Admin)
- ✅ Deletar promoção (Admin)

## Requisitos Técnicos Atendidos

### Validações
- ✅ Validação de formato de e-mail
- ✅ Validação de senha segura (mínimo 8 caracteres com números, letras maiúsculas, minúsculas e caracteres especiais)
- ✅ E-mail único no sistema

### Persistência de Dados
- ✅ Entity Framework Core para gerenciar modelos
- ✅ Migrations para versionamento do schema
- ✅ SQL Server como banco de dados

### Desenvolvimento da API
- ✅ Padrão Controllers MVC
- ✅ Middleware para tratamento de erros
- ✅ Documentação Swagger com suporte a JWT
- ✅ Autenticação via token JWT

### Qualidade de Software
- ✅ Testes unitários para validações
- ✅ Testes para regras de negócio
- ✅ 33 testes implementados com 100% de sucesso

### Domain-Driven Design (DDD)
- ✅ Organização em camadas
- ✅ Entidades do domínio bem definidas
- ✅ Separação de responsabilidades
- ✅ Repositórios e Unit of Work

## Pré-requisitos

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- SQLite (incluído automaticamente)

## Instalação e Configuração

### 1. Clonar o Repositório

```bash
git clone <url-do-repositorio>
cd FiapCloudGames
```

### 2. Configurar a Connection String

O projeto está configurado para usar SQLite por padrão. O arquivo de banco de dados será criado automaticamente na primeira execução.

Se desejar usar SQL Server, edite o arquivo `FiapCloudGames.API/appsettings.json` e ajuste a connection string.

### 3. Restaurar Pacotes

```bash
dotnet restore
```

### 4. Aplicar Migrations

```bash
cd FiapCloudGames.API
dotnet ef database update --project ../FiapCloudGames.Infrastructure
```

### 5. Executar a API

```bash
dotnet run
```

A API estará disponível em:
- HTTP: `http://localhost:5000`
- HTTPS: `https://localhost:5001`
- Swagger UI: `http://localhost:5000` ou `https://localhost:5001`

## Executar Testes

Para executar todos os testes unitários:

```bash
dotnet test
```

## Endpoints da API

### Autenticação

#### Registrar Usuário
```http
POST /api/auth/register
Content-Type: application/json

{
  "name": "João Silva",
  "email": "joao@example.com",
  "password": "Senha123@"
}
```

#### Login
```http
POST /api/auth/login
Content-Type: application/json

{
  "email": "joao@example.com",
  "password": "Senha123@"
}
```

### Usuários (requer autenticação)

#### Obter Dados do Usuário Autenticado
```http
GET /api/users/me
Authorization: Bearer {token}
```

#### Atualizar Dados do Usuário Autenticado
```http
PUT /api/users/me
Authorization: Bearer {token}
Content-Type: application/json

{
  "name": "João Silva Atualizado",
  "email": "joao.novo@example.com"
}
```

#### Listar Todos os Usuários (Admin)
```http
GET /api/users
Authorization: Bearer {token}
```

### Jogos

#### Listar Todos os Jogos
```http
GET /api/games
```

#### Obter Jogo por ID
```http
GET /api/games/{id}
```

#### Cadastrar Novo Jogo (Admin)
```http
POST /api/games
Authorization: Bearer {token}
Content-Type: application/json

{
  "title": "The Legend of Zelda",
  "description": "Um jogo de aventura épico",
  "price": 299.90,
  "genre": "Aventura",
  "releaseDate": "2024-01-15",
  "developer": "Nintendo",
  "publisher": "Nintendo"
}
```

### Biblioteca

#### Obter Biblioteca do Usuário
```http
GET /api/library
Authorization: Bearer {token}
```

#### Comprar Jogo
```http
POST /api/library/purchase
Authorization: Bearer {token}
Content-Type: application/json

{
  "gameId": "3fa85f64-5717-4562-b3fc-2c963f66afa6"
}
```

### Promoções

#### Listar Promoções Ativas
```http
GET /api/promotions
```

#### Criar Promoção (Admin)
```http
POST /api/promotions
Authorization: Bearer {token}
Content-Type: application/json

{
  "gameId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "discountPercentage": 20,
  "startDate": "2024-11-01T00:00:00Z",
  "endDate": "2024-11-30T23:59:59Z"
}
```

## Estrutura do Banco de Dados

### Tabelas

- **Users** - Usuários do sistema
- **Games** - Jogos disponíveis na plataforma
- **UserGames** - Biblioteca de jogos dos usuários (relacionamento N:N)
- **Promotions** - Promoções aplicadas aos jogos

### Relacionamentos

- Um usuário pode ter vários jogos (UserGames)
- Um jogo pode pertencer a vários usuários (UserGames)
- Um jogo pode ter várias promoções
- Uma promoção pertence a um jogo

## Regras de Negócio

### Senha Segura
- Mínimo de 8 caracteres
- Pelo menos 1 letra maiúscula
- Pelo menos 1 letra minúscula
- Pelo menos 1 número
- Pelo menos 1 caractere especial

### E-mail
- Formato válido
- Único no sistema

### Compra de Jogos
- Usuário não pode comprar o mesmo jogo duas vezes
- Preço de compra é calculado com base na promoção ativa (se houver)
- Apenas jogos ativos podem ser comprados

### Promoções
- Data de início deve ser anterior à data de término
- Percentual de desconto entre 0 e 100
- Apenas uma promoção é aplicada por vez (a de maior desconto)

## Níveis de Acesso

### Usuário (User)
- Visualizar jogos
- Comprar jogos
- Visualizar sua biblioteca
- Atualizar seus próprios dados

### Administrador (Admin)
- Todas as permissões de usuário
- Cadastrar, atualizar e desativar jogos
- Criar, atualizar e deletar promoções
- Listar e gerenciar todos os usuários

## Middleware de Tratamento de Erros

A API possui um middleware global que captura exceções não tratadas e retorna uma resposta padronizada:

```json
{
  "error": "Ocorreu um erro interno no servidor.",
  "message": "Detalhes do erro",
  "timestamp": "2024-11-01T10:00:00Z"
}
```

## Documentação Swagger

A documentação interativa da API está disponível através do Swagger UI. Acesse a raiz da aplicação (`http://localhost:5000`) para visualizar e testar todos os endpoints.

### Como usar o Swagger com JWT

1. Faça login através do endpoint `/api/auth/login`
2. Copie o token retornado
3. Clique no botão "Authorize" no topo da página do Swagger
4. Digite `Bearer {seu-token}` no campo de autorização
5. Clique em "Authorize"
6. Agora você pode testar os endpoints protegidos

## Testes Implementados

### Validadores
- ✅ PasswordValidatorTests (13 testes)
- ✅ EmailValidatorTests (8 testes)

### Entidades
- ✅ PromotionTests (12 testes)

Total: **33 testes** com **100% de sucesso**

## Próximas Fases

Este projeto está preparado para receber as seguintes funcionalidades nas próximas fases:

- Matchmaking para partidas online
- Gerenciamento de servidores
- Sistema de amigos e chat
- Conquistas e estatísticas
- Sistema de avaliações e comentários

## Autor

Projeto desenvolvido para o Tech Challenge da FIAP - Fase 1

## Licença

Este projeto é parte de um trabalho acadêmico e não possui licença de uso comercial.
