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

PRINT 'Banco de dados criado com sucesso!';
GO

