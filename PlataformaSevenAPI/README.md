# Plataforma Seven API

API RESTful completa desenvolvida em **C# com .NET 8** e **Dapper** para gerenciamento de colaboradores, funções, postos, supervisores, usuários e diárias.

## 🚀 Tecnologias Utilizadas

- **.NET 8.0** - Framework principal
- **Dapper** - Micro ORM para acesso a dados
- **SQL Server** - Banco de dados
- **Swagger/OpenAPI** - Documentação da API
- **ASP.NET Core Web API** - Framework web

## 📋 Funcionalidades

### Entidades com CRUD Completo

1. **Colaborador** - Gerenciamento de colaboradores
2. **ColaboradorDetalhe** - Detalhes dos colaboradores (função, posto, supervisor, valor diária)
3. **Funcao** - Funções dos colaboradores
4. **Posto** - Postos de trabalho
5. **Supervisor** - Supervisores
6. **Usuario** - Usuários do sistema
7. **Diaria** - Registro de diárias

## 🏗️ Arquitetura

O projeto segue uma arquitetura em camadas:

```
PlataformaSeven.API/
├── Controllers/       # Endpoints da API
├── Services/          # Lógica de negócio
├── Repositories/      # Acesso a dados com Dapper
├── Models/            # Entidades do domínio
├── Data/              # Contexto e scripts SQL
├── DTOs/              # Data Transfer Objects (opcional)
└── Program.cs         # Configuração da aplicação
```

## 📦 Instalação e Configuração

### Pré-requisitos

- .NET 8.0 SDK
- SQL Server (2019 ou superior)
- Visual Studio 2022 ou VS Code

### Passo 1: Restaurar Pacotes

```bash
dotnet restore
```

### Passo 2: Configurar Connection String

Edite o arquivo `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=SEU_SERVIDOR;Database=PlataformaSeven;User Id=SEU_USUARIO;Password=SUA_SENHA;TrustServerCertificate=True;"
  }
}
```

### Passo 3: Criar o Banco de Dados

Execute o script SQL localizado em `Data/DatabaseScript.sql` no SQL Server Management Studio ou Azure Data Studio.

```sql
-- O script cria:
-- - Banco de dados PlataformaSeven
-- - Todas as tabelas
-- - Relacionamentos (Foreign Keys)
-- - Dados de exemplo
```

### Passo 4: Executar a API

```bash
dotnet run
```

A API estará disponível em:
- **HTTP**: http://localhost:5000
- **HTTPS**: https://localhost:5001
- **Swagger**: https://localhost:5001/swagger

## 📚 Documentação da API

### Colaborador

| Método | Endpoint | Descrição |
|--------|----------|-----------|
| GET | `/api/Colaborador` | Lista todos os colaboradores |
| GET | `/api/Colaborador/{id}` | Busca colaborador por ID |
| POST | `/api/Colaborador` | Cria novo colaborador |
| PUT | `/api/Colaborador/{id}` | Atualiza colaborador |
| DELETE | `/api/Colaborador/{id}` | Exclui colaborador (soft delete) |

**Exemplo de Request (POST):**

```json
{
  "nome": "João Silva",
  "pix": "11987654321",
  "referencia": "REF001",
  "endereco": "Rua das Flores",
  "numero": "100",
  "bairro": "Centro",
  "cidade": "São Paulo",
  "uf": "SP",
  "cep": "01000-000",
  "userCad": "admin"
}
```

### ColaboradorDetalhe

| Método | Endpoint | Descrição |
|--------|----------|-----------|
| GET | `/api/ColaboradorDetalhe` | Lista todos os detalhes |
| GET | `/api/ColaboradorDetalhe/{id}` | Busca por ID |
| GET | `/api/ColaboradorDetalhe/colaborador/{idColaborador}` | Busca por colaborador |
| POST | `/api/ColaboradorDetalhe` | Cria novo detalhe |
| PUT | `/api/ColaboradorDetalhe/{id}` | Atualiza detalhe |
| DELETE | `/api/ColaboradorDetalhe/{id}` | Exclui detalhe |

**Exemplo de Request (POST):**

```json
{
  "idColaborador": 1,
  "valorDiaria": 150.00,
  "idFuncao": 1,
  "idSupervisor": 1,
  "idPosto": 1
}
```

### Funcao

| Método | Endpoint | Descrição |
|--------|----------|-----------|
| GET | `/api/Funcao` | Lista todas as funções |
| GET | `/api/Funcao/{id}` | Busca função por ID |
| POST | `/api/Funcao` | Cria nova função |
| PUT | `/api/Funcao/{id}` | Atualiza função |
| DELETE | `/api/Funcao/{id}` | Exclui função |

### Posto

| Método | Endpoint | Descrição |
|--------|----------|-----------|
| GET | `/api/Posto` | Lista todos os postos |
| GET | `/api/Posto/{id}` | Busca posto por ID |
| POST | `/api/Posto` | Cria novo posto |
| PUT | `/api/Posto/{id}` | Atualiza posto |
| DELETE | `/api/Posto/{id}` | Exclui posto |

### Supervisor

| Método | Endpoint | Descrição |
|--------|----------|-----------|
| GET | `/api/Supervisor` | Lista todos os supervisores |
| GET | `/api/Supervisor/{id}` | Busca supervisor por ID |
| POST | `/api/Supervisor` | Cria novo supervisor |
| PUT | `/api/Supervisor/{id}` | Atualiza supervisor |
| DELETE | `/api/Supervisor/{id}` | Exclui supervisor |

### Usuario

| Método | Endpoint | Descrição |
|--------|----------|-----------|
| GET | `/api/Usuario` | Lista todos os usuários |
| GET | `/api/Usuario/{id}` | Busca usuário por ID |
| GET | `/api/Usuario/username/{username}` | Busca por username |
| POST | `/api/Usuario` | Cria novo usuário |
| PUT | `/api/Usuario/{id}` | Atualiza usuário |
| DELETE | `/api/Usuario/{id}` | Exclui usuário |

**Exemplo de Request (POST):**

```json
{
  "user": "joao.silva",
  "password": "senha123",
  "tipo": "Usuario",
  "userCadatro": "admin"
}
```

### Diaria

| Método | Endpoint | Descrição |
|--------|----------|-----------|
| GET | `/api/Diaria` | Lista todas as diárias |
| GET | `/api/Diaria/{id}` | Busca diária por ID |
| GET | `/api/Diaria/colaborador-detalhe/{id}` | Busca por colaborador detalhe |
| GET | `/api/Diaria/periodo?startDate=2024-01-01&endDate=2024-12-31` | Busca por período |
| POST | `/api/Diaria` | Cria nova diária |
| PUT | `/api/Diaria/{id}` | Atualiza diária |
| DELETE | `/api/Diaria/{id}` | Exclui diária |

**Exemplo de Request (POST):**

```json
{
  "idColaboradorDetalhe": 1,
  "dataDiaria": "2024-01-15T00:00:00",
  "userCadastro": "admin"
}
```

## 🔧 Configurações Adicionais

### CORS

A API está configurada para aceitar requisições de qualquer origem. Para produção, ajuste em `Program.cs`:

```csharp
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder =>
        {
            builder.WithOrigins("https://seudominio.com")
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});
```

### Logging

Os logs são configurados em `appsettings.json`:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  }
}
```

## 🧪 Testando a API

### Usando Swagger

1. Execute a aplicação
2. Acesse https://localhost:5001/swagger
3. Teste os endpoints diretamente na interface

### Usando cURL

```bash
# Listar colaboradores
curl -X GET "https://localhost:5001/api/Colaborador" -H "accept: application/json"

# Criar colaborador
curl -X POST "https://localhost:5001/api/Colaborador" \
  -H "Content-Type: application/json" \
  -d '{
    "nome": "Maria Santos",
    "pix": "11976543210",
    "userCad": "admin"
  }'
```

### Usando Postman

Importe a coleção do Swagger:
1. Acesse `/swagger/v1/swagger.json`
2. Importe no Postman
3. Configure a base URL

## 📊 Estrutura do Banco de Dados

### Diagrama de Relacionamentos

```
Usuario
  |
Colaborador
  |
  └── ColaboradorDetalhe
        ├── Funcao
        ├── Supervisor
        ├── Posto
        └── Diaria
```

### Principais Tabelas

- **Colaborador**: Dados pessoais e endereço
- **ColaboradorDetalhe**: Vínculo com função, posto e supervisor + valor diária
- **Diaria**: Registro de diárias trabalhadas
- **Funcao, Posto, Supervisor**: Tabelas de apoio
- **Usuario**: Controle de acesso

## 🛠️ Desenvolvimento

### Adicionar Nova Entidade

1. Criar Model em `/Models`
2. Criar Interface e Repository em `/Repositories`
3. Criar Interface e Service em `/Services`
4. Criar Controller em `/Controllers`
5. Registrar no `Program.cs`

### Padrões de Código

- **Repository Pattern**: Acesso a dados isolado
- **Service Layer**: Lógica de negócio
- **Dependency Injection**: Inversão de controle
- **Async/Await**: Operações assíncronas

## 📝 Notas Importantes

- **Soft Delete**: Colaboradores são marcados como excluídos (campo `Excluido`)
- **Timestamps**: Datas de cadastro e alteração são gerenciadas automaticamente
- **Validações**: Implementadas via Data Annotations
- **Segurança**: Implemente autenticação JWT para produção

## 🚀 Deploy

### Azure App Service

```bash
dotnet publish -c Release
# Fazer deploy via Azure Portal ou CLI
```

### Docker

```dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY ./publish .
ENTRYPOINT ["dotnet", "PlataformaSeven.API.dll"]
```

## 📄 Licença

Este projeto é de uso interno da Plataforma Seven.

## 👥 Suporte

Para dúvidas ou problemas, entre em contato com a equipe de desenvolvimento.

---

**Desenvolvido com ❤️ usando .NET 8 e Dapper**

