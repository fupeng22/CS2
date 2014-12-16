using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SQLDAL;
using System.Data;
using System.Text;

namespace CS.Controllers.Common
{
    public class ForTooltipController : Controller
    {
        //
        // GET: /ForTooltip/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ShowTaxSettingInfo(string TaxNO)
        {
            ViewData["hd_TaxNO"] = TaxNO;
            return View();
        }

        [HttpPost]
        public string LoadTaxRateSettingInfo(string TaxNO)
        {
            T_TaxRateSetting t_TaxRateSetting = new T_TaxRateSetting();
            DataSet ds = null;
            DataTable dt = null;
            StringBuilder sb = new StringBuilder("");

            string vd_TaxNo = "";
            string vd_CargoName = "";
            string vd_FullValue = "";
            string vd_TaxRate = "";

            TaxNO = Server.UrlDecode(TaxNO);

            if (!string.IsNullOrEmpty(TaxNO))
            {
                ds = t_TaxRateSetting.GetCargoNameByTaxNO(TaxNO);
                if (ds != null)
                {
                    dt = ds.Tables[0];
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        vd_TaxNo = dt.Rows[0]["TaxNo"] == DBNull.Value ? "" : dt.Rows[0]["TaxNo"].ToString();
                        vd_CargoName = dt.Rows[0]["CargoName"] == DBNull.Value ? "" : dt.Rows[0]["CargoName"].ToString();
                        vd_FullValue = dt.Rows[0]["FullValue"] == DBNull.Value ? "" : dt.Rows[0]["FullValue"].ToString();
                        vd_TaxRate = dt.Rows[0]["TaxRate"] == DBNull.Value ? "" : dt.Rows[0]["TaxRate"].ToString();
                    }
                }
            }

            sb.Append("{\"total\":4,\"rows\":[");
            sb.Append("{\"name\":\"税号\",\"value\":\"" + TaxNO + "\"},");
            sb.Append("{\"name\":\"品名\",\"value\":\"" + vd_CargoName + "\"},");
            sb.Append("{\"name\":\"完税价格\",\"value\":\"" + vd_FullValue + "\"},");
            sb.Append("{\"name\":\"税率\",\"value\":\"" + vd_TaxRate + "\"}");
            sb.Append("]}");

            return sb.ToString();
        }

        [HttpPost]
        public string LoadTaxRateSettingInfo_Html(string TaxNO)
        {
            T_TaxRateSetting t_TaxRateSetting = new T_TaxRateSetting();
            DataSet ds = null;
            DataTable dt = null;
            StringBuilder sb = new StringBuilder("");

            string vd_TaxNo = "";
            string vd_CargoName = "";
            string vd_FullValue = "";
            string vd_TaxRate = "";

            TaxNO = Server.UrlDecode(TaxNO);

            if (!string.IsNullOrEmpty(TaxNO))
            {
                ds = t_TaxRateSetting.GetCargoNameByTaxNO(TaxNO);
                if (ds != null)
                {
                    dt = ds.Tables[0];
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        vd_TaxNo = dt.Rows[0]["TaxNo"] == DBNull.Value ? "" : dt.Rows[0]["TaxNo"].ToString();
                        vd_CargoName = dt.Rows[0]["CargoName"] == DBNull.Value ? "" : dt.Rows[0]["CargoName"].ToString();
                        vd_FullValue = dt.Rows[0]["FullValue"] == DBNull.Value ? "" : dt.Rows[0]["FullValue"].ToString();
                        vd_TaxRate = dt.Rows[0]["TaxRate"] == DBNull.Value ? "" : dt.Rows[0]["TaxRate"].ToString();
                    }
                }
            }

            sb.Append("<table cellpadding='3' cellspacing='5'>");

            sb.Append("<thead>");
            sb.Append("<tr>");
            sb.Append("<td colspan='2' style='background-color: ButtonFace'>");
            sb.Append("<font style='font-weight:bold'>" + "对应标准税率信息" + "</font>");
            sb.Append("</td>");
            sb.Append("</tr>");
            sb.Append("</thead>");

            sb.Append("<tbody>");

            sb.Append("<tr>");
            sb.Append("<td>");
            sb.Append("<font style='color:red;font-weight:bold'>" + "税号" + "</font>");
            sb.Append("</td>");
            sb.Append("<td>");
            sb.Append(TaxNO);
            sb.Append("</td>");
            sb.Append("</tr>");

            sb.Append("<tr>");
            sb.Append("<td>");
            sb.Append("<font style='color:red;font-weight:bold'>" + "品名" + "</font>");
            sb.Append("</td>");
            sb.Append("<td>");
            sb.Append(vd_CargoName);
            sb.Append("</td>");
            sb.Append("</tr>");

            sb.Append("<tr>");
            sb.Append("<td>");
            sb.Append("<font style='color:red;font-weight:bold'>" + "完税价格" + "</font>");
            sb.Append("</td>");
            sb.Append("<td>");
            sb.Append(vd_FullValue);
            sb.Append("</td>");
            sb.Append("</tr>");

            sb.Append("<tr>");
            sb.Append("<td>");
            sb.Append("<font style='color:red;font-weight:bold'>" + "税率" + "</font>");
            sb.Append("</td>");
            sb.Append("<td>");
            sb.Append(vd_TaxRate);
            sb.Append("</td>");
            sb.Append("</tr>");

            sb.Append("</tbody>");

            sb.Append("</table>");

            return sb.ToString();
        }

        [HttpPost]
        public string ExplainDiffrentColorInfo_Html(string strID, string Type, string font_color)
        {
            //TaxValueCheck、mismatchCargoName、belowFullPrice、above1000
            StringBuilder sb = new StringBuilder("");
            switch (Type.ToLower())
            {
                case "mismatchcargoname":
                    sb.AppendFormat("<span style='color: " + font_color + "'>品名与税号不符</span>");
                    break;
                case "belowfullprice":
                    sb.AppendFormat("<span style='color: " + font_color + "'>低于限价</span>");
                    break;
                case "above1000":
                    sb.AppendFormat("<span style='color: " + font_color + "'>高于1000</span>");
                    break;
                case "taxvaluecheck":
                    sb.AppendFormat("<span style='color:" + font_color + "'>核准税金低于50</span>");
                    break;
                case "pickgoodsagain":
                    sb.AppendFormat("<span style='color:" + font_color + "'>十五日内重复提货</span>");
                    break;
                default:
                    break;
            }

            return sb.ToString();
        }

        [HttpPost]
        public string LoadLastPickGoodInfo_Html(string IDCard, string strSwbId)
        {
            DataSet ds = null;
            DataTable dt = null;
            StringBuilder sb = new StringBuilder("");

            string vd_IDCard = "";
            string vd_WbSerialNum = "";
            string vd_SwbSerialNum = "";
            string vd_WbStorageDate = "";

            IDCard = Server.UrlDecode(IDCard);
            strSwbId = Server.UrlDecode(strSwbId);

            if (!string.IsNullOrEmpty(IDCard) && !string.IsNullOrEmpty(strSwbId))
            {
                ds = new T_SubWayBill().LoadLastPickGoodInfo(strSwbId);
                if (ds != null)
                {
                    dt = ds.Tables[0];
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        vd_IDCard = dt.Rows[0]["ReceiverIDCard"] == DBNull.Value ? "" : dt.Rows[0]["ReceiverIDCard"].ToString();
                        vd_WbSerialNum = dt.Rows[0]["wbSerialNum"] == DBNull.Value ? "" : dt.Rows[0]["wbSerialNum"].ToString();
                        vd_SwbSerialNum = dt.Rows[0]["swbSerialNum"] == DBNull.Value ? "" : dt.Rows[0]["swbSerialNum"].ToString();
                        vd_WbStorageDate = dt.Rows[0]["wbStorageDate"] == DBNull.Value ? "" : dt.Rows[0]["wbStorageDate"].ToString();
                    }
                }
            }

            sb.Append("<table cellpadding='3' cellspacing='5'>");

            sb.Append("<thead>");
            sb.Append("<tr>");
            sb.Append("<td colspan='2' style='background-color: ButtonFace'>");
            sb.Append("<font style='font-weight:bold'>" + "上次提货信息" + "</font>");
            sb.Append("</td>");
            sb.Append("</tr>");
            sb.Append("</thead>");

            sb.Append("<tbody>");

            sb.Append("<tr>");
            sb.Append("<td>");
            sb.Append("<font style='color:red;font-weight:bold'>" + "身份证号" + "</font>");
            sb.Append("</td>");
            sb.Append("<td>");
            sb.Append(IDCard);
            sb.Append("</td>");
            sb.Append("</tr>");

            sb.Append("<tr>");
            sb.Append("<td>");
            sb.Append("<font style='color:red;font-weight:bold'>" + "总运单号" + "</font>");
            sb.Append("</td>");
            sb.Append("<td>");
            sb.Append(vd_WbSerialNum);
            sb.Append("</td>");
            sb.Append("</tr>");

            sb.Append("<tr>");
            sb.Append("<td>");
            sb.Append("<font style='color:red;font-weight:bold'>" + "分运单号" + "</font>");
            sb.Append("</td>");
            sb.Append("<td>");
            sb.Append(vd_SwbSerialNum);
            sb.Append("</td>");
            sb.Append("</tr>");

            sb.Append("<tr>");
            sb.Append("<td>");
            sb.Append("<font style='color:red;font-weight:bold'>" + "提货日期" + "</font>");
            sb.Append("</td>");
            sb.Append("<td>");
            sb.Append(vd_WbStorageDate);
            sb.Append("</td>");
            sb.Append("</tr>");

            sb.Append("</tbody>");

            sb.Append("</table>");

            return sb.ToString();
        }
    }
}
