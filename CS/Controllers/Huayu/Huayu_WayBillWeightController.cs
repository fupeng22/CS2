﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CS.Filter;
using SQLDAL;
using System.Data.SqlClient;
using System.Data;
using DBUtility;
using Microsoft.Reporting.WebForms;
using System.IO;
using System.Text;
using Model;

namespace CS.Controllers.Huayu
{
    [ErrorAttribute]
    public class Huayu_WayBillWeightController : Controller
    {
        SQLDAL.T_WayBill tWayBill = new T_WayBill();
        SQLDAL.T_SubWayBill tSubWayBill = new T_SubWayBill();
        public const string strFileds = "wbStorageDate,wbCompany,wbSerialNum,wbSubNumber,wbTotalWeight,swbTotalNumber,swbTotalWeight,swbTotalActualWeight,wbStatus,releseNum,notReleseNum,wbwId,ActualWeight,wbID";

        public const string strFieldsForExcel = "wbStorageDate,wbCompany,wbSerialNum,swbTotalNumber,swbTotalWeight,ActualWeight,wbStatus,releseNum,notReleseNum";
        public const string strColumnNames = "报关日期,货代公司,总运单号,分运单总件数,分运单总重量(公斤),计费重量(公斤),状态,放行件数,扣留件数";

        public const string STR_TEMPLATE_EXCEL = "~/Temp/Template/template.xls";
        public const string STR_REPORT_URL = "~/Content/Reports/HuayuWayBillWeight.rdlc";
        //
        // GET: /Forwarder_QueryCompany/
        [HuayuRequiresLoginAttribute]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Print(string order, string page, string rows, string sort, string inputBeginDate, string inputEndDate, string txtGCode, string txtVoyage)
        {
            SqlParameter[] param = new SqlParameter[8];
            param[0] = new SqlParameter();
            param[0].SqlDbType = SqlDbType.VarChar;
            param[0].ParameterName = "@TableName";
            param[0].Direction = ParameterDirection.Input;
            param[0].Value = "V_WayBillWeight";

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
                    strWhereTemp = strWhereTemp + "  (wbSerialNum like '%" + txtGCode.ToString() + "%') ";
                }
            }

            if (inputBeginDate.ToString() != "" && inputEndDate.ToString() != "")
            {
                if (strWhereTemp != "")
                {
                    strWhereTemp = strWhereTemp + "  and wbStorageDate>='" + Convert.ToDateTime(inputBeginDate).ToString("yyyyMMdd") + "' and wbStorageDate<='" + Convert.ToDateTime(inputEndDate).ToString("yyyyMMdd") + "'   ";
                }
                else
                {
                    strWhereTemp = strWhereTemp + "  wbStorageDate>='" + Convert.ToDateTime(inputBeginDate).ToString("yyyyMMdd") + "' and wbStorageDate<='" + Convert.ToDateTime(inputEndDate).ToString("yyyyMMdd") + "'   ";
                }
            }

            if (txtVoyage.ToString() != "" && txtVoyage.ToString() != "---请选择---")
            {
                if (strWhereTemp != "")
                {
                    strWhereTemp = strWhereTemp + "  and wbCompany like '%" + txtVoyage.ToString() + "%'  ";
                }
                else
                {
                    strWhereTemp = strWhereTemp + "  wbCompany like '%" + txtVoyage.ToString() + "%'  ";
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
            dtCustom.Columns.Add("wbStorageDate", Type.GetType("System.String"));
            dtCustom.Columns.Add("wbCompany", Type.GetType("System.String"));
            dtCustom.Columns.Add("wbSerialNum", Type.GetType("System.String"));
            dtCustom.Columns.Add("wbSubNumber", Type.GetType("System.String"));
            dtCustom.Columns.Add("wbTotalWeight", Type.GetType("System.String"));
            dtCustom.Columns.Add("swbTotalNumber", Type.GetType("System.String"));
            dtCustom.Columns.Add("swbTotalWeight", Type.GetType("System.String"));
            dtCustom.Columns.Add("swbTotalActualWeight", Type.GetType("System.String"));
            dtCustom.Columns.Add("wbStatus", Type.GetType("System.String"));
            dtCustom.Columns.Add("releseNum", Type.GetType("System.String"));
            dtCustom.Columns.Add("notReleseNum", Type.GetType("System.String"));
            dtCustom.Columns.Add("wbID", Type.GetType("System.String"));
            dtCustom.Columns.Add("wbwId", Type.GetType("System.String"));
            dtCustom.Columns.Add("ActualWeight", Type.GetType("System.String"));

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
                        case "wbSubNumber":
                            break;
                        case "wbTotalWeight":
                            break;
                        case "swbTotalNumber":
                            break;
                        case "swbTotalActualWeight":
                            break;
                        case "swbTotalWeight":
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
                            drCustom["swbTotalNumber"] = swbTotalNumber;
                            drCustom["swbTotalActualWeight"] = swbTotalActualWeight + "公斤";
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
            ReportDataSource reportDataSource = new ReportDataSource("HuayuWayBillWeight_DS", dtCustom);

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

        //[HttpGet]
        //public ActionResult Print1(string order, string page, string rows, string sort, string inputBeginDate, string inputEndDate, string txtGCode, string txtVoyage)
        //{
        //    SqlParameter[] param = new SqlParameter[8];
        //    param[0] = new SqlParameter();
        //    param[0].SqlDbType = SqlDbType.VarChar;
        //    param[0].ParameterName = "@TableName";
        //    param[0].Direction = ParameterDirection.Input;
        //    param[0].Value = "V_WayBillWeight";

        //    param[1] = new SqlParameter();
        //    param[1].SqlDbType = SqlDbType.VarChar;
        //    param[1].ParameterName = "@FieldKey";
        //    param[1].Direction = ParameterDirection.Input;
        //    param[1].Value = "wbID";

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

        //    txtGCode = Server.UrlDecode(txtGCode.ToString());
        //    inputBeginDate = Server.UrlDecode(inputBeginDate.ToString());
        //    inputEndDate = Server.UrlDecode(inputEndDate.ToString());
        //    txtVoyage = Server.UrlDecode(txtVoyage.ToString());

        //    string strWhereTemp = "";
        //    if (txtGCode.ToString() != "")
        //    {
        //        if (strWhereTemp != "")
        //        {
        //            strWhereTemp = strWhereTemp + " and   (wbSerialNum like '%" + txtGCode.ToString() + "%') ";
        //        }
        //        else
        //        {
        //            strWhereTemp = strWhereTemp + "  (wbSerialNum like '%" + txtGCode.ToString() + "%') ";
        //        }
        //    }

        //    if (inputBeginDate.ToString() != "" && inputEndDate.ToString() != "")
        //    {
        //        if (strWhereTemp != "")
        //        {
        //            strWhereTemp = strWhereTemp + "  and wbStorageDate>='" + Convert.ToDateTime(inputBeginDate).ToString("yyyyMMdd") + "' and wbStorageDate<='" + Convert.ToDateTime(inputEndDate).ToString("yyyyMMdd") + "'   ";
        //        }
        //        else
        //        {
        //            strWhereTemp = strWhereTemp + "  wbStorageDate>='" + Convert.ToDateTime(inputBeginDate).ToString("yyyyMMdd") + "' and wbStorageDate<='" + Convert.ToDateTime(inputEndDate).ToString("yyyyMMdd") + "'   ";
        //        }
        //    }

        //    if (txtVoyage.ToString() != "" && txtVoyage.ToString() != "---请选择---")
        //    {
        //        if (strWhereTemp != "")
        //        {
        //            strWhereTemp = strWhereTemp + "  and wbCompany like '%" + txtVoyage.ToString() + "%'  ";
        //        }
        //        else
        //        {
        //            strWhereTemp = strWhereTemp + "  wbCompany like '%" + txtVoyage.ToString() + "%'  ";
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
        //    dtCustom.Columns.Add("wbStorageDate", Type.GetType("System.String"));
        //    dtCustom.Columns.Add("wbCompany", Type.GetType("System.String"));
        //    dtCustom.Columns.Add("wbSerialNum", Type.GetType("System.String"));
        //    dtCustom.Columns.Add("wbSubNumber", Type.GetType("System.String"));
        //    dtCustom.Columns.Add("wbTotalWeight", Type.GetType("System.String"));
        //    dtCustom.Columns.Add("swbTotalNumber", Type.GetType("System.String"));
        //    dtCustom.Columns.Add("swbTotalWeight", Type.GetType("System.String"));
        //    dtCustom.Columns.Add("swbTotalActualWeight", Type.GetType("System.String"));
        //    dtCustom.Columns.Add("wbStatus", Type.GetType("System.String"));
        //    dtCustom.Columns.Add("releseNum", Type.GetType("System.String"));
        //    dtCustom.Columns.Add("notReleseNum", Type.GetType("System.String"));
        //    dtCustom.Columns.Add("wbID", Type.GetType("System.String"));
        //    dtCustom.Columns.Add("wbwId", Type.GetType("System.String"));
        //    dtCustom.Columns.Add("ActualWeight", Type.GetType("System.String"));

        //    DataRow drCustom = null;

        //    int releseNum = -1;
        //    int notReleseNum = -1;

        //    for (int i = 0; i < dt.Rows.Count; i++)
        //    {
        //        drCustom = dtCustom.NewRow();
        //        string[] strFiledArray = strFileds.Split(',');
        //        for (int j = 0; j < strFiledArray.Length; j++)
        //        {
        //            switch (strFiledArray[j])
        //            {
        //                case "wbSubNumber":
        //                    break;
        //                case "wbTotalWeight":
        //                    break;
        //                case "swbTotalNumber":
        //                    break;
        //                case "swbTotalActualWeight":
        //                    break;
        //                case "swbTotalWeight":
        //                    string swbTotalNumber = "0";
        //                    string swbTotalActualWeight = "0";
        //                    string swbTotalWeight = "0";

        //                    DataSet dsSum = tSubWayBill.GetSubWayBillSumInfo(Convert.ToInt32(dt.Rows[i]["wbID"]));
        //                    if (dsSum != null)
        //                    {
        //                        DataTable dtSum = dsSum.Tables[0];
        //                        if (dtSum != null && dtSum.Rows.Count > 0)
        //                        {
        //                            swbTotalNumber = dtSum.Rows[0]["swbTotalNumber"].ToString();
        //                            swbTotalWeight = dtSum.Rows[0]["swbTotalWeight"].ToString();
        //                            swbTotalActualWeight = dtSum.Rows[0]["swbTotalActualWeight"].ToString();
        //                        }
        //                    }

        //                    drCustom[strFiledArray[j]] = swbTotalWeight + "公斤";
        //                    drCustom["swbTotalNumber"] = swbTotalNumber;
        //                    drCustom["swbTotalActualWeight"] = swbTotalActualWeight + "公斤";
        //                    break;
        //                case "wbStatus":
        //                    releseNum = -1;
        //                    notReleseNum = -1;
        //                    if (j != strFiledArray.Length - 1)
        //                    {
        //                        if (dt.Rows[i][strFiledArray[j]].ToString() != "")
        //                        {
        //                            switch (int.Parse(dt.Rows[i][strFiledArray[j]].ToString()))
        //                            {
        //                                case 0:
        //                                    drCustom[strFiledArray[j]] = dt.Rows[i][strFiledArray[j]] == DBNull.Value ? "" : "等待预检";
        //                                    break;
        //                                case 1:
        //                                    drCustom[strFiledArray[j]] = dt.Rows[i][strFiledArray[j]] == DBNull.Value ? "" : "查验中";
        //                                    break;
        //                                case 2:
        //                                    releseNum = tSubWayBill.GetActualReleseNum(int.Parse(dt.Rows[i]["wbID"].ToString()));
        //                                    notReleseNum = tSubWayBill.GetActualNotReleseNum(int.Parse(dt.Rows[i]["wbID"].ToString()));
        //                                    drCustom[strFiledArray[j]] = dt.Rows[i][strFiledArray[j]] == DBNull.Value ? "" : "已放行";
        //                                    break;
        //                                default:
        //                                    drCustom[strFiledArray[j]] = dt.Rows[i][strFiledArray[j]] == DBNull.Value ? "" : "未知";
        //                                    break;
        //                            }

        //                            if (releseNum != -1)
        //                            {
        //                                drCustom["releseNum"] = releseNum.ToString();
        //                            }
        //                            else
        //                            {
        //                                drCustom["releseNum"] = "无";

        //                            }
        //                            if (notReleseNum != -1)
        //                            {
        //                                drCustom["notReleseNum"] = notReleseNum.ToString();

        //                            }
        //                            else
        //                            {
        //                                drCustom["notReleseNum"] = "无";
        //                            }

        //                        }

        //                    }
        //                    else
        //                    {
        //                        if (dt.Rows[i][strFiledArray[j]].ToString() != "")
        //                        {
        //                            switch (int.Parse(dt.Rows[i][strFiledArray[j]].ToString()))
        //                            {
        //                                case 0:
        //                                    drCustom[strFiledArray[j]] = dt.Rows[i][strFiledArray[j]] == DBNull.Value ? "" : "等待预检";
        //                                    break;
        //                                case 1:
        //                                    drCustom[strFiledArray[j]] = dt.Rows[i][strFiledArray[j]] == DBNull.Value ? "" : "查验中";
        //                                    break;
        //                                case 2:
        //                                    releseNum = tSubWayBill.GetActualReleseNum(int.Parse(dt.Rows[i]["wbID"].ToString()));
        //                                    notReleseNum = tSubWayBill.GetActualNotReleseNum(int.Parse(dt.Rows[i]["wbID"].ToString()));
        //                                    drCustom[strFiledArray[j]] = dt.Rows[i][strFiledArray[j]] == DBNull.Value ? "" : "已放行";
        //                                    break;
        //                                default:
        //                                    drCustom[strFiledArray[j]] = dt.Rows[i][strFiledArray[j]] == DBNull.Value ? "" : "未知";
        //                                    break;
        //                            }

        //                            if (releseNum != -1)
        //                            {
        //                                drCustom["releseNum"] = releseNum.ToString();
        //                            }
        //                            else
        //                            {
        //                                drCustom["releseNum"] = "无";

        //                            }
        //                            if (notReleseNum != -1)
        //                            {
        //                                drCustom["notReleseNum"] = notReleseNum.ToString();

        //                            }
        //                            else
        //                            {
        //                                drCustom["notReleseNum"] = "无";
        //                            }

        //                        }
        //                    }
        //                    break;
        //                case "releseNum":
        //                    break;
        //                case "notReleseNum":
        //                    break;
        //                default:
        //                    drCustom[strFiledArray[j]] = dt.Rows[i][strFiledArray[j]] == DBNull.Value ? "" : (dt.Rows[i][strFiledArray[j]].ToString().Replace("\r\n", ""));
        //                    break;
        //            }

        //        }
        //        if (drCustom["wbID"].ToString() != "")
        //        {
        //            dtCustom.Rows.Add(drCustom);
        //        }
        //    }
        //    dt = null;

        //    DataTable dtExcel = new DataTable();
        //    string[] strFieldsArray = strFieldsForExcel.Split(',');
        //    string[] strColumnArray = strColumnNames.Split(',');
        //    for (int i = 0; i < strFieldsArray.Length; i++)
        //    {
        //        dtExcel.Columns.Add(strFieldsArray[i], Type.GetType("System.String"));
        //    }

        //    for (int i = 0; i < dtCustom.Rows.Count; i++)
        //    {
        //        DataRow drExcel = dtExcel.NewRow();
        //        for (int j = 0; j < strFieldsArray.Length; j++)
        //        {
        //            drExcel[strFieldsArray[j]] = dtCustom.Rows[i][strFieldsArray[j]];
        //        }
        //        dtExcel.Rows.Add(drExcel);
        //    }

        //    for (int i = 0; i < strFieldsArray.Length; i++)
        //    {
        //        dtExcel.Columns[i].ColumnName = strColumnArray[i];
        //    }

        //    System.Web.UI.WebControls.DataGrid dgExport = null;
        //    // 当前对话 
        //    System.Web.HttpContext curContext = System.Web.HttpContext.Current;
        //    // IO用于导出并返回excel文件 
        //    System.IO.StringWriter strWriter = null;
        //    System.Web.UI.HtmlTextWriter htmlWriter = null;
        //    string filename = DateTime.Now.Month + "_" + DateTime.Now.Day + "_"
        //        + DateTime.Now.Hour + "_" + DateTime.Now.Minute+"中文";
        //    byte[] str = null;

        //    if (dtExcel != null)
        //    {
        //        // 设置编码和附件格式 
        //        curContext.Response.ContentType = "application/vnd.ms-excel";
        //        curContext.Response.ContentEncoding = System.Text.Encoding.UTF8;
        //        curContext.Response.Charset = "gb2312";

        //        Response.AppendHeader("Content-Disposition", "attachment;filename=" + filename + ".xls");
        //        // 导出excel文件 
        //        strWriter = new System.IO.StringWriter();
        //        htmlWriter = new System.Web.UI.HtmlTextWriter(strWriter);

        //        // 为了解决dgData中可能进行了分页的情况，需要重新定义一个无分页的DataGrid 
        //        dgExport = new System.Web.UI.WebControls.DataGrid();
        //        dgExport.DataSource = dtExcel.DefaultView;
        //        dgExport.AllowPaging = false;
        //        dgExport.DataBind();
        //        dgExport.RenderControl(htmlWriter);
        //        // 返回客户端 
        //        str = System.Text.Encoding.UTF8.GetBytes(strWriter.ToString());
        //    }
        //    return File(str, "attachment;filename=" + filename + ".xls");
        //}

        //public ActionResult DataTableToExcel(string fundtype)
        //{
        //    //根据传过来的参数查询要到处的数据
        //    System.Data.DataTable dtData = stateService.Getexcetdata(fundtype);
        //    System.Web.UI.WebControls.DataGrid dgExport = null;
        //    // 当前对话 
        //    System.Web.HttpContext curContext = System.Web.HttpContext.Current;
        //    // IO用于导出并返回excel文件 
        //    System.IO.StringWriter strWriter = null;
        //    System.Web.UI.HtmlTextWriter htmlWriter = null;
        //    string filename = DateTime.Now.Month + "_" + DateTime.Now.Day + "_"
        //        + DateTime.Now.Hour + "_" + DateTime.Now.Minute;
        //    byte[] str = null;

        //    if (dtData != null)
        //    {
        //        // 设置编码和附件格式 
        //        curContext.Response.ContentType = "application/vnd.ms-excel";
        //        curContext.Response.ContentEncoding = System.Text.Encoding.UTF8;
        //        curContext.Response.Charset = "gb2312";

        //        Response.AppendHeader("Content-Disposition", "attachment;filename=" + filename + ".xls");
        //        // 导出excel文件 
        //        strWriter = new System.IO.StringWriter();
        //        htmlWriter = new System.Web.UI.HtmlTextWriter(strWriter);

        //        // 为了解决dgData中可能进行了分页的情况，需要重新定义一个无分页的DataGrid 
        //        dgExport = new System.Web.UI.WebControls.DataGrid();
        //        dgExport.DataSource = dtData.DefaultView;
        //        dgExport.AllowPaging = false;
        //        dgExport.DataBind();
        //        dgExport.RenderControl(htmlWriter);
        //        // 返回客户端 
        //        str = System.Text.Encoding.UTF8.GetBytes(strWriter.ToString());
        //    }
        //    return File(str, "attachment;filename=" + filename + ".xls");
        //}

        [HttpGet]
        public ActionResult Excel(string order, string page, string rows, string sort, string inputBeginDate, string inputEndDate, string txtGCode, string txtVoyage, string browserType)
        {
            SqlParameter[] param = new SqlParameter[8];
            param[0] = new SqlParameter();
            param[0].SqlDbType = SqlDbType.VarChar;
            param[0].ParameterName = "@TableName";
            param[0].Direction = ParameterDirection.Input;
            param[0].Value = "V_WayBillWeight";

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
                    strWhereTemp = strWhereTemp + "  (wbSerialNum like '%" + txtGCode.ToString() + "%') ";
                }
            }

            if (inputBeginDate.ToString() != "" && inputEndDate.ToString() != "")
            {
                if (strWhereTemp != "")
                {
                    strWhereTemp = strWhereTemp + "  and wbStorageDate>='" + Convert.ToDateTime(inputBeginDate).ToString("yyyyMMdd") + "' and wbStorageDate<='" + Convert.ToDateTime(inputEndDate).ToString("yyyyMMdd") + "'   ";
                }
                else
                {
                    strWhereTemp = strWhereTemp + "  wbStorageDate>='" + Convert.ToDateTime(inputBeginDate).ToString("yyyyMMdd") + "' and wbStorageDate<='" + Convert.ToDateTime(inputEndDate).ToString("yyyyMMdd") + "'   ";
                }
            }

            if (txtVoyage.ToString() != "" && txtVoyage.ToString() != "---请选择---")
            {
                if (strWhereTemp != "")
                {
                    strWhereTemp = strWhereTemp + "  and wbCompany like '%" + txtVoyage.ToString() + "%'  ";
                }
                else
                {
                    strWhereTemp = strWhereTemp + "  wbCompany like '%" + txtVoyage.ToString() + "%'  ";
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
            dtCustom.Columns.Add("wbStorageDate", Type.GetType("System.String"));
            dtCustom.Columns.Add("wbCompany", Type.GetType("System.String"));
            dtCustom.Columns.Add("wbSerialNum", Type.GetType("System.String"));
            dtCustom.Columns.Add("wbSubNumber", Type.GetType("System.String"));
            dtCustom.Columns.Add("wbTotalWeight", Type.GetType("System.String"));
            dtCustom.Columns.Add("swbTotalNumber", Type.GetType("System.String"));
            dtCustom.Columns.Add("swbTotalWeight", Type.GetType("System.String"));
            dtCustom.Columns.Add("swbTotalActualWeight", Type.GetType("System.String"));
            dtCustom.Columns.Add("wbStatus", Type.GetType("System.String"));
            dtCustom.Columns.Add("releseNum", Type.GetType("System.String"));
            dtCustom.Columns.Add("notReleseNum", Type.GetType("System.String"));
            dtCustom.Columns.Add("wbID", Type.GetType("System.String"));
            dtCustom.Columns.Add("wbwId", Type.GetType("System.String"));
            dtCustom.Columns.Add("ActualWeight", Type.GetType("System.String"));

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
                        case "wbSubNumber":
                            break;
                        case "wbTotalWeight":
                            break;
                        case "swbTotalNumber":
                            break;
                        case "swbTotalActualWeight":
                            break;
                        case "swbTotalWeight":
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
                            drCustom["swbTotalNumber"] = swbTotalNumber;
                            drCustom["swbTotalActualWeight"] = swbTotalActualWeight + "公斤";
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
            ReportDataSource reportDataSource = new ReportDataSource("HuayuWayBillWeight_DS", dtCustom);

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

            string strOutputFileName = "货物统计信息_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";

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
            param[0].Value = "V_WayBillWeight";

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
                    strWhereTemp = strWhereTemp + "  (wbSerialNum like '%" + txtGCode.ToString() + "%') ";
                }
            }

            if (inputBeginDate.ToString() != "" && inputEndDate.ToString() != "")
            {
                if (strWhereTemp != "")
                {
                    strWhereTemp = strWhereTemp + "  and wbStorageDate>='" + Convert.ToDateTime(inputBeginDate).ToString("yyyyMMdd") + "' and wbStorageDate<='" + Convert.ToDateTime(inputEndDate).ToString("yyyyMMdd") + "'   ";
                }
                else
                {
                    strWhereTemp = strWhereTemp + "  wbStorageDate>='" + Convert.ToDateTime(inputBeginDate).ToString("yyyyMMdd") + "' and wbStorageDate<='" + Convert.ToDateTime(inputEndDate).ToString("yyyyMMdd") + "'   ";
                }
            }

            if (txtVoyage.ToString() != "" && txtVoyage.ToString() != "---请选择---")
            {
                if (strWhereTemp != "")
                {
                    strWhereTemp = strWhereTemp + "  and wbCompany like '%" + txtVoyage.ToString() + "%'  ";
                }
                else
                {
                    strWhereTemp = strWhereTemp + "  wbCompany like '%" + txtVoyage.ToString() + "%'  ";
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
                        case "wbSubNumber":
                            break;
                        case "wbTotalWeight":
                            break;
                        case "swbTotalNumber":
                            break;
                        case "swbTotalActualWeight":
                            break;
                        case "swbTotalWeight":
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

                            if (j != strFiledArray.Length - 1)
                            {
                                sb.AppendFormat("\"{0}\":\"{1}公斤\",", strFiledArray[j], swbTotalWeight);
                            }
                            else
                            {
                                sb.AppendFormat("\"{0}\":\"{1}公斤\"", strFiledArray[j], swbTotalWeight);
                            }

                            if (j != strFiledArray.Length - 1)
                            {
                                sb.AppendFormat("\"{0}\":\"{1}\",", "swbTotalNumber", swbTotalNumber);
                            }
                            else
                            {
                                sb.AppendFormat("\"{0}\":\"{1}\"", "swbTotalNumber", swbTotalNumber);
                            }

                            if (j != strFiledArray.Length - 1)
                            {
                                sb.AppendFormat("\"{0}\":\"{1}公斤\",", "swbTotalActualWeight", swbTotalActualWeight);
                            }
                            else
                            {
                                sb.AppendFormat("\"{0}\":\"{1}公斤\"", "swbTotalActualWeight", swbTotalActualWeight);
                            }
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
                                            sb.AppendFormat("\"{0}\":\"{1}\",", strFiledArray[j], dt.Rows[i][strFiledArray[j]] == DBNull.Value ? "" : "等待预检");
                                            break;
                                        case 1:
                                            sb.AppendFormat("\"{0}\":\"{1}\",", strFiledArray[j], dt.Rows[i][strFiledArray[j]] == DBNull.Value ? "" : "查验中");
                                            break;
                                        case 2:
                                            sb.AppendFormat("\"{0}\":\"{1}\",", strFiledArray[j], dt.Rows[i][strFiledArray[j]] == DBNull.Value ? "" : "已放行");
                                            releseNum = tSubWayBill.GetActualReleseNum(int.Parse(dt.Rows[i]["wbID"].ToString()));
                                            notReleseNum = tSubWayBill.GetActualNotReleseNum(int.Parse(dt.Rows[i]["wbID"].ToString()));
                                            break;
                                        default:
                                            sb.AppendFormat("\"{0}\":\"{1}\",", strFiledArray[j], dt.Rows[i][strFiledArray[j]] == DBNull.Value ? "" : "未知");
                                            break;
                                    }



                                    if (releseNum != -1)
                                    {
                                        sb.AppendFormat("\"{0}\":\"{1}\",", "releseNum", releseNum.ToString());
                                    }
                                    else
                                    {
                                        sb.AppendFormat("\"{0}\":\"{1}\",", "releseNum", "无");

                                    }
                                    if (notReleseNum != -1)
                                    {
                                        sb.AppendFormat("\"{0}\":\"{1}\",", "notReleseNum", notReleseNum.ToString());

                                    }
                                    else
                                    {
                                        sb.AppendFormat("\"{0}\":\"{1}\",", "notReleseNum", "无");
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
                                            sb.AppendFormat("\"{0}\":\"{1}\"", strFiledArray[j], "等待预检");
                                            break;
                                        case 1:
                                            sb.AppendFormat("\"{0}\":\"{1}\"", strFiledArray[j], "查验中");
                                            break;
                                        case 2:
                                            sb.AppendFormat("\"{0}\":\"{1}\"", strFiledArray[j], "已放行");
                                            releseNum = tSubWayBill.GetActualReleseNum(int.Parse(dt.Rows[i]["wbID"].ToString()));
                                            notReleseNum = tSubWayBill.GetActualNotReleseNum(int.Parse(dt.Rows[i]["wbID"].ToString()));
                                            break;
                                        default:
                                            sb.AppendFormat("\"{0}\":\"{1}\"", strFiledArray[j], "未知");
                                            break;
                                    }


                                    if (releseNum != -1)
                                    {

                                        sb.AppendFormat("\"{0}\":\"{1}\"", "releseNum", releseNum.ToString());
                                    }
                                    else
                                    {

                                        sb.AppendFormat("\"{0}\":\"{1}\"", "releseNum", "无");

                                    }
                                    if (notReleseNum != -1)
                                    {
                                        sb.AppendFormat("\"{0}\":\"{1}\"", "notReleseNum", notReleseNum.ToString());

                                    }
                                    else
                                    {
                                        sb.AppendFormat("\"{0}\":\"{1}\"", "notReleseNum", "无");
                                    }

                                }
                            }
                            break;
                        case "releseNum":
                            break;
                        case "notReleseNum":
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

        [HttpPost]
        public string UpdateWayBillWeight(string wbID, string wayBillWeight)
        {
            string strRet = "{\"result\":\"error\",\"message\":\"原因未知\"}";
            T_WayBillWeight t_WayBillWeight = new T_WayBillWeight();
            M_WayBillWeight m_WayBillWeight = new M_WayBillWeight();
            wbID = Server.UrlDecode(wbID);
            wayBillWeight = Server.UrlDecode(wayBillWeight);
            try
            {
                m_WayBillWeight.wbw_wbID = Convert.ToInt32(wbID);
                m_WayBillWeight.ActualWeight = Convert.ToDouble(wayBillWeight);
                if (t_WayBillWeight.ExistInWayBillWeight(wbID))
                {
                    t_WayBillWeight.UpdateWayBillWeight(m_WayBillWeight);
                }
                else
                {
                    t_WayBillWeight.AddWayBillWeight(m_WayBillWeight);
                }
                strRet = "{\"result\":\"ok\",\"message\":\"" + "修改成功" + "\"}";
            }
            catch (Exception ex)
            {
                strRet = "{\"result\":\"error\",\"message\":\"" + ex.Message + "\"}";
            }

            return strRet;
        }

    }
}
