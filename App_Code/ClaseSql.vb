Imports Microsoft.VisualBasic
Imports MySql.Data.MySqlClient

Public Class DatosSql

    Public Shared Function permiso(ByVal usuario As String, ByVal accion As Integer, ByVal codigopagina As Integer) As Integer
        Dim resultado As Integer = 0
        Using connection As New MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("EjemploConString").ConnectionString)

            Dim comando As MySqlCommand
            Dim reader As MySqlDataReader

            Try
                connection.Open()
                comando = New MySqlCommand("select fn_permiso('" & usuario & "'," & accion & "," & codigopagina & ") valor", connection)
                reader = comando.ExecuteReader
                reader.Read()
                If reader.HasRows Then
                    If reader("valor") = 0 Then
                        reader.Close()
                        resultado = 0
                    ElseIf reader("valor") = 1 Then
                        reader.Close()
                        resultado = 1
                    End If
                Else
                    resultado = 0
                End If
            Catch ex As MySqlException
                resultado = 0
            End Try
        End Using
        Return resultado
    End Function

    Public Shared Function cargar_datatable(ByVal strsql As String) As System.Data.DataTable
        Using conexion As New MySqlConnection
            conexion.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings("EjemploConString").ConnectionString
            Dim ds As New System.Data.DataTable
            Try
                conexion.Open()
                Dim comando As New MySqlCommand(strsql, conexion)
                'comando.CommandTimeout = 500
                Dim da As New MySqlDataAdapter(comando)

                da.Fill(ds)
                comando.Dispose()
                da.Dispose()


            Catch ex As MySqlException

            End Try
            Return ds
        End Using
    End Function

    Public Shared Function cargar_cuadrecaja(ByVal strsql As String) As System.Data.DataTable

        Using conexion As New MySqlConnection
            conexion.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings("EjemploConString").ConnectionString
            Dim ds As New System.Data.DataTable
            Try
                conexion.Open()
                Dim comando As New MySqlCommand(strsql, conexion)
                comando.CommandTimeout = 100
                Dim da As New MySqlDataAdapter(comando)

                da.Fill(ds)
                comando.Dispose()
                da.Dispose()

            Catch ex As MySqlException

            End Try
            Return ds
        End Using
    End Function

    Public Shared Sub cargar_Grid(ByVal Datagridview As GridView, ByVal strsql As String)
        Using conexion As New MySqlConnection
            'conexion = New MySqlConnection()
            conexion.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings("EjemploConString").ConnectionString
            Try
                conexion.Open()
                Dim comando As New MySqlCommand(strsql, conexion)
                Dim da As New MySqlDataAdapter(comando)
                Dim ds As New System.Data.DataSet
                da.Fill(ds)
                Datagridview.DataSource = ds.Tables(0)
                Datagridview.AutoGenerateColumns = True
                Datagridview.DataBind()
                comando.Dispose()
                da.Dispose()

            Catch ex As MySqlException

            Finally
                'conexion.Close()
            End Try
        End Using
    End Sub

End Class