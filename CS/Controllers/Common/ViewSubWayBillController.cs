using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model;
using SQLDAL;
using System.Data.SqlClient;
using System.Data;
using System.Text;
using DBUtility;
using Microsoft.Reporting.WebForms;
using System.IO;
using CS.Filter;

namespace CS.Controllers.Common
{
    [ErrorAttribute]
    public class ViewSubWayBillController : Controller
    {
        Model.M_WayBill wayBillModel = new M_WayBill();
        Model.M_SubWayBill subWayBillModel = new M_SubWayBill();
        SQLDAL.T_WayBill wayBillSql = new T_WayBill();
        SQLDAL.T_SubWayBill subWayBillSql = new T_SubWayBill();
        public const string strFileds = "wbSerialNum,swbSerialNum,swbNeedCheck,swbDescription_CHN,swbDescription_ENG,swbNumber,swbWeight,swbValue,swbMonetary,swbRecipients,swbCustomsCategory,TaxNo,TaxRate,TaxRateDescription,ActualTaxRate,CategoryNo,Sender,ReceiverIDCard,ReceiverPhone,EmailAddress,PickGoodsAgain,mismatchCargoName,belowFullPrice,above1000,parentID,ID,state,wbID,swbID";
        public const string strFileds_Main = "wbSerialNum,swbSerialNum,Sender,ReceiverIDCard,ReceiverPhone,EmailAddress,swbRecipients,swbCustomsCategory,swbCustomsCategory_Desc,wbStorageDate,wbCompany,PickGoodsAgain,PickGoodsAgain_Desc,wbID,swbID";
        public const string strFileds_Sub = "swbDescription_CHN,swbDescription_ENG,swbNumber,swbWeight,swbValue,swbMonetary,TaxNo,TaxRate,TaxRateDescription,mismatchCargoName,belowFullPrice,above1000,FinalCheckResultDescription,FinalHandleSuggestDescription,CheckResultOperator,swbID,swbdID";

        public const string STR_TEMPLATE_EXCEL = "~/Temp/Template/template.xls";
        public const string STR_REPORT_URL = "~/Content/Reports/ViewSubWayBill.rdlc";
        //
        // GET: /Customer_Detail/

        public ActionResult Index()
        {
            ViewData["Detail_wbSerialNum"] = Request.QueryString["Detail_wbSerialNum"] == null ? "" : Request.QueryString["Detail_wbSerialNum"];
            ViewData["Detail_swbSerialNum"] = Request.QueryString["Detail_swbSerialNum"] == null ? "" : Request.QueryString["Detail_swbSerialNum"];
            ViewData["Detail_swbStatus"] = Request.QueryString["Detail_swbStatus"] == null ? "" : Request.QueryString["Detail_swbStatus"];

            ViewData["Detail_bEnableReject"] = Request.QueryString["Detail_bEnableReject"] == null ? "" : Request.QueryString["Detail_bEnableReject"];
            ViewData["Detail_bEnableUnRelease"] = Request.QueryString["Detail_bEnableUnRelease"] == null ? "" : Request.QueryString["Detail_bEnableUnRelease"];
            ViewData["Detail_bEnableRelease"] = Request.QueryString["Detail_bEnableRelease"] == null ? "" : Request.QueryString["Detail_bEnableRelease"];

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
        //public string GetData(string order, string page, string rows, string sort, string Detail_wbSerialNum, string Detail_swbSerialNum, string txtswbDescription_CHN, string txtswbDescription_ENG, string txtSwbStatus, string ddlSortingTimes,string id)
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
        //    string wbSerialNum = Server.UrlDecode(Detail_wbSerialNum.ToString());
        //    string swbSerialNum = Server.UrlDecode(Detail_swbSerialNum.ToString());
        //    string swbDescription_CHN = Server.UrlDecode(txtswbDescription_CHN.ToString());
        //    string swbDescription_ENG = Server.UrlDecode(txtswbDescription_ENG.ToString());
        //    string SwbStatus = Server.UrlDecode(txtSwbStatus.ToString());
        //    string strddlSortingTimes = Server.UrlDecode(ddlSortingTimes.ToString());

        //    if (wbSerialNum != "")
        //    {
        //        if (strWhereTemp != "")
        //        {
        //            strWhereTemp = strWhereTemp + " and (wbSerialNum ='" + wbSerialNum + "') ";
        //        }
        //        else
        //        {
        //            strWhereTemp = strWhereTemp + "  (wbSerialNum ='" + wbSerialNum + "') ";
        //        }
        //    }

        //    if (swbSerialNum != "")
        //    {
        //        if (strWhereTemp != "")
        //        {
        //            strWhereTemp = strWhereTemp + " and (swbSerialNum like '%" + swbSerialNum + "%') ";
        //        }
        //        else
        //        {
        //            strWhereTemp = strWhereTemp + "  (swbSerialNum like '%" + swbSerialNum + "%') ";
        //        }
        //    }

        //    if (swbDescription_CHN != "")
        //    {
        //        if (strWhereTemp != "")
        //        {
        //            strWhereTemp = strWhereTemp + " and (swbDescription_CHN like '%" + swbDescription_CHN + "%') ";
        //        }
        //        else
        //        {
        //            strWhereTemp = strWhereTemp + "  (swbDescription_CHN like '%" + swbDescription_CHN + "%') ";
        //        }
        //    }

        //    if (swbDescription_ENG != "")
        //    {
        //        if (strWhereTemp != "")
        //        {
        //            strWhereTemp = strWhereTemp + " and (swbDescription_ENG like '%" + swbDescription_ENG + "%') ";
        //        }
        //        else
        //        {
        //            strWhereTemp = strWhereTemp + "  (swbDescription_ENG like '%" + swbDescription_ENG + "%') ";
        //        }
        //    }

        //    if (SwbStatus != "" && SwbStatus != "-99")
        //    {
        //        if (strWhereTemp != "")
        //        {
        //            strWhereTemp = strWhereTemp + string.Format(" and (swbNeedCheck in ({0})) ", SwbStatus);
        //        }
        //        else
        //        {
        //            strWhereTemp = strWhereTemp + string.Format(" (swbNeedCheck in ({0})) ", SwbStatus);
        //        }
        //    }

        //    if (strddlSortingTimes != "---请选择---")
        //    {
        //        switch (strddlSortingTimes)
        //        {
        //            case "0":
        //                if (strWhereTemp != "")
        //                {
        //                    strWhereTemp = strWhereTemp + " and (swbActualNumber is null or swbActualNumber=0) ";
        //                }
        //                else
        //                {
        //                    strWhereTemp = strWhereTemp + " (swbActualNumber is null or swbActualNumber=0) ";
        //                }
        //                break;
        //            case "1":
        //                if (strWhereTemp != "")
        //                {
        //                    strWhereTemp = strWhereTemp + " and (swbActualNumber is not null and swbActualNumber<>0) ";
        //                }
        //                else
        //                {
        //                    strWhereTemp = strWhereTemp +" (swbActualNumber is not null and swbActualNumber<>0) ";
        //                }
        //                break;
        //            default:
        //                break;
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
        //                case "swbNeedCheck":
        //                    string strSubWayBillStatus = "";
        //                    switch (dt.Rows[i][strFiledArray[j]].ToString())
        //                    {
        //                        case "0":
        //                            strSubWayBillStatus = "放行";
        //                            break;
        //                        case "1":
        //                            strSubWayBillStatus = "等待预检";
        //                            break;
        //                        case "2":
        //                            strSubWayBillStatus = "查验放行";
        //                            break;
        //                        case "3":
        //                            strSubWayBillStatus = "查验扣留";
        //                            break;
        //                        case "4":
        //                            strSubWayBillStatus = "查验待处理";
        //                            break;
        //                        default:
        //                            strSubWayBillStatus = "未知";
        //                            break;
        //                    }
        //                    if (j != strFiledArray.Length - 1)
        //                    {
        //                        sb.AppendFormat("\"{0}\":\"{1}\",", strFiledArray[j], strSubWayBillStatus);
        //                    }
        //                    else
        //                    {
        //                        sb.AppendFormat("\"{0}\":\"{1}\"", strFiledArray[j], strSubWayBillStatus);
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


        public string GetData(string order, string page, string rows, string sort, string Detail_wbSerialNum, string Detail_swbSerialNum, string txtswbDescription_CHN, string txtswbDescription_ENG, string txtSwbStatus, string ddlSortingTimes, string id)
        {
            string strRet = "";
            if (string.IsNullOrEmpty(id))
            {
                strRet = GetData_Sub_Main(order, page, rows, sort, Detail_wbSerialNum, Detail_swbSerialNum, txtswbDescription_CHN, txtswbDescription_ENG, txtSwbStatus, ddlSortingTimes);
            }
            else
            {
                strRet = GetData_Sub_Detail(order, page, rows, sort, txtswbDescription_CHN, txtswbDescription_ENG, id);
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
        public string GetData_Sub_Main(string order, string page, string rows, string sort, string Detail_wbSerialNum, string Detail_swbSerialNum, string txtswbDescription_CHN, string txtswbDescription_ENG, string txtSwbStatus, string ddlSortingTimes)
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
            string wbSerialNum = Server.UrlDecode(Detail_wbSerialNum.ToString());
            string swbSerialNum = Server.UrlDecode(Detail_swbSerialNum.ToString());
            string swbDescription_CHN = Server.UrlDecode(txtswbDescription_CHN.ToString());
            string swbDescription_ENG = Server.UrlDecode(txtswbDescription_ENG.ToString());
            string SwbStatus = Server.UrlDecode(txtSwbStatus.ToString());
            string strddlSortingTimes = Server.UrlDecode(ddlSortingTimes.ToString());

            if (wbSerialNum != "")
            {
                if (strWhereTemp != "")
                {
                    strWhereTemp = strWhereTemp + " and (wbSerialNum ='" + wbSerialNum + "') ";
                }
                else
                {
                    strWhereTemp = strWhereTemp + "  (wbSerialNum ='" + wbSerialNum + "') ";
                }
            }

            if (swbSerialNum != "")
            {
                if (strWhereTemp != "")
                {
                    strWhereTemp = strWhereTemp + " and (swbSerialNum like '%" + swbSerialNum + "%') ";
                }
                else
                {
                    strWhereTemp = strWhereTemp + "  (swbSerialNum like '%" + swbSerialNum + "%') ";
                }
            }

            //if (swbDescription_CHN != "")
            //{
            //    if (strWhereTemp != "")
            //    {
            //        strWhereTemp = strWhereTemp + " and (swbDescription_CHN like '%" + swbDescription_CHN + "%') ";
            //    }
            //    else
            //    {
            //        strWhereTemp = strWhereTemp + "  (swbDescription_CHN like '%" + swbDescription_CHN + "%') ";
            //    }
            //}

            //if (swbDescription_ENG != "")
            //{
            //    if (strWhereTemp != "")
            //    {
            //        strWhereTemp = strWhereTemp + " and (swbDescription_ENG like '%" + swbDescription_ENG + "%') ";
            //    }
            //    else
            //    {
            //        strWhereTemp = strWhereTemp + "  (swbDescription_ENG like '%" + swbDescription_ENG + "%') ";
            //    }
            //}

            string strSwbIdsForFilter = new T_SubWayBillDetail().CreateSwbIds(swbDescription_CHN, swbDescription_ENG);
            if (strSwbIdsForFilter != "")
            {
                if (strWhereTemp != "")
                {
                    strWhereTemp = strWhereTemp + " and ( swbID in " + strSwbIdsForFilter + " ) ";
                }
                else
                {
                    strWhereTemp = strWhereTemp + "   ( swbID in " + strSwbIdsForFilter + " ) ";
                }
            }

            if (SwbStatus != "" && SwbStatus != "-99")
            {
                if (strWhereTemp != "")
                {
                    strWhereTemp = strWhereTemp + string.Format(" and (swbNeedCheck in ({0})) ", SwbStatus);
                }
                else
                {
                    strWhereTemp = strWhereTemp + string.Format(" (swbNeedCheck in ({0})) ", SwbStatus);
                }
            }

            if (strddlSortingTimes != "---请选择---")
            {
                switch (strddlSortingTimes)
                {
                    case "0":
                        if (strWhereTemp != "")
                        {
                            strWhereTemp = strWhereTemp + " and (swbActualNumber is null or swbActualNumber=0) ";
                        }
                        else
                        {
                            strWhereTemp = strWhereTemp + " (swbActualNumber is null or swbActualNumber=0) ";
                        }
                        break;
                    case "1":
                        if (strWhereTemp != "")
                        {
                            strWhereTemp = strWhereTemp + " and (swbActualNumber is not null and swbActualNumber<>0) ";
                        }
                        else
                        {
                            strWhereTemp = strWhereTemp + " (swbActualNumber is not null and swbActualNumber<>0) ";
                        }
                        break;
                    default:
                        break;
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
                        case "swbNeedCheck":
                            string strSubWayBillStatus = "";
                            strSubWayBillStatus = Util.CommonHelper.ParseSwbNeedCheck(dt.Rows[i][strFiledArray[j]].ToString());

                            if (j != strFiledArray.Length - 1)
                            {
                                sb.AppendFormat("\"{0}\":\"{1}\",", strFiledArray[j], strSubWayBillStatus);
                            }
                            else
                            {
                                sb.AppendFormat("\"{0}\":\"{1}\"", strFiledArray[j], strSubWayBillStatus);
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
        public string GetData_Sub_Detail(string order, string page, string rows, string sort, string txtswbDescription_CHN, string txtswbDescription_ENG, string id)
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
            string swbDescription_CHN = Server.UrlDecode(txtswbDescription_CHN.ToString());
            string swbDescription_ENG = Server.UrlDecode(txtswbDescription_ENG.ToString());

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

            if (swbDescription_CHN != "")
            {
                if (strWhereTemp != "")
                {
                    strWhereTemp = strWhereTemp + " and (swbDescription_CHN like '%" + swbDescription_CHN + "%') ";
                }
                else
                {
                    strWhereTemp = strWhereTemp + "  (swbDescription_CHN like '%" + swbDescription_CHN + "%') ";
                }
            }

            if (swbDescription_ENG != "")
            {
                if (strWhereTemp != "")
                {
                    strWhereTemp = strWhereTemp + " and (swbDescription_ENG like '%" + swbDescription_ENG + "%') ";
                }
                else
                {
                    strWhereTemp = strWhereTemp + "  (swbDescription_ENG like '%" + swbDescription_ENG + "%') ";
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
        public ActionResult Print(string order, string page, string rows, string sort, string Detail_wbSerialNum, string Detail_swbSerialNum, string txtswbDescription_CHN, string txtswbDescription_ENG, string txtSwbStatus, string ddlSortingTimes)
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
            string wbSerialNum = Server.UrlDecode(Detail_wbSerialNum.ToString());
            string swbSerialNum = Server.UrlDecode(Detail_swbSerialNum.ToString());
            string swbDescription_CHN = Server.UrlDecode(txtswbDescription_CHN.ToString());
            string swbDescription_ENG = Server.UrlDecode(txtswbDescription_ENG.ToString());
            string SwbStatus = Server.UrlDecode(txtSwbStatus.ToString());
            string strddlSortingTimes = Server.UrlDecode(ddlSortingTimes.ToString());

            if (wbSerialNum != "")
            {
                if (strWhereTemp != "")
                {
                    strWhereTemp = strWhereTemp + " and (wbSerialNum ='" + wbSerialNum + "') ";
                }
                else
                {
                    strWhereTemp = strWhereTemp + "  (wbSerialNum ='" + wbSerialNum + "') ";
                }
            }

            if (swbSerialNum != "")
            {
                if (strWhereTemp != "")
                {
                    strWhereTemp = strWhereTemp + " and (swbSerialNum like '%" + swbSerialNum + "%') ";
                }
                else
                {
                    strWhereTemp = strWhereTemp + "  (swbSerialNum like '%" + swbSerialNum + "%') ";
                }
            }

            string strSwbIdsForFilter = new T_SubWayBillDetail().CreateSwbIds(swbDescription_CHN, swbDescription_ENG);
            if (strSwbIdsForFilter != "")
            {
                if (strWhereTemp != "")
                {
                    strWhereTemp = strWhereTemp + " and ( swbID in " + strSwbIdsForFilter + " ) ";
                }
                else
                {
                    strWhereTemp = strWhereTemp + "   ( swbID in " + strSwbIdsForFilter + " ) ";
                }
            }

            if (SwbStatus != "" && SwbStatus != "-99")
            {
                if (strWhereTemp != "")
                {
                    strWhereTemp = strWhereTemp + string.Format(" and (swbNeedCheck in ({0})) ", SwbStatus);
                }
                else
                {
                    strWhereTemp = strWhereTemp + string.Format(" (swbNeedCheck in ({0})) ", SwbStatus);
                }
            }

            if (strddlSortingTimes != "---请选择---")
            {
                switch (strddlSortingTimes)
                {
                    case "0":
                        if (strWhereTemp != "")
                        {
                            strWhereTemp = strWhereTemp + " and (swbActualNumber is null or swbActualNumber=0) ";
                        }
                        else
                        {
                            strWhereTemp = strWhereTemp + " (swbActualNumber is null or swbActualNumber=0) ";
                        }
                        break;
                    case "1":
                        if (strWhereTemp != "")
                        {
                            strWhereTemp = strWhereTemp + " and (swbActualNumber is not null and swbActualNumber<>0) ";
                        }
                        else
                        {
                            strWhereTemp = strWhereTemp + " (swbActualNumber is not null and swbActualNumber<>0) ";
                        }
                        break;
                    default:
                        break;
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
            dtCustom.Columns.Add("wbSerialNum", Type.GetType("System.String"));
            dtCustom.Columns.Add("swbSerialNum", Type.GetType("System.String"));
            dtCustom.Columns.Add("Sender", Type.GetType("System.String"));
            dtCustom.Columns.Add("ReceiverIDCard", Type.GetType("System.String"));
            dtCustom.Columns.Add("ReceiverPhone", Type.GetType("System.String"));
            dtCustom.Columns.Add("EmailAddress", Type.GetType("System.String"));
            dtCustom.Columns.Add("swbRecipients", Type.GetType("System.String"));
            dtCustom.Columns.Add("swbCustomsCategory", Type.GetType("System.String"));
            dtCustom.Columns.Add("swbCustomsCategory_Desc", Type.GetType("System.String"));
            dtCustom.Columns.Add("wbStorageDate", Type.GetType("System.String"));
            dtCustom.Columns.Add("wbCompany", Type.GetType("System.String"));
            dtCustom.Columns.Add("PickGoodsAgain", Type.GetType("System.String"));
            dtCustom.Columns.Add("PickGoodsAgain_Desc", Type.GetType("System.String"));
            dtCustom.Columns.Add("wbID", Type.GetType("System.String"));
            dtCustom.Columns.Add("swbID", Type.GetType("System.String"));

            DataRow drCustom = null;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                drCustom = dtCustom.NewRow();
                string[] strFiledArray = strFileds_Main.Split(',');
                for (int j = 0; j < strFiledArray.Length; j++)
                {
                    switch (strFiledArray[j])
                    {
                        case "swbCustomsCategory_Desc":
                            string str_swbCustomsCategory_Desc = "未知";
                            switch (dt.Rows[i]["swbCustomsCategory"].ToString())
                            {
                                case "2":
                                    str_swbCustomsCategory_Desc = "样品";
                                    break;
                                case "3":
                                    str_swbCustomsCategory_Desc = "KJ-3";
                                    break;
                                case "4":
                                    str_swbCustomsCategory_Desc = "D类";
                                    break;
                                case "5":
                                    str_swbCustomsCategory_Desc = "个人物品";
                                    break;
                                case "6":
                                    str_swbCustomsCategory_Desc = "分运行李";
                                    break;
                                default:
                                    break;
                            }
                            drCustom[strFiledArray[j]] = str_swbCustomsCategory_Desc;
                            break;
                        case "PickGoodsAgain_Desc":
                            string str_PickGoodsAgain_Desc = "未知";
                            switch (dt.Rows[i]["PickGoodsAgain"].ToString())
                            {
                                case "1":
                                    str_PickGoodsAgain_Desc = "15日内重复提货";
                                    break;
                                case "0":
                                    str_PickGoodsAgain_Desc = "15日内未重复提货";
                                    break;
                                default:
                                    break;
                            }
                            drCustom[strFiledArray[j]] = str_PickGoodsAgain_Desc;
                            break;
                        default:
                            drCustom[strFiledArray[j]] = dt.Rows[i][strFiledArray[j]] == DBNull.Value ? "" : (dt.Rows[i][strFiledArray[j]].ToString().Replace("\r\n", ""));
                            break;
                    }
                }

                if (drCustom["swbID"].ToString() != "")
                {
                    dtCustom.Rows.Add(drCustom);
                }
            }
            dt = null;

            LocalReport localReport = new LocalReport();
            localReport.ReportPath = Server.MapPath(STR_REPORT_URL);
            ReportDataSource reportDataSource = new ReportDataSource("ViewSubWayBill_DS", dtCustom);

            string wbSerialNum_rpt = "";
            string wbCompany_rpt = "";
            string wbStoragedate_rpt = "";
            //获取总运单信息
            DataTable dtWayBill = null;
            DataSet dsWayBill = null;
            dsWayBill=new T_WayBill().GetWayBill(Detail_wbSerialNum);
            if (dsWayBill!=null)
            {
                dtWayBill = dsWayBill.Tables[0];
                if (dtWayBill!=null && dtWayBill.Rows.Count>0)
                {
                    wbSerialNum_rpt=dtWayBill.Rows[0]["wbSerialNum"].ToString();
                    wbCompany_rpt = dtWayBill.Rows[0]["wbCompany"].ToString();
                    wbStoragedate_rpt = dtWayBill.Rows[0]["wbStorageDate"].ToString();
                }
            }
            ReportParameter var_wbSerialNum_rpt = new ReportParameter("wbSerialNum_rpt", wbSerialNum_rpt.ToString());
            ReportParameter var_wbCompany_rpt = new ReportParameter("wbCompany_rpt", wbCompany_rpt.ToString());
            ReportParameter var_wbStoragedate_rpt = new ReportParameter("wbStoragedate_rpt", wbStoragedate_rpt.ToString());

            localReport.SetParameters(new ReportParameter[] { var_wbSerialNum_rpt });
            localReport.SetParameters(new ReportParameter[] { var_wbCompany_rpt });
            localReport.SetParameters(new ReportParameter[] { var_wbStoragedate_rpt });
           
            localReport.SubreportProcessing += new SubreportProcessingEventHandler(LocalReport_SubreportProcessing);

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
        public ActionResult Excel(string order, string page, string rows, string sort, string Detail_wbSerialNum, string Detail_swbSerialNum, string txtswbDescription_CHN, string txtswbDescription_ENG, string txtSwbStatus, string ddlSortingTimes, string browserType)
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
            string wbSerialNum = Server.UrlDecode(Detail_wbSerialNum.ToString());
            string swbSerialNum = Server.UrlDecode(Detail_swbSerialNum.ToString());
            string swbDescription_CHN = Server.UrlDecode(txtswbDescription_CHN.ToString());
            string swbDescription_ENG = Server.UrlDecode(txtswbDescription_ENG.ToString());
            string SwbStatus = Server.UrlDecode(txtSwbStatus.ToString());
            string strddlSortingTimes = Server.UrlDecode(ddlSortingTimes.ToString());
            if (wbSerialNum != "")
            {
                if (strWhereTemp != "")
                {
                    strWhereTemp = strWhereTemp + " and (wbSerialNum ='" + wbSerialNum + "') ";
                }
                else
                {
                    strWhereTemp = strWhereTemp + "  (wbSerialNum ='" + wbSerialNum + "') ";
                }
            }

            if (swbSerialNum != "")
            {
                if (strWhereTemp != "")
                {
                    strWhereTemp = strWhereTemp + " and (swbSerialNum  like '%" + swbSerialNum + "%') ";
                }
                else
                {
                    strWhereTemp = strWhereTemp + "  (swbSerialNum like '%" + swbSerialNum + "%') ";
                }
            }

            if (swbDescription_CHN != "")
            {
                if (strWhereTemp != "")
                {
                    strWhereTemp = strWhereTemp + " and (swbDescription_CHN like '%" + swbDescription_CHN + "%') ";
                }
                else
                {
                    strWhereTemp = strWhereTemp + "  (swbDescription_CHN like '%" + swbDescription_CHN + "%') ";
                }
            }

            if (swbDescription_ENG != "")
            {
                if (strWhereTemp != "")
                {
                    strWhereTemp = strWhereTemp + " and (swbDescription_ENG like '%" + swbDescription_ENG + "%') ";
                }
                else
                {
                    strWhereTemp = strWhereTemp + "  (swbDescription_ENG like '%" + swbDescription_ENG + "%') ";
                }
            }

            if (SwbStatus != "" && SwbStatus != "-99")
            {
                if (strWhereTemp != "")
                {
                    strWhereTemp = strWhereTemp + string.Format(" and (swbNeedCheck in ({0})) ", SwbStatus);
                }
                else
                {
                    strWhereTemp = strWhereTemp + string.Format(" (swbNeedCheck in ({0})) ", SwbStatus);
                }
            }

            if (strddlSortingTimes != "---请选择---")
            {
                switch (strddlSortingTimes)
                {
                    case "0":
                        if (strWhereTemp != "")
                        {
                            strWhereTemp = strWhereTemp + " and (swbActualNumber is null or swbActualNumber=0) ";
                        }
                        else
                        {
                            strWhereTemp = strWhereTemp + " (swbActualNumber is null or swbActualNumber=0) ";
                        }
                        break;
                    case "1":
                        if (strWhereTemp != "")
                        {
                            strWhereTemp = strWhereTemp + " and (swbActualNumber is not null and swbActualNumber<>0) ";
                        }
                        else
                        {
                            strWhereTemp = strWhereTemp + " (swbActualNumber is not null and swbActualNumber<>0) ";
                        }
                        break;
                    default:
                        break;
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
            dtCustom.Columns.Add("swbNeedCheck", Type.GetType("System.String"));
            dtCustom.Columns.Add("wbStorageDate", Type.GetType("System.String"));
            dtCustom.Columns.Add("wbCompany", Type.GetType("System.String"));
            dtCustom.Columns.Add("wbSerialNum", Type.GetType("System.String"));
            dtCustom.Columns.Add("wbStatus", Type.GetType("System.String"));
            dtCustom.Columns.Add("swbDescription_CHN", Type.GetType("System.String"));
            dtCustom.Columns.Add("swbDescription_ENG", Type.GetType("System.String"));
            dtCustom.Columns.Add("swbNumber", Type.GetType("System.String"));
            dtCustom.Columns.Add("swbWeight", Type.GetType("System.String"));
            dtCustom.Columns.Add("swbSerialNum", Type.GetType("System.String"));
            dtCustom.Columns.Add("swbActualNumber", Type.GetType("System.String"));
            dtCustom.Columns.Add("wbID", Type.GetType("System.String"));
            dtCustom.Columns.Add("swbID", Type.GetType("System.String"));

            DataRow drCustom = null;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                drCustom = dtCustom.NewRow();

                string[] strFiledArray = strFileds.Split(',');
                for (int j = 0; j < strFiledArray.Length; j++)
                {
                    switch (strFiledArray[j])
                    {
                        case "swbNeedCheck":
                            string strSubWayBillStatus = "";
                            strSubWayBillStatus = Util.CommonHelper.ParseSwbNeedCheck(dt.Rows[i][strFiledArray[j]].ToString());

                            drCustom[strFiledArray[j]] = strSubWayBillStatus;
                            break;
                        default:
                            drCustom[strFiledArray[j]] = dt.Rows[i][strFiledArray[j]] == DBNull.Value ? "" : (dt.Rows[i][strFiledArray[j]].ToString().Replace("\r\n", ""));
                            break;
                    }

                }
                if (drCustom["swbID"].ToString() != "")
                {
                    dtCustom.Rows.Add(drCustom);
                }
            }
            dt = null;
            LocalReport localReport = new LocalReport();
            localReport.ReportPath = Server.MapPath(STR_REPORT_URL);
            ReportDataSource reportDataSource = new ReportDataSource("ViewSubWayBillDetail_DS", dtCustom);

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

            string strOutputFileName = "子运单信息_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";

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

            e.DataSources.Add(new Microsoft.Reporting.WebForms.ReportDataSource("ViewSubWayBill_Detail_DS", dtSubWayBillDetail_Sub));
        }

        //[HttpGet]
        //public ActionResult Print(string order, string page, string rows, string sort, string Detail_wbSerialNum, string Detail_swbSerialNum, string txtswbDescription_CHN, string txtswbDescription_ENG, string txtSwbStatus, string ddlSortingTimes)
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
        //    string wbSerialNum = Server.UrlDecode(Detail_wbSerialNum.ToString());
        //    string swbSerialNum = Server.UrlDecode(Detail_swbSerialNum.ToString());
        //    string swbDescription_CHN = Server.UrlDecode(txtswbDescription_CHN.ToString());
        //    string swbDescription_ENG = Server.UrlDecode(txtswbDescription_ENG.ToString());
        //    string SwbStatus = Server.UrlDecode(txtSwbStatus.ToString());
        //    string strddlSortingTimes = Server.UrlDecode(ddlSortingTimes.ToString());
        //    if (wbSerialNum != "")
        //    {
        //        if (strWhereTemp != "")
        //        {
        //            strWhereTemp = strWhereTemp + " and (wbSerialNum ='" + wbSerialNum + "') ";
        //        }
        //        else
        //        {
        //            strWhereTemp = strWhereTemp + "  (wbSerialNum ='" + wbSerialNum + "') ";
        //        }
        //    }

        //    if (swbSerialNum != "")
        //    {
        //        if (strWhereTemp != "")
        //        {
        //            strWhereTemp = strWhereTemp + " and (swbSerialNum like '%" + swbSerialNum + "%') ";
        //        }
        //        else
        //        {
        //            strWhereTemp = strWhereTemp + "  (swbSerialNum  like '%" + swbSerialNum + "%') ";
        //        }
        //    }

        //    if (swbDescription_CHN != "")
        //    {
        //        if (strWhereTemp != "")
        //        {
        //            strWhereTemp = strWhereTemp + " and (swbDescription_CHN like '%" + swbDescription_CHN + "%') ";
        //        }
        //        else
        //        {
        //            strWhereTemp = strWhereTemp + "  (swbDescription_CHN like '%" + swbDescription_CHN + "%') ";
        //        }
        //    }

        //    if (swbDescription_ENG != "")
        //    {
        //        if (strWhereTemp != "")
        //        {
        //            strWhereTemp = strWhereTemp + " and (swbDescription_ENG like '%" + swbDescription_ENG + "%') ";
        //        }
        //        else
        //        {
        //            strWhereTemp = strWhereTemp + "  (swbDescription_ENG like '%" + swbDescription_ENG + "%') ";
        //        }
        //    }

        //    if (SwbStatus != "" && SwbStatus != "-99")
        //    {
        //        if (strWhereTemp != "")
        //        {
        //            strWhereTemp = strWhereTemp + string.Format(" and (swbNeedCheck in ({0})) ", SwbStatus);
        //        }
        //        else
        //        {
        //            strWhereTemp = strWhereTemp + string.Format(" (swbNeedCheck in ({0})) ", SwbStatus);
        //        }
        //    }

        //    if (strddlSortingTimes != "---请选择---")
        //    {
        //        switch (strddlSortingTimes)
        //        {
        //            case "0":
        //                if (strWhereTemp != "")
        //                {
        //                    strWhereTemp = strWhereTemp + " and (swbActualNumber is null or swbActualNumber=0) ";
        //                }
        //                else
        //                {
        //                    strWhereTemp = strWhereTemp + " (swbActualNumber is null or swbActualNumber=0) ";
        //                }
        //                break;
        //            case "1":
        //                if (strWhereTemp != "")
        //                {
        //                    strWhereTemp = strWhereTemp + " and (swbActualNumber is not null and swbActualNumber<>0) ";
        //                }
        //                else
        //                {
        //                    strWhereTemp = strWhereTemp + " (swbActualNumber is not null and swbActualNumber<>0) ";
        //                }
        //                break;
        //            default:
        //                break;
        //        }
        //    }

        //    param[6].Value = strWhereTemp;

        //    param[7] = new SqlParameter();
        //    param[7].SqlDbType = SqlDbType.Int;
        //    param[7].ParameterName = "@RecordCount";
        //    param[7].Direction = ParameterDirection.Output;

        //    DataSet ds = SqlServerHelper.RunProcedure("spPageViewByStr", param, "result");
        //    DataTable dt = ds.Tables["result"];

        //    DataTable dtCustom = new DataTable();
        //    dtCustom.Columns.Add("swbNeedCheck", Type.GetType("System.String"));
        //    dtCustom.Columns.Add("wbStorageDate", Type.GetType("System.String"));
        //    dtCustom.Columns.Add("wbCompany", Type.GetType("System.String"));
        //    dtCustom.Columns.Add("wbSerialNum", Type.GetType("System.String"));
        //    dtCustom.Columns.Add("wbStatus", Type.GetType("System.String"));
        //    dtCustom.Columns.Add("swbDescription_CHN", Type.GetType("System.String"));
        //    dtCustom.Columns.Add("swbDescription_ENG", Type.GetType("System.String"));
        //    dtCustom.Columns.Add("swbNumber", Type.GetType("System.String"));
        //    dtCustom.Columns.Add("swbWeight", Type.GetType("System.String"));
        //    dtCustom.Columns.Add("swbSerialNum", Type.GetType("System.String"));
        //    dtCustom.Columns.Add("swbActualNumber", Type.GetType("System.String"));
        //    dtCustom.Columns.Add("wbID", Type.GetType("System.String"));
        //    dtCustom.Columns.Add("swbID", Type.GetType("System.String"));

        //    DataRow drCustom = null;
        //    for (int i = 0; i < dt.Rows.Count; i++)
        //    {
        //        drCustom = dtCustom.NewRow();

        //        string[] strFiledArray = strFileds.Split(',');
        //        for (int j = 0; j < strFiledArray.Length; j++)
        //        {
        //            switch (strFiledArray[j])
        //            {
        //                case "swbNeedCheck":
        //                    string strSubWayBillStatus = "";
        //                    switch (dt.Rows[i][strFiledArray[j]].ToString())
        //                    {
        //                        case "0":
        //                            strSubWayBillStatus = "放行";
        //                            break;
        //                        case "1":
        //                            strSubWayBillStatus = "等待预检";
        //                            break;
        //                        case "2":
        //                            strSubWayBillStatus = "查验放行";
        //                            break;
        //                        case "3":
        //                            strSubWayBillStatus = "查验扣留";
        //                            break;
        //                        case "4":
        //                            strSubWayBillStatus = "查验待处理";
        //                            break;
        //                        default:
        //                            strSubWayBillStatus = "未知";
        //                            break;
        //                    }
        //                    drCustom[strFiledArray[j]] = strSubWayBillStatus;
        //                    break;
        //                default:
        //                    drCustom[strFiledArray[j]] = dt.Rows[i][strFiledArray[j]] == DBNull.Value ? "" : (dt.Rows[i][strFiledArray[j]].ToString().Replace("\r\n", ""));
        //                    break;
        //            }

        //        }
        //        if (drCustom["swbID"].ToString() != "")
        //        {
        //            dtCustom.Rows.Add(drCustom);
        //        }
        //    }
        //    dt = null;
        //    LocalReport localReport = new LocalReport();
        //    localReport.ReportPath = Server.MapPath(STR_REPORT_URL);
        //    ReportDataSource reportDataSource = new ReportDataSource("ViewSubWayBillDetail_DS", dtCustom);

        //    localReport.DataSources.Add(reportDataSource);
        //    string reportType = "PDF";
        //    string mimeType;
        //    string encoding = "UTF-8";
        //    string fileNameExtension;

        //    string deviceInfo = "<DeviceInfo>" +
        //        " <OutputFormat>PDF</OutputFormat>" +
        //        " <PageWidth>12in</PageWidth>" +
        //        " <PageHeigth>11in</PageHeigth>" +
        //        " <MarginTop>0.5in</MarginTop>" +
        //        " <MarginLeft>1in</MarginLeft>" +
        //        " <MarginRight>1in</MarginRight>" +
        //        " <MarginBottom>0.5in</MarginBottom>" +
        //        " </DeviceInfo>";

        //    Warning[] warnings;
        //    string[] streams;
        //    byte[] renderedBytes;

        //    renderedBytes = localReport.Render(reportType, deviceInfo, out mimeType, out encoding, out fileNameExtension, out streams, out warnings);

        //    return File(renderedBytes, mimeType);
        //}

        //[HttpGet]
        //public ActionResult Excel(string order, string page, string rows, string sort, string Detail_wbSerialNum, string Detail_swbSerialNum, string txtswbDescription_CHN, string txtswbDescription_ENG, string txtSwbStatus, string ddlSortingTimes, string browserType)
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
        //    string wbSerialNum = Server.UrlDecode(Detail_wbSerialNum.ToString());
        //    string swbSerialNum = Server.UrlDecode(Detail_swbSerialNum.ToString());
        //    string swbDescription_CHN = Server.UrlDecode(txtswbDescription_CHN.ToString());
        //    string swbDescription_ENG = Server.UrlDecode(txtswbDescription_ENG.ToString());
        //    string SwbStatus = Server.UrlDecode(txtSwbStatus.ToString());
        //    string strddlSortingTimes = Server.UrlDecode(ddlSortingTimes.ToString());
        //    if (wbSerialNum != "")
        //    {
        //        if (strWhereTemp != "")
        //        {
        //            strWhereTemp = strWhereTemp + " and (wbSerialNum ='" + wbSerialNum + "') ";
        //        }
        //        else
        //        {
        //            strWhereTemp = strWhereTemp + "  (wbSerialNum ='" + wbSerialNum + "') ";
        //        }
        //    }

        //    if (swbSerialNum != "")
        //    {
        //        if (strWhereTemp != "")
        //        {
        //            strWhereTemp = strWhereTemp + " and (swbSerialNum  like '%" + swbSerialNum + "%') ";
        //        }
        //        else
        //        {
        //            strWhereTemp = strWhereTemp + "  (swbSerialNum like '%" + swbSerialNum + "%') ";
        //        }
        //    }

        //    if (swbDescription_CHN != "")
        //    {
        //        if (strWhereTemp != "")
        //        {
        //            strWhereTemp = strWhereTemp + " and (swbDescription_CHN like '%" + swbDescription_CHN + "%') ";
        //        }
        //        else
        //        {
        //            strWhereTemp = strWhereTemp + "  (swbDescription_CHN like '%" + swbDescription_CHN + "%') ";
        //        }
        //    }

        //    if (swbDescription_ENG != "")
        //    {
        //        if (strWhereTemp != "")
        //        {
        //            strWhereTemp = strWhereTemp + " and (swbDescription_ENG like '%" + swbDescription_ENG + "%') ";
        //        }
        //        else
        //        {
        //            strWhereTemp = strWhereTemp + "  (swbDescription_ENG like '%" + swbDescription_ENG + "%') ";
        //        }
        //    }

        //    if (SwbStatus != "" && SwbStatus != "-99")
        //    {
        //        if (strWhereTemp != "")
        //        {
        //            strWhereTemp = strWhereTemp + string.Format(" and (swbNeedCheck in ({0})) ", SwbStatus);
        //        }
        //        else
        //        {
        //            strWhereTemp = strWhereTemp + string.Format(" (swbNeedCheck in ({0})) ", SwbStatus);
        //        }
        //    }

        //    if (strddlSortingTimes != "---请选择---")
        //    {
        //        switch (strddlSortingTimes)
        //        {
        //            case "0":
        //                if (strWhereTemp != "")
        //                {
        //                    strWhereTemp = strWhereTemp + " and (swbActualNumber is null or swbActualNumber=0) ";
        //                }
        //                else
        //                {
        //                    strWhereTemp = strWhereTemp + " (swbActualNumber is null or swbActualNumber=0) ";
        //                }
        //                break;
        //            case "1":
        //                if (strWhereTemp != "")
        //                {
        //                    strWhereTemp = strWhereTemp + " and (swbActualNumber is not null and swbActualNumber<>0) ";
        //                }
        //                else
        //                {
        //                    strWhereTemp = strWhereTemp + " (swbActualNumber is not null and swbActualNumber<>0) ";
        //                }
        //                break;
        //            default:
        //                break;
        //        }
        //    }

        //    param[6].Value = strWhereTemp;

        //    param[7] = new SqlParameter();
        //    param[7].SqlDbType = SqlDbType.Int;
        //    param[7].ParameterName = "@RecordCount";
        //    param[7].Direction = ParameterDirection.Output;

        //    DataSet ds = SqlServerHelper.RunProcedure("spPageViewByStr", param, "result");
        //    DataTable dt = ds.Tables["result"];

        //    DataTable dtCustom = new DataTable();
        //    dtCustom.Columns.Add("swbNeedCheck", Type.GetType("System.String"));
        //    dtCustom.Columns.Add("wbStorageDate", Type.GetType("System.String"));
        //    dtCustom.Columns.Add("wbCompany", Type.GetType("System.String"));
        //    dtCustom.Columns.Add("wbSerialNum", Type.GetType("System.String"));
        //    dtCustom.Columns.Add("wbStatus", Type.GetType("System.String"));
        //    dtCustom.Columns.Add("swbDescription_CHN", Type.GetType("System.String"));
        //    dtCustom.Columns.Add("swbDescription_ENG", Type.GetType("System.String"));
        //    dtCustom.Columns.Add("swbNumber", Type.GetType("System.String"));
        //    dtCustom.Columns.Add("swbWeight", Type.GetType("System.String"));
        //    dtCustom.Columns.Add("swbSerialNum", Type.GetType("System.String"));
        //    dtCustom.Columns.Add("swbActualNumber", Type.GetType("System.String"));
        //    dtCustom.Columns.Add("wbID", Type.GetType("System.String"));
        //    dtCustom.Columns.Add("swbID", Type.GetType("System.String"));

        //    DataRow drCustom = null;
        //    for (int i = 0; i < dt.Rows.Count; i++)
        //    {
        //        drCustom = dtCustom.NewRow();

        //        string[] strFiledArray = strFileds.Split(',');
        //        for (int j = 0; j < strFiledArray.Length; j++)
        //        {
        //            switch (strFiledArray[j])
        //            {
        //                case "swbNeedCheck":
        //                    string strSubWayBillStatus = "";
        //                    switch (dt.Rows[i][strFiledArray[j]].ToString())
        //                    {
        //                        case "0":
        //                            strSubWayBillStatus = "放行";
        //                            break;
        //                        case "1":
        //                            strSubWayBillStatus = "等待预检";
        //                            break;
        //                        case "2":
        //                            strSubWayBillStatus = "查验放行";
        //                            break;
        //                        case "3":
        //                            strSubWayBillStatus = "查验扣留";
        //                            break;
        //                        case "4":
        //                            strSubWayBillStatus = "查验待处理";
        //                            break;
        //                        default:
        //                            strSubWayBillStatus = "未知";
        //                            break;
        //                    }
        //                    drCustom[strFiledArray[j]] = strSubWayBillStatus;
        //                    break;
        //                default:
        //                    drCustom[strFiledArray[j]] = dt.Rows[i][strFiledArray[j]] == DBNull.Value ? "" : (dt.Rows[i][strFiledArray[j]].ToString().Replace("\r\n", ""));
        //                    break;
        //            }

        //        }
        //        if (drCustom["swbID"].ToString() != "")
        //        {
        //            dtCustom.Rows.Add(drCustom);
        //        }
        //    }
        //    dt = null;
        //    LocalReport localReport = new LocalReport();
        //    localReport.ReportPath = Server.MapPath(STR_REPORT_URL);
        //    ReportDataSource reportDataSource = new ReportDataSource("ViewSubWayBillDetail_DS", dtCustom);

        //    localReport.DataSources.Add(reportDataSource);

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

        //    string strOutputFileName = "子运单信息_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";

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

        [HttpPost]
        public string PatchUpdateSwbNeedCheck(string strSwbIds, string iNeedCheck)
        {
            string strRet = "{\"result\":\"error\",\"message\":\"更改失败，原因未知\"}";
            strSwbIds = Server.UrlDecode(strSwbIds);
            iNeedCheck = Server.UrlDecode(iNeedCheck);
            try
            {
                if (strSwbIds != "")
                {
                    if ((new T_SubWayBill()).upDateSwbNeedCheck(strSwbIds, iNeedCheck))
                    {
                        strRet = "{\"result\":\"ok\",\"message\":\"" + "更改成功" + "\"}";
                    }
                    else
                    {
                        strRet = "{\"result\":\"error\",\"message\":\"" + "更改失败" + "\"}";
                    }
                }
                else
                {
                    strRet = "{\"result\":\"error\",\"message\":\"" + "请先选择数据" + "\"}";
                }

            }
            catch (Exception ex)
            {
                strRet = "{\"result\":\"error\",\"message\":\"" + ex.Message + "\"}";
            }

            return strRet;
        }

        [HttpPost]
        public string PatchRejectSubWayBill(string strSwbIds)
        {
            string strRet = "{\"result\":\"error\",\"message\":\"退货失败，原因未知\"}";
            strSwbIds = Server.UrlDecode(strSwbIds);
            try
            {
                if (strSwbIds != "")
                {
                    if ((new T_SubWayBill()).RejectSubWayBill(strSwbIds, Session["Global_Huayu_UserName"] == null ? "" : Session["Global_Huayu_UserName"].ToString()))
                    {
                        strRet = "{\"result\":\"ok\",\"message\":\"" + "退货成功" + "\"}";
                    }
                    else
                    {
                        strRet = "{\"result\":\"error\",\"message\":\"" + "退货失败" + "\"}";
                    }
                }
                else
                {
                    strRet = "{\"result\":\"error\",\"message\":\"" + "请先选择数据" + "\"}";
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
