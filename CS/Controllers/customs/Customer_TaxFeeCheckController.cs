using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model;
using SQLDAL;
using System.Data.SqlClient;
using System.Data;
using DBUtility;
using Microsoft.Reporting.WebForms;
using System.IO;
using System.Text;

namespace CS.Controllers.customs
{
    public class Customer_TaxFeeCheckController : Controller
    {
        Model.M_WayBill wayBillModel = new M_WayBill();
        Model.M_SubWayBill subWayBillModel = new M_SubWayBill();
        SQLDAL.T_WayBill wayBillSql = new T_WayBill();
        SQLDAL.T_SubWayBill subWayBillSql = new T_SubWayBill();

        SQLDAL.T_WayBill tWayBill = new T_WayBill();
        SQLDAL.T_SubWayBill tSubWayBill = new T_SubWayBill();

        public const string strFileds = "wbSerialNum,wbTotalNumber,wbTotalWeight,wbTotalNumber_Customize,wbTotalWeight_Customize,wbActualTotalWeight__Customize,wbCompany,wbStorageDate,wbStatus,wbTaxFeeUnCheckCount,wbID";
        public const string strFileds_Main = "wbSerialNum,swbSerialNum,Sender,ReceiverIDCard,ReceiverPhone,EmailAddress,swbRecipients,swbCustomsCategory,swbCustomsCategory_Desc,wbStorageDate,wbCompany,PickGoodsAgain,PickGoodsAgain_Desc,wbID,swbID";
        public const string strFileds_Sub = "swbDescription_CHN,swbDescription_ENG,swbNumber,swbWeight,swbValue,swbMonetary,TaxNo,TaxRate,mismatchCargoName,belowFullPrice,above1000,FinalCheckResultDescription,FinalHandleSuggestDescription,CheckResultOperator,swbID,swbdID";

        public const string STR_TEMPLATE_EXCEL = "~/Temp/Template/template.xls";
        public const string STR_REPORT_URL = "~/Content/Reports/CustomerCheck.rdlc";

        public const string STR_REPORT_SUBWAYBILL_MAIN_URL = "~/Content/Reports/CheckWayBillSheet_Main_Personal.rdlc";
        //
        // GET: /CustomerCheck/
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Print(string order, string page, string rows, string sort, string txtBeginDate, string txtEndDate, string ddCompany, string txtCode)
        {
            SqlParameter[] param = new SqlParameter[8];
            param[0] = new SqlParameter();
            param[0].SqlDbType = SqlDbType.VarChar;
            param[0].ParameterName = "@TableName";
            param[0].Direction = ParameterDirection.Input;
            param[0].Value = "V_Check_WayBill";

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

            txtBeginDate = Server.UrlDecode(txtBeginDate.ToString());
            txtEndDate = Server.UrlDecode(txtEndDate.ToString());
            ddCompany = Server.UrlDecode(ddCompany.ToString());
            txtCode = Server.UrlDecode(txtCode.ToString());

            string strWhereTemp = "";
            if (txtBeginDate != "" && txtEndDate != "")
            {
                if (strWhereTemp != "")
                {
                    strWhereTemp = strWhereTemp + " and (wbStorageDate>='" + Convert.ToDateTime(txtBeginDate).ToString("yyyyMMdd") + "' and wbStorageDate<='" + Convert.ToDateTime(txtEndDate).ToString("yyyyMMdd") + "')";
                }
                else
                {
                    strWhereTemp = strWhereTemp + " (wbStorageDate>='" + Convert.ToDateTime(txtBeginDate).ToString("yyyyMMdd") + "' and wbStorageDate<='" + Convert.ToDateTime(txtEndDate).ToString("yyyyMMdd") + "')";
                }
            }

            if (txtCode != "")
            {
                if (strWhereTemp != "")
                {
                    strWhereTemp = strWhereTemp + " and ( wbSerialNum like'%" + txtCode.ToString() + "%')";
                }
                else
                {
                    strWhereTemp = strWhereTemp + " ( wbSerialNum like'%" + txtCode.ToString() + "%')";
                }
            }

            if (ddCompany != "" && ddCompany != "---请选择---")
            {
                if (strWhereTemp != "")
                {
                    strWhereTemp = strWhereTemp + " and  (wbCompany like '%" + ddCompany.ToString() + "%') ";
                }
                else
                {
                    strWhereTemp = strWhereTemp + "  (wbCompany like '%" + ddCompany.ToString() + "%') ";
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
            dtCustom.Columns.Add("wbTotalNumber", Type.GetType("System.String"));
            dtCustom.Columns.Add("wbTotalWeight", Type.GetType("System.String"));
            dtCustom.Columns.Add("wbTotalNumber_Customize", Type.GetType("System.String"));
            dtCustom.Columns.Add("wbTotalWeight_Customize", Type.GetType("System.String"));
            dtCustom.Columns.Add("wbActualTotalWeight__Customize", Type.GetType("System.String"));
            dtCustom.Columns.Add("wbCompany", Type.GetType("System.String"));
            dtCustom.Columns.Add("wbStorageDate", Type.GetType("System.String"));
            dtCustom.Columns.Add("wbStatus", Type.GetType("System.String"));
            dtCustom.Columns.Add("wbID", Type.GetType("System.String"));

            DataRow drCustom = null;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                drCustom = dtCustom.NewRow();
                string[] strFiledArray = strFileds.Split(',');
                for (int j = 0; j < strFiledArray.Length; j++)
                {
                    switch (strFiledArray[j])
                    {
                        case "wbTotalNumber_Customize":
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
                            drCustom[strFiledArray[j]] = swbTotalNumber;
                            drCustom["wbTotalWeight_Customize"] = swbTotalWeight;
                            drCustom["wbActualTotalWeight__Customize"] = swbTotalActualWeight;
                            break;
                        case "wbTotalWeight_Customize":
                            break;
                        case "wbActualTotalWeight__Customize":
                            break;
                        case "wbStatus":
                            int needCheck = subWayBillSql.GetNeedCheckNum(Convert.ToInt32(dt.Rows[i]["wbID"]));
                            int status = Convert.ToInt32(dt.Rows[i]["wbStatus"]);
                            drCustom[strFiledArray[j]] = needCheck;
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
            ReportDataSource reportDataSource = new ReportDataSource("CustomerCheck_DS", dtCustom);

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
        public ActionResult Print_CheckListSheet(string strWbId, string wbStorageDate, string wbCompany, string wbSerialNum)
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
            param[3].Value = "swbID" + " " + "asc";

            param[4] = new SqlParameter();
            param[4].SqlDbType = SqlDbType.Int;
            param[4].ParameterName = "@PageSize";
            param[4].Direction = ParameterDirection.Input;
            param[4].Value = Convert.ToInt32(1000);

            param[5] = new SqlParameter();
            param[5].SqlDbType = SqlDbType.Int;
            param[5].ParameterName = "@PageCurrent";
            param[5].Direction = ParameterDirection.Input;
            param[5].Value = Convert.ToInt32(1);

            param[6] = new SqlParameter();
            param[6].SqlDbType = SqlDbType.VarChar;
            param[6].ParameterName = "@Where";
            param[6].Direction = ParameterDirection.Input;

            strWbId = Server.UrlDecode(strWbId.ToString());
            wbStorageDate = Server.UrlDecode(wbStorageDate.ToString());
            wbCompany = Server.UrlDecode(wbCompany.ToString());
            wbSerialNum = Server.UrlDecode(wbSerialNum.ToString());

            string strWhereTemp = "";
            strWhereTemp = strWhereTemp + " ( (swbNeedCheck = 1)) and (wbID=" + strWbId.ToString() + ") ";

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
            localReport.ReportPath = Server.MapPath(STR_REPORT_SUBWAYBILL_MAIN_URL);
            ReportDataSource reportDataSource = new ReportDataSource("CheckWayBillSheet_Main_Personal_DS", dtCustom);

            ReportParameter var_wbStorageDate = new ReportParameter("str_wbStorageDate", wbStorageDate.ToString());
            ReportParameter var_wbCompany = new ReportParameter("str_wbCompany", wbCompany.ToString());
            ReportParameter var_wbSerialNum = new ReportParameter("str_wbSerialNum", wbSerialNum.ToString());

            localReport.SubreportProcessing += new SubreportProcessingEventHandler(LocalReport_SubreportProcessing);

            localReport.SetParameters(new ReportParameter[] { var_wbStorageDate });
            localReport.SetParameters(new ReportParameter[] { var_wbCompany });
            localReport.SetParameters(new ReportParameter[] { var_wbSerialNum });

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

            e.DataSources.Add(new Microsoft.Reporting.WebForms.ReportDataSource("CheckWayBillSheet_Sub_Personal_DS", dtSubWayBillDetail_Sub));
        }

        [HttpGet]
        public ActionResult Excel(string order, string page, string rows, string sort, string txtBeginDate, string txtEndDate, string ddCompany, string txtCode, string browserType)
        {
            SqlParameter[] param = new SqlParameter[8];
            param[0] = new SqlParameter();
            param[0].SqlDbType = SqlDbType.VarChar;
            param[0].ParameterName = "@TableName";
            param[0].Direction = ParameterDirection.Input;
            param[0].Value = "V_Check_WayBill";

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

            txtBeginDate = Server.UrlDecode(txtBeginDate.ToString());
            txtEndDate = Server.UrlDecode(txtEndDate.ToString());
            ddCompany = Server.UrlDecode(ddCompany.ToString());
            txtCode = Server.UrlDecode(txtCode.ToString());

            string strWhereTemp = "";
            if (txtBeginDate != "" && txtEndDate != "")
            {
                if (strWhereTemp != "")
                {
                    strWhereTemp = strWhereTemp + " and (wbStorageDate>='" + Convert.ToDateTime(txtBeginDate).ToString("yyyyMMdd") + "' and wbStorageDate<='" + Convert.ToDateTime(txtEndDate).ToString("yyyyMMdd") + "')";
                }
                else
                {
                    strWhereTemp = strWhereTemp + " (wbStorageDate>='" + Convert.ToDateTime(txtBeginDate).ToString("yyyyMMdd") + "' and wbStorageDate<='" + Convert.ToDateTime(txtEndDate).ToString("yyyyMMdd") + "')";
                }
            }

            if (txtCode != "")
            {
                if (strWhereTemp != "")
                {
                    strWhereTemp = strWhereTemp + " and ( wbSerialNum like'%" + txtCode.ToString() + "%')";
                }
                else
                {
                    strWhereTemp = strWhereTemp + " ( wbSerialNum like'%" + txtCode.ToString() + "%')";
                }
            }

            if (ddCompany != "" && ddCompany != "---请选择---")
            {
                if (strWhereTemp != "")
                {
                    strWhereTemp = strWhereTemp + " and  (wbCompany like '%" + ddCompany.ToString() + "%') ";
                }
                else
                {
                    strWhereTemp = strWhereTemp + "  (wbCompany like '%" + ddCompany.ToString() + "%') ";
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
            dtCustom.Columns.Add("wbTotalNumber", Type.GetType("System.String"));
            dtCustom.Columns.Add("wbTotalWeight", Type.GetType("System.String"));
            dtCustom.Columns.Add("wbTotalNumber_Customize", Type.GetType("System.String"));
            dtCustom.Columns.Add("wbTotalWeight_Customize", Type.GetType("System.String"));
            dtCustom.Columns.Add("wbActualTotalWeight__Customize", Type.GetType("System.String"));
            dtCustom.Columns.Add("wbCompany", Type.GetType("System.String"));
            dtCustom.Columns.Add("wbStorageDate", Type.GetType("System.String"));
            dtCustom.Columns.Add("wbStatus", Type.GetType("System.String"));
            dtCustom.Columns.Add("wbID", Type.GetType("System.String"));

            DataRow drCustom = null;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                drCustom = dtCustom.NewRow();
                string[] strFiledArray = strFileds.Split(',');
                for (int j = 0; j < strFiledArray.Length; j++)
                {
                    switch (strFiledArray[j])
                    {
                        case "wbTotalNumber_Customize":
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
                            drCustom[strFiledArray[j]] = swbTotalNumber;
                            drCustom["wbTotalWeight_Customize"] = swbTotalWeight;
                            drCustom["wbActualTotalWeight__Customize"] = swbTotalActualWeight;
                            break;
                        case "wbTotalWeight_Customize":
                            break;
                        case "wbActualTotalWeight__Customize":
                            break;
                        case "wbStatus":
                            int needCheck = subWayBillSql.GetNeedCheckNum(Convert.ToInt32(dt.Rows[i]["wbID"]));
                            int status = Convert.ToInt32(dt.Rows[i]["wbStatus"]);
                            drCustom[strFiledArray[j]] = needCheck;
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
            ReportDataSource reportDataSource = new ReportDataSource("CustomerCheck_DS", dtCustom);

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

            string strOutputFileName = "货物预检信息_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";

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

        public string GetData(string order, string page, string rows, string sort, string txtBeginDate, string txtEndDate, string ddCompany, string txtCode)
        {
            SqlParameter[] param = new SqlParameter[8];
            param[0] = new SqlParameter();
            param[0].SqlDbType = SqlDbType.VarChar;
            param[0].ParameterName = "@TableName";
            param[0].Direction = ParameterDirection.Input;
            param[0].Value = "V_Check_WayBill";

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

            txtBeginDate = Server.UrlDecode(txtBeginDate.ToString());
            txtEndDate = Server.UrlDecode(txtEndDate.ToString());
            ddCompany = Server.UrlDecode(ddCompany.ToString());
            txtCode = Server.UrlDecode(txtCode.ToString());

            string strWhereTemp = "";
            if (txtBeginDate != "" && txtEndDate != "")
            {
                if (strWhereTemp != "")
                {
                    strWhereTemp = strWhereTemp + " and (wbStorageDate>='" + Convert.ToDateTime(txtBeginDate).ToString("yyyyMMdd") + "' and wbStorageDate<='" + Convert.ToDateTime(txtEndDate).ToString("yyyyMMdd") + "')";
                }
                else
                {
                    strWhereTemp = strWhereTemp + " (wbStorageDate>='" + Convert.ToDateTime(txtBeginDate).ToString("yyyyMMdd") + "' and wbStorageDate<='" + Convert.ToDateTime(txtEndDate).ToString("yyyyMMdd") + "')";
                }
            }

            if (txtCode != "")
            {
                if (strWhereTemp != "")
                {
                    strWhereTemp = strWhereTemp + " and ( wbSerialNum like'%" + txtCode.ToString() + "%')";
                }
                else
                {
                    strWhereTemp = strWhereTemp + " ( wbSerialNum like'%" + txtCode.ToString() + "%')";
                }
            }

            if (ddCompany != "" && ddCompany != "---请选择---")
            {
                if (strWhereTemp != "")
                {
                    strWhereTemp = strWhereTemp + " and  (wbCompany like '%" + ddCompany.ToString() + "%') ";
                }
                else
                {
                    strWhereTemp = strWhereTemp + "  (wbCompany like '%" + ddCompany.ToString() + "%') ";
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
                        case "wbTotalNumber_Customize":
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
                                sb.AppendFormat("\"{0}\":\"{1}\",", strFiledArray[j], swbTotalNumber);
                            }
                            else
                            {
                                sb.AppendFormat("\"{0}\":\"{1}\"", strFiledArray[j], swbTotalNumber);
                            }

                            if (j != strFiledArray.Length - 1)
                            {
                                sb.AppendFormat("\"{0}\":\"{1}公斤\",", "wbTotalWeight_Customize", swbTotalWeight);
                            }
                            else
                            {
                                sb.AppendFormat("\"{0}\":\"{1}公斤\"", "wbTotalWeight_Customize", swbTotalWeight);
                            }

                            if (j != strFiledArray.Length - 1)
                            {
                                sb.AppendFormat("\"{0}\":\"{1}公斤\",", "wbActualTotalWeight__Customize", swbTotalActualWeight);
                            }
                            else
                            {
                                sb.AppendFormat("\"{0}\":\"{1}公斤\"", "wbActualTotalWeight__Customize", swbTotalActualWeight);
                            }
                            break;
                        case "wbTotalWeight_Customize":
                            break;
                        case "wbActualTotalWeight__Customize":
                            break;
                        case "wbTaxFeeUnCheckCount":
                            Int32 iCount = 0;
                            DataSet dsTaxFeeUnCheckSubWayBillInfo = new T_SubWayBill().getAllTaxFeeUnCheckSubWayBillInfo(dt.Rows[i]["wbID"].ToString());
                            if (dsTaxFeeUnCheckSubWayBillInfo!=null)
                            {
                                DataTable dtTaxFeeUnCheckSubWayBillInfo=dsTaxFeeUnCheckSubWayBillInfo.Tables[0];
                                if (dtTaxFeeUnCheckSubWayBillInfo!=null)
                                {
                                    iCount = dtTaxFeeUnCheckSubWayBillInfo.Rows.Count;
                                }
                            }
                            if (j != strFiledArray.Length - 1)
                            {
                                sb.AppendFormat("\"{0}\":\"{1}\",", strFiledArray[j], iCount);
                            }
                            else
                            {
                                sb.AppendFormat("\"{0}\":\"{1}\"", strFiledArray[j], iCount);
                            }
                            break;
                        case "wbStatus":
                            int needCheck = subWayBillSql.GetNeedCheckNum(Convert.ToInt32(dt.Rows[i]["wbID"]));
                            int status = Convert.ToInt32(dt.Rows[i]["wbStatus"]);

                            if (j != strFiledArray.Length - 1)
                            {
                                sb.AppendFormat("\"{0}\":\"{1}${2}\",", strFiledArray[j], needCheck, status);
                            }
                            else
                            {
                                sb.AppendFormat("\"{0}\":\"{1}${2}\"", strFiledArray[j], needCheck, status);
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


        [HttpGet]
        public string upDateSwbNeedCheck(int swID, string ids, int swbNeedCheck)
        {
            string strRet = "{\"result\":\"error\",\"message\":\"提交失败，原因未知\"}";
            swID = Convert.ToInt32(Server.UrlDecode(swID.ToString()));
            ids = Server.UrlDecode(ids.ToString());
            try
            {
                if (ids == "")
                {
                    //strRet = "{\"result\":\"error\",\"message\":\"" + "没有进行过新的选择，无需提交" + "\"}";
                    strRet = "{\"result\":\"ok\",\"message\":\"" + "提交成功" + "\"}";
                }
                else
                {
                    if (ids.EndsWith("*"))
                    {
                        ids = ids.Substring(0, ids.Length - 1);
                    }

                    ids = ids.Replace("*", ",");

                    if (subWayBillSql.upDateSwbNeedCheck(swID, ids, 1, swbNeedCheck))
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
                strRet = "{\"result\":\"error\",\"message\":\"" + ex.Message + "\"}";
            }
            return strRet;
        }
    }
}
