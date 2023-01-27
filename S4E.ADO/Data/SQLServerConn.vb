Imports System.Data
Imports System.Data.SqlClient

Namespace Data
    Public Class SQLServerConn
        Implements IDisposable
#Region "PROPRIEDADES"
        Public Property StringConn As String
        Public Property connDb As SqlConnection
        Private disposedValue As Boolean

#End Region


#Region "CONSTRUTORES"
        Public Sub New()
            StringConn = "Data Source=DESKTOP-8KTP5PQ;Initial Catalog=S4E;Integrated Security=True"
            connDb = New SqlConnection(StringConn)
            connDb.Open()
        End Sub
#End Region
#Region "METODOS"
        Protected Overridable Sub Dispose(disposing As Boolean)
            If Not disposedValue Then
                If disposing Then
                    Close()
                End If
                disposedValue = True
            End If
        End Sub

        Public Sub Dispose() Implements IDisposable.Dispose
            Dispose(disposing:=True)
            GC.SuppressFinalize(Me)
        End Sub

        Public Sub Close()
            connDb.Close()
        End Sub
#End Region

    End Class
End Namespace

