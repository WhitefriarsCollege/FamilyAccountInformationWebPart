Imports System
Imports System.Data
Imports System.Drawing
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Security.Cryptography
Imports System.Text
Imports Microsoft.SharePoint
Imports Microsoft.SharePoint.WebControls
Imports System.Web.UI.WebControls.WebParts
Imports System.ComponentModel
Imports System.Net
Imports Microsoft.VisualBasic
Imports System.Collections
Imports System.Security.Principal
Imports System.Text.RegularExpressions

Namespace FamilyAccountInformationWebPart

    Partial Public Class FamilyAccountInformationUserControl
        Inherits System.Web.UI.UserControl

        Protected WithEvents lblFamilySurname As Global.System.Web.UI.WebControls.Label
        Protected WithEvents phBoysAtCollege As Global.System.Web.UI.WebControls.PlaceHolder
        Protected WithEvents phFamilyEmailAddress As Global.System.Web.UI.WebControls.PlaceHolder
        Protected WithEvents ibtnNotifyAccountDataIssue As Global.System.Web.UI.WebControls.ImageButton
        Protected WithEvents hlNotifyAccountDataIssue As Global.System.Web.UI.WebControls.HyperLink
        Protected WithEvents btnGotoManagePassword As Global.System.Web.UI.WebControls.Button
        Protected WithEvents btnGotoManageSecurityQuestion As Global.System.Web.UI.WebControls.Button

        Protected Friend dataConnectionStringDC As String = "Data Source=inetfs2;Initial Catalog=DataCentral;Persist Security Info=True;User ID=WEBAUthenticationStaff;Password=Freck!ed38"
        Protected Friend managePasswordURL As String = "ManagePassword.aspx"
        Protected Friend manageSecurityQuestionURL As String = "ManageSecurityQuestion.aspx"
        Protected Friend manageNotifyDataMailtoURL As String = "mailto:studentrecords@whitefriars.vic.edu.au"

        Dim urlPath As String


        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

            urlPath = getCurrentURL()

            hlNotifyAccountDataIssue.NavigateUrl = manageNotifyDataMailtoURL

            Dim currentUser As String = getCurrentUsername()
            'currentUser = "225978" 'for Moseley in testing

            Dim familyData As Collection = getFamilyData(currentUser)
            If familyData.Count > 0 Then
                If IsDBNull(familyData(1).securityquestion) Or familyData(1).securityquestion = String.Empty Then
                    Response.Redirect(managePasswordURL)
                Else
                    lblFamilySurname.Text = familyData(1).FamilySurname

                    For Each detail In familyData
                        Dim detailRow As New TableRow
                        Dim emails As Collection = detail.emails
                        For Each email In emails
                            Dim lblFamilyEmail As New Label
                            lblFamilyEmail.Text = email
                            phFamilyEmailAddress.Controls.Add(lblFamilyEmail)
                            phFamilyEmailAddress.Controls.Add(New LiteralControl("<br />"))
                        Next
                    Next

                    Dim familesBoys As Collection = getFamiliesBoys(currentUser)
                    Dim boysTable As New Table
                    For Each boy In familesBoys
                        Dim boysRow As New TableRow
                        Dim lblStudentFirstname As New Label
                        lblStudentFirstname.Text = boy.BoysFirstname
                        Dim firstName As New TableCell
                        firstName.Controls.Add(lblStudentFirstname)
                        boysRow.Controls.Add(firstName)

                        Dim lblStudentSurname As New Label
                        lblStudentSurname.Text = boy.BoysSurname
                        Dim firstSurname As New TableCell
                        firstSurname.Controls.Add(lblStudentSurname)
                        boysRow.Controls.Add(firstSurname)

                        Dim hlYearLevel As New HyperLink
                        hlYearLevel.Text = boy.YearLevel
                        hlYearLevel.NavigateUrl = "mailto://" & boy.LevelCoordinatorUsername & "@whitefriars.vic.edu.au"
                        Dim yearLevel As New TableCell
                        yearLevel.Controls.Add(hlYearLevel)
                        boysRow.Controls.Add(yearLevel)

                        Dim hlPastoralClass As New HyperLink
                        hlPastoralClass.Text = boy.PastoralClass
                        hlPastoralClass.NavigateUrl = "mailto://" & boy.PastoralTeacherUsername & "@whitefriars.vic.edu.au"
                        Dim pastoralClass As New TableCell
                        pastoralClass.Controls.Add(hlPastoralClass)
                        boysRow.Controls.Add(pastoralClass)
                        boysTable.Controls.Add(boysRow)

                        Dim hlTimetable As New HyperLink
                        hlTimetable.Text = "TT"
                        hlTimetable.Font.Italic = True
                        hlTimetable.ToolTip = "Timetable"

                        hlTimetable.NavigateUrl = urlPath & "pages/Timetable.aspx?login=" & boy.userName
                        Dim timetable As New TableCell
                        timetable.Controls.Add(hlTimetable)
                        boysRow.Controls.Add(timetable)
                        boysTable.Controls.Add(boysRow)
                    Next
                    phBoysAtCollege.Controls.Add(boysTable)
                End If
            Else
                lblFamilySurname.Text = "Error"
                lblFamilySurname.ForeColor = Drawing.Color.Red
                Dim errorLabel As New Label
                errorLabel.ForeColor = Drawing.Color.Red
                errorLabel.Text = "Family data not found please contact college"
                phFamilyEmailAddress.Controls.Add(errorLabel)
            End If
        End Sub

        Private Sub btnGotoManagePassword_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnGotoManagePassword.Click
            Response.Redirect(urlPath & "pages/" & managePasswordURL)
        End Sub

        Private Sub btnGotoManageSecurityQuestion_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnGotoManageSecurityQuestion.Click
            Response.Redirect(urlPath & "pages/" & manageSecurityQuestionURL)
        End Sub

        Private Sub ibtnNotifyAccountDataIssue_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnNotifyAccountDataIssue.Click
            Response.Write("<script>window.open('" & manageNotifyDataMailtoURL & "','_top');<" & Chr(47) & "script>")
        End Sub

        Function getCurrentURL() As String
            Dim pageURL As String = String.Empty
            Try
                Dim currentURL As String = Request.Url.ToString
                Dim fullURL() As String = currentURL.Split("/")
                For i As Integer = 0 To fullURL.Length - 2
                    pageURL += fullURL(i).ToString & "/"
                Next
            Catch

            End Try
            Return pageURL
        End Function

        Function getCurrentUsername() As String
            Dim userName As String = String.Empty
            Try
                Dim current As System.Security.Principal.WindowsIdentity = System.Security.Principal.WindowsIdentity.GetCurrent
                Dim userFullName As String = current.Name
                Dim userNameArr() As String = userFullName.Split("\")
                userName = userNameArr(1)
            Catch

            End Try
            Return userName

        End Function

        Function getFamiliesBoys(ByVal familyID As String) As Collection
            Dim familyBoys As New Collection

            Dim conn As New System.Data.SqlClient.SqlConnection
            conn.ConnectionString = dataConnectionStringDC
            Dim ds As New DataSet
            Try
                Using conn
                    Dim adp As New System.Data.SqlClient.SqlDataAdapter("WEBPART_FAMILY_Select_Student_Details", conn)
                    adp.SelectCommand.CommandType = CommandType.StoredProcedure
                    adp.SelectCommand.Parameters.Add("@FamilyID", SqlDbType.VarChar, 255).Value = familyID

                    conn.Open()
                    adp.Fill(ds, "familyBoysDetails")
                End Using

                If ds.Tables(0).Rows.Count > 0 Then
                    For Each row As DataRow In ds.Tables(0).Rows
                        Dim boy As New BoysDetails(row("studentID"), row("Surname"), row("S_SURNAME"), row("S_FIRST"), row("S_YEAR"), row("S_CLASS"), row("usernameLevelCoordinator"), row("usernamelPastoralTeacher"), row("userName"))
                        familyBoys.Add(boy, row("studentID"))
                    Next
                End If
            Catch ex As Exception

            Finally
                conn.Close()
                conn.Dispose()
            End Try

            Return familyBoys
        End Function

        Function getFamilyData(ByVal familyID As String) As Collection
            Dim familyData As New Collection

            Dim conn As New System.Data.SqlClient.SqlConnection
            conn.ConnectionString = dataConnectionStringDC
            Dim ds As New DataSet
            Try
                Using conn
                    Dim adp As New System.Data.SqlClient.SqlDataAdapter("WEBPART_FAMILY_Select_Family_Details", conn)
                    adp.SelectCommand.CommandType = CommandType.StoredProcedure
                    adp.SelectCommand.Parameters.Add("@FamilyID", SqlDbType.VarChar, 255).Value = familyID

                    conn.Open()
                    adp.Fill(ds, "familyDetails")
                End Using

                If ds.Tables(0).Rows.Count > 0 Then
                    For Each row As DataRow In ds.Tables(0).Rows
                        Dim emails As Collection = stripEmail(row("InternetEmailAddress"))
                        Dim securityQuestion As String = String.Empty
                        If Not IsDBNull(row("securityQuestion")) Then
                            securityQuestion = row("securityQuestion")
                        End If
                        Dim family As New FamilyDetails(row("Z_ID"), row("Surname"), emails, securityQuestion)
                        familyData.Add(family, row("Z_ID"))
                    Next
                End If
            Catch ex As Exception

            Finally
                conn.Close()
                conn.Dispose()
            End Try

            Return familyData
        End Function

        Function stripEmail(ByVal allEmails As String) As Collection
            Dim emailCollection As New Collection
            Dim splitEmailArr() As String = allEmails.Split(";")
            For Each email In splitEmailArr
                emailCollection.Add(email, email)
            Next
            Return emailCollection
        End Function

       
    End Class


    Public Class FamilyDetails
        Public Sub New(ByVal FamilyID As String, ByVal FamilySurname As String, ByVal emails As Collection, ByVal securityQuestion As String)
            _FamilyID = FamilyID
            _FamilySurname = FamilySurname
            _emails = emails
            _securityQuestion = securityQuestion
        End Sub

        Private _FamilyID As String
        Public Property FamilyID() As String
            Get
                Return _FamilyID
            End Get
            Set(ByVal value As String)
                _FamilyID = value
            End Set
        End Property

        Private _FamilySurname As String
        Public Property FamilySurname() As String
            Get
                Return _FamilySurname
            End Get
            Set(ByVal value As String)
                _FamilySurname = value
            End Set
        End Property

        Private _emails As Collection
        Public Property emails() As Collection
            Get
                Return _emails
            End Get
            Set(ByVal value As Collection)
                _emails = value
            End Set
        End Property

        Public WriteOnly Property newEmail() As String
            Set(ByVal value As String)
                _emails.Add(value)
            End Set
        End Property

        Private _securityQuestion As String
        Public Property securityQuestion() As String
            Get
                Return _securityQuestion
            End Get
            Set(ByVal value As String)
                _securityQuestion = value
            End Set
        End Property

    End Class

    Public Class BoysDetails

        Public Sub New(ByVal StudentID As String, ByVal FamilySurname As String, ByVal BoysSurname As String, ByVal BoysFirstname As String, ByVal YearLevel As Integer, ByVal PastoralClass As String, ByVal LevelCoordinatorUsername As String, ByVal PastoralTeacherUsername As String, ByVal userName As String)
            _StudentID = StudentID
            _FamilySurname = FamilySurname
            _BoysSurname = BoysSurname
            _BoysFirstname = BoysFirstname
            _YearLevel = YearLevel
            _PastoralClass = PastoralClass
            _LevelCoordinatorUsername = LevelCoordinatorUsername
            _PastoralTeacherUsername = PastoralTeacherUsername
            _userName = userName
        End Sub

        Private _StudentID As String
        Public Property StudentID()
            Get
                Return _StudentID
            End Get
            Set(ByVal value)
                _StudentID = value
            End Set
        End Property

        Private _FamilySurname As String
        Public Property FamilySurname()
            Get
                Return _FamilySurname
            End Get
            Set(ByVal value)
                _FamilySurname = value
            End Set
        End Property

        Private _BoysSurname As String
        Public Property BoysSurname() As String
            Get
                Return _BoysSurname
            End Get
            Set(ByVal value As String)
                _BoysSurname = value
            End Set
        End Property

        Private _BoysFirstname As String
        Public Property BoysFirstname() As String
            Get
                Return _BoysFirstname
            End Get
            Set(ByVal value As String)
                _BoysFirstname = value
            End Set
        End Property

        Private _YearLevel As Integer
        Public Property YearLevel() As Integer
            Get
                Return _YearLevel
            End Get
            Set(ByVal value As Integer)
                _YearLevel = value
            End Set
        End Property

        Private _PastoralClass As String
        Public Property PastoralClass() As String
            Get
                Return _PastoralClass
            End Get
            Set(ByVal value As String)
                _PastoralClass = value
            End Set
        End Property

        Private _LevelCoordinatorUsername As String
        Public Property LevelCoordinatorUsername() As String
            Get
                Return _LevelCoordinatorUsername
            End Get
            Set(ByVal value As String)
                _LevelCoordinatorUsername = value
            End Set
        End Property

        Private _PastoralTeacherUsername As String
        Public Property PastoralTeacherUsername() As String
            Get
                Return _PastoralTeacherUsername
            End Get
            Set(ByVal value As String)
                _PastoralTeacherUsername = value
            End Set
        End Property


        Private _userName As String
        Public Property userName() As String
            Get
                Return _userName
            End Get
            Set(ByVal value As String)
                _userName = value
            End Set
        End Property


    End Class

End Namespace