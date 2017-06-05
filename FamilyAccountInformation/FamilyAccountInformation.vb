Option Explicit On
Option Strict On

Imports System
Imports System.Runtime.InteropServices
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Web.UI.WebControls.WebParts
Imports System.Xml.Serialization

Imports Microsoft.SharePoint
Imports Microsoft.SharePoint.WebControls
Imports Microsoft.SharePoint.WebPartPages

Namespace FamilyAccountInformationWebPart

    <Guid("1389a2b2-0801-4c1b-952f-9897a0ae5d43")> _
    Public Class FamilyAccountInformation
        Inherits System.Web.UI.WebControls.WebParts.WebPart

        Public Sub New()
        End Sub

        Private _dataConnectionStringDC As String = "Data Source=inetfs2;Initial Catalog=DataCentral;Persist Security Info=True;User ID=WEBAuthenticationFamily;Password=GreyTe@!410"
        <WebBrowsable(True), _
        Personalizable(PersonalizationScope.Shared), _
        WebDescription("Database Connection String for ClassLinks WP"), _
        WebDisplayName("Database Connection String for ClassLinks WP")> _
        Public Property dataConnectionStringDC() As String
            Get
                Return _dataConnectionStringDC
            End Get
            Set(ByVal value As String)
                _dataConnectionStringDC = value
            End Set
        End Property

        Private _managePasswordURL As String = "ManagePassword.aspx"
        <WebBrowsable(True), _
        Personalizable(PersonalizationScope.Shared), _
        WebDescription("URL String for the page that Manages user Password"), _
        WebDisplayName("URL String for the page that Manages user Password")> _
        Public Property managePasswordURL() As String
            Get
                Return _managePasswordURL
            End Get
            Set(ByVal value As String)
                _managePasswordURL = value
            End Set
        End Property

        Private _manageSecurityQuestionURL As String = "ManageSecurityQuestion.aspx"
        <WebBrowsable(True), _
        Personalizable(PersonalizationScope.Shared), _
        WebDescription("URL String for the page that Manages user Password"), _
        WebDisplayName("URL String for the page that Manages user Password")> _
        Public Property manageSecurityQuestionURL() As String
            Get
                Return _manageSecurityQuestionURL
            End Get
            Set(ByVal value As String)
                _manageSecurityQuestionURL = value
            End Set
        End Property

        Private _manageNotifyDataMailtoURL As String = "mailto:studentrecords@whitefriars.vic.edu.au"
        <WebBrowsable(True), _
        Personalizable(PersonalizationScope.Shared), _
        WebDescription("URL String for the mailto URL"), _
        WebDisplayName("URL String for the mailto URL")> _
        Public Property manageNotifyDataMailtoURL() As String
            Get
                Return _manageNotifyDataMailtoURL
            End Get
            Set(ByVal value As String)
                _manageNotifyDataMailtoURL = value
            End Set
        End Property

        Protected Overrides Sub CreateChildControls()
            MyBase.CreateChildControls()
            Dim uc As UserControl = _
                    TryCast(Page.LoadControl("~/_controltemplates/FamilyAccountInformationUserControl.ascx"), UserControl)
            DirectCast(uc, FamilyAccountInformationWebPart.FamilyAccountInformationUserControl).dataConnectionStringDC = _dataConnectionStringDC
            DirectCast(uc, FamilyAccountInformationWebPart.FamilyAccountInformationUserControl).managePasswordURL = _managePasswordURL
            DirectCast(uc, FamilyAccountInformationWebPart.FamilyAccountInformationUserControl).manageSecurityQuestionURL = _manageSecurityQuestionURL
            DirectCast(uc, FamilyAccountInformationWebPart.FamilyAccountInformationUserControl).manageNotifyDataMailtoURL = _manageNotifyDataMailtoURL
            Controls.Add(uc)
        End Sub

    End Class

End Namespace
