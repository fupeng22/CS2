using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SQLDAL;
using CS.Filter;
using System.Data.SqlClient;
using System.Data;
using DBUtility;
using System.Text;
using Microsoft.Reporting.WebForms;
using System.IO;
using Model;

namespace CS.Controllers.Forwarder
{
    public class Forwarder_WayBillInputController : Controller
    {
        SQLDAL.T_WayBill tWayBill = new T_WayBill();
        SQLDAL.T_SubWayBill tSubWayBill = new T_SubWayBill();

        public const string strFileds = "wbSerialNum,wbTotalNumber,wbTotalWeight,wbVoyage,wbIOmark,wbChinese,wbEnglish,wbSubNumber,wbTransportMode,wbEntryDate,wbSRport,wbPortCode,wbStorageDate,wbCompany,wbStatus,wbID";
        public const string strFileds_Sub = "wbSerialNum,swbSerialNum,swbDescription_CHN,swbDescription_ENG,swbNumber,swbWeight,swbValue,swbMonetary,swbRecipients,swbCustomsCategory,TaxNo,TaxRate,TaxRateDescription,ActualTaxRate,CategoryNo,Sender,ReceiverIDCard,ReceiverPhone,EmailAddress,PickGoodsAgain,mismatchCargoName,belowFullPrice,above1000,parentID,ID,state,wbID,swbID";

        public const string STR_TEMPLATE_EXCEL = "~/Temp/Template/template.xls";
        public const string STR_REPORT_URL = "~/Content/Reports/ForwarderQueryCompany.rdlc";
        //
        // GET: /Forwarder_QueryCompany/
        [ForwarderRequiresLoginAttribute]
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
        public string GetData(string order, string page, string rows, string sort, string inputBeginDate, string inputEndDate, string txtGCode, string txtVoyage)
        {
            SqlParameter[] param = new SqlParameter[8];
            param[0] = new SqlParameter();
            param[0].SqlDbType = SqlDbType.VarChar;
            param[0].ParameterName = "@TableName";
            param[0].Direction = ParameterDirection.Input;
            param[0].Value = "V_Distinct_WayBill";

            param[1] = new SqlParameter();
            param[1].SqlDbType = SqlDbType.VarChar;
            param[1].ParameterName = "@FieldKey";
            param[1].Direction = ParameterDirection.Input;
            param[1].Value = "wbID";

            param[2] = new SqlParameter();
            param[2].SqlDbType = SqlDbType.VarChar;
            param[2].ParameterName = "@FieldShow";
            param[2].Direction = ParameterDirection.Input;
            param[2].Value = "*";

            param[3] = new SqlParameter();
            param[3].SqlDbType = SqlDbType.VarChar;
            param[3].ParameterName = "@FieldOrder";
            param[3].Direction = ParameterDirection.Input;
            param[3].Value = sort + " " + order;

            param[4] = new SqlParameter();
            param[4].SqlDbType = SqlDbType.Int;
            param[4].ParameterName = "@PageSize";
            param[4].Direction = ParameterDirection.Input;
            param[4].Value = Convert.ToInt32(rows);

            param[5] = new SqlParameter();
            param[5].SqlDbType = SqlDbType.Int;
            param[5].ParameterName = "@PageCurrent";
            param[5].Direction = ParameterDirection.Input;
            param[5].Value = Convert.ToInt32(page);

            param[6] = new SqlParameter();
            param[6].SqlDbType = SqlDbType.VarChar;
            param[6].ParameterName = "@Where";
            param[6].Direction = ParameterDirection.Input;

            txtGCode = Server.UrlDecode(txtGCode.ToString());
            inputBeginDate = Server.UrlDecode(inputBeginDate.ToString());
            inputEndDate = Server.UrlDecode(inputEndDate.ToString());
            txtVoyage = Server.UrlDecode(txtVoyage.ToString());

            string strWhereTemp = "";

            if (txtGCode.ToString() != "")
            {
                if (strWhereTemp != "")
                {
                    strWhereTemp = strWhereTemp + " and   (wbSerialNum like '%" + txtGCode.ToString() + "%') ";
                }
                else
                {
                    strWhereTemp = strWhereTemp + " (wbSerialNum like '%" + txtGCode.ToString() + "%') ";
                }
            }

            if (txtVoyage.ToString() != "" && txtVoyage.ToString() != "---请选择---")
            {
                if (strWhereTemp != "")
                {
                    strWhereTemp = strWhereTemp + "  and wbCompany like '%" + txtVoyage.ToString() + "%' ";
                }
                else
                {
                    strWhereTemp = strWhereTemp + "  wbCompany like '%" + txtVoyage.ToString() + "%' ";
                }
            }

            if (inputBeginDate.ToString() != "" && inputEndDate.ToString() != "")
            {
                if (strWhereTemp != "")
                {
                    strWhereTemp = strWhereTemp + "   and wbStorageDate>='" + Convert.ToDateTime(inputBeginDate).ToString("yyyyMMdd") + "' and wbStorageDate<='" + Convert.ToDateTime(inputEndDate).ToString("yyyyMMdd") + "' ";
                }
                else
                {
                    strWhereTemp = strWhereTemp + "  wbStorageDate>='" + Convert.ToDateTime(inputBeginDate).ToString("yyyyMMdd") + "' and wbStorageDate<='" + Convert.ToDateTime(inputEndDate).ToString("yyyyMMdd") + "' ";
                }
            }

            param[6].Value = strWhereTemp;

            param[7] = new SqlParameter();
            param[7].SqlDbType = SqlDbType.Int;
            param[7].ParameterName = "@RecordCount";
            param[7].Direction = ParameterDirection.Output;

            DataSet ds = SqlServerHelper.RunProcedure("spPageViewByStr", param, "result");
            DataTable dt = ds.Tables["result"];

            int releseNum = -1;
            int notReleseNum = -1;

            StringBuilder sb = new StringBuilder("");
            sb.Append("{");
            sb.AppendFormat("\"total\":{0}", Convert.ToInt32(param[7].Value.ToString()));
            sb.Append(",\"rows\":[");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                sb.Append("{");

                string[] strFiledArray = strFileds.Split(',');
                for (int j = 0; j < strFiledArray.Length; j++)
                {
                    switch (strFiledArray[j])
                    {
                        case "wbTotalWeight":
                            if (j != strFiledArray.Length - 1)
                            {
                                sb.AppendFormat("\"{0}\":\"{1}公斤\",", strFiledArray[j], dt.Rows[i][strFiledArray[j]] == DBNull.Value ? "" : (dt.Rows[i][strFiledArray[j]].ToString().Replace("\r\n", "")));
                            }
                            else
                            {
                                sb.AppendFormat("\"{0}\":\"{1}公斤\"", strFiledArray[j], dt.Rows[i][strFiledArray[j]] == DBNull.Value ? "" : (dt.Rows[i][strFiledArray[j]].ToString().Replace("\r\n", "")));
                            }
                            break;
                        default:
                            if (j != strFiledArray.Length - 1)
                            {
                                sb.AppendFormat("\"{0}\":\"{1}\",", strFiledArray[j], dt.Rows[i][strFiledArray[j]] == DBNull.Value ? "" : (dt.Rows[i][strFiledArray[j]].ToString().Replace("\r\n", "")));
                            }
                            else
                            {
                                sb.AppendFormat("\"{0}\":\"{1}\"", strFiledArray[j], dt.Rows[i][strFiledArray[j]] == DBNull.Value ? "" : (dt.Rows[i][strFiledArray[j]].ToString().Replace("\r\n", "")));
                            }
                            break;
                    }



                }

                if (i == dt.Rows.Count - 1)
                {
                    sb.Append("}");
                }
                else
                {
                    sb.Append("},");
                }
            }
            dt = null;
            if (sb.ToString().EndsWith(","))
            {
                sb = new StringBuilder(sb.ToString().Substring(0, sb.ToString().Length - 1));
            }
            sb.Append("]");
            sb.Append("}");
            return sb.ToString();
        }

        /// <summary>
        /// 分页查询类
        /// </summary>
        /// <param name="order"></param>
        /// <param name="page"></param>
        /// <param name="rows"></param>
        /// <param name="sort"></param>
        /// <returns></returns>
        public string GetData_Sub(string order, string page, string rows, string sort, string txtWbSerialNum_Sub, string txtSwbSerialNum_Sub, string id)
        {
            string strRet = "";
            if (string.IsNullOrEmpty(id))
            {
                strRet = GetData_Sub_Main(order, page, rows, sort, txtWbSerialNum_Sub, txtSwbSerialNum_Sub);
            }
            else
            {
                strRet = GetData_Sub_Detail(order, page, rows, sort, id);
            }
            return strRet;
        }

        /// <summary>
        /// 分页查询类
        /// </summary>
        /// <param name="order"></param>
        /// <param name="page"></param>
        /// <param name="rows"></param>
        /// <param name="sort"></param>
        /// <returns></returns>
        public string GetData_Sub_Main(string order, string page, string rows, string sort, string txtWbSerialNum_Sub, string txtSwbSerialNum_Sub)
        {
            SqlParameter[] param = new SqlParameter[8];
            param[0] = new SqlParameter();
            param[0].SqlDbType = SqlDbType.VarChar;
            param[0].ParameterName = "@TableName";
            param[0].Direction = ParameterDirection.Input;
            param[0].Value = "V_WayBill_SubWayBill";

            param[1] = new SqlParameter();
            param[1].SqlDbType = SqlDbType.VarChar;
            param[1].ParameterName = "@FieldKey";
            param[1].Direction = ParameterDirection.Input;
            param[1].Value = "swbID";

            param[2] = new SqlParameter();
            param[2].SqlDbType = SqlDbType.VarChar;
            param[2].ParameterName = "@FieldShow";
            param[2].Direction = ParameterDirection.Input;
            param[2].Value = "*";

            param[3] = new SqlParameter();
            param[3].SqlDbType = SqlDbType.VarChar;
            param[3].ParameterName = "@FieldOrder";
            param[3].Direction = ParameterDirection.Input;
            param[3].Value = sort + " " + order;

            param[4] = new SqlParameter();
            param[4].SqlDbType = SqlDbType.Int;
            param[4].ParameterName = "@PageSize";
            param[4].Direction = ParameterDirection.Input;
            param[4].Value = Convert.ToInt32(rows);

            param[5] = new SqlParameter();
            param[5].SqlDbType = SqlDbType.Int;
            param[5].ParameterName = "@PageCurrent";
            param[5].Direction = ParameterDirection.Input;
            param[5].Value = Convert.ToInt32(page);

            param[6] = new SqlParameter();
            param[6].SqlDbType = SqlDbType.VarChar;
            param[6].ParameterName = "@Where";
            param[6].Direction = ParameterDirection.Input;

            txtWbSerialNum_Sub = Server.UrlDecode(txtWbSerialNum_Sub.ToString());
            txtSwbSerialNum_Sub = Server.UrlDecode(txtSwbSerialNum_Sub.ToString());

            string strWhereTemp = "";

            if (txtWbSerialNum_Sub.ToString() != "")
            {
                if (strWhereTemp != "")
                {
                    strWhereTemp = strWhereTemp + " and  (wbSerialNum='" + txtWbSerialNum_Sub.ToString() + "') ";
                }
                else
                {
                    strWhereTemp = strWhereTemp + " (wbSerialNum='" + txtWbSerialNum_Sub.ToString() + "') ";
                }
            }

            if (txtSwbSerialNum_Sub.ToString() != "")
            {
                if (strWhereTemp != "")
                {
                    strWhereTemp = strWhereTemp + " and  (swbSerialNum like '%" + txtSwbSerialNum_Sub.ToString() + "%') ";
                }
                else
                {
                    strWhereTemp = strWhereTemp + " (swbSerialNum like '%" + txtSwbSerialNum_Sub.ToString() + "%') ";
                }
            }

            param[6].Value = strWhereTemp;

            param[7] = new SqlParameter();
            param[7].SqlDbType = SqlDbType.Int;
            param[7].ParameterName = "@RecordCount";
            param[7].Direction = ParameterDirection.Output;

            DataSet ds = SqlServerHelper.RunProcedure("spPageViewByStr", param, "result");
            DataTable dt = ds.Tables["result"];

            StringBuilder sb = new StringBuilder("");
            sb.Append("{");
            sb.AppendFormat("\"total\":{0}", Convert.ToInt32(param[7].Value.ToString()));
            sb.Append(",\"rows\":[");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                sb.Append("{");

                string[] strFiledArray = strFileds_Sub.Split(',');
                for (int j = 0; j < strFiledArray.Length; j++)
                {
                    switch (strFiledArray[j])
                    {
                        case "TaxRateDescription":
                            if (j != strFiledArray.Length - 1)
                            {
                                sb.AppendFormat("\"{0}\":\"{1}\",", strFiledArray[j], "");
                            }
                            else
                            {
                                sb.AppendFormat("\"{0}\":\"{1}\"", strFiledArray[j], "");
                            }
                            break;
                        case "swbDescription_CHN":
                            if (j != strFiledArray.Length - 1)
                            {
                                sb.AppendFormat("\"{0}\":\"{1}\",", strFiledArray[j], "");
                            }
                            else
                            {
                                sb.AppendFormat("\"{0}\":\"{1}\"", strFiledArray[j], "");
                            }
                            break;
                        case "swbDescription_ENG":
                            if (j != strFiledArray.Length - 1)
                            {
                                sb.AppendFormat("\"{0}\":\"{1}\",", strFiledArray[j], "");
                            }
                            else
                            {
                                sb.AppendFormat("\"{0}\":\"{1}\"", strFiledArray[j], "");
                            }
                            break;
                        case "swbNumber":
                            if (j != strFiledArray.Length - 1)
                            {
                                sb.AppendFormat("\"{0}\":\"{1}\",", strFiledArray[j], "");
                            }
                            else
                            {
                                sb.AppendFormat("\"{0}\":\"{1}\"", strFiledArray[j], "");
                            }
                            break;
                        case "swbWeight":
                            if (j != strFiledArray.Length - 1)
                            {
                                sb.AppendFormat("\"{0}\":\"{1}\",", strFiledArray[j], "");
                            }
                            else
                            {
                                sb.AppendFormat("\"{0}\":\"{1}\"", strFiledArray[j], "");
                            }
                            break;
                        case "swbValue":
                            if (j != strFiledArray.Length - 1)
                            {
                                sb.AppendFormat("\"{0}\":\"{1}\",", strFiledArray[j], "");
                            }
                            else
                            {
                                sb.AppendFormat("\"{0}\":\"{1}\"", strFiledArray[j], "");
                            }
                            break;
                        case "TaxNo":
                            if (j != strFiledArray.Length - 1)
                            {
                                sb.AppendFormat("\"{0}\":\"{1}\",", strFiledArray[j], "");
                            }
                            else
                            {
                                sb.AppendFormat("\"{0}\":\"{1}\"", strFiledArray[j], "");
                            }
                            break;
                        case "TaxRate":
                            if (j != strFiledArray.Length - 1)
                            {
                                sb.AppendFormat("\"{0}\":\"{1}\",", strFiledArray[j], "");
                            }
                            else
                            {
                                sb.AppendFormat("\"{0}\":\"{1}\"", strFiledArray[j], "");
                            }
                            break;
                        case "ActualTaxRate":
                            if (j != strFiledArray.Length - 1)
                            {
                                sb.AppendFormat("\"{0}\":\"{1}\",", strFiledArray[j], "");
                            }
                            else
                            {
                                sb.AppendFormat("\"{0}\":\"{1}\"", strFiledArray[j], "");
                            }
                            break;
                        case "CategoryNo":
                            if (j != strFiledArray.Length - 1)
                            {
                                sb.AppendFormat("\"{0}\":\"{1}\",", strFiledArray[j], "");
                            }
                            else
                            {
                                sb.AppendFormat("\"{0}\":\"{1}\"", strFiledArray[j], "");
                            }
                            break;
                        case "swbCustomsCategory":
                            if (j != strFiledArray.Length - 1)
                            {
                                sb.AppendFormat("\"{0}\":\"{1}\",", strFiledArray[j], new T_SubWayBill().CreateCategoryByType(dt.Rows[i][strFiledArray[j]] == DBNull.Value ? "" : (dt.Rows[i][strFiledArray[j]].ToString().Replace("\r\n", ""))));
                            }
                            else
                            {
                                sb.AppendFormat("\"{0}\":\"{1}\"", strFiledArray[j], new T_SubWayBill().CreateCategoryByType(dt.Rows[i][strFiledArray[j]] == DBNull.Value ? "" : (dt.Rows[i][strFiledArray[j]].ToString().Replace("\r\n", ""))));
                            }
                            break;
                        case "parentID":
                            if (j != strFiledArray.Length - 1)
                            {
                                sb.AppendFormat("\"{0}\":\"{1}\",", strFiledArray[j], "top");
                            }
                            else
                            {
                                sb.AppendFormat("\"{0}\":\"{1}\"", strFiledArray[j], "top");
                            }
                            break;
                        case "ID":
                            if (j != strFiledArray.Length - 1)
                            {
                                sb.AppendFormat("\"{0}\":\"{1}\",", strFiledArray[j], dt.Rows[i]["swbID"].ToString());
                            }
                            else
                            {
                                sb.AppendFormat("\"{0}\":\"{1}\"", strFiledArray[j], dt.Rows[i]["swbID"].ToString());
                            }
                            break;
                        case "state":
                            if (j != strFiledArray.Length - 1)
                            {
                                sb.AppendFormat("\"{0}\":\"{1}\",", strFiledArray[j], "closed");
                            }
                            else
                            {
                                sb.AppendFormat("\"{0}\":\"{1}\"", strFiledArray[j], "closed");
                            }
                            break;
                        case "mismatchCargoName":
                            if (j != strFiledArray.Length - 1)
                            {
                                sb.AppendFormat("\"{0}\":\"{1}\",", strFiledArray[j], "0");
                            }
                            else
                            {
                                sb.AppendFormat("\"{0}\":\"{1}\"", strFiledArray[j], "0");
                            }
                            break;
                        case "belowFullPrice":
                            if (j != strFiledArray.Length - 1)
                            {
                                sb.AppendFormat("\"{0}\":\"{1}\",", strFiledArray[j], "0");
                            }
                            else
                            {
                                sb.AppendFormat("\"{0}\":\"{1}\"", strFiledArray[j], "0");
                            }
                            break;
                        case "above1000":
                            if (j != strFiledArray.Length - 1)
                            {
                                sb.AppendFormat("\"{0}\":\"{1}\",", strFiledArray[j], "0");
                            }
                            else
                            {
                                sb.AppendFormat("\"{0}\":\"{1}\"", strFiledArray[j], "0");
                            }
                            break;
                        default:
                            if (j != strFiledArray.Length - 1)
                            {
                                sb.AppendFormat("\"{0}\":\"{1}\",", strFiledArray[j], dt.Rows[i][strFiledArray[j]] == DBNull.Value ? "" : (dt.Rows[i][strFiledArray[j]].ToString().Replace("\r\n", "")));
                            }
                            else
                            {
                                sb.AppendFormat("\"{0}\":\"{1}\"", strFiledArray[j], dt.Rows[i][strFiledArray[j]] == DBNull.Value ? "" : (dt.Rows[i][strFiledArray[j]].ToString().Replace("\r\n", "")));
                            }
                            break;
                    }
                }

                if (i == dt.Rows.Count - 1)
                {
                    sb.Append("}");
                }
                else
                {
                    sb.Append("},");
                }
            }
            dt = null;
            if (sb.ToString().EndsWith(","))
            {
                sb = new StringBuilder(sb.ToString().Substring(0, sb.ToString().Length - 1));
            }
            sb.Append("]");
            sb.Append("}");
            return sb.ToString();
        }

        /// <summary>
        /// 分页查询类
        /// </summary>
        /// <param name="order"></param>
        /// <param name="page"></param>
        /// <param name="rows"></param>
        /// <param name="sort"></param>
        /// <returns></returns>
        public string GetData_Sub_Detail(string order, string page, string rows, string sort, string id)
        {
            SqlParameter[] param = new SqlParameter[8];
            param[0] = new SqlParameter();
            param[0].SqlDbType = SqlDbType.VarChar;
            param[0].ParameterName = "@TableName";
            param[0].Direction = ParameterDirection.Input;
            param[0].Value = "V_SubWayBill_SubWayBillDetail";

            param[1] = new SqlParameter();
            param[1].SqlDbType = SqlDbType.VarChar;
            param[1].ParameterName = "@FieldKey";
            param[1].Direction = ParameterDirection.Input;
            param[1].Value = "swbdID";

            param[2] = new SqlParameter();
            param[2].SqlDbType = SqlDbType.VarChar;
            param[2].ParameterName = "@FieldShow";
            param[2].Direction = ParameterDirection.Input;
            param[2].Value = "*";

            param[3] = new SqlParameter();
            param[3].SqlDbType = SqlDbType.VarChar;
            param[3].ParameterName = "@FieldOrder";
            param[3].Direction = ParameterDirection.Input;
            param[3].Value = " swbDescription_CHN asc ";//sort + " " + order;

            param[4] = new SqlParameter();
            param[4].SqlDbType = SqlDbType.Int;
            param[4].ParameterName = "@PageSize";
            param[4].Direction = ParameterDirection.Input;
            rows = "1000";
            param[4].Value = Convert.ToInt32(rows);

            param[5] = new SqlParameter();
            param[5].SqlDbType = SqlDbType.Int;
            param[5].ParameterName = "@PageCurrent";
            param[5].Direction = ParameterDirection.Input;
            param[5].Value = 1;// Convert.ToInt32(page);

            param[6] = new SqlParameter();
            param[6].SqlDbType = SqlDbType.VarChar;
            param[6].ParameterName = "@Where";
            param[6].Direction = ParameterDirection.Input;

            string strWhereTemp = "";

            if (id.ToString() != "")
            {
                if (strWhereTemp != "")
                {
                    strWhereTemp = strWhereTemp + " and  (swbID=" + id.ToString() + ") ";
                }
                else
                {
                    strWhereTemp = strWhereTemp + " (swbID=" + id.ToString() + ") ";
                }
            }

            param[6].Value = strWhereTemp;

            param[7] = new SqlParameter();
            param[7].SqlDbType = SqlDbType.Int;
            param[7].ParameterName = "@RecordCount";
            param[7].Direction = ParameterDirection.Output;

            DataSet ds = SqlServerHelper.RunProcedure("spPageViewByStr", param, "result");
            DataTable dt = ds.Tables["result"];

            StringBuilder sb = new StringBuilder("");
            //sb.Append("{");
            //sb.AppendFormat("\"total\":{0}", Convert.ToInt32(param[7].Value.ToString()));
            //sb.Append(",\"rows\":[");
            sb.Append("[");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                sb.Append("{");

                string[] strFiledArray = strFileds_Sub.Split(',');
                for (int j = 0; j < strFiledArray.Length; j++)
                {
                    switch (strFiledArray[j])
                    {
                        case "swbWeight":
                            if (j != strFiledArray.Length - 1)
                            {
                                sb.AppendFormat("\"{0}\":\"{1}公斤\",", strFiledArray[j], dt.Rows[i][strFiledArray[j]] == DBNull.Value ? "" : (dt.Rows[i][strFiledArray[j]].ToString().Replace("\r\n", "")));
                            }
                            else
                            {
                                sb.AppendFormat("\"{0}\":\"{1}公斤\"", strFiledArray[j], dt.Rows[i][strFiledArray[j]] == DBNull.Value ? "" : (dt.Rows[i][strFiledArray[j]].ToString().Replace("\r\n", "")));
                            }
                            break;
                        case "parentID":
                            if (j != strFiledArray.Length - 1)
                            {
                                sb.AppendFormat("\"{0}\":\"{1}\",", strFiledArray[j], dt.Rows[i]["swbID"].ToString());
                            }
                            else
                            {
                                sb.AppendFormat("\"{0}\":\"{1}\"", strFiledArray[j], dt.Rows[i]["swbID"].ToString());
                            }
                            break;
                        case "ID":
                            //if (j != strFiledArray.Length - 1)
                            //{
                            //    sb.AppendFormat("\"{0}\":\"{1}\",", strFiledArray[j], dt.Rows[i]["swbID"].ToString() + "#" + i.ToString());
                            //}
                            //else
                            //{
                            //    sb.AppendFormat("\"{0}\":\"{1}\"", strFiledArray[j], dt.Rows[i]["swbID"].ToString() + "#" + i.ToString());
                            //}
                            if (j != strFiledArray.Length - 1)
                            {
                                sb.AppendFormat("\"{0}\":\"{1}\",", strFiledArray[j], dt.Rows[i]["swbdID"].ToString());
                            }
                            else
                            {
                                sb.AppendFormat("\"{0}\":\"{1}\"", strFiledArray[j], dt.Rows[i]["swbdID"].ToString());
                            }
                            break;
                        case "state":
                            if (j != strFiledArray.Length - 1)
                            {
                                sb.AppendFormat("\"{0}\":\"{1}\",", strFiledArray[j], "open");
                            }
                            else
                            {
                                sb.AppendFormat("\"{0}\":\"{1}\"", strFiledArray[j], "open");
                            }
                            break;
                        case "swbSerialNum":
                            if (j != strFiledArray.Length - 1)
                            {
                                sb.AppendFormat("\"{0}\":\"{1}\",", strFiledArray[j], dt.Rows[i]["swbSerialNum"].ToString());
                            }
                            else
                            {
                                sb.AppendFormat("\"{0}\":\"{1}\"", strFiledArray[j], dt.Rows[i]["swbSerialNum"].ToString());
                            }
                            break;
                        //case "swbMonetary":
                        //    if (j != strFiledArray.Length - 1)
                        //    {
                        //        sb.AppendFormat("\"{0}\":\"{1}\",", strFiledArray[j], "");
                        //    }
                        //    else
                        //    {
                        //        sb.AppendFormat("\"{0}\":\"{1}\"", strFiledArray[j], "");
                        //    }
                        //    break;
                        case "swbRecipients":
                            if (j != strFiledArray.Length - 1)
                            {
                                sb.AppendFormat("\"{0}\":\"{1}\",", strFiledArray[j], "");
                            }
                            else
                            {
                                sb.AppendFormat("\"{0}\":\"{1}\"", strFiledArray[j], "");
                            }
                            break;
                        case "Sender":
                            if (j != strFiledArray.Length - 1)
                            {
                                sb.AppendFormat("\"{0}\":\"{1}\",", strFiledArray[j], "");
                            }
                            else
                            {
                                sb.AppendFormat("\"{0}\":\"{1}\"", strFiledArray[j], "");
                            }
                            break;
                        case "PickGoodsAgain":
                            if (j != strFiledArray.Length - 1)
                            {
                                sb.AppendFormat("\"{0}\":\"{1}\",", strFiledArray[j], "");
                            }
                            else
                            {
                                sb.AppendFormat("\"{0}\":\"{1}\"", strFiledArray[j], "");
                            }
                            break;
                        case "ReceiverIDCard":
                            if (j != strFiledArray.Length - 1)
                            {
                                sb.AppendFormat("\"{0}\":\"{1}\",", strFiledArray[j], "");
                            }
                            else
                            {
                                sb.AppendFormat("\"{0}\":\"{1}\"", strFiledArray[j], "");
                            }
                            break;
                        case "ReceiverPhone":
                            if (j != strFiledArray.Length - 1)
                            {
                                sb.AppendFormat("\"{0}\":\"{1}\",", strFiledArray[j], "");
                            }
                            else
                            {
                                sb.AppendFormat("\"{0}\":\"{1}\"", strFiledArray[j], "");
                            }
                            break;
                        case "EmailAddress":
                            if (j != strFiledArray.Length - 1)
                            {
                                sb.AppendFormat("\"{0}\":\"{1}\",", strFiledArray[j], "");
                            }
                            else
                            {
                                sb.AppendFormat("\"{0}\":\"{1}\"", strFiledArray[j], "");
                            }
                            break;
                        case "swbCustomsCategory":
                            if (j != strFiledArray.Length - 1)
                            {
                                sb.AppendFormat("\"{0}\":\"{1}\",", strFiledArray[j], "");
                            }
                            else
                            {
                                sb.AppendFormat("\"{0}\":\"{1}\"", strFiledArray[j], "");
                            }
                            break;
                        default:
                            if (j != strFiledArray.Length - 1)
                            {
                                sb.AppendFormat("\"{0}\":\"{1}\",", strFiledArray[j], dt.Rows[i][strFiledArray[j]] == DBNull.Value ? "" : (dt.Rows[i][strFiledArray[j]].ToString().Replace("\r\n", "")));
                            }
                            else
                            {
                                sb.AppendFormat("\"{0}\":\"{1}\"", strFiledArray[j], dt.Rows[i][strFiledArray[j]] == DBNull.Value ? "" : (dt.Rows[i][strFiledArray[j]].ToString().Replace("\r\n", "")));
                            }
                            break;
                    }
                }

                if (i == dt.Rows.Count - 1)
                {
                    sb.Append("}");
                }
                else
                {
                    sb.Append("},");
                }
            }
            dt = null;
            if (sb.ToString().EndsWith(","))
            {
                sb = new StringBuilder(sb.ToString().Substring(0, sb.ToString().Length - 1));
            }
            sb.Append("]");
            //sb.Append("}");
            return sb.ToString();
        }

        [HttpGet]
        public ActionResult Print(string order, string page, string rows, string sort, string txtCode, string inputBeginDate, string inputEndDate, string txtGCode, string hidSearchType, string txtVoyage)
        {
            SqlParameter[] param = new SqlParameter[8];
            param[0] = new SqlParameter();
            param[0].SqlDbType = SqlDbType.VarChar;
            param[0].ParameterName = "@TableName";
            param[0].Direction = ParameterDirection.Input;
            param[0].Value = "V_Distinct_WayBill";

            param[1] = new SqlParameter();
            param[1].SqlDbType = SqlDbType.VarChar;
            param[1].ParameterName = "@FieldKey";
            param[1].Direction = ParameterDirection.Input;
            param[1].Value = "wbID";

            param[2] = new SqlParameter();
            param[2].SqlDbType = SqlDbType.VarChar;
            param[2].ParameterName = "@FieldShow";
            param[2].Direction = ParameterDirection.Input;
            param[2].Value = "*";

            param[3] = new SqlParameter();
            param[3].SqlDbType = SqlDbType.VarChar;
            param[3].ParameterName = "@FieldOrder";
            param[3].Direction = ParameterDirection.Input;
            param[3].Value = sort + " " + order;

            param[4] = new SqlParameter();
            param[4].SqlDbType = SqlDbType.Int;
            param[4].ParameterName = "@PageSize";
            param[4].Direction = ParameterDirection.Input;
            param[4].Value = Convert.ToInt32(rows);

            param[5] = new SqlParameter();
            param[5].SqlDbType = SqlDbType.Int;
            param[5].ParameterName = "@PageCurrent";
            param[5].Direction = ParameterDirection.Input;
            param[5].Value = Convert.ToInt32(page);

            param[6] = new SqlParameter();
            param[6].SqlDbType = SqlDbType.VarChar;
            param[6].ParameterName = "@Where";
            param[6].Direction = ParameterDirection.Input;

            txtCode = Server.UrlDecode(txtCode.ToString());
            txtGCode = Server.UrlDecode(txtGCode.ToString());
            inputBeginDate = Server.UrlDecode(inputBeginDate.ToString());
            inputEndDate = Server.UrlDecode(inputEndDate.ToString());
            txtVoyage = Server.UrlDecode(txtVoyage.ToString());

            string strWhereTemp = "";

            switch (hidSearchType.ToString())
            {
                case "-1"://没有选择条件,则返回空记录
                    strWhereTemp = " 1=2 ";
                    break;
                case "1"://选择普通选择
                    if (txtCode.ToString() != "")
                    {
                        if (strWhereTemp != "")
                        {
                            strWhereTemp = strWhereTemp + " and   (wbSerialNum like '%" + txtCode.ToString() + "%') ";
                        }
                        else
                        {
                            strWhereTemp = strWhereTemp + " (wbSerialNum like '%" + txtCode.ToString() + "%') ";
                        }
                    }
                    break;
                case "0"://选择高级查询
                    if (txtGCode.ToString() != "")
                    {
                        if (strWhereTemp != "")
                        {
                            strWhereTemp = strWhereTemp + " and   (wbSerialNum like '%" + txtGCode.ToString() + "%') ";
                        }
                        else
                        {
                            strWhereTemp = strWhereTemp + " (wbSerialNum like '%" + txtGCode.ToString() + "%') ";
                        }
                    }

                    if (txtVoyage.ToString() != "" && txtVoyage.ToString() != "---请选择---")
                    {
                        if (strWhereTemp != "")
                        {
                            strWhereTemp = strWhereTemp + "  and wbCompany like '%" + txtVoyage.ToString() + "%' ";
                        }
                        else
                        {
                            strWhereTemp = strWhereTemp + "  wbCompany like '%" + txtVoyage.ToString() + "%' ";
                        }
                    }

                    if (inputBeginDate.ToString() != "" && inputEndDate.ToString() != "")
                    {
                        if (strWhereTemp != "")
                        {
                            strWhereTemp = strWhereTemp + "   and wbStorageDate>='" + Convert.ToDateTime(inputBeginDate).ToString("yyyyMMdd") + "' and wbStorageDate<='" + Convert.ToDateTime(inputEndDate).ToString("yyyyMMdd") + "' ";
                        }
                        else
                        {
                            strWhereTemp = strWhereTemp + "  wbStorageDate>='" + Convert.ToDateTime(inputBeginDate).ToString("yyyyMMdd") + "' and wbStorageDate<='" + Convert.ToDateTime(inputEndDate).ToString("yyyyMMdd") + "' ";
                        }
                    }
                    break;
                default:
                    strWhereTemp = " 1=2 ";
                    break;
            }

            param[6].Value = strWhereTemp;

            param[7] = new SqlParameter();
            param[7].SqlDbType = SqlDbType.Int;
            param[7].ParameterName = "@RecordCount";
            param[7].Direction = ParameterDirection.Output;

            DataSet ds = SqlServerHelper.RunProcedure("spPageViewByStr", param, "result");
            DataTable dt = ds.Tables["result"];

            DataTable dtCustom = new DataTable();
            dtCustom.Columns.Add("wbStorageDate", Type.GetType("System.String"));
            dtCustom.Columns.Add("wbCompany", Type.GetType("System.String"));
            dtCustom.Columns.Add("wbSerialNum", Type.GetType("System.String"));
            dtCustom.Columns.Add("wbTotalNumber", Type.GetType("System.String"));
            dtCustom.Columns.Add("wbTotalWeight", Type.GetType("System.String"));
            dtCustom.Columns.Add("wbTotalNumber_Customize", Type.GetType("System.String"));
            dtCustom.Columns.Add("wbTotalWeight_Customize", Type.GetType("System.String"));
            dtCustom.Columns.Add("wbActualTotalWeight__Customize", Type.GetType("System.String"));
            dtCustom.Columns.Add("wbInStoreType_Customize", Type.GetType("System.String"));
            dtCustom.Columns.Add("wbStatus", Type.GetType("System.String"));
            dtCustom.Columns.Add("releseNum", Type.GetType("System.String"));
            dtCustom.Columns.Add("notReleseNum", Type.GetType("System.String"));
            dtCustom.Columns.Add("wbID", Type.GetType("System.String"));

            DataRow drCustom = null;

            int releseNum = -1;
            int notReleseNum = -1;

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                drCustom = dtCustom.NewRow();
                string[] strFiledArray = strFileds.Split(',');
                for (int j = 0; j < strFiledArray.Length; j++)
                {
                    switch (strFiledArray[j])
                    {
                        case "wbTotalWeight_Customize"://重量与件数以实际子运单中的总重量与总件数为准,并得到子运单实际重量,以备将来需要
                            string swbTotalNumber = "0";
                            string swbTotalActualWeight = "0";
                            string swbTotalWeight = "0";

                            DataSet dsSum = tSubWayBill.GetSubWayBillSumInfo(Convert.ToInt32(dt.Rows[i]["wbID"]));
                            if (dsSum != null)
                            {
                                DataTable dtSum = dsSum.Tables[0];
                                if (dtSum != null && dtSum.Rows.Count > 0)
                                {
                                    swbTotalNumber = dtSum.Rows[0]["swbTotalNumber"].ToString();
                                    swbTotalWeight = dtSum.Rows[0]["swbTotalWeight"].ToString();
                                    swbTotalActualWeight = dtSum.Rows[0]["swbTotalActualWeight"].ToString();
                                }
                            }
                            drCustom[strFiledArray[j]] = swbTotalWeight + "公斤";
                            drCustom["wbTotalNumber_Customize"] = swbTotalNumber;
                            drCustom["wbActualTotalWeight__Customize"] = swbTotalActualWeight + "公斤";
                            break;
                        case "wbTotalWeight":
                            drCustom[strFiledArray[j]] = dt.Rows[i][strFiledArray[j]] == DBNull.Value ? "" : (dt.Rows[i][strFiledArray[j]].ToString().Replace("\r\n", "")) + "公斤";
                            break;
                        case "wbTotalNumber_Customize":
                            break;
                        case "wbActualTotalWeight__Customize":
                            break;
                        case "wbInStoreType_Customize":
                            string str_wbInStoreType_Customize = "";
                            if (new T_WayBillFlow().IsInStore(dt.Rows[i]["wbID"].ToString()))
                            {
                                str_wbInStoreType_Customize = "已入库";
                            }
                            else
                            {
                                str_wbInStoreType_Customize = "未入库";
                            }
                            drCustom[strFiledArray[j]] = str_wbInStoreType_Customize;
                            break;
                        case "wbStatus":
                            releseNum = -1;
                            notReleseNum = -1;
                            if (j != strFiledArray.Length - 1)
                            {
                                if (dt.Rows[i][strFiledArray[j]].ToString() != "")
                                {
                                    switch (int.Parse(dt.Rows[i][strFiledArray[j]].ToString()))
                                    {
                                        case 0:
                                            drCustom[strFiledArray[j]] = dt.Rows[i][strFiledArray[j]] == DBNull.Value ? "" : "等待预检";
                                            break;
                                        case 1:
                                            drCustom[strFiledArray[j]] = dt.Rows[i][strFiledArray[j]] == DBNull.Value ? "" : "查验中";
                                            break;
                                        case 2:
                                            releseNum = tSubWayBill.GetActualReleseNum(int.Parse(dt.Rows[i]["wbID"].ToString()));
                                            notReleseNum = tSubWayBill.GetActualNotReleseNum(int.Parse(dt.Rows[i]["wbID"].ToString()));
                                            drCustom[strFiledArray[j]] = dt.Rows[i][strFiledArray[j]] == DBNull.Value ? "" : "已放行";
                                            break;
                                        default:
                                            drCustom[strFiledArray[j]] = dt.Rows[i][strFiledArray[j]] == DBNull.Value ? "" : "未知";
                                            break;
                                    }


                                    if (releseNum != -1)
                                    {
                                        drCustom["releseNum"] = releseNum.ToString();
                                    }
                                    else
                                    {
                                        drCustom["releseNum"] = "无";

                                    }
                                    if (notReleseNum != -1)
                                    {
                                        drCustom["notReleseNum"] = notReleseNum.ToString();

                                    }
                                    else
                                    {
                                        drCustom["notReleseNum"] = "无";
                                    }

                                }

                            }
                            else
                            {
                                if (dt.Rows[i][strFiledArray[j]].ToString() != "")
                                {
                                    switch (int.Parse(dt.Rows[i][strFiledArray[j]].ToString()))
                                    {
                                        case 0:
                                            drCustom[strFiledArray[j]] = dt.Rows[i][strFiledArray[j]] == DBNull.Value ? "" : "等待预检";
                                            break;
                                        case 1:
                                            drCustom[strFiledArray[j]] = dt.Rows[i][strFiledArray[j]] == DBNull.Value ? "" : "查验中";
                                            break;
                                        case 2:
                                            releseNum = tSubWayBill.GetActualReleseNum(int.Parse(dt.Rows[i]["wbID"].ToString()));
                                            notReleseNum = tSubWayBill.GetActualNotReleseNum(int.Parse(dt.Rows[i]["wbID"].ToString()));
                                            drCustom[strFiledArray[j]] = dt.Rows[i][strFiledArray[j]] == DBNull.Value ? "" : "已放行";
                                            break;
                                        default:
                                            drCustom[strFiledArray[j]] = dt.Rows[i][strFiledArray[j]] == DBNull.Value ? "" : "未知";
                                            break;
                                    }


                                    if (releseNum != -1)
                                    {
                                        drCustom["releseNum"] = releseNum.ToString();
                                    }
                                    else
                                    {
                                        drCustom["releseNum"] = "无";

                                    }
                                    if (notReleseNum != -1)
                                    {
                                        drCustom["notReleseNum"] = notReleseNum.ToString();

                                    }
                                    else
                                    {
                                        drCustom["notReleseNum"] = "无";
                                    }

                                }
                            }
                            break;
                        case "releseNum":
                            break;
                        case "notReleseNum":
                            break;
                        default:
                            drCustom[strFiledArray[j]] = dt.Rows[i][strFiledArray[j]] == DBNull.Value ? "" : (dt.Rows[i][strFiledArray[j]].ToString().Replace("\r\n", ""));
                            break;
                    }


                }
                if (drCustom["wbID"].ToString() != "")
                {
                    dtCustom.Rows.Add(drCustom);
                }
            }
            dt = null;

            LocalReport localReport = new LocalReport();
            localReport.ReportPath = Server.MapPath(STR_REPORT_URL);
            ReportDataSource reportDataSource = new ReportDataSource("ForwarderQueryCompany_DS", dtCustom);

            localReport.DataSources.Add(reportDataSource);
            string reportType = "PDF";
            string mimeType;
            string encoding = "UTF-8";
            string fileNameExtension;

            string deviceInfo = "<DeviceInfo>" +
                " <OutputFormat>PDF</OutputFormat>" +
                " <PageWidth>12in</PageWidth>" +
                " <PageHeigth>11in</PageHeigth>" +
                " <MarginTop>0.5in</MarginTop>" +
                " <MarginLeft>1in</MarginLeft>" +
                " <MarginRight>1in</MarginRight>" +
                " <MarginBottom>0.5in</MarginBottom>" +
                " </DeviceInfo>";

            Warning[] warnings;
            string[] streams;
            byte[] renderedBytes;

            renderedBytes = localReport.Render(reportType, deviceInfo, out mimeType, out encoding, out fileNameExtension, out streams, out warnings);

            return File(renderedBytes, mimeType);
        }

        [HttpGet]
        public ActionResult Excel(string order, string page, string rows, string sort, string txtCode, string inputBeginDate, string inputEndDate, string txtGCode, string hidSearchType, string txtVoyage, string browserType)
        {
            SqlParameter[] param = new SqlParameter[8];
            param[0] = new SqlParameter();
            param[0].SqlDbType = SqlDbType.VarChar;
            param[0].ParameterName = "@TableName";
            param[0].Direction = ParameterDirection.Input;
            param[0].Value = "V_Distinct_WayBill";

            param[1] = new SqlParameter();
            param[1].SqlDbType = SqlDbType.VarChar;
            param[1].ParameterName = "@FieldKey";
            param[1].Direction = ParameterDirection.Input;
            param[1].Value = "wbID";

            param[2] = new SqlParameter();
            param[2].SqlDbType = SqlDbType.VarChar;
            param[2].ParameterName = "@FieldShow";
            param[2].Direction = ParameterDirection.Input;
            param[2].Value = "*";

            param[3] = new SqlParameter();
            param[3].SqlDbType = SqlDbType.VarChar;
            param[3].ParameterName = "@FieldOrder";
            param[3].Direction = ParameterDirection.Input;
            param[3].Value = sort + " " + order;

            param[4] = new SqlParameter();
            param[4].SqlDbType = SqlDbType.Int;
            param[4].ParameterName = "@PageSize";
            param[4].Direction = ParameterDirection.Input;
            param[4].Value = Convert.ToInt32(rows);

            param[5] = new SqlParameter();
            param[5].SqlDbType = SqlDbType.Int;
            param[5].ParameterName = "@PageCurrent";
            param[5].Direction = ParameterDirection.Input;
            param[5].Value = Convert.ToInt32(page);

            param[6] = new SqlParameter();
            param[6].SqlDbType = SqlDbType.VarChar;
            param[6].ParameterName = "@Where";
            param[6].Direction = ParameterDirection.Input;

            txtCode = Server.UrlDecode(txtCode.ToString());
            txtGCode = Server.UrlDecode(txtGCode.ToString());
            inputBeginDate = Server.UrlDecode(inputBeginDate.ToString());
            inputEndDate = Server.UrlDecode(inputEndDate.ToString());
            txtVoyage = Server.UrlDecode(txtVoyage.ToString());

            string strWhereTemp = "";

            switch (hidSearchType.ToString())
            {
                case "-1"://没有选择条件,则返回空记录
                    strWhereTemp = " 1=2 ";
                    break;
                case "1"://选择普通选择
                    if (txtCode.ToString() != "")
                    {
                        if (strWhereTemp != "")
                        {
                            strWhereTemp = strWhereTemp + " and   (wbSerialNum like '%" + txtCode.ToString() + "%') ";
                        }
                        else
                        {
                            strWhereTemp = strWhereTemp + " (wbSerialNum like '%" + txtCode.ToString() + "%') ";
                        }
                    }
                    break;
                case "0"://选择高级查询
                    if (txtGCode.ToString() != "")
                    {
                        if (strWhereTemp != "")
                        {
                            strWhereTemp = strWhereTemp + " and   (wbSerialNum like '%" + txtGCode.ToString() + "%') ";
                        }
                        else
                        {
                            strWhereTemp = strWhereTemp + " (wbSerialNum like '%" + txtGCode.ToString() + "%') ";
                        }
                    }

                    if (txtVoyage.ToString() != "" && txtVoyage.ToString() != "---请选择---")
                    {
                        if (strWhereTemp != "")
                        {
                            strWhereTemp = strWhereTemp + "  and wbCompany like '%" + txtVoyage.ToString() + "%' ";
                        }
                        else
                        {
                            strWhereTemp = strWhereTemp + "  wbCompany like '%" + txtVoyage.ToString() + "%' ";
                        }
                    }

                    if (inputBeginDate.ToString() != "" && inputEndDate.ToString() != "")
                    {
                        if (strWhereTemp != "")
                        {
                            strWhereTemp = strWhereTemp + "   and wbStorageDate>='" + Convert.ToDateTime(inputBeginDate).ToString("yyyyMMdd") + "' and wbStorageDate<='" + Convert.ToDateTime(inputEndDate).ToString("yyyyMMdd") + "' ";
                        }
                        else
                        {
                            strWhereTemp = strWhereTemp + "  wbStorageDate>='" + Convert.ToDateTime(inputBeginDate).ToString("yyyyMMdd") + "' and wbStorageDate<='" + Convert.ToDateTime(inputEndDate).ToString("yyyyMMdd") + "' ";
                        }
                    }
                    break;
                default:
                    strWhereTemp = " 1=2 ";
                    break;
            }

            param[6].Value = strWhereTemp;

            param[7] = new SqlParameter();
            param[7].SqlDbType = SqlDbType.Int;
            param[7].ParameterName = "@RecordCount";
            param[7].Direction = ParameterDirection.Output;

            DataSet ds = SqlServerHelper.RunProcedure("spPageViewByStr", param, "result");
            DataTable dt = ds.Tables["result"];

            DataTable dtCustom = new DataTable();
            dtCustom.Columns.Add("wbStorageDate", Type.GetType("System.String"));
            dtCustom.Columns.Add("wbCompany", Type.GetType("System.String"));
            dtCustom.Columns.Add("wbSerialNum", Type.GetType("System.String"));
            dtCustom.Columns.Add("wbTotalNumber", Type.GetType("System.String"));
            dtCustom.Columns.Add("wbTotalWeight", Type.GetType("System.String"));
            dtCustom.Columns.Add("wbTotalNumber_Customize", Type.GetType("System.String"));
            dtCustom.Columns.Add("wbTotalWeight_Customize", Type.GetType("System.String"));
            dtCustom.Columns.Add("wbActualTotalWeight__Customize", Type.GetType("System.String"));
            dtCustom.Columns.Add("wbInStoreType_Customize", Type.GetType("System.String"));
            dtCustom.Columns.Add("wbStatus", Type.GetType("System.String"));
            dtCustom.Columns.Add("releseNum", Type.GetType("System.String"));
            dtCustom.Columns.Add("notReleseNum", Type.GetType("System.String"));
            dtCustom.Columns.Add("wbID", Type.GetType("System.String"));

            DataRow drCustom = null;

            int releseNum = -1;
            int notReleseNum = -1;

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                drCustom = dtCustom.NewRow();
                string[] strFiledArray = strFileds.Split(',');
                for (int j = 0; j < strFiledArray.Length; j++)
                {
                    switch (strFiledArray[j])
                    {
                        case "wbTotalWeight_Customize"://重量与件数以实际子运单中的总重量与总件数为准,并得到子运单实际重量,以备将来需要
                            string swbTotalNumber = "0";
                            string swbTotalActualWeight = "0";
                            string swbTotalWeight = "0";

                            DataSet dsSum = tSubWayBill.GetSubWayBillSumInfo(Convert.ToInt32(dt.Rows[i]["wbID"]));
                            if (dsSum != null)
                            {
                                DataTable dtSum = dsSum.Tables[0];
                                if (dtSum != null && dtSum.Rows.Count > 0)
                                {
                                    swbTotalNumber = dtSum.Rows[0]["swbTotalNumber"].ToString();
                                    swbTotalWeight = dtSum.Rows[0]["swbTotalWeight"].ToString();
                                    swbTotalActualWeight = dtSum.Rows[0]["swbTotalActualWeight"].ToString();
                                }
                            }
                            drCustom[strFiledArray[j]] = swbTotalWeight + "公斤";
                            drCustom["wbTotalNumber_Customize"] = swbTotalNumber;
                            drCustom["wbActualTotalWeight__Customize"] = swbTotalActualWeight + "公斤";
                            break;
                        case "wbTotalWeight":
                            drCustom[strFiledArray[j]] = dt.Rows[i][strFiledArray[j]] == DBNull.Value ? "" : (dt.Rows[i][strFiledArray[j]].ToString().Replace("\r\n", "")) + "公斤";
                            break;
                        case "wbTotalNumber_Customize":
                            break;
                        case "wbActualTotalWeight__Customize":
                            break;
                        case "wbInStoreType_Customize":
                            string str_wbInStoreType_Customize = "";
                            if (new T_WayBillFlow().IsInStore(dt.Rows[i]["wbID"].ToString()))
                            {
                                str_wbInStoreType_Customize = "已入库";
                            }
                            else
                            {
                                str_wbInStoreType_Customize = "未入库";
                            }
                            drCustom[strFiledArray[j]] = str_wbInStoreType_Customize;
                            break;
                        case "wbStatus":
                            releseNum = -1;
                            notReleseNum = -1;
                            if (j != strFiledArray.Length - 1)
                            {
                                if (dt.Rows[i][strFiledArray[j]].ToString() != "")
                                {
                                    switch (int.Parse(dt.Rows[i][strFiledArray[j]].ToString()))
                                    {
                                        case 0:
                                            drCustom[strFiledArray[j]] = dt.Rows[i][strFiledArray[j]] == DBNull.Value ? "" : "等待预检";
                                            break;
                                        case 1:
                                            drCustom[strFiledArray[j]] = dt.Rows[i][strFiledArray[j]] == DBNull.Value ? "" : "查验中";
                                            break;
                                        case 2:
                                            releseNum = tSubWayBill.GetActualReleseNum(int.Parse(dt.Rows[i]["wbID"].ToString()));
                                            notReleseNum = tSubWayBill.GetActualNotReleseNum(int.Parse(dt.Rows[i]["wbID"].ToString()));
                                            drCustom[strFiledArray[j]] = dt.Rows[i][strFiledArray[j]] == DBNull.Value ? "" : "已放行";
                                            break;
                                        default:
                                            drCustom[strFiledArray[j]] = dt.Rows[i][strFiledArray[j]] == DBNull.Value ? "" : "未知";
                                            break;
                                    }


                                    if (releseNum != -1)
                                    {
                                        drCustom["releseNum"] = releseNum.ToString();
                                    }
                                    else
                                    {
                                        drCustom["releseNum"] = "无";

                                    }
                                    if (notReleseNum != -1)
                                    {
                                        drCustom["notReleseNum"] = notReleseNum.ToString();

                                    }
                                    else
                                    {
                                        drCustom["notReleseNum"] = "无";
                                    }

                                }

                            }
                            else
                            {
                                if (dt.Rows[i][strFiledArray[j]].ToString() != "")
                                {
                                    switch (int.Parse(dt.Rows[i][strFiledArray[j]].ToString()))
                                    {
                                        case 0:
                                            drCustom[strFiledArray[j]] = dt.Rows[i][strFiledArray[j]] == DBNull.Value ? "" : "等待预检";
                                            break;
                                        case 1:
                                            drCustom[strFiledArray[j]] = dt.Rows[i][strFiledArray[j]] == DBNull.Value ? "" : "查验中";
                                            break;
                                        case 2:
                                            releseNum = tSubWayBill.GetActualReleseNum(int.Parse(dt.Rows[i]["wbID"].ToString()));
                                            notReleseNum = tSubWayBill.GetActualNotReleseNum(int.Parse(dt.Rows[i]["wbID"].ToString()));
                                            drCustom[strFiledArray[j]] = dt.Rows[i][strFiledArray[j]] == DBNull.Value ? "" : "已放行";
                                            break;
                                        default:
                                            drCustom[strFiledArray[j]] = dt.Rows[i][strFiledArray[j]] == DBNull.Value ? "" : "未知";
                                            break;
                                    }


                                    if (releseNum != -1)
                                    {
                                        drCustom["releseNum"] = releseNum.ToString();
                                    }
                                    else
                                    {
                                        drCustom["releseNum"] = "无";

                                    }
                                    if (notReleseNum != -1)
                                    {
                                        drCustom["notReleseNum"] = notReleseNum.ToString();

                                    }
                                    else
                                    {
                                        drCustom["notReleseNum"] = "无";
                                    }

                                }
                            }
                            break;
                        case "releseNum":
                            break;
                        case "notReleseNum":
                            break;
                        default:
                            drCustom[strFiledArray[j]] = dt.Rows[i][strFiledArray[j]] == DBNull.Value ? "" : (dt.Rows[i][strFiledArray[j]].ToString().Replace("\r\n", ""));
                            break;
                    }


                }
                if (drCustom["wbID"].ToString() != "")
                {
                    dtCustom.Rows.Add(drCustom);
                }
            }
            dt = null;
            LocalReport localReport = new LocalReport();
            localReport.ReportPath = Server.MapPath(STR_REPORT_URL);
            ReportDataSource reportDataSource = new ReportDataSource("ForwarderQueryCompany_DS", dtCustom);

            localReport.DataSources.Add(reportDataSource);

            Warning[] warnings;
            string[] streamids;
            string mimeType;
            string encoding;
            string extension;

            byte[] bytes = localReport.Render(
               "Excel", null, out mimeType, out encoding, out extension,
               out streamids, out warnings);
            string strFileName = Server.MapPath(STR_TEMPLATE_EXCEL);
            FileStream fs = new FileStream(strFileName, FileMode.Create);
            fs.Write(bytes, 0, bytes.Length);
            fs.Close();

            string strOutputFileName = "统计信息_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";

            switch (browserType.ToLower())
            {
                case "safari":
                    break;
                case "mozilla":
                    break;
                default:
                    strOutputFileName = HttpUtility.UrlEncode(strOutputFileName);
                    break;
            }

            return File(strFileName, "application/vnd.ms-excel", strOutputFileName);
        }

        [HttpPost]
        public string CreateWayBill(string txtwbSerialNum, string txtwbVoyage, string txtwbIOmark, string txtwbChinese, string txtwbEnglish, string txtwbTransportMode, string txtwbEntryDate, string txtwbSRport, string txtwbPortCode, string txtwbTotalWeight, string txtwbTotalNumber, string txtwbSubNumber)
        {
            string strRet = "{\"result\":\"error\",\"message\":\"提交失败，原因未知\"}";

            txtwbSerialNum = Server.UrlDecode(txtwbSerialNum);
            txtwbVoyage = Server.UrlDecode(txtwbVoyage);
            txtwbIOmark = Server.UrlDecode(txtwbIOmark);
            txtwbChinese = Server.UrlDecode(txtwbChinese);
            txtwbEnglish = Server.UrlDecode(txtwbEnglish);
            txtwbTransportMode = Server.UrlDecode(txtwbTransportMode);
            txtwbEntryDate = Server.UrlDecode(txtwbEntryDate);
            txtwbSRport = Server.UrlDecode(txtwbSRport);
            txtwbPortCode = Server.UrlDecode(txtwbPortCode);
            txtwbTotalWeight = Server.UrlDecode(txtwbTotalWeight);
            txtwbTotalNumber = Server.UrlDecode(txtwbTotalNumber);
            txtwbSubNumber = Server.UrlDecode(txtwbSubNumber);

            M_WayBill m_WayBill = new M_WayBill();
            try
            {
                //校验此总运单号之前是否使用
                if (!(new T_WayBill().ExistWbSerialNum(txtwbSerialNum)))
                {
                    strRet = "{\"result\":\"error\",\"message\":\"提交失败，此总运单号已经使用\"}";
                }
                else
                {
                    m_WayBill.WbSerialNum = txtwbSerialNum;
                    m_WayBill.WbSRPort = txtwbSRport;
                    m_WayBill.WbSubNumber = Convert.ToInt32(txtwbSubNumber);
                    m_WayBill.WbTotalNumber = Convert.ToInt32(txtwbTotalNumber);
                    m_WayBill.WbTotalWeight = Convert.ToDouble(txtwbTotalWeight);
                    m_WayBill.WbPortCode = txtwbPortCode;
                    m_WayBill.WbTransportMode = txtwbTransportMode;
                    m_WayBill.WbVoyage = txtwbVoyage;
                    m_WayBill.WbIOmark = txtwbIOmark;
                    m_WayBill.WbChinese = txtwbChinese;
                    m_WayBill.WbEnglish = txtwbEnglish;
                    m_WayBill.WbEntryDate = txtwbEntryDate;
                    m_WayBill.StorageDate = DateTime.Now.ToString("yyyyMMdd");

                    if (new T_WayBill().addWayBill(m_WayBill, Session["Global_Forwarder_UserName"] == null ? "" : Session["Global_Forwarder_UserName"].ToString()))
                    {
                        strRet = "{\"result\":\"ok\",\"message\":\"提交成功\"}";
                    }
                    else
                    {
                        strRet = "{\"result\":\"error\",\"message\":\"提交失败，原因未知\"}";
                    }
                }

            }
            catch (Exception ex)
            {
                strRet = "{\"result\":\"error\",\"message\":\"提交失败，原因:" + ex.Message + "\"}";
            }

            return strRet;
        }

        [HttpPost]
        public string UpdateWayBill(string txtwbSerialNum, string txtwbVoyage, string txtwbIOmark, string txtwbChinese, string txtwbEnglish, string txtwbTransportMode, string txtwbEntryDate, string txtwbSRport, string txtwbPortCode, string txtwbTotalWeight, string txtwbTotalNumber, string txtwbSubNumber, string txtwbId)
        {
            string strRet = "{\"result\":\"error\",\"message\":\"提交失败，原因未知\"}";

            txtwbSerialNum = Server.UrlDecode(txtwbSerialNum);
            txtwbVoyage = Server.UrlDecode(txtwbVoyage);
            txtwbIOmark = Server.UrlDecode(txtwbIOmark);
            txtwbChinese = Server.UrlDecode(txtwbChinese);
            txtwbEnglish = Server.UrlDecode(txtwbEnglish);
            txtwbTransportMode = Server.UrlDecode(txtwbTransportMode);
            txtwbEntryDate = Server.UrlDecode(txtwbEntryDate);
            txtwbSRport = Server.UrlDecode(txtwbSRport);
            txtwbPortCode = Server.UrlDecode(txtwbPortCode);
            txtwbTotalWeight = Server.UrlDecode(txtwbTotalWeight);
            txtwbTotalNumber = Server.UrlDecode(txtwbTotalNumber);
            txtwbSubNumber = Server.UrlDecode(txtwbSubNumber);
            txtwbId = Server.UrlDecode(txtwbId);

            M_WayBill m_WayBill = new M_WayBill();
            try
            {
                //校验此总运单号之前是否使用
                if (new T_WayBill().ExistWbSerialNum(txtwbSerialNum, Convert.ToInt32(txtwbId)))
                {
                    strRet = "{\"result\":\"error\",\"message\":\"提交失败，此总运单号已经使用\"}";
                }
                else
                {
                    m_WayBill.WbSerialNum = txtwbSerialNum;
                    m_WayBill.WbSRPort = txtwbSRport;
                    m_WayBill.WbSubNumber = Convert.ToInt32(txtwbSubNumber);
                    m_WayBill.WbTotalNumber = Convert.ToInt32(txtwbTotalNumber);
                    m_WayBill.WbTotalWeight = Convert.ToDouble(txtwbTotalWeight);
                    m_WayBill.WbPortCode = txtwbPortCode;
                    m_WayBill.WbTransportMode = txtwbTransportMode;
                    m_WayBill.WbVoyage = txtwbVoyage;
                    m_WayBill.WbIOmark = txtwbIOmark;
                    m_WayBill.WbChinese = txtwbChinese;
                    m_WayBill.WbEnglish = txtwbEnglish;
                    m_WayBill.WbEntryDate = txtwbEntryDate;
                    m_WayBill.StorageDate = DateTime.Now.ToString("yyyyMMdd");
                    m_WayBill.WbID = Convert.ToInt32(txtwbId);

                    if (new T_WayBill().updateWayBillBywbId(m_WayBill, Session["Global_Forwarder_UserName"] == null ? "" : Session["Global_Forwarder_UserName"].ToString()))
                    {
                        strRet = "{\"result\":\"ok\",\"message\":\"提交成功\"}";
                    }
                    else
                    {
                        strRet = "{\"result\":\"error\",\"message\":\"提交失败，原因未知\"}";
                    }
                }
            }
            catch (Exception ex)
            {
                strRet = "{\"result\":\"error\",\"message\":\"提交失败，原因:" + ex.Message + "\"}";
            }

            return strRet;
        }

        [HttpPost]
        public string DeleWayBill(string ids)
        {
            string strRet = "{\"result\":\"error\",\"message\":\"删除失败，原因未知\"}";

            ids = Server.UrlDecode(ids);

            try
            {
                if (new T_WayBill().DeleWayBill(ids))
                {
                    strRet = "{\"result\":\"ok\",\"message\":\"删除成功\"}";
                }
                else
                {
                    strRet = "{\"result\":\"error\",\"message\":\"删除失败，原因未知\"}";
                }
            }
            catch (Exception ex)
            {
                strRet = "{\"result\":\"error\",\"message\":\"删除失败，原因:" + ex.Message + "\"}";
            }

            return strRet;
        }

        [HttpPost]
        public string CreateSubWayBillMain(string hid_wbId_Sele, string txtSwbSerialNum_Main, string txtSwbRecipients, string txtSwbCustomsCategory_Main, string txtSender, string txtReceiverIDCard, string txtReceiverPhone, string txtEmailAddress)
        {
            string strRet = "{\"result\":\"error\",\"message\":\"提交失败，原因未知\"}";

            hid_wbId_Sele = Server.UrlDecode(hid_wbId_Sele);
            txtSwbSerialNum_Main = Server.UrlDecode(txtSwbSerialNum_Main);
            txtSwbRecipients = Server.UrlDecode(txtSwbRecipients);
            txtSwbCustomsCategory_Main = Server.UrlDecode(txtSwbCustomsCategory_Main);
            txtSender = Server.UrlDecode(txtSender);
            txtReceiverIDCard = Server.UrlDecode(txtReceiverIDCard);
            txtReceiverPhone = Server.UrlDecode(txtReceiverPhone);
            txtEmailAddress = Server.UrlDecode(txtEmailAddress);

            M_SubWayBill m_SubWayBill = new M_SubWayBill();
            try
            {
                //校验此总运单号之前是否使用
                if ((new T_SubWayBill().TestExistSwbSerialNum_Create(Convert.ToInt32(hid_wbId_Sele), txtSwbSerialNum_Main)))
                {
                    strRet = "{\"result\":\"error\",\"message\":\"提交失败，此分运单号已经使用\"}";
                }
                else
                {
                    m_SubWayBill.Swb_wbID = Convert.ToInt32(hid_wbId_Sele);
                    m_SubWayBill.SwbSerialNum = txtSwbSerialNum_Main;
                    m_SubWayBill.SwbDescription_CHN = "";
                    m_SubWayBill.SwbDescription_ENG = "";
                    m_SubWayBill.SwbNumber = 0;
                    m_SubWayBill.SwbWeight = 0.00;
                    m_SubWayBill.SwbValue = 0.00;
                    m_SubWayBill.SwbMonetary = "";
                    m_SubWayBill.SwbRecipients = txtSwbRecipients;
                    m_SubWayBill.SwbCustomsCategory = txtSwbCustomsCategory_Main;
                    m_SubWayBill.swbValueDetail = "";
                    m_SubWayBill.Sender = txtSender;
                    m_SubWayBill.ReceiverIDCard = txtReceiverIDCard;
                    m_SubWayBill.ReceiverPhone = txtReceiverPhone;
                    m_SubWayBill.EmailAddress = txtEmailAddress;
                    m_SubWayBill.PickGoodsAgain = new T_SubWayBill().IsPickGoodsAgain(txtReceiverIDCard);

                    if (new T_SubWayBill().addSubWayBill(m_SubWayBill))
                    {
                        strRet = "{\"result\":\"ok\",\"message\":\"提交成功\"}";
                    }
                    else
                    {
                        strRet = "{\"result\":\"error\",\"message\":\"提交失败，原因未知\"}";
                    }
                }

            }
            catch (Exception ex)
            {
                strRet = "{\"result\":\"error\",\"message\":\"提交失败，原因:" + ex.Message + "\"}";
            }

            return strRet;
        }

        [HttpPost]
        public string UpdateSubWayBillMain(string hid_swbId_Main, string hid_wbId_Sele, string txtSwbSerialNum_Main, string txtSwbRecipients, string txtSwbCustomsCategory_Main, string txtSender, string txtReceiverIDCard, string txtReceiverPhone, string txtEmailAddress)
        {
            string strRet = "{\"result\":\"error\",\"message\":\"提交失败，原因未知\"}";

            hid_swbId_Main = Server.UrlDecode(hid_swbId_Main);
            hid_wbId_Sele = Server.UrlDecode(hid_wbId_Sele);
            txtSwbSerialNum_Main = Server.UrlDecode(txtSwbSerialNum_Main);
            txtSwbRecipients = Server.UrlDecode(txtSwbRecipients);
            txtSwbCustomsCategory_Main = Server.UrlDecode(txtSwbCustomsCategory_Main);
            txtSender = Server.UrlDecode(txtSender);
            txtReceiverIDCard = Server.UrlDecode(txtReceiverIDCard);
            txtReceiverPhone = Server.UrlDecode(txtReceiverPhone);
            txtEmailAddress = Server.UrlDecode(txtEmailAddress);

            M_SubWayBill m_SubWayBill = new M_SubWayBill();
            try
            {
                //校验此总运单号之前是否使用
                if ((new T_SubWayBill().TestExistSwbSerialNum_Update(Convert.ToInt32(hid_swbId_Main), Convert.ToInt32(hid_wbId_Sele), txtSwbSerialNum_Main)))
                {
                    strRet = "{\"result\":\"error\",\"message\":\"提交失败，此分运单号已经使用\"}";
                }
                else
                {
                    m_SubWayBill.Swb_wbID = Convert.ToInt32(hid_wbId_Sele);
                    m_SubWayBill.SwbSerialNum = txtSwbSerialNum_Main;
                    m_SubWayBill.SwbDescription_CHN = "";
                    m_SubWayBill.SwbDescription_ENG = "";
                    m_SubWayBill.SwbNumber = 0;
                    m_SubWayBill.SwbWeight = 0.00;
                    m_SubWayBill.SwbValue = 0.00;
                    m_SubWayBill.SwbMonetary = "";
                    m_SubWayBill.SwbRecipients = txtSwbRecipients;
                    m_SubWayBill.SwbCustomsCategory = txtSwbCustomsCategory_Main;
                    m_SubWayBill.swbValueDetail = "";
                    m_SubWayBill.SwbID = Convert.ToInt32(hid_swbId_Main);
                    m_SubWayBill.Sender = txtSender;
                    m_SubWayBill.ReceiverIDCard = txtReceiverIDCard;
                    m_SubWayBill.ReceiverPhone = txtReceiverPhone;
                    m_SubWayBill.EmailAddress = txtEmailAddress;
                    m_SubWayBill.PickGoodsAgain = new T_SubWayBill().LoadLastPickGoodInfo(hid_swbId_Main) == null ? 0 : (new T_SubWayBill().LoadLastPickGoodInfo(hid_swbId_Main).Tables[0].Rows.Count > 0 ? 1 : 0);

                    if (new T_SubWayBill().updateSubWayBill(m_SubWayBill))
                    {
                        strRet = "{\"result\":\"ok\",\"message\":\"提交成功\"}";
                    }
                    else
                    {
                        strRet = "{\"result\":\"error\",\"message\":\"提交失败，原因未知\"}";
                    }
                }

            }
            catch (Exception ex)
            {
                strRet = "{\"result\":\"error\",\"message\":\"提交失败，原因:" + ex.Message + "\"}";
            }

            return strRet;
        }

        [HttpPost]
        public string DeleSubWayBillMain(string ids)
        {
            string strRet = "{\"result\":\"error\",\"message\":\"删除失败，原因未知\"}";

            ids = Server.UrlDecode(ids);

            try
            {
                if (new T_SubWayBill().DeleteSubWayBillByID(ids))
                {
                    strRet = "{\"result\":\"ok\",\"message\":\"删除成功\"}";
                }
                else
                {
                    strRet = "{\"result\":\"error\",\"message\":\"删除失败，原因未知\"}";
                }
            }
            catch (Exception ex)
            {
                strRet = "{\"result\":\"error\",\"message\":\"删除失败，原因:" + ex.Message + "\"}";
            }

            return strRet;
        }

        [HttpPost]
        public string CreateSubWayBillDetail(string hid_swbId_Detail, string txtSwbDescription_CHN_Detail, string txtSwbDescription_ENG_Detail, string txtSwbNumber_Detail, string txtSwbWeight_Detail, string txtSwbValue_Detail, string txtTaxNo_Detail, string txtTaxRate_Detail, string txtSwbMonetary_Detail)
        {
            string strRet = "{\"result\":\"error\",\"message\":\"提交失败，原因未知\"}";

            hid_swbId_Detail = Server.UrlDecode(hid_swbId_Detail);
            txtSwbDescription_CHN_Detail = Server.UrlDecode(txtSwbDescription_CHN_Detail);
            txtSwbDescription_ENG_Detail = Server.UrlDecode(txtSwbDescription_ENG_Detail);
            txtSwbNumber_Detail = Server.UrlDecode(txtSwbNumber_Detail);
            txtSwbWeight_Detail = Server.UrlDecode(txtSwbWeight_Detail);
            txtSwbValue_Detail = Server.UrlDecode(txtSwbValue_Detail);
            txtTaxNo_Detail = Server.UrlDecode(txtTaxNo_Detail);
            txtTaxRate_Detail = Server.UrlDecode(txtTaxRate_Detail);
            txtSwbMonetary_Detail = Server.UrlDecode(txtSwbMonetary_Detail);

            M_SubWayBillDetail m_SubWayBillDetail = new M_SubWayBillDetail();
            try
            {
                //校验此总运单号之前是否使用
                //if ((new T_SubWayBill().TestExistSwbSerialNum_Create(Convert.ToInt32(hid_wbId_Sele), txtSwbSerialNum_Main)))
                //{
                //    strRet = "{\"result\":\"error\",\"message\":\"提交失败，此分运单号已经使用\"}";
                //}
                //else
                //{
                m_SubWayBillDetail.swbd_swbID = Convert.ToInt32(hid_swbId_Detail);
                m_SubWayBillDetail.SwbDescription_CHN = txtSwbDescription_CHN_Detail;
                m_SubWayBillDetail.SwbDescription_ENG = txtSwbDescription_ENG_Detail;
                m_SubWayBillDetail.SwbNumber = Convert.ToInt32(txtSwbNumber_Detail);
                m_SubWayBillDetail.SwbWeight = Convert.ToDouble(txtSwbWeight_Detail);
                m_SubWayBillDetail.SwbValue = Convert.ToDouble(Convert.ToDouble(txtSwbValue_Detail).ToString("0.00"));
                m_SubWayBillDetail.SwbMonetary = txtSwbMonetary_Detail;
                m_SubWayBillDetail.SwbRecipients = "";
                m_SubWayBillDetail.SwbCustomsCategory = "";
                m_SubWayBillDetail.swbValueDetail = "";
                m_SubWayBillDetail.TaxNo = txtTaxNo_Detail;
                m_SubWayBillDetail.TaxRate = Convert.ToDouble(txtTaxRate_Detail); ;
                m_SubWayBillDetail.CategoryNo = new T_TaxRateSetting().GetCategoryByTaxNO(txtTaxNo_Detail);
                m_SubWayBillDetail.mismatchCargoName = new T_TaxRateSetting().IsMismatchCargoName(txtSwbDescription_CHN_Detail, txtTaxNo_Detail) ? 0 : 1;
                m_SubWayBillDetail.belowFullPrice = new T_TaxRateSetting().IsBelowFullValue(txtSwbValue_Detail, txtTaxNo_Detail) ? 1 : 0;
                m_SubWayBillDetail.above1000 = Convert.ToDouble(txtSwbValue_Detail) > 1000 ? 1 : 0;
                m_SubWayBillDetail.TaxValue = Convert.ToDouble(Convert.ToDouble(Convert.ToDouble(txtSwbValue_Detail) * Convert.ToDouble(txtTaxRate_Detail)).ToString("0.00"));

                if (new T_SubWayBillDetail().AddSubWayBillDetail(m_SubWayBillDetail))
                {
                    new T_SubWayBill().updateSwbValue(hid_swbId_Detail);
                    strRet = "{\"result\":\"ok\",\"message\":\"提交成功\"}";
                }
                else
                {
                    strRet = "{\"result\":\"error\",\"message\":\"提交失败，原因未知\"}";
                }
                //}

            }
            catch (Exception ex)
            {
                strRet = "{\"result\":\"error\",\"message\":\"提交失败，原因:" + ex.Message + "\"}";
            }

            return strRet;
        }

        [HttpPost]
        public string UpdateSubWayBillDetail(string hid_swbId_Detail, string hid_swbdId_Detail, string txtSwbDescription_CHN_Detail, string txtSwbDescription_ENG_Detail, string txtSwbNumber_Detail, string txtSwbWeight_Detail, string txtSwbValue_Detail, string txtTaxNo_Detail, string txtTaxRate_Detail, string txtSwbMonetary_Detail)
        {
            string strRet = "{\"result\":\"error\",\"message\":\"提交失败，原因未知\"}";

            hid_swbId_Detail = Server.UrlDecode(hid_swbId_Detail);
            hid_swbdId_Detail = Server.UrlDecode(hid_swbdId_Detail);
            txtSwbDescription_CHN_Detail = Server.UrlDecode(txtSwbDescription_CHN_Detail);
            txtSwbDescription_ENG_Detail = Server.UrlDecode(txtSwbDescription_ENG_Detail);
            txtSwbNumber_Detail = Server.UrlDecode(txtSwbNumber_Detail);
            txtSwbWeight_Detail = Server.UrlDecode(txtSwbWeight_Detail);
            txtSwbValue_Detail = Server.UrlDecode(txtSwbValue_Detail);
            txtTaxNo_Detail = Server.UrlDecode(txtTaxNo_Detail);
            txtTaxRate_Detail = Server.UrlDecode(txtTaxRate_Detail);
            txtSwbMonetary_Detail = Server.UrlDecode(txtSwbMonetary_Detail);

            M_SubWayBillDetail m_SubWayBillDetail = new M_SubWayBillDetail();
            try
            {
                //校验此总运单号之前是否使用
                //if ((new T_SubWayBill().TestExistSwbSerialNum_Create(Convert.ToInt32(hid_wbId_Sele), txtSwbSerialNum_Main)))
                //{
                //    strRet = "{\"result\":\"error\",\"message\":\"提交失败，此分运单号已经使用\"}";
                //}
                //else
                //{
                m_SubWayBillDetail.swbd_swbID = Convert.ToInt32(hid_swbId_Detail);
                m_SubWayBillDetail.SwbDescription_CHN = txtSwbDescription_CHN_Detail;
                m_SubWayBillDetail.SwbDescription_ENG = txtSwbDescription_ENG_Detail;
                m_SubWayBillDetail.SwbNumber = Convert.ToInt32(txtSwbNumber_Detail);
                m_SubWayBillDetail.SwbWeight = Convert.ToDouble(txtSwbWeight_Detail);
                m_SubWayBillDetail.SwbValue = Convert.ToDouble(Convert.ToDouble(txtSwbValue_Detail).ToString("0.00"));
                m_SubWayBillDetail.SwbMonetary = txtSwbMonetary_Detail;
                m_SubWayBillDetail.SwbRecipients = "";
                m_SubWayBillDetail.SwbCustomsCategory = "";
                m_SubWayBillDetail.swbValueDetail = "";
                m_SubWayBillDetail.TaxNo = txtTaxNo_Detail;
                m_SubWayBillDetail.TaxRate = Convert.ToDouble(txtTaxRate_Detail); ;
                m_SubWayBillDetail.CategoryNo = new T_TaxRateSetting().GetCategoryByTaxNO(txtTaxNo_Detail);
                m_SubWayBillDetail.swbdID = Convert.ToInt32(hid_swbdId_Detail);
                m_SubWayBillDetail.mismatchCargoName = new T_TaxRateSetting().IsMismatchCargoName(txtSwbDescription_CHN_Detail, txtTaxNo_Detail) ? 0 : 1;
                m_SubWayBillDetail.belowFullPrice = new T_TaxRateSetting().IsBelowFullValue(txtSwbValue_Detail, txtTaxNo_Detail) ? 1 : 0;
                m_SubWayBillDetail.above1000 = Convert.ToDouble(txtSwbValue_Detail) > 1000 ? 1 : 0;
                m_SubWayBillDetail.TaxValue = Convert.ToDouble(Convert.ToDouble(Convert.ToDouble(txtSwbValue_Detail) * Convert.ToDouble(txtTaxRate_Detail)).ToString("0.00"));


                if (new T_SubWayBillDetail().UpdateSubWayBillDetail(m_SubWayBillDetail))
                {
                    new T_SubWayBill().updateSwbValue(hid_swbId_Detail);
                    strRet = "{\"result\":\"ok\",\"message\":\"提交成功\"}";
                }
                else
                {
                    strRet = "{\"result\":\"error\",\"message\":\"提交失败，原因未知\"}";
                }
                //}

            }
            catch (Exception ex)
            {
                strRet = "{\"result\":\"error\",\"message\":\"提交失败，原因:" + ex.Message + "\"}";
            }

            return strRet;
        }

        [HttpPost]
        public string DeleSubWayBillDetail(string ids)
        {
            string strRet = "{\"result\":\"error\",\"message\":\"删除失败，原因未知\"}";

            ids = Server.UrlDecode(ids);

            try
            {
                if (new T_SubWayBillDetail().DeleteSubWayBillDetail(ids))
                {
                    strRet = "{\"result\":\"ok\",\"message\":\"删除成功\"}";
                }
                else
                {
                    strRet = "{\"result\":\"error\",\"message\":\"删除失败，原因未知\"}";
                }
            }
            catch (Exception ex)
            {
                strRet = "{\"result\":\"error\",\"message\":\"删除失败，原因:" + ex.Message + "\"}";
            }

            return strRet;
        }
    }
}
