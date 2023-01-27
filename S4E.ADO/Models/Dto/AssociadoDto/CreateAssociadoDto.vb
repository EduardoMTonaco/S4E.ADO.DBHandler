Imports System.ComponentModel.DataAnnotations

Namespace Models.Dto.AssociadoDto
    Public Class CreateAssociadoDto

        <Required>
        <MaxLength(200, ErrorMessage:="O nome não pode ter mais de 200 caracteres")>
        Public Property Nome As String
        <Required>
        <StringLength(11, ErrorMessage:="O CPF deve ter 11 caracteres")>
        Public Property Cpf As String
        Public Property DataDeNascimento As DateTime
        Public Property EmpresasId As ICollection(Of Integer)

        Public Sub New()
            EmpresasId = New HashSet(Of Integer)
        End Sub

    End Class

End Namespace
