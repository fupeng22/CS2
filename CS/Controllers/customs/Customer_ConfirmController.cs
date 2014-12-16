﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.SqlClient;
using System.Data;
using DBUtility;
using System.Text;
using Model;
using SQLDAL;
using CS.Filter;
using Microsoft.Reporting.WebForms;
using System.IO;

namespace CS.Controllers.customs
{
    [ErrorAttribute]
    public class Customer_ConfirmController : Controller
    {
        Model.M_WayBill wayBillModel = new M_WayBill();
        Model.M_SubWayBill subWayBillModel = new M_SubWayBill();
        SQLDAL.T_WayBill wayBillSql = new T_WayBill();
        SQLDAL.T_SubWayBill tSubWayBill = new T_SubWayBill();

        public const string strFileds = "wbStorageDate,wbCompany,wbSerialNum,wbSubNumber_Custom,wbReleaseCount_Custom,wbUnReleaseCount_Custom,wbNotProCount_Custom,wbID";

        public const string STR_TEMPLATE_EXCEL = "~/Temp/Template/template.xls";
        public const string STR_REPORT_URL = "~/Content/Reports/CustomerConfirm.rdlc";
        //
        // GET: /Customer_Confirm/
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
        public string GetData(string order, string page, string rows, string sort, string ddStatus, string inputBeginDate, string inputEndDate, string txtGCode, string txtVoyage)
        {
            string strSQLQuery = "";
            string strTemp = "";
            int iRowsCount = 0;
            int iMaxCount = 0;

            strSQLQuery = "select * from V_Distinct_WayBill {0} ";

            ddStatus = Server.UrlDecode(ddStatus.ToString());
            inputBeginDate = Server.UrlDecode(inputBeginDate.ToString());
            inputEndDate = Server.UrlDecode(inputEndDate.ToString());
            txtGCode = Server.UrlDecode(txtGCode.ToString());
            txtVoyage = Server.UrlDecode(txtVoyage.ToString());

            if (ddStatus == "-1" || ddStatus == "1")//查看已预检
            {
                strTemp = " where ( wbStatus=1 or wbStatus=2 )  and (wbTaxFeeConfirm=1)";
            }
            else if (ddStatus == "0")//查看未预检
            {
                strTemp = " where (  wbStatus=0 ) and (wbTaxFeeConfirm=1)";
            }

            if (inputBeginDate != "" && inputEndDate != "")
            {
                strTemp = strTemp + " and (  wbStorageDate>='" + Convert.ToDateTime(inputBeginDate).ToString("yyyyMMdd") + "' and wbStorageDate<='" + Convert.ToDateTime(inputEndDate).ToString("yyyyMMdd") + "' ) ";
            }

            if (txtGCode != "")
            {
                strTemp = strTemp + " and (  wbSerialNum like '%" + txtGCode + "%') ";
            }

            if (txtVoyage != "" && txtVoyage != "---请选择---")
            {
                strTemp = strTemp + " and (  wbCompany like '%" + txtVoyage + "%') ";
            }

            strTemp = strTemp + " order by " + sort + " " + order;

            DataSet ds = SqlServerHelper.Query(string.Format(strSQLQuery, strTemp));
            DataTable dt = ds.Tables[0];
            Boolean bNeedCheck = false;

            Dictionary<string, int> dic_detainNum = null;
            Dictionary<string, Struct_SumInfo> dic_SumInfo = null;

            DataSet dsSubWayBillInfo = null;
            DataTable dtSubWayBillInfo = null;
            dsSubWayBillInfo = tSubWayBill.GetAllSubWayBillInfo();
            if (dsSubWayBillInfo != null)
            {
                dtSubWayBillInfo = dsSubWayBillInfo.Tables[0];
            }

            //if (dtSubWayBillInfo != null && dtSubWayBillInfo.Rows.Count > 0)
            //{
            //    for (int m = 0; m < dtSubWayBillInfo.Rows.Count; m++)
            //    {
            //        if (dtSubWayBillInfo.Rows[m]["swb_wbID"].ToString() == dt.Rows[i]["wbID"].ToString())
            //        {
            //            if (Convert.ToInt32(dtSubWayBillInfo.Rows[m]["swbNeedCheck"].ToString()) == 3)
            //            {
            //                detainNum = detainNum + 1;
            //                break;
            //            }
            //        }
            //    }
            //}
            dic_detainNum = new Dictionary<string, int>();
            dic_SumInfo = new Dictionary<string, Struct_SumInfo>();
            if (dtSubWayBillInfo != null && dtSubWayBillInfo.Rows.Count > 0)
            {
                for (int m = 0; m < dtSubWayBillInfo.Rows.Count; m++)
                {
                    if (!dic_SumInfo.ContainsKey(dtSubWayBillInfo.Rows[m]["swb_wbID"].ToString()))
                    {
                        dic_SumInfo.Add(dtSubWayBillInfo.Rows[m]["swb_wbID"].ToString(), new Struct_SumInfo()
                        {
                            releseNum = 0,
                            notReleseNum = 0,
                            notProNum = 0,
                            subNum = 0,
                        });
                    }

                    if (Convert.ToInt32(dtSubWayBillInfo.Rows[m]["swbNeedCheck"].ToString()) == 3)
                    {
                        if (dic_detainNum.ContainsKey(dtSubWayBillInfo.Rows[m]["swb_wbID"].ToString()))
                        {
                            dic_detainNum[dtSubWayBillInfo.Rows[m]["swb_wbID"].ToString()] = dic_detainNum[dtSubWayBillInfo.Rows[m]["swb_wbID"].ToString()] + 1;
                        }
                        else
                        {
                            dic_detainNum.Add(dtSubWayBillInfo.Rows[m]["swb_wbID"].ToString(), 1);
                        }
                    }

                    if (((Convert.ToInt32(dtSubWayBillInfo.Rows[m]["swbNeedCheck"]) == 0) && (dtSubWayBillInfo.Rows[m]["swbSortingTime"] != DBNull.Value)) || ((Convert.ToInt32(dtSubWayBillInfo.Rows[m]["swbNeedCheck"]) == 2) && (dtSubWayBillInfo.Rows[m]["swbSortingTime"] != DBNull.Value)))//(swbNeedCheck=0 or swbNeedCheck=2) and swbSortingTime is not null
                    {
                        dic_SumInfo[dtSubWayBillInfo.Rows[m]["swb_wbID"].ToString()].releseNum = dic_SumInfo[dtSubWayBillInfo.Rows[m]["swb_wbID"].ToString()].releseNum + 1;
                    }
                    if (Convert.ToInt32(dtSubWayBillInfo.Rows[m]["swbNeedCheck"]) == 3)//swbNeedCheck=3
                    {
                        dic_SumInfo[dtSubWayBillInfo.Rows[m]["swb_wbID"].ToString()].notReleseNum = dic_SumInfo[dtSubWayBillInfo.Rows[m]["swb_wbID"].ToString()].notReleseNum + 1;
                    }
                    if (dtSubWayBillInfo.Rows[m]["swbSortingTime"] == DBNull.Value)// swbSortingTime is null 
                    {
                        dic_SumInfo[dtSubWayBillInfo.Rows[m]["swb_wbID"].ToString()].notProNum = dic_SumInfo[dtSubWayBillInfo.Rows[m]["swb_wbID"].ToString()].notProNum + 1;
                    }

                    dic_SumInfo[dtSubWayBillInfo.Rows[m]["swb_wbID"].ToString()].subNum = dic_SumInfo[dtSubWayBillInfo.Rows[m]["swb_wbID"].ToString()].subNum + 1;
                }
            }

            DataTable dt_Actual = new DataTable();
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                dt_Actual.Columns.Add(dt.Columns[i].ColumnName, dt.Columns[i].DataType);
            }

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                bNeedCheck = false;
                if ("2" == dt.Rows[i]["wbStatus"].ToString())
                {
                    int detainNum = 0;
                    // swb_wbID=" + wbID + " and  swbNeedCheck=3 "
                    if (dic_detainNum.ContainsKey(dt.Rows[i]["wbID"].ToString()))
                    {
                        detainNum = dic_detainNum[dt.Rows[i]["wbID"].ToString()];
                    }
                    //detainNum= tSubWayBill.GetActualNotReleseNum(int.Parse(dt.Rows[i]["wbID"].ToString()));
                    if (detainNum != 0)
                    {
                        bNeedCheck = true;
                    }
                }
                else
                {
                    bNeedCheck = true;
                }

                if (bNeedCheck)
                {
                    DataRow row = dt_Actual.NewRow();
                    for (int k = 0; k < row.ItemArray.Length; k++)
                    {
                        row[k] = dt.Rows[i][k];
                    }
                    dt_Actual.Rows.Add(row);

                    iRowsCount = iRowsCount + 1;
                }
            }

            if (Convert.ToInt32(page) > iRowsCount / Convert.ToInt32(rows) && Convert.ToInt32(page) <= iRowsCount / Convert.ToInt32(rows) + 1)
            {
                iMaxCount = iRowsCount;
            }
            else
            {
                iMaxCount = Convert.ToInt32(rows) * Convert.ToInt32(page);
            }

            StringBuilder sb = new StringBuilder("");
            sb.Append("{");
            sb.AppendFormat("\"total\":{0}", iRowsCount);
            sb.Append(",\"rows\":[");
            for (int i = Convert.ToInt32(rows) * (Convert.ToInt32(page) - 1); i < iMaxCount; i++)
            {
                sb.Append("{");

                string[] strFiledArray = strFileds.Split(',');
                for (int j = 0; j < strFiledArray.Length; j++)
                {
                    //获取称重总重量
                    int releseNum = 0;
                    int notReleseNum = 0;
                    int notProNum = 0;
                    int subNum = 0;

                    //releseNum = tSubWayBill.GetActualReleseNum(int.Parse(dt_Actual.Rows[i]["wbID"].ToString()));
                    //notReleseNum = tSubWayBill.GetActualNotReleseNum(int.Parse(dt_Actual.Rows[i]["wbID"].ToString()));
                    //notProNum = tSubWayBill.GetActualNotProNum(int.Parse(dt_Actual.Rows[i]["wbID"].ToString()));
                    //subNum = tSubWayBill.GetActualSubNum(int.Parse(dt_Actual.Rows[i]["wbID"].ToString()));
                    if (dic_SumInfo.ContainsKey(dt_Actual.Rows[i]["wbID"].ToString()))
                    {
                        releseNum = dic_SumInfo[dt_Actual.Rows[i]["wbID"].ToString()].releseNum;
                        notReleseNum = dic_SumInfo[dt_Actual.Rows[i]["wbID"].ToString()].notReleseNum;
                        notProNum = dic_SumInfo[dt_Actual.Rows[i]["wbID"].ToString()].notProNum;
                        subNum = dic_SumInfo[dt_Actual.Rows[i]["wbID"].ToString()].subNum;
                    }

                    //if (dtSubWayBillInfo != null && dtSubWayBillInfo.Rows.Count > 0)
                    //{
                    //    for (int k = 0; k < dtSubWayBillInfo.Rows.Count; k++)
                    //    {
                    //        if (dtSubWayBillInfo.Rows[k]["swb_wbID"].ToString() == dt_Actual.Rows[i]["wbID"].ToString())
                    //        {
                    //            if (((Convert.ToInt32(dtSubWayBillInfo.Rows[k]["swbNeedCheck"]) == 0) && (dtSubWayBillInfo.Rows[k]["swbSortingTime"] != DBNull.Value)) || ((Convert.ToInt32(dtSubWayBillInfo.Rows[k]["swbNeedCheck"]) == 2) && (dtSubWayBillInfo.Rows[k]["swbSortingTime"] != DBNull.Value)))//(swbNeedCheck=0 or swbNeedCheck=2) and swbSortingTime is not null
                    //            {
                    //                releseNum = releseNum + 1;
                    //            }
                    //            if (Convert.ToInt32(dtSubWayBillInfo.Rows[k]["swbNeedCheck"]) == 3)//swbNeedCheck=3
                    //            {
                    //                notReleseNum = notReleseNum + 1;
                    //            }
                    //            if (dtSubWayBillInfo.Rows[k]["swbSortingTime"] == DBNull.Value)// swbSortingTime is null 
                    //            {
                    //                notProNum = notProNum + 1;
                    //            }

                    //            subNum = subNum + 1;

                    //        }

                    //    }

                    //}

                    switch (strFiledArray[j])
                    {
                        case "wbCompany"://格式化公司(保存的是用户名，取出公司名)
                            if (j != strFiledArray.Length - 1)
                            {
                                sb.AppendFormat("\"{0}\":\"{1}\",", strFiledArray[j], dt.Rows[i][strFiledArray[j]] == DBNull.Value ? "" : dt.Rows[i][strFiledArray[j]].ToString());
                            }
                            else
                            {
                                sb.AppendFormat("\"{0}\":\"{1}\"", strFiledArray[j], dt.Rows[i][strFiledArray[j]] == DBNull.Value ? "" : dt.Rows[i][strFiledArray[j]].ToString());
                            }
                            break;
                        case "wbSubNumber_Custom":
                            if (subNum != -1)
                            {
                                if (j != strFiledArray.Length - 1)
                                {
                                    sb.AppendFormat("\"{0}\":\"{1}\",", strFiledArray[j], subNum);
                                }
                                else
                                {
                                    sb.AppendFormat("\"{0}\":\"{1}\"", strFiledArray[j], subNum);
                                }
                            }
                            else
                            {
                                if (j != strFiledArray.Length - 1)
                                {
                                    sb.AppendFormat("\"{0}\":\"{1}\",", strFiledArray[j], "无");
                                }
                                else
                                {
                                    sb.AppendFormat("\"{0}\":\"{1}\"", strFiledArray[j], "无");
                                }
                            }
                            break;
                        case "wbReleaseCount_Custom":
                            if (releseNum != -1)
                            {
                                if (j != strFiledArray.Length - 1)
                                {
                                    sb.AppendFormat("\"{0}\":\"{1}\",", strFiledArray[j], releseNum);
                                }
                                else
                                {
                                    sb.AppendFormat("\"{0}\":\"{1}\"", strFiledArray[j], releseNum);
                                }
                            }
                            else
                            {
                                if (j != strFiledArray.Length - 1)
                                {
                                    sb.AppendFormat("\"{0}\":\"{1}\",", strFiledArray[j], "无");
                                }
                                else
                                {
                                    sb.AppendFormat("\"{0}\":\"{1}\"", strFiledArray[j], "无");
                                }
                            }
                            break;
                        case "wbUnReleaseCount_Custom":
                            if (notReleseNum != -1)
                            {
                                if (j != strFiledArray.Length - 1)
                                {
                                    sb.AppendFormat("\"{0}\":\"{1}\",", strFiledArray[j], notReleseNum);
                                }
                                else
                                {
                                    sb.AppendFormat("\"{0}\":\"{1}\"", strFiledArray[j], notReleseNum);
                                }
                            }
                            else
                            {
                                if (j != strFiledArray.Length - 1)
                                {
                                    sb.AppendFormat("\"{0}\":\"{1}\",", strFiledArray[j], "无");
                                }
                                else
                                {
                                    sb.AppendFormat("\"{0}\":\"{1}\"", strFiledArray[j], "无");
                                }
                            }
                            break;
                        case "wbNotProCount_Custom":
                            if (notProNum != -1)
                            {
                                if (j != strFiledArray.Length - 1)
                                {
                                    sb.AppendFormat("\"{0}\":\"{1}\",", strFiledArray[j], notProNum);
                                }
                                else
                                {
                                    sb.AppendFormat("\"{0}\":\"{1}\"", strFiledArray[j], notProNum);
                                }
                            }
                            else
                            {
                                if (j != strFiledArray.Length - 1)
                                {
                                    sb.AppendFormat("\"{0}\":\"{1}\",", strFiledArray[j], "无");
                                }
                                else
                                {
                                    sb.AppendFormat("\"{0}\":\"{1}\"", strFiledArray[j], "无");
                                }
                            }
                            break;
                        default:
                            if (j != strFiledArray.Length - 1)
                            {
                                sb.AppendFormat("\"{0}\":\"{1}\",", strFiledArray[j], dt_Actual.Rows[i][strFiledArray[j]] == DBNull.Value ? "" : (dt_Actual.Rows[i][strFiledArray[j]].ToString().Replace("\r\n", "")));
                            }
                            else
                            {
                                sb.AppendFormat("\"{0}\":\"{1}\"", strFiledArray[j], dt_Actual.Rows[i][strFiledArray[j]] == DBNull.Value ? "" : (dt_Actual.Rows[i][strFiledArray[j]].ToString().Replace("\r\n", "")));
                            }
                            break;
                    }
                }

                if (i == dt_Actual.Rows.Count - 1)
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


        [HttpGet]
        public ActionResult Print(string order, string page, string rows, string sort, string ddStatus, string inputBeginDate, string inputEndDate, string txtGCode, string txtVoyage)
        {
            string strSQLQuery = "";
            string strTemp = "";
            int iRowsCount = 0;
            int iMaxCount = 0;

            strSQLQuery = "select * from V_Distinct_WayBill {0} ";

            ddStatus = Server.UrlDecode(ddStatus.ToString());
            inputBeginDate = Server.UrlDecode(inputBeginDate.ToString());
            inputEndDate = Server.UrlDecode(inputEndDate.ToString());
            txtGCode = Server.UrlDecode(txtGCode.ToString());
            txtVoyage = Server.UrlDecode(txtVoyage.ToString());

            if (ddStatus == "-1" || ddStatus == "1")//查看已预检
            {
                strTemp = " where ( wbStatus=1 or wbStatus=2 ) ";
            }
            else if (ddStatus == "0")//查看未预检
            {
                strTemp = " where (  wbStatus=0 ) and (wbTaxFeeConfirm=1)";
            }

            if (inputBeginDate != "" && inputEndDate != "")
            {
                strTemp = strTemp + " and (  wbStorageDate>='" + Convert.ToDateTime(inputBeginDate).ToString("yyyyMMdd") + "' and wbStorageDate<='" + Convert.ToDateTime(inputEndDate).ToString("yyyyMMdd") + "' ) ";
            }

            if (txtGCode != "")
            {
                strTemp = strTemp + " and (  wbSerialNum like '%" + txtGCode + "%') ";
            }

            if (txtVoyage != "" && txtVoyage != "---请选择---")
            {
                strTemp = strTemp + " and (  wbCompany like '%" + txtVoyage + "%') ";
            }

            strTemp = strTemp + " order by " + sort + " " + order;

            DataSet ds = SqlServerHelper.Query(string.Format(strSQLQuery, strTemp));
            DataTable dt = ds.Tables[0];
            Boolean bNeedCheck = false;

            Dictionary<string, int> dic_detainNum = null;
            Dictionary<string, Struct_SumInfo> dic_SumInfo = null;

            DataSet dsSubWayBillInfo = null;
            DataTable dtSubWayBillInfo = null;
            dsSubWayBillInfo = tSubWayBill.GetAllSubWayBillInfo();
            if (dsSubWayBillInfo != null)
            {
                dtSubWayBillInfo = dsSubWayBillInfo.Tables[0];
            }

            //if (dtSubWayBillInfo != null && dtSubWayBillInfo.Rows.Count > 0)
            //{
            //    for (int m = 0; m < dtSubWayBillInfo.Rows.Count; m++)
            //    {
            //        if (dtSubWayBillInfo.Rows[m]["swb_wbID"].ToString() == dt.Rows[i]["wbID"].ToString())
            //        {
            //            if (Convert.ToInt32(dtSubWayBillInfo.Rows[m]["swbNeedCheck"].ToString()) == 3)
            //            {
            //                detainNum = detainNum + 1;
            //                break;
            //            }
            //        }
            //    }
            //}
            dic_detainNum = new Dictionary<string, int>();
            dic_SumInfo = new Dictionary<string, Struct_SumInfo>();
            if (dtSubWayBillInfo != null && dtSubWayBillInfo.Rows.Count > 0)
            {
                for (int m = 0; m < dtSubWayBillInfo.Rows.Count; m++)
                {
                    if (!dic_SumInfo.ContainsKey(dtSubWayBillInfo.Rows[m]["swb_wbID"].ToString()))
                    {
                        dic_SumInfo.Add(dtSubWayBillInfo.Rows[m]["swb_wbID"].ToString(), new Struct_SumInfo()
                        {
                            releseNum = 0,
                            notReleseNum = 0,
                            notProNum = 0,
                            subNum = 0,
                        });
                    }

                    if (Convert.ToInt32(dtSubWayBillInfo.Rows[m]["swbNeedCheck"].ToString()) == 3)
                    {
                        if (dic_detainNum.ContainsKey(dtSubWayBillInfo.Rows[m]["swb_wbID"].ToString()))
                        {
                            dic_detainNum[dtSubWayBillInfo.Rows[m]["swb_wbID"].ToString()] = dic_detainNum[dtSubWayBillInfo.Rows[m]["swb_wbID"].ToString()] + 1;
                        }
                        else
                        {
                            dic_detainNum.Add(dtSubWayBillInfo.Rows[m]["swb_wbID"].ToString(), 1);
                        }
                    }

                    if (((Convert.ToInt32(dtSubWayBillInfo.Rows[m]["swbNeedCheck"]) == 0) && (dtSubWayBillInfo.Rows[m]["swbSortingTime"] != DBNull.Value)) || ((Convert.ToInt32(dtSubWayBillInfo.Rows[m]["swbNeedCheck"]) == 2) && (dtSubWayBillInfo.Rows[m]["swbSortingTime"] != DBNull.Value)))//(swbNeedCheck=0 or swbNeedCheck=2) and swbSortingTime is not null
                    {
                        dic_SumInfo[dtSubWayBillInfo.Rows[m]["swb_wbID"].ToString()].releseNum = dic_SumInfo[dtSubWayBillInfo.Rows[m]["swb_wbID"].ToString()].releseNum + 1;
                    }
                    if (Convert.ToInt32(dtSubWayBillInfo.Rows[m]["swbNeedCheck"]) == 3)//swbNeedCheck=3
                    {
                        dic_SumInfo[dtSubWayBillInfo.Rows[m]["swb_wbID"].ToString()].notReleseNum = dic_SumInfo[dtSubWayBillInfo.Rows[m]["swb_wbID"].ToString()].notReleseNum + 1;
                    }
                    if (dtSubWayBillInfo.Rows[m]["swbSortingTime"] == DBNull.Value)// swbSortingTime is null 
                    {
                        dic_SumInfo[dtSubWayBillInfo.Rows[m]["swb_wbID"].ToString()].notProNum = dic_SumInfo[dtSubWayBillInfo.Rows[m]["swb_wbID"].ToString()].notProNum + 1;
                    }

                    dic_SumInfo[dtSubWayBillInfo.Rows[m]["swb_wbID"].ToString()].subNum = dic_SumInfo[dtSubWayBillInfo.Rows[m]["swb_wbID"].ToString()].subNum + 1;
                }
            }

            DataTable dt_Actual = new DataTable();
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                dt_Actual.Columns.Add(dt.Columns[i].ColumnName, dt.Columns[i].DataType);
            }

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                bNeedCheck = false;
                if ("2" == dt.Rows[i]["wbStatus"].ToString())
                {
                    int detainNum = 0;
                    // swb_wbID=" + wbID + " and  swbNeedCheck=3 "
                    if (dic_detainNum.ContainsKey(dt.Rows[i]["wbID"].ToString()))
                    {
                        detainNum = dic_detainNum[dt.Rows[i]["wbID"].ToString()];
                    }
                    //detainNum= tSubWayBill.GetActualNotReleseNum(int.Parse(dt.Rows[i]["wbID"].ToString()));
                    if (detainNum != 0)
                    {
                        bNeedCheck = true;
                    }
                }
                else
                {
                    bNeedCheck = true;
                }

                if (bNeedCheck)
                {
                    DataRow row = dt_Actual.NewRow();
                    for (int k = 0; k < row.ItemArray.Length; k++)
                    {
                        row[k] = dt.Rows[i][k];
                    }
                    dt_Actual.Rows.Add(row);

                    iRowsCount = iRowsCount + 1;
                }
            }

            if (Convert.ToInt32(page) > iRowsCount / Convert.ToInt32(rows) && Convert.ToInt32(page) <= iRowsCount / Convert.ToInt32(rows) + 1)
            {
                iMaxCount = iRowsCount;
            }
            else
            {
                iMaxCount = Convert.ToInt32(rows) * Convert.ToInt32(page);
            }
            //,,,,,,,
            DataTable dtCustom = new DataTable();
            dtCustom.Columns.Add("wbStorageDate", Type.GetType("System.String"));
            dtCustom.Columns.Add("wbCompany", Type.GetType("System.String"));
            dtCustom.Columns.Add("wbSerialNum", Type.GetType("System.String"));
            dtCustom.Columns.Add("wbSubNumber_Custom", Type.GetType("System.String"));
            dtCustom.Columns.Add("wbReleaseCount_Custom", Type.GetType("System.String"));
            dtCustom.Columns.Add("wbUnReleaseCount_Custom", Type.GetType("System.String"));
            dtCustom.Columns.Add("wbNotProCount_Custom", Type.GetType("System.String"));
            dtCustom.Columns.Add("wbID", Type.GetType("System.String"));

            DataRow drCustom = null;
            for (int i = Convert.ToInt32(rows) * (Convert.ToInt32(page) - 1); i < iMaxCount; i++)
            {
                drCustom = dtCustom.NewRow();
                string[] strFiledArray = strFileds.Split(',');
                for (int j = 0; j < strFiledArray.Length; j++)
                {
                    //获取称重总重量
                    int releseNum = -1;
                    int notReleseNum = -1;
                    int notProNum = -1;
                    int subNum = -1;

                    //releseNum = tSubWayBill.GetActualReleseNum(int.Parse(dt_Actual.Rows[i]["wbID"].ToString()));
                    //notReleseNum = tSubWayBill.GetActualNotReleseNum(int.Parse(dt_Actual.Rows[i]["wbID"].ToString()));
                    //notProNum = tSubWayBill.GetActualNotProNum(int.Parse(dt_Actual.Rows[i]["wbID"].ToString()));
                    //subNum = tSubWayBill.GetActualSubNum(int.Parse(dt_Actual.Rows[i]["wbID"].ToString()));
                    if (dic_SumInfo.ContainsKey(dt_Actual.Rows[i]["wbID"].ToString()))
                    {
                        releseNum = dic_SumInfo[dt_Actual.Rows[i]["wbID"].ToString()].releseNum;
                        notReleseNum = dic_SumInfo[dt_Actual.Rows[i]["wbID"].ToString()].notReleseNum;
                        notProNum = dic_SumInfo[dt_Actual.Rows[i]["wbID"].ToString()].notProNum;
                        subNum = dic_SumInfo[dt_Actual.Rows[i]["wbID"].ToString()].subNum;
                    }

                    //if (dtSubWayBillInfo != null && dtSubWayBillInfo.Rows.Count > 0)
                    //{
                    //    for (int k = 0; k < dtSubWayBillInfo.Rows.Count; k++)
                    //    {
                    //        if (dtSubWayBillInfo.Rows[k]["swb_wbID"].ToString() == dt_Actual.Rows[i]["wbID"].ToString())
                    //        {
                    //            if (((Convert.ToInt32(dtSubWayBillInfo.Rows[k]["swbNeedCheck"]) == 0) && (dtSubWayBillInfo.Rows[k]["swbSortingTime"] != DBNull.Value)) || ((Convert.ToInt32(dtSubWayBillInfo.Rows[k]["swbNeedCheck"]) == 2) && (dtSubWayBillInfo.Rows[k]["swbSortingTime"] != DBNull.Value)))//(swbNeedCheck=0 or swbNeedCheck=2) and swbSortingTime is not null
                    //            {
                    //                releseNum = releseNum + 1;
                    //            }
                    //            if (Convert.ToInt32(dtSubWayBillInfo.Rows[k]["swbNeedCheck"]) == 3)//swbNeedCheck=3
                    //            {
                    //                notReleseNum = notReleseNum + 1;
                    //            }
                    //            if (dtSubWayBillInfo.Rows[k]["swbSortingTime"] == DBNull.Value)// swbSortingTime is null 
                    //            {
                    //                notProNum = notProNum + 1;
                    //            }

                    //            subNum = subNum + 1;

                    //        }

                    //    }

                    //}

                    switch (strFiledArray[j])
                    {
                        case "wbCompany"://格式化公司(保存的是用户名，取出公司名)
                            drCustom[strFiledArray[j]] = dt.Rows[i][strFiledArray[j]] == DBNull.Value ? "" : dt.Rows[i][strFiledArray[j]].ToString();
                            break;
                        case "wbSubNumber_Custom":
                            if (subNum != -1)
                            {
                                drCustom[strFiledArray[j]] = subNum;
                            }
                            else
                            {
                                drCustom[strFiledArray[j]] = "无";
                            }
                            break;
                        case "wbReleaseCount_Custom":
                            if (releseNum != -1)
                            {
                                drCustom[strFiledArray[j]] = releseNum;
                            }
                            else
                            {
                                drCustom[strFiledArray[j]] = "无";
                            }
                            break;
                        case "wbUnReleaseCount_Custom":
                            if (notReleseNum != -1)
                            {
                                drCustom[strFiledArray[j]] = notReleseNum;
                            }
                            else
                            {
                                drCustom[strFiledArray[j]] = "无";
                            }
                            break;
                        case "wbNotProCount_Custom":
                            if (notProNum != -1)
                            {
                                drCustom[strFiledArray[j]] = notProNum;
                            }
                            else
                            {
                                drCustom[strFiledArray[j]] = "无";
                            }
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
            ReportDataSource reportDataSource = new ReportDataSource("CustomerConfirm_DS", dtCustom);

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
        public ActionResult Excel(string order, string page, string rows, string sort, string ddStatus, string inputBeginDate, string inputEndDate, string txtGCode, string txtVoyage, string browserType)
        {
            string strSQLQuery = "";
            string strTemp = "";
            int iRowsCount = 0;
            int iMaxCount = 0;

            strSQLQuery = "select * from V_Distinct_WayBill {0} ";

            ddStatus = Server.UrlDecode(ddStatus.ToString());
            inputBeginDate = Server.UrlDecode(inputBeginDate.ToString());
            inputEndDate = Server.UrlDecode(inputEndDate.ToString());
            txtGCode = Server.UrlDecode(txtGCode.ToString());
            txtVoyage = Server.UrlDecode(txtVoyage.ToString());

            if (ddStatus == "-1" || ddStatus == "1")//查看已预检
            {
                strTemp = " where ( wbStatus=1 or wbStatus=2 ) ";
            }
            else if (ddStatus == "0")//查看未预检
            {
                strTemp = " where (  wbStatus=0 ) and (wbTaxFeeConfirm=1)";
            }

            if (inputBeginDate != "" && inputEndDate != "")
            {
                strTemp = strTemp + " and (  wbStorageDate>='" + Convert.ToDateTime(inputBeginDate).ToString("yyyyMMdd") + "' and wbStorageDate<='" + Convert.ToDateTime(inputEndDate).ToString("yyyyMMdd") + "' ) ";
            }

            if (txtGCode != "")
            {
                strTemp = strTemp + " and (  wbSerialNum like '%" + txtGCode + "%') ";
            }

            if (txtVoyage != "" && txtVoyage != "---请选择---")
            {
                strTemp = strTemp + " and (  wbCompany like '%" + txtVoyage + "%') ";
            }

            strTemp = strTemp + " order by " + sort + " " + order;

            DataSet ds = SqlServerHelper.Query(string.Format(strSQLQuery, strTemp));
            DataTable dt = ds.Tables[0];
            Boolean bNeedCheck = false;

            Dictionary<string, int> dic_detainNum = null;
            Dictionary<string, Struct_SumInfo> dic_SumInfo = null;

            DataSet dsSubWayBillInfo = null;
            DataTable dtSubWayBillInfo = null;
            dsSubWayBillInfo = tSubWayBill.GetAllSubWayBillInfo();
            if (dsSubWayBillInfo != null)
            {
                dtSubWayBillInfo = dsSubWayBillInfo.Tables[0];
            }

            //if (dtSubWayBillInfo != null && dtSubWayBillInfo.Rows.Count > 0)
            //{
            //    for (int m = 0; m < dtSubWayBillInfo.Rows.Count; m++)
            //    {
            //        if (dtSubWayBillInfo.Rows[m]["swb_wbID"].ToString() == dt.Rows[i]["wbID"].ToString())
            //        {
            //            if (Convert.ToInt32(dtSubWayBillInfo.Rows[m]["swbNeedCheck"].ToString()) == 3)
            //            {
            //                detainNum = detainNum + 1;
            //                break;
            //            }
            //        }
            //    }
            //}
            dic_detainNum = new Dictionary<string, int>();
            dic_SumInfo = new Dictionary<string, Struct_SumInfo>();
            if (dtSubWayBillInfo != null && dtSubWayBillInfo.Rows.Count > 0)
            {
                for (int m = 0; m < dtSubWayBillInfo.Rows.Count; m++)
                {
                    if (!dic_SumInfo.ContainsKey(dtSubWayBillInfo.Rows[m]["swb_wbID"].ToString()))
                    {
                        dic_SumInfo.Add(dtSubWayBillInfo.Rows[m]["swb_wbID"].ToString(), new Struct_SumInfo()
                        {
                            releseNum = 0,
                            notReleseNum = 0,
                            notProNum = 0,
                            subNum = 0,
                        });
                    }

                    if (Convert.ToInt32(dtSubWayBillInfo.Rows[m]["swbNeedCheck"].ToString()) == 3)
                    {
                        if (dic_detainNum.ContainsKey(dtSubWayBillInfo.Rows[m]["swb_wbID"].ToString()))
                        {
                            dic_detainNum[dtSubWayBillInfo.Rows[m]["swb_wbID"].ToString()] = dic_detainNum[dtSubWayBillInfo.Rows[m]["swb_wbID"].ToString()] + 1;
                        }
                        else
                        {
                            dic_detainNum.Add(dtSubWayBillInfo.Rows[m]["swb_wbID"].ToString(), 1);
                        }
                    }

                    if (((Convert.ToInt32(dtSubWayBillInfo.Rows[m]["swbNeedCheck"]) == 0) && (dtSubWayBillInfo.Rows[m]["swbSortingTime"] != DBNull.Value)) || ((Convert.ToInt32(dtSubWayBillInfo.Rows[m]["swbNeedCheck"]) == 2) && (dtSubWayBillInfo.Rows[m]["swbSortingTime"] != DBNull.Value)))//(swbNeedCheck=0 or swbNeedCheck=2) and swbSortingTime is not null
                    {
                        dic_SumInfo[dtSubWayBillInfo.Rows[m]["swb_wbID"].ToString()].releseNum = dic_SumInfo[dtSubWayBillInfo.Rows[m]["swb_wbID"].ToString()].releseNum + 1;
                    }
                    if (Convert.ToInt32(dtSubWayBillInfo.Rows[m]["swbNeedCheck"]) == 3)//swbNeedCheck=3
                    {
                        dic_SumInfo[dtSubWayBillInfo.Rows[m]["swb_wbID"].ToString()].notReleseNum = dic_SumInfo[dtSubWayBillInfo.Rows[m]["swb_wbID"].ToString()].notReleseNum + 1;
                    }
                    if (dtSubWayBillInfo.Rows[m]["swbSortingTime"] == DBNull.Value)// swbSortingTime is null 
                    {
                        dic_SumInfo[dtSubWayBillInfo.Rows[m]["swb_wbID"].ToString()].notProNum = dic_SumInfo[dtSubWayBillInfo.Rows[m]["swb_wbID"].ToString()].notProNum + 1;
                    }

                    dic_SumInfo[dtSubWayBillInfo.Rows[m]["swb_wbID"].ToString()].subNum = dic_SumInfo[dtSubWayBillInfo.Rows[m]["swb_wbID"].ToString()].subNum + 1;
                }
            }

            DataTable dt_Actual = new DataTable();
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                dt_Actual.Columns.Add(dt.Columns[i].ColumnName, dt.Columns[i].DataType);
            }

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                bNeedCheck = false;
                if ("2" == dt.Rows[i]["wbStatus"].ToString())
                {
                    int detainNum = 0;
                    // swb_wbID=" + wbID + " and  swbNeedCheck=3 "
                    if (dic_detainNum.ContainsKey(dt.Rows[i]["wbID"].ToString()))
                    {
                        detainNum = dic_detainNum[dt.Rows[i]["wbID"].ToString()];
                    }
                    //detainNum= tSubWayBill.GetActualNotReleseNum(int.Parse(dt.Rows[i]["wbID"].ToString()));
                    if (detainNum != 0)
                    {
                        bNeedCheck = true;
                    }
                }
                else
                {
                    bNeedCheck = true;
                }

                if (bNeedCheck)
                {
                    DataRow row = dt_Actual.NewRow();
                    for (int k = 0; k < row.ItemArray.Length; k++)
                    {
                        row[k] = dt.Rows[i][k];
                    }
                    dt_Actual.Rows.Add(row);

                    iRowsCount = iRowsCount + 1;
                }
            }

            if (Convert.ToInt32(page) > iRowsCount / Convert.ToInt32(rows) && Convert.ToInt32(page) <= iRowsCount / Convert.ToInt32(rows) + 1)
            {
                iMaxCount = iRowsCount;
            }
            else
            {
                iMaxCount = Convert.ToInt32(rows) * Convert.ToInt32(page);
            }
            //,,,,,,,
            DataTable dtCustom = new DataTable();
            dtCustom.Columns.Add("wbStorageDate", Type.GetType("System.String"));
            dtCustom.Columns.Add("wbCompany", Type.GetType("System.String"));
            dtCustom.Columns.Add("wbSerialNum", Type.GetType("System.String"));
            dtCustom.Columns.Add("wbSubNumber_Custom", Type.GetType("System.String"));
            dtCustom.Columns.Add("wbReleaseCount_Custom", Type.GetType("System.String"));
            dtCustom.Columns.Add("wbUnReleaseCount_Custom", Type.GetType("System.String"));
            dtCustom.Columns.Add("wbNotProCount_Custom", Type.GetType("System.String"));
            dtCustom.Columns.Add("wbID", Type.GetType("System.String"));

            DataRow drCustom = null;
            for (int i = Convert.ToInt32(rows) * (Convert.ToInt32(page) - 1); i < iMaxCount; i++)
            {
                drCustom = dtCustom.NewRow();
                string[] strFiledArray = strFileds.Split(',');
                for (int j = 0; j < strFiledArray.Length; j++)
                {
                    //获取称重总重量
                    int releseNum = -1;
                    int notReleseNum = -1;
                    int notProNum = -1;
                    int subNum = -1;

                    //releseNum = tSubWayBill.GetActualReleseNum(int.Parse(dt_Actual.Rows[i]["wbID"].ToString()));
                    //notReleseNum = tSubWayBill.GetActualNotReleseNum(int.Parse(dt_Actual.Rows[i]["wbID"].ToString()));
                    //notProNum = tSubWayBill.GetActualNotProNum(int.Parse(dt_Actual.Rows[i]["wbID"].ToString()));
                    //subNum = tSubWayBill.GetActualSubNum(int.Parse(dt_Actual.Rows[i]["wbID"].ToString()));
                    if (dic_SumInfo.ContainsKey(dt_Actual.Rows[i]["wbID"].ToString()))
                    {
                        releseNum = dic_SumInfo[dt_Actual.Rows[i]["wbID"].ToString()].releseNum;
                        notReleseNum = dic_SumInfo[dt_Actual.Rows[i]["wbID"].ToString()].notReleseNum;
                        notProNum = dic_SumInfo[dt_Actual.Rows[i]["wbID"].ToString()].notProNum;
                        subNum = dic_SumInfo[dt_Actual.Rows[i]["wbID"].ToString()].subNum;
                    }

                    //if (dtSubWayBillInfo != null && dtSubWayBillInfo.Rows.Count > 0)
                    //{
                    //    for (int k = 0; k < dtSubWayBillInfo.Rows.Count; k++)
                    //    {
                    //        if (dtSubWayBillInfo.Rows[k]["swb_wbID"].ToString() == dt_Actual.Rows[i]["wbID"].ToString())
                    //        {
                    //            if (((Convert.ToInt32(dtSubWayBillInfo.Rows[k]["swbNeedCheck"]) == 0) && (dtSubWayBillInfo.Rows[k]["swbSortingTime"] != DBNull.Value)) || ((Convert.ToInt32(dtSubWayBillInfo.Rows[k]["swbNeedCheck"]) == 2) && (dtSubWayBillInfo.Rows[k]["swbSortingTime"] != DBNull.Value)))//(swbNeedCheck=0 or swbNeedCheck=2) and swbSortingTime is not null
                    //            {
                    //                releseNum = releseNum + 1;
                    //            }
                    //            if (Convert.ToInt32(dtSubWayBillInfo.Rows[k]["swbNeedCheck"]) == 3)//swbNeedCheck=3
                    //            {
                    //                notReleseNum = notReleseNum + 1;
                    //            }
                    //            if (dtSubWayBillInfo.Rows[k]["swbSortingTime"] == DBNull.Value)// swbSortingTime is null 
                    //            {
                    //                notProNum = notProNum + 1;
                    //            }

                    //            subNum = subNum + 1;

                    //        }

                    //    }

                    //}

                    switch (strFiledArray[j])
                    {
                        case "wbCompany"://格式化公司(保存的是用户名，取出公司名)
                            drCustom[strFiledArray[j]] = dt.Rows[i][strFiledArray[j]] == DBNull.Value ? "" : dt.Rows[i][strFiledArray[j]].ToString();
                            break;
                        case "wbSubNumber_Custom":
                            if (subNum != -1)
                            {
                                drCustom[strFiledArray[j]] = subNum;
                            }
                            else
                            {
                                drCustom[strFiledArray[j]] = "无";
                            }
                            break;
                        case "wbReleaseCount_Custom":
                            if (releseNum != -1)
                            {
                                drCustom[strFiledArray[j]] = releseNum;
                            }
                            else
                            {
                                drCustom[strFiledArray[j]] = "无";
                            }
                            break;
                        case "wbUnReleaseCount_Custom":
                            if (notReleseNum != -1)
                            {
                                drCustom[strFiledArray[j]] = notReleseNum;
                            }
                            else
                            {
                                drCustom[strFiledArray[j]] = "无";
                            }
                            break;
                        case "wbNotProCount_Custom":
                            if (notProNum != -1)
                            {
                                drCustom[strFiledArray[j]] = notProNum;
                            }
                            else
                            {
                                drCustom[strFiledArray[j]] = "无";
                            }
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
            ReportDataSource reportDataSource = new ReportDataSource("CustomerConfirm_DS", dtCustom);

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

            string strOutputFileName = "放行记录_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";

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


        [HttpGet]
        public string UpdateReleaseStatus(string ids)
        {
            string strRet = "{\"result\":\"error\",\"message\":\"放行失败,原因未知\"}";
            ids = Server.UrlDecode(ids);
            if (ids != "")
            {
                try
                {
                    if (ids.EndsWith("*"))
                    {
                        ids = ids.Substring(0, ids.Length - 1);
                        ids = ids.Replace("*", ",");
                    }

                    if (wayBillSql.updateReleaStatus(ids))
                    {
                        strRet = "{\"result\":\"ok\",\"message\":\"已经成功将所选择的运单放行\"}";
                    }
                    else
                    {
                        strRet = "{\"result\":\"error\",\"message\":\"放行失败,原因未知\"}";
                    }
                }
                catch (Exception ex)
                {
                    strRet = "{\"result\":\"error\",\"message\":\"" + ex.Message + "\"}";
                }

            }
            else
            {
                strRet = "{\"result\":\"error\",\"message\":\"未选择需要放行的运单号，无需放行\"}";
            }

            return strRet;
        }
    }

    public class Struct_SumInfo
    {
        public int releseNum
        {
            get;
            set;
        }
        public int notReleseNum
        {
            get;
            set;
        }
        public int notProNum
        {
            get;
            set;
        }
        public int subNum
        {
            get;
            set;
        }

    }
}
