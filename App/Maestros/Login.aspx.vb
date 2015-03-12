Imports System.Data
Imports MySql.Data.MySqlClient

Partial Class Maestros_Login
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Session.Clear()
    End Sub
    Protected Sub conectar_Click(sender As Object, e As EventArgs) Handles conectar.Click
        Try
            Dim str As String

            Dim Adaptador As New MySqlDataAdapter
            Dim TablaUsuario As New DataTable
            Using conexion As New MySqlConnection()
                conexion.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings("EjemploConString").ConnectionString
                conexion.Open()
                Dim comando As MySqlCommand
                comando = conexion.CreateCommand
                comando.CommandType = Data.CommandType.Text
                comando.Parameters.Add("@id_usr", MySqlDbType.VarChar, 15).Value = TxtUser.Text
                comando.Parameters.Add("@pass_usr", MySqlDbType.VarChar, 15).Value = TxtPass.Text

                str = "select id_usr,id_tipousuario tipou,primer_nombre nombre " +
                    "from usuarios u " +
                    "inner join datosusuario du on u.id_datosusuario=du.id_datosusuario " +
                    "where id_usr ='" & TxtUser.Text & "' and contraseña=AES_ENCRYPT('" & TxtPass.Text & "','AGROGT2015') and u.estatus=1"
                'str = "select id_usr,id_agc,id_empsa,id_tipoU from Facturacionbusesa.usuario where id_usr = @idusuario and pass_usr=AES_ENCRYPT(@password,'DIOSESAMOR') and status_usr=1"
                comando.CommandText = str
                Adaptador.SelectCommand = comando
                Adaptador.Fill(TablaUsuario)
                If TablaUsuario.Rows.Count > 0 Then

                    Session("id_usr") = TablaUsuario.Rows(0).Item("id_usr")
                    Session("usuario") = TablaUsuario.Rows(0).Item("nombre")
                    'Session("id_agc") = TablaUsuario.Rows(0).Item("id_agc")
                    'Session("id_empsa") = TablaUsuario.Rows(0).Item("id_empsa")
                    Session("perfil") = TablaUsuario.Rows(0).Item("tipoU")
                    comando.Dispose()

                   
                   
                    'INTEGRADO PARA EVITAR ERROR DE INSERT CUANDO SE CREA GUIA AL CREDITO


                    Dim TablaFecha As DataTable = DatosSql.cargar_datatable("select DATE_FORMAT(curdate(),'%Y%m%d') AS FechaActual")
                    If TablaFecha.Rows.Count > 0 Then
                        Session("FechaActual") = TablaFecha.Rows(0).Item("FechaActual")
                    End If
                    FormsAuthentication.RedirectFromLoginPage(TxtUser.Text, False)
                Else
                    LblEstado.InnerText = "Datos incorrectos"
                End If
            End Using
        Catch ex As MySqlException
            LblEstado.InnerText = "Sucedio un error: " + ex.Message
        Finally
            'conexion.Close()
            'conexion = Nothing
        End Try
    End Sub
End Class
