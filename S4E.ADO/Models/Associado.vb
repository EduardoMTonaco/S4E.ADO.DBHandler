Imports System.ComponentModel.DataAnnotations
Imports S4E.ADO.Models.Dto.EmpresaDto

Namespace Models

    Public Class Associado

        <Required>
        Public Property id As Integer
        <Required>
        <MaxLength(200, ErrorMessage:="O nome não pode ter mais de 200 caracteres")>
        Public Property Nome As String
        <Required>
        <StringLength(11, ErrorMessage:="O CPF deve ter 11 caracteres")>
        Public Property Cpf As String
        Public Property DataDeNascimento As DateTime
        Public Property Empresas As ICollection(Of ReadEmpresaDto)

        Public Sub New()
            Empresas = New HashSet(Of ReadEmpresaDto)
        End Sub
    End Class

End Namespace
