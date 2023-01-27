Imports S4E.ADO.Models.Dto
Imports S4E.ADO.Models.Dto.AssociadoDto

Namespace Models


    Public Class Empresa
        Public Id As Integer
        Public Nome As String
        Public CNPJ As String
        Public Associados As ICollection(Of ReadAssociadoDto)

        Public Sub New()
            Associados = New HashSet(Of ReadAssociadoDto)
        End Sub

    End Class
End Namespace
