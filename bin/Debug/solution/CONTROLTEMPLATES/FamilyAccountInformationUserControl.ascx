<%@ Control Language="vb" AutoEventWireup="false" Inherits="FamilyAccountInformationWebPart.FamilyAccountInformationUserControl, FamilyAccountInformationWebPart,Version=1.0.0.0, Culture=neutral, PublicKeyToken=9f4da00116c38ec5" %>
<h2>
    <asp:Label ID="lblFamilySurname" runat="server" Text="Label"></asp:Label>
    &nbsp;Family Information</h2>
<h3>
    Boys at College</h3>
<p>
    <asp:PlaceHolder ID="phBoysAtCollege" runat="server"></asp:PlaceHolder>
</p>
<h3>
    Family email addresses</h3>
<p>
    <asp:PlaceHolder ID="phFamilyEmailAddress" runat="server"></asp:PlaceHolder>
</p>
<p>
    <asp:ImageButton ID="ibtnNotifyAccountDataIssue" runat="server" ImageUrl="mailplain.jpg"/>
    &nbsp;<asp:HyperLink ID="hlNotifyAccountDataIssue" runat="server" NavigateUrl="mailto:studentrecords@whitefriars.vic.edu.au">Notify student records about issue with account information</asp:HyperLink>
</p>
<p align="right">
    <asp:Button ID="btnGotoManagePassword" runat="server" Text="Manage Password"  />
    <br />
    <asp:Button ID="btnGotoManageSecurityQuestion" runat="server" Text="Manage Security Question"  />
</p>
