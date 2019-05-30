
namespace ItemSearch_OR
{
    public class SQLSource
    {
        string sqlQuery = "";

        public string SqlQuery
        {
            get { return sqlQuery; }
        }

        public void GetSQLQuery(string queryType, string searchText)
        {
            switch (queryType)
            {
                case "PMM_ITEM_NO":
                    sqlQuery = BuildItem(searchText);
                    break;
                case "PMM_DESCR":
                    sqlQuery = BuildDescription(searchText);
                    break;
                case "PMM_MFG_CAT_NO":
                    sqlQuery = BuildCatalog(searchText);
                    break;
            }
        }

        private string BuildItem(string searchText)
        {
            #region Item
            return "SELECT I.ITEM_NO as [Item No], I.CTLG_NO as [Catalog No],substring(L.NAME, 5, len(L.NAME)) as Location, SIB.BIN_LOC as Bin, " +
                   "PKG_STR.PKG_STR,substring(IVP.TO_UM_CD, 7, 2) as [To UM], '$' + CONVERT(VARCHAR(12), CONVERT(MONEY, IVP.PRICE)) as [Price], " +
                   "I.DESCR as Description, I.DESCR1 as [Surgery Description],I.DESCR2 AS [Nursing Description], " +
                   "[UW ITEM].System_ItemNo AS [UW_ITEM] " +

                   "FROM [h-hemm].dbo.SLOC_ITEM SI  " +
                   "INNER JOIN [h-hemm].dbo.ITEM I ON SI.ITEM_ID = I.ITEM_ID  " +
                   "INNER JOIN [h-hemm].dbo.LOC L ON SI.LOC_ID = L.LOC_ID  " +
                   "LEFT JOIN [h-hemm].dbo.SLOC_ITEM_BIN SIB ON SI.LOC_ID = SIB.LOC_ID AND SI.ITEM_ID = SIB.ITEM_ID AND SI.ITEM_ID = SIB.ITEM_ID  " +
                   "INNER JOIN [h-hemm].dbo.ITEM_VEND IV ON I.ITEM_ID = IV.ITEM_ID     " +
                   "INNER JOIN [h-hemm].dbo.ITEM_VEND_PKG IVP ON IV.ITEM_VEND_ID = IVP.ITEM_VEND_ID     " +
                   "INNER JOIN [h-hemm].dbo.ITEM_VEND_PKG_FACTOR ON IVP.ITEM_VEND_ID = ITEM_VEND_PKG_FACTOR.ITEM_VEND_ID    " +
                   "AND IVP.UM_CD = ITEM_VEND_PKG_FACTOR.UM_CD  " +
                   "AND IVP.TO_UM_CD = ITEM_VEND_PKG_FACTOR.TO_UM_CD " +
                   "JOIN (SELECT System_ItemNo,UWM_MfrCtlgNo FROM dbo.SC_UWMItemMaster UIM " +
                   "    JOIN dbo.SC_UWMItemMasterLink UIML ON UIML.UWM_ItemId = UIM.UWM_ItemId " +
                   "    WHERE UWM_MfrCtlgNo = (select CTLG_NO from [h-hemm].dbo.ITEM where ITEM_NO = '" + searchText + "') AND System_Id = 'UWMC') " +
                   "    AS [UW ITEM] ON [UW ITEM].UWM_MfrCtlgNo = I.CTLG_NO " +
                   "JOIN ( SELECT I.ITEM_ID, RIGHT(RTRIM(ITEM_VEND_PKG.UM_CD), 2) + ' ' + CAST(RIGHT(RTRIM(ITEM_VEND_PKG_FACTOR.TO_QTY), 4) AS VARCHAR)   " +
                   "+ ' ' + RIGHT(RTRIM(ITEM_VEND_PKG.TO_UM_CD), 2) AS PKG_STR " +
                   "FROM [h-hemm].dbo.ITEM I JOIN  " +
                   "[h-hemm].dbo.ITEM_VEND ON I.ITEM_ID = ITEM_VEND.ITEM_ID INNER JOIN   " +
                   "[h-hemm].dbo.ITEM_VEND_PKG ON ITEM_VEND.ITEM_VEND_ID = ITEM_VEND_PKG.ITEM_VEND_ID INNER JOIN   " +
                   "[h-hemm].dbo.ITEM_VEND_PKG_FACTOR ON ITEM_VEND_PKG.ITEM_VEND_ID = ITEM_VEND_PKG_FACTOR.ITEM_VEND_ID AND   " +
                   "[h-hemm].dbo.ITEM_VEND_PKG.UM_CD = ITEM_VEND_PKG_FACTOR.UM_CD AND   " +
                   "[h-hemm].dbo.ITEM_VEND_PKG.TO_UM_CD = ITEM_VEND_PKG_FACTOR.TO_UM_CD " +
                   "WHERE UPPER(I.ITEM_NO) LIKE UPPER('%" + searchText + "%') " +
                   "AND (I.STAT IN (1,2)) AND(ITEM_VEND.SEQ_NO = 1) AND(ITEM_VEND_PKG.SEQ_NO = 1))  " +
                   "AS PKG_STR ON PKG_STR.ITEM_ID = I.ITEM_ID " +

                   "WHERE (L.LOC_ID IN (1001, 1002, 1003,2254,2658)) AND (UPPER(I.DESCR) NOT LIKE 'CHG%') AND (SIB.BIN_LOC_NO IN (0, 1, 2)) AND (SI.STAT IN (1, 2)) " +
                   "AND I.ITEM_NO = '" + searchText + "'  " +
                   "AND IV.SEQ_NO = 1 " +
                   "AND IV.CORP_ID = 1000 " +
                   "AND IVP.SEQ_NO = (SELECT MAX(SEQ_NO) MAX_SEQ_NO FROM [h-hemm].dbo.ITEM_VEND_PKG WHERE IV.ITEM_VEND_ID = ITEM_VEND_ID) " +
                   "AND (I.STAT IN (1,2))  " +
                   "ORDER BY I.DESCR, L.NAME";

            #endregion
        }

        private string BuildDescription(string searchText)
        {
            #region Description
            return "SELECT DISTINCT I.ITEM_NO as [Item No], I.CTLG_NO as [Catalog No], " +
                         "substring(L.NAME, 5, len(L.NAME)) as Location, SIB.BIN_LOC as Bin, PKG_STR.PKG_STR,substring(IVP.TO_UM_CD, 7, 2) as [To UM], " +
                         "'$' + CONVERT(VARCHAR(12), CONVERT(MONEY, IVP.PRICE)) as [Price],I.DESCR as Description, I.DESCR1 as [Surgery Description], " +
                         "I.DESCR2 AS[Nursing Description], " +
                         "[UW ITEM].System_ItemNo as [UW ITEM] " +
                         "FROM[h-hemm].dbo.SLOC_ITEM SI " +
                         "INNER JOIN[h-hemm].dbo.ITEM I ON SI.ITEM_ID = I.ITEM_ID " +
                          "INNER JOIN[h-hemm].dbo.LOC L ON SI.LOC_ID = L.LOC_ID " +
                          "LEFT JOIN [h-hemm].dbo.SLOC_ITEM_BIN SIB ON SI.LOC_ID = SIB.LOC_ID AND SI.ITEM_ID = SIB.ITEM_ID AND SI.ITEM_ID = SIB.ITEM_ID " +
                          "INNER JOIN [h-hemm].dbo.ITEM_VEND IV ON I.ITEM_ID = IV.ITEM_ID " +
                          "INNER JOIN [h-hemm].dbo.ITEM_VEND_PKG IVP ON IV.ITEM_VEND_ID = IVP.ITEM_VEND_ID " +
                          "JOIN(SELECT System_ItemNo, UWM_MfrCtlgNo FROM dbo.SC_UWMItemMaster UIM " +
                          "JOIN dbo.SC_UWMItemMasterLink UIML ON UIML.UWM_ItemId = UIM.UWM_ItemId " +
                          "JOIN(select CTLG_NO from[h-hemm].dbo.ITEM where UPPER(DESCR) LIKE UPPER('%" + searchText + "%')) " +
                          "AS ITEM_DESC ON ITEM_DESC.CTLG_NO = UWM_MfrCtlgNo " +
                          "WHERE System_Id = 'UWMC') " +
                          "AS [UW ITEM] ON [UW ITEM].UWM_MfrCtlgNo = I.CTLG_NO " +
                          "JOIN (SELECT I.ITEM_ID, RIGHT(RTRIM(ITEM_VEND_PKG.UM_CD), 2) + ' ' + CAST(RIGHT(RTRIM(ITEM_VEND_PKG_FACTOR.TO_QTY), 4) AS VARCHAR) " +
                          "   + ' ' + RIGHT(RTRIM(ITEM_VEND_PKG.TO_UM_CD), 2) AS PKG_STR " +
                          "FROM[h-hemm].dbo.ITEM I JOIN " +
                          "[h-hemm].dbo.ITEM_VEND ON I.ITEM_ID = ITEM_VEND.ITEM_ID INNER JOIN " +
                          "[h-hemm].dbo.ITEM_VEND_PKG ON ITEM_VEND.ITEM_VEND_ID = ITEM_VEND_PKG.ITEM_VEND_ID INNER JOIN " +
                          "[h-hemm].dbo.ITEM_VEND_PKG_FACTOR ON ITEM_VEND_PKG.ITEM_VEND_ID = ITEM_VEND_PKG_FACTOR.ITEM_VEND_ID AND " +
                          "[h-hemm].dbo.ITEM_VEND_PKG.UM_CD = ITEM_VEND_PKG_FACTOR.UM_CD AND " +
                          "[h-hemm].dbo.ITEM_VEND_PKG.TO_UM_CD = ITEM_VEND_PKG_FACTOR.TO_UM_CD " +
                          "WHERE UPPER(I.DESCR) LIKE UPPER('%" + searchText + "%') AND(I.STAT IN (1,2)) AND(ITEM_VEND.SEQ_NO = 1) AND(ITEM_VEND_PKG.SEQ_NO = 1))  " +
                          "AS PKG_STR ON PKG_STR.ITEM_ID = I.ITEM_ID " +
                          "WHERE (L.LOC_ID IN (1001, 1002, 1003,2254,2658)) AND(I.DESCR NOT LIKE 'CHG%') AND(SIB.BIN_LOC_NO IN (0, 1, 2)) AND(SI.STAT IN (1, 2)) " +
                          "AND(UPPER(I.DESCR) LIKE UPPER('%" + searchText + "%') or UPPER(I.DESCR1) LIKE UPPER('%" + searchText + "%') or UPPER(I.DESCR2) LIKE UPPER('%" + searchText + "%')  ) " +
                          "AND IV.SEQ_NO = 1 " +
                          "AND IV.CORP_ID = 1000 " +
                          "AND IVP.SEQ_NO = (SELECT MAX(SEQ_NO) MAX_SEQ_NO FROM[h-hemm].dbo.ITEM_VEND_PKG WHERE IV.ITEM_VEND_ID = ITEM_VEND_ID) " +
                          "AND(I.STAT IN (1,2)) " +
                          "ORDER BY[Item No], [Catalog No], Bin, PKG_STR.PKG_STR,[To UM],[Price],Description,[Surgery Description], " +
                          "[Nursing Description],[UW ITEM] ";
            #endregion
        }

        private string BuildCatalog(string searchText)
        {
            #region CatalogNumber
            return "SELECT I.ITEM_NO as [Item No], I.CTLG_NO as [Catalog No], 		  " +
                   "substring(L.NAME, 5, len(L.NAME)) as Location, SIB.BIN_LOC as Bin,  " +
                   "PKG_STR.PKG_STR, substring(IVP.TO_UM_CD, 7, 2) as [To UM],'$' + CONVERT(VARCHAR(12), CONVERT(MONEY, IVP.PRICE)) as [Price], " +
                   "I.DESCR as Description, I.DESCR1 as [Surgery Description],I.DESCR2 AS[Nursing Description], " +
                   "[UW ITEM].System_ItemNo AS [UW_ITEM] " +

                   "FROM[h-hemm].dbo.SLOC_ITEM SI " +
                   "INNER JOIN[h-hemm].dbo.ITEM I ON SI.ITEM_ID = I.ITEM_ID " +
                   "INNER JOIN[h-hemm].dbo.LOC L ON SI.LOC_ID = L.LOC_ID " +
                   "LEFT JOIN [h-hemm].dbo.SLOC_ITEM_BIN SIB ON SI.LOC_ID = SIB.LOC_ID AND SI.ITEM_ID = SIB.ITEM_ID AND SI.ITEM_ID = SIB.ITEM_ID " +
                   "INNER JOIN [h-hemm].dbo.ITEM_VEND IV ON I.ITEM_ID = IV.ITEM_ID " +
                   "INNER JOIN [h-hemm].dbo.ITEM_VEND_PKG IVP ON IV.ITEM_VEND_ID = IVP.ITEM_VEND_ID " +
                   "JOIN(SELECT System_ItemNo, UWM_MfrCtlgNo FROM SC_UWMItemMaster UIM " +
                            "JOIN dbo.SC_UWMItemMasterLink UIML ON UIML.UWM_ItemId = UIM.UWM_ItemId " +
                            "WHERE UWM_MfrCtlgNo LIKE '%" + searchText + "%' and System_Id = 'UWMC') " +
                            "AS[UW ITEM] ON[UW ITEM].UWM_MfrCtlgNo = I.CTLG_NO " +
                   "join (SELECT I.ITEM_ID, RIGHT(RTRIM(ITEM_VEND_PKG.UM_CD), 2) + ' ' + CAST(RIGHT(RTRIM(ITEM_VEND_PKG_FACTOR.TO_QTY), 4) AS VARCHAR)   " +
                   "        + ' ' + RIGHT(RTRIM(ITEM_VEND_PKG.TO_UM_CD), 2) AS PKG_STR " +
                   "        FROM[h-hemm].dbo.ITEM I JOIN " +
                   "        [h-hemm].dbo.ITEM_VEND ON I.ITEM_ID = ITEM_VEND.ITEM_ID INNER JOIN " +
                   "        [h-hemm].dbo.ITEM_VEND_PKG ON ITEM_VEND.ITEM_VEND_ID = ITEM_VEND_PKG.ITEM_VEND_ID INNER JOIN " +
                   "        [h-hemm].dbo.ITEM_VEND_PKG_FACTOR ON ITEM_VEND_PKG.ITEM_VEND_ID = ITEM_VEND_PKG_FACTOR.ITEM_VEND_ID AND " +
                   "        [h-hemm].dbo.ITEM_VEND_PKG.UM_CD = ITEM_VEND_PKG_FACTOR.UM_CD AND " +
                   "        [h-hemm].dbo.ITEM_VEND_PKG.TO_UM_CD = ITEM_VEND_PKG_FACTOR.TO_UM_CD " +
                   "        WHERE UPPER(I.CTLG_NO) LIKE UPPER('%" + searchText + "%') " +
                   "        AND(I.STAT IN (1,2)) AND(ITEM_VEND.SEQ_NO = 1) AND(ITEM_VEND_PKG.SEQ_NO = 1))  " +
                   "        AS PKG_STR ON PKG_STR.ITEM_ID = I.ITEM_ID " +

                   "WHERE (L.LOC_ID IN (1001, 1002, 1003,2254,2658)) AND(UPPER(I.DESCR) NOT LIKE 'CHG%') AND(SIB.BIN_LOC_NO IN (0, 1, 2)) " +
                   "AND(SI.STAT IN (1, 2)) " +
                   "AND I.CTLG_NO LIKE UPPER('%" + searchText + "%') " +
                   "AND IV.SEQ_NO = 1 " +
                   "AND IV.CORP_ID = 1000 " +
                   "AND IVP.SEQ_NO = (SELECT MAX(SEQ_NO) MAX_SEQ_NO FROM[h-hemm].dbo.ITEM_VEND_PKG WHERE IV.ITEM_VEND_ID = ITEM_VEND_ID) " +
                   "AND(I.STAT IN (1,2))  " +
                   "ORDER BY I.DESCR, L.NAME ";
            #endregion
        }
    }
}