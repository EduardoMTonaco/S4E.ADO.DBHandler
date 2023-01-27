Imports S4E.ADO.Models.Dto
Imports S4E.ADO.Models.Dto.AssociadoDto
Imports S4E.ADO.Models.Dto.EmpresaDto

Namespace Models


    Public Class Associado

        Public id As Integer
        Public Nome As String
        Public Cpf As String
        Public DataDeNascimento As DateTime
        Public Empresas As ICollection(Of ReadEmpresaDto)

        Public Sub New()
            Empresas = New HashSet(Of ReadEmpresaDto)
        End Sub
    End Class

End Namespace
