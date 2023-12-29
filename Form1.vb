Imports System.DirectoryServices.ActiveDirectory
Imports System.Net.Http
Imports System.Text
Imports Newtonsoft.Json


Public Class Form1
    Public TokenApi As String = ""
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        ' URL del endpoint
        Dim apiUrl As String = "http://localhost:7500/login"
        Dim jsonData As String = "{""email"": """ + Me.TextBox1.Text + """, ""password"": """ + Me.TextBox2.Text + """}"
        Dim responseContent As String = Task.Run(Function() PostData(apiUrl, jsonData)).Result
        'Me.TextBox3.Text = responseContent
        'MessageBox.Show("Respuesta del servidor: " & responseContent)
        If responseContent <> "" Then

            ' Deserializar JSON
            Dim tokenInfo As TokenInfo = JsonConvert.DeserializeObject(Of TokenInfo)(responseContent)


            ' Acceder a la propiedad deserializada
            TokenApi = tokenInfo.token

            ' Imprimir el token
            MessageBox.Show(TokenApi)

            'MessageBox.Show("1")


            'Me.Close()
        Else
            MessageBox.Show("Combinación de usuario/contraseña invalida.")
        End If
        Console.WriteLine("Respuesta del servidor: " & responseContent)
        Console.ReadLine()
    End Sub

    Async Function PostData(ByVal apiUrl As String, ByVal jsonData As String) As Task(Of String)
        Using httpClient As New HttpClient()
            ' Crear el contenido de la solicitud
            Dim content As New StringContent(jsonData, Encoding.UTF8, "application/json")

            ' Realizar la solicitud POST y obtener la respuesta
            Dim response As HttpResponseMessage = Await httpClient.PostAsync(apiUrl, content)

            ' Verificar si la solicitud fue exitosa (código 200)
            If response.IsSuccessStatusCode Then
                ' Leer y devolver el contenido de la respuesta
                Return Await response.Content.ReadAsStringAsync()
            Else
                ' Imprimir un mensaje de error si la solicitud no fue exitosa
                Console.WriteLine("Error en la solicitud. Código de estado: " & response.StatusCode)
                Return String.Empty
            End If
        End Using
    End Function

    Public Class TokenInfo
        Public Property token As String
    End Class

End Class
