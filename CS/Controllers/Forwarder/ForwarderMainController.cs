using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;
using System.Data;
using Model;
using SQLDAL;
using CS.Filter;

namespace CS.Controllers.Forwarder
{
    [ErrorAttribute]
    public class ForwarderMainController : Controller
    {
        Model.M_WayBill wayBillModel = new M_WayBill();
        Model.M_SubWayBill subWayBillModel = new M_SubWayBill();
        SQLDAL.T_WayBill wayBillSql = new T_WayBill();
        SQLDAL.T_SubWayBill subWayBillSql = new T_SubWayBill();
        SQLDAL.T_User tUser = new T_User();

        //
        // GET: /ForwarderMain/
        [ForwarderRequiresLoginAttribute]
        public ActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public string LoadComboxJSON()
        {
            string strResult = "";
            StringBuilder sb = new StringBuilder("");
            DataSet ds = tUser.GetAllCompany();
            DataTable dt = new DataTable();
            sb.Append("[");
            sb.Append("{");
            sb.AppendFormat("\"id\":\"{0}\",\"text\":\"{1}\"", "---请选择---", "---请选择---");
            sb.Append("},");
            if (ds != null)
            {
                dt = ds.Tables[0];
                if (dt != null)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        sb.Append("{");
                        sb.AppendFormat("\"id\":\"{0}\",\"text\":\"{1}\"", dt.Rows[i]["company"].ToString(), dt.Rows[i]["company"].ToString());
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
            sb.Append("]");
            strResult = sb.ToString();
            return strResult;
        }

        [HttpPost]
        public string LoadComboxJSON_Forwarder()
        {
            string Company = "";
            string strResult = "";
            string strUserName = "";
            StringBuilder sb = new StringBuilder("");
            //if (Session["Global_Company"] != null)
            //{
            //    Company = Session["Global_Company"].ToString();
            //}
            strUserName = Session["Global_Forwarder_UserName"] == null ? "" : Session["Global_Forwarder_UserName"].ToString();
            if (strUserName!="")
            {
                Company = new T_User().GetUserByUserName(strUserName);
            }
            sb.Append("[");
            sb.Append("{");
            sb.AppendFormat("\"id\":\"{0}\",\"text\":\"{1}\"", Company, Company);
            sb.Append("}");
           
            if (sb.ToString().EndsWith(","))
            {
                sb = new StringBuilder(sb.ToString().Remove(sb.ToString().Length - 1));
            }
            sb.Append("]");
            strResult = sb.ToString();
            return strResult;
        }

        [HttpPost]
        public string GetCurrentCompany_Forwarder()
        {
            string strRet = "";
            string strUserName = "";
            strUserName = Session["Global_Forwarder_UserName"] == null ? "" : Session["Global_Forwarder_UserName"].ToString();
            if (strUserName != "")
            {
                strRet = new T_User().GetUserByUserName(strUserName);
            }
            return strRet;
        }

        [HttpPost]
        public string GetCurrentCompany()
        {
            string strRet = "";

            if (Session["Global_Company"] != null)
            {
                strRet = Session["Global_Company"].ToString();
            }
            return strRet;
        }

        [HttpPost]
        public string LoadAllCustomCategory()
        {
            string strResult = "";
            StringBuilder sb = new StringBuilder("");

            sb.Append("[");

            sb.Append("{");
            sb.AppendFormat("\"id\":\"{0}\",\"text\":\"{1}\"", 2, "样品");
            sb.Append("},");

            sb.Append("{");
            sb.AppendFormat("\"id\":\"{0}\",\"text\":\"{1}\"", 3, "KJ-3");
            sb.Append("},");

            sb.Append("{");
            sb.AppendFormat("\"id\":\"{0}\",\"text\":\"{1}\"", 4, "D类");
            sb.Append("},");

            sb.Append("{");
            sb.AppendFormat("\"id\":\"{0}\",\"text\":\"{1}\"", 5, "个人物品");
            sb.Append("},");

            sb.Append("{");
            sb.AppendFormat("\"id\":\"{0}\",\"text\":\"{1}\"", 6, "分运行李");
            sb.Append("},");
           
            if (sb.ToString().EndsWith(","))
            {
                sb = new StringBuilder(sb.ToString().Remove(sb.ToString().Length - 1));
            }
            sb.Append("]");
            strResult = sb.ToString();
            return strResult;
        }
    }
}
