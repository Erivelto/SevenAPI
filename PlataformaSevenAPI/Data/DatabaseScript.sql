-- ============================================
-- Script de Criação do Banco de Dados
-- Plataforma Seven API
-- ============================================

USE master;
GO

-- Criar banco de dados
IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'PlataformaSeven')
BEGIN
    CREATE DATABASE PlataformaSeven;
END
GO

USE PlataformaSeven;
GO

-- ============================================
-- Tabela: Funcao
-- ============================================
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Funcao')
BEGIN
    CREATE TABLE Funcao (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        Nome NVARCHAR(150) NOT NULL
    );
END
GO

-- ============================================
-- Tabela: Posto
-- ============================================
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Posto')
BEGIN
    CREATE TABLE Posto (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        Nome NVARCHAR(150) NOT NULL
    );
END
GO

-- ============================================
-- Tabela: Supervisor
-- ============================================
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Supervisor')
BEGIN
    CREATE TABLE Supervisor (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        Nome NVARCHAR(150) NOT NULL
    );
END
GO

-- ============================================
-- Tabela: Usuario
-- ============================================
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Usuario')
BEGIN
    CREATE TABLE Usuario (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        [User] NVARCHAR(100) NOT NULL UNIQUE,
        [Password] NVARCHAR(255) NOT NULL,
        DataCadastro DATETIME NULL,
        DataAtualizacao DATETIME NULL,
        UserCadatro NVARCHAR(100) NULL,
        Tipo NVARCHAR(50) NOT NULL
    );
END
GO

-- ============================================
-- Tabela: Colaborador
-- ============================================
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Colaborador')
BEGIN
    CREATE TABLE Colaborador (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        Nome NVARCHAR(150) NOT NULL,
        Pix NVARCHAR(100) NULL,
        Referencia NVARCHAR(100) NULL,
        Endereco NVARCHAR(250) NULL,
        Numero NVARCHAR(10) NULL,
        Complemento NVARCHAR(30) NULL,
        Bairro NVARCHAR(30) NULL,
        Cidade NVARCHAR(30) NULL,
        UF NVARCHAR(2) NULL,
        CEP NVARCHAR(10) NULL,
        DataCadastro DATETIME NOT NULL DEFAULT GETDATE(),
        DataAlteracao DATETIME NOT NULL DEFAULT GETDATE(),
        UserCad NVARCHAR(100) NULL,
        UserAlt NVARCHAR(100) NULL,
        Excluido BIT NOT NULL DEFAULT 0
    );
END
GO

-- ============================================
-- Tabela: ColaboradorDetalhe
-- ============================================
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'ColaboradorDetalhe')
BEGIN
    CREATE TABLE ColaboradorDetalhe (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        IdColaborador INT NOT NULL,
        ValorDiaria DECIMAL(18,2) NOT NULL,
        IdFuncao INT NOT NULL,
        IdSupervisor INT NOT NULL,
        IdPosto INT NOT NULL,
        CONSTRAINT FK_ColaboradorDetalhe_Colaborador FOREIGN KEY (IdColaborador) REFERENCES Colaborador(Id),
        CONSTRAINT FK_ColaboradorDetalhe_Funcao FOREIGN KEY (IdFuncao) REFERENCES Funcao(Id),
        CONSTRAINT FK_ColaboradorDetalhe_Supervisor FOREIGN KEY (IdSupervisor) REFERENCES Supervisor(Id),
        CONSTRAINT FK_ColaboradorDetalhe_Posto FOREIGN KEY (IdPosto) REFERENCES Posto(Id)
    );
END
GO

-- ============================================
-- Tabela: Diaria
-- ============================================
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Diaria')
BEGIN
    CREATE TABLE Diaria (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        IdColaboradorDetalhe INT NOT NULL,
        DataDiaria DATETIME NOT NULL,
        UserCadastro NVARCHAR(100) NULL,
        CONSTRAINT FK_Diaria_ColaboradorDetalhe FOREIGN KEY (IdColaboradorDetalhe) REFERENCES ColaboradorDetalhe(Id)
    );
END
GO

-- ============================================
-- Dados de Exemplo
-- ============================================

-- Inserir Funções
IF NOT EXISTS (SELECT * FROM Funcao)
BEGIN
    INSERT INTO Funcao (Nome) VALUES 
    ('Vigilante'),
    ('Porteiro'),
    ('Recepcionista'),
    ('Supervisor de Segurança'),
    ('Coordenador');
END
GO

-- Inserir Postos
IF NOT EXISTS (SELECT * FROM Posto)
BEGIN
    INSERT INTO Posto (Nome) VALUES 
    ('Posto A - Centro'),
    ('Posto B - Zona Norte'),
    ('Posto C - Zona Sul'),
    ('Posto D - Zona Leste'),
    ('Posto E - Zona Oeste');
END
GO

-- Inserir Supervisores
IF NOT EXISTS (SELECT * FROM Supervisor)
BEGIN
    INSERT INTO Supervisor (Nome) VALUES 
    ('João Silva'),
    ('Maria Santos'),
    ('Pedro Oliveira'),
    ('Ana Costa');
END
GO

-- Inserir Usuários
IF NOT EXISTS (SELECT * FROM Usuario)
BEGIN
    INSERT INTO Usuario ([User], [Password], DataCadastro, Tipo, UserCadatro) VALUES 
    ('admin', '123456', GETDATE(), 'Administrador', 'Sistema'),
    ('user1', '123456', GETDATE(), 'Usuario', 'admin'),
    ('supervisor1', '123456', GETDATE(), 'Supervisor', 'admin');
END
GO

-- Inserir Colaboradores
IF NOT EXISTS (SELECT * FROM Colaborador)
BEGIN
    INSERT INTO Colaborador (Nome, Pix, Referencia, Endereco, Numero, Bairro, Cidade, UF, CEP, UserCad) VALUES 
    ('Carlos Souza', '11987654321', 'REF001', 'Rua das Flores', '100', 'Centro', 'São Paulo', 'SP', '01000-000', 'admin'),
    ('Fernanda Lima', '11976543210', 'REF002', 'Av. Paulista', '1500', 'Bela Vista', 'São Paulo', 'SP', '01310-000', 'admin'),
    ('Roberto Alves', '11965432109', 'REF003', 'Rua Augusta', '2000', 'Consolação', 'São Paulo', 'SP', '01305-000', 'admin');
END
GO

-- Inserir Detalhes dos Colaboradores
IF NOT EXISTS (SELECT * FROM ColaboradorDetalhe)
BEGIN
    INSERT INTO ColaboradorDetalhe (IdColaborador, ValorDiaria, IdFuncao, IdSupervisor, IdPosto) VALUES 
    (1, 150.00, 1, 1, 1),
    (2, 180.00, 2, 2, 2),
    (3, 200.00, 4, 1, 3);
END
GO

-- Inserir Diárias
IF NOT EXISTS (SELECT * FROM Diaria)
BEGIN
    INSERT INTO Diaria (IdColaboradorDetalhe, DataDiaria, UserCadastro) VALUES 
    (1, GETDATE(), 'admin'),
    (1, DATEADD(DAY, -1, GETDATE()), 'admin'),
    (2, GETDATE(), 'admin'),
    (3, GETDATE(), 'admin');
END
GO

-- ============================================
-- Tabela: Menu
-- ============================================
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Menu')
BEGIN
    CREATE TABLE Menu (
        Codigo INT IDENTITY(1,1) PRIMARY KEY,
        Descricao NVARCHAR(100) NOT NULL,
        Icone     NVARCHAR(100) NULL,
        Ordem     INT NOT NULL DEFAULT 0,
        Ativo     BIT NOT NULL DEFAULT 1
    );
END
GO

-- ============================================
-- Tabela: SubMenu
-- ============================================
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'SubMenu')
BEGIN
    CREATE TABLE SubMenu (
        Codigo      INT IDENTITY(1,1) PRIMARY KEY,
        CodigoMenu  INT NOT NULL,
        Descricao   NVARCHAR(100) NOT NULL,
        Icone       NVARCHAR(100) NULL,
        Url         NVARCHAR(200) NULL,
        Ordem       INT NOT NULL DEFAULT 0,
        Ativo       BIT NOT NULL DEFAULT 1,
        CONSTRAINT FK_SubMenu_Menu FOREIGN KEY (CodigoMenu) REFERENCES Menu(Codigo)
    );
END
GO

-- ============================================
-- Tabela: UsuarioPermissao
-- ============================================
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'UsuarioPermissao')
BEGIN
    CREATE TABLE UsuarioPermissao (
        Id             INT IDENTITY(1,1) PRIMARY KEY,
        IdUsuario      INT NOT NULL,
        CodigoSubMenu  INT NOT NULL,
        ApenasLeitura  BIT NOT NULL DEFAULT 0,
        Ativo          BIT NOT NULL DEFAULT 1,
        DataCadastro   DATETIME NULL,
        CONSTRAINT FK_UsuarioPermissao_Usuario  FOREIGN KEY (IdUsuario)     REFERENCES Usuario(Id),
        CONSTRAINT FK_UsuarioPermissao_SubMenu  FOREIGN KEY (CodigoSubMenu) REFERENCES SubMenu(Codigo)
    );
END
GO

-- Adicionar coluna ApenasLeitura caso a tabela ja exista sem ela
IF EXISTS (SELECT * FROM sys.tables WHERE name = 'UsuarioPermissao')
   AND NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('UsuarioPermissao') AND name = 'ApenasLeitura')
BEGIN
    ALTER TABLE UsuarioPermissao
    ADD ApenasLeitura BIT NOT NULL DEFAULT 0;
END
GO

-- ============================================
-- Dados de Exemplo: Menu e SubMenu
-- ============================================
IF NOT EXISTS (SELECT * FROM Menu)
BEGIN
    INSERT INTO Menu (Descricao, Icone, Ordem) VALUES
    ('Cadastros',  'mdi-folder',    1),
    ('Operacional','mdi-briefcase', 2),
    ('Relatorios', 'mdi-chart-bar', 3);
END
GO

IF NOT EXISTS (SELECT * FROM SubMenu)
BEGIN
    INSERT INTO SubMenu (CodigoMenu, Descricao, Icone, Url, Ordem) VALUES
    (1, 'Colaboradores',  'mdi-account-group',  '/api/Colaborador',       1),
    (1, 'Funcoes',        'mdi-tag',             '/api/Funcao',            2),
    (1, 'Postos',         'mdi-map-marker',      '/api/Posto',             3),
    (1, 'Supervisores',   'mdi-account-tie',     '/api/Supervisor',        4),
    (1, 'Usuarios',       'mdi-account-cog',     '/api/Usuario',           5),
    (2, 'Diarias',        'mdi-calendar-check',  '/api/Diaria',            1),
    (2, 'Diaria Disponivel','mdi-calendar-plus', '/api/DiariaDisponivel',  2),
    (2, 'Descontos',      'mdi-minus-circle',    '/api/DiariaDesconto',    3),
    (3, 'Lista Diarias',  'mdi-file-table',      '/api/Relatorio',         1);
END
GO

-- ============================================
-- Tabela: AprovacaoConfig
-- Define qual usuario aprova o CREATE de cada entidade.
-- Gerenciado pelo usuario master. Pode ter multiplos aprovadores por entidade.
-- ============================================
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'AprovacaoConfig')
BEGIN
    CREATE TABLE AprovacaoConfig (
        Id                 INT IDENTITY(1,1) PRIMARY KEY,
        Entidade           NVARCHAR(100) NOT NULL,
        IdUsuarioAprovador INT NOT NULL,
        Ativo              BIT NOT NULL DEFAULT 1,
        DataCadastro       DATETIME NOT NULL DEFAULT GETDATE(),
        CONSTRAINT FK_AprovacaoConfig_Usuario FOREIGN KEY (IdUsuarioAprovador) REFERENCES Usuario(Id)
    );
END
GO

-- ============================================
-- Tabela: AprovacaoStage
-- Fila de solicitacoes de criacao aguardando aprovacao.
-- ============================================
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'AprovacaoStage')
BEGIN
    CREATE TABLE AprovacaoStage (
        Id                   INT IDENTITY(1,1) PRIMARY KEY,
        Entidade             NVARCHAR(100)  NOT NULL,
        PayloadJson          NVARCHAR(MAX)  NOT NULL,
        IdUsuarioSolicitante INT            NOT NULL,
        IdUsuarioAprovador   INT            NOT NULL,
        Status               NVARCHAR(20)   NOT NULL DEFAULT 'Pendente',
        DataSolicitacao      DATETIME       NOT NULL DEFAULT GETDATE(),
        DataAprovacao        DATETIME       NULL,
        Observacao           NVARCHAR(500)  NULL,
        IdRegistroGerado     INT            NULL,
        CONSTRAINT FK_AprovacaoStage_Solicitante FOREIGN KEY (IdUsuarioSolicitante) REFERENCES Usuario(Id),
        CONSTRAINT FK_AprovacaoStage_Aprovador   FOREIGN KEY (IdUsuarioAprovador)   REFERENCES Usuario(Id),
        CONSTRAINT CK_AprovacaoStage_Status CHECK (Status IN ('Pendente','Aprovado','Rejeitado'))
    );
END
GO

PRINT 'Banco de dados criado com sucesso!';
GO

