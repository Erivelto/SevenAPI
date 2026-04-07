# 🔐 Autenticação JWT - Plataforma Seven API

## 📋 Visão Geral

A API utiliza **autenticação baseada em JWT (JSON Web Token)** para proteger endpoints e gerenciar sessões de usuários.

---

## 🎯 Endpoints de Autenticação

### 1. Login

**POST** `/api/Autenticacao/login`

Autentica o usuário e retorna um token JWT.

**Request Body:**
```json
{
  "user": "admin",
  "password": "123456"
}
```

**Response (200 OK):**
```json
{
  "success": true,
  "message": "Login realizado com sucesso",
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "refreshToken": "abcd1234...",
  "expiresAt": "2024-01-15T22:00:00Z",
  "user": {
    "id": 1,
    "user": "admin",
    "tipo": "Administrador",
    "dataCadastro": "2024-01-01T10:00:00"
  }
}
```

**Response (401 Unauthorized):**
```json
{
  "success": false,
  "message": "Usuário ou senha inválidos"
}
```

---

### 2. Registrar Novo Usuário

**POST** `/api/Autenticacao/register`

Cria um novo usuário e retorna um token JWT.

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
  "success": true,
  "message": "Usuário registrado com sucesso",
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "refreshToken": "xyz789...",
  "expiresAt": "2024-01-15T22:00:00Z",
  "user": {
    "id": 4,
    "user": "novo.usuario",
    "tipo": "Usuario",
    "dataCadastro": "2024-01-15T14:00:00"
  }
}
```

---

### 3. Obter Usuário Atual

**GET** `/api/Autenticacao/me`

**Requer autenticação** - Retorna informações do usuário autenticado.

**Headers:**
```
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```

**Response (200 OK):**
```json
{
  "id": 1,
  "user": "admin",
  "tipo": "Administrador",
  "dataCadastro": "2024-01-01T10:00:00"
}
```

---

### 4. Alterar Senha

**POST** `/api/Autenticacao/change-password`

**Requer autenticação** - Altera a senha do usuário autenticado.

**Headers:**
```
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```

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
  "success": true,
  "message": "Senha alterada com sucesso"
}
```

---

### 5. Renovar Token (Refresh)

**POST** `/api/Autenticacao/refresh-token`

Renova o token JWT usando o refresh token.

**Request Body:**
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "refreshToken": "abcd1234..."
}
```

**Response (200 OK):**
```json
{
  "success": true,
  "message": "Token renovado com sucesso",
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "refreshToken": "xyz789...",
  "expiresAt": "2024-01-16T06:00:00Z",
  "user": {
    "id": 1,
    "user": "admin",
    "tipo": "Administrador",
    "dataCadastro": "2024-01-01T10:00:00"
  }
}
```

---

### 6. Validar Token

**GET** `/api/Autenticacao/validate`

**Requer autenticação** - Valida se o token JWT é válido.

**Headers:**
```
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```

**Response (200 OK):**
```json
{
  "valid": true,
  "user": {
    "id": "1",
    "username": "admin",
    "role": "Administrador"
  }
}
```

---

## 🔒 Protegendo Endpoints

### Requer Autenticação

Use o atributo `[Authorize]`:

```csharp
[Authorize]
[HttpGet]
public async Task<ActionResult> GetProtectedData()
{
    return Ok("Dados protegidos");
}
```

### Requer Função Específica

Use `[Authorize(Roles = "...")]`:

```csharp
[Authorize(Roles = "Administrador")]
[HttpDelete("{id}")]
public async Task<ActionResult> Delete(int id)
{
    // Apenas administradores podem deletar
    return Ok();
}
```

### Múltiplas Funções

```csharp
[Authorize(Roles = "Administrador,Supervisor")]
[HttpGet("relatorio")]
public async Task<ActionResult> GetRelatorio()
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

## 🎭 Claims no Token JWT

O token JWT contém os seguintes claims:

| Claim | Tipo | Descrição |
|-------|------|-----------|
| `sub` | `JwtRegisteredClaimNames.Sub` | ID do usuário |
| `unique_name` | `JwtRegisteredClaimNames.UniqueName` | Nome de usuário |
| `nameid` | `ClaimTypes.NameIdentifier` | ID do usuário |
| `name` | `ClaimTypes.Name` | Nome de usuário |
| `role` | `ClaimTypes.Role` | Tipo/Função do usuário |
| `UserId` | Custom | ID do usuário |
| `Tipo` | Custom | Tipo do usuário |
| `jti` | `JwtRegisteredClaimNames.Jti` | ID único do token |
| `iat` | `JwtRegisteredClaimNames.Iat` | Data de emissão |

### Acessando Claims no Controller

```csharp
var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
var username = User.FindFirst(ClaimTypes.Name)?.Value;
var role = User.FindFirst(ClaimTypes.Role)?.Value;
```

---

## ⚙️ Configuração JWT

### appsettings.json

```json
{
  "JwtSettings": {
    "SecretKey": "PlataformaSeven_SuperSecretKey_2024_MinLength32Characters!",
    "Issuer": "PlataformaSeven.API",
    "Audience": "PlataformaSeven.Client",
    "ExpirationInMinutes": 480,
    "RefreshTokenExpirationInDays": 7
  }
}
```

| Propriedade | Valor | Descrição |
|-------------|-------|-----------|
| **SecretKey** | String (min 32 chars) | Chave secreta para assinar o token |
| **Issuer** | `PlataformaSeven.API` | Emissor do token |
| **Audience** | `PlataformaSeven.Client` | Destinatário do token |
| **ExpirationInMinutes** | 480 (8 horas) | Tempo de expiração do token |
| **RefreshTokenExpirationInDays** | 7 dias | Tempo de expiração do refresh token |

---

## 🧪 Testando com Swagger

### Passo 1: Fazer Login

1. Acesse `/swagger`
2. Vá em `POST /api/Autenticacao/login`
3. Clique em "Try it out"
4. Preencha:
```json
{
  "user": "admin",
  "password": "123456"
}
```
5. Execute
6. **Copie o token** da resposta

### Passo 2: Autorizar no Swagger

1. Clique no botão **"Authorize"** (cadeado) no topo
2. Cole o token no formato:
```
Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```
3. Clique em "Authorize"
4. Clique em "Close"

### Passo 3: Acessar Endpoints Protegidos

Agora todos os endpoints protegidos funcionarão automaticamente!

---

## 🧪 Testando com cURL

### 1. Login

```bash
curl -X POST "https://localhost:5001/api/Autenticacao/login" \
  -H "Content-Type: application/json" \
  -d '{
    "user": "admin",
    "password": "123456"
  }'
```

**Copie o token da resposta**

### 2. Acessar Endpoint Protegido

```bash
curl -X GET "https://localhost:5001/api/Colaborador" \
  -H "Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
```

### 3. Obter Usuário Atual

```bash
curl -X GET "https://localhost:5001/api/Autenticacao/me" \
  -H "Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
```

---

## 🌐 Testando com JavaScript/Fetch

### Login

```javascript
async function login(user, password) {
    const response = await fetch('https://localhost:5001/api/Autenticacao/login', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ user, password })
    });
    
    const data = await response.json();
    
    if (data.success) {
        // Armazenar token
        localStorage.setItem('token', data.token);
        localStorage.setItem('refreshToken', data.refreshToken);
        console.log('Login bem-sucedido!', data.user);
    }
    
    return data;
}
```

### Acessar Endpoint Protegido

```javascript
async function getColaboradores() {
    const token = localStorage.getItem('token');
    
    const response = await fetch('https://localhost:5001/api/Colaborador', {
        headers: {
            'Authorization': `Bearer ${token}`
        }
    });
    
    if (response.status === 401) {
        // Token expirado, fazer refresh
        await refreshToken();
        return getColaboradores(); // Tentar novamente
    }
    
    const data = await response.json();
    return data;
}
```

### Renovar Token

```javascript
async function refreshToken() {
    const token = localStorage.getItem('token');
    const refreshToken = localStorage.getItem('refreshToken');
    
    const response = await fetch('https://localhost:5001/api/Autenticacao/refresh-token', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ token, refreshToken })
    });
    
    const data = await response.json();
    
    if (data.success) {
        localStorage.setItem('token', data.token);
        localStorage.setItem('refreshToken', data.refreshToken);
    }
    
    return data;
}
```

### Logout

```javascript
function logout() {
    localStorage.removeItem('token');
    localStorage.removeItem('refreshToken');
    console.log('Logout realizado');
}
```

---

## 👥 Tipos de Usuário

| Tipo | Permissões |
|------|------------|
| **Administrador** | Acesso total a todos os endpoints |
| **Supervisor** | Acesso intermediário |
| **Usuario** | Acesso básico |

### Dados de Exemplo

| Username | Password | Tipo |
|----------|----------|------|
| admin | 123456 | Administrador |
| user1 | 123456 | Usuario |
| supervisor1 | 123456 | Supervisor |

---

## 🛡️ Segurança

### Boas Práticas Implementadas

✅ **HTTPS Only** - Tokens só devem ser enviados via HTTPS
✅ **Token Expiration** - Tokens expiram em 8 horas
✅ **Refresh Token** - Permite renovar sessão sem re-login
✅ **Claims-based** - Autorização granular
✅ **HMAC SHA256** - Algoritmo de assinatura seguro

### Melhorias Recomendadas para Produção

1. **Hash de Senhas** - Use BCrypt ou Argon2
```bash
dotnet add package BCrypt.Net-Next
```

```csharp
// Ao criar usuário
usuario.Password = BCrypt.Net.BCrypt.HashPassword(password);

// Ao validar login
if (!BCrypt.Net.BCrypt.Verify(request.Password, usuario.Password))
{
    return Unauthorized();
}
```

2. **Armazenar Refresh Tokens no Banco**
   - Criar tabela RefreshTokens
   - Validar refresh token no banco
   - Revogar tokens ao fazer logout

3. **Rate Limiting** - Limite tentativas de login

4. **HTTPS Obrigatório** - Produção deve usar apenas HTTPS

5. **Rotação de Chaves** - Trocar SecretKey periodicamente

---

## 📊 Estrutura do Token JWT

### Header
```json
{
  "alg": "HS256",
  "typ": "JWT"
}
```

### Payload
```json
{
  "sub": "1",
  "unique_name": "admin",
  "nameid": "1",
  "name": "admin",
  "role": "Administrador",
  "UserId": "1",
  "Tipo": "Administrador",
  "jti": "abc-123-def-456",
  "iat": "1705334400",
  "exp": "1705363200",
  "iss": "PlataformaSeven.API",
  "aud": "PlataformaSeven.Client"
}
```

### Signature
```
HMACSHA256(
  base64UrlEncode(header) + "." +
  base64UrlEncode(payload),
  secret
)
```

---

## ❌ Erros Comuns

### Erro 401 - Unauthorized

**Causa:** Token inválido, expirado ou ausente

**Solução:**
1. Verificar se o token está sendo enviado no header
2. Verificar formato: `Authorization: Bearer {token}`
3. Fazer login novamente ou usar refresh token

### Erro 403 - Forbidden

**Causa:** Usuário autenticado mas sem permissão (role incorreta)

**Solução:** Verificar role do usuário

### Token não é aceito

**Causa:** SecretKey, Issuer ou Audience incorretos

**Solução:** Verificar configuração em `appsettings.json`

---

## 🔄 Fluxo de Autenticação

```
1. Cliente envia credenciais (POST /login)
   ↓
2. API valida no banco de dados
   ↓
3. API gera token JWT com claims
   ↓
4. API retorna token + refresh token
   ↓
5. Cliente armazena tokens (localStorage)
   ↓
6. Cliente envia token em cada requisição
   (Header: Authorization: Bearer {token})
   ↓
7. API valida token automaticamente
   ↓
8. Se válido, autoriza acesso
   ↓
9. Se expirado, cliente usa refresh token
   ↓
10. API gera novos tokens
```

---

## 🎯 Exemplo Completo de Integração

### React/TypeScript

```typescript
// services/auth.service.ts
import axios from 'axios';

const API_URL = 'https://localhost:5001/api/Autenticacao';

export interface LoginRequest {
  user: string;
  password: string;
}

export interface LoginResponse {
  success: boolean;
  message: string;
  token?: string;
  refreshToken?: string;
  expiresAt?: string;
  user?: {
    id: number;
    user: string;
    tipo: string;
  };
}

class AuthService {
  async login(user: string, password: string): Promise<LoginResponse> {
    const response = await axios.post<LoginResponse>(`${API_URL}/login`, {
      user,
      password
    });
    
    if (response.data.success && response.data.token) {
      localStorage.setItem('token', response.data.token);
      localStorage.setItem('refreshToken', response.data.refreshToken!);
    }
    
    return response.data;
  }

  logout() {
    localStorage.removeItem('token');
    localStorage.removeItem('refreshToken');
  }

  getToken(): string | null {
    return localStorage.getItem('token');
  }

  async refreshToken(): Promise<boolean> {
    const token = this.getToken();
    const refreshToken = localStorage.getItem('refreshToken');
    
    if (!token || !refreshToken) return false;
    
    try {
      const response = await axios.post<LoginResponse>(`${API_URL}/refresh-token`, {
        token,
        refreshToken
      });
      
      if (response.data.success && response.data.token) {
        localStorage.setItem('token', response.data.token);
        localStorage.setItem('refreshToken', response.data.refreshToken!);
        return true;
      }
    } catch {
      this.logout();
    }
    
    return false;
  }
}

export default new AuthService();
```

```typescript
// services/api.service.ts
import axios from 'axios';
import authService from './auth.service';

const api = axios.create({
  baseURL: 'https://localhost:5001/api'
});

// Interceptor para adicionar token
api.interceptors.request.use(
  (config) => {
    const token = authService.getToken();
    if (token) {
      config.headers.Authorization = `Bearer ${token}`;
    }
    return config;
  },
  (error) => Promise.reject(error)
);

// Interceptor para refresh token
api.interceptors.response.use(
  (response) => response,
  async (error) => {
    const originalRequest = error.config;
    
    if (error.response?.status === 401 && !originalRequest._retry) {
      originalRequest._retry = true;
      
      const refreshed = await authService.refreshToken();
      
      if (refreshed) {
        const token = authService.getToken();
        originalRequest.headers.Authorization = `Bearer ${token}`;
        return api(originalRequest);
      }
    }
    
    return Promise.reject(error);
  }
);

export default api;
```

---

**Autenticação JWT configurada e pronta para uso! 🔐**

