using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using Microsoft.Reporting.WebForms;
using Model;
using SQLDAL;
using Util;

namespace CS.Controllers.Common
{
    public class PrintTaxSheetController : Controller
    {
        public const string STR_TEMPLATE_EXCEL = "~/Temp/Template/template.xls";
        public const string STR_REPORT_URL = "~/Content/Reports/PrintTaxSheet.rdlc";
        //
        // GET: /PrintTaxSheet/

        public ActionResult Index(string wbID)
        {
            ViewData["wbID"] = wbID;

            DataSet dsWayBill = null;
            DataTable dtWayBill = null;

            dsWayBill = new T_WayBill().getWayBillInfo(wbID);
            if (dsWayBill != null)
            {
                dtWayBill = dsWayBill.Tables[0];
                ViewData["wbSerialNum"] = dtWayBill.Rows[0]["wbSerialNum"].ToString();
            }

            string MaxNO = new T_PrintTaxSheetInfo().GetNextOrderNumber();

            M_TaxSheet m_TaxSheet_CustomsTax = new M_TaxSheet();
            M_TaxSheet m_TaxSheet_ValueAddedTax = new M_TaxSheet();

            ViewData["TaxSheetMaxNO"] = MaxNO;

            T_TaxSheetItems t_TaxSheetItems = new T_TaxSheetItems();
            DataSet dsTaxSheetItems = null;

            m_TaxSheet_CustomsTax.NO = "20135" + MaxNO + "A1";
            m_TaxSheet_CustomsTax.txtInComeOffice = "";
            m_TaxSheet_CustomsTax.txtSubject = "";
            m_TaxSheet_CustomsTax.txtBudgetLevels = "";
            m_TaxSheet_CustomsTax.txtRecipientTreasury = "";
            m_TaxSheet_CustomsTax.txtPaymentUnit = "";
            m_TaxSheet_CustomsTax.txtAccountNo = "";
            m_TaxSheet_CustomsTax.txtBankName = "";
            m_TaxSheet_CustomsTax.txtNumber = "";
            m_TaxSheet_CustomsTax.txtUnit = "";
            m_TaxSheet_CustomsTax.txtTaxRate = "0";
            m_TaxSheet_CustomsTax.txtFullValue = "";
            m_TaxSheet_CustomsTax.txtTaxValue = "";
            m_TaxSheet_CustomsTax.txtTotalTaxValueToUpper = "";
            m_TaxSheet_CustomsTax.txtTotalTaxValue = "";

            dsTaxSheetItems = null;
            dsTaxSheetItems = t_TaxSheetItems.GetTaxSheetItemsInfo("CustomsTaxSheet");
            if (dsTaxSheetItems != null)
            {
                DataTable dtTaxSheetItems = dsTaxSheetItems.Tables[0];
                if (dtTaxSheetItems != null && dtTaxSheetItems.Rows.Count > 0)
                {
                    for (int i = 0; i < dtTaxSheetItems.Rows.Count; i++)
                    {
                        switch (dtTaxSheetItems.Rows[i]["tssItem"].ToString())
                        {
                            case "txtInComeOffice":
                                m_TaxSheet_CustomsTax.txtInComeOffice = dtTaxSheetItems.Rows[i]["tssFormItemValue"].ToString();
                                break;
                            case "txtSubject":
                                m_TaxSheet_CustomsTax.txtSubject = dtTaxSheetItems.Rows[i]["tssFormItemValue"].ToString();
                                break;
                            case "txtBudgetLevels":
                                m_TaxSheet_CustomsTax.txtBudgetLevels = dtTaxSheetItems.Rows[i]["tssFormItemValue"].ToString();
                                break;
                            case "txtRecipientTreasury":
                                m_TaxSheet_CustomsTax.txtRecipientTreasury = dtTaxSheetItems.Rows[i]["tssFormItemValue"].ToString();
                                break;
                            case "txtPaymentUnit":
                                m_TaxSheet_CustomsTax.txtPaymentUnit = dtTaxSheetItems.Rows[i]["tssFormItemValue"].ToString();
                                break;
                            case "txtAccountNo":
                                m_TaxSheet_CustomsTax.txtAccountNo = dtTaxSheetItems.Rows[i]["tssFormItemValue"].ToString();
                                break;
                            case "txtBankName":
                                m_TaxSheet_CustomsTax.txtBankName = dtTaxSheetItems.Rows[i]["tssFormItemValue"].ToString();
                                break;
                            case "txtNumber":
                                m_TaxSheet_CustomsTax.txtNumber = dtTaxSheetItems.Rows[i]["tssFormItemValue"].ToString();
                                break;
                            case "txtUnit":
                                m_TaxSheet_CustomsTax.txtUnit = dtTaxSheetItems.Rows[i]["tssFormItemValue"].ToString();
                                break;
                            case "txtTaxRate":
                                m_TaxSheet_CustomsTax.txtTaxRate = dtTaxSheetItems.Rows[i]["tssFormItemValue"].ToString();
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
            //计算关税的完税价格、税金、税金金额大写
            m_TaxSheet_CustomsTax.txtFullValue = new T_SubWayBill().getSumTaxFeeCheckValue(wbID);
            m_TaxSheet_CustomsTax.txtTaxValue = (Convert.ToDouble(m_TaxSheet_CustomsTax.txtFullValue) * Convert.ToDouble(CommonHelper.PerctangleToDecimal(m_TaxSheet_CustomsTax.txtTaxRate))).ToString("0.00");
            m_TaxSheet_CustomsTax.txtTotalTaxValueToUpper = CurrencyTool.NumGetStr(Convert.ToDouble(m_TaxSheet_CustomsTax.txtTaxValue));
            m_TaxSheet_CustomsTax.txtTotalTaxValue = m_TaxSheet_CustomsTax.txtTaxValue;

            m_TaxSheet_ValueAddedTax.NO = "20135" + MaxNO + "L2";
            m_TaxSheet_ValueAddedTax.txtInComeOffice = "";
            m_TaxSheet_ValueAddedTax.txtSubject = "";
            m_TaxSheet_ValueAddedTax.txtBudgetLevels = "";
            m_TaxSheet_ValueAddedTax.txtRecipientTreasury = "";
            m_TaxSheet_ValueAddedTax.txtPaymentUnit = "";
            m_TaxSheet_ValueAddedTax.txtAccountNo = "";
            m_TaxSheet_ValueAddedTax.txtBankName = "";
            m_TaxSheet_ValueAddedTax.txtNumber = "";
            m_TaxSheet_ValueAddedTax.txtUnit = "";
            m_TaxSheet_ValueAddedTax.txtTaxRate = "0";
            m_TaxSheet_ValueAddedTax.txtFullValue = "";
            m_TaxSheet_ValueAddedTax.txtTaxValue = "";
            m_TaxSheet_ValueAddedTax.txtTotalTaxValueToUpper = "";
            m_TaxSheet_ValueAddedTax.txtTotalTaxValue = "";

            dsTaxSheetItems = null;
            dsTaxSheetItems = t_TaxSheetItems.GetTaxSheetItemsInfo("ValueAddedTaxSheet");
            if (dsTaxSheetItems != null)
            {
                DataTable dtTaxSheetItems = dsTaxSheetItems.Tables[0];
                if (dtTaxSheetItems != null && dtTaxSheetItems.Rows.Count > 0)
                {
                    for (int i = 0; i < dtTaxSheetItems.Rows.Count; i++)
                    {
                        switch (dtTaxSheetItems.Rows[i]["tssItem"].ToString())
                        {
                            case "txtInComeOffice":
                                m_TaxSheet_ValueAddedTax.txtInComeOffice = dtTaxSheetItems.Rows[i]["tssFormItemValue"].ToString();
                                break;
                            case "txtSubject":
                                m_TaxSheet_ValueAddedTax.txtSubject = dtTaxSheetItems.Rows[i]["tssFormItemValue"].ToString();
                                break;
                            case "txtBudgetLevels":
                                m_TaxSheet_ValueAddedTax.txtBudgetLevels = dtTaxSheetItems.Rows[i]["tssFormItemValue"].ToString();
                                break;
                            case "txtRecipientTreasury":
                                m_TaxSheet_ValueAddedTax.txtRecipientTreasury = dtTaxSheetItems.Rows[i]["tssFormItemValue"].ToString();
                                break;
                            case "txtPaymentUnit":
                                m_TaxSheet_ValueAddedTax.txtPaymentUnit = dtTaxSheetItems.Rows[i]["tssFormItemValue"].ToString();
                                break;
                            case "txtAccountNo":
                                m_TaxSheet_ValueAddedTax.txtAccountNo = dtTaxSheetItems.Rows[i]["tssFormItemValue"].ToString();
                                break;
                            case "txtBankName":
                                m_TaxSheet_ValueAddedTax.txtBankName = dtTaxSheetItems.Rows[i]["tssFormItemValue"].ToString();
                                break;
                            case "txtNumber":
                                m_TaxSheet_ValueAddedTax.txtNumber = dtTaxSheetItems.Rows[i]["tssFormItemValue"].ToString();
                                break;
                            case "txtUnit":
                                m_TaxSheet_ValueAddedTax.txtUnit = dtTaxSheetItems.Rows[i]["tssFormItemValue"].ToString();
                                break;
                            case "txtTaxRate":
                                m_TaxSheet_ValueAddedTax.txtTaxRate = dtTaxSheetItems.Rows[i]["tssFormItemValue"].ToString();
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
            //计算增值税的完税价格、税金、税金金额大写
            m_TaxSheet_ValueAddedTax.txtFullValue = m_TaxSheet_CustomsTax.txtFullValue;
            m_TaxSheet_ValueAddedTax.txtTaxValue = (Convert.ToDouble(m_TaxSheet_ValueAddedTax.txtFullValue) * Convert.ToDouble(CommonHelper.PerctangleToDecimal(m_TaxSheet_ValueAddedTax.txtTaxRate))).ToString("0.00");
            m_TaxSheet_ValueAddedTax.txtTotalTaxValueToUpper = CurrencyTool.NumGetStr(Convert.ToDouble(m_TaxSheet_ValueAddedTax.txtTaxValue));
            m_TaxSheet_ValueAddedTax.txtTotalTaxValue = m_TaxSheet_ValueAddedTax.txtTaxValue;

            ViewBag.CustomsTax_FormItems = m_TaxSheet_CustomsTax;
            ViewBag.ValueAddedTax_FormItems = m_TaxSheet_ValueAddedTax;
            //ViewBag["CustomsTax_FormItems"] = m_TaxSheet_CustomsTax;
            //ViewBag["ValueAddedTax_FormItems"] = m_TaxSheet_ValueAddedTax;

            return View();
        }

        public ActionResult Print(string wbID, string sIDC, string sNC,
            string tICOC, string tSC,
            string tBLC, string tRTC,
            string tPUC, string tANC,
            string tBNC, string tTNC,
            string tDCC, string tNC,
            string tUC, string tFVC, string tTRC,
            string tTVC, string tTTVTUC,
            string tTTVC, string tACNC,
            string tBNEC, string tCNC,
            string tTTC, string tELOPC,
            string tPGNC, string hdMC,
            string tTMC, string tTCC,
            string tNTC,
            string sIDV, string sNV,
            string tICOV, string tSV,
            string tBLV, string tRTV,
            string tPUV, string tANV,
            string tBNV, string tTNV,
            string tDCV, string tNV,
            string tUV, string tFVV, string tTRV,
            string tTVV, string tTTVTUV,
            string tTTVV, string tACNV,
            string tBNEV, string tCNV,
            string tTTV, string tELOPV,
            string tPGNV, string hdMV,
            string tTMV, string tTCV,
            string tNTV)
        {
            DataSet dsAllUnReleaseSubWayBill = new DataSet();
            //dsAllUnReleaseSubWayBill = SqlServerHelper.Query(string.Format(" select swbSerialNum,swbDescription_CHN,swbDescription_ENG,swbNumber,swbWeight,convert(nvarchar(10),DetainDate,120) DetainDate from  V_WayBill_SubWayBill where wbID={0} and swbNeedCheck=3 ", strWBID));
            string str_WbID = wbID == null ? "" : Server.UrlDecode(wbID);

            DataSet dsWayBill = null;
            DataTable dtWayBill = null;

            dsWayBill = new T_WayBill().getWayBillInfo(wbID);
            if (dsWayBill != null)
            {
                dtWayBill = dsWayBill.Tables[0];
            }

            string str_span_IssuanceDate_CustomsTax = sIDC == null ? "" : Server.UrlDecode(sIDC);
            string str_span_NO_CustomsTax = sNC == null ? "" : Server.UrlDecode(sNC);
            string str_txtInComeOffice_CustomsTax = tICOC == null ? "" : Server.UrlDecode(tICOC);
            string str_txtSubject_CustomsTax = tSC == null ? "" : Server.UrlDecode(tSC);
            string str_txtBudgetLevels_CustomsTax = tBLC == null ? "" : Server.UrlDecode(tBLC);
            string str_txtRecipientTreasury_CustomsTax = tRTC == null ? "" : Server.UrlDecode(tRTC);
            string str_txtPaymentUnit_CustomsTax = tPUC == null ? "" : Server.UrlDecode(tPUC);
            string str_txtAccountNo_CustomsTax = tANC == null ? "" : Server.UrlDecode(tANC);
            string str_txtBankName_CustomsTax = tBNC == null ? "" : Server.UrlDecode(tBNC);
            string str_txtTaxNo_CustomsTax = tTNC == null ? "" : Server.UrlDecode(tTNC);
            string str_txtDescription_CHN_CustomsTax = tDCC == null ? "" : Server.UrlDecode(tDCC);
            string str_txtNumber_CustomsTax = tNC == null ? "" : Server.UrlDecode(tNC);
            string str_txtUnit_CustomsTax = tUC == null ? "" : Server.UrlDecode(tUC);
            string str_txtFullValue_CustomsTax = tFVC == null ? "" : Server.UrlDecode(tFVC);
            string str_txtTaxRate_CustomsTax = tTRC == null ? "" : Server.UrlDecode(tTRC);
            string str_txtTaxValue_CustomsTax = tTVC == null ? "" : Server.UrlDecode(tTVC);
            string str_txtTotalTaxValueToUpper_CustomsTax = tTTVTUC == null ? "" : Server.UrlDecode(tTTVTUC);
            string str_txtTotalTaxValue_CustomsTax = tTTVC == null ? "" : Server.UrlDecode(tTTVC);
            string str_txtApplyCompanyNo_CustomsTax = tACNC == null ? "" : Server.UrlDecode(tACNC);
            string str_txtBillNOofEntry_CustomsTax = tBNEC == null ? "" : Server.UrlDecode(tBNEC);
            string str_txtContractNO_CustomsTax = tCNC == null ? "" : Server.UrlDecode(tCNC);
            string str_txtTransportTools_CustomsTax = tTTC == null ? "" : Server.UrlDecode(tTTC);
            string str_txtEndLineOfPay_CustomsTax = tELOPC == null ? "" : Server.UrlDecode(tELOPC);
            string str_txtPickGoodsNO_CustomsTax = tPGNC == null ? "" : Server.UrlDecode(tPGNC);
            string str_hd_taMemo_CustomsTax = hdMC == null ? "" : Server.UrlDecode(hdMC);
            string str_txtTableMaker_CustomsTax = tTMC == null ? "" : Server.UrlDecode(tTMC);
            string str_txtTableChecker_CustomsTax = tTCC == null ? "" : Server.UrlDecode(tTCC);
            string str_txtNationTreasury_CustomsTax = tNTC == null ? "" : Server.UrlDecode(tNTC);

            //处理货币大小写
            if (str_txtTotalTaxValueToUpper_CustomsTax == "")
            {
                if (str_txtTotalTaxValue_CustomsTax != "")
                {
                    str_txtTotalTaxValueToUpper_CustomsTax = CurrencyTool.NumGetStr(Convert.ToDouble(str_txtTotalTaxValue_CustomsTax));
                }
            }

            //处理备注栏
            if (str_hd_taMemo_CustomsTax == "")
            {
                if (dtWayBill != null && dtWayBill.Rows.Count > 0)
                {
                    str_hd_taMemo_CustomsTax = dtWayBill.Rows[0]["wbSerialNum"].ToString();
                }
            }

            string str_span_IssuanceDate_ValueAddedTax = sIDV == null ? "" : Server.UrlDecode(sIDV);
            string str_span_NO_ValueAddedTax = sNV == null ? "" : Server.UrlDecode(sNV);
            string str_txtInComeOffice_ValueAddedTax = tICOV == null ? "" : Server.UrlDecode(tICOV);
            string str_txtSubject_ValueAddedTax = tSV == null ? "" : Server.UrlDecode(tSV);
            string str_txtBudgetLevels_ValueAddedTax = tBLV == null ? "" : Server.UrlDecode(tBLV);
            string str_txtRecipientTreasury_ValueAddedTax = tRTV == null ? "" : Server.UrlDecode(tRTV);
            string str_txtPaymentUnit_ValueAddedTax = tPUV == null ? "" : Server.UrlDecode(tPUV);
            string str_txtAccountNo_ValueAddedTax = tANV == null ? "" : Server.UrlDecode(tANV);
            string str_txtBankName_ValueAddedTax = tBNV == null ? "" : Server.UrlDecode(tBNV);
            string str_txtTaxNo_ValueAddedTax = tTNV == null ? "" : Server.UrlDecode(tTNV);
            string str_txtDescription_CHN_ValueAddedTax = tDCV == null ? "" : Server.UrlDecode(tDCV);
            string str_txtNumber_ValueAddedTax = tNV == null ? "" : Server.UrlDecode(tNV);
            string str_txtUnit_ValueAddedTax = tUV == null ? "" : Server.UrlDecode(tUV);
            string str_txtFullValue_ValueAddedTax = tFVV == null ? "" : Server.UrlDecode(tFVV);
            string str_txtTaxRate_ValueAddedTax = tTRV == null ? "" : Server.UrlDecode(tTRV);
            string str_txtTaxValue_ValueAddedTax = tTVV == null ? "" : Server.UrlDecode(tTVV);
            string str_txtTotalTaxValueToUpper_ValueAddedTax = tTTVTUV == null ? "" : Server.UrlDecode(tTTVTUV);
            string str_txtTotalTaxValue_ValueAddedTax = tTTVV == null ? "" : Server.UrlDecode(tTTVV);
            string str_txtApplyCompanyNo_ValueAddedTax = tACNV == null ? "" : Server.UrlDecode(tACNV);
            string str_txtBillNOofEntry_ValueAddedTax = tBNEV == null ? "" : Server.UrlDecode(tBNEV);
            string str_txtContractNO_ValueAddedTax = tCNV == null ? "" : Server.UrlDecode(tCNV);
            string str_txtTransportTools_ValueAddedTax = tTTV == null ? "" : Server.UrlDecode(tTTV);
            string str_txtEndLineOfPay_ValueAddedTax = tELOPV == null ? "" : Server.UrlDecode(tELOPV);
            string str_txtPickGoodsNO_ValueAddedTax = tPGNV == null ? "" : Server.UrlDecode(tPGNV);
            string str_hd_taMemo_ValueAddedTax = hdMV == null ? "" : Server.UrlDecode(hdMV);
            string str_txtTableMaker_ValueAddedTax = tTMV == null ? "" : Server.UrlDecode(tTMV);
            string str_txtTableChecker_ValueAddedTax = tTCV == null ? "" : Server.UrlDecode(tTCV);
            string str_txtNationTreasury_ValueAddedTax = tNTV == null ? "" : Server.UrlDecode(tNTV);

            //处理货币大小写
            if (str_txtTotalTaxValueToUpper_ValueAddedTax == "")
            {
                if (str_txtTotalTaxValue_ValueAddedTax != "")
                {
                    str_txtTotalTaxValueToUpper_ValueAddedTax = CurrencyTool.NumGetStr(Convert.ToDouble(str_txtTotalTaxValue_ValueAddedTax));
                }
            }

            //处理备注栏
            if (str_hd_taMemo_ValueAddedTax == "")
            {
                if (dtWayBill != null && dtWayBill.Rows.Count > 0)
                {
                    str_hd_taMemo_ValueAddedTax = dtWayBill.Rows[0]["wbSerialNum"].ToString();
                }
            }

            LocalReport localReport = new LocalReport();
            localReport.ReportPath = Server.MapPath(STR_REPORT_URL);
            //ReportDataSource dtAllUnReleaseSubWayBill = new ReportDataSource("FirstPickGoodsSheetSetting_DS", dsAllUnReleaseSubWayBill.Tables[0]);

            ReportParameter var_span_IssuanceDate_CustomsTax = new ReportParameter("span_IssuanceDate_CustomsTax", str_span_IssuanceDate_CustomsTax.ToString());
            ReportParameter var_span_NO_CustomsTax = new ReportParameter("span_NO_CustomsTax", str_span_NO_CustomsTax.ToString());
            ReportParameter var_txtInComeOffice_CustomsTax = new ReportParameter("txtInComeOffice_CustomsTax", str_txtInComeOffice_CustomsTax.ToString());
            ReportParameter var_txtSubject_CustomsTax = new ReportParameter("txtSubject_CustomsTax", str_txtSubject_CustomsTax.ToString());
            ReportParameter var_txtBudgetLevels_CustomsTax = new ReportParameter("txtBudgetLevels_CustomsTax", str_txtBudgetLevels_CustomsTax.ToString());
            ReportParameter var_txtRecipientTreasury_CustomsTax = new ReportParameter("txtRecipientTreasury_CustomsTax", str_txtRecipientTreasury_CustomsTax.ToString());
            ReportParameter var_txtPaymentUnit_CustomsTax = new ReportParameter("txtPaymentUnit_CustomsTax", str_txtPaymentUnit_CustomsTax.ToString());
            ReportParameter var_txtAccountNo_CustomsTax = new ReportParameter("txtAccountNo_CustomsTax", str_txtAccountNo_CustomsTax.ToString());
            ReportParameter var_txtBankName_CustomsTax = new ReportParameter("txtBankName_CustomsTax", str_txtBankName_CustomsTax.ToString());
            ReportParameter var_txtTaxNo_CustomsTax = new ReportParameter("txtTaxNo_CustomsTax", str_txtTaxNo_CustomsTax.ToString());
            ReportParameter var_txtDescription_CHN_CustomsTax = new ReportParameter("txtDescription_CHN_CustomsTax", str_txtDescription_CHN_CustomsTax.ToString());
            ReportParameter var_txtNumber_CustomsTax = new ReportParameter("txtNumber_CustomsTax", str_txtNumber_CustomsTax.ToString());
            ReportParameter var_txtUnit_CustomsTax = new ReportParameter("txtUnit_CustomsTax", str_txtUnit_CustomsTax.ToString());
            ReportParameter var_txtFullValue_CustomsTax = new ReportParameter("txtFullValue_CustomsTax", str_txtFullValue_CustomsTax.ToString());
            ReportParameter var_txtTaxRate_CustomsTax = new ReportParameter("txtTaxRate_CustomsTax", str_txtTaxRate_CustomsTax.ToString());
            ReportParameter var_txtTaxValue_CustomsTax = new ReportParameter("txtTaxValue_CustomsTax", str_txtTaxValue_CustomsTax.ToString());
            ReportParameter var_txtTotalTaxValueToUpper_CustomsTax = new ReportParameter("txtTotalTaxValueToUpper_CustomsTax", str_txtTotalTaxValueToUpper_CustomsTax.ToString());
            ReportParameter var_txtTotalTaxValue_CustomsTax = new ReportParameter("txtTotalTaxValue_CustomsTax", str_txtTotalTaxValue_CustomsTax.ToString());
            ReportParameter var_txtApplyCompanyNo_CustomsTax = new ReportParameter("txtApplyCompanyNo_CustomsTax", str_txtApplyCompanyNo_CustomsTax.ToString());
            ReportParameter var_txtBillNOofEntry_CustomsTax = new ReportParameter("txtBillNOofEntry_CustomsTax", str_txtBillNOofEntry_CustomsTax.ToString());
            ReportParameter var_txtContractNO_CustomsTax = new ReportParameter("txtContractNO_CustomsTax", str_txtContractNO_CustomsTax.ToString());
            ReportParameter var_txtTransportTools_CustomsTax = new ReportParameter("txtTransportTools_CustomsTax", str_txtTransportTools_CustomsTax.ToString());
            ReportParameter var_txtEndLineOfPay_CustomsTax = new ReportParameter("txtEndLineOfPay_CustomsTax", str_txtEndLineOfPay_CustomsTax.ToString());
            ReportParameter var_txtPickGoodsNO_CustomsTax = new ReportParameter("txtPickGoodsNO_CustomsTax", str_txtPickGoodsNO_CustomsTax.ToString());
            ReportParameter var_hd_taMemo_CustomsTax = new ReportParameter("hd_taMemo_CustomsTax", str_hd_taMemo_CustomsTax.ToString());
            ReportParameter var_txtTableMaker_CustomsTax = new ReportParameter("txtTableMaker_CustomsTax", str_txtTableMaker_CustomsTax.ToString());
            ReportParameter var_txtTableChecker_CustomsTax = new ReportParameter("txtTableChecker_CustomsTax", str_txtTableChecker_CustomsTax.ToString());
            ReportParameter var_txtNationTreasury_CustomsTax = new ReportParameter("txtNationTreasury_CustomsTax", str_txtNationTreasury_CustomsTax.ToString());

            ReportParameter var_span_IssuanceDate_ValueAddedTax = new ReportParameter("span_IssuanceDate_ValueAddedTax", str_span_IssuanceDate_ValueAddedTax.ToString());
            ReportParameter var_span_NO_ValueAddedTax = new ReportParameter("span_NO_ValueAddedTax", str_span_NO_ValueAddedTax.ToString());
            ReportParameter var_txtInComeOffice_ValueAddedTax = new ReportParameter("txtInComeOffice_ValueAddedTax", str_txtInComeOffice_ValueAddedTax.ToString());
            ReportParameter var_txtSubject_ValueAddedTax = new ReportParameter("txtSubject_ValueAddedTax", str_txtSubject_ValueAddedTax.ToString());
            ReportParameter var_txtBudgetLevels_ValueAddedTax = new ReportParameter("txtBudgetLevels_ValueAddedTax", str_txtBudgetLevels_ValueAddedTax.ToString());
            ReportParameter var_txtRecipientTreasury_ValueAddedTax = new ReportParameter("txtRecipientTreasury_ValueAddedTax", str_txtRecipientTreasury_ValueAddedTax.ToString());
            ReportParameter var_txtPaymentUnit_ValueAddedTax = new ReportParameter("txtPaymentUnit_ValueAddedTax", str_txtPaymentUnit_ValueAddedTax.ToString());
            ReportParameter var_txtAccountNo_ValueAddedTax = new ReportParameter("txtAccountNo_ValueAddedTax", str_txtAccountNo_ValueAddedTax.ToString());
            ReportParameter var_txtBankName_ValueAddedTax = new ReportParameter("txtBankName_ValueAddedTax", str_txtBankName_ValueAddedTax.ToString());
            ReportParameter var_txtTaxNo_ValueAddedTax = new ReportParameter("txtTaxNo_ValueAddedTax", str_txtTaxNo_ValueAddedTax.ToString());
            ReportParameter var_txtDescription_CHN_ValueAddedTax = new ReportParameter("txtDescription_CHN_ValueAddedTax", str_txtDescription_CHN_ValueAddedTax.ToString());
            ReportParameter var_txtNumber_ValueAddedTax = new ReportParameter("txtNumber_ValueAddedTax", str_txtNumber_ValueAddedTax.ToString());
            ReportParameter var_txtUnit_ValueAddedTax = new ReportParameter("txtUnit_ValueAddedTax", str_txtUnit_ValueAddedTax.ToString());
            ReportParameter var_txtFullValue_ValueAddedTax = new ReportParameter("txtFullValue_ValueAddedTax", str_txtFullValue_ValueAddedTax.ToString());
            ReportParameter var_txtTaxRate_ValueAddedTax = new ReportParameter("txtTaxRate_ValueAddedTax", str_txtTaxRate_ValueAddedTax.ToString());
            ReportParameter var_txtTaxValue_ValueAddedTax = new ReportParameter("txtTaxValue_ValueAddedTax", str_txtTaxValue_ValueAddedTax.ToString());
            ReportParameter var_txtTotalTaxValueToUpper_ValueAddedTax = new ReportParameter("txtTotalTaxValueToUpper_ValueAddedTax", str_txtTotalTaxValueToUpper_ValueAddedTax.ToString());
            ReportParameter var_txtTotalTaxValue_ValueAddedTax = new ReportParameter("txtTotalTaxValue_ValueAddedTax", str_txtTotalTaxValue_ValueAddedTax.ToString());
            ReportParameter var_txtApplyCompanyNo_ValueAddedTax = new ReportParameter("txtApplyCompanyNo_ValueAddedTax", str_txtApplyCompanyNo_ValueAddedTax.ToString());
            ReportParameter var_txtBillNOofEntry_ValueAddedTax = new ReportParameter("txtBillNOofEntry_ValueAddedTax", str_txtBillNOofEntry_ValueAddedTax.ToString());
            ReportParameter var_txtContractNO_ValueAddedTax = new ReportParameter("txtContractNO_ValueAddedTax", str_txtContractNO_ValueAddedTax.ToString());
            ReportParameter var_txtTransportTools_ValueAddedTax = new ReportParameter("txtTransportTools_ValueAddedTax", str_txtTransportTools_ValueAddedTax.ToString());
            ReportParameter var_txtEndLineOfPay_ValueAddedTax = new ReportParameter("txtEndLineOfPay_ValueAddedTax", str_txtEndLineOfPay_ValueAddedTax.ToString());
            ReportParameter var_txtPickGoodsNO_ValueAddedTax = new ReportParameter("txtPickGoodsNO_ValueAddedTax", str_txtPickGoodsNO_ValueAddedTax.ToString());
            ReportParameter var_hd_taMemo_ValueAddedTax = new ReportParameter("hd_taMemo_ValueAddedTax", str_hd_taMemo_ValueAddedTax.ToString());
            ReportParameter var_txtTableMaker_ValueAddedTax = new ReportParameter("txtTableMaker_ValueAddedTax", str_txtTableMaker_ValueAddedTax.ToString());
            ReportParameter var_txtTableChecker_ValueAddedTax = new ReportParameter("txtTableChecker_ValueAddedTax", str_txtTableChecker_ValueAddedTax.ToString());
            ReportParameter var_txtNationTreasury_ValueAddedTax = new ReportParameter("txtNationTreasury_ValueAddedTax", str_txtNationTreasury_ValueAddedTax.ToString());

            localReport.SetParameters(new ReportParameter[] { var_span_IssuanceDate_CustomsTax });
            localReport.SetParameters(new ReportParameter[] { var_span_NO_CustomsTax });
            localReport.SetParameters(new ReportParameter[] { var_txtInComeOffice_CustomsTax });
            localReport.SetParameters(new ReportParameter[] { var_txtSubject_CustomsTax });
            localReport.SetParameters(new ReportParameter[] { var_txtBudgetLevels_CustomsTax });
            localReport.SetParameters(new ReportParameter[] { var_txtRecipientTreasury_CustomsTax });
            localReport.SetParameters(new ReportParameter[] { var_txtPaymentUnit_CustomsTax });
            localReport.SetParameters(new ReportParameter[] { var_txtAccountNo_CustomsTax });
            localReport.SetParameters(new ReportParameter[] { var_txtBankName_CustomsTax });
            localReport.SetParameters(new ReportParameter[] { var_txtTaxNo_CustomsTax });
            localReport.SetParameters(new ReportParameter[] { var_txtDescription_CHN_CustomsTax });
            localReport.SetParameters(new ReportParameter[] { var_txtNumber_CustomsTax });
            localReport.SetParameters(new ReportParameter[] { var_txtUnit_CustomsTax });
            localReport.SetParameters(new ReportParameter[] { var_txtFullValue_CustomsTax });
            localReport.SetParameters(new ReportParameter[] { var_txtTaxRate_CustomsTax });
            localReport.SetParameters(new ReportParameter[] { var_txtTaxValue_CustomsTax });
            localReport.SetParameters(new ReportParameter[] { var_txtTotalTaxValueToUpper_CustomsTax });
            localReport.SetParameters(new ReportParameter[] { var_txtTotalTaxValue_CustomsTax });
            localReport.SetParameters(new ReportParameter[] { var_txtApplyCompanyNo_CustomsTax });
            localReport.SetParameters(new ReportParameter[] { var_txtBillNOofEntry_CustomsTax });
            localReport.SetParameters(new ReportParameter[] { var_txtContractNO_CustomsTax });
            localReport.SetParameters(new ReportParameter[] { var_txtTransportTools_CustomsTax });
            localReport.SetParameters(new ReportParameter[] { var_txtEndLineOfPay_CustomsTax });
            localReport.SetParameters(new ReportParameter[] { var_txtPickGoodsNO_CustomsTax });
            localReport.SetParameters(new ReportParameter[] { var_hd_taMemo_CustomsTax });
            localReport.SetParameters(new ReportParameter[] { var_txtTableMaker_CustomsTax });
            localReport.SetParameters(new ReportParameter[] { var_txtTableChecker_CustomsTax });
            localReport.SetParameters(new ReportParameter[] { var_txtNationTreasury_CustomsTax });

            localReport.SetParameters(new ReportParameter[] { var_span_IssuanceDate_ValueAddedTax });
            localReport.SetParameters(new ReportParameter[] { var_span_NO_ValueAddedTax });
            localReport.SetParameters(new ReportParameter[] { var_txtInComeOffice_ValueAddedTax });
            localReport.SetParameters(new ReportParameter[] { var_txtSubject_ValueAddedTax });
            localReport.SetParameters(new ReportParameter[] { var_txtBudgetLevels_ValueAddedTax });
            localReport.SetParameters(new ReportParameter[] { var_txtRecipientTreasury_ValueAddedTax });
            localReport.SetParameters(new ReportParameter[] { var_txtPaymentUnit_ValueAddedTax });
            localReport.SetParameters(new ReportParameter[] { var_txtAccountNo_ValueAddedTax });
            localReport.SetParameters(new ReportParameter[] { var_txtBankName_ValueAddedTax });
            localReport.SetParameters(new ReportParameter[] { var_txtTaxNo_ValueAddedTax });
            localReport.SetParameters(new ReportParameter[] { var_txtDescription_CHN_ValueAddedTax });
            localReport.SetParameters(new ReportParameter[] { var_txtNumber_ValueAddedTax });
            localReport.SetParameters(new ReportParameter[] { var_txtUnit_ValueAddedTax });
            localReport.SetParameters(new ReportParameter[] { var_txtFullValue_ValueAddedTax });
            localReport.SetParameters(new ReportParameter[] { var_txtTaxRate_ValueAddedTax });
            localReport.SetParameters(new ReportParameter[] { var_txtTaxValue_ValueAddedTax });
            localReport.SetParameters(new ReportParameter[] { var_txtTotalTaxValueToUpper_ValueAddedTax });
            localReport.SetParameters(new ReportParameter[] { var_txtTotalTaxValue_ValueAddedTax });
            localReport.SetParameters(new ReportParameter[] { var_txtApplyCompanyNo_ValueAddedTax });
            localReport.SetParameters(new ReportParameter[] { var_txtBillNOofEntry_ValueAddedTax });
            localReport.SetParameters(new ReportParameter[] { var_txtContractNO_ValueAddedTax });
            localReport.SetParameters(new ReportParameter[] { var_txtTransportTools_ValueAddedTax });
            localReport.SetParameters(new ReportParameter[] { var_txtEndLineOfPay_ValueAddedTax });
            localReport.SetParameters(new ReportParameter[] { var_txtPickGoodsNO_ValueAddedTax });
            localReport.SetParameters(new ReportParameter[] { var_hd_taMemo_ValueAddedTax });
            localReport.SetParameters(new ReportParameter[] { var_txtTableMaker_ValueAddedTax });
            localReport.SetParameters(new ReportParameter[] { var_txtTableChecker_ValueAddedTax });
            localReport.SetParameters(new ReportParameter[] { var_txtNationTreasury_ValueAddedTax });

            //localReport.DataSources.Add(dtAllUnReleaseSubWayBill);
            string reportType = "PDF";
            string mimeType;
            string encoding = "UTF-8";
            string fileNameExtension;

            string deviceInfo = "<DeviceInfo>" +
                " <OutputFormat>PDF</OutputFormat>" +
                " <PageWidth>11in</PageWidth>" +
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

        [HttpPost]
        public string AddPrintTaxSheetInfo(string oldOrderNumber)
        {
            string strRet = "{\"result\":\"error\",\"message\":\"保存税表信息失败,不允许继续打印或导出,原因未知\"}";
            string strUserName = "";
            M_PrintTaxSheetInfo m_PrintTaxSheetInfo = null;

            oldOrderNumber = Server.UrlDecode(oldOrderNumber);
            strUserName = Session["Global_Customer_UserName"] == null ? "" : Session["Global_Customer_UserName"].ToString();

            try
            {
                m_PrintTaxSheetInfo = new M_PrintTaxSheetInfo(); ;

                m_PrintTaxSheetInfo.PrintDate = DateTime.Now;
                m_PrintTaxSheetInfo.OrderNumber = Convert.ToInt32(oldOrderNumber);
                m_PrintTaxSheetInfo.Operator = strUserName;
                m_PrintTaxSheetInfo.mMemo = "";

                if (new T_PrintTaxSheetInfo().addPrintTaxSheetInfo(m_PrintTaxSheetInfo))
                {
                    strRet = "{\"result\":\"ok\",\"message\":\"保存税表信息成功,开始打印或导出\"}";
                }

            }
            catch (Exception ex)
            {
                strRet = "{\"result\":\"error\",\"message\":\"保存税表信息失败,不允许继续打印或导出,原因:" + ex.Message + "\"}";
            }

            return strRet;
        }

        [HttpPost]
        public string CurrencyToUpper(string TaxValue)
        {
            string strRet = "";
            TaxValue = Server.UrlDecode(TaxValue);
            try
            {
                strRet = CurrencyTool.NumGetStr(Convert.ToDouble(TaxValue));
            }
            catch (Exception ex)
            {

            }

            return strRet;
        }
    }
}
