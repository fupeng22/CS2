﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using SQLDAL;
using System.Data.SqlClient;
using System.Text;
using DBUtility;
using Microsoft.Reporting.WebForms;
using CS.Filter;
using System.IO;
using Util;
using Model;
using System.Configuration;
using System.Net.Mail;

namespace CS.Controllers.Common
{
    [ErrorAttribute]
    public class UnReleaseSheetSettingController : Controller
    {
        //public const string strFileds = "wbCompany,wbStorageDate,wbSerialNum,swbSerialNum,DetainDate,swbDescription_CHN,swbDescription_ENG,swbNumber,swbWeight,swbNeedCheck,wbID,swbID";
        public const string strFileds = "wbSerialNum,wbCompany,wbStorageDate,swbSerialNum,DetainDate,swbNeedCheck,swbDescription_CHN,swbDescription_ENG,swbNumber,swbWeight,swbValue,swbMonetary,swbRecipients,swbCustomsCategory,TaxNo,TaxRate,TaxRateDescription,ActualTaxRate,CategoryNo,Sender,ReceiverIDCard,ReceiverPhone,EmailAddress,PickGoodsAgain,mismatchCargoName,belowFullPrice,above1000,chkNeedCheck,CheckResult,HandleSuggestion,CheckResultDescription,HandleSuggestionDescription,FinalCheckResultDescription,FinalHandleSuggestDescription,CheckResultOperator,IsConfirmCheck,IsConfirmCheckDescription,ConfirmCheckOperator,TaxValue,TaxValueCheck,TaxValueCheckOperator,swbValueTotal,parentID,ID,state,wbID,swbdID,swbID";

        public const string strFileds_Sub = "swbDescription_CHN,swbDescription_ENG,swbNumber,DetainDate,swbWeight,swbValue,swbMonetary,TaxNo,TaxRate,TaxRateDescription,mismatchCargoName,belowFullPrice,above1000,FinalCheckResultDescription,FinalHandleSuggestDescription,CheckResultOperator,swbID,swbdID";

        public const string STR_TEMPLATE_EXCEL = "~/Temp/Template/template.xls";
        public const string STR_REPORT_URL = "~/Content/Reports/Huayu_UnReleaseSeet.rdlc";

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
            DataSet ds_EverUnReleaseWayBill = null;
            ds_EverUnReleaseWayBill = (new T_SubWayBill()).getWayBill_SubWayBill(wbID, 3);
            string strAllUnReleaseSubwayBill = "";
            string wbSerialNum = "";
            string InStoreDate_ForSetting = "";//入库日期
            string IntervalDays_ForSetting = "0";//扣留天数

            if (ds_EverUnReleaseWayBill != null)
            {
                dt = ds_EverUnReleaseWayBill.Tables[0];
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
            DataSet dsWayBillFlow_InStoreDate = new T_WayBillFlow().getTop1SubWayBill(wbID);
            if (dsWayBillFlow_InStoreDate != null)
            {
                DataTable dtWayBillFlow_InStoreDate = dsWayBillFlow_InStoreDate.Tables[0];
                if (dtWayBillFlow_InStoreDate != null && dtWayBillFlow_InStoreDate.Rows.Count > 0)
                {
                    switch (dtWayBillFlow_InStoreDate.Rows[0]["InOutStoreType"].ToString())
                    {
                        case "3":
                            try
                            {
                                InStoreDate_ForSetting = Convert.ToDateTime(dtWayBillFlow_InStoreDate.Rows[0]["InOutStoreDate"].ToString()).ToString("yyyy-MM-dd");
                                IntervalDays_ForSetting = DateTimeHelper.DateDiff(EnumDateCompare.day, Convert.ToDateTime(Convert.ToDateTime(InStoreDate_ForSetting).ToString("yyyy-MM-dd")), Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"))).ToString();
                            }
                            catch (Exception ex)
                            {
                                IntervalDays_ForSetting = "0";
                            }
                            break;
                        default:
                            InStoreDate_ForSetting = "未入库或已出库";
                            break;
                    }
                }
            }
            ViewData["InStoreDate_ForSetting"] = InStoreDate_ForSetting;
            ViewData["IntervalDays_ForSetting"] = IntervalDays_ForSetting;

            //设置默认的扣留货物信息
            ComputeUnReleaseInfo(strAllUnReleaseSubwayBill, wbID);
        }

        public string ComputeUnReleaseInfo(string swbSerialNums, string wbID)
        {
            string resulr = "error";
            string message = "计算失败，原因未知";
            string EverUnReleaseNum_ForSetting = "0";//曾经扣留的总件数
            string EverUnReleaseWeight_ForSetting = "0.00";//曾经扣留的总重量
            string CustomCategory_ForSetting = "";//业务类型
            string hid_CustomCategory_ForSetting = "";//隐藏业务类型
            string EverUnReleaseFee_ForSetting = "0.00";//扣货费
            string strRet = "";

            DataSet ds_EverUnReleaseWayBill = null;
            DataTable dt = null;
            if (swbSerialNums != "")
            {
                string CurrentReleaseSubWayBill = Server.UrlDecode(swbSerialNums).Replace("，", ",");
                string[] arrCurrentReleaseSubWayBill = CurrentReleaseSubWayBill.Split(',');
                StringBuilder sbCurrentReleaseSubWayBill = new StringBuilder("");
                for (int i = 0; i < arrCurrentReleaseSubWayBill.Length; i++)
                {
                    if (arrCurrentReleaseSubWayBill[i].Trim() != "")
                    {
                        sbCurrentReleaseSubWayBill.AppendFormat("'{0}',", arrCurrentReleaseSubWayBill[i].Trim());
                    }
                }

                if (sbCurrentReleaseSubWayBill.ToString().EndsWith(","))
                {
                    sbCurrentReleaseSubWayBill = new StringBuilder(sbCurrentReleaseSubWayBill.ToString().Substring(0, sbCurrentReleaseSubWayBill.ToString().Length - 1));
                }

                ds_EverUnReleaseWayBill = new T_SubWayBill().getWayBill_SubWayBill(wbID, sbCurrentReleaseSubWayBill.ToString());
                if (ds_EverUnReleaseWayBill != null)
                {
                    dt = ds_EverUnReleaseWayBill.Tables[0];
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        //得到曾经扣留的件数，以及扣留的货物总重量:
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            try
                            {
                                EverUnReleaseNum_ForSetting = (Convert.ToInt32(EverUnReleaseNum_ForSetting) + 1).ToString();
                                //EverUnReleaseNum_ForSetting = (Convert.ToInt32(EverUnReleaseNum_ForSetting) + Convert.ToInt32(dt.Rows[i]["swbNumber"].ToString())).ToString();
                                EverUnReleaseWeight_ForSetting = (Convert.ToDouble(EverUnReleaseWeight_ForSetting) + Convert.ToDouble(dt.Rows[i]["swbWeight"].ToString())).ToString();
                            }
                            catch (Exception ex)
                            {

                            }
                        }

                        ViewData["EverUnReleaseNum_ForSetting"] = EverUnReleaseNum_ForSetting;
                        ViewData["EverUnReleaseWeight_ForSetting"] = EverUnReleaseWeight_ForSetting;
                        resulr = "ok";
                    }
                }
            }

            ds_EverUnReleaseWayBill = new T_SubWayBill().GetSubWayBillInfoBywbID(Convert.ToInt32(wbID));
            if (ds_EverUnReleaseWayBill != null)
            {
                dt = ds_EverUnReleaseWayBill.Tables[0];
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

            ViewData["CustomCategory_ForSetting"] = CustomCategory_ForSetting;
            ViewData["hid_CustomCategory_ForSetting"] = hid_CustomCategory_ForSetting;

            //计算费用
            switch (hid_CustomCategory_ForSetting)
            {
                case "2"://样品
                    //扣货费
                    EverUnReleaseFee_ForSetting = ((DateTimeHelper.DateDiff(EnumDateCompare.day, Convert.ToDateTime(ViewData["InStoreDate_ForSetting"].ToString()), Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"))) - 3 <= 0 ? 0 : DateTimeHelper.DateDiff(EnumDateCompare.day, Convert.ToDateTime(ViewData["InStoreDate_ForSetting"].ToString()), Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"))) - 3) * Convert.ToInt32(EverUnReleaseNum_ForSetting) * Convert.ToDouble(new T_FeeRate().GetFeeRateValue("2-1-1"))).ToString("0.00");

                    break;
                case "3"://KJ-3
                    //扣货费
                    EverUnReleaseFee_ForSetting = ((DateTimeHelper.DateDiff(EnumDateCompare.day, Convert.ToDateTime(ViewData["InStoreDate_ForSetting"].ToString()), Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"))) - 3 <= 0 ? 0 : DateTimeHelper.DateDiff(EnumDateCompare.day, Convert.ToDateTime(ViewData["InStoreDate_ForSetting"].ToString()), Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"))) - 3) * Convert.ToInt32(EverUnReleaseNum_ForSetting) * Convert.ToDouble(new T_FeeRate().GetFeeRateValue("2-1-1"))).ToString("0.00");

                    break;
                case "4"://D类
                    //扣货费
                    EverUnReleaseFee_ForSetting = ((DateTimeHelper.DateDiff(EnumDateCompare.day, Convert.ToDateTime(ViewData["InStoreDate_ForSetting"].ToString()), Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"))) - 3 <= 0 ? 0 : DateTimeHelper.DateDiff(EnumDateCompare.day, Convert.ToDateTime(ViewData["InStoreDate_ForSetting"].ToString()), Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"))) - 3) * Convert.ToInt32(EverUnReleaseNum_ForSetting) * Convert.ToDouble(new T_FeeRate().GetFeeRateValue("2-1-1"))).ToString("0.00");

                    break;
                case "5"://个人物品
                    //扣货费
                    EverUnReleaseFee_ForSetting = ((DateTimeHelper.DateDiff(EnumDateCompare.day, Convert.ToDateTime(ViewData["InStoreDate_ForSetting"].ToString()), Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"))) - 7 <= 0 ? 0 : DateTimeHelper.DateDiff(EnumDateCompare.day, Convert.ToDateTime(ViewData["InStoreDate_ForSetting"].ToString()), Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"))) - 7) * Convert.ToDouble(EverUnReleaseWeight_ForSetting) * Convert.ToDouble(new T_FeeRate().GetFeeRateValue("5-1-5"))).ToString("0.00");

                    break;
                case "6"://分运行李
                    //扣货费
                    EverUnReleaseFee_ForSetting = ((DateTimeHelper.DateDiff(EnumDateCompare.day, Convert.ToDateTime(ViewData["InStoreDate_ForSetting"].ToString()), Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"))) - 7 <= 0 ? 0 : DateTimeHelper.DateDiff(EnumDateCompare.day, Convert.ToDateTime(ViewData["InStoreDate_ForSetting"].ToString()), Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"))) - 7) * Convert.ToDouble(EverUnReleaseWeight_ForSetting) * Convert.ToDouble(new T_FeeRate().GetFeeRateValue("6-1-5"))).ToString("0.00");

                    break;
                default:
                    break;
            }
            ViewData["EverUnReleaseFee_ForSetting"] = EverUnReleaseFee_ForSetting;

            strRet = "{\"result\":\"" + resulr + "\",\"message\":\"" + message + "\",\"row\":[{\"EverUnReleaseNum_ForSetting\":\"" + EverUnReleaseNum_ForSetting + "\",\"EverUnReleaseWeight_ForSetting\":\"" + EverUnReleaseWeight_ForSetting + "\",\"CustomCategory_ForSetting\":\"" + CustomCategory_ForSetting + "\",\"hid_CustomCategory_ForSetting\":\"" + hid_CustomCategory_ForSetting + "\",\"EverUnReleaseFee_ForSetting\":\"" + EverUnReleaseFee_ForSetting + "\"}]}";
            return strRet;
        }

        [HttpPost]
        public string Manul_ComputeUnReleaseInfo(string customCategory, string intervalNum, string intervalDays, string ActualWeight)
        {
            string resulr = "0.00";
            customCategory = Server.UrlDecode(customCategory);
            intervalNum = Server.UrlDecode(intervalNum);
            intervalDays = Server.UrlDecode(intervalDays);
            ActualWeight = Server.UrlDecode(ActualWeight);
            try
            {
                //计算费用
                switch (customCategory)
                {
                    case "2"://样品
                        //扣货费
                        resulr = ((Convert.ToInt32(intervalDays) - 3 <= 0 ? 0 : Convert.ToInt32(intervalDays) - 3) * Convert.ToInt32(intervalNum) * Convert.ToDouble(new T_FeeRate().GetFeeRateValue("2-1-3"))).ToString();

                        break;
                    case "3"://KJ-3
                        //扣货费
                        resulr = ((Convert.ToInt32(intervalDays) - 3 <= 0 ? 0 : Convert.ToInt32(intervalDays) - 3) * Convert.ToInt32(intervalNum) * Convert.ToDouble(new T_FeeRate().GetFeeRateValue("2-1-3"))).ToString();

                        break;
                    case "4"://D类
                        //扣货费
                        resulr = ((Convert.ToInt32(intervalDays) - 3 <= 0 ? 0 : Convert.ToInt32(intervalDays) - 3) * Convert.ToInt32(intervalNum) * Convert.ToDouble(new T_FeeRate().GetFeeRateValue("2-1-3"))).ToString();

                        break;
                    case "5"://个人物品
                        //扣货费
                        resulr = ((Convert.ToInt32(intervalDays) - 7 <= 0 ? 0 : Convert.ToInt32(intervalDays) - 7) * Convert.ToDouble(ActualWeight) * Convert.ToDouble(new T_FeeRate().GetFeeRateValue("5-1-5"))).ToString();

                        break;
                    case "6"://分运行李
                        //扣货费
                        resulr = ((Convert.ToInt32(intervalDays) - 7 <= 0 ? 0 : Convert.ToInt32(intervalDays) - 7) * Convert.ToDouble(ActualWeight) * Convert.ToDouble(new T_FeeRate().GetFeeRateValue("5-1-5"))).ToString();

                        break;
                    default:
                        break;
                }

            }
            catch (Exception ex)
            {
                resulr = "0.00";
            }

            return resulr;
        }

        ///// <summary>
        ///// 打印扣货单
        ///// </summary>
        ///// <param name="strCurrentReleaseSubWayBill">本次放行的分运单信息(单号1,单号2……)</param>
        ///// <param name="strWBID"></param>
        ///// <param name="iPrintType">0:打印预览   1:确认打印</param>
        ///// <returns></returns>
        //[HttpGet]
        //public ActionResult Print(string strCurrentReleaseSubWayBill, string strWBID, int iPrintType, string FlowNum_ForPrint, string wbSerialNum_ForPrint, string InStoreDate_ForSetting, string PickGoodsDate_ForSetting, string EverUnReleaseNum_ForSetting, string IntervalDays_ForSetting, string EverUnReleaseWeight_ForSetting, string EverUnReleaseFee_ForSetting, string ddlPayMode_ForSetting)
        //{
        //    string CurrentReleaseSubWayBill = Server.UrlDecode(strCurrentReleaseSubWayBill).Replace("，", ",");
        //    string[] arrCurrentReleaseSubWayBill = CurrentReleaseSubWayBill.Split(',');
        //    StringBuilder sbCurrentReleaseSubWayBill = new StringBuilder("");
        //    DataSet dsCurrentReleaseSubWayBill = new DataSet();
        //    DataSet dsStillUnReleaseSubWayBill = new DataSet();
        //    for (int i = 0; i < arrCurrentReleaseSubWayBill.Length; i++)
        //    {
        //        if (arrCurrentReleaseSubWayBill[i].Trim() != "")
        //        {
        //            sbCurrentReleaseSubWayBill.AppendFormat("'{0}',", arrCurrentReleaseSubWayBill[i].Trim());
        //        }
        //    }

        //    if (sbCurrentReleaseSubWayBill.ToString().EndsWith(","))
        //    {
        //        sbCurrentReleaseSubWayBill = new StringBuilder(sbCurrentReleaseSubWayBill.ToString().Substring(0, sbCurrentReleaseSubWayBill.ToString().Length - 1));
        //    }

        //    if (sbCurrentReleaseSubWayBill.ToString() != "")
        //    {
        //        dsCurrentReleaseSubWayBill = SqlServerHelper.Query(string.Format(" select swbSerialNum,swbDescription_CHN,swbDescription_ENG,swbNumber,swbWeight,convert(nvarchar(10),DetainDate,120) DetainDate,swbID from  V_WayBill_SubWayBill where wbID={0} and swbSerialNum in ({1})", strWBID, sbCurrentReleaseSubWayBill.ToString()));
        //        dsStillUnReleaseSubWayBill = SqlServerHelper.Query(string.Format(" select swbSerialNum,swbDescription_CHN,swbDescription_ENG,swbNumber,swbWeight,convert(nvarchar(10),DetainDate,120) DetainDate,swbID from  V_WayBill_SubWayBill where wbID={0} and swbNeedCheck=3 and swbSerialNum not in ({1})", strWBID, sbCurrentReleaseSubWayBill.ToString()));
        //    }

        //    string str_FlowNum_ForPrint = FlowNum_ForPrint == null ? "" : Server.UrlDecode(FlowNum_ForPrint);
        //    string str_wbSerialNum_ForPrint = FlowNum_ForPrint == null ? "" : Server.UrlDecode(wbSerialNum_ForPrint);
        //    string str_InStoreDate_ForSetting = FlowNum_ForPrint == null ? "" : Server.UrlDecode(InStoreDate_ForSetting);
        //    string str_PickGoodsDate_ForSetting = FlowNum_ForPrint == null ? "" : Server.UrlDecode(PickGoodsDate_ForSetting);
        //    string str_EverUnReleaseNum_ForSetting = FlowNum_ForPrint == null ? "" : Server.UrlDecode(EverUnReleaseNum_ForSetting);
        //    string str_IntervalDays_ForSetting = FlowNum_ForPrint == null ? "" : Server.UrlDecode(IntervalDays_ForSetting);
        //    string str_EverUnReleaseWeight_ForSetting = FlowNum_ForPrint == null ? "" : Server.UrlDecode(EverUnReleaseWeight_ForSetting);
        //    string str_EverUnReleaseFee_ForSetting = FlowNum_ForPrint == null ? "" : Server.UrlDecode(EverUnReleaseFee_ForSetting);
        //    string str_ddlPayMode_ForSetting = ddlPayMode_ForSetting == null ? "" : Server.UrlDecode(ddlPayMode_ForSetting);

        //    LocalReport localReport = new LocalReport();
        //    localReport.ReportPath = Server.MapPath(STR_REPORT_URL);
        //    ReportDataSource dtCurrentReleaseSubWayBill = new ReportDataSource("CurrentReleaseSubWayBill_DS", dsCurrentReleaseSubWayBill.Tables[0]);
        //    ReportDataSource dtStillUnReleaseSubWayBill = new ReportDataSource("StillUnReleaseSubWayBill_DS", dsStillUnReleaseSubWayBill.Tables[0]);

        //    ReportParameter var_FlowNum_ForPrint = new ReportParameter("FlowNum_ForPrint", str_FlowNum_ForPrint.ToString());
        //    ReportParameter var_wbSerialNum_ForPrint = new ReportParameter("wbSerialNum_ForPrint", str_wbSerialNum_ForPrint.ToString());
        //    ReportParameter var_InStoreDate_ForSetting = new ReportParameter("InStoreDate_ForSetting", str_InStoreDate_ForSetting.ToString());
        //    ReportParameter var_PickGoodsDate_ForSetting = new ReportParameter("PickGoodsDate_ForSetting", str_PickGoodsDate_ForSetting.ToString());
        //    ReportParameter var_EverUnReleaseNum_ForSetting = new ReportParameter("EverUnReleaseNum_ForSetting", str_EverUnReleaseNum_ForSetting.ToString());
        //    ReportParameter var_IntervalDays_ForSetting = new ReportParameter("IntervalDays_ForSetting", str_IntervalDays_ForSetting.ToString());
        //    ReportParameter var_EverUnReleaseWeight_ForSetting = new ReportParameter("EverUnReleaseWeight_ForSetting", str_EverUnReleaseWeight_ForSetting.ToString());
        //    ReportParameter var_EverUnReleaseFee_ForSetting = new ReportParameter("EverUnReleaseFee_ForSetting", str_EverUnReleaseFee_ForSetting.ToString());
        //    ReportParameter var_ddlPayMode_ForSetting = new ReportParameter("ddlPayMode_ForSetting", str_ddlPayMode_ForSetting.ToString());

        //    localReport.SetParameters(new ReportParameter[] { var_FlowNum_ForPrint });
        //    localReport.SetParameters(new ReportParameter[] { var_wbSerialNum_ForPrint });
        //    localReport.SetParameters(new ReportParameter[] { var_InStoreDate_ForSetting });
        //    localReport.SetParameters(new ReportParameter[] { var_PickGoodsDate_ForSetting });
        //    localReport.SetParameters(new ReportParameter[] { var_EverUnReleaseNum_ForSetting });
        //    localReport.SetParameters(new ReportParameter[] { var_IntervalDays_ForSetting });
        //    localReport.SetParameters(new ReportParameter[] { var_EverUnReleaseWeight_ForSetting });
        //    localReport.SetParameters(new ReportParameter[] { var_EverUnReleaseFee_ForSetting });
        //    localReport.SetParameters(new ReportParameter[] { var_ddlPayMode_ForSetting });

        //    localReport.DataSources.Add(dtCurrentReleaseSubWayBill);
        //    localReport.DataSources.Add(dtStillUnReleaseSubWayBill);
        //    string reportType = "PDF";
        //    string mimeType;
        //    string encoding = "UTF-8";
        //    string fileNameExtension;

        //    string deviceInfo = "<DeviceInfo>" +
        //        " <OutputFormat>PDF</OutputFormat>" +
        //        " <PageWidth>12in</PageWidth>" +
        //        " <PageHeigth>6in</PageHeigth>" +
        //        " <MarginTop>0.2in</MarginTop>" +
        //        " <MarginLeft>1in</MarginLeft>" +
        //        " <MarginRight>1in</MarginRight>" +
        //        " <MarginBottom>0.2in</MarginBottom>" +
        //        " </DeviceInfo>";

        //    Warning[] warnings;
        //    string[] streams;
        //    byte[] renderedBytes;

        //    renderedBytes = localReport.Render(reportType, deviceInfo, out mimeType, out encoding, out fileNameExtension, out streams, out warnings);

        //    return File(renderedBytes, mimeType);
        //}

        /// <summary>
        /// 打印扣货单
        /// </summary>
        /// <param name="strCurrentReleaseSubWayBill">本次放行的分运单信息(单号1,单号2……)</param>
        /// <param name="strWBID"></param>
        /// <param name="iPrintType">0:打印预览   1:确认打印</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Print(string strCurrentReleaseSubWayBill, string strWBID, int iPrintType, string FlowNum_ForPrint, string wbSerialNum_ForPrint, string InStoreDate_ForSetting, string PickGoodsDate_ForSetting, string EverUnReleaseNum_ForSetting, string IntervalDays_ForSetting, string EverUnReleaseWeight_ForSetting, string EverUnReleaseFee_ForSetting, string ddlPayMode_ForSetting)
        {
            string CurrentReleaseSubWayBill = Server.UrlDecode(strCurrentReleaseSubWayBill).Replace("，", ",");
            string[] arrCurrentReleaseSubWayBill = CurrentReleaseSubWayBill.Split(',');
            StringBuilder sbCurrentReleaseSubWayBill = new StringBuilder("");
            DataSet dsCurrentReleaseSubWayBill = new DataSet();
            DataSet dsStillUnReleaseSubWayBill = new DataSet();
            for (int i = 0; i < arrCurrentReleaseSubWayBill.Length; i++)
            {
                if (arrCurrentReleaseSubWayBill[i].Trim() != "")
                {
                    sbCurrentReleaseSubWayBill.AppendFormat("'{0}',", arrCurrentReleaseSubWayBill[i].Trim());
                }
            }

            if (sbCurrentReleaseSubWayBill.ToString().EndsWith(","))
            {
                sbCurrentReleaseSubWayBill = new StringBuilder(sbCurrentReleaseSubWayBill.ToString().Substring(0, sbCurrentReleaseSubWayBill.ToString().Length - 1));
            }

            if (sbCurrentReleaseSubWayBill.ToString() != "")
            {
                dsCurrentReleaseSubWayBill = SqlServerHelper.Query(string.Format(" select swbSerialNum,DetainDate,swbID from  V_WayBill_SubWayBill where wbID={0} and swbSerialNum in ({1})", strWBID, sbCurrentReleaseSubWayBill.ToString()));
                dsStillUnReleaseSubWayBill = SqlServerHelper.Query(string.Format(" select swbSerialNum,DetainDate,swbID from  V_WayBill_SubWayBill where wbID={0} and swbNeedCheck=3 and swbSerialNum not in ({1})", strWBID, sbCurrentReleaseSubWayBill.ToString()));
            }

            string str_FlowNum_ForPrint = FlowNum_ForPrint == null ? "" : Server.UrlDecode(FlowNum_ForPrint);
            string str_wbSerialNum_ForPrint = FlowNum_ForPrint == null ? "" : Server.UrlDecode(wbSerialNum_ForPrint);
            string str_InStoreDate_ForSetting = FlowNum_ForPrint == null ? "" : Server.UrlDecode(InStoreDate_ForSetting);
            string str_PickGoodsDate_ForSetting = FlowNum_ForPrint == null ? "" : Server.UrlDecode(PickGoodsDate_ForSetting);
            string str_EverUnReleaseNum_ForSetting = FlowNum_ForPrint == null ? "" : Server.UrlDecode(EverUnReleaseNum_ForSetting);
            string str_IntervalDays_ForSetting = FlowNum_ForPrint == null ? "" : Server.UrlDecode(IntervalDays_ForSetting);
            string str_EverUnReleaseWeight_ForSetting = FlowNum_ForPrint == null ? "" : Server.UrlDecode(EverUnReleaseWeight_ForSetting);
            string str_EverUnReleaseFee_ForSetting = FlowNum_ForPrint == null ? "" : Server.UrlDecode(EverUnReleaseFee_ForSetting);
            string str_ddlPayMode_ForSetting = ddlPayMode_ForSetting == null ? "" : Server.UrlDecode(ddlPayMode_ForSetting);

            LocalReport localReport = new LocalReport();
            localReport.ReportPath = Server.MapPath(STR_REPORT_URL);
            ReportDataSource dtCurrentReleaseSubWayBill = new ReportDataSource("CurrentReleaseSubWayBill_DS", dsCurrentReleaseSubWayBill.Tables[0]);
            ReportDataSource dtStillUnReleaseSubWayBill = new ReportDataSource("StillUnReleaseSubWayBill_DS", dsStillUnReleaseSubWayBill.Tables[0]);

            ReportParameter var_FlowNum_ForPrint = new ReportParameter("FlowNum_ForPrint", str_FlowNum_ForPrint.ToString());
            ReportParameter var_wbSerialNum_ForPrint = new ReportParameter("wbSerialNum_ForPrint", str_wbSerialNum_ForPrint.ToString());
            ReportParameter var_InStoreDate_ForSetting = new ReportParameter("InStoreDate_ForSetting", str_InStoreDate_ForSetting.ToString());
            ReportParameter var_PickGoodsDate_ForSetting = new ReportParameter("PickGoodsDate_ForSetting", str_PickGoodsDate_ForSetting.ToString());
            ReportParameter var_EverUnReleaseNum_ForSetting = new ReportParameter("EverUnReleaseNum_ForSetting", str_EverUnReleaseNum_ForSetting.ToString());
            ReportParameter var_IntervalDays_ForSetting = new ReportParameter("IntervalDays_ForSetting", str_IntervalDays_ForSetting.ToString());
            ReportParameter var_EverUnReleaseWeight_ForSetting = new ReportParameter("EverUnReleaseWeight_ForSetting", str_EverUnReleaseWeight_ForSetting.ToString());
            ReportParameter var_EverUnReleaseFee_ForSetting = new ReportParameter("EverUnReleaseFee_ForSetting", str_EverUnReleaseFee_ForSetting.ToString());
            ReportParameter var_ddlPayMode_ForSetting = new ReportParameter("ddlPayMode_ForSetting", str_ddlPayMode_ForSetting.ToString());

            localReport.SetParameters(new ReportParameter[] { var_FlowNum_ForPrint });
            localReport.SetParameters(new ReportParameter[] { var_wbSerialNum_ForPrint });
            localReport.SetParameters(new ReportParameter[] { var_InStoreDate_ForSetting });
            localReport.SetParameters(new ReportParameter[] { var_PickGoodsDate_ForSetting });
            localReport.SetParameters(new ReportParameter[] { var_EverUnReleaseNum_ForSetting });
            localReport.SetParameters(new ReportParameter[] { var_IntervalDays_ForSetting });
            localReport.SetParameters(new ReportParameter[] { var_EverUnReleaseWeight_ForSetting });
            localReport.SetParameters(new ReportParameter[] { var_EverUnReleaseFee_ForSetting });
            localReport.SetParameters(new ReportParameter[] { var_ddlPayMode_ForSetting });

            localReport.SubreportProcessing += new SubreportProcessingEventHandler(LocalReport_SubreportProcessing);

            localReport.DataSources.Add(dtCurrentReleaseSubWayBill);
            localReport.DataSources.Add(dtStillUnReleaseSubWayBill);
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
            dtCustom.Columns.Add("DetainDate", Type.GetType("System.String"));
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

        public DataTable GetData_Sub_Detail_Current(string swbSerialNums,string  wbId)
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

            if (wbId.ToString() != "")
            {
                if (strWhereTemp != "")
                {
                    strWhereTemp = strWhereTemp + " and  (wbID=" + wbId.ToString() + ") ";
                }
                else
                {
                    strWhereTemp = strWhereTemp + " (wbID=" + wbId.ToString() + ") ";
                }
            }

            if (swbSerialNums.ToString() != "")
            {
                if (strWhereTemp != "")
                {
                    strWhereTemp = strWhereTemp + " and  (swbSerialNum in (" + swbSerialNums.ToString() + ")) ";
                }
                else
                {
                    strWhereTemp = strWhereTemp + " (swbSerialNum in (" + swbSerialNums.ToString() + ")) ";
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
            dtCustom.Columns.Add("DetainDate", Type.GetType("System.String"));
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

        public DataTable GetData_Sub_Detail_Still(string swbSerialNums, string wbId)
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

            strWhereTemp = " (swbNeedCheck=3) ";

            if (wbId.ToString() != "")
            {
                if (strWhereTemp != "")
                {
                    strWhereTemp = strWhereTemp + " and  (wbID=" + wbId.ToString() + ") ";
                }
                else
                {
                    strWhereTemp = strWhereTemp + " (wbID=" + wbId.ToString() + ") ";
                }
            }

            if (swbSerialNums.ToString() != "")
            {
                if (strWhereTemp != "")
                {
                    strWhereTemp = strWhereTemp + " and  (swbSerialNum  not in (" + swbSerialNums.ToString() + ")) ";
                }
                else
                {
                    strWhereTemp = strWhereTemp + " (swbSerialNum not in (" + swbSerialNums.ToString() + ")) ";
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
            dtCustom.Columns.Add("DetainDate", Type.GetType("System.String"));
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
            string strSwbId_Current="-1";
            string strSwbId_Still = "-1";
            DataTable dtSubWayBillDetail_Sub = null;

            try
            {
                strSwbId_Current = e.Parameters["SubWayBillDetail_SwbId_Current"].Values[0];
            }
            catch (Exception ex)
            {
                strSwbId_Current = "-1";
            }

            try
            {
                strSwbId_Still = e.Parameters["SubWayBillDetail_SwbId_Still"].Values[0];
            }
            catch (Exception ex)
            {
                strSwbId_Still = "-1";
            }

            if (strSwbId_Current != "-1")
            {
                dtSubWayBillDetail_Sub = null;
                dtSubWayBillDetail_Sub = GetData_Sub_Detail(strSwbId_Current);
                e.DataSources.Add(new Microsoft.Reporting.WebForms.ReportDataSource("Huayu_UnReleaseSeet_Current_Detail_DS", dtSubWayBillDetail_Sub));
            }

            if (strSwbId_Still != "-1")
            {
                dtSubWayBillDetail_Sub = null;
                dtSubWayBillDetail_Sub = GetData_Sub_Detail(strSwbId_Still);
                e.DataSources.Add(new Microsoft.Reporting.WebForms.ReportDataSource("Huayu_UnReleaseSeet_Still_Detail_DS", dtSubWayBillDetail_Sub));
            }

        }

        [HttpPost]
        public string SendMail_PDF(string strCurrentReleaseSubWayBill, string strWBID, int iPrintType, string FlowNum_ForPrint, string wbSerialNum_ForPrint, string InStoreDate_ForSetting, string PickGoodsDate_ForSetting, string EverUnReleaseNum_ForSetting, string IntervalDays_ForSetting, string EverUnReleaseWeight_ForSetting, string EverUnReleaseFee_ForSetting, string ddlPayMode_ForSetting)
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
                            if (dsUser.Tables[0].Rows[0]["iSendUnReleaseGoodsEmail"].ToString() == "1")
                            {
                                string CurrentReleaseSubWayBill = Server.UrlDecode(strCurrentReleaseSubWayBill).Replace("，", ",");
                                string[] arrCurrentReleaseSubWayBill = CurrentReleaseSubWayBill.Split(',');
                                StringBuilder sbCurrentReleaseSubWayBill = new StringBuilder("");
                                DataSet dsCurrentReleaseSubWayBill = new DataSet();
                                DataSet dsStillUnReleaseSubWayBill = new DataSet();
                                for (int i = 0; i < arrCurrentReleaseSubWayBill.Length; i++)
                                {
                                    if (arrCurrentReleaseSubWayBill[i].Trim() != "")
                                    {
                                        sbCurrentReleaseSubWayBill.AppendFormat("'{0}',", arrCurrentReleaseSubWayBill[i].Trim());
                                    }
                                }

                                if (sbCurrentReleaseSubWayBill.ToString().EndsWith(","))
                                {
                                    sbCurrentReleaseSubWayBill = new StringBuilder(sbCurrentReleaseSubWayBill.ToString().Substring(0, sbCurrentReleaseSubWayBill.ToString().Length - 1));
                                }

                                if (sbCurrentReleaseSubWayBill.ToString() != "")
                                {
                                    dsCurrentReleaseSubWayBill = SqlServerHelper.Query(string.Format(" select swbSerialNum,swbDescription_CHN,swbDescription_ENG,swbNumber,swbWeight,convert(nvarchar(10),DetainDate,120) DetainDate from  V_WayBill_SubWayBill where wbID={0} and swbSerialNum in ({1})", strWBID, sbCurrentReleaseSubWayBill.ToString()));
                                    dsStillUnReleaseSubWayBill = SqlServerHelper.Query(string.Format(" select swbSerialNum,swbDescription_CHN,swbDescription_ENG,swbNumber,swbWeight,convert(nvarchar(10),DetainDate,120) DetainDate from  V_WayBill_SubWayBill where wbID={0} and swbNeedCheck=3 and swbSerialNum not in ({1})", strWBID, sbCurrentReleaseSubWayBill.ToString()));
                                }

                                string str_FlowNum_ForPrint = FlowNum_ForPrint == null ? "" : Server.UrlDecode(FlowNum_ForPrint);
                                string str_wbSerialNum_ForPrint = FlowNum_ForPrint == null ? "" : Server.UrlDecode(wbSerialNum_ForPrint);
                                string str_InStoreDate_ForSetting = FlowNum_ForPrint == null ? "" : Server.UrlDecode(InStoreDate_ForSetting);
                                string str_PickGoodsDate_ForSetting = FlowNum_ForPrint == null ? "" : Server.UrlDecode(PickGoodsDate_ForSetting);
                                string str_EverUnReleaseNum_ForSetting = FlowNum_ForPrint == null ? "" : Server.UrlDecode(EverUnReleaseNum_ForSetting);
                                string str_IntervalDays_ForSetting = FlowNum_ForPrint == null ? "" : Server.UrlDecode(IntervalDays_ForSetting);
                                string str_EverUnReleaseWeight_ForSetting = FlowNum_ForPrint == null ? "" : Server.UrlDecode(EverUnReleaseWeight_ForSetting);
                                string str_EverUnReleaseFee_ForSetting = FlowNum_ForPrint == null ? "" : Server.UrlDecode(EverUnReleaseFee_ForSetting);
                                string str_ddlPayMode_ForSetting = ddlPayMode_ForSetting == null ? "" : Server.UrlDecode(ddlPayMode_ForSetting);

                                LocalReport localReport = new LocalReport();
                                localReport.ReportPath = Server.MapPath(STR_REPORT_URL);
                                ReportDataSource dtCurrentReleaseSubWayBill = new ReportDataSource("CurrentReleaseSubWayBill_DS", dsCurrentReleaseSubWayBill.Tables[0]);
                                ReportDataSource dtStillUnReleaseSubWayBill = new ReportDataSource("StillUnReleaseSubWayBill_DS", dsStillUnReleaseSubWayBill.Tables[0]);

                                ReportParameter var_FlowNum_ForPrint = new ReportParameter("FlowNum_ForPrint", str_FlowNum_ForPrint.ToString());
                                ReportParameter var_wbSerialNum_ForPrint = new ReportParameter("wbSerialNum_ForPrint", str_wbSerialNum_ForPrint.ToString());
                                ReportParameter var_InStoreDate_ForSetting = new ReportParameter("InStoreDate_ForSetting", str_InStoreDate_ForSetting.ToString());
                                ReportParameter var_PickGoodsDate_ForSetting = new ReportParameter("PickGoodsDate_ForSetting", str_PickGoodsDate_ForSetting.ToString());
                                ReportParameter var_EverUnReleaseNum_ForSetting = new ReportParameter("EverUnReleaseNum_ForSetting", str_EverUnReleaseNum_ForSetting.ToString());
                                ReportParameter var_IntervalDays_ForSetting = new ReportParameter("IntervalDays_ForSetting", str_IntervalDays_ForSetting.ToString());
                                ReportParameter var_EverUnReleaseWeight_ForSetting = new ReportParameter("EverUnReleaseWeight_ForSetting", str_EverUnReleaseWeight_ForSetting.ToString());
                                ReportParameter var_EverUnReleaseFee_ForSetting = new ReportParameter("EverUnReleaseFee_ForSetting", str_EverUnReleaseFee_ForSetting.ToString());
                                ReportParameter var_ddlPayMode_ForSetting = new ReportParameter("ddlPayMode_ForSetting", str_ddlPayMode_ForSetting.ToString());

                                localReport.SetParameters(new ReportParameter[] { var_FlowNum_ForPrint });
                                localReport.SetParameters(new ReportParameter[] { var_wbSerialNum_ForPrint });
                                localReport.SetParameters(new ReportParameter[] { var_InStoreDate_ForSetting });
                                localReport.SetParameters(new ReportParameter[] { var_PickGoodsDate_ForSetting });
                                localReport.SetParameters(new ReportParameter[] { var_EverUnReleaseNum_ForSetting });
                                localReport.SetParameters(new ReportParameter[] { var_IntervalDays_ForSetting });
                                localReport.SetParameters(new ReportParameter[] { var_EverUnReleaseWeight_ForSetting });
                                localReport.SetParameters(new ReportParameter[] { var_EverUnReleaseFee_ForSetting });
                                localReport.SetParameters(new ReportParameter[] { var_ddlPayMode_ForSetting });

                                localReport.DataSources.Add(dtCurrentReleaseSubWayBill);
                                localReport.DataSources.Add(dtStillUnReleaseSubWayBill);
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
                                            string FileName = Server.MapPath("~/Temp/PDF/") + "扣货提货单_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".pdf";
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
                strResult = "{\"result\":\"error\",\"message\":\"发送邮件失败，原因:"+ex.Message+"\"}";
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
        //public ActionResult Excel(string strCurrentReleaseSubWayBill, string strWBID, int iPrintType, string FlowNum_ForPrint, string wbSerialNum_ForPrint, string InStoreDate_ForSetting, string PickGoodsDate_ForSetting, string EverUnReleaseNum_ForSetting, string IntervalDays_ForSetting, string EverUnReleaseWeight_ForSetting, string EverUnReleaseFee_ForSetting, string ddlPayMode_ForSetting, string browserType)
        //{
        //    string CurrentReleaseSubWayBill = Server.UrlDecode(strCurrentReleaseSubWayBill).Replace("，", ",");
        //    string[] arrCurrentReleaseSubWayBill = CurrentReleaseSubWayBill.Split(',');
        //    StringBuilder sbCurrentReleaseSubWayBill = new StringBuilder("");
        //    DataSet dsCurrentReleaseSubWayBill = new DataSet();
        //    DataSet dsStillUnReleaseSubWayBill = new DataSet();
        //    for (int i = 0; i < arrCurrentReleaseSubWayBill.Length; i++)
        //    {
        //        if (arrCurrentReleaseSubWayBill[i].Trim() != "")
        //        {
        //            sbCurrentReleaseSubWayBill.AppendFormat("'{0}',", arrCurrentReleaseSubWayBill[i].Trim());
        //        }
        //    }

        //    if (sbCurrentReleaseSubWayBill.ToString().EndsWith(","))
        //    {
        //        sbCurrentReleaseSubWayBill = new StringBuilder(sbCurrentReleaseSubWayBill.ToString().Substring(0, sbCurrentReleaseSubWayBill.ToString().Length - 1));
        //    }

        //    if (sbCurrentReleaseSubWayBill.ToString() != "")
        //    {
        //        dsCurrentReleaseSubWayBill = SqlServerHelper.Query(string.Format(" select swbSerialNum,swbDescription_CHN,swbDescription_ENG,swbNumber,swbWeight,convert(nvarchar(10),DetainDate,120) DetainDate from  V_WayBill_SubWayBill where wbID={0} and swbSerialNum in ({1})", strWBID, sbCurrentReleaseSubWayBill.ToString()));
        //        dsStillUnReleaseSubWayBill = SqlServerHelper.Query(string.Format(" select swbSerialNum,swbDescription_CHN,swbDescription_ENG,swbNumber,swbWeight,convert(nvarchar(10),DetainDate,120) DetainDate from  V_WayBill_SubWayBill where wbID={0} and swbNeedCheck=3 and swbSerialNum not in ({1})", strWBID, sbCurrentReleaseSubWayBill.ToString()));
        //    }

        //    string str_FlowNum_ForPrint = FlowNum_ForPrint == null ? "" : Server.UrlDecode(FlowNum_ForPrint);
        //    string str_wbSerialNum_ForPrint = FlowNum_ForPrint == null ? "" : Server.UrlDecode(wbSerialNum_ForPrint);
        //    string str_InStoreDate_ForSetting = FlowNum_ForPrint == null ? "" : Server.UrlDecode(InStoreDate_ForSetting);
        //    string str_PickGoodsDate_ForSetting = FlowNum_ForPrint == null ? "" : Server.UrlDecode(PickGoodsDate_ForSetting);
        //    string str_EverUnReleaseNum_ForSetting = FlowNum_ForPrint == null ? "" : Server.UrlDecode(EverUnReleaseNum_ForSetting);
        //    string str_IntervalDays_ForSetting = FlowNum_ForPrint == null ? "" : Server.UrlDecode(IntervalDays_ForSetting);
        //    string str_EverUnReleaseWeight_ForSetting = FlowNum_ForPrint == null ? "" : Server.UrlDecode(EverUnReleaseWeight_ForSetting);
        //    string str_EverUnReleaseFee_ForSetting = FlowNum_ForPrint == null ? "" : Server.UrlDecode(EverUnReleaseFee_ForSetting);
        //    string str_ddlPayMode_ForSetting = ddlPayMode_ForSetting == null ? "" : Server.UrlDecode(ddlPayMode_ForSetting);

        //    LocalReport localReport = new LocalReport();
        //    localReport.ReportPath = Server.MapPath(STR_REPORT_URL);
        //    ReportDataSource dtCurrentReleaseSubWayBill = new ReportDataSource("CurrentReleaseSubWayBill_DS", dsCurrentReleaseSubWayBill.Tables[0]);
        //    ReportDataSource dtStillUnReleaseSubWayBill = new ReportDataSource("StillUnReleaseSubWayBill_DS", dsStillUnReleaseSubWayBill.Tables[0]);

        //    ReportParameter var_FlowNum_ForPrint = new ReportParameter("FlowNum_ForPrint", str_FlowNum_ForPrint.ToString());
        //    ReportParameter var_wbSerialNum_ForPrint = new ReportParameter("wbSerialNum_ForPrint", str_wbSerialNum_ForPrint.ToString());
        //    ReportParameter var_InStoreDate_ForSetting = new ReportParameter("InStoreDate_ForSetting", str_InStoreDate_ForSetting.ToString());
        //    ReportParameter var_PickGoodsDate_ForSetting = new ReportParameter("PickGoodsDate_ForSetting", str_PickGoodsDate_ForSetting.ToString());
        //    ReportParameter var_EverUnReleaseNum_ForSetting = new ReportParameter("EverUnReleaseNum_ForSetting", str_EverUnReleaseNum_ForSetting.ToString());
        //    ReportParameter var_IntervalDays_ForSetting = new ReportParameter("IntervalDays_ForSetting", str_IntervalDays_ForSetting.ToString());
        //    ReportParameter var_EverUnReleaseWeight_ForSetting = new ReportParameter("EverUnReleaseWeight_ForSetting", str_EverUnReleaseWeight_ForSetting.ToString());
        //    ReportParameter var_EverUnReleaseFee_ForSetting = new ReportParameter("EverUnReleaseFee_ForSetting", str_EverUnReleaseFee_ForSetting.ToString());
        //    ReportParameter var_ddlPayMode_ForSetting = new ReportParameter("ddlPayMode_ForSetting", str_ddlPayMode_ForSetting.ToString());

        //    localReport.SetParameters(new ReportParameter[] { var_FlowNum_ForPrint });
        //    localReport.SetParameters(new ReportParameter[] { var_wbSerialNum_ForPrint });
        //    localReport.SetParameters(new ReportParameter[] { var_InStoreDate_ForSetting });
        //    localReport.SetParameters(new ReportParameter[] { var_PickGoodsDate_ForSetting });
        //    localReport.SetParameters(new ReportParameter[] { var_EverUnReleaseNum_ForSetting });
        //    localReport.SetParameters(new ReportParameter[] { var_IntervalDays_ForSetting });
        //    localReport.SetParameters(new ReportParameter[] { var_EverUnReleaseWeight_ForSetting });
        //    localReport.SetParameters(new ReportParameter[] { var_EverUnReleaseFee_ForSetting });
        //    localReport.SetParameters(new ReportParameter[] { var_ddlPayMode_ForSetting });

        //    localReport.DataSources.Add(dtCurrentReleaseSubWayBill);
        //    localReport.DataSources.Add(dtStillUnReleaseSubWayBill);
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

        //    string strOutputFileName = "扣货提货单_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";

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

        //    switch (iPrintType)
        //    {
        //        case 1:
        //            try
        //            {
        //                string FileName = Server.MapPath("~/Temp/PDF/") + strOutputFileName;
        //                using (FileStream wf = new FileStream(FileName, FileMode.Create))
        //                {
        //                    wf.Write(bytes, 0, bytes.Length);
        //                    wf.Flush();
        //                    wf.Close();

        //                    DataSet dsWayBill = new T_WayBill().getWayBillInfo(strWBID);
        //                    if (dsWayBill != null)
        //                    {
        //                        if (dsWayBill.Tables[0] != null && dsWayBill.Tables[0].Rows.Count > 0)
        //                        {
        //                            if (NetMail_SendMail(FileName, dsWayBill.Tables[0].Rows[0]["wbCompany"].ToString(), "1"))
        //                            {

        //                            }
        //                            else
        //                            {
        //                                //throw new Exception("邮件发送失败，允许打印");
        //                            }
        //                        }
        //                    }
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                //throw ex;
        //            }

        //            break;
        //        default:
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
        public ActionResult Excel(string strCurrentReleaseSubWayBill, string strWBID, int iPrintType, string FlowNum_ForPrint, string wbSerialNum_ForPrint, string InStoreDate_ForSetting, string PickGoodsDate_ForSetting, string EverUnReleaseNum_ForSetting, string IntervalDays_ForSetting, string EverUnReleaseWeight_ForSetting, string EverUnReleaseFee_ForSetting, string ddlPayMode_ForSetting, string browserType)
        {
            string CurrentReleaseSubWayBill = Server.UrlDecode(strCurrentReleaseSubWayBill).Replace("，", ",");
            string[] arrCurrentReleaseSubWayBill = CurrentReleaseSubWayBill.Split(',');
            StringBuilder sbCurrentReleaseSubWayBill = new StringBuilder("");
            DataSet dsCurrentReleaseSubWayBill = new DataSet();
            DataSet dsStillUnReleaseSubWayBill = new DataSet();
            for (int i = 0; i < arrCurrentReleaseSubWayBill.Length; i++)
            {
                if (arrCurrentReleaseSubWayBill[i].Trim() != "")
                {
                    sbCurrentReleaseSubWayBill.AppendFormat("'{0}',", arrCurrentReleaseSubWayBill[i].Trim());
                }
            }

            if (sbCurrentReleaseSubWayBill.ToString().EndsWith(","))
            {
                sbCurrentReleaseSubWayBill = new StringBuilder(sbCurrentReleaseSubWayBill.ToString().Substring(0, sbCurrentReleaseSubWayBill.ToString().Length - 1));
            }

            if (sbCurrentReleaseSubWayBill.ToString() != "")
            {
                dsCurrentReleaseSubWayBill = SqlServerHelper.Query(string.Format(" select swbSerialNum,DetainDate,swbID from  V_WayBill_SubWayBill where wbID={0} and swbSerialNum in ({1})", strWBID, sbCurrentReleaseSubWayBill.ToString()));
                dsStillUnReleaseSubWayBill = SqlServerHelper.Query(string.Format(" select swbSerialNum,DetainDate,swbID from  V_WayBill_SubWayBill where wbID={0} and swbNeedCheck=3 and swbSerialNum not in ({1})", strWBID, sbCurrentReleaseSubWayBill.ToString()));
            }

            string str_FlowNum_ForPrint = FlowNum_ForPrint == null ? "" : Server.UrlDecode(FlowNum_ForPrint);
            string str_wbSerialNum_ForPrint = FlowNum_ForPrint == null ? "" : Server.UrlDecode(wbSerialNum_ForPrint);
            string str_InStoreDate_ForSetting = FlowNum_ForPrint == null ? "" : Server.UrlDecode(InStoreDate_ForSetting);
            string str_PickGoodsDate_ForSetting = FlowNum_ForPrint == null ? "" : Server.UrlDecode(PickGoodsDate_ForSetting);
            string str_EverUnReleaseNum_ForSetting = FlowNum_ForPrint == null ? "" : Server.UrlDecode(EverUnReleaseNum_ForSetting);
            string str_IntervalDays_ForSetting = FlowNum_ForPrint == null ? "" : Server.UrlDecode(IntervalDays_ForSetting);
            string str_EverUnReleaseWeight_ForSetting = FlowNum_ForPrint == null ? "" : Server.UrlDecode(EverUnReleaseWeight_ForSetting);
            string str_EverUnReleaseFee_ForSetting = FlowNum_ForPrint == null ? "" : Server.UrlDecode(EverUnReleaseFee_ForSetting);
            string str_ddlPayMode_ForSetting = ddlPayMode_ForSetting == null ? "" : Server.UrlDecode(ddlPayMode_ForSetting);

            LocalReport localReport = new LocalReport();
            localReport.ReportPath = Server.MapPath(STR_REPORT_URL);
            ReportDataSource dtCurrentReleaseSubWayBill = new ReportDataSource("CurrentReleaseSubWayBill_DS", dsCurrentReleaseSubWayBill.Tables[0]);
            ReportDataSource dtStillUnReleaseSubWayBill = new ReportDataSource("StillUnReleaseSubWayBill_DS", dsStillUnReleaseSubWayBill.Tables[0]);

            ReportParameter var_FlowNum_ForPrint = new ReportParameter("FlowNum_ForPrint", str_FlowNum_ForPrint.ToString());
            ReportParameter var_wbSerialNum_ForPrint = new ReportParameter("wbSerialNum_ForPrint", str_wbSerialNum_ForPrint.ToString());
            ReportParameter var_InStoreDate_ForSetting = new ReportParameter("InStoreDate_ForSetting", str_InStoreDate_ForSetting.ToString());
            ReportParameter var_PickGoodsDate_ForSetting = new ReportParameter("PickGoodsDate_ForSetting", str_PickGoodsDate_ForSetting.ToString());
            ReportParameter var_EverUnReleaseNum_ForSetting = new ReportParameter("EverUnReleaseNum_ForSetting", str_EverUnReleaseNum_ForSetting.ToString());
            ReportParameter var_IntervalDays_ForSetting = new ReportParameter("IntervalDays_ForSetting", str_IntervalDays_ForSetting.ToString());
            ReportParameter var_EverUnReleaseWeight_ForSetting = new ReportParameter("EverUnReleaseWeight_ForSetting", str_EverUnReleaseWeight_ForSetting.ToString());
            ReportParameter var_EverUnReleaseFee_ForSetting = new ReportParameter("EverUnReleaseFee_ForSetting", str_EverUnReleaseFee_ForSetting.ToString());
            ReportParameter var_ddlPayMode_ForSetting = new ReportParameter("ddlPayMode_ForSetting", str_ddlPayMode_ForSetting.ToString());

            localReport.SetParameters(new ReportParameter[] { var_FlowNum_ForPrint });
            localReport.SetParameters(new ReportParameter[] { var_wbSerialNum_ForPrint });
            localReport.SetParameters(new ReportParameter[] { var_InStoreDate_ForSetting });
            localReport.SetParameters(new ReportParameter[] { var_PickGoodsDate_ForSetting });
            localReport.SetParameters(new ReportParameter[] { var_EverUnReleaseNum_ForSetting });
            localReport.SetParameters(new ReportParameter[] { var_IntervalDays_ForSetting });
            localReport.SetParameters(new ReportParameter[] { var_EverUnReleaseWeight_ForSetting });
            localReport.SetParameters(new ReportParameter[] { var_EverUnReleaseFee_ForSetting });
            localReport.SetParameters(new ReportParameter[] { var_ddlPayMode_ForSetting });

            localReport.SubreportProcessing += new SubreportProcessingEventHandler(LocalReport_SubreportProcessing);

            localReport.DataSources.Add(dtCurrentReleaseSubWayBill);
            localReport.DataSources.Add(dtStillUnReleaseSubWayBill);
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

            string strOutputFileName = "扣货提货单_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";

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

                            DataSet dsWayBill = new T_WayBill().getWayBillInfo(strWBID);
                            if (dsWayBill != null)
                            {
                                if (dsWayBill.Tables[0] != null && dsWayBill.Tables[0].Rows.Count > 0)
                                {
                                    if (NetMail_SendMail(FileName, dsWayBill.Tables[0].Rows[0]["wbCompany"].ToString(), "1"))
                                    {

                                    }
                                    else
                                    {
                                        //throw new Exception("邮件发送失败，允许打印");
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        //throw ex;
                    }

                    break;
                default:
                    break;
            }

            return File(strFileName, "application/vnd.ms-excel", strOutputFileName);
        }

        [HttpPost]
        public string SendEmail_Excel(string strCurrentReleaseSubWayBill, string strWBID, int iPrintType, string FlowNum_ForPrint, string wbSerialNum_ForPrint, string InStoreDate_ForSetting, string PickGoodsDate_ForSetting, string EverUnReleaseNum_ForSetting, string IntervalDays_ForSetting, string EverUnReleaseWeight_ForSetting, string EverUnReleaseFee_ForSetting, string ddlPayMode_ForSetting, string browserType)
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
                            if (dsUser.Tables[0].Rows[0]["iSendUnReleaseGoodsEmail"].ToString() == "1")
                            {
                                string CurrentReleaseSubWayBill = Server.UrlDecode(strCurrentReleaseSubWayBill).Replace("，", ",");
                                string[] arrCurrentReleaseSubWayBill = CurrentReleaseSubWayBill.Split(',');
                                StringBuilder sbCurrentReleaseSubWayBill = new StringBuilder("");
                                DataSet dsCurrentReleaseSubWayBill = new DataSet();
                                DataSet dsStillUnReleaseSubWayBill = new DataSet();
                                for (int i = 0; i < arrCurrentReleaseSubWayBill.Length; i++)
                                {
                                    if (arrCurrentReleaseSubWayBill[i].Trim() != "")
                                    {
                                        sbCurrentReleaseSubWayBill.AppendFormat("'{0}',", arrCurrentReleaseSubWayBill[i].Trim());
                                    }
                                }

                                if (sbCurrentReleaseSubWayBill.ToString().EndsWith(","))
                                {
                                    sbCurrentReleaseSubWayBill = new StringBuilder(sbCurrentReleaseSubWayBill.ToString().Substring(0, sbCurrentReleaseSubWayBill.ToString().Length - 1));
                                }

                                if (sbCurrentReleaseSubWayBill.ToString() != "")
                                {
                                    dsCurrentReleaseSubWayBill = SqlServerHelper.Query(string.Format(" select swbSerialNum,swbDescription_CHN,swbDescription_ENG,swbNumber,swbWeight,convert(nvarchar(10),DetainDate,120) DetainDate from  V_WayBill_SubWayBill where wbID={0} and swbSerialNum in ({1})", strWBID, sbCurrentReleaseSubWayBill.ToString()));
                                    dsStillUnReleaseSubWayBill = SqlServerHelper.Query(string.Format(" select swbSerialNum,swbDescription_CHN,swbDescription_ENG,swbNumber,swbWeight,convert(nvarchar(10),DetainDate,120) DetainDate from  V_WayBill_SubWayBill where wbID={0} and swbNeedCheck=3 and swbSerialNum not in ({1})", strWBID, sbCurrentReleaseSubWayBill.ToString()));
                                }

                                string str_FlowNum_ForPrint = FlowNum_ForPrint == null ? "" : Server.UrlDecode(FlowNum_ForPrint);
                                string str_wbSerialNum_ForPrint = FlowNum_ForPrint == null ? "" : Server.UrlDecode(wbSerialNum_ForPrint);
                                string str_InStoreDate_ForSetting = FlowNum_ForPrint == null ? "" : Server.UrlDecode(InStoreDate_ForSetting);
                                string str_PickGoodsDate_ForSetting = FlowNum_ForPrint == null ? "" : Server.UrlDecode(PickGoodsDate_ForSetting);
                                string str_EverUnReleaseNum_ForSetting = FlowNum_ForPrint == null ? "" : Server.UrlDecode(EverUnReleaseNum_ForSetting);
                                string str_IntervalDays_ForSetting = FlowNum_ForPrint == null ? "" : Server.UrlDecode(IntervalDays_ForSetting);
                                string str_EverUnReleaseWeight_ForSetting = FlowNum_ForPrint == null ? "" : Server.UrlDecode(EverUnReleaseWeight_ForSetting);
                                string str_EverUnReleaseFee_ForSetting = FlowNum_ForPrint == null ? "" : Server.UrlDecode(EverUnReleaseFee_ForSetting);
                                string str_ddlPayMode_ForSetting = ddlPayMode_ForSetting == null ? "" : Server.UrlDecode(ddlPayMode_ForSetting);

                                LocalReport localReport = new LocalReport();
                                localReport.ReportPath = Server.MapPath(STR_REPORT_URL);
                                ReportDataSource dtCurrentReleaseSubWayBill = new ReportDataSource("CurrentReleaseSubWayBill_DS", dsCurrentReleaseSubWayBill.Tables[0]);
                                ReportDataSource dtStillUnReleaseSubWayBill = new ReportDataSource("StillUnReleaseSubWayBill_DS", dsStillUnReleaseSubWayBill.Tables[0]);

                                ReportParameter var_FlowNum_ForPrint = new ReportParameter("FlowNum_ForPrint", str_FlowNum_ForPrint.ToString());
                                ReportParameter var_wbSerialNum_ForPrint = new ReportParameter("wbSerialNum_ForPrint", str_wbSerialNum_ForPrint.ToString());
                                ReportParameter var_InStoreDate_ForSetting = new ReportParameter("InStoreDate_ForSetting", str_InStoreDate_ForSetting.ToString());
                                ReportParameter var_PickGoodsDate_ForSetting = new ReportParameter("PickGoodsDate_ForSetting", str_PickGoodsDate_ForSetting.ToString());
                                ReportParameter var_EverUnReleaseNum_ForSetting = new ReportParameter("EverUnReleaseNum_ForSetting", str_EverUnReleaseNum_ForSetting.ToString());
                                ReportParameter var_IntervalDays_ForSetting = new ReportParameter("IntervalDays_ForSetting", str_IntervalDays_ForSetting.ToString());
                                ReportParameter var_EverUnReleaseWeight_ForSetting = new ReportParameter("EverUnReleaseWeight_ForSetting", str_EverUnReleaseWeight_ForSetting.ToString());
                                ReportParameter var_EverUnReleaseFee_ForSetting = new ReportParameter("EverUnReleaseFee_ForSetting", str_EverUnReleaseFee_ForSetting.ToString());
                                ReportParameter var_ddlPayMode_ForSetting = new ReportParameter("ddlPayMode_ForSetting", str_ddlPayMode_ForSetting.ToString());

                                localReport.SetParameters(new ReportParameter[] { var_FlowNum_ForPrint });
                                localReport.SetParameters(new ReportParameter[] { var_wbSerialNum_ForPrint });
                                localReport.SetParameters(new ReportParameter[] { var_InStoreDate_ForSetting });
                                localReport.SetParameters(new ReportParameter[] { var_PickGoodsDate_ForSetting });
                                localReport.SetParameters(new ReportParameter[] { var_EverUnReleaseNum_ForSetting });
                                localReport.SetParameters(new ReportParameter[] { var_IntervalDays_ForSetting });
                                localReport.SetParameters(new ReportParameter[] { var_EverUnReleaseWeight_ForSetting });
                                localReport.SetParameters(new ReportParameter[] { var_EverUnReleaseFee_ForSetting });
                                localReport.SetParameters(new ReportParameter[] { var_ddlPayMode_ForSetting });

                                localReport.DataSources.Add(dtCurrentReleaseSubWayBill);
                                localReport.DataSources.Add(dtStillUnReleaseSubWayBill);
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

                                string strOutputFileName = "扣货提货单_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";

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
                strResult = "{\"result\":\"error\",\"message\":\"发送邮件失败，原因:"+ex.Message+"\"}";
            }
            return strResult;
        }

        [HttpPost]
        public string SaveSaleInfo(string strSwbSerialNums, string FlowNum_ForPrint, string hid_CustomCategory_ForSetting, string wbID_ForPrint, string InStoreDate_ForSetting, string PickGoodsDate_ForSetting, string wbActualWeight_ForPrint, string OperateFee_ForSetting, string PickGoodsFee_ForSetting, string KeepGoodsFee_ForSetting, string ShiftGoodsFee_ForSetting, string CollectionFee_ForSetting, string ddlPayMode_ForSetting, string ShouldPayUnit_ForSetting, string shouldPay_ForSetting, string TotalFee_ForSetting, string ddlReceiptMode_ForSetting, string Receipt_ForSetting)
        {
            string strRet = "{\"result\":\"error\",\"message\":\"销售报表保存失败,不允许继续打印或导出,原因未知\"}";
            M_WayBillDailyReport m_WayBillDailyReport = null;

            string CurrentReleaseSubWayBill = Server.UrlDecode(strSwbSerialNums).Replace("，", ",");
            string[] arrCurrentReleaseSubWayBill = CurrentReleaseSubWayBill.Split(',');
            StringBuilder sbCurrentReleaseSubWayBill = new StringBuilder("");
            for (int i = 0; i < arrCurrentReleaseSubWayBill.Length; i++)
            {
                if (arrCurrentReleaseSubWayBill[i].Trim() != "")
                {
                    sbCurrentReleaseSubWayBill.AppendFormat("'{0}',", arrCurrentReleaseSubWayBill[i].Trim());
                }
            }

            if (sbCurrentReleaseSubWayBill.ToString().EndsWith(","))
            {
                sbCurrentReleaseSubWayBill = new StringBuilder(sbCurrentReleaseSubWayBill.ToString().Substring(0, sbCurrentReleaseSubWayBill.ToString().Length - 1));
            }

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


                if (sbCurrentReleaseSubWayBill.ToString() != "")
                {
                    SqlServerHelper.ExecuteSql(string.Format(" update  SubWaybill set swbNeedCheck=2 where swbID in (select swbID from V_WayBill_SubWayBill where swbSerialNum in ({0}))", sbCurrentReleaseSubWayBill.ToString()));
                }


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
            STR_SENDER_USERPWD = Util.CryptographyTool.Decrypt(new T_EmailManagement().GetEmailContent(EmailType.EmailSenderPwd),"HuayuTAT");
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
                        if (dsUser.Tables[0].Rows[0]["iSendUnReleaseGoodsEmail"].ToString() == "1")
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

                                    strSubJect = new T_EmailManagement().GetEmailContent(EmailType.EmailSubject_UnReleaseGoods).Replace("[Date]", DateTime.Now.ToString("yyyyMMddHHmmss")).Replace("【Date]", DateTime.Now.ToString("yyyyMMddHHmmss")).Replace("[Date】", DateTime.Now.ToString("yyyyMMddHHmmss")).Replace("【Date】", DateTime.Now.ToString("yyyyMMddHHmmss")); ;
                                    strBody = new T_EmailManagement().GetEmailContent(EmailType.EmailBody_UnReleaseGoods);

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
