Imports System.Collections.ObjectModel
Imports System.Data.SqlClient
Imports S4E.ADO.Data

Public Class CriarTabelas
	Public Property conn As SQLServerConn

	Public Sub CriarTabelaAssociadoEmpresa()
		Dim comando As String = ComandoCriarTabelaAssociadoEmpresa()

		Using command As New SqlCommand(comando, conn.connDb)
			command.ExecuteNonQuery()
		End Using
	End Sub
	Public Sub CriarTabelaAssociados()
		Dim comando As String = ComandoCriarTabelaAssociados()

		Using command As New SqlCommand(comando, conn.connDb)
			command.ExecuteNonQuery()
		End Using
	End Sub
	Public Sub CriarTabelaEmpresas()
		Dim comando As String = ComandoCriarTabelaEmpresas()

		Using command As New SqlCommand(comando, conn.connDb)
			command.ExecuteNonQuery()
		End Using
	End Sub
	Function ComandoCriarTabelaAssociadoEmpresa() As String

		Return "USE [S4E]
GO

/****** Object:  Table [dbo].[AssociadoEmpresa]    Script Date: 27/01/2023 11:14:41 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[AssociadoEmpresa](
	[AssociadoId] [int] NOT NULL,
	[EmpresaId] [int] NOT NULL,
 CONSTRAINT [PK_AssociadoEmpresa] PRIMARY KEY CLUSTERED 
(
	[AssociadoId] ASC,
	[EmpresaId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[AssociadoEmpresa]  WITH CHECK ADD  CONSTRAINT [FK_AssociadoEmpresa_Associados_AssociadoId] FOREIGN KEY([AssociadoId])
REFERENCES [dbo].[Associados] ([Id])
GO

ALTER TABLE [dbo].[AssociadoEmpresa] CHECK CONSTRAINT [FK_AssociadoEmpresa_Associados_AssociadoId]
GO

ALTER TABLE [dbo].[AssociadoEmpresa]  WITH CHECK ADD  CONSTRAINT [FK_AssociadoEmpresa_Empresas_EmpresaId] FOREIGN KEY([EmpresaId])
REFERENCES [dbo].[Empresas] ([Id])
GO

ALTER TABLE [dbo].[AssociadoEmpresa] CHECK CONSTRAINT [FK_AssociadoEmpresa_Empresas_EmpresaId]
GO"

	End Function
	Function ComandoCriarTabelaAssociados() As String
		Return "USE [S4E]
GO

/****** Object:  Table [dbo].[Associados]    Script Date: 27/01/2023 11:25:02 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Associados](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Nome] [nvarchar](200) NOT NULL,
	[Cpf] [nvarchar](11) NOT NULL,
	[DataDeNascimento] [datetime2](7) NULL,
 CONSTRAINT [PK_Associados] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
"

	End Function
	Function ComandoCriarTabelaEmpresas() As String
		Return "USE [S4E]
GO

/****** Object:  Table [dbo].[Empresas]    Script Date: 27/01/2023 11:25:31 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Empresas](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Nome] [nvarchar](200) NOT NULL,
	[Cnpj] [nvarchar](14) NOT NULL,
 CONSTRAINT [PK_Empresas] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO


"
	End Function

End Class
