# 🔐 Guia de Autenticação - Plataforma Seven API

## 📋 Visão Geral

A API utiliza **autenticação baseada em Cookies** com **Claims** para gerenciar sessões de usuários.

---

## 🎯 Endpoints de Autenticação

### 1. Login

**POST** `/api/Autenticacao/login`

Autentica o usuário e cria uma sessão com cookie.

**Request Body:**
```json
{
  "username": "admin",
  "password": "123456",
  "rememberMe": false
}
```

**Response (200 OK):**
```json
{
  "message": "Login realizado com sucesso",
  "user": {
    "id": 1,
    "username": "admin",
    "tipo": "Administrador"
  }
}
```

**Response (401 Unauthorized):**
```json
{
  "message": "Usuário ou senha inválidos"
}
```

---

### 2. Logout

**POST** `/api/Autenticacao/logout`

Encerra a sessão do usuário.

**Response (200 OK):**
```json
{
  "message": "Logout realizado com sucesso"
}
```

---

### 3. Verificar Autenticação

**GET** `/api/Autenticacao/check`

Verifica se o usuário está autenticado.

**Response (200 OK) - Autenticado:**
```json
{
  "authenticated": true,
  "user": {
    "id": "1",
    "username": "admin",
    "role": "Administrador"
  }
}
```

**Response (200 OK) - Não Autenticado:**
```json
{
  "authenticated": false
}
```

---

### 4. Obter Usuário Atual

**GET** `/api/Autenticacao/me`

Retorna informações do usuário autenticado.

**Response (200 OK):**
```json
{
  "id": 1,
  "username": "admin",
  "tipo": "Administrador",
  "dataCadastro": "2024-01-01T10:00:00",
  "dataAtualizacao": "2024-01-15T14:30:00"
}
```

**Response (401 Unauthorized):**
```json
{
  "message": "Usuário não autenticado"
}
```

---

### 5. Registrar Novo Usuário

**POST** `/api/Autenticacao/register`

Cria um novo usuário no sistema.

**Request Body:**
```json
{
  "user": "novo.usuario",
  "password": "senha123",
  "tipo": "Usuario",
  "userCadatro": "admin"
}
```

**Response (201 Created):**
```json
{
  "id": 4,
  "message": "Usuário registrado com sucesso"
}
```

**Response (400 Bad Request):**
```json
{
  "message": "Usuário já existe"
}
```

---

### 6. Alterar Senha

**POST** `/api/Autenticacao/change-password`

Altera a senha do usuário autenticado.

**Request Body:**
```json
{
  "currentPassword": "senha123",
  "newPassword": "novaSenha456",
  "confirmPassword": "novaSenha456"
}
```

**Response (200 OK):**
```json
{
  "message": "Senha alterada com sucesso"
}
```

**Response (400 Bad Request):**
```json
{
  "message": "Senha atual incorreta"
}
```

---

## 🔒 Protegendo Endpoints

### Requer Autenticação

Use o atributo `[Authorize]`:

```csharp
[Authorize]
[HttpGet]
public ActionResult GetProtectedData()
{
    return Ok("Dados protegidos");
}
```

### Requer Função Específica

Use `[Authorize(Roles = "...")]`:

```csharp
[Authorize(Roles = "Administrador")]
[HttpDelete("{id}")]
public ActionResult Delete(int id)
{
    // Apenas administradores podem deletar
    return Ok();
}
```

### Múltiplas Funções

```csharp
[Authorize(Roles = "Administrador,Supervisor")]
[HttpGet("relatorio")]
public ActionResult GetRelatorio()
{
    // Administradores OU Supervisores
    return Ok();
}
```

### Permitir Acesso Anônimo

```csharp
[AllowAnonymous]
[HttpGet("public")]
public ActionResult GetPublicData()
{
    return Ok("Dados públicos");
}
```

---

## 🎭 Claims Disponíveis

Após o login, os seguintes claims estão disponíveis:

| Claim | Tipo | Descrição |
|-------|------|-----------|
| `NameIdentifier` | `ClaimTypes.NameIdentifier` | ID do usuário |
| `Name` | `ClaimTypes.Name` | Nome de usuário |
| `Role` | `ClaimTypes.Role` | Tipo/Função do usuário |
| `UserId` | Custom | ID do usuário (duplicado) |

### Acessando Claims no Controller

```csharp
var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
var username = User.FindFirst(ClaimTypes.Name)?.Value;
var role = User.FindFirst(ClaimTypes.Role)?.Value;
```

---

## 🍪 Configuração de Cookies

### Propriedades do Cookie

- **Nome**: `PlataformaSeven.Auth`
- **HttpOnly**: `true` (não acessível via JavaScript)
- **Secure**: `true` (apenas HTTPS)
- **SameSite**: `Strict` (proteção CSRF)
- **Expiração**: 8 horas (padrão) ou 30 dias (com "Lembrar-me")
- **SlidingExpiration**: `true` (renova automaticamente)

### Configuração no Program.cs

```csharp
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.Name = "PlataformaSeven.Auth";
        options.Cookie.HttpOnly = true;
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
        options.ExpireTimeSpan = TimeSpan.FromHours(8);
        options.SlidingExpiration = true;
    });
```

---

## 🧪 Testando Autenticação

### 1. Login via cURL

```bash
curl -X POST "https://localhost:5001/api/Autenticacao/login" \
  -H "Content-Type: application/json" \
  -c cookies.txt \
  -d '{
    "username": "admin",
    "password": "123456",
    "rememberMe": false
  }'
```

### 2. Acessar Endpoint Protegido

```bash
curl -X GET "https://localhost:5001/api/ProtectedExample/authenticated" \
  -b cookies.txt
```

### 3. Logout

```bash
curl -X POST "https://localhost:5001/api/Autenticacao/logout" \
  -b cookies.txt
```

---

## 🔐 Testando no Swagger

### Passo 1: Fazer Login

1. Acesse `/swagger`
2. Vá em `POST /api/Autenticacao/login`
3. Clique em "Try it out"
4. Preencha:
```json
{
  "username": "admin",
  "password": "123456",
  "rememberMe": false
}
```
5. Execute

### Passo 2: Acessar Endpoints Protegidos

Após o login, o cookie é armazenado automaticamente pelo navegador.

1. Vá em qualquer endpoint protegido
2. Execute normalmente
3. O cookie será enviado automaticamente

### Passo 3: Verificar Autenticação

1. Vá em `GET /api/Autenticacao/check`
2. Execute
3. Deve retornar `authenticated: true`

---

## 👥 Tipos de Usuário

Os usuários podem ter os seguintes tipos:

- **Administrador** - Acesso total
- **Supervisor** - Acesso intermediário
- **Usuario** - Acesso básico

### Dados de Exemplo

O script SQL cria 3 usuários:

| Username | Password | Tipo |
|----------|----------|------|
| admin | 123456 | Administrador |
| user1 | 123456 | Usuario |
| supervisor1 | 123456 | Supervisor |

---

## 🛡️ Segurança

### Boas Práticas Implementadas

✅ **HttpOnly Cookies** - Protege contra XSS
✅ **Secure Cookies** - Apenas HTTPS
✅ **SameSite Strict** - Proteção CSRF
✅ **Sliding Expiration** - Renova sessão automaticamente
✅ **Claims-based** - Autorização granular

### Melhorias Recomendadas para Produção

1. **Hash de Senhas** - Use BCrypt ou Argon2
```csharp
// Instalar: dotnet add package BCrypt.Net-Next
var hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);
var isValid = BCrypt.Net.BCrypt.Verify(password, hashedPassword);
```

2. **Rate Limiting** - Limite tentativas de login
3. **Two-Factor Authentication** - Autenticação de dois fatores
4. **Refresh Tokens** - Para sessões longas
5. **Audit Log** - Registrar logins e ações

---

## 📊 Fluxo de Autenticação

```
1. Cliente envia credenciais
   ↓
2. API valida no banco de dados
   ↓
3. Cria Claims (NameIdentifier, Name, Role)
   ↓
4. Cria ClaimsIdentity e ClaimsPrincipal
   ↓
5. Chama HttpContext.SignInAsync()
   ↓
6. Cookie é criado e enviado ao cliente
   ↓
7. Cliente armazena cookie
   ↓
8. Requisições futuras incluem cookie automaticamente
   ↓
9. API valida cookie e autoriza acesso
```

---

## ❌ Erros Comuns

### Erro 401 - Unauthorized

**Causa:** Usuário não autenticado ou cookie expirado

**Solução:** Fazer login novamente

### Erro 403 - Forbidden

**Causa:** Usuário autenticado mas sem permissão

**Solução:** Verificar role do usuário

### Cookie não é enviado

**Causa:** CORS ou SameSite

**Solução:** Configurar CORS corretamente:
```csharp
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        builder => builder
            .WithOrigins("https://seudominio.com")
            .AllowCredentials() // IMPORTANTE!
            .AllowAnyMethod()
            .AllowAnyHeader());
});
```

---

## 🔧 Exemplo Completo de Uso

### Frontend (JavaScript/Fetch)

```javascript
// Login
async function login(username, password) {
    const response = await fetch('https://localhost:5001/api/Autenticacao/login', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        credentials: 'include', // IMPORTANTE!
        body: JSON.stringify({ username, password, rememberMe: false })
    });
    
    const data = await response.json();
    console.log(data);
}

// Acessar endpoint protegido
async function getProtectedData() {
    const response = await fetch('https://localhost:5001/api/ProtectedExample/authenticated', {
        credentials: 'include' // IMPORTANTE!
    });
    
    const data = await response.json();
    console.log(data);
}

// Logout
async function logout() {
    const response = await fetch('https://localhost:5001/api/Autenticacao/logout', {
        method: 'POST',
        credentials: 'include' // IMPORTANTE!
    });
    
    const data = await response.json();
    console.log(data);
}
```

---

**Autenticação configurada e pronta para uso! 🔐**

