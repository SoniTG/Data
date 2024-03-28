Module Module1

    Sub Main()
        Try

            Net.ServicePointManager.ServerCertificateValidationCallback = Function() True

            Dim folderstructure As String = Configuration.ConfigurationManager.AppSettings.Item("folderstructure")
            Dim folderpath As String = Configuration.ConfigurationManager.AppSettings.Item("folderpath")
            Dim plant As String = Configuration.ConfigurationManager.AppSettings.Item("plant")

            Dim ibafolder = folderpath & DateTime.Now.ToString(folderstructure)

            If IO.Directory.Exists(ibafolder) Then
                Dim files = IO.Directory.GetFiles(ibafolder, "*.dat").OrderByDescending(Function(f) New IO.FileInfo(f).CreationTime).ToList

                Dim b() As Byte
                Dim filename As String = ""

                For i As Integer = 0 To files.Count - 1
                    Dim finfo As New IO.FileInfo(files(i))
                    If finfo.Attributes.ToString.ToLower.Contains("offline") Or finfo.Attributes.ToString = "-1" Then
                        Continue For
                    End If
                    b = IO.File.ReadAllBytes(files(i))
                    filename = finfo.Name

                    Dim client As New myservice.wsitisconicSoapClient("wsitisconicSoap")

                    Dim res As String = client.SendFile(b, filename)

                    If res = "1" Then
                        Console.WriteLine("ok")
                    Else
                        Console.WriteLine("error")
                    End If
                Next

            End If


        Catch ex As Exception
            Console.WriteLine(ex.Message.ToString)
            Console.WriteLine(ex.StackTrace.ToString)
        End Try



    End Sub

End Module
