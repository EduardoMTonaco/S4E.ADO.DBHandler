Imports System.ComponentModel.DataAnnotations
Imports S4E.ADO.Models.Dto
Imports S4E.ADO.Models.Dto.AssociadoDto

Namespace Models


    Public Class Empresa

        <Required>
        Public Property Id As Integer
        <Required>
        <MaxLength(200, ErrorMessage:="O nome não pode ter mais de 200 caracteres")>
        Public Property Nome As String
        <Required>
        <StringLength(14, ErrorMessage:="O CNPJ deve ter 14 caracteres")>
        Public Property Cnpj As String

        Public Property Associados As ICollection(Of ReadAssociadoDto)

        Public Sub New()
            Associados = New HashSet(Of ReadAssociadoDto)
        End Sub

    End Class
End Namespace
