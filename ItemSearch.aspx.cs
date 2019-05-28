using System;
using System.Configuration;
using System.Collections.Specialized;
using LogDefault;
using System.Web.UI;



namespace ItemSearch_OR
{
    public partial class ItemSearch : System.Web.UI.Page
    {
        //private string logFilePath = "";
        //private string logFile = "";
        //private bool debug = false;
        private NameValueCollection ConfigData = null;
        private LogManager lm = LogManager.GetInstance();

        protected void Page_Load(object sender, EventArgs e)
        {
            ConfigData = (NameValueCollection)ConfigurationManager.GetSection("appSettings");
            lm.LogFile = ConfigData.Get("logFile");
            lm.LogFilePath = ConfigData.Get("logFilePath");
        }
    

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                if (rblSearch.SelectedValue.Trim().Length > 0)
                {
                    string test = PMM_DESCR.ConnectionString;
                    gvItemResults.DataSourceID = rblSearch.SelectedValue;
                    gvItemResults.DataBind();
                    lblCount.Text = gvItemResults.Rows.Count.ToString();
                }

                /* code loc:
                 * Prod:  \\itsvm-genweb1\hmcmm$  (use webdev)
                 * Test:  \\testvm-genweb1\hmcmm$
                 * https://testapps.uwmedicine.org/hmcmm/ItemSearch
                 * DESCR = PRIMARY
                 * DESCR1 = SURGERY DESC
                 * DESCR2 = NURSING
                 * SAMPLE ITEM# - 100354 FOR UWMC# 
                 * SAMPLE ITEM# - 10003 FOR GHX# 
                 */
            }
            catch (Exception ex)
            {
                  lm.Write("ItemSearch.btnSearch_Click " + ex.Message);                      
            }
        }
     
    }
}