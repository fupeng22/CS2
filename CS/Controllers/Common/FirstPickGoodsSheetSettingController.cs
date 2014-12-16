using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CS.Filter;
using System.Data.SqlClient;
using System.Data;
using DBUtility;
using System.Text;
using SQLDAL;
using Microsoft.Reporting.WebForms;
using System.IO;
using Util;
using Model;
using System.Configuration;
using System.Net.Mail;

namespace CS.Controllers.Common
{
    [ErrorAttribute]
    public class FirstPickGoodsSheetSettingController : Controller
    {
        //public const string strFileds = "wbCompany,wbStorageDate,wbSerialNum,swbSerialNum,DetainDate,swbDescription_CHN,swbDescription_ENG,swbNumber,swbWeight,swbNeedCheck,wbID,swbID";
        public const string strFileds = "wbSerialNum,wbCompany,wbStorageDate,swbSerialNum,DetainDate,swbNeedCheck,swbDescription_CHN,swbDescription_ENG,swbNumber,swbWeight,swbValue,swbMonetary,swbRecipients,swbCustomsCategory,TaxNo,TaxRate,TaxRateDescription,ActualTaxRate,CategoryNo,Sender,ReceiverIDCard,ReceiverPhone,EmailAddress,PickGoodsAgain,mismatchCargoName,belowFullPrice,above1000,chkNeedCheck,CheckResult,HandleSuggestion,CheckResultDescription,HandleSuggestionDescription,FinalCheckResultDescription,FinalHandleSuggestDescription,CheckResultOperator,IsConfirmCheck,IsConfirmCheckDescription,ConfirmCheckOperator,TaxValue,TaxValueCheck,TaxValueCheckOperator,swbValueTotal,parentID,ID,state,wbID,swbdID,swbID";

        //public const string strFileds_Main = "wbSerialNum,swbSerialNum,Sender,ReceiverIDCard,ReceiverPhone,EmailAddress,swbRecipients,swbCustomsCategory,swbCustomsCategory_Desc,wbStorageDate,wbCompany,PickGoodsAgain,PickGoodsAgain_Desc,wbID,swbID";
        public const string strFileds_Sub = "swbDescription_CHN,swbDescription_ENG,swbNumber,swbWeight,swbValue,swbMonetary,TaxNo,TaxRate,TaxRateDescription,mismatchCargoName,belowFullPrice,above1000,FinalCheckResultDescription,FinalHandleSuggestDescription,CheckResultOperator,swbID,swbdID";

        public const string STR_TEMPLATE_EXCEL = "~/Temp/Template/template.xls";
        public const string STR_REPORT_URL = "~/Content/Reports/FirstPickGoodsSheetSetting.rdlc";

        //public string STR_SENDER_SMTP = ConfigurationManager.AppSettings["SenderSMTP"];
        //public string STR_SENDER_USERNAME = ConfigurationManager.AppSettings["SenderUserName"];
        //public string STR_SENDER_USERMAIL = ConfigurationManager.AppSettings["SenderUserMail"];
        //public string STR_SENDER_USERPWD = ConfigurationManager.AppSettings["SenderPwd"];
        //public string STR_SENDER_RECIEVEREMAIL = ConfigurationManager.AppSettings["RecieverEmail"];
        //public string STR_SENDER_RECIEVERUSERNAME = ConfigurationManager.AppSettings["RecieverName"];
        //public string STR_CARBONCODY = ConfigurationManager.AppSettings["CarbonCopy"];
        public string STR_TIMEOUT = ConfigurationManager.AppSettings["MaxTimeOut"];
        //public string STR_MAILSUBJECT = ConfigurationManager.AppSettings["MailSubject"];
        //public string STR_MAILBODY = ConfigurationManager.AppSettings["MailBody"];
        //
        // GET: /UnReleaseSheetSettion/

        public ActionResult Index()
        {
            string strWBID = Request.QueryString["wbID"] == null ? "" : Request.QueryString["wbID"].ToString();
            SetViewData(Server.UrlDecode(strWBID));
            return View();
        }

        //public string GetData(string order, string page, string rows, string sort, string wbID, string swbNeedCheck)
        //{
        //    SqlParameter[] param = new SqlParameter[8];
        //    param[0] = new SqlParameter();
        //    param[0].SqlDbType = SqlDbType.VarChar;
        //    param[0].ParameterName = "@TableName";
        //    param[0].Direction = ParameterDirection.Input;
        //    param[0].Value = "V_WayBill_SubWayBill";

        //    param[1] = new SqlParameter();
        //    param[1].SqlDbType = SqlDbType.VarChar;
        //    param[1].ParameterName = "@FieldKey";
        //    param[1].Direction = ParameterDirection.Input;
        //    param[1].Value = "swbID";

        //    param[2] = new SqlParameter();
        //    param[2].SqlDbType = SqlDbType.VarChar;
        //    param[2].ParameterName = "@FieldShow";
        //    param[2].Direction = ParameterDirection.Input;
        //    param[2].Value = "*";

        //    param[3] = new SqlParameter();
        //    param[3].SqlDbType = SqlDbType.VarChar;
        //    param[3].ParameterName = "@FieldOrder";
        //    param[3].Direction = ParameterDirection.Input;
        //    param[3].Value = sort + " " + order;

        //    param[4] = new SqlParameter();
        //    param[4].SqlDbType = SqlDbType.Int;
        //    param[4].ParameterName = "@PageSize";
        //    param[4].Direction = ParameterDirection.Input;
        //    param[4].Value = Convert.ToInt32(rows);

        //    param[5] = new SqlParameter();
        //    param[5].SqlDbType = SqlDbType.Int;
        //    param[5].ParameterName = "@PageCurrent";
        //    param[5].Direction = ParameterDirection.Input;
        //    param[5].Value = Convert.ToInt32(page);

        //    param[6] = new SqlParameter();
        //    param[6].SqlDbType = SqlDbType.VarChar;
        //    param[6].ParameterName = "@Where";
        //    param[6].Direction = ParameterDirection.Input;

        //    string strWhereTemp = "";
        //    string strSwbNeedCheck = Server.UrlDecode(swbNeedCheck.ToString());
        //    string strWBID = Server.UrlDecode(wbID.ToString());

        //    if (strSwbNeedCheck != "")
        //    {
        //        if (strWhereTemp != "")
        //        {
        //            strWhereTemp = strWhereTemp + " and (swbNeedCheck =" + strSwbNeedCheck + ") ";
        //        }
        //        else
        //        {
        //            strWhereTemp = strWhereTemp + "  (swbNeedCheck =" + strSwbNeedCheck + ") ";
        //        }
        //    }

        //    if (strWBID != "")
        //    {
        //        if (strWhereTemp != "")
        //        {
        //            strWhereTemp = strWhereTemp + " and (wbID =" + strWBID + ") ";
        //        }
        //        else
        //        {
        //            strWhereTemp = strWhereTemp + "  (wbID =" + strWBID + ") ";
        //        }
        //    }

        //    param[6].Value = strWhereTemp;

        //    param[7] = new SqlParameter();
        //    param[7].SqlDbType = SqlDbType.Int;
        //    param[7].ParameterName = "@RecordCount";
        //    param[7].Direction = ParameterDirection.Output;

        //    DataSet ds = SqlServerHelper.RunProcedure("spPageViewByStr", param, "result");
        //    DataTable dt = ds.Tables["result"];

        //    StringBuilder sb = new StringBuilder("");
        //    sb.Append("{");
        //    sb.AppendFormat("\"total\":{0}", Convert.ToInt32(param[7].Value.ToString()));
        //    sb.Append(",\"rows\":[");
        //    for (int i = 0; i < dt.Rows.Count; i++)
        //    {
        //        sb.Append("{");

        //        string[] strFiledArray = strFileds.Split(',');
        //        for (int j = 0; j < strFiledArray.Length; j++)
        //        {
        //            switch (strFiledArray[j])
        //            {
        //                case "DetainDate":
        //                    if (j != strFiledArray.Length - 1)
        //                    {
        //                        sb.AppendFormat("\"{0}\":\"{1}\",", strFiledArray[j], dt.Rows[i][strFiledArray[j]] == DBNull.Value ? "" : (Convert.ToDateTime(dt.Rows[i][strFiledArray[j]]).ToString("yyyy-MM-dd").Replace("\r\n", "")));
        //                    }
        //                    else
        //                    {
        //                        sb.AppendFormat("\"{0}\":\"{1}\"", strFiledArray[j], dt.Rows[i][strFiledArray[j]] == DBNull.Value ? "" : (Convert.ToDateTime(dt.Rows[i][strFiledArray[j]]).ToString("yyyy-MM-dd").Replace("\r\n", "")));
        //                    }
        //                    break;
        //                default:
        //                    if (j != strFiledArray.Length - 1)
        //                    {
        //                        sb.AppendFormat("\"{0}\":\"{1}\",", strFiledArray[j], dt.Rows[i][strFiledArray[j]] == DBNull.Value ? "" : (dt.Rows[i][strFiledArray[j]].ToString().Replace("\r\n", "")));
        //                    }
        //                    else
        //                    {
        //                        sb.AppendFormat("\"{0}\":\"{1}\"", strFiledArray[j], dt.Rows[i][strFiledArray[j]] == DBNull.Value ? "" : (dt.Rows[i][strFiledArray[j]].ToString().Replace("\r\n", "")));
        //                    }
        //                    break;
        //            }
        //        }

        //        if (i == dt.Rows.Count - 1)
        //        {
        //            sb.Append("}");
        //        }
        //        else
        //        {
        //            sb.Append("},");
        //        }
        //    }
        //    dt = null;
        //    if (sb.ToString().EndsWith(","))
        //    {
        //        sb = new StringBuilder(sb.ToString().Substring(0, sb.ToString().Length - 1));
        //    }
        //    sb.Append("]");
        //    sb.Append("}");
        //    return sb.ToString();
        //}

        /// <summary>
        /// 分页查询类
        /// </summary>
        /// <param name="order"></param>
        /// <param name="page"></param>
        /// <param name="rows"></param>
        /// <param name="sort"></param>
        /// <returns></returns>
        public string GetData(string order, string page, string rows, string sort, string wbID, string swbNeedCheck, string id)
        {
            string strRet = "";
            if (string.IsNullOrEmpty(id))
            {
                strRet = GetData_Sub_Main(order, page, rows, sort, wbID, swbNeedCheck);
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
        public string GetData_Sub_Main(string order, string page, string rows, string sort, string wbID, string swbNeedCheck)
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

            string strWhereTemp = "";
            string strSwbNeedCheck = Server.UrlDecode(swbNeedCheck.ToString());
            string strWBID = Server.UrlDecode(wbID.ToString());

            if (strSwbNeedCheck != "")
            {
                if (strWhereTemp != "")
                {
                    strWhereTemp = strWhereTemp + " and (swbNeedCheck =" + strSwbNeedCheck + ") ";
                }
                else
                {
                    strWhereTemp = strWhereTemp + "  (swbNeedCheck =" + strSwbNeedCheck + ") ";
                }
            }

            if (strWBID != "")
            {
                if (strWhereTemp != "")
                {
                    strWhereTemp = strWhereTemp + " and (wbID =" + strWBID + ") ";
                }
                else
                {
                    strWhereTemp = strWhereTemp + "  (wbID =" + strWBID + ") ";
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

                string[] strFiledArray = strFileds.Split(',');
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
                        case "CheckResultOperator":
                            if (j != strFiledArray.Length - 1)
                            {
                                sb.AppendFormat("\"{0}\":\"{1}\",", strFiledArray[j], "");
                            }
                            else
                            {
                                sb.AppendFormat("\"{0}\":\"{1}\"", strFiledArray[j], "");
                            }
                            break;
                        case "chkNeedCheck":
                            if (Convert.ToInt32(dt.Rows[i]["swbNeedCheck"]) == 1)
                            {
                                if (j != strFiledArray.Length - 1)
                                {
                                    sb.AppendFormat("\"{0}\":\"{1}\",", strFiledArray[j], 1);
                                }
                                else
                                {
                                    sb.AppendFormat("\"{0}\":\"{1}\"", strFiledArray[j], 1);
                                }
                            }
                            else
                            {
                                if (j != strFiledArray.Length - 1)
                                {
                                    sb.AppendFormat("\"{0}\":\"{1}\",", strFiledArray[j], 0);
                                }
                                else
                                {
                                    sb.AppendFormat("\"{0}\":\"{1}\"", strFiledArray[j], 0);
                                }
                            }
                            break;
                        case "swbdID":
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
                        case "IsConfirmCheck":
                            if (j != strFiledArray.Length - 1)
                            {
                                sb.AppendFormat("\"{0}\":\"{1}\",", strFiledArray[j], "");
                            }
                            else
                            {
                                sb.AppendFormat("\"{0}\":\"{1}\"", strFiledArray[j], "");
                            }
                            break;
                        case "IsConfirmCheckDescription":
                            if (j != strFiledArray.Length - 1)
                            {
                                sb.AppendFormat("\"{0}\":\"{1}\",", strFiledArray[j], "");
                            }
                            else
                            {
                                sb.AppendFormat("\"{0}\":\"{1}\"", strFiledArray[j], "");
                            }
                            break;
                        case "ConfirmCheckOperator":
                            if (j != strFiledArray.Length - 1)
                            {
                                sb.AppendFormat("\"{0}\":\"{1}\",", strFiledArray[j], "");
                            }
                            else
                            {
                                sb.AppendFormat("\"{0}\":\"{1}\"", strFiledArray[j], "");
                            }
                            break;
                        case "TaxValue":
                            if (j != strFiledArray.Length - 1)
                            {
                                sb.AppendFormat("\"{0}\":\"{1}\",", strFiledArray[j], "");
                            }
                            else
                            {
                                sb.AppendFormat("\"{0}\":\"{1}\"", strFiledArray[j], "");
                            }
                            break;
                        case "swbValueTotal":
                            if (j != strFiledArray.Length - 1)
                            {
                                sb.AppendFormat("\"{0}\":\"{1}\",", strFiledArray[j], dt.Rows[i]["swbValue"].ToString());
                            }
                            else
                            {
                                sb.AppendFormat("\"{0}\":\"{1}\"", strFiledArray[j], dt.Rows[i]["swbValue"].ToString());
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
                        case "CheckResult":
                            if (j != strFiledArray.Length - 1)
                            {
                                sb.AppendFormat("\"{0}\":\"{1}\",", strFiledArray[j], "-99");
                            }
                            else
                            {
                                sb.AppendFormat("\"{0}\":\"{1}\"", strFiledArray[j], "-99");
                            }
                            break;
                        case "CheckResultDescription":
                            if (j != strFiledArray.Length - 1)
                            {
                                sb.AppendFormat("\"{0}\":\"{1}\",", strFiledArray[j], "");
                            }
                            else
                            {
                                sb.AppendFormat("\"{0}\":\"{1}\"", strFiledArray[j], "");
                            }
                            break;
                        case "HandleSuggestion":
                            if (j != strFiledArray.Length - 1)
                            {
                                sb.AppendFormat("\"{0}\":\"{1}\",", strFiledArray[j], "-99");
                            }
                            else
                            {
                                sb.AppendFormat("\"{0}\":\"{1}\"", strFiledArray[j], "-99");
                            }
                            break;
                        case "HandleSuggestionDescription":
                            if (j != strFiledArray.Length - 1)
                            {
                                sb.AppendFormat("\"{0}\":\"{1}\",", strFiledArray[j], "");
                            }
                            else
                            {
                                sb.AppendFormat("\"{0}\":\"{1}\"", strFiledArray[j], "");
                            }
                            break;
                        case "FinalCheckResultDescription":
                            if (j != strFiledArray.Length - 1)
                            {
                                sb.AppendFormat("\"{0}\":\"{1}\",", strFiledArray[j], "");
                            }
                            else
                            {
                                sb.AppendFormat("\"{0}\":\"{1}\"", strFiledArray[j], "");
                            }
                            break;
                        case "FinalHandleSuggestDescription":
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

                string[] strFiledArray = strFileds.Split(',');
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
                        case "chkNeedCheck":
                            if (j != strFiledArray.Length - 1)
                            {
                                sb.AppendFormat("\"{0}\":\"{1}\",", strFiledArray[j], "-1");
                            }
                            else
                            {
                                sb.AppendFormat("\"{0}\":\"{1}\"", strFiledArray[j], "-1");
                            }
                            break;
                        //case "wbSerialNum":
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
                        case "Operator":
                            if (j != strFiledArray.Length - 1)
                            {
                                sb.AppendFormat("\"{0}\":\"{1}\",", strFiledArray[j], "");
                            }
                            else
                            {
                                sb.AppendFormat("\"{0}\":\"{1}\"", strFiledArray[j], "");
                            }
                            break;
                        case "operateDate":
                            if (j != strFiledArray.Length - 1)
                            {
                                sb.AppendFormat("\"{0}\":\"{1}\",", strFiledArray[j], "");
                            }
                            else
                            {
                                sb.AppendFormat("\"{0}\":\"{1}\"", strFiledArray[j], "");
                            }
                            break;
                        case "WbfID":
                            if (j != strFiledArray.Length - 1)
                            {
                                sb.AppendFormat("\"{0}\":\"{1}\",", strFiledArray[j], "");
                            }
                            else
                            {
                                sb.AppendFormat("\"{0}\":\"{1}\"", strFiledArray[j], "");
                            }
                            break;
                        case "wbCompany":
                            if (j != strFiledArray.Length - 1)
                            {
                                sb.AppendFormat("\"{0}\":\"{1}\",", strFiledArray[j], "");
                            }
                            else
                            {
                                sb.AppendFormat("\"{0}\":\"{1}\"", strFiledArray[j], "");
                            }
                            break;
                        case "wbStorageDate":
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
                        case "IsConfirmCheckDescription":
                            string strIsConfirmCheck = dt.Rows[i]["IsConfirmCheck"] == DBNull.Value ? "" : (dt.Rows[i]["IsConfirmCheck"].ToString().Replace("\r\n", ""));
                            string strIsConfirmCheckDescription = "";
                            switch (strIsConfirmCheck)
                            {
                                case "0":
                                    strIsConfirmCheckDescription = "未审核";
                                    break;
                                case "1":
                                    strIsConfirmCheckDescription = "已审核";
                                    break;
                                default:
                                    break;
                            }
                            if (j != strFiledArray.Length - 1)
                            {
                                sb.AppendFormat("\"{0}\":\"{1}\",", strFiledArray[j], strIsConfirmCheckDescription);
                            }
                            else
                            {
                                sb.AppendFormat("\"{0}\":\"{1}\"", strFiledArray[j], strIsConfirmCheckDescription);
                            }
                            break;
                        case "TaxValueCheck":
                            if (j != strFiledArray.Length - 1)
                            {
                                sb.AppendFormat("\"{0}\":\"{1}\",", strFiledArray[j], "");
                            }
                            else
                            {
                                sb.AppendFormat("\"{0}\":\"{1}\"", strFiledArray[j], "");
                            }
                            break;
                        case "TaxValueCheckOperator":
                            if (j != strFiledArray.Length - 1)
                            {
                                sb.AppendFormat("\"{0}\":\"{1}\",", strFiledArray[j], "");
                            }
                            else
                            {
                                sb.AppendFormat("\"{0}\":\"{1}\"", strFiledArray[j], "");
                            }
                            break;
                        case "swbValueTotal":
                            if (j != strFiledArray.Length - 1)
                            {
                                sb.AppendFormat("\"{0}\":\"{1}\",", strFiledArray[j], "");
                            }
                            else
                            {
                                sb.AppendFormat("\"{0}\":\"{1}\"", strFiledArray[j], "");
                            }
                            break;
                        case "swbNeedCheck":
                            if (j != strFiledArray.Length - 1)
                            {
                                sb.AppendFormat("\"{0}\":\"{1}\",", strFiledArray[j], "");
                            }
                            else
                            {
                                sb.AppendFormat("\"{0}\":\"{1}\"", strFiledArray[j], "");
                            }
                            break;
                        case "DetainDate":
                            if (j != strFiledArray.Length - 1)
                            {
                                sb.AppendFormat("\"{0}\":\"{1}\",", strFiledArray[j], "");
                            }
                            else
                            {
                                sb.AppendFormat("\"{0}\":\"{1}\"", strFiledArray[j], "");
                            }
                            break;
                        case "swbNeedCheckDescription":
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

        protected void SetViewData(string wbID)
        {
            DataTable dt = null;
            DataSet ds = null;
            ds = (new T_SubWayBill()).getWayBill_SubWayBill(wbID, 3);
            string strAllUnReleaseSubwayBill = "";
            string wbSerialNum = "";

            string wbSerialNumber_ForPrint = "0";//总件数
            string InStoreDate_ForSetting = "";//入库日期
            string wbCompany_ForSetting = "";//客户名（即导入运单的单位）

            if (ds != null)
            {
                dt = ds.Tables[0];
                if (dt != null && dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        strAllUnReleaseSubwayBill = strAllUnReleaseSubwayBill + dt.Rows[i]["swbSerialNum"].ToString() + ",";
                    }
                }
            }
            if (strAllUnReleaseSubwayBill.EndsWith(","))
            {
                strAllUnReleaseSubwayBill = strAllUnReleaseSubwayBill.Substring(0, strAllUnReleaseSubwayBill.Length - 1);
            }

            ds = (new T_WayBill()).getWayBillInfo(wbID);
            if (ds != null)
            {
                dt = ds.Tables[0];
                if (dt != null && dt.Rows.Count > 0)
                {
                    wbSerialNum = dt.Rows[0]["wbSerialNum"].ToString();
                }
            }

            ViewData["wbSerialNum_ForSetting"] = wbSerialNum;
            ViewData["wbID_ForSetting"] = wbID;
            ViewData["OldUnReleaseSubWayBill"] = strAllUnReleaseSubwayBill;
            ViewData["FlowNum_ForSetting"] = DateTime.Now.ToString("yyyyMMddHHmmss") + wbSerialNum;

            //得到其入库日期，每票总运单的分运单入库日期都是一天，没有例外
            DataSet dsWayBillFlow_InStoreDate = new T_WayBillFlow().getTop1UnReleaseSubWayBill(wbID);
            if (dsWayBillFlow_InStoreDate != null)
            {
                DataTable dtWayBillFlow_InStoreDate = dsWayBillFlow_InStoreDate.Tables[0];
                if (dtWayBillFlow_InStoreDate != null && dtWayBillFlow_InStoreDate.Rows.Count > 0)
                {
                    switch (dtWayBillFlow_InStoreDate.Rows[0]["status"].ToString())
                    {
                        case "1":
                            try
                            {
                                InStoreDate_ForSetting = Convert.ToDateTime(dtWayBillFlow_InStoreDate.Rows[0]["InOutStoreDate"].ToString()).ToString("yyyy-MM-dd");
                            }
                            catch (Exception ex)
                            {

                            }
                            break;
                        default:
                            InStoreDate_ForSetting = "未入库或已出库";
                            break;
                    }
                }
            }
            ViewData["InStoreDate_ForSetting"] = InStoreDate_ForSetting;

            //得到总运单表中的总件数，总重量
            ds = new T_WayBill().GetWayBill(wbSerialNum);
            if (ds != null)
            {
                dt = ds.Tables[0];
                if (dt != null && dt.Rows.Count > 0)
                {
                    wbSerialNumber_ForPrint = dt.Rows[0]["wbTotalNumber"].ToString();
                    wbCompany_ForSetting = dt.Rows[0]["wbCompany"].ToString();
                }
            }

            ViewData["wbSerialNumber_ForPrint"] = wbSerialNumber_ForPrint;
            ViewData["wbCompany_ForSetting"] = wbCompany_ForSetting;

            //计算分运单统计信息
            ComputeSubWayBillInfo(wbID);
        }

        public string ComputeSubWayBillInfo(string wbID)
        {
            string resulr = "error";
            string message = "计算失败，原因未知";
            string swbTotalNumber_ForPrint = "0";//分运单件数
            string ReleaseNum_ForSetting = "0";//放行件数
            string UnReleaseNum_ForSetting = "0";//扣留件数
            string wbActualWeight_ForPrint = "0.00";//重量
            string CustomCategory_ForSetting = "";//业务类型
            string hid_CustomCategory_ForSetting = "";//隐藏业务类型
            string OperateFee_ForSetting = "0.00";//操作费
            string PickGoodsFee_ForSetting = "0.00";//提货费
            string KeepGoodsFee_ForSetting = "0.00";//仓储费
            string ShiftGoodsFee_ForSetting = "0.00";//移库费
            string strRet = "";


            DataSet ds_SubWayBill = null;
            DataTable dt = null;
            if (wbID != "")
            {
                ds_SubWayBill = new T_SubWayBill().GetSubWayBillSumInfo(Convert.ToInt32(wbID));
                if (ds_SubWayBill != null)
                {
                    dt = ds_SubWayBill.Tables[0];
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        swbTotalNumber_ForPrint = dt.Rows[0]["swbTotalNumber"].ToString();

                    }
                }

                ds_SubWayBill = new T_SubWayBill().getWayBill_SubWayBill(wbID, 3);
                if (ds_SubWayBill != null)
                {
                    dt = ds_SubWayBill.Tables[0];
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        UnReleaseNum_ForSetting = dt.Rows.Count.ToString();

                    }
                }

                ds_SubWayBill = new T_SubWayBill().getWayBill_SubWayBill(wbID, 0);
                if (ds_SubWayBill != null)
                {
                    dt = ds_SubWayBill.Tables[0];
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        ReleaseNum_ForSetting = (Convert.ToInt32(ReleaseNum_ForSetting) + dt.Rows.Count).ToString(); ;
                    }
                }

                ds_SubWayBill = new T_SubWayBill().getWayBill_SubWayBill(wbID, 2);
                if (ds_SubWayBill != null)
                {
                    dt = ds_SubWayBill.Tables[0];
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        ReleaseNum_ForSetting = (Convert.ToInt32(ReleaseNum_ForSetting) + dt.Rows.Count).ToString(); ;
                    }
                }

                resulr = "ok";
            }

            wbActualWeight_ForPrint = new T_WayBillWeight().getWeightForCompute(wbID);

            ds_SubWayBill = new T_SubWayBill().GetSubWayBillInfoBywbID(Convert.ToInt32(wbID));
            if (ds_SubWayBill != null)
            {
                dt = ds_SubWayBill.Tables[0];
                if (dt != null && dt.Rows.Count > 0)
                {
                    CustomCategory_ForSetting = dt.Rows[0]["swbCustomsCategory"].ToString();
                    switch (CustomCategory_ForSetting)
                    {
                        case "2":
                            CustomCategory_ForSetting = "样品";
                            break;
                        case "3":
                            CustomCategory_ForSetting = "KJ-3";
                            break;
                        case "4":
                            CustomCategory_ForSetting = "D类";
                            break;
                        case "5":
                            CustomCategory_ForSetting = "个人物品";
                            break;
                        case "6":
                            CustomCategory_ForSetting = "分运行李";
                            break;
                        default:
                            break;
                    }
                    hid_CustomCategory_ForSetting = dt.Rows[0]["swbCustomsCategory"].ToString();
                }
            }

            ViewData["swbTotalNumber_ForPrint"] = swbTotalNumber_ForPrint;
            ViewData["UnReleaseNum_ForSetting"] = UnReleaseNum_ForSetting;
            ViewData["ReleaseNum_ForSetting"] = ReleaseNum_ForSetting;
            ViewData["CustomCategory_ForSetting"] = CustomCategory_ForSetting;
            ViewData["hid_CustomCategory_ForSetting"] = hid_CustomCategory_ForSetting;
            ViewData["wbActualWeight_ForPrint"] = wbActualWeight_ForPrint;

            //计算费用
            switch (hid_CustomCategory_ForSetting)
            {
                case "2"://样品
                    //操作费
                    OperateFee_ForSetting = (Convert.ToDouble(wbActualWeight_ForPrint) * Convert.ToDouble(new T_FeeRate().GetFeeRateValue("2-1-1"))).ToString("0.00");
                    //提货费
                    PickGoodsFee_ForSetting = (Convert.ToDouble(wbActualWeight_ForPrint) * Convert.ToDouble(new T_FeeRate().GetFeeRateValue("2-1-2"))).ToString("0.00");
                    //仓储费
                    KeepGoodsFee_ForSetting = ((DateTimeHelper.DateDiff(EnumDateCompare.day, Convert.ToDateTime(ViewData["InStoreDate_ForSetting"].ToString()), Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"))) - 3 <= 0 ? 0 : DateTimeHelper.DateDiff(EnumDateCompare.day, Convert.ToDateTime(ViewData["InStoreDate_ForSetting"].ToString()), Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"))) - 3) * Convert.ToDouble(ReleaseNum_ForSetting) * Convert.ToDouble(new T_FeeRate().GetFeeRateValue("2-1-3"))).ToString("0.00");

                    break;
                case "3"://KJ-3
                    //操作费
                    OperateFee_ForSetting = (Convert.ToDouble(wbActualWeight_ForPrint) * Convert.ToDouble(new T_FeeRate().GetFeeRateValue("2-1-1"))).ToString("0.00");
                    //提货费
                    PickGoodsFee_ForSetting = (Convert.ToDouble(wbActualWeight_ForPrint) * Convert.ToDouble(new T_FeeRate().GetFeeRateValue("2-1-2"))).ToString("0.00");
                    //仓储费
                    KeepGoodsFee_ForSetting = ((DateTimeHelper.DateDiff(EnumDateCompare.day, Convert.ToDateTime(ViewData["InStoreDate_ForSetting"].ToString()), Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"))) - 3 <= 0 ? 0 : DateTimeHelper.DateDiff(EnumDateCompare.day, Convert.ToDateTime(ViewData["InStoreDate_ForSetting"].ToString()), Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"))) - 3) * Convert.ToDouble(ReleaseNum_ForSetting) * Convert.ToDouble(new T_FeeRate().GetFeeRateValue("2-1-3"))).ToString("0.00");

                    break;
                case "4"://D类
                    //操作费
                    OperateFee_ForSetting = (Convert.ToDouble(wbActualWeight_ForPrint) * Convert.ToDouble(new T_FeeRate().GetFeeRateValue("2-1-1"))).ToString("0.00");
                    //提货费
                    PickGoodsFee_ForSetting = (Convert.ToDouble(wbActualWeight_ForPrint) * Convert.ToDouble(new T_FeeRate().GetFeeRateValue("2-1-2"))).ToString("0.00");
                    //仓储费
                    KeepGoodsFee_ForSetting = ((DateTimeHelper.DateDiff(EnumDateCompare.day, Convert.ToDateTime(ViewData["InStoreDate_ForSetting"].ToString()), Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"))) - 3 <= 0 ? 0 : DateTimeHelper.DateDiff(EnumDateCompare.day, Convert.ToDateTime(ViewData["InStoreDate_ForSetting"].ToString()), Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"))) - 3) * Convert.ToDouble(ReleaseNum_ForSetting) * Convert.ToDouble(new T_FeeRate().GetFeeRateValue("2-1-3"))).ToString("0.00");

                    break;
                case "5"://个人物品
                    //操作费
                    OperateFee_ForSetting = (Convert.ToDouble(wbActualWeight_ForPrint) * Convert.ToDouble(new T_FeeRate().GetFeeRateValue("5-1-1"))).ToString("0.00");
                    //提货费
                    PickGoodsFee_ForSetting = (Convert.ToDouble(wbActualWeight_ForPrint) * Convert.ToDouble(new T_FeeRate().GetFeeRateValue("5-1-2"))).ToString("0.00");
                    //仓储费
                    KeepGoodsFee_ForSetting = ((DateTimeHelper.DateDiff(EnumDateCompare.day, Convert.ToDateTime(ViewData["InStoreDate_ForSetting"].ToString()), Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"))) - 7 <= 0 ? 0 : DateTimeHelper.DateDiff(EnumDateCompare.day, Convert.ToDateTime(ViewData["InStoreDate_ForSetting"].ToString()), Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"))) - 7) * Convert.ToDouble(wbActualWeight_ForPrint) * Convert.ToDouble(new T_FeeRate().GetFeeRateValue("5-1-5"))).ToString("0.00");

                    break;
                case "6"://分运行李
                    //操作费
                    OperateFee_ForSetting = (Convert.ToDouble(wbActualWeight_ForPrint) * Convert.ToDouble(new T_FeeRate().GetFeeRateValue("6-1-1"))).ToString("0.00");
                    //提货费
                    PickGoodsFee_ForSetting = (Convert.ToDouble(wbActualWeight_ForPrint) * Convert.ToDouble(new T_FeeRate().GetFeeRateValue("6-1-2"))).ToString("0.00");
                    //仓储费
                    KeepGoodsFee_ForSetting = ((DateTimeHelper.DateDiff(EnumDateCompare.day, Convert.ToDateTime(ViewData["InStoreDate_ForSetting"].ToString()), Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"))) - 7 <= 0 ? 0 : DateTimeHelper.DateDiff(EnumDateCompare.day, Convert.ToDateTime(ViewData["InStoreDate_ForSetting"].ToString()), Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"))) - 7) * Convert.ToDouble(wbActualWeight_ForPrint) * Convert.ToDouble(new T_FeeRate().GetFeeRateValue("6-1-5"))).ToString("0.00");

                    break;
                default:
                    break;
            }
            ViewData["OperateFee_ForSetting"] = OperateFee_ForSetting;
            ViewData["PickGoodsFee_ForSetting"] = PickGoodsFee_ForSetting;
            ViewData["KeepGoodsFee_ForSetting"] = KeepGoodsFee_ForSetting;
            ViewData["ShiftGoodsFee_ForSetting"] = ShiftGoodsFee_ForSetting;

            strRet = "{\"result\":\"" + resulr + "\",\"message\":\"" + message + "\",\"row\":[{\"swbTotalNumber_ForPrint\":\"" + swbTotalNumber_ForPrint + "\",\"UnReleaseNum_ForSetting\":\"" + UnReleaseNum_ForSetting + "\",\"ReleaseNum_ForSetting\":\"" + ReleaseNum_ForSetting + "\",\"wbActualWeight_ForPrint\":\"" + wbActualWeight_ForPrint + "\",\"OperateFee_ForSetting\":\"" + OperateFee_ForSetting + "\",\"CustomCategory_ForSetting\":\"" + CustomCategory_ForSetting + "\",\"hid_CustomCategory_ForSetting\":\"" + hid_CustomCategory_ForSetting + "\",\"PickGoodsFee_ForSetting\":\"" + PickGoodsFee_ForSetting + "\",\"KeepGoodsFee_ForSetting\":\"" + KeepGoodsFee_ForSetting + "\",\"ShiftGoodsFee_ForSetting\":\"" + ShiftGoodsFee_ForSetting + "\"}]}";
            return strRet;
        }

        /// <summary>
        /// 打印提货单
        /// </summary>
        /// <param name="strCurrentReleaseSubWayBill">本次放行的分运单信息(单号1,单号2……)</param>
        /// <param name="strWBID"></param>
        /// <param name="iPrintType">0:打印预览   1:确认打印</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Print(string strWBID, int iPrintType, string wbSerialNum_ForPrint, string FlowNum_ForPrint, string wbSerialNumber_ForPrint, string swbTotalNumber_ForPrint, string ReleaseNum_ForSetting, string UnReleaseNum_ForSetting, string wbActualWeight_ForPrint, string InStoreDate_ForSetting, string PickGoodsDate_ForSetting, string OperateFee_ForSetting, string PickGoodsFee_ForSetting, string KeepGoodsFee_ForSetting, string ShiftGoodsFee_ForSetting, string CollectionFee_ForSetting, string TotalFee_ForSetting, string ddlPayMode_ForSetting)
        {
            DataSet dsAllUnReleaseSubWayBill = new DataSet();

            dsAllUnReleaseSubWayBill = SqlServerHelper.Query(string.Format(" select swbSerialNum,swbDescription_CHN,swbDescription_ENG,swbNumber,swbWeight,convert(nvarchar(10),DetainDate,120) DetainDate,swbID from  V_WayBill_SubWayBill where wbID={0} and swbNeedCheck=3 ", strWBID));

            string str_wbSerialNum_ForPrint = wbSerialNum_ForPrint == null ? "" : Server.UrlDecode(wbSerialNum_ForPrint);
            string str_FlowNum_ForPrint = FlowNum_ForPrint == null ? "" : Server.UrlDecode(FlowNum_ForPrint);
            string str_wbSerialNumber_ForPrint = wbSerialNumber_ForPrint == null ? "" : Server.UrlDecode(wbSerialNumber_ForPrint);
            string str_swbTotalNumber_ForPrint = swbTotalNumber_ForPrint == null ? "" : Server.UrlDecode(swbTotalNumber_ForPrint);
            string str_ReleaseNum_ForSetting = ReleaseNum_ForSetting == null ? "" : Server.UrlDecode(ReleaseNum_ForSetting);
            string str_UnReleaseNum_ForSetting = UnReleaseNum_ForSetting == null ? "" : Server.UrlDecode(UnReleaseNum_ForSetting);
            string str_wbActualWeight_ForPrint = wbActualWeight_ForPrint == null ? "" : Server.UrlDecode(wbActualWeight_ForPrint);
            string str_InStoreDate_ForSetting = InStoreDate_ForSetting == null ? "" : Server.UrlDecode(InStoreDate_ForSetting);
            string str_PickGoodsDate_ForSetting = PickGoodsDate_ForSetting == null ? "" : Server.UrlDecode(PickGoodsDate_ForSetting);
            string str_OperateFee_ForSetting = OperateFee_ForSetting == null ? "" : Server.UrlDecode(OperateFee_ForSetting);
            string str_PickGoodsFee_ForSetting = PickGoodsFee_ForSetting == null ? "" : Server.UrlDecode(PickGoodsFee_ForSetting);
            string str_KeepGoodsFee_ForSetting = KeepGoodsFee_ForSetting == null ? "" : Server.UrlDecode(KeepGoodsFee_ForSetting);
            string str_ShiftGoodsFee_ForSetting = ShiftGoodsFee_ForSetting == null ? "" : Server.UrlDecode(ShiftGoodsFee_ForSetting);
            string str_CollectionFee_ForSetting = CollectionFee_ForSetting == null ? "" : Server.UrlDecode(CollectionFee_ForSetting);
            string str_TotalFee_ForSetting = TotalFee_ForSetting == null ? "" : Server.UrlDecode(TotalFee_ForSetting);
            string str_ddlPayMode_ForSetting = ddlPayMode_ForSetting == null ? "" : Server.UrlDecode(ddlPayMode_ForSetting);

            LocalReport localReport = new LocalReport();
            localReport.ReportPath = Server.MapPath(STR_REPORT_URL);
            ReportDataSource dtAllUnReleaseSubWayBill = new ReportDataSource("FirstPickGoodsSheetSetting_DS", dsAllUnReleaseSubWayBill.Tables[0]);

            ReportParameter var_wbSerialNum_ForPrint = new ReportParameter("wbSerialNum_ForPrint", str_wbSerialNum_ForPrint.ToString());
            ReportParameter var_FlowNum_ForPrint = new ReportParameter("FlowNum_ForPrint", str_FlowNum_ForPrint.ToString());
            ReportParameter var_wbSerialNumber_ForPrint = new ReportParameter("wbSerialNumber_ForPrint", str_wbSerialNumber_ForPrint.ToString());
            ReportParameter var_swbTotalNumber_ForPrint = new ReportParameter("swbTotalNumber_ForPrint", str_swbTotalNumber_ForPrint.ToString());
            ReportParameter var_ReleaseNum_ForSetting = new ReportParameter("ReleaseNum_ForSetting", str_ReleaseNum_ForSetting.ToString());
            ReportParameter var_UnReleaseNum_ForSetting = new ReportParameter("UnReleaseNum_ForSetting", str_UnReleaseNum_ForSetting.ToString());
            ReportParameter var_wbActualWeight_ForPrint = new ReportParameter("wbActualWeight_ForPrint", str_wbActualWeight_ForPrint.ToString());
            ReportParameter var_InStoreDate_ForSetting = new ReportParameter("InStoreDate_ForSetting", str_InStoreDate_ForSetting.ToString());
            ReportParameter var_PickGoodsDate_ForSetting = new ReportParameter("PickGoodsDate_ForSetting", str_PickGoodsDate_ForSetting.ToString());
            ReportParameter var_OperateFee_ForSetting = new ReportParameter("OperateFee_ForSetting", str_OperateFee_ForSetting.ToString());
            ReportParameter var_PickGoodsFee_ForSetting = new ReportParameter("PickGoodsFee_ForSetting", str_PickGoodsFee_ForSetting.ToString());
            ReportParameter var_KeepGoodsFee_ForSetting = new ReportParameter("KeepGoodsFee_ForSetting", str_KeepGoodsFee_ForSetting.ToString());
            ReportParameter var_ShiftGoodsFee_ForSetting = new ReportParameter("ShiftGoodsFee_ForSetting", str_ShiftGoodsFee_ForSetting.ToString());
            ReportParameter var_CollectionFee_ForSetting = new ReportParameter("CollectionFee_ForSetting", str_CollectionFee_ForSetting.ToString());
            ReportParameter var_TotalFee_ForSetting = new ReportParameter("TotalFee_ForSetting", str_TotalFee_ForSetting.ToString());
            ReportParameter var_ddlPayMode_ForSetting = new ReportParameter("ddlPayMode_ForSetting", str_ddlPayMode_ForSetting.ToString());

            localReport.SetParameters(new ReportParameter[] { var_wbSerialNum_ForPrint });
            localReport.SetParameters(new ReportParameter[] { var_FlowNum_ForPrint });
            localReport.SetParameters(new ReportParameter[] { var_wbSerialNumber_ForPrint });
            localReport.SetParameters(new ReportParameter[] { var_swbTotalNumber_ForPrint });
            localReport.SetParameters(new ReportParameter[] { var_ReleaseNum_ForSetting });
            localReport.SetParameters(new ReportParameter[] { var_UnReleaseNum_ForSetting });
            localReport.SetParameters(new ReportParameter[] { var_wbActualWeight_ForPrint });
            localReport.SetParameters(new ReportParameter[] { var_InStoreDate_ForSetting });
            localReport.SetParameters(new ReportParameter[] { var_PickGoodsDate_ForSetting });
            localReport.SetParameters(new ReportParameter[] { var_OperateFee_ForSetting });
            localReport.SetParameters(new ReportParameter[] { var_PickGoodsFee_ForSetting });
            localReport.SetParameters(new ReportParameter[] { var_KeepGoodsFee_ForSetting });
            localReport.SetParameters(new ReportParameter[] { var_ShiftGoodsFee_ForSetting });
            localReport.SetParameters(new ReportParameter[] { var_CollectionFee_ForSetting });
            localReport.SetParameters(new ReportParameter[] { var_TotalFee_ForSetting });
            localReport.SetParameters(new ReportParameter[] { var_ddlPayMode_ForSetting });

            localReport.SubreportProcessing += new SubreportProcessingEventHandler(LocalReport_SubreportProcessing);

            localReport.DataSources.Add(dtAllUnReleaseSubWayBill);
            string reportType = "PDF";
            string mimeType;
            string encoding = "UTF-8";
            string fileNameExtension;

            string deviceInfo = "<DeviceInfo>" +
                " <OutputFormat>PDF</OutputFormat>" +
                " <PageWidth>12in</PageWidth>" +
                " <PageHeigth>6in</PageHeigth>" +
                " <MarginTop>0.2in</MarginTop>" +
                " <MarginLeft>1in</MarginLeft>" +
                " <MarginRight>1in</MarginRight>" +
                " <MarginBottom>0.2in</MarginBottom>" +
                " </DeviceInfo>";

            Warning[] warnings;
            string[] streams;
            byte[] renderedBytes;

            renderedBytes = localReport.Render(reportType, deviceInfo, out mimeType, out encoding, out fileNameExtension, out streams, out warnings);

            return File(renderedBytes, mimeType);
        }

        public DataTable GetData_Sub_Detail(string strSwbId)
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
            param[3].Value = " swbDescription_CHN asc ";

            param[4] = new SqlParameter();
            param[4].SqlDbType = SqlDbType.Int;
            param[4].ParameterName = "@PageSize";
            param[4].Direction = ParameterDirection.Input;
            param[4].Value = 1000;

            param[5] = new SqlParameter();
            param[5].SqlDbType = SqlDbType.Int;
            param[5].ParameterName = "@PageCurrent";
            param[5].Direction = ParameterDirection.Input;
            param[5].Value = 1;

            param[6] = new SqlParameter();
            param[6].SqlDbType = SqlDbType.VarChar;
            param[6].ParameterName = "@Where";
            param[6].Direction = ParameterDirection.Input;

            string strWhereTemp = "";

            if (strSwbId.ToString() != "")
            {
                if (strWhereTemp != "")
                {
                    strWhereTemp = strWhereTemp + " and  (swbID=" + strSwbId.ToString() + ") ";
                }
                else
                {
                    strWhereTemp = strWhereTemp + " (swbID=" + strSwbId.ToString() + ") ";
                }
            }

            param[6].Value = strWhereTemp;

            param[7] = new SqlParameter();
            param[7].SqlDbType = SqlDbType.Int;
            param[7].ParameterName = "@RecordCount";
            param[7].Direction = ParameterDirection.Output;

            DataSet ds = SqlServerHelper.RunProcedure("spPageViewByStr", param, "result");
            DataTable dt = ds.Tables["result"];

            DataTable dtCustom = new DataTable();
            dtCustom.Columns.Add("swbDescription_CHN", Type.GetType("System.String"));
            dtCustom.Columns.Add("swbDescription_ENG", Type.GetType("System.String"));
            dtCustom.Columns.Add("swbNumber", Type.GetType("System.String"));
            dtCustom.Columns.Add("swbWeight", Type.GetType("System.String"));
            dtCustom.Columns.Add("swbValue", Type.GetType("System.String"));
            dtCustom.Columns.Add("TaxNo", Type.GetType("System.String"));
            dtCustom.Columns.Add("TaxRate", Type.GetType("System.String"));
            dtCustom.Columns.Add("TaxRateDescription", Type.GetType("System.String"));
            dtCustom.Columns.Add("swbMonetary", Type.GetType("System.String"));
            dtCustom.Columns.Add("mismatchCargoName", Type.GetType("System.String"));
            dtCustom.Columns.Add("belowFullPrice", Type.GetType("System.String"));
            dtCustom.Columns.Add("above1000", Type.GetType("System.String"));
            dtCustom.Columns.Add("FinalCheckResultDescription", Type.GetType("System.String"));
            dtCustom.Columns.Add("FinalHandleSuggestDescription", Type.GetType("System.String"));
            dtCustom.Columns.Add("CheckResultOperator", Type.GetType("System.String"));
            dtCustom.Columns.Add("swbID", Type.GetType("System.Int32"));
            dtCustom.Columns.Add("swbdID", Type.GetType("System.String"));

            DataRow drCustom = null;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                drCustom = dtCustom.NewRow();
                string[] strFiledArray = strFileds_Sub.Split(',');
                for (int j = 0; j < strFiledArray.Length; j++)
                {
                    switch (strFiledArray[j])
                    {
                        case "mismatchCargoName":
                            string str_mismatchCargoName = "";
                            switch (dt.Rows[i]["mismatchCargoName"].ToString())
                            {
                                case "1":
                                    str_mismatchCargoName = "否";
                                    break;
                                case "0":
                                    str_mismatchCargoName = "是";
                                    break;
                                default:
                                    break;
                            }
                            drCustom[strFiledArray[j]] = str_mismatchCargoName;
                            break;
                        case "belowFullPrice":
                            string str_belowFullPrice = "";
                            switch (dt.Rows[i]["belowFullPrice"].ToString())
                            {
                                case "1":
                                    str_belowFullPrice = "是";
                                    break;
                                case "0":
                                    str_belowFullPrice = "否";
                                    break;
                                default:
                                    break;
                            }
                            drCustom[strFiledArray[j]] = str_belowFullPrice;
                            break;
                        case "above1000":
                            string str_above1000 = "";
                            switch (dt.Rows[i]["above1000"].ToString())
                            {
                                case "1":
                                    str_above1000 = "是";
                                    break;
                                case "0":
                                    str_above1000 = "否";
                                    break;
                                default:
                                    break;
                            }
                            drCustom[strFiledArray[j]] = str_above1000;
                            break;
                        default:
                            drCustom[strFiledArray[j]] = dt.Rows[i][strFiledArray[j]] == DBNull.Value ? "" : (dt.Rows[i][strFiledArray[j]].ToString().Replace("\r\n", ""));
                            break;
                    }
                }

                if (drCustom["swbdID"].ToString() != "")
                {
                    dtCustom.Rows.Add(drCustom);
                }
            }

            return dtCustom;
        }

        void LocalReport_SubreportProcessing(object sender, Microsoft.Reporting.WebForms.SubreportProcessingEventArgs e)
        {
            string strSwbId = e.Parameters["SubWayBillDetail_SwbId"].Values[0];

            DataTable dtSubWayBillDetail_Sub = GetData_Sub_Detail(strSwbId);

            e.DataSources.Add(new Microsoft.Reporting.WebForms.ReportDataSource("FirstPickGoodsSheetSetting_Detail_DS", dtSubWayBillDetail_Sub));
        }

        [HttpPost]
        public string SendMail_PDF(string strWBID, int iPrintType, string wbSerialNum_ForPrint, string FlowNum_ForPrint, string wbSerialNumber_ForPrint, string swbTotalNumber_ForPrint, string ReleaseNum_ForSetting, string UnReleaseNum_ForSetting, string wbActualWeight_ForPrint, string InStoreDate_ForSetting, string PickGoodsDate_ForSetting, string OperateFee_ForSetting, string PickGoodsFee_ForSetting, string KeepGoodsFee_ForSetting, string ShiftGoodsFee_ForSetting, string CollectionFee_ForSetting, string TotalFee_ForSetting, string ddlPayMode_ForSetting)
        {
            string strResult = "{\"result\":\"error\",\"message\":\"发送邮件失败，原因未知\"}";
            string wbCompany = "";
            try
            {
                DataSet dsWayBill = new T_WayBill().getWayBillInfo(strWBID);
                if (dsWayBill != null)
                {
                    if (dsWayBill.Tables[0] != null && dsWayBill.Tables[0].Rows.Count > 0)
                    {
                        wbCompany = dsWayBill.Tables[0].Rows[0]["wbCompany"].ToString();
                    }
                }

                if (!string.IsNullOrEmpty(wbCompany))
                {
                    DataSet dsUser = new T_User().GetUserByCompany(wbCompany);
                    if (dsUser != null)
                    {
                        if (dsUser.Tables[0] != null && dsUser.Tables[0].Rows.Count > 0)
                        {
                            if (dsUser.Tables[0].Rows[0]["iSendFirstPickGoodsEmail"].ToString() == "1")
                            {
                                DataSet dsAllUnReleaseSubWayBill = new DataSet();

                                dsAllUnReleaseSubWayBill = SqlServerHelper.Query(string.Format(" select swbSerialNum,swbDescription_CHN,swbDescription_ENG,swbNumber,swbWeight,convert(nvarchar(10),DetainDate,120) DetainDate from  V_WayBill_SubWayBill where wbID={0} and swbNeedCheck=3 ", strWBID));

                                string str_wbSerialNum_ForPrint = wbSerialNum_ForPrint == null ? "" : Server.UrlDecode(wbSerialNum_ForPrint);
                                string str_FlowNum_ForPrint = FlowNum_ForPrint == null ? "" : Server.UrlDecode(FlowNum_ForPrint);
                                string str_wbSerialNumber_ForPrint = wbSerialNumber_ForPrint == null ? "" : Server.UrlDecode(wbSerialNumber_ForPrint);
                                string str_swbTotalNumber_ForPrint = swbTotalNumber_ForPrint == null ? "" : Server.UrlDecode(swbTotalNumber_ForPrint);
                                string str_ReleaseNum_ForSetting = ReleaseNum_ForSetting == null ? "" : Server.UrlDecode(ReleaseNum_ForSetting);
                                string str_UnReleaseNum_ForSetting = UnReleaseNum_ForSetting == null ? "" : Server.UrlDecode(UnReleaseNum_ForSetting);
                                string str_wbActualWeight_ForPrint = wbActualWeight_ForPrint == null ? "" : Server.UrlDecode(wbActualWeight_ForPrint);
                                string str_InStoreDate_ForSetting = InStoreDate_ForSetting == null ? "" : Server.UrlDecode(InStoreDate_ForSetting);
                                string str_PickGoodsDate_ForSetting = PickGoodsDate_ForSetting == null ? "" : Server.UrlDecode(PickGoodsDate_ForSetting);
                                string str_OperateFee_ForSetting = OperateFee_ForSetting == null ? "" : Server.UrlDecode(OperateFee_ForSetting);
                                string str_PickGoodsFee_ForSetting = PickGoodsFee_ForSetting == null ? "" : Server.UrlDecode(PickGoodsFee_ForSetting);
                                string str_KeepGoodsFee_ForSetting = KeepGoodsFee_ForSetting == null ? "" : Server.UrlDecode(KeepGoodsFee_ForSetting);
                                string str_ShiftGoodsFee_ForSetting = ShiftGoodsFee_ForSetting == null ? "" : Server.UrlDecode(ShiftGoodsFee_ForSetting);
                                string str_CollectionFee_ForSetting = CollectionFee_ForSetting == null ? "" : Server.UrlDecode(CollectionFee_ForSetting);
                                string str_TotalFee_ForSetting = TotalFee_ForSetting == null ? "" : Server.UrlDecode(TotalFee_ForSetting);
                                string str_ddlPayMode_ForSetting = ddlPayMode_ForSetting == null ? "" : Server.UrlDecode(ddlPayMode_ForSetting);

                                LocalReport localReport = new LocalReport();
                                localReport.ReportPath = Server.MapPath(STR_REPORT_URL);
                                ReportDataSource dtAllUnReleaseSubWayBill = new ReportDataSource("FirstPickGoodsSheetSetting_DS", dsAllUnReleaseSubWayBill.Tables[0]);

                                ReportParameter var_wbSerialNum_ForPrint = new ReportParameter("wbSerialNum_ForPrint", str_wbSerialNum_ForPrint.ToString());
                                ReportParameter var_FlowNum_ForPrint = new ReportParameter("FlowNum_ForPrint", str_FlowNum_ForPrint.ToString());
                                ReportParameter var_wbSerialNumber_ForPrint = new ReportParameter("wbSerialNumber_ForPrint", str_wbSerialNumber_ForPrint.ToString());
                                ReportParameter var_swbTotalNumber_ForPrint = new ReportParameter("swbTotalNumber_ForPrint", str_swbTotalNumber_ForPrint.ToString());
                                ReportParameter var_ReleaseNum_ForSetting = new ReportParameter("ReleaseNum_ForSetting", str_ReleaseNum_ForSetting.ToString());
                                ReportParameter var_UnReleaseNum_ForSetting = new ReportParameter("UnReleaseNum_ForSetting", str_UnReleaseNum_ForSetting.ToString());
                                ReportParameter var_wbActualWeight_ForPrint = new ReportParameter("wbActualWeight_ForPrint", str_wbActualWeight_ForPrint.ToString());
                                ReportParameter var_InStoreDate_ForSetting = new ReportParameter("InStoreDate_ForSetting", str_InStoreDate_ForSetting.ToString());
                                ReportParameter var_PickGoodsDate_ForSetting = new ReportParameter("PickGoodsDate_ForSetting", str_PickGoodsDate_ForSetting.ToString());
                                ReportParameter var_OperateFee_ForSetting = new ReportParameter("OperateFee_ForSetting", str_OperateFee_ForSetting.ToString());
                                ReportParameter var_PickGoodsFee_ForSetting = new ReportParameter("PickGoodsFee_ForSetting", str_PickGoodsFee_ForSetting.ToString());
                                ReportParameter var_KeepGoodsFee_ForSetting = new ReportParameter("KeepGoodsFee_ForSetting", str_KeepGoodsFee_ForSetting.ToString());
                                ReportParameter var_ShiftGoodsFee_ForSetting = new ReportParameter("ShiftGoodsFee_ForSetting", str_ShiftGoodsFee_ForSetting.ToString());
                                ReportParameter var_CollectionFee_ForSetting = new ReportParameter("CollectionFee_ForSetting", str_CollectionFee_ForSetting.ToString());
                                ReportParameter var_TotalFee_ForSetting = new ReportParameter("TotalFee_ForSetting", str_TotalFee_ForSetting.ToString());
                                ReportParameter var_ddlPayMode_ForSetting = new ReportParameter("ddlPayMode_ForSetting", str_ddlPayMode_ForSetting.ToString());

                                localReport.SetParameters(new ReportParameter[] { var_wbSerialNum_ForPrint });
                                localReport.SetParameters(new ReportParameter[] { var_FlowNum_ForPrint });
                                localReport.SetParameters(new ReportParameter[] { var_wbSerialNumber_ForPrint });
                                localReport.SetParameters(new ReportParameter[] { var_swbTotalNumber_ForPrint });
                                localReport.SetParameters(new ReportParameter[] { var_ReleaseNum_ForSetting });
                                localReport.SetParameters(new ReportParameter[] { var_UnReleaseNum_ForSetting });
                                localReport.SetParameters(new ReportParameter[] { var_wbActualWeight_ForPrint });
                                localReport.SetParameters(new ReportParameter[] { var_InStoreDate_ForSetting });
                                localReport.SetParameters(new ReportParameter[] { var_PickGoodsDate_ForSetting });
                                localReport.SetParameters(new ReportParameter[] { var_OperateFee_ForSetting });
                                localReport.SetParameters(new ReportParameter[] { var_PickGoodsFee_ForSetting });
                                localReport.SetParameters(new ReportParameter[] { var_KeepGoodsFee_ForSetting });
                                localReport.SetParameters(new ReportParameter[] { var_ShiftGoodsFee_ForSetting });
                                localReport.SetParameters(new ReportParameter[] { var_CollectionFee_ForSetting });
                                localReport.SetParameters(new ReportParameter[] { var_TotalFee_ForSetting });
                                localReport.SetParameters(new ReportParameter[] { var_ddlPayMode_ForSetting });

                                localReport.DataSources.Add(dtAllUnReleaseSubWayBill);
                                string reportType = "PDF";
                                string mimeType;
                                string encoding = "UTF-8";
                                string fileNameExtension;

                                string deviceInfo = "<DeviceInfo>" +
                                    " <OutputFormat>PDF</OutputFormat>" +
                                    " <PageWidth>12in</PageWidth>" +
                                    " <PageHeigth>6in</PageHeigth>" +
                                    " <MarginTop>0.2in</MarginTop>" +
                                    " <MarginLeft>1in</MarginLeft>" +
                                    " <MarginRight>1in</MarginRight>" +
                                    " <MarginBottom>0.2in</MarginBottom>" +
                                    " </DeviceInfo>";

                                Warning[] warnings;
                                string[] streams;
                                byte[] renderedBytes;

                                renderedBytes = localReport.Render(reportType, deviceInfo, out mimeType, out encoding, out fileNameExtension, out streams, out warnings);

                                switch (iPrintType)
                                {
                                    case 1:
                                        try
                                        {
                                            string FileName = Server.MapPath("~/Temp/PDF/") + "快件提货单_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".pdf";
                                            using (FileStream wf = new FileStream(FileName, FileMode.Create))
                                            {
                                                wf.Write(renderedBytes, 0, renderedBytes.Length);
                                                wf.Flush();
                                                wf.Close();

                                                if (NetMail_SendMail(FileName, wbCompany, "0"))
                                                {
                                                    strResult = "{\"result\":\"ok\",\"message\":\"发送邮件成功\"}";
                                                }
                                                else
                                                {
                                                    strResult = "{\"result\":\"error\",\"message\":\"发送邮件失败\"}";
                                                }
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            strResult = "{\"result\":\"error\",\"message\":\"发送邮件失败，原因:" + ex.Message + "\"}";
                                        }

                                        break;
                                    default:
                                        break;
                                }
                            }
                            else
                            {
                                strResult = "{\"result\":\"ok\",\"message\":\"无需邮件推送\"}";
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                strResult = "{\"result\":\"error\",\"message\":\"发送邮件失败，原因:" + ex.Message + "\"}";
            }
            return strResult;
        }

        ///// <summary>
        ///// 导出扣货单
        ///// </summary>
        ///// <param name="strCurrentReleaseSubWayBill">本次放行的分运单信息(单号1,单号2……)</param>
        ///// <param name="strWBID"></param>
        ///// <param name="iPrintType">0:导出预览   1:确认导出</param>
        ///// <returns></returns>
        //[HttpGet]
        //public ActionResult Excel(string strWBID, int iPrintType, string wbSerialNum_ForPrint, string FlowNum_ForPrint, string wbSerialNumber_ForPrint, string swbTotalNumber_ForPrint, string ReleaseNum_ForSetting, string UnReleaseNum_ForSetting, string wbActualWeight_ForPrint, string InStoreDate_ForSetting, string PickGoodsDate_ForSetting, string OperateFee_ForSetting, string PickGoodsFee_ForSetting, string KeepGoodsFee_ForSetting, string ShiftGoodsFee_ForSetting, string CollectionFee_ForSetting, string TotalFee_ForSetting, string ddlPayMode_ForSetting, string browserType)
        //{
        //    DataSet dsAllUnReleaseSubWayBill = new DataSet();

        //    dsAllUnReleaseSubWayBill = SqlServerHelper.Query(string.Format(" select swbSerialNum,swbDescription_CHN,swbDescription_ENG,swbNumber,swbWeight,convert(nvarchar(10),DetainDate,120) DetainDate from  V_WayBill_SubWayBill where wbID={0} and swbNeedCheck=3 ", strWBID));

        //    string str_wbSerialNum_ForPrint = wbSerialNum_ForPrint == null ? "" : Server.UrlDecode(wbSerialNum_ForPrint);
        //    string str_FlowNum_ForPrint = FlowNum_ForPrint == null ? "" : Server.UrlDecode(FlowNum_ForPrint);
        //    string str_wbSerialNumber_ForPrint = wbSerialNumber_ForPrint == null ? "" : Server.UrlDecode(wbSerialNumber_ForPrint);
        //    string str_swbTotalNumber_ForPrint = swbTotalNumber_ForPrint == null ? "" : Server.UrlDecode(swbTotalNumber_ForPrint);
        //    string str_ReleaseNum_ForSetting = ReleaseNum_ForSetting == null ? "" : Server.UrlDecode(ReleaseNum_ForSetting);
        //    string str_UnReleaseNum_ForSetting = UnReleaseNum_ForSetting == null ? "" : Server.UrlDecode(UnReleaseNum_ForSetting);
        //    string str_wbActualWeight_ForPrint = wbActualWeight_ForPrint == null ? "" : Server.UrlDecode(wbActualWeight_ForPrint);
        //    string str_InStoreDate_ForSetting = InStoreDate_ForSetting == null ? "" : Server.UrlDecode(InStoreDate_ForSetting);
        //    string str_PickGoodsDate_ForSetting = PickGoodsDate_ForSetting == null ? "" : Server.UrlDecode(PickGoodsDate_ForSetting);
        //    string str_OperateFee_ForSetting = OperateFee_ForSetting == null ? "" : Server.UrlDecode(OperateFee_ForSetting);
        //    string str_PickGoodsFee_ForSetting = PickGoodsFee_ForSetting == null ? "" : Server.UrlDecode(PickGoodsFee_ForSetting);
        //    string str_KeepGoodsFee_ForSetting = KeepGoodsFee_ForSetting == null ? "" : Server.UrlDecode(KeepGoodsFee_ForSetting);
        //    string str_ShiftGoodsFee_ForSetting = ShiftGoodsFee_ForSetting == null ? "" : Server.UrlDecode(ShiftGoodsFee_ForSetting);
        //    string str_CollectionFee_ForSetting = CollectionFee_ForSetting == null ? "" : Server.UrlDecode(CollectionFee_ForSetting);
        //    string str_TotalFee_ForSetting = TotalFee_ForSetting == null ? "" : Server.UrlDecode(TotalFee_ForSetting);
        //    string str_ddlPayMode_ForSetting = ddlPayMode_ForSetting == null ? "" : Server.UrlDecode(ddlPayMode_ForSetting);

        //    LocalReport localReport = new LocalReport();
        //    localReport.ReportPath = Server.MapPath(STR_REPORT_URL);
        //    ReportDataSource dtAllUnReleaseSubWayBill = new ReportDataSource("FirstPickGoodsSheetSetting_DS", dsAllUnReleaseSubWayBill.Tables[0]);

        //    ReportParameter var_wbSerialNum_ForPrint = new ReportParameter("wbSerialNum_ForPrint", str_wbSerialNum_ForPrint.ToString());
        //    ReportParameter var_FlowNum_ForPrint = new ReportParameter("FlowNum_ForPrint", str_FlowNum_ForPrint.ToString());
        //    ReportParameter var_wbSerialNumber_ForPrint = new ReportParameter("wbSerialNumber_ForPrint", str_wbSerialNumber_ForPrint.ToString());
        //    ReportParameter var_swbTotalNumber_ForPrint = new ReportParameter("swbTotalNumber_ForPrint", str_swbTotalNumber_ForPrint.ToString());
        //    ReportParameter var_ReleaseNum_ForSetting = new ReportParameter("ReleaseNum_ForSetting", str_ReleaseNum_ForSetting.ToString());
        //    ReportParameter var_UnReleaseNum_ForSetting = new ReportParameter("UnReleaseNum_ForSetting", str_UnReleaseNum_ForSetting.ToString());
        //    ReportParameter var_wbActualWeight_ForPrint = new ReportParameter("wbActualWeight_ForPrint", str_wbActualWeight_ForPrint.ToString());
        //    ReportParameter var_InStoreDate_ForSetting = new ReportParameter("InStoreDate_ForSetting", str_InStoreDate_ForSetting.ToString());
        //    ReportParameter var_PickGoodsDate_ForSetting = new ReportParameter("PickGoodsDate_ForSetting", str_PickGoodsDate_ForSetting.ToString());
        //    ReportParameter var_OperateFee_ForSetting = new ReportParameter("OperateFee_ForSetting", str_OperateFee_ForSetting.ToString());
        //    ReportParameter var_PickGoodsFee_ForSetting = new ReportParameter("PickGoodsFee_ForSetting", str_PickGoodsFee_ForSetting.ToString());
        //    ReportParameter var_KeepGoodsFee_ForSetting = new ReportParameter("KeepGoodsFee_ForSetting", str_KeepGoodsFee_ForSetting.ToString());
        //    ReportParameter var_ShiftGoodsFee_ForSetting = new ReportParameter("ShiftGoodsFee_ForSetting", str_ShiftGoodsFee_ForSetting.ToString());
        //    ReportParameter var_CollectionFee_ForSetting = new ReportParameter("CollectionFee_ForSetting", str_CollectionFee_ForSetting.ToString());
        //    ReportParameter var_TotalFee_ForSetting = new ReportParameter("TotalFee_ForSetting", str_TotalFee_ForSetting.ToString());
        //    ReportParameter var_ddlPayMode_ForSetting = new ReportParameter("ddlPayMode_ForSetting", str_ddlPayMode_ForSetting.ToString());

        //    localReport.SetParameters(new ReportParameter[] { var_wbSerialNum_ForPrint });
        //    localReport.SetParameters(new ReportParameter[] { var_FlowNum_ForPrint });
        //    localReport.SetParameters(new ReportParameter[] { var_wbSerialNumber_ForPrint });
        //    localReport.SetParameters(new ReportParameter[] { var_swbTotalNumber_ForPrint });
        //    localReport.SetParameters(new ReportParameter[] { var_ReleaseNum_ForSetting });
        //    localReport.SetParameters(new ReportParameter[] { var_UnReleaseNum_ForSetting });
        //    localReport.SetParameters(new ReportParameter[] { var_wbActualWeight_ForPrint });
        //    localReport.SetParameters(new ReportParameter[] { var_InStoreDate_ForSetting });
        //    localReport.SetParameters(new ReportParameter[] { var_PickGoodsDate_ForSetting });
        //    localReport.SetParameters(new ReportParameter[] { var_OperateFee_ForSetting });
        //    localReport.SetParameters(new ReportParameter[] { var_PickGoodsFee_ForSetting });
        //    localReport.SetParameters(new ReportParameter[] { var_KeepGoodsFee_ForSetting });
        //    localReport.SetParameters(new ReportParameter[] { var_ShiftGoodsFee_ForSetting });
        //    localReport.SetParameters(new ReportParameter[] { var_CollectionFee_ForSetting });
        //    localReport.SetParameters(new ReportParameter[] { var_TotalFee_ForSetting });
        //    localReport.SetParameters(new ReportParameter[] { var_ddlPayMode_ForSetting });

        //    localReport.DataSources.Add(dtAllUnReleaseSubWayBill);
        //    Warning[] warnings;
        //    string[] streamids;
        //    string mimeType;
        //    string encoding;
        //    string extension;

        //    byte[] bytes = localReport.Render(
        //       "Excel", null, out mimeType, out encoding, out extension,
        //       out streamids, out warnings);
        //    string strFileName = Server.MapPath(STR_TEMPLATE_EXCEL);
        //    FileStream fs = new FileStream(strFileName, FileMode.Create);
        //    fs.Write(bytes, 0, bytes.Length);
        //    fs.Close();

        //    string strOutputFileName = "快件提货单_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";

        //    switch (browserType.ToLower())
        //    {
        //        case "safari":
        //            break;
        //        case "mozilla":
        //            break;
        //        default:
        //            strOutputFileName = HttpUtility.UrlEncode(strOutputFileName);
        //            break;
        //    }

        //    return File(strFileName, "application/vnd.ms-excel", strOutputFileName);
        //}

        /// <summary>
        /// 导出扣货单
        /// </summary>
        /// <param name="strCurrentReleaseSubWayBill">本次放行的分运单信息(单号1,单号2……)</param>
        /// <param name="strWBID"></param>
        /// <param name="iPrintType">0:导出预览   1:确认导出</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Excel(string strWBID, int iPrintType, string wbSerialNum_ForPrint, string FlowNum_ForPrint, string wbSerialNumber_ForPrint, string swbTotalNumber_ForPrint, string ReleaseNum_ForSetting, string UnReleaseNum_ForSetting, string wbActualWeight_ForPrint, string InStoreDate_ForSetting, string PickGoodsDate_ForSetting, string OperateFee_ForSetting, string PickGoodsFee_ForSetting, string KeepGoodsFee_ForSetting, string ShiftGoodsFee_ForSetting, string CollectionFee_ForSetting, string TotalFee_ForSetting, string ddlPayMode_ForSetting, string browserType)
        {
            DataSet dsAllUnReleaseSubWayBill = new DataSet();

            dsAllUnReleaseSubWayBill = SqlServerHelper.Query(string.Format(" select swbSerialNum,swbDescription_CHN,swbDescription_ENG,swbNumber,swbWeight,convert(nvarchar(10),DetainDate,120) DetainDate,swbID from  V_WayBill_SubWayBill where wbID={0} and swbNeedCheck=3 ", strWBID));

            string str_wbSerialNum_ForPrint = wbSerialNum_ForPrint == null ? "" : Server.UrlDecode(wbSerialNum_ForPrint);
            string str_FlowNum_ForPrint = FlowNum_ForPrint == null ? "" : Server.UrlDecode(FlowNum_ForPrint);
            string str_wbSerialNumber_ForPrint = wbSerialNumber_ForPrint == null ? "" : Server.UrlDecode(wbSerialNumber_ForPrint);
            string str_swbTotalNumber_ForPrint = swbTotalNumber_ForPrint == null ? "" : Server.UrlDecode(swbTotalNumber_ForPrint);
            string str_ReleaseNum_ForSetting = ReleaseNum_ForSetting == null ? "" : Server.UrlDecode(ReleaseNum_ForSetting);
            string str_UnReleaseNum_ForSetting = UnReleaseNum_ForSetting == null ? "" : Server.UrlDecode(UnReleaseNum_ForSetting);
            string str_wbActualWeight_ForPrint = wbActualWeight_ForPrint == null ? "" : Server.UrlDecode(wbActualWeight_ForPrint);
            string str_InStoreDate_ForSetting = InStoreDate_ForSetting == null ? "" : Server.UrlDecode(InStoreDate_ForSetting);
            string str_PickGoodsDate_ForSetting = PickGoodsDate_ForSetting == null ? "" : Server.UrlDecode(PickGoodsDate_ForSetting);
            string str_OperateFee_ForSetting = OperateFee_ForSetting == null ? "" : Server.UrlDecode(OperateFee_ForSetting);
            string str_PickGoodsFee_ForSetting = PickGoodsFee_ForSetting == null ? "" : Server.UrlDecode(PickGoodsFee_ForSetting);
            string str_KeepGoodsFee_ForSetting = KeepGoodsFee_ForSetting == null ? "" : Server.UrlDecode(KeepGoodsFee_ForSetting);
            string str_ShiftGoodsFee_ForSetting = ShiftGoodsFee_ForSetting == null ? "" : Server.UrlDecode(ShiftGoodsFee_ForSetting);
            string str_CollectionFee_ForSetting = CollectionFee_ForSetting == null ? "" : Server.UrlDecode(CollectionFee_ForSetting);
            string str_TotalFee_ForSetting = TotalFee_ForSetting == null ? "" : Server.UrlDecode(TotalFee_ForSetting);
            string str_ddlPayMode_ForSetting = ddlPayMode_ForSetting == null ? "" : Server.UrlDecode(ddlPayMode_ForSetting);

            LocalReport localReport = new LocalReport();
            localReport.ReportPath = Server.MapPath(STR_REPORT_URL);
            ReportDataSource dtAllUnReleaseSubWayBill = new ReportDataSource("FirstPickGoodsSheetSetting_DS", dsAllUnReleaseSubWayBill.Tables[0]);

            ReportParameter var_wbSerialNum_ForPrint = new ReportParameter("wbSerialNum_ForPrint", str_wbSerialNum_ForPrint.ToString());
            ReportParameter var_FlowNum_ForPrint = new ReportParameter("FlowNum_ForPrint", str_FlowNum_ForPrint.ToString());
            ReportParameter var_wbSerialNumber_ForPrint = new ReportParameter("wbSerialNumber_ForPrint", str_wbSerialNumber_ForPrint.ToString());
            ReportParameter var_swbTotalNumber_ForPrint = new ReportParameter("swbTotalNumber_ForPrint", str_swbTotalNumber_ForPrint.ToString());
            ReportParameter var_ReleaseNum_ForSetting = new ReportParameter("ReleaseNum_ForSetting", str_ReleaseNum_ForSetting.ToString());
            ReportParameter var_UnReleaseNum_ForSetting = new ReportParameter("UnReleaseNum_ForSetting", str_UnReleaseNum_ForSetting.ToString());
            ReportParameter var_wbActualWeight_ForPrint = new ReportParameter("wbActualWeight_ForPrint", str_wbActualWeight_ForPrint.ToString());
            ReportParameter var_InStoreDate_ForSetting = new ReportParameter("InStoreDate_ForSetting", str_InStoreDate_ForSetting.ToString());
            ReportParameter var_PickGoodsDate_ForSetting = new ReportParameter("PickGoodsDate_ForSetting", str_PickGoodsDate_ForSetting.ToString());
            ReportParameter var_OperateFee_ForSetting = new ReportParameter("OperateFee_ForSetting", str_OperateFee_ForSetting.ToString());
            ReportParameter var_PickGoodsFee_ForSetting = new ReportParameter("PickGoodsFee_ForSetting", str_PickGoodsFee_ForSetting.ToString());
            ReportParameter var_KeepGoodsFee_ForSetting = new ReportParameter("KeepGoodsFee_ForSetting", str_KeepGoodsFee_ForSetting.ToString());
            ReportParameter var_ShiftGoodsFee_ForSetting = new ReportParameter("ShiftGoodsFee_ForSetting", str_ShiftGoodsFee_ForSetting.ToString());
            ReportParameter var_CollectionFee_ForSetting = new ReportParameter("CollectionFee_ForSetting", str_CollectionFee_ForSetting.ToString());
            ReportParameter var_TotalFee_ForSetting = new ReportParameter("TotalFee_ForSetting", str_TotalFee_ForSetting.ToString());
            ReportParameter var_ddlPayMode_ForSetting = new ReportParameter("ddlPayMode_ForSetting", str_ddlPayMode_ForSetting.ToString());

            localReport.SetParameters(new ReportParameter[] { var_wbSerialNum_ForPrint });
            localReport.SetParameters(new ReportParameter[] { var_FlowNum_ForPrint });
            localReport.SetParameters(new ReportParameter[] { var_wbSerialNumber_ForPrint });
            localReport.SetParameters(new ReportParameter[] { var_swbTotalNumber_ForPrint });
            localReport.SetParameters(new ReportParameter[] { var_ReleaseNum_ForSetting });
            localReport.SetParameters(new ReportParameter[] { var_UnReleaseNum_ForSetting });
            localReport.SetParameters(new ReportParameter[] { var_wbActualWeight_ForPrint });
            localReport.SetParameters(new ReportParameter[] { var_InStoreDate_ForSetting });
            localReport.SetParameters(new ReportParameter[] { var_PickGoodsDate_ForSetting });
            localReport.SetParameters(new ReportParameter[] { var_OperateFee_ForSetting });
            localReport.SetParameters(new ReportParameter[] { var_PickGoodsFee_ForSetting });
            localReport.SetParameters(new ReportParameter[] { var_KeepGoodsFee_ForSetting });
            localReport.SetParameters(new ReportParameter[] { var_ShiftGoodsFee_ForSetting });
            localReport.SetParameters(new ReportParameter[] { var_CollectionFee_ForSetting });
            localReport.SetParameters(new ReportParameter[] { var_TotalFee_ForSetting });
            localReport.SetParameters(new ReportParameter[] { var_ddlPayMode_ForSetting });

            localReport.SubreportProcessing += new SubreportProcessingEventHandler(LocalReport_SubreportProcessing);

            localReport.DataSources.Add(dtAllUnReleaseSubWayBill);

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

            string strOutputFileName = "快件提货单_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";

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
        public string SendEmail_Excel(string strWBID, int iPrintType, string wbSerialNum_ForPrint, string FlowNum_ForPrint, string wbSerialNumber_ForPrint, string swbTotalNumber_ForPrint, string ReleaseNum_ForSetting, string UnReleaseNum_ForSetting, string wbActualWeight_ForPrint, string InStoreDate_ForSetting, string PickGoodsDate_ForSetting, string OperateFee_ForSetting, string PickGoodsFee_ForSetting, string KeepGoodsFee_ForSetting, string ShiftGoodsFee_ForSetting, string CollectionFee_ForSetting, string TotalFee_ForSetting, string ddlPayMode_ForSetting, string browserType)
        {
            string strResult = "{\"result\":\"error\",\"message\":\"发送邮件失败，原因未知\"}";
            string wbCompany = "";
            try
            {
                DataSet dsWayBill = new T_WayBill().getWayBillInfo(strWBID);
                if (dsWayBill != null)
                {
                    if (dsWayBill.Tables[0] != null && dsWayBill.Tables[0].Rows.Count > 0)
                    {
                        wbCompany = dsWayBill.Tables[0].Rows[0]["wbCompany"].ToString();
                    }
                }
                if (!string.IsNullOrEmpty(wbCompany))
                {
                    DataSet dsUser = new T_User().GetUserByCompany(wbCompany);
                    if (dsUser != null)
                    {
                        if (dsUser.Tables[0] != null && dsUser.Tables[0].Rows.Count > 0)
                        {
                            if (dsUser.Tables[0].Rows[0]["iSendFirstPickGoodsEmail"].ToString() == "1")
                            {
                                DataSet dsAllUnReleaseSubWayBill = new DataSet();

                                dsAllUnReleaseSubWayBill = SqlServerHelper.Query(string.Format(" select swbSerialNum,swbDescription_CHN,swbDescription_ENG,swbNumber,swbWeight,convert(nvarchar(10),DetainDate,120) DetainDate from  V_WayBill_SubWayBill where wbID={0} and swbNeedCheck=3 ", strWBID));

                                string str_wbSerialNum_ForPrint = wbSerialNum_ForPrint == null ? "" : Server.UrlDecode(wbSerialNum_ForPrint);
                                string str_FlowNum_ForPrint = FlowNum_ForPrint == null ? "" : Server.UrlDecode(FlowNum_ForPrint);
                                string str_wbSerialNumber_ForPrint = wbSerialNumber_ForPrint == null ? "" : Server.UrlDecode(wbSerialNumber_ForPrint);
                                string str_swbTotalNumber_ForPrint = swbTotalNumber_ForPrint == null ? "" : Server.UrlDecode(swbTotalNumber_ForPrint);
                                string str_ReleaseNum_ForSetting = ReleaseNum_ForSetting == null ? "" : Server.UrlDecode(ReleaseNum_ForSetting);
                                string str_UnReleaseNum_ForSetting = UnReleaseNum_ForSetting == null ? "" : Server.UrlDecode(UnReleaseNum_ForSetting);
                                string str_wbActualWeight_ForPrint = wbActualWeight_ForPrint == null ? "" : Server.UrlDecode(wbActualWeight_ForPrint);
                                string str_InStoreDate_ForSetting = InStoreDate_ForSetting == null ? "" : Server.UrlDecode(InStoreDate_ForSetting);
                                string str_PickGoodsDate_ForSetting = PickGoodsDate_ForSetting == null ? "" : Server.UrlDecode(PickGoodsDate_ForSetting);
                                string str_OperateFee_ForSetting = OperateFee_ForSetting == null ? "" : Server.UrlDecode(OperateFee_ForSetting);
                                string str_PickGoodsFee_ForSetting = PickGoodsFee_ForSetting == null ? "" : Server.UrlDecode(PickGoodsFee_ForSetting);
                                string str_KeepGoodsFee_ForSetting = KeepGoodsFee_ForSetting == null ? "" : Server.UrlDecode(KeepGoodsFee_ForSetting);
                                string str_ShiftGoodsFee_ForSetting = ShiftGoodsFee_ForSetting == null ? "" : Server.UrlDecode(ShiftGoodsFee_ForSetting);
                                string str_CollectionFee_ForSetting = CollectionFee_ForSetting == null ? "" : Server.UrlDecode(CollectionFee_ForSetting);
                                string str_TotalFee_ForSetting = TotalFee_ForSetting == null ? "" : Server.UrlDecode(TotalFee_ForSetting);
                                string str_ddlPayMode_ForSetting = ddlPayMode_ForSetting == null ? "" : Server.UrlDecode(ddlPayMode_ForSetting);

                                LocalReport localReport = new LocalReport();
                                localReport.ReportPath = Server.MapPath(STR_REPORT_URL);
                                ReportDataSource dtAllUnReleaseSubWayBill = new ReportDataSource("FirstPickGoodsSheetSetting_DS", dsAllUnReleaseSubWayBill.Tables[0]);

                                ReportParameter var_wbSerialNum_ForPrint = new ReportParameter("wbSerialNum_ForPrint", str_wbSerialNum_ForPrint.ToString());
                                ReportParameter var_FlowNum_ForPrint = new ReportParameter("FlowNum_ForPrint", str_FlowNum_ForPrint.ToString());
                                ReportParameter var_wbSerialNumber_ForPrint = new ReportParameter("wbSerialNumber_ForPrint", str_wbSerialNumber_ForPrint.ToString());
                                ReportParameter var_swbTotalNumber_ForPrint = new ReportParameter("swbTotalNumber_ForPrint", str_swbTotalNumber_ForPrint.ToString());
                                ReportParameter var_ReleaseNum_ForSetting = new ReportParameter("ReleaseNum_ForSetting", str_ReleaseNum_ForSetting.ToString());
                                ReportParameter var_UnReleaseNum_ForSetting = new ReportParameter("UnReleaseNum_ForSetting", str_UnReleaseNum_ForSetting.ToString());
                                ReportParameter var_wbActualWeight_ForPrint = new ReportParameter("wbActualWeight_ForPrint", str_wbActualWeight_ForPrint.ToString());
                                ReportParameter var_InStoreDate_ForSetting = new ReportParameter("InStoreDate_ForSetting", str_InStoreDate_ForSetting.ToString());
                                ReportParameter var_PickGoodsDate_ForSetting = new ReportParameter("PickGoodsDate_ForSetting", str_PickGoodsDate_ForSetting.ToString());
                                ReportParameter var_OperateFee_ForSetting = new ReportParameter("OperateFee_ForSetting", str_OperateFee_ForSetting.ToString());
                                ReportParameter var_PickGoodsFee_ForSetting = new ReportParameter("PickGoodsFee_ForSetting", str_PickGoodsFee_ForSetting.ToString());
                                ReportParameter var_KeepGoodsFee_ForSetting = new ReportParameter("KeepGoodsFee_ForSetting", str_KeepGoodsFee_ForSetting.ToString());
                                ReportParameter var_ShiftGoodsFee_ForSetting = new ReportParameter("ShiftGoodsFee_ForSetting", str_ShiftGoodsFee_ForSetting.ToString());
                                ReportParameter var_CollectionFee_ForSetting = new ReportParameter("CollectionFee_ForSetting", str_CollectionFee_ForSetting.ToString());
                                ReportParameter var_TotalFee_ForSetting = new ReportParameter("TotalFee_ForSetting", str_TotalFee_ForSetting.ToString());
                                ReportParameter var_ddlPayMode_ForSetting = new ReportParameter("ddlPayMode_ForSetting", str_ddlPayMode_ForSetting.ToString());

                                localReport.SetParameters(new ReportParameter[] { var_wbSerialNum_ForPrint });
                                localReport.SetParameters(new ReportParameter[] { var_FlowNum_ForPrint });
                                localReport.SetParameters(new ReportParameter[] { var_wbSerialNumber_ForPrint });
                                localReport.SetParameters(new ReportParameter[] { var_swbTotalNumber_ForPrint });
                                localReport.SetParameters(new ReportParameter[] { var_ReleaseNum_ForSetting });
                                localReport.SetParameters(new ReportParameter[] { var_UnReleaseNum_ForSetting });
                                localReport.SetParameters(new ReportParameter[] { var_wbActualWeight_ForPrint });
                                localReport.SetParameters(new ReportParameter[] { var_InStoreDate_ForSetting });
                                localReport.SetParameters(new ReportParameter[] { var_PickGoodsDate_ForSetting });
                                localReport.SetParameters(new ReportParameter[] { var_OperateFee_ForSetting });
                                localReport.SetParameters(new ReportParameter[] { var_PickGoodsFee_ForSetting });
                                localReport.SetParameters(new ReportParameter[] { var_KeepGoodsFee_ForSetting });
                                localReport.SetParameters(new ReportParameter[] { var_ShiftGoodsFee_ForSetting });
                                localReport.SetParameters(new ReportParameter[] { var_CollectionFee_ForSetting });
                                localReport.SetParameters(new ReportParameter[] { var_TotalFee_ForSetting });
                                localReport.SetParameters(new ReportParameter[] { var_ddlPayMode_ForSetting });

                                localReport.DataSources.Add(dtAllUnReleaseSubWayBill);
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

                                string strOutputFileName = "快件提货单_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";

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

                                switch (iPrintType)
                                {
                                    case 1:
                                        try
                                        {
                                            string FileName = Server.MapPath("~/Temp/PDF/") + strOutputFileName;
                                            using (FileStream wf = new FileStream(FileName, FileMode.Create))
                                            {
                                                wf.Write(bytes, 0, bytes.Length);
                                                wf.Flush();
                                                wf.Close();
                                                if (NetMail_SendMail(FileName, wbCompany, "1"))
                                                {
                                                    strResult = "{\"result\":\"ok\",\"message\":\"发送邮件成功\"}";
                                                }
                                                else
                                                {
                                                    strResult = "{\"result\":\"error\",\"message\":\"发送邮件失败\"}";
                                                }
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            strResult = "{\"result\":\"error\",\"message\":\"发送邮件失败，原因:" + ex.Message + "\"}";
                                        }

                                        break;
                                    default:
                                        break;
                                }
                            }
                            else
                            {
                                strResult = "{\"result\":\"ok\",\"message\":\"无需邮件推送\"}";
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                strResult = "{\"result\":\"error\",\"message\":\"发送邮件失败，原因:" + ex.Message + "\"}";
            }
            return strResult;
        }

        [HttpPost]
        public string SaveSaleInfo(string FlowNum_ForPrint, string hid_CustomCategory_ForSetting, string wbID_ForPrint, string InStoreDate_ForSetting, string PickGoodsDate_ForSetting, string wbActualWeight_ForPrint, string OperateFee_ForSetting, string PickGoodsFee_ForSetting, string KeepGoodsFee_ForSetting, string ShiftGoodsFee_ForSetting, string CollectionFee_ForSetting, string ddlPayMode_ForSetting, string ShouldPayUnit_ForSetting, string shouldPay_ForSetting, string TotalFee_ForSetting, string ddlReceiptMode_ForSetting, string Receipt_ForSetting)
        {
            string strRet = "{\"result\":\"error\",\"message\":\"销售报表保存失败,不允许继续打印或导出,原因未知\"}";
            M_WayBillDailyReport m_WayBillDailyReport = null;
            FlowNum_ForPrint = Server.UrlDecode(FlowNum_ForPrint);
            hid_CustomCategory_ForSetting = Server.UrlDecode(hid_CustomCategory_ForSetting);
            wbID_ForPrint = Server.UrlDecode(wbID_ForPrint);
            InStoreDate_ForSetting = Server.UrlDecode(InStoreDate_ForSetting);
            PickGoodsDate_ForSetting = Server.UrlDecode(PickGoodsDate_ForSetting);
            wbActualWeight_ForPrint = Server.UrlDecode(wbActualWeight_ForPrint);
            OperateFee_ForSetting = Server.UrlDecode(OperateFee_ForSetting);
            PickGoodsFee_ForSetting = Server.UrlDecode(PickGoodsFee_ForSetting);
            KeepGoodsFee_ForSetting = Server.UrlDecode(KeepGoodsFee_ForSetting);
            ShiftGoodsFee_ForSetting = Server.UrlDecode(ShiftGoodsFee_ForSetting);
            CollectionFee_ForSetting = Server.UrlDecode(CollectionFee_ForSetting);
            ddlPayMode_ForSetting = Server.UrlDecode(ddlPayMode_ForSetting);
            ShouldPayUnit_ForSetting = Server.UrlDecode(ShouldPayUnit_ForSetting);
            shouldPay_ForSetting = Server.UrlDecode(shouldPay_ForSetting);
            TotalFee_ForSetting = Server.UrlDecode(TotalFee_ForSetting);
            ddlReceiptMode_ForSetting = Server.UrlDecode(ddlReceiptMode_ForSetting);
            Receipt_ForSetting = Server.UrlDecode(Receipt_ForSetting);

            try
            {
                m_WayBillDailyReport = new M_WayBillDailyReport()
                {
                    wbrCode = FlowNum_ForPrint,
                    CustomsCategory = hid_CustomCategory_ForSetting,
                    wbr_wbID = Convert.ToInt32(wbID_ForPrint),
                    InStoreDate = InStoreDate_ForSetting,
                    OutStoreDate = PickGoodsDate_ForSetting,
                    WayBillActualWeight = wbActualWeight_ForPrint,
                    OperateFee = OperateFee_ForSetting,
                    PickGoodsFee = PickGoodsFee_ForSetting,
                    KeepGoodsFee = KeepGoodsFee_ForSetting,
                    ShiftGoodsFee = ShiftGoodsFee_ForSetting,
                    CollectionKeepGoodsFee = CollectionFee_ForSetting,
                    PayMethod = ddlPayMode_ForSetting,
                    ShouldPayUnit = ShouldPayUnit_ForSetting,
                    shouldPay = shouldPay_ForSetting,
                    ActualPay = TotalFee_ForSetting,
                    ReceptMethod = ddlReceiptMode_ForSetting,
                    Receipt = Receipt_ForSetting,
                    RejectGoodsFee = "0.00",
                    SalesMan = Session["Global_Huayu_UserName"] == null ? "" : Session["Global_Huayu_UserName"].ToString(),
                    mMemo = ""

                };

                new T_WayBillDailyReport().addWayBillDailyReport(m_WayBillDailyReport);

                strRet = "{\"result\":\"ok\",\"message\":\"销售报表保存成功,开始打印或导出\"}";
            }
            catch (Exception ex)
            {
                strRet = "{\"result\":\"error\",\"message\":\"销售报表保存失败,不允许继续打印或导出,原因:" + ex.Message + "\"}";
            }

            return strRet;
        }

        private Boolean NetMail_SendMail(string AttachmentFileName, string company, string FileType)
        {
            Boolean bOK = false;
            SmtpClient client = new SmtpClient();
            MailAddress mailTo = null;
            MailMessage mail = null;
            string strSubJect = "";// string.Format(STR_MAILSUBJECT, DateTime.Now.ToString("yyyyMMddHHmmss"));
            string strBody = "";// string.Format(STR_MAILBODY, DateTime.Now.ToString("yyyyMMddHHmmss"));
            string[] arrEmails = null;

            string STR_SENDER_SMTP = "";
            string STR_SENDER_USERMAIL = "";
            string STR_SENDER_USERPWD = "";
            string STR_SENDER_USERNAME = "";

            STR_SENDER_SMTP = new T_EmailManagement().GetEmailContent(EmailType.EmailSenderSMTP);
            STR_SENDER_USERMAIL = new T_EmailManagement().GetEmailContent(EmailType.EmailSenderUserName);
            STR_SENDER_USERPWD = Util.CryptographyTool.Decrypt(new T_EmailManagement().GetEmailContent(EmailType.EmailSenderPwd), "HuayuTAT");
            STR_SENDER_USERNAME = STR_SENDER_USERMAIL;

            client.Host = STR_SENDER_SMTP;
            client.Credentials = new System.Net.NetworkCredential(STR_SENDER_USERMAIL, STR_SENDER_USERPWD);

            DataSet dsUser = new T_User().GetUserByCompany(company);
            if (dsUser != null)
            {
                if (dsUser.Tables[0] != null && dsUser.Tables[0].Rows.Count > 0)
                {
                    try
                    {
                        if (dsUser.Tables[0].Rows[0]["iSendFirstPickGoodsEmail"].ToString() == "1")
                        {
                            arrEmails = dsUser.Tables[0].Rows[0]["LinkMail"].ToString().Replace('，', ',').Split(',');
                            for (int i = 0; i < arrEmails.Length; i++)
                            {
                                if (arrEmails[i].Trim() != "")
                                {
                                    mail = new MailMessage();
                                    mail.From = new MailAddress(STR_SENDER_USERMAIL, STR_SENDER_USERNAME, Encoding.GetEncoding(936));
                                    mailTo = new MailAddress(arrEmails[i].Trim(), company, Encoding.GetEncoding(936));
                                    mail.To.Add(mailTo);
                                    //mail.CC.Add(STR_CARBONCODY);抄送
                                    switch (FileType)
                                    {
                                        case "0":
                                            mail.Attachments.Add(new Attachment(AttachmentFileName, System.Net.Mime.MediaTypeNames.Application.Pdf));
                                            break;
                                        case "1":
                                            mail.Attachments.Add(new Attachment(AttachmentFileName, System.Net.Mime.MediaTypeNames.Application.Octet));
                                            break;
                                        default:
                                            break;
                                    }

                                    strSubJect = new T_EmailManagement().GetEmailContent(EmailType.EmailSubject_FirstPickGood).Replace("[Date]", DateTime.Now.ToString("yyyyMMddHHmmss")).Replace("【Date]", DateTime.Now.ToString("yyyyMMddHHmmss")).Replace("[Date】", DateTime.Now.ToString("yyyyMMddHHmmss")).Replace("【Date】", DateTime.Now.ToString("yyyyMMddHHmmss"));
                                    strBody = new T_EmailManagement().GetEmailContent(EmailType.EmailBody_FirstPickGood);

                                    mail.Subject = strSubJect;
                                    mail.Body = strBody;
                                    mail.SubjectEncoding = Encoding.UTF8;
                                    mail.IsBodyHtml = true;

                                    client.Timeout = Convert.ToInt32(STR_TIMEOUT);
                                    client.Send(mail);
                                }
                            }

                        }

                        bOK = true;
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }


            return bOK;
        }
    }
}
