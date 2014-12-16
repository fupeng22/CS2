﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model;
using SQLDAL;
using System.Data.SqlClient;
using System.Data;
using DBUtility;
using System.Text;

namespace CS.Controllers.Forwarder
{
    public class Forwarder_PendingWayBillsDetailController : Controller
    {
        Model.M_WayBill wayBillModel = new M_WayBill();
        Model.M_SubWayBill subWayBillModel = new M_SubWayBill();
        SQLDAL.T_WayBill wayBillSql = new T_WayBill();
        SQLDAL.T_SubWayBill subWayBillSql = new T_SubWayBill();
        public const string strFileds = "wbSerialNum,swbSerialNum,swbDescription_CHN,swbDescription_ENG,swbNumber,swbWeight,swbValue,swbMonetary,swbRecipients,swbCustomsCategory,TaxNo,TaxRate,TaxRateDescription,ActualTaxRate,CategoryNo,Sender,ReceiverIDCard,ReceiverPhone,EmailAddress,PickGoodsAgain,mismatchCargoName,belowFullPrice,above1000,chkNeedCheck,CheckResult,HandleSuggestion,CheckResultDescription,HandleSuggestionDescription,FinalCheckResultDescription,FinalHandleSuggestDescription,CheckResultOperator,parentID,ID,state,wbID,swbdID,swbID";

        //
        // GET: /Customer_Detail/
        public ActionResult Index()
        {
            ViewData["Detail_wbID"] = Request.QueryString["Detail_wbID"];
            ViewData["Detail_wbSerialNum"] = Request.QueryString["Detail_wbSerialNum"];
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
        public string GetData(string order, string page, string rows, string sort, int Detail_wbID, string Detail_swbSerialNum, string id)
        {
            string strRet = "";
            if (string.IsNullOrEmpty(id))
            {
                strRet = GetData_Sub_Main(order, page, rows, sort, Detail_wbID, Detail_swbSerialNum);
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
        public string GetData_Sub_Main(string order, string page, string rows, string sort, int Detail_wbID, string Detail_swbSerialNum)
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

            int wbID = Convert.ToInt32(Server.UrlDecode(Detail_wbID.ToString()));
            string swbSerialNum = Server.UrlDecode(Detail_swbSerialNum.ToString());

            string strWhereTemp = "";
            string strAllPendingSubWayBillIds = "";
            strWhereTemp = strWhereTemp + " ( (swbNeedCheck = 1)) and (wbID=" + wbID.ToString() + ") ";
            strAllPendingSubWayBillIds = new T_SubWayBillDetail().getAllPendingSubWayBillsBywbID(wbID.ToString());
            if (strAllPendingSubWayBillIds != "")
            {
                strWhereTemp = strWhereTemp + " and (swbID in (" + strAllPendingSubWayBillIds + ")) ";
            }
            else
            {
                strWhereTemp = strWhereTemp + " and (1=2) ";
            }
            if (Detail_swbSerialNum.ToString() != "")
            {
                if (strWhereTemp != "")
                {
                    strWhereTemp = strWhereTemp + " and   (swbSerialNum like '%" + swbSerialNum.ToString() + "%') ";
                }
                else
                {
                    strWhereTemp = strWhereTemp + "  (swbSerialNum like '%" + swbSerialNum.ToString() + "%') ";
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

            strWhereTemp = " (CheckResult<>-1 or HandleSuggestion<>-1) ";

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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        [HttpPost]
        public string LoadComboJSONByType(string type)
        {
            string strResult = "";
            StringBuilder sb = new StringBuilder("");
            DataSet ds = new T_BasicSetting().LoadItemsByType(type);
            DataTable dt = new DataTable();
            sb.Append("[");
            sb.Append("{");
            sb.AppendFormat("\"id\":\"{0}\",\"text\":\"{1}\"", "-99", "---请选择---");
            sb.Append("},");
            if (ds != null)
            {
                dt = ds.Tables[0];
                if (dt != null)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        sb.Append("{");
                        sb.AppendFormat("\"id\":\"{0}\",\"text\":\"{1}\"", dt.Rows[i]["ItemId"].ToString(), dt.Rows[i]["ItemValue"].ToString());
                        if (i != dt.Rows.Count - 1)
                        {
                            sb.Append("},");
                        }
                        else
                        {
                            sb.Append("}");
                        }
                    }
                }
            }
            if (sb.ToString().EndsWith(","))
            {
                sb = new StringBuilder(sb.ToString().Remove(sb.ToString().Length - 1));
            }
            sb.Append(",{");
            sb.AppendFormat("\"id\":\"{0}\",\"text\":\"{1}\"", "99", "其他");
            sb.Append("}");
            sb.Append("]");
            strResult = sb.ToString();
            return strResult;
        }

        [HttpPost]
        public string UpdateCheckResult(string swbdID_Local, string CheckResult_Local, string HandleSuggestion_Local, string FinalCheckResultDescription_Local, string FinalHandleSuggestDescription_Local)
        {
            string strRet = "{\"result\":\"error\",\"message\":\"提交失败，原因未知\"}";
            string strUserName = "";
            swbdID_Local = Server.UrlDecode(swbdID_Local.ToString());
            CheckResult_Local = Server.UrlDecode(CheckResult_Local.ToString());
            HandleSuggestion_Local = Server.UrlDecode(HandleSuggestion_Local.ToString());
            FinalCheckResultDescription_Local = Server.UrlDecode(FinalCheckResultDescription_Local.ToString());
            FinalHandleSuggestDescription_Local = Server.UrlDecode(FinalHandleSuggestDescription_Local.ToString());
            strUserName = Session["Global_Customer_UserName"] == null ? "" : Session["Global_Customer_UserName"].ToString();
            try
            {
                M_SubWayBillDetail m_SubWayBillDetail = new M_SubWayBillDetail();
                m_SubWayBillDetail.swbdID = Convert.ToInt32(swbdID_Local);
                m_SubWayBillDetail.CheckResult = Convert.ToInt32(CheckResult_Local);
                m_SubWayBillDetail.HandleSuggestion = Convert.ToInt32(HandleSuggestion_Local);
                m_SubWayBillDetail.CheckResultDescription = FinalCheckResultDescription_Local;
                m_SubWayBillDetail.HandleSuggestionDescription = FinalHandleSuggestDescription_Local;
                m_SubWayBillDetail.CheckResultOperator = strUserName;

                if (new T_SubWayBillDetail().UpdateCheckResultInfo(m_SubWayBillDetail))
                {
                    strRet = "{\"result\":\"ok\",\"message\":\"" + "提交成功" + "\"}";
                }
                else
                {
                    strRet = "{\"result\":\"ok\",\"message\":\"" + "提交失败" + "\"}";
                }

            }
            catch (Exception ex)
            {
                strRet = "{\"result\":\"error\",\"message\":\"" + ex.Message + "\"}";
            }

            return strRet;
        }

        [HttpPost]
        public string UpdateSwbNeedCheck(string swbID, string swbNeedCheck)
        {
            string strRet = "{\"result\":\"error\",\"message\":\"提交失败，原因未知\"}";
            string strUserName = "";
            swbID = Server.UrlDecode(swbID.ToString());
            swbNeedCheck = Server.UrlDecode(swbNeedCheck.ToString());

            strUserName = Session["Global_Customer_UserName"] == null ? "" : Session["Global_Customer_UserName"].ToString();
            try
            {
                //if (new T_SubWayBill().upDateSwbNeedCheck(-1,swbID,2,Convert.ToInt32(swbNeedCheck)))
                if (new T_SubWayBill().upDateSwbNeedCheck(swbID, swbNeedCheck))
                {
                    strRet = "{\"result\":\"ok\",\"message\":\"" + "提交成功" + "\"}";
                }
                else
                {
                    strRet = "{\"result\":\"ok\",\"message\":\"" + "提交失败" + "\"}";
                }

            }
            catch (Exception ex)
            {
                strRet = "{\"result\":\"error\",\"message\":\"" + ex.Message + "\"}";
            }

            return strRet;
        }
    }
}
