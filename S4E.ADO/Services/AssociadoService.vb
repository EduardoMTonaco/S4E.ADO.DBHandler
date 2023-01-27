Imports System.Data.SqlClient
Imports FluentResults
Imports S4E.ADO.Data
Imports S4E.ADO.Models
Imports S4E.ADO.Models.Dto.AssociadoDto
Imports S4E.ADO.Models.Dto.EmpresaDto
Imports S4E.ADO.Profiles

Namespace Services
    Public Class AssociadoService
#Region "PROPRIEDADES"
        Private Property _meuMapper As Mapper
#End Region
#Region "CONSTRUTORTES"
        Public Sub New()
            Me._meuMapper = New Mapper
        End Sub
#End Region

#Region "METODOS"
        Public Function AdicionaAssociado(associadoDto As CreateAssociadoDto) As Associado
            Dim Id As Integer
            Using conection As New SQLServerConn
                Id = AdicionaAssociado(conection, associadoDto)
                AdicionaRelacao(conection, associadoDto, Id)
            End Using
            Return RecuperaAssociadoPorId(Id)
        End Function
        Public Function RecuperaAssociados() As IEnumerable(Of Associado)
            Using conection As New SQLServerConn
                Dim associadosDto As HashSet(Of GetAssociadoDto) = RecuperaAssociadosComRelacao(conection)
                Dim associados As New HashSet(Of Associado)
                For Each getEmpresaDto In associadosDto
                    associados.Add(RecuperaEmpresas(conection, getEmpresaDto))
                Next
                Return associados
            End Using
        End Function

        Public Function RecuperaAssociadoPorId(id As Integer) As Associado
            Using conection As New SQLServerConn
                Dim associadoDto = RecuperaAssociadoComRelacaoPorId(conection, id)
                Return RecuperaEmpresas(conection, associadoDto)
            End Using
        End Function
        Public Function RecuperaAssociadoPorCPF(cpf As String) As Associado
            Using conection As New SQLServerConn
                Dim associadoDto = RecuperaAssociadoComRelacaoPorCpf(conection, cpf)
                Return RecuperaEmpresas(conection, associadoDto)
            End Using
        End Function
        Public Function RecuperaAssociadosPorNome(nome As String) As IEnumerable(Of Associado)
            Using conection As New SQLServerConn
                Dim associadosDto As HashSet(Of GetAssociadoDto) = RecuperaAssociadosComRelacaoPorNome(conection, nome)
                Dim associados As New HashSet(Of Associado)
                For Each getEmpresaDto In associadosDto
                    associados.Add(RecuperaEmpresas(conection, getEmpresaDto))
                Next
                Return associados

            End Using
        End Function

        Public Function AtualizaAssociado(id As Integer, associadoDto As CreateAssociadoDto) As Result
            Dim associado As Associado
            Try
                associado = RecuperaAssociadoPorId(id)
                If associado Is Nothing Then
                    Throw New Exception("associadovazio")
                    Return Result.Fail("Associado não encontrado")
                End If
                Using conection As New SQLServerConn
                    RemoveRelacoes(conection, id)
                    AtualizaAssociado(conection, associadoDto, id)
                    AdicionaRelacao(conection, associadoDto, id)
                End Using

            Catch ex As Exception
                Throw New Exception("Nao existe")
                Return Result.Fail("Associado não encontrado")
            End Try
            Return Result.Ok
        End Function
        Public Function DeletaAssociado(id As Integer) As Result
            Using conection As New SQLServerConn
                'Try
                Dim comando As String = $"DELETE FROM ASSOCIADOS WHERE ID = {id}"
                RemoveRelacoes(conection, id)
                Using command As New SqlCommand(comando, conection.connDb)
                    command.ExecuteNonQuery()
                End Using
                'Catch
                ' Return Result.Fail("Associado não encontrado")
                ' End Try
            End Using
            Return Result.Ok
        End Function
#End Region
#Region "METODOS PRIVADOS"
        Private Function AdicionaAssociado(conection As SQLServerConn, associadoDto As CreateAssociadoDto) As Integer
            Dim comando As String = "INSERT INTO ASSOCIADOS (NOME, CPF, DATADENASCIMENTO)" +
                $" VALUES ('{associadoDto.Nome}','{associadoDto.Cpf}', '{associadoDto.DataDeNascimento}'); SELECT SCOPE_IDENTITY()"
            Using command As New SqlCommand(comando, conection.connDb)
                Return command.ExecuteScalar()
            End Using
        End Function
        Private Sub AdicionaRelacao(connection As SQLServerConn, associadoDto As CreateAssociadoDto, id As Integer)
            Dim comando As String = ""
            For Each empresaId In associadoDto.EmpresasId
                If empresaId <> 0 Then
                    comando += $"INSERT INTO ASSOCIADOEMPRESA (ASSOCIADOID, EMPRESAID) VALUES ('{id}', '{empresaId}');"
                End If
            Next
            If Not String.IsNullOrEmpty(comando) Then
                Using command As New SqlCommand(comando, connection.connDb)
                    command.ExecuteNonQuery()
                End Using
            End If
        End Sub
        Private Function RecuperaAssociadosComRelacao(connection As SQLServerConn) As ICollection(Of GetAssociadoDto)
            Dim associados As New HashSet(Of GetAssociadoDto)
            Dim comando As String = "SELECT A.ID, A.NOME, A.CPF, A.DATADENASCIMENTO, STRING_AGG(B.EMPRESAID, ',') AS RELACAO FROM " +
            "ASSOCIADOS A FULL OUTER JOIN ASSOCIADOEMPRESA B ON A.ID = B.ASSOCIADOID GROUP BY A.ID, A.NOME , A.CPF, A.DATADENASCIMENTO"
            Using command As New SqlCommand(comando, connection.connDb)
                Using myReader As SqlDataReader = command.ExecuteReader
                    While myReader.Read()
                        associados.Add(_meuMapper.MapGetAssociadoDto(myReader))
                    End While
                    myReader.Close()
                End Using
            End Using
            Return associados
        End Function

        Private Function RecuperaAssociadosComRelacaoPorNome(connection As SQLServerConn, nome As String) As ICollection(Of GetAssociadoDto)
            Dim associados As New HashSet(Of GetAssociadoDto)
            Dim comando As String =
                "SELECT ID, NOME, CPF, DATADENASCIMENTO, RELACAO FROM" +
                " (SELECT A.ID, A.NOME, A.CPF, A.DATADENASCIMENTO, STRING_AGG(B.EMPRESAID, ',') AS RELACAO" +
                " FROM ASSOCIADOS A FULL OUTER JOIN ASSOCIADOEMPRESA B ON A.ID = B.ASSOCIADOID GROUP BY A.ID," +
                $" A.NOME , A.CPF, A.DATADENASCIMENTO) AS A WHERE NOME = '{nome}'"
            Using command As New SqlCommand(comando, connection.connDb)
                Using myReader As SqlDataReader = command.ExecuteReader
                    While myReader.Read()
                        associados.Add(_meuMapper.MapGetAssociadoDto(myReader))
                    End While
                    myReader.Close()
                End Using
            End Using
            Return associados
        End Function

        Private Function RecuperaAssociadoComRelacaoPorId(connection As SQLServerConn, id As Integer) As GetAssociadoDto
            Dim comando As String = "SELECT ID, NOME, CPF, DATADENASCIMENTO, RELACAO FROM (SELECT A.ID, A.NOME, A.CPF, A.DATADENASCIMENTO, STRING_AGG(B.EMPRESAID, ',') AS RELACAO FROM " +
            $"ASSOCIADOS A FULL OUTER JOIN ASSOCIADOEMPRESA B ON A.ID = B.ASSOCIADOID GROUP BY A.ID, A.NOME , A.CPF, A.DATADENASCIMENTO)AS A WHERE ID = {id}"
            Using command As New SqlCommand(comando, connection.connDb)
                Using myReader As SqlDataReader = command.ExecuteReader
                    myReader.Read()
                    Dim getAssociado As GetAssociadoDto = _meuMapper.MapGetAssociadoDto(myReader)
                    myReader.Close()
                    Return getAssociado
                End Using
            End Using
        End Function

        Private Function RecuperaAssociadoComRelacaoPorCpf(connection As SQLServerConn, cpf As String) As GetAssociadoDto
            Dim comando As String = "SELECT ID, NOME, CPF, DATADENASCIMENTO, RELACAO FROM (SELECT A.ID, A.NOME, A.CPF, A.DATADENASCIMENTO, STRING_AGG(B.EMPRESAID, ',') AS RELACAO FROM " +
            $"ASSOCIADOS A FULL OUTER JOIN ASSOCIADOEMPRESA B ON A.ID = B.ASSOCIADOID GROUP BY A.ID, A.NOME , A.CPF, A.DATADENASCIMENTO) AS A WHERE CPF = '{cpf}'"
            Using command As New SqlCommand(comando, connection.connDb)
                Using myReader As SqlDataReader = command.ExecuteReader
                    myReader.Read()
                    Dim getAssociado As GetAssociadoDto = _meuMapper.MapGetAssociadoDto(myReader)
                    myReader.Close()
                    Return getAssociado
                End Using
            End Using
        End Function

        Private Function RecuperaAssociadoComRelacaoPorNome(connection As SQLServerConn, nome As String) As GetAssociadoDto
            Dim comando As String = "SELECT ID, NOME, CPF, DATADENASCIMENTO, RELACAO FROM (SELECT A.ID, A.NOME, A.CPF, A.DATADENASCIMENTO, STRING_AGG(B.ASSOCIADOID, ',') AS RELACAO FROM " +
            $"ASSOCIADOS A FULL OUTER JOIN ASSOCIADOEMPRESA B ON A.ID = B.ASSOCIADOID GROUP BY A.ID, A.NOME , A.CPF, A.DATADENASCIMENTO) AS A WHERE NOME = '{nome}'"
            Using command As New SqlCommand(comando, connection.connDb)
                Using myReader As SqlDataReader = command.ExecuteReader
                    myReader.Read()
                    Dim getAssociado As GetAssociadoDto = _meuMapper.MapGetAssociadoDto(myReader)
                    myReader.Close()
                    Return getAssociado
                End Using
            End Using
        End Function

        Private Sub RemoveRelacoes(connection As SQLServerConn, id As Integer)
            Dim comando As String = $"DELETE FROM ASSOCIADOEMPRESA WHERE ASSOCIADOID = {id} "
            Using command As New SqlCommand(comando, connection.connDb)
                command.ExecuteNonQuery()
            End Using
        End Sub

        Private Sub AtualizaAssociado(connection As SQLServerConn, associadoDto As CreateAssociadoDto, id As Integer)
            Dim comando As String = $"UPDATE ASSOCIADOS SET NOME = '{associadoDto.Nome}', CPF = '{associadoDto.Cpf}' WHERE ID = '{id}';"
            Using command As New SqlCommand(comando, connection.connDb)
                command.ExecuteNonQuery()
            End Using
        End Sub
        Private Function RecuperaEmpresas(connection As SQLServerConn, associadoDto As GetAssociadoDto) As Associado
            Dim empresasId = associadoDto.Empresas
            Dim condicao As String = " WHERE"
            Dim associado As Associado = _meuMapper.MapAssociado(associadoDto)
            If empresasId(0) <> 0 Then
                For Each i In empresasId
                    condicao += $" ID = {i} OR"
                Next
                Dim comando As String = $"SELECT ID, NOME, CNPJ FROM EMPRESAS{condicao.Remove(condicao.LastIndexOf("OR"))}"

                Using command As New SqlCommand(comando, connection.connDb)
                    Using myReader As SqlDataReader = command.ExecuteReader
                        While myReader.Read()
                            Dim associadoId As Integer = myReader.GetInt32(0)
                            Dim empresa As ReadEmpresaDto = _meuMapper.MapReadEmpresaDto(myReader)
                            associado.Empresas.Add(empresa)
                        End While
                        myReader.Close()
                    End Using
                End Using
            End If
            Return associado
        End Function
#End Region


    End Class
End Namespace

