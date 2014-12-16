using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using DBUtility;

namespace CS.Controllers.customs
{
    public class Customer_StatisticController : Controller
    {
        public const string strFileds = "CategoryName,WayBillCount,vStatisticInfo";

        //
        // GET: /Customer_Statistic/

        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 分页查询类
        /// </summary>
        /// <param name="order"></param>
        /// <param name="page"></param>
        /// <param name="rows"></param>
        /// <param name="sort"></param>
        /// <returns></returns>
        public string GetData(string order, string page, string rows, string sort, string ddCompany, string txtBeginDate, string txtEndDate)
        {
            StringBuilder sb = new StringBuilder("");
            SqlParameter[] parameters = {
                      
                                new SqlParameter("@vCompany_input",SqlDbType.NVarChar),
                                new SqlParameter("@vStorageBeginDate_input",SqlDbType.NVarChar),
                                new SqlParameter("@vStorageEndDate_input",SqlDbType.NVarChar)
                                                        };

            ddCompany = Server.UrlDecode(ddCompany);
            txtBeginDate = Server.UrlDecode(txtBeginDate);
            txtEndDate = Server.UrlDecode(txtEndDate);

            parameters[0].Value = ddCompany;
            parameters[1].Value = Convert.ToDateTime(txtBeginDate).ToString("yyyyMMdd");
            parameters[2].Value = Convert.ToDateTime(txtEndDate).ToString("yyyyMMdd");
            try
            {
                DataSet dsAnaysic = SqlServerHelper.RunProcedure("sp_Customs_AnanalyseStatistic", parameters, "Default");
                DataTable dtAnaysic = dsAnaysic.Tables["Default"];

                sb.Append("{");
                sb.AppendFormat("\"total\":{0}", dtAnaysic.Rows.Count);
                sb.Append(",\"rows\":[");

                for (int i = 0; i < dtAnaysic.Rows.Count; i++)
                {
                    sb.Append("{");

                    string[] strFiledArray = strFileds.Split(',');
                    for (int j = 0; j < strFiledArray.Length; j++)
                    {
                        switch (strFiledArray[j])
                        {
                            default:
                                if (j != strFiledArray.Length - 1)
                                {
                                    sb.AppendFormat("\"{0}\":\"{1}\",", strFiledArray[j], dtAnaysic.Rows[i][strFiledArray[j]] == DBNull.Value ? "" : (dtAnaysic.Rows[i][strFiledArray[j]].ToString().Replace("\r\n", "")));
                                }
                                else
                                {
                                    sb.AppendFormat("\"{0}\":\"{1}\"", strFiledArray[j], dtAnaysic.Rows[i][strFiledArray[j]] == DBNull.Value ? "" : (dtAnaysic.Rows[i][strFiledArray[j]].ToString().Replace("\r\n", "")));
                                }
                                break;
                        }



                    }

                    if (i == dtAnaysic.Rows.Count - 1)
                    {
                        sb.Append("}");
                    }
                    else
                    {
                        sb.Append("},");
                    }
                }
                dtAnaysic = null;
                if (sb.ToString().EndsWith(","))
                {
                    sb = new StringBuilder(sb.ToString().Substring(0, sb.ToString().Length - 1));
                }
                sb.Append("]");
                sb.Append("}");
                
            }
            catch (Exception ex)
            {

            }
            return sb.ToString();
        }
    }
}
