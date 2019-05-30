﻿using System;
using System.Configuration;
using System.Collections.Specialized;
using LogDefault;
using System.Web.UI;
using System.Data;
using System.Data.SqlClient;



namespace ItemSearch_OR
{
    public partial class ItemSearch : System.Web.UI.Page
    {
        //private string logFilePath = "";
        //private string logFile = "";
        //private bool debug = false;
        private NameValueCollection ConfigData = null;
        private LogManager lm = LogManager.GetInstance();
        private SqlConnection dbaseConn;
        private SqlDataAdapter dataAdapter = new SqlDataAdapter();
        private string dbaseConnStr = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            ConfigData = (NameValueCollection)ConfigurationManager.GetSection("appSettings");
            lm.LogFile = ConfigData.Get("logFile");
            lm.LogFilePath = ConfigData.Get("logFilePath");
            Session["connect"] = PMM_DESCR.ConnectionString; //ConfigurationManager.ConnectionStrings["BIAdminConnectString"].ConnectionString;
        }
    
        private DataTable ReadDataSet(SqlDataReader dr)
        {
            string colData = "";
            DataTable dt = new DataTable();
            dt.Columns.Add("ITEM NO", typeof(string));
            dt.Columns.Add("CTLG NO", typeof(string));
            dt.Columns.Add("LOC NO", typeof(string));
            dt.Columns.Add("BIN LOC", typeof(string));
            dt.Columns.Add("PKG STR", typeof(string));
            dt.Columns.Add("TO UM", typeof(string));
            dt.Columns.Add("PRICE", typeof(string));
            dt.Columns.Add("DESC", typeof(string));
            dt.Columns.Add("DESC1", typeof(string));
            dt.Columns.Add("DESC2", typeof(string));
            dt.Columns.Add("UW ITEM", typeof(string));
            try
            {
                while (dr.Read())
                {
                    dt.Rows.Add(dr[0].ToString().Trim(),
                        dr[1].ToString().Trim(),
                        dr[2].ToString().Trim(),
                        dr[3].ToString().Trim(),
                        dr[4].ToString().Trim(),
                        dr[5].ToString().Trim(),
                        dr[6].ToString().Trim(),
                        dr[7].ToString().Trim(),
                        dr[8].ToString().Trim(),
                        dr[9].ToString().Trim(),
                        dr[10].ToString().Trim());
                }

            }catch(Exception ex)
            {
                lm.Write(ex.Message);
            }
            return dt;
            

        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            string itemNo = "";
            string sql = "";
            lblError.Text = "";
            lblError.Visible = false;
            DataSet dsSearchResults = new DataSet();
            try
            {
                if (rblSearch.SelectedValue.Trim().Length > 0)
                {
                    sql = GetSQL(rblSearch.SelectedValue, txtSearch.Text.Trim());
                    dbaseConn = new SqlConnection(Session["connect"].ToString());
                    SqlCommand comm = new SqlCommand(sql, dbaseConn);
                    dbaseConn.Open();
                    SqlDataReader dr = comm.ExecuteReader();
                    DataTable dtSearchResults = ReadDataSet(dr);                   
                   
                    gvItemResults.DataSource = dtSearchResults;
                    gvItemResults.DataBind();
                    lblCount.Text = gvItemResults.Rows.Count.ToString();
                    dbaseConn.Close();
                }

            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Subquery returned more than 1 value"))
                {
                    lblError.Text = "The Search term must be an exact match.";
                    lblError.Visible = true;
                }
                  lm.Write("ItemSearch.btnSearch_Click - Search Term:" + txtSearch.Text + Environment.NewLine + ex.Message);                      
            }
        }

        private string GetSQL(string searchType, string searchText)
        {
            string sql = "";
            SQLSource sqlSrc = new SQLSource();
            sqlSrc.GetSQLQuery(searchType, searchText);
            sql = sqlSrc.SqlQuery;
            return sql;
        }
       

    }
}