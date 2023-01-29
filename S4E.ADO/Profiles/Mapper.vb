Imports System.Data
Imports System.Data.Common
Imports System.Data.SqlClient
Imports S4E.ADO.Models
Imports S4E.ADO.Models.Dto
Imports S4E.ADO.Models.Dto.AssociadoDto
Imports S4E.ADO.Models.Dto.EmpresaDto

Namespace Profiles
    Public Class Mapper
        Public Function MapAssociado(dataReader As SqlDataReader) As Associado
            VerificaId(dataReader)

            Return New Associado With {
                            .Id = dataReader.GetInt32(0),
                            .Nome = dataReader.GetString(1),
                            .Cpf = dataReader.GetString(2),
                            .DataDeNascimento = dataReader.GetDateTime(3)
                        }
        End Function

        Public Function MapEmpresa(dataReader As SqlDataReader) As Empresa
            VerificaId(dataReader)
            Return New Empresa With {
                            .Id = dataReader.GetInt32(0),
                            .Nome = dataReader.GetString(1),
                            .Cnpj = dataReader.GetString(2)
                        }
        End Function
        Public Function MapReadAssociadoDto(dataReader As SqlDataReader) As ReadAssociadoDto
            VerificaId(dataReader)
            Dim associadoDto As New ReadAssociadoDto With {
                            .Id = dataReader.GetInt32(0),
                            .Nome = dataReader.GetString(1),
                            .Cpf = dataReader.GetString(2)
                        }
            If Not dataReader.IsDBNull(3) Then
                associadoDto.DataDeNascimento = dataReader.GetDateTime(3)
            End If
            Return associadoDto
        End Function

        Public Function MapReadEmpresaDto(dataReader As SqlDataReader) As ReadEmpresaDto
            VerificaId(dataReader)
            Return New ReadEmpresaDto With {
                            .Id = dataReader.GetInt32(0),
                            .Nome = dataReader.GetString(1),
                            .Cnpj = dataReader.GetString(2)
                        }
        End Function

        Public Function MapGetEmpresaDto(dataReader As SqlDataReader) As GetEmpresaDto
            VerificaId(dataReader)
            Dim empresa As New GetEmpresaDto With {
                            .Id = dataReader.GetInt32(0),
                            .Nome = dataReader.GetString(1),
                            .Cnpj = dataReader.GetString(2)
                        }
            If Not dataReader.IsDBNull(3) Then
                Dim associados() As String = dataReader.GetString(3).Split(",")
                For Each associado In associados
                    If Not String.IsNullOrEmpty(associado) Then
                        empresa.Associados.Add(associado)
                    End If
                Next
            End If
            Return empresa
        End Function

        Public Function MapGetAssociadoDto(dataReader As SqlDataReader) As GetAssociadoDto
            VerificaId(dataReader)
            Dim associado As New GetAssociadoDto With {
                            .Id = dataReader.GetInt32(0),
                            .Nome = dataReader.GetString(1),
                            .Cpf = dataReader.GetString(2)
                        }
            If Not dataReader.IsDBNull(3) Then
                associado.DataDeNascimento = dataReader.GetDateTime(3)
            End If
            If Not dataReader.IsDBNull(4) Then
                Dim empresas() As String = dataReader.GetString(4).Split(",")
                For Each empresa In empresas
                    If Not String.IsNullOrEmpty(empresa) Then
                        associado.Empresas.Add(empresa)
                    End If
                Next
            End If


            Return associado
        End Function
        Public Function MapEmpresa(getEmpresaDto As GetEmpresaDto) As Empresa
            Return New Empresa With {
                .Id = getEmpresaDto.Id,
                .Nome = getEmpresaDto.Nome,
                .Cnpj = getEmpresaDto.Cnpj
            }
        End Function

        Public Function MapAssociado(getAssociadoDto As GetAssociadoDto) As Associado
            Return New Associado With {
                .Id = getAssociadoDto.Id,
                .Nome = getAssociadoDto.Nome,
                .Cpf = getAssociadoDto.Cpf,
                .DataDeNascimento = getAssociadoDto.DataDeNascimento
            }
        End Function


        Private Sub VerificaId(dataReader As SqlDataReader)
            If dataReader.IsDBNull(0) Then
                Throw New ArgumentNullException("Id", "Cliente não localizado.")
            End If
        End Sub
    End Class
End Namespace


