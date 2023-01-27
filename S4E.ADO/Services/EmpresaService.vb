Imports System.Data
Imports System.Data.SqlClient
Imports FluentResults
Imports S4E.ADO.Data
Imports S4E.ADO.Models
Imports S4E.ADO.Models.Dto.AssociadoDto
Imports S4E.ADO.Models.Dto.EmpresaDto
Imports S4E.ADO.Profiles

Namespace Services
    Public Class EmpresaService
#Region "PROPRIEDADES"
        Private _mapper As Mapper
#End Region
#Region "CONSTRUTORTES"
        Public Sub New()
            _mapper = New Mapper
        End Sub
#End Region

#Region "METODOS"
        Public Function AdicionaEmpresa(empresaDto As CreateEmpresaDto) As Empresa
            Dim Id As Integer
            Using conection As New SQLServerConn
                Id = AdicionaEmpresa(conection, empresaDto)
                AdicionaRelacao(conection, empresaDto, Id)
            End Using
            Dim empresa As Empresa = RecuperaEmpresaPorId(Id)
            Return empresa

        End Function
        Public Function RecuperaEmpresas() As IEnumerable(Of Empresa)
            Using conection As New SQLServerConn
                Dim empresasDto As HashSet(Of GetEmpresaDto) = RecuperaEmpresasComRelacao(conection)
                Dim empresas As New HashSet(Of Empresa)
                For Each getEmpresaDto In empresasDto
                    empresas.Add(RecuperaAssociados(conection, getEmpresaDto))
                Next
                Return empresas
            End Using
        End Function

        Public Function RecuperaEmpresaPorId(id As Integer) As Empresa
            Using conection As New SQLServerConn
                Dim empresaDto = RecuperaEmpresaComRelacaoPorId(conection, id)
                Return RecuperaAssociados(conection, empresaDto)
            End Using
        End Function
        Public Function RecuperaEmpresaPorCnpj(cnpj As String) As Empresa
            Using conection As New SQLServerConn
                Dim empresaDto = RecuperaEmpresaComRelacaoPorCnpj(conection, cnpj)
                Return RecuperaAssociados(conection, empresaDto)
            End Using
        End Function
        Public Function RecuperaEmpresasPorNome(nome As String) As IEnumerable(Of Empresa)
            Using conection As New SQLServerConn
                Dim empresasDto As HashSet(Of GetEmpresaDto) = RecuperaEmpresasComRelacaoPorNome(conection, nome)
                Dim empresas As New HashSet(Of Empresa)
                For Each getEmpresaDto In empresasDto
                    empresas.Add(RecuperaAssociados(conection, getEmpresaDto))
                Next
                Return empresas
            End Using
        End Function

        Public Function AtualizaEmpresa(id As Integer, empresaDto As CreateEmpresaDto) As Result
            Dim empresa As Empresa
            'Try
            empresa = RecuperaEmpresaPorId(id)
            If empresa Is Nothing Then
                Return Result.Fail("Empresa não encontrada")
            End If
            Using conection As New SQLServerConn
                RemoveRelacoes(conection, id)
                AtualizaEmpresa(conection, empresaDto, id)

                AdicionaRelacao(conection, empresaDto, id)
            End Using

            'Catch ex As Exception
            'Return Result.Fail("Empresa não encontrada")
            'End Try
            Return Result.Ok
        End Function
        Public Function DeletaEmpresa(id As Integer) As Result
            Using conection As New SQLServerConn
                'Try
                Dim comando As String = $"DELETE FROM EMPRESAS WHERE ID = {id}"
                RemoveRelacoes(conection, id)
                Using command As New SqlCommand(comando, conection.connDb)
                    command.ExecuteNonQuery()

                End Using
                'Catch
                'Return Result.Fail("Empresa não encontrada")
                'End Try
            End Using
            Return Result.Ok
        End Function
#End Region

#Region "METODOS PRIVADOS"
        Private Function AdicionaEmpresa(conection As SQLServerConn, empresaDto As CreateEmpresaDto) As Integer
            Dim comando As String = $"INSERT INTO EMPRESAS (NOME, CNPJ) VALUES ('{empresaDto.Nome}','{empresaDto.Cnpj}'); SELECT SCOPE_IDENTITY()"
            Using command As New SqlCommand(comando, conection.connDb)
                Return command.ExecuteScalar()
            End Using
        End Function
        Private Sub AdicionaRelacao(connection As SQLServerConn, empresaDto As CreateEmpresaDto, id As Integer)
            Dim comando As String = ""
            For Each associadoId In empresaDto.Associados
                If associadoId <> 0 Then
                    comando += $"INSERT INTO ASSOCIADOEMPRESA (ASSOCIADOID, EMPRESAID) VALUES ('{associadoId}', '{id}');"
                End If
            Next
            If Not String.IsNullOrEmpty(comando) Then
                Using command As New SqlCommand(comando, connection.connDb)
                    command.ExecuteNonQuery()
                End Using
            End If
        End Sub
        Private Function RecuperaEmpresasComRelacao(connection As SQLServerConn) As ICollection(Of GetEmpresaDto)
            Dim empresas As New HashSet(Of GetEmpresaDto)
            Dim comando As String = "SELECT A.ID, A.NOME, A.CNPJ, STRING_AGG(B.ASSOCIADOID, ',') AS RELACAO FROM " +
            "EMPRESAS A FULL OUTER JOIN ASSOCIADOEMPRESA B ON A.ID = B.EMPRESAID GROUP BY A.ID, A.NOME , A.CNPJ"
            Using command As New SqlCommand(comando, connection.connDb)
                Using myReader As SqlDataReader = command.ExecuteReader
                    While myReader.Read()
                        empresas.Add(_mapper.MapGetEmpresaDto(myReader))
                    End While
                End Using
            End Using
            Return empresas
        End Function

        Private Function RecuperaEmpresasComRelacaoPorNome(connection As SQLServerConn, nome As String) As ICollection(Of GetEmpresaDto)
            Dim empresas As New HashSet(Of GetEmpresaDto)
            Dim comando As String = "SELECT ID, NOME, CNPJ, RELACAO FROM (SELECT A.ID, A.NOME, A.CNPJ, STRING_AGG(B.ASSOCIADOID, ',')" +
                $"AS RELACAO FROM EMPRESAS A FULL OUTER JOIN ASSOCIADOEMPRESA B ON A.ID = B.EMPRESAID GROUP BY A.ID, A.NOME , A.CNPJ)" +
                $" AS A WHERE NOME = '{nome}'"
            Using command As New SqlCommand(comando, connection.connDb)
                Using myReader As SqlDataReader = command.ExecuteReader
                    While myReader.Read()
                        empresas.Add(_mapper.MapGetEmpresaDto(myReader))
                    End While
                End Using
            End Using
            Return empresas
        End Function

        Private Function RecuperaEmpresaComRelacaoPorId(connection As SQLServerConn, id As Integer) As GetEmpresaDto
            Dim comando As String = "SELECT ID, NOME, CNPJ, RELACAO FROM (SELECT A.ID, A.NOME, A.CNPJ, STRING_AGG(B.ASSOCIADOID, ',') AS RELACAO FROM " +
            $"EMPRESAS A FULL OUTER JOIN ASSOCIADOEMPRESA B ON A.ID = B.EMPRESAID GROUP BY A.ID, A.NOME , A.CNPJ)AS A WHERE ID = {id}"
            Using command As New SqlCommand(comando, connection.connDb)
                Using myReader As SqlDataReader = command.ExecuteReader
                    myReader.Read()
                    Return _mapper.MapGetEmpresaDto(myReader)
                End Using
            End Using
        End Function

        Private Function RecuperaEmpresaComRelacaoPorCnpj(connection As SQLServerConn, cnpj As String) As GetEmpresaDto
            Dim comando As String = "SELECT ID, NOME, CNPJ, RELACAO FROM (SELECT A.ID, A.NOME, A.CNPJ, STRING_AGG(B.ASSOCIADOID, ',') AS RELACAO FROM " +
            $"EMPRESAS A FULL OUTER JOIN ASSOCIADOEMPRESA B ON A.ID = B.EMPRESAID GROUP BY A.ID, A.NOME , A.CNPJ)AS A WHERE CNPJ = '{cnpj}'"
            Using command As New SqlCommand(comando, connection.connDb)
                Using myReader As SqlDataReader = command.ExecuteReader
                    myReader.Read()
                    Return _mapper.MapGetEmpresaDto(myReader)
                End Using
            End Using
        End Function

        Private Function RecuperaEmpresaComRelacaoPorNome(connection As SQLServerConn, nome As String) As GetEmpresaDto
            Dim comando As String = "SELECT ID, NOME, CNPJ, RELACAO FROM (SELECT A.ID, A.NOME, A.CNPJ, STRING_AGG(B.EMPRESAID, ',') AS RELACAO FROM " +
            $"EMPRESAS A FULL OUTER JOIN ASSOCIADOEMPRESA B ON A.ID = B.EMPRESAID GROUP BY A.ID, A.NOME , A.CNPJ)AS A WHERE NOME = '{nome}'"
            Using command As New SqlCommand(comando, connection.connDb)
                Using myReader As SqlDataReader = command.ExecuteReader
                    myReader.Read()
                    Return _mapper.MapGetEmpresaDto(myReader)
                End Using
            End Using
        End Function

        Private Sub RemoveRelacoes(connection As SQLServerConn, id As Integer)
            Dim comando As String = $"DELETE FROM ASSOCIADOEMPRESA WHERE EMPRESAID = {id} "
            Using command As New SqlCommand(comando, connection.connDb)
                command.ExecuteNonQuery()
            End Using
        End Sub

        Private Sub AtualizaEmpresa(connection As SQLServerConn, empresaDto As CreateEmpresaDto, id As Integer)
            Dim comando As String = $"UPDATE EMPRESAS SET NOME = '{empresaDto.Nome}', CNPJ = '{empresaDto.Cnpj}' WHERE ID = '{id}';"
            Using command As New SqlCommand(comando, connection.connDb)
                command.ExecuteNonQuery()
            End Using
        End Sub

        Private Function RecuperaAssociados(connection As SQLServerConn, empresaDto As GetEmpresaDto) As Empresa
            Dim associadosId = empresaDto.Associados
            Dim condicao As String = " WHERE"
            Dim empresa As Empresa = _mapper.MapEmpresa(empresaDto)
            If associadosId(0) <> 0 Then
                For Each i In associadosId
                    condicao += $" ID = {i} OR"
                Next
                Dim comando As String = $"SELECT ID, NOME, CPF, DATADENASCIMENTO FROM ASSOCIADOS{condicao.Remove(condicao.LastIndexOf("OR"))}"

                Using command As New SqlCommand(comando, connection.connDb)
                    Using myReader As SqlDataReader = command.ExecuteReader
                        While myReader.Read()
                            Dim associadoId As Integer = myReader.GetInt32(0)
                            Dim associado As ReadAssociadoDto = _mapper.MapReadAssociadoDto(myReader)
                            empresa.Associados.Add(associado)
                        End While
                    End Using
                End Using
            End If
            Return empresa
        End Function

#End Region
    End Class
End Namespace

