<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ItemSearch.aspx.cs" Inherits="ItemSearch_OR.ItemSearch" %>

<!DOCTYPE html>
<!-- https://apps.uwmedicine.org/or_location/or_location.aspx.    -->
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container body-content">
 
             <div>
                 <asp:Image ID="Image1" runat="server" Height="67px" Width="176px" ImageUrl="~/images/hmcLogo.jpg" /></div>
         
            <div><h1>Item Search</h1></div>
       
        <asp:RadioButtonList ID="rblSearch" runat="server" RepeatColumns="1">
            <asp:ListItem Selected="True" Value="PMM_ITEM_NO">Item Number&nbsp;&nbsp;&nbsp;&nbsp;(exact match)</asp:ListItem>
            <asp:ListItem Value="PMM_DESCR">Item Description</asp:ListItem>
            <asp:ListItem Value="PMM_MFG_CAT_NO">Catalog Number</asp:ListItem>
        </asp:RadioButtonList>

    <p>
        <asp:TextBox ID="txtSearch" runat="server"></asp:TextBox>
        <asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click" />
        <br />
            &nbsp; &nbsp; &nbsp; &nbsp;<asp:Label ID="lblRC" runat="server" Text="Record Count: " Font-Size="Small"></asp:Label> 
            &nbsp;&nbsp;<asp:Label ID="lblCount" runat="server" Text="0" Font-Size="Small"></asp:Label> 
        <!--<br /> <a href="https://www.google.com/search?site=imghp&tbm=isch&source=hp&biw=1600&bih=771&q=BANDAGE ESMARK LF 6 X 9" target ="_blank">BANDAGE ESMARK LF 6 X 9</a>-->
    </p>
             <p>
                 <asp:Label ID="lblError" runat="server" ForeColor="Red" Text="Error" Visible="False"></asp:Label>
    </p>
        <asp:GridView ID="gvItemResults" runat="server" htmlencode="false" AutoGenerateColumns ="false" AllowSorting="True" CellPadding="5" CellSpacing="5">
            <Columns>
                <asp:BoundField DataField="ITEM NO" HeaderText="ITEM NO" />
                <asp:BoundField DataField="CTLG NO" HeaderText="CTLG NO" />
                <asp:BoundField DataField="LOC NO" HeaderText="LOC" />
                <asp:BoundField DataField="BIN LOC" HeaderText="BIN LOC" />
                <asp:BoundField DataField="PKG STR" HeaderText="PKG STR" />
                <asp:BoundField DataField="TO UM" HeaderText="TO UM" />
                <asp:BoundField DataField="PRICE" HeaderText="PRICE" />
                <asp:BoundField DataField="DESCR" HeaderText="DESC" HtmlEncode="False" />
                <%--<asp:HyperLinkField DataTextField="DESCR"  DataNavigateUrlFields="imgSearch" HeaderText="DESC" />--%>
                <asp:BoundField DataField="DESC1" HeaderText="DESC1" />
                <asp:BoundField DataField="DESC2" HeaderText="DESC2" />
                <asp:BoundField DataField="UW ITEM" HeaderText="UW ITEM" />
            </Columns>
        </asp:GridView>

            <!--AND I.ITEM_NO LIKE '%' + @ITEM_NO + '%'-->

<!-- ITEM NUMBER -->
        <asp:SqlDataSource ID="PMM_ITEM_NO" runat="server" ConnectionString="<%$ ConnectionStrings:BIAdminConnectString %>" 
            SelectCommand="">
            <SelectParameters>
                <asp:ControlParameter ControlID="txtSearch" DefaultValue="" Name="ITEM_NO" PropertyName="Text"  Type="String" />
            </SelectParameters>
        </asp:SqlDataSource>

<!-- DESCRIPTION -->
        <asp:SqlDataSource ID="PMM_DESCR" runat="server" 
        ConnectionString="<%$ ConnectionStrings:BIAdminConnectString %>"                 
        SelectCommand="" >   
            
        <SelectParameters>
            <asp:ControlParameter Name="DESCR" ControlID="txtSearch" PropertyName="Text" 
                Type="String"  />
        </SelectParameters>
    </asp:SqlDataSource>
            
<!-- CATALOG NUMBER     -->
    <asp:SqlDataSource ID="PMM_MFG_CAT_NO" runat="server" 
        ConnectionString="<%$ ConnectionStrings:BIAdminConnectString %>" 
                
        SelectCommand="">

          
        <SelectParameters>
            <asp:ControlParameter ControlID="txtSearch" Name="MFG_CAT_NO" 
                PropertyName="Text" Type="String" />
        </SelectParameters>
    </asp:SqlDataSource>
            <br />
            <br />
            <footer>
            </footer>
        </div>
    </form>
</body>
</html>
