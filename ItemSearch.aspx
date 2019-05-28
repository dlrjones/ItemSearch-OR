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
            <asp:ListItem Selected="True" Value="PMM_ITEM_NO">Item Number</asp:ListItem>
            <asp:ListItem Value="PMM_DESCR">Item Description</asp:ListItem>
            <asp:ListItem Value="PMM_MFG_CAT_NO">Catalog Number</asp:ListItem>
        </asp:RadioButtonList>
    
    <p>
        <asp:TextBox ID="txtSearch" runat="server"></asp:TextBox>
        <asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click" />
        <br />
            &nbsp; &nbsp; &nbsp; &nbsp;<asp:Label ID="lblRC" runat="server" Text="Record Count: " Font-Size="Small"></asp:Label> 
            &nbsp;&nbsp;<asp:Label ID="lblCount" runat="server" Text="0" Font-Size="Small"></asp:Label> 
    </p>
        <asp:GridView ID="gvItemResults" runat="server" DataSourceID="PMM_ITEM_NO" AllowSorting="True" CellPadding="5" CellSpacing="5">
            <AlternatingRowStyle BackColor="#FFFF99" />
        </asp:GridView>

            <!--AND I.ITEM_NO LIKE '%' + @ITEM_NO + '%'-->

<!-- ITEM NUMBER -->
        <asp:SqlDataSource ID="PMM_ITEM_NO" runat="server" ConnectionString="<%$ ConnectionStrings:BIAdminConnectString %>" 
            SelectCommand="SELECT I.ITEM_NO as [Item No], I.CTLG_NO as [Catalog No], uwI.ITEM_NO as [UW ITEM], 
			substring(L.NAME, 5, len(L.NAME)) as Location, SIB.BIN_LOC as Bin, substring(IVP.TO_UM_CD, 7, 2) as [To UM],  
			'$' + CONVERT(VARCHAR(12), CONVERT(MONEY, IVP.PRICE)) as [Price],I.DESCR as Description, I.DESCR1 as [Surgery Description],
            I.DESCR2 AS [Nursing Description],SC_UWMItemMaster.GHX_FullDescr AS [GHX Description] 

            FROM [h-hemm].dbo.SLOC_ITEM SI 
            INNER JOIN [h-hemm].dbo.ITEM I ON SI.ITEM_ID = I.ITEM_ID 
            INNER JOIN [h-hemm].dbo.LOC L ON SI.LOC_ID = L.LOC_ID 
            LEFT JOIN [h-hemm].dbo.SLOC_ITEM_BIN SIB ON SI.LOC_ID = SIB.LOC_ID AND SI.ITEM_ID = SIB.ITEM_ID AND SI.ITEM_ID = SIB.ITEM_ID 
            INNER JOIN [h-hemm].dbo.ITEM_VEND IV ON I.ITEM_ID = IV.ITEM_ID
            INNER JOIN [h-hemm].dbo.ITEM_VEND_PKG IVP ON IV.ITEM_VEND_ID = IVP.ITEM_VEND_ID
            LEFT OUTER JOIN dbo.SC_UWMItemMaster ON SC_UWMItemMaster.UWM_ItemId = I.ITEM_ID 
             LEFT OUTER JOIN [h-hemm].dbo.ITEM uwI ON uwI.CTLG_NO = I.CTLG_NO
            
            WHERE (L.LOC_ID IN (1001, 1002, 1003,2254,2658)) AND (I.DESCR NOT LIKE 'CHG%') AND (SIB.BIN_LOC_NO IN (0, 1, 2)) AND (SI.STAT IN (1, 2))
            AND I.ITEM_NO = '' + @ITEM_NO + '' 		
            AND IV.SEQ_NO = 1
            AND IV.CORP_ID = 1000
            AND IVP.SEQ_NO = (SELECT MAX(SEQ_NO) MAX_SEQ_NO FROM [h-hemm].dbo.ITEM_VEND_PKG WHERE IV.ITEM_VEND_ID = ITEM_VEND_ID)            			
            ORDER BY I.DESCR, L.NAME">
            <SelectParameters>
                <asp:ControlParameter ControlID="txtSearch" DefaultValue="" Name="ITEM_NO" PropertyName="Text"  Type="String" />
            </SelectParameters>
        </asp:SqlDataSource>

<!-- DESCRIPTION -->
        <asp:SqlDataSource ID="PMM_DESCR" runat="server" 
        ConnectionString="<%$ ConnectionStrings:BIAdminConnectString %>"                 
        SelectCommand="SELECT I.ITEM_NO as [Item No], I.CTLG_NO as [Catalog No], uwI.ITEM_NO as [UW ITEM],          
			substring(L.NAME, 5, len(L.NAME)) as Location, SIB.BIN_LOC as Bin, substring(IVP.TO_UM_CD, 7, 2) as [To UM],  
			'$' + CONVERT(VARCHAR(12), CONVERT(MONEY, IVP.PRICE)) as [Price],I.DESCR as Description, I.DESCR1 as [Surgery Description],
            I.DESCR2 AS [Nursing Description],SC_UWMItemMaster.GHX_FullDescr AS [GHX Description]

            FROM [h-hemm].dbo.SLOC_ITEM SI 
            INNER JOIN [h-hemm].dbo.ITEM I ON SI.ITEM_ID = I.ITEM_ID 
            INNER JOIN [h-hemm].dbo.LOC L ON SI.LOC_ID = L.LOC_ID 
            LEFT JOIN [h-hemm].dbo.SLOC_ITEM_BIN SIB ON SI.LOC_ID = SIB.LOC_ID AND SI.ITEM_ID = SIB.ITEM_ID AND SI.ITEM_ID = SIB.ITEM_ID 
            INNER JOIN [h-hemm].dbo.ITEM_VEND IV ON I.ITEM_ID = IV.ITEM_ID
            INNER JOIN [h-hemm].dbo.ITEM_VEND_PKG IVP ON IV.ITEM_VEND_ID = IVP.ITEM_VEND_ID
            LEFT OUTER JOIN dbo.SC_UWMItemMaster ON SC_UWMItemMaster.UWM_ItemId = I.ITEM_ID 
            LEFT OUTER JOIN [h-hemm].dbo.ITEM uwI ON uwI.CTLG_NO = I.CTLG_NO

            WHERE (L.LOC_ID IN (1001, 1002, 1003,2254,2658)) AND (I.DESCR NOT LIKE 'CHG%') AND (SIB.BIN_LOC_NO IN (0, 1, 2)) AND (SI.STAT IN (1, 2))
            AND (I.DESCR LIKE UPPER('%' + @DESCR + '%') or I.DESCR1 LIKE UPPER('%' + @DESCR + '%') or I.DESCR2 LIKE UPPER('%' + @DESCR + '%') or GHX_FullDescr LIKE UPPER('%' + @DESCR + '%') )
            AND IV.SEQ_NO = 1
            AND IV.CORP_ID = 1000
            AND IVP.SEQ_NO = (SELECT MAX(SEQ_NO) MAX_SEQ_NO FROM [h-hemm].dbo.ITEM_VEND_PKG WHERE IV.ITEM_VEND_ID = ITEM_VEND_ID)
            			
            ORDER BY I.DESCR, L.NAME">
        <SelectParameters>
            <asp:ControlParameter Name="DESCR" ControlID="txtSearch" PropertyName="Text" 
                Type="String"  />
        </SelectParameters>
    </asp:SqlDataSource>
            
<!-- CATALOG NUMBER     -->
    <asp:SqlDataSource ID="PMM_MFG_CAT_NO" runat="server" 
        ConnectionString="<%$ ConnectionStrings:BIAdminConnectString %>" 
                
        SelectCommand="SELECT I.ITEM_NO as [Item No], I.CTLG_NO as [Catalog No], uwI.ITEM_NO as [UW ITEM],	
			substring(L.NAME, 5, len(L.NAME)) as Location, SIB.BIN_LOC as Bin, substring(IVP.TO_UM_CD, 7, 2) as [To UM],  
			'$' + CONVERT(VARCHAR(12), CONVERT(MONEY, IVP.PRICE)) as [Price],I.DESCR as Description, I.DESCR1 as [Surgery Description],
            I.DESCR2 AS [Nursing Description],SC_UWMItemMaster.GHX_FullDescr AS [GHX Description] 

            FROM [h-hemm].dbo.SLOC_ITEM SI 
            INNER JOIN [h-hemm].dbo.ITEM I ON SI.ITEM_ID = I.ITEM_ID 
            INNER JOIN [h-hemm].dbo.LOC L ON SI.LOC_ID = L.LOC_ID 
            LEFT JOIN [h-hemm].dbo.SLOC_ITEM_BIN SIB ON SI.LOC_ID = SIB.LOC_ID AND SI.ITEM_ID = SIB.ITEM_ID AND SI.ITEM_ID = SIB.ITEM_ID 
            INNER JOIN [h-hemm].dbo.ITEM_VEND IV ON I.ITEM_ID = IV.ITEM_ID
            INNER JOIN [h-hemm].dbo.ITEM_VEND_PKG IVP ON IV.ITEM_VEND_ID = IVP.ITEM_VEND_ID
        LEFT OUTER JOIN dbo.SC_UWMItemMaster ON SC_UWMItemMaster.UWM_ItemId = I.ITEM_ID 
        LEFT OUTER JOIN [h-hemm].dbo.ITEM uwI ON uwI.CTLG_NO = '' + @MFG_CAT_NO + ''

            WHERE (L.LOC_ID IN (1001, 1002, 1003,2254,2658)) AND (I.DESCR NOT LIKE 'CHG%') AND (SIB.BIN_LOC_NO IN (0, 1, 2)) AND (SI.STAT IN (1, 2))
            AND I.CTLG_NO LIKE UPPER('%' + @MFG_CAT_NO + '%')
            AND IV.SEQ_NO = 1
            AND IV.CORP_ID = 1000
            AND IVP.SEQ_NO = (SELECT MAX(SEQ_NO) MAX_SEQ_NO FROM [h-hemm].dbo.ITEM_VEND_PKG WHERE IV.ITEM_VEND_ID = ITEM_VEND_ID)
            			
            ORDER BY I.DESCR, L.NAME"
            ProviderName="<%$ ConnectionStrings:PMM.ProviderName %>">
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
