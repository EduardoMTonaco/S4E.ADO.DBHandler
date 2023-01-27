Imports System.ComponentModel.DataAnnotations

Namespace Models.Dto.AssociadoDto
    Public Class ReadAssociadoDto
        <Required>
        Public Property id As Integer
        <Required>
        <MaxLength(200, ErrorMessage:="O nome não pode ter mais de 200 caracteres")>
        Public Property Nome As String
        <Required>
        <StringLength(11, ErrorMessage:="O CPF deve ter 11 caracteres")>
        Public Property Cpf As String
        Public Property DataDeNascimento As DateTime

    End Class
End Namespace


