Imports System.ComponentModel.DataAnnotations

Namespace Models.Dto.EmpresaDto
    Public Class ReadEmpresaDto

        <Required>
        Public Id As Integer
        <Required>
        <MaxLength(200, ErrorMessage:="O nome não pode ter mais de 200 caracteres")>
        Public Nome As String
        <Required>
        <StringLength(14, ErrorMessage:="O CNPJ deve ter 14 caracteres")>
        Public Cnpj As String

    End Class
End Namespace

