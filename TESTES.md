# Guia de Testes - FIAP Cloud Games API

## Testes Realizados

Este documento descreve os testes realizados na API para validar todas as funcionalidades implementadas.

## Ambiente de Testes

- **Plataforma**: Linux (Ubuntu 22.04)
- **Banco de Dados**: SQLite (FiapCloudGames.db)
- **Framework**: .NET 8
- **Porta**: 5000 (HTTP)

## 1. Testes de Autenticação

### 1.1 Registro de Usuário

**Endpoint**: `POST /api/auth/register`

**Requisição**:
```bash
curl -X POST http://localhost:5000/api/auth/register \
  -H "Content-Type: application/json" \
  -d '{
    "name": "Admin User",
    "email": "admin@fiap.com",
    "password": "Admin123@"
  }'
```

**Resposta Esperada** (Status 200):
```json
{
  "message": "Usuário registrado com sucesso.",
  "data": {
    "id": "a4a5a528-53b8-451f-ac9f-cab6cab7423c",
    "name": "Admin User",
    "email": "admin@fiap.com",
    "role": 0,
    "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
  }
}
```

**Validações Testadas**:
- ✅ E-mail válido
- ✅ Senha com mínimo 8 caracteres
- ✅ Senha com letra maiúscula
- ✅ Senha com letra minúscula
- ✅ Senha com número
- ✅ Senha com caractere especial
- ✅ Token JWT gerado corretamente

### 1.2 Login

**Endpoint**: `POST /api/auth/login`

**Requisição**:
```bash
curl -X POST http://localhost:5000/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{
    "email": "admin@fiap.com",
    "password": "Admin123@"
  }'
```

**Resposta Esperada** (Status 200):
```json
{
  "message": "Login realizado com sucesso.",
  "data": {
    "id": "a4a5a528-53b8-451f-ac9f-cab6cab7423c",
    "name": "Admin User",
    "email": "admin@fiap.com",
    "role": 1,
    "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
  }
}
```

**Validações Testadas**:
- ✅ Autenticação com credenciais corretas
- ✅ Verificação de senha com BCrypt
- ✅ Geração de novo token JWT

## 2. Testes de Usuários

### 2.1 Obter Dados do Usuário Autenticado

**Endpoint**: `GET /api/users/me`

**Requisição**:
```bash
curl -X GET http://localhost:5000/api/users/me \
  -H "Authorization: Bearer {token}"
```

**Resposta Esperada** (Status 200):
```json
{
  "id": "a4a5a528-53b8-451f-ac9f-cab6cab7423c",
  "name": "Admin User",
  "email": "admin@fiap.com",
  "role": 1,
  "createdAt": "2025-11-02T22:57:49.3200593"
}
```

**Validações Testadas**:
- ✅ Autenticação via JWT
- ✅ Retorno dos dados do usuário autenticado

## 3. Testes de Jogos

### 3.1 Criar Jogo (Admin)

**Endpoint**: `POST /api/games`

**Requisição**:
```bash
curl -X POST http://localhost:5000/api/games \
  -H "Authorization: Bearer {token}" \
  -H "Content-Type: application/json" \
  -d '{
    "title": "The Legend of Zelda",
    "description": "Um jogo de aventura épico",
    "price": 299.90,
    "genre": "Aventura",
    "releaseDate": "2024-01-15T00:00:00Z",
    "developer": "Nintendo",
    "publisher": "Nintendo"
  }'
```

**Resposta Esperada** (Status 201):
```json
{
  "message": "Jogo criado com sucesso.",
  "data": {
    "id": "7b6d807a-f6c5-4395-9e3a-191ef936a3f6",
    "title": "The Legend of Zelda",
    "description": "Um jogo de aventura épico",
    "price": 299.90,
    "discountedPrice": null,
    "genre": "Aventura",
    "releaseDate": "2024-01-15T00:00:00Z",
    "developer": "Nintendo",
    "publisher": "Nintendo",
    "isActive": true
  }
}
```

**Validações Testadas**:
- ✅ Autorização Admin
- ✅ Validação de campos obrigatórios
- ✅ Criação com sucesso

### 3.2 Listar Todos os Jogos

**Endpoint**: `GET /api/games`

**Requisição**:
```bash
curl -X GET http://localhost:5000/api/games
```

**Resposta Esperada** (Status 200):
```json
[
  {
    "id": "7b6d807a-f6c5-4395-9e3a-191ef936a3f6",
    "title": "The Legend of Zelda",
    "description": "Um jogo de aventura épico",
    "price": 299.9,
    "discountedPrice": null,
    "genre": "Aventura",
    "releaseDate": "2024-01-15T00:00:00",
    "developer": "Nintendo",
    "publisher": "Nintendo",
    "isActive": true
  }
]
```

**Validações Testadas**:
- ✅ Listagem sem autenticação
- ✅ Retorno apenas de jogos ativos
- ✅ Cálculo de preço com desconto (quando aplicável)

## 4. Testes de Biblioteca

### 4.1 Comprar Jogo

**Endpoint**: `POST /api/library/purchase`

**Requisição**:
```bash
curl -X POST http://localhost:5000/api/library/purchase \
  -H "Authorization: Bearer {token}" \
  -H "Content-Type: application/json" \
  -d '{
    "gameId": "7b6d807a-f6c5-4395-9e3a-191ef936a3f6"
  }'
```

**Resposta Esperada** (Status 200):
```json
{
  "message": "Jogo adicionado à biblioteca com sucesso."
}
```

**Validações Testadas**:
- ✅ Autenticação obrigatória
- ✅ Verificação de jogo existente
- ✅ Verificação de jogo ativo
- ✅ Prevenção de compra duplicada
- ✅ Aplicação de promoção (se houver)

### 4.2 Visualizar Biblioteca

**Endpoint**: `GET /api/library`

**Requisição**:
```bash
curl -X GET http://localhost:5000/api/library \
  -H "Authorization: Bearer {token}"
```

**Resposta Esperada** (Status 200):
```json
[
  {
    "id": "cd952cb1-df5a-4bd4-9869-27b5789a982b",
    "gameId": "7b6d807a-f6c5-4395-9e3a-191ef936a3f6",
    "gameTitle": "The Legend of Zelda",
    "purchaseDate": "2025-11-02T22:58:55.3942634",
    "purchasePrice": 299.9
  }
]
```

**Validações Testadas**:
- ✅ Autenticação obrigatória
- ✅ Retorno apenas dos jogos do usuário
- ✅ Informações de compra corretas

## 5. Testes Unitários

### Executar Testes

```bash
dotnet test
```

### Resultados

```
Passed!  - Failed:     0, Passed:    33, Skipped:     0, Total:    33
```

### Cobertura de Testes

#### PasswordValidatorTests (13 testes)
- ✅ Senha nula
- ✅ Senha muito curta
- ✅ Senha sem letra maiúscula
- ✅ Senha sem letra minúscula
- ✅ Senha sem número
- ✅ Senha sem caractere especial
- ✅ Senhas válidas (múltiplos casos)

#### EmailValidatorTests (8 testes)
- ✅ E-mail nulo ou vazio
- ✅ Formatos inválidos
- ✅ Formatos válidos

#### PromotionTests (12 testes)
- ✅ Validação de promoção ativa
- ✅ Validação de período de validade
- ✅ Cálculo de desconto
- ✅ Múltiplos cenários de desconto

## 6. Testes de Validação

### 6.1 Validação de Senha Fraca

**Requisição**:
```bash
curl -X POST http://localhost:5000/api/auth/register \
  -H "Content-Type: application/json" \
  -d '{
    "name": "Test User",
    "email": "test@example.com",
    "password": "123456"
  }'
```

**Resposta Esperada** (Status 400):
```json
{
  "message": "A senha deve ter no mínimo 8 caracteres."
}
```

### 6.2 Validação de E-mail Inválido

**Requisição**:
```bash
curl -X POST http://localhost:5000/api/auth/register \
  -H "Content-Type: application/json" \
  -d '{
    "name": "Test User",
    "email": "invalid-email",
    "password": "Senha123@"
  }'
```

**Resposta Esperada** (Status 400):
```json
{
  "message": "E-mail inválido."
}
```

### 6.3 Validação de E-mail Duplicado

**Requisição**:
```bash
curl -X POST http://localhost:5000/api/auth/register \
  -H "Content-Type: application/json" \
  -d '{
    "name": "Another User",
    "email": "admin@fiap.com",
    "password": "Senha123@"
  }'
```

**Resposta Esperada** (Status 400):
```json
{
  "message": "E-mail já cadastrado."
}
```

## 7. Testes de Autorização

### 7.1 Acesso Sem Token

**Requisição**:
```bash
curl -X GET http://localhost:5000/api/users/me
```

**Resposta Esperada** (Status 401):
```json
{
  "error": "Unauthorized"
}
```

### 7.2 Acesso de Usuário Comum a Endpoint Admin

**Requisição** (com token de usuário comum):
```bash
curl -X POST http://localhost:5000/api/games \
  -H "Authorization: Bearer {user_token}" \
  -H "Content-Type: application/json" \
  -d '{...}'
```

**Resposta Esperada** (Status 403):
```json
{
  "error": "Forbidden"
}
```

## 8. Testes de Middleware

### 8.1 Tratamento de Erro Global

O middleware de tratamento de erros captura exceções não tratadas e retorna uma resposta padronizada.

**Exemplo de Resposta de Erro**:
```json
{
  "error": "Ocorreu um erro interno no servidor.",
  "message": "Detalhes do erro",
  "timestamp": "2025-11-02T22:56:15.4067733Z"
}
```

## 9. Documentação Swagger

### Acesso

A documentação interativa está disponível em:
- **URL**: http://localhost:5000

### Funcionalidades Testadas

- ✅ Visualização de todos os endpoints
- ✅ Schemas de DTOs
- ✅ Autenticação JWT via interface
- ✅ Testes interativos de endpoints

## 10. Resumo dos Testes

### Funcionalidades Testadas

| Categoria | Testes | Status |
|-----------|--------|--------|
| Autenticação | 2 | ✅ Passou |
| Usuários | 1 | ✅ Passou |
| Jogos | 2 | ✅ Passou |
| Biblioteca | 2 | ✅ Passou |
| Validações | 3 | ✅ Passou |
| Autorização | 2 | ✅ Passou |
| Testes Unitários | 33 | ✅ Passou |
| **Total** | **45** | **✅ 100%** |

### Requisitos Atendidos

- ✅ Cadastro de usuários com validações
- ✅ Autenticação JWT
- ✅ Dois níveis de acesso (User/Admin)
- ✅ CRUD de jogos
- ✅ Biblioteca de jogos
- ✅ Sistema de promoções
- ✅ Middleware de erro
- ✅ Documentação Swagger
- ✅ Testes unitários
- ✅ Entity Framework Core
- ✅ Migrations
- ✅ Arquitetura DDD

## Conclusão

Todos os testes foram executados com sucesso, validando que a API está funcionando conforme os requisitos especificados no Tech Challenge - Fase 1.

A API está pronta para ser demonstrada e entregue.
