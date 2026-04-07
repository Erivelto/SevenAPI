# 🚀 Guia de Início Rápido - Plataforma Seven API

## ⚡ Começando em 5 Minutos

### 1️⃣ Configurar Banco de Dados

**Opção A: SQL Server Local**

1. Abra o SQL Server Management Studio (SSMS)
2. Conecte ao servidor local
3. Abra o arquivo `Data/DatabaseScript.sql`
4. Execute o script completo (F5)

**Opção B: Azure SQL Database**

1. Crie um Azure SQL Database
2. Copie a connection string
3. Execute o script `Data/DatabaseScript.sql`

### 2️⃣ Configurar Connection String

Edite `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=PlataformaSeven;User Id=sa;Password=SuaSenha123;TrustServerCertificate=True;"
  }
}
```

**Exemplos de Connection String:**

**SQL Server Local:**
```
Server=localhost;Database=PlataformaSeven;User Id=sa;Password=SuaSenha123;TrustServerCertificate=True;
```

**SQL Server com Windows Authentication:**
```
Server=localhost;Database=PlataformaSeven;Integrated Security=True;TrustServerCertificate=True;
```

**Azure SQL:**
```
Server=tcp:seuservidor.database.windows.net,1433;Database=PlataformaSeven;User ID=usuario;Password=senha;Encrypt=True;
```

### 3️⃣ Restaurar Pacotes

```bash
dotnet restore
```

### 4️⃣ Executar a API

```bash
dotnet run
```

### 5️⃣ Testar

Acesse: **https://localhost:5001/swagger**

---

## 📋 Checklist de Instalação

- [ ] .NET 8 SDK instalado
- [ ] SQL Server instalado e rodando
- [ ] Banco de dados criado (script executado)
- [ ] Connection string configurada
- [ ] Pacotes restaurados
- [ ] API executando
- [ ] Swagger acessível

---

## 🧪 Testando os Endpoints

### 1. Listar Funções

```bash
curl -X GET "https://localhost:5001/api/Funcao"
```

**Resposta esperada:**
```json
[
  { "id": 1, "nome": "Vigilante" },
  { "id": 2, "nome": "Porteiro" },
  ...
]
```

### 2. Criar Colaborador

```bash
curl -X POST "https://localhost:5001/api/Colaborador" \
  -H "Content-Type: application/json" \
  -d '{
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
  }'
```

### 3. Criar Detalhe do Colaborador

```bash
curl -X POST "https://localhost:5001/api/ColaboradorDetalhe" \
  -H "Content-Type: application/json" \
  -d '{
    "idColaborador": 1,
    "valorDiaria": 150.00,
    "idFuncao": 1,
    "idSupervisor": 1,
    "idPosto": 1
  }'
```

### 4. Registrar Diária

```bash
curl -X POST "https://localhost:5001/api/Diaria" \
  -H "Content-Type: application/json" \
  -d '{
    "idColaboradorDetalhe": 1,
    "dataDiaria": "2024-01-15T00:00:00",
    "userCadastro": "admin"
  }'
```

---

## 🔍 Verificando se Está Funcionando

### Teste 1: API está rodando?

Acesse: http://localhost:5000/api/Funcao

**Esperado:** Lista de funções em JSON

### Teste 2: Swagger está acessível?

Acesse: https://localhost:5001/swagger

**Esperado:** Interface do Swagger com todos os endpoints

### Teste 3: Banco de dados conectado?

Execute qualquer GET endpoint no Swagger

**Esperado:** Dados retornados (pode ser array vazio se não houver dados)

---

## ❌ Problemas Comuns

### Erro: "Cannot open database"

**Solução:** Verifique se o banco foi criado executando o script SQL

### Erro: "Login failed for user"

**Solução:** Verifique usuário e senha na connection string

### Erro: "Unable to resolve service"

**Solução:** Verifique se todos os serviços estão registrados em `Program.cs`

### Erro: "Port already in use"

**Solução:** Mude a porta em `launchSettings.json` ou mate o processo:

```bash
# Windows
netstat -ano | findstr :5000
taskkill /PID <PID> /F

# Linux/Mac
lsof -i :5000
kill -9 <PID>
```

---

## 📊 Dados de Exemplo

O script SQL já cria dados de exemplo:

- **5 Funções** (Vigilante, Porteiro, etc.)
- **5 Postos** (Posto A, B, C, D, E)
- **4 Supervisores** (João, Maria, Pedro, Ana)
- **3 Usuários** (admin, user1, supervisor1)
- **3 Colaboradores** com detalhes
- **4 Diárias** registradas

---

## 🎯 Próximos Passos

1. ✅ Testar todos os endpoints no Swagger
2. ✅ Criar seus próprios colaboradores
3. ✅ Registrar diárias
4. ✅ Implementar autenticação JWT (opcional)
5. ✅ Adicionar validações customizadas
6. ✅ Criar relatórios

---

## 📞 Precisa de Ajuda?

1. Verifique o `README.md` para documentação completa
2. Consulte os logs da aplicação
3. Teste os endpoints no Swagger
4. Verifique a connection string

---

**Pronto! Sua API está funcionando! 🎉**

