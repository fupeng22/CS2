using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model;
using SQLDAL;
using System.Data;

namespace CS.Controllers.Common
{
    public class CustomsTaxSheetSettingController : Controller
    {
        //
        // GET: /CustomsTaxSheetSetting/

        public ActionResult Index()
        {
            T_TaxSheetItems t_TaxSheetItems = new T_TaxSheetItems();
            M_TaxSheet m_TaxSheet = new M_TaxSheet();
            m_TaxSheet.txtInComeOffice = "";
            m_TaxSheet.txtSubject = "";
            m_TaxSheet.txtBudgetLevels = "";
            m_TaxSheet.txtRecipientTreasury = "";
            m_TaxSheet.txtPaymentUnit = "";
            m_TaxSheet.txtAccountNo = "";
            m_TaxSheet.txtBankName = "";
            m_TaxSheet.txtNumber = "";
            m_TaxSheet.txtUnit = "";
            m_TaxSheet.txtTaxRate = "";

            DataSet dsTaxSheetItems = t_TaxSheetItems.GetTaxSheetItemsInfo("CustomsTaxSheet");
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
                                m_TaxSheet.txtInComeOffice = dtTaxSheetItems.Rows[i]["tssFormItemValue"].ToString();
                                break;
                            case "txtSubject":
                                m_TaxSheet.txtSubject = dtTaxSheetItems.Rows[i]["tssFormItemValue"].ToString();
                                break;
                            case "txtBudgetLevels":
                                m_TaxSheet.txtBudgetLevels = dtTaxSheetItems.Rows[i]["tssFormItemValue"].ToString();
                                break;
                            case "txtRecipientTreasury":
                                m_TaxSheet.txtRecipientTreasury = dtTaxSheetItems.Rows[i]["tssFormItemValue"].ToString();
                                break;
                            case "txtPaymentUnit":
                                m_TaxSheet.txtPaymentUnit = dtTaxSheetItems.Rows[i]["tssFormItemValue"].ToString();
                                break;
                            case "txtAccountNo":
                                m_TaxSheet.txtAccountNo = dtTaxSheetItems.Rows[i]["tssFormItemValue"].ToString();
                                break;
                            case "txtBankName":
                                m_TaxSheet.txtBankName = dtTaxSheetItems.Rows[i]["tssFormItemValue"].ToString();
                                break;
                            case "txtNumber":
                                m_TaxSheet.txtNumber = dtTaxSheetItems.Rows[i]["tssFormItemValue"].ToString();
                                break;
                            case "txtUnit":
                                m_TaxSheet.txtUnit = dtTaxSheetItems.Rows[i]["tssFormItemValue"].ToString();
                                break;
                            case "txtTaxRate":
                                m_TaxSheet.txtTaxRate = dtTaxSheetItems.Rows[i]["tssFormItemValue"].ToString();
                                break;
                            default:
                                break;
                        }
                    }
                }
            }

            return View(m_TaxSheet);
        }


        [HttpPost]
        public string SaveData(FormCollection collection)
        {
            string strRet = "{\"result\":\"error\",\"\":\"保存失败，原因未知\"}";
            string strKind="";
            M_TaxSheet m_TaxSheet = new M_TaxSheet();
            try
            {
                m_TaxSheet.txtInComeOffice = collection["txtInComeOffice"].ToString();
                m_TaxSheet.txtSubject = collection["txtSubject"].ToString();
                m_TaxSheet.txtBudgetLevels = collection["txtBudgetLevels"].ToString();
                m_TaxSheet.txtRecipientTreasury = collection["txtRecipientTreasury"].ToString();
                m_TaxSheet.txtPaymentUnit = collection["txtPaymentUnit"].ToString();
                m_TaxSheet.txtAccountNo = collection["txtAccountNo"].ToString();
                m_TaxSheet.txtBankName = collection["txtBankName"].ToString();
                m_TaxSheet.txtNumber = collection["txtNumber"].ToString();
                m_TaxSheet.txtUnit = collection["txtUnit"].ToString();
                m_TaxSheet.txtTaxRate = collection["txtTaxRate"].ToString();

                strKind= collection["txtKind"].ToString();

                M_TaxSheetItems m_TaxSheetItems = null;
                T_TaxSheetItems t_TaxSheetItems = new T_TaxSheetItems();

                m_TaxSheetItems = new M_TaxSheetItems();
                m_TaxSheetItems.tssKind = strKind;
                m_TaxSheetItems.tssItem = "txtInComeOffice";
                m_TaxSheetItems.tssFormItemName = "";
                m_TaxSheetItems.tssFormItemValue = m_TaxSheet.txtInComeOffice;
                m_TaxSheetItems.tssFormItemMemo = "";

                t_TaxSheetItems.deleTaxSheetItems(m_TaxSheetItems);
                t_TaxSheetItems.addTaxSheetItems(m_TaxSheetItems);

                m_TaxSheetItems = new M_TaxSheetItems();
                m_TaxSheetItems.tssKind = strKind;
                m_TaxSheetItems.tssItem = "txtSubject";
                m_TaxSheetItems.tssFormItemName = "";
                m_TaxSheetItems.tssFormItemValue = m_TaxSheet.txtSubject;
                m_TaxSheetItems.tssFormItemMemo = "";

                t_TaxSheetItems.deleTaxSheetItems(m_TaxSheetItems);
                t_TaxSheetItems.addTaxSheetItems(m_TaxSheetItems);

                m_TaxSheetItems = new M_TaxSheetItems();
                m_TaxSheetItems.tssKind = strKind;
                m_TaxSheetItems.tssItem = "txtBudgetLevels";
                m_TaxSheetItems.tssFormItemName = "";
                m_TaxSheetItems.tssFormItemValue = m_TaxSheet.txtBudgetLevels;
                m_TaxSheetItems.tssFormItemMemo = "";

                t_TaxSheetItems.deleTaxSheetItems(m_TaxSheetItems);
                t_TaxSheetItems.addTaxSheetItems(m_TaxSheetItems);

                m_TaxSheetItems = new M_TaxSheetItems();
                m_TaxSheetItems.tssKind = strKind;
                m_TaxSheetItems.tssItem = "txtRecipientTreasury";
                m_TaxSheetItems.tssFormItemName = "";
                m_TaxSheetItems.tssFormItemValue = m_TaxSheet.txtRecipientTreasury;
                m_TaxSheetItems.tssFormItemMemo = "";

                t_TaxSheetItems.deleTaxSheetItems(m_TaxSheetItems);
                t_TaxSheetItems.addTaxSheetItems(m_TaxSheetItems);

                m_TaxSheetItems = new M_TaxSheetItems();
                m_TaxSheetItems.tssKind = strKind;
                m_TaxSheetItems.tssItem = "txtPaymentUnit";
                m_TaxSheetItems.tssFormItemName = "";
                m_TaxSheetItems.tssFormItemValue = m_TaxSheet.txtPaymentUnit;
                m_TaxSheetItems.tssFormItemMemo = "";

                t_TaxSheetItems.deleTaxSheetItems(m_TaxSheetItems);
                t_TaxSheetItems.addTaxSheetItems(m_TaxSheetItems);

                m_TaxSheetItems = new M_TaxSheetItems();
                m_TaxSheetItems.tssKind = strKind;
                m_TaxSheetItems.tssItem = "txtAccountNo";
                m_TaxSheetItems.tssFormItemName = "";
                m_TaxSheetItems.tssFormItemValue = m_TaxSheet.txtAccountNo;
                m_TaxSheetItems.tssFormItemMemo = "";

                t_TaxSheetItems.deleTaxSheetItems(m_TaxSheetItems);
                t_TaxSheetItems.addTaxSheetItems(m_TaxSheetItems);

                m_TaxSheetItems = new M_TaxSheetItems();
                m_TaxSheetItems.tssKind = strKind;
                m_TaxSheetItems.tssItem = "txtBankName";
                m_TaxSheetItems.tssFormItemName = "";
                m_TaxSheetItems.tssFormItemValue = m_TaxSheet.txtBankName;
                m_TaxSheetItems.tssFormItemMemo = "";

                t_TaxSheetItems.deleTaxSheetItems(m_TaxSheetItems);
                t_TaxSheetItems.addTaxSheetItems(m_TaxSheetItems);

                m_TaxSheetItems = new M_TaxSheetItems();
                m_TaxSheetItems.tssKind = strKind;
                m_TaxSheetItems.tssItem = "txtNumber";
                m_TaxSheetItems.tssFormItemName = "";
                m_TaxSheetItems.tssFormItemValue = m_TaxSheet.txtNumber;
                m_TaxSheetItems.tssFormItemMemo = "";

                t_TaxSheetItems.deleTaxSheetItems(m_TaxSheetItems);
                t_TaxSheetItems.addTaxSheetItems(m_TaxSheetItems);

                m_TaxSheetItems = new M_TaxSheetItems();
                m_TaxSheetItems.tssKind = strKind;
                m_TaxSheetItems.tssItem = "txtUnit";
                m_TaxSheetItems.tssFormItemName = "";
                m_TaxSheetItems.tssFormItemValue = m_TaxSheet.txtUnit;
                m_TaxSheetItems.tssFormItemMemo = "";

                t_TaxSheetItems.deleTaxSheetItems(m_TaxSheetItems);
                t_TaxSheetItems.addTaxSheetItems(m_TaxSheetItems);

                m_TaxSheetItems = new M_TaxSheetItems();
                m_TaxSheetItems.tssKind = strKind;
                m_TaxSheetItems.tssItem = "txtTaxRate";
                m_TaxSheetItems.tssFormItemName = "";
                m_TaxSheetItems.tssFormItemValue = m_TaxSheet.txtTaxRate;
                m_TaxSheetItems.tssFormItemMemo = "";

                t_TaxSheetItems.deleTaxSheetItems(m_TaxSheetItems);
                t_TaxSheetItems.addTaxSheetItems(m_TaxSheetItems);

                strRet = "{\"result\":\"ok\",\"message\":\"保存成功\"}";
            }
            catch (Exception ex)
            {
                strRet = "{\"result\":\"error\",\"message\":\"" + ex.Message + "\"}";
            }

            return strRet;
        }
    }
}
