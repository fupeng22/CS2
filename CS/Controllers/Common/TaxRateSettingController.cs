using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using DBUtility;
using SQLDAL;
using Model;

namespace CS.Controllers.Common
{
    public class TaxRateSettingController : Controller
    {
        SQLDAL.T_User tUser = new SQLDAL.T_User();
        Model.M_User mUser = new Model.M_User();
        public const string STR_TOP_ID = "top";
        public const string strFileds = "TaxNo,ParentTaxNo,CargoName,Unit,FullValue,TaxRate,TaxRateDescription,isLeaf,mMemo,parentID,ID,state,trsID";

        public const string STR_TEMPLATE_EXCEL = "~/Temp/Template/template.xls";
        public const string STR_REPORT_URL = "~/Content/Reports/HuayuUserMaintain.rdlc";
        //
        // GET: /Forwarder_QueryCompany/
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
        public string GetData(string order, string page, string rows, string sort, string id)
        {
            string strId = id == null ? STR_TOP_ID : id;
            rows = "1000";
            SqlParameter[] param = new SqlParameter[8];
            param[0] = new SqlParameter();
            param[0].SqlDbType = SqlDbType.VarChar;
            param[0].ParameterName = "@TableName";
            param[0].Direction = ParameterDirection.Input;
            param[0].Value = "V_TaxRateSetting";

            param[1] = new SqlParameter();
            param[1].SqlDbType = SqlDbType.VarChar;
            param[1].ParameterName = "@FieldKey";
            param[1].Direction = ParameterDirection.Input;
            param[1].Value = "trsID";

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
            param[6].Value = " ParentTaxNo='" + strId + "' ";

            param[7] = new SqlParameter();
            param[7].SqlDbType = SqlDbType.Int;
            param[7].ParameterName = "@RecordCount";
            param[7].Direction = ParameterDirection.Output;

            DataSet ds = SqlServerHelper.RunProcedure("spPageViewByStr", param, "result");
            DataTable dt = ds.Tables["result"];

            StringBuilder sb = new StringBuilder("");
            switch (strId)
            {
                case STR_TOP_ID:
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
                                case "parentID":
                                    if (j != strFiledArray.Length - 1)
                                    {
                                        sb.AppendFormat("\"{0}\":\"{1}\",", strFiledArray[j], dt.Rows[i]["ParentTaxNo"].ToString());
                                    }
                                    else
                                    {
                                        sb.AppendFormat("\"{0}\":\"{1}\"", strFiledArray[j], dt.Rows[i]["ParentTaxNo"].ToString());
                                    }
                                    break;
                                case "ID":
                                    if (j != strFiledArray.Length - 1)
                                    {
                                        sb.AppendFormat("\"{0}\":\"{1}\",", strFiledArray[j], dt.Rows[i]["TaxNo"].ToString());
                                    }
                                    else
                                    {
                                        sb.AppendFormat("\"{0}\":\"{1}\"", strFiledArray[j], dt.Rows[i]["TaxNo"].ToString());
                                    }
                                    break;
                                case "state":
                                    if (dt.Rows[i]["isLeaf"].ToString() == "1")
                                    {
                                        if (j != strFiledArray.Length - 1)
                                        {
                                            sb.AppendFormat("\"{0}\":\"{1}\",", strFiledArray[j], "open");
                                        }
                                        else
                                        {
                                            sb.AppendFormat("\"{0}\":\"{1}\"", strFiledArray[j], "open");
                                        }
                                    }
                                    else
                                    {
                                        if (j != strFiledArray.Length - 1)
                                        {
                                            sb.AppendFormat("\"{0}\":\"{1}\",", strFiledArray[j], "closed");
                                        }
                                        else
                                        {
                                            sb.AppendFormat("\"{0}\":\"{1}\"", strFiledArray[j], "closed");
                                        }
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
                    break;
                default:
                    sb.Append("[");
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        sb.Append("{");

                        string[] strFiledArray = strFileds.Split(',');
                        for (int j = 0; j < strFiledArray.Length; j++)
                        {
                            switch (strFiledArray[j])
                            {
                                case "parentID":
                                    if (j != strFiledArray.Length - 1)
                                    {
                                        sb.AppendFormat("\"{0}\":\"{1}\",", strFiledArray[j], dt.Rows[i]["ParentTaxNo"].ToString());
                                    }
                                    else
                                    {
                                        sb.AppendFormat("\"{0}\":\"{1}\"", strFiledArray[j], dt.Rows[i]["ParentTaxNo"].ToString());
                                    }
                                    break;
                                case "ID":
                                    if (j != strFiledArray.Length - 1)
                                    {
                                        sb.AppendFormat("\"{0}\":\"{1}\",", strFiledArray[j], dt.Rows[i]["TaxNo"].ToString());
                                    }
                                    else
                                    {
                                        sb.AppendFormat("\"{0}\":\"{1}\"", strFiledArray[j], dt.Rows[i]["TaxNo"].ToString());
                                    }
                                    break;
                                case "state":
                                    if (dt.Rows[i]["isLeaf"].ToString() == "1")
                                    {
                                        if (j != strFiledArray.Length - 1)
                                        {
                                            sb.AppendFormat("\"{0}\":\"{1}\",", strFiledArray[j], "open");
                                        }
                                        else
                                        {
                                            sb.AppendFormat("\"{0}\":\"{1}\"", strFiledArray[j], "open");
                                        }
                                    }
                                    else
                                    {
                                        if (j != strFiledArray.Length - 1)
                                        {
                                            sb.AppendFormat("\"{0}\":\"{1}\",", strFiledArray[j], "closed");
                                        }
                                        else
                                        {
                                            sb.AppendFormat("\"{0}\":\"{1}\"", strFiledArray[j], "closed");
                                        }
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
                    break;
            }

            return sb.ToString();
        }

        /// <summary>
        /// 新增类别
        /// </summary>
        /// <param name="parentTaxNo"></param>
        /// <param name="parentCategoryName"></param>
        /// <param name="currentTaxNo"></param>
        /// <param name="currentCategoryName"></param>
        /// <returns></returns>
        [HttpPost]
        public string CreateCategory(string parentTaxNo, string currentTaxNo, string currentCategoryName)
        {
            string strRet = "{\"result\":\"error\",\"message\":\"添加失败，原因未知\"}";

            parentTaxNo = Server.UrlDecode(parentTaxNo);
            currentTaxNo = Server.UrlDecode(currentTaxNo);
            currentCategoryName = Server.UrlDecode(currentCategoryName);

            try
            {
                //检验此税号是否已经使用
                if (new T_TaxRateSetting().TaxNoExist_ForCreate(currentTaxNo))
                {
                    strRet = "{\"result\":\"error\",\"message\":\"添加失败，此税号已经使用\"}";
                }
                else
                {
                    //检验此品名是否已经使用
                    if (new T_TaxRateSetting().CategoryNameExist_ForCreate(currentCategoryName))
                    {
                        strRet = "{\"result\":\"error\",\"message\":\"添加失败，此品名已经使用\"}";
                    }
                    else
                    {
                        M_TaxRateSetting m_TaxRateSetting = new M_TaxRateSetting();
                        m_TaxRateSetting.TaxNo = currentTaxNo;
                        m_TaxRateSetting.CargoName = currentCategoryName;
                        m_TaxRateSetting.ParentTaxNo = parentTaxNo;
                        if (new T_TaxRateSetting().CreateCategory(m_TaxRateSetting))
                        {
                            strRet = "{\"result\":\"ok\",\"message\":\"添加成功\"}";
                        }
                        else
                        {
                            strRet = "{\"result\":\"error\",\"message\":\"添加失败\"}";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                strRet = "{\"result\":\"error\",\"message\":\"添加失败,原因:" + ex.Message + "\"}";
            }

            return strRet;
        }

        [HttpPost]
        public string UpdateCategory(string currentTaxNo, string currentCategoryName, string trsId)
        {
            string strRet = "{\"result\":\"error\",\"message\":\"修改失败，原因未知\"}";

            trsId = Server.UrlDecode(trsId);
            currentTaxNo = Server.UrlDecode(currentTaxNo);
            currentCategoryName = Server.UrlDecode(currentCategoryName);

            try
            {
                //检验此税号是否已经使用
                if (new T_TaxRateSetting().TaxNoExist_ForUpdate(currentTaxNo, trsId))
                {
                    strRet = "{\"result\":\"error\",\"message\":\"修改失败，此税号已经使用\"}";
                }
                else
                {
                    //检验此品名是否已经使用
                    if (new T_TaxRateSetting().CategoryNameExist_ForUpdate(currentCategoryName, trsId))
                    {
                        strRet = "{\"result\":\"error\",\"message\":\"修改失败，此品名已经使用\"}";
                    }
                    else
                    {
                        M_TaxRateSetting m_TaxRateSetting = new M_TaxRateSetting();
                        m_TaxRateSetting.TaxNo = currentTaxNo;
                        m_TaxRateSetting.CargoName = currentCategoryName;
                        m_TaxRateSetting.trsID = Convert.ToInt32(trsId);
                        if (new T_TaxRateSetting().UpdateCategory(m_TaxRateSetting))
                        {
                            strRet = "{\"result\":\"ok\",\"message\":\"修改成功\"}";
                        }
                        else
                        {
                            strRet = "{\"result\":\"error\",\"message\":\"修改失败\"}";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                strRet = "{\"result\":\"error\",\"message\":\"修改失败,原因:" + ex.Message + "\"}";
            }

            return strRet;
        }

        [HttpPost]
        public string CreateItem(string parentTaxNo, string currentTaxNo, string currentCategoryName, string txtItemUnit, string txtItemFullValue, string txtItemTaxRate)
        {
            string strRet = "{\"result\":\"error\",\"message\":\"添加失败，原因未知\"}";

            parentTaxNo = Server.UrlDecode(parentTaxNo);
            currentTaxNo = Server.UrlDecode(currentTaxNo);
            currentCategoryName = Server.UrlDecode(currentCategoryName);
            txtItemUnit = Server.UrlDecode(txtItemUnit);
            txtItemFullValue = Server.UrlDecode(txtItemFullValue);
            txtItemTaxRate = Server.UrlDecode(txtItemTaxRate);

            try
            {
                //检验此税号是否已经使用
                if (new T_TaxRateSetting().TaxNoExist_ForCreate(currentTaxNo))
                {
                    strRet = "{\"result\":\"error\",\"message\":\"添加失败，此税号已经使用\"}";
                }
                else
                {
                    //检验此品名是否已经使用
                    if (new T_TaxRateSetting().CategoryNameExist_ForCreate(currentCategoryName))
                    {
                        strRet = "{\"result\":\"error\",\"message\":\"添加失败，此品名已经使用\"}";
                    }
                    else
                    {
                        M_TaxRateSetting m_TaxRateSetting = new M_TaxRateSetting();
                        m_TaxRateSetting.TaxNo = currentTaxNo;
                        m_TaxRateSetting.CargoName = currentCategoryName;
                        m_TaxRateSetting.ParentTaxNo = parentTaxNo;
                        m_TaxRateSetting.TaxRate = Convert.ToDouble(txtItemTaxRate);
                        m_TaxRateSetting.FullValue = txtItemFullValue;
                        m_TaxRateSetting.Unit = txtItemUnit;
                        if (new T_TaxRateSetting().CreateItem(m_TaxRateSetting))
                        {
                            strRet = "{\"result\":\"ok\",\"message\":\"添加成功\"}";
                        }
                        else
                        {
                            strRet = "{\"result\":\"error\",\"message\":\"添加失败\"}";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                strRet = "{\"result\":\"error\",\"message\":\"添加失败,原因:" + ex.Message + "\"}";
            }

            return strRet;
        }

        [HttpPost]
        public string UpdateItem(string currentTaxNo, string currentCategoryName, string txtItemUnit, string txtItemFullValue, string txtItemTaxRate, string trsId)
        {
            string strRet = "{\"result\":\"error\",\"message\":\"修改失败，原因未知\"}";

            trsId = Server.UrlDecode(trsId);
            currentTaxNo = Server.UrlDecode(currentTaxNo);
            currentCategoryName = Server.UrlDecode(currentCategoryName);
            txtItemUnit = Server.UrlDecode(txtItemUnit);
            txtItemFullValue = Server.UrlDecode(txtItemFullValue);
            txtItemTaxRate = Server.UrlDecode(txtItemTaxRate);

            try
            {
                //检验此税号是否已经使用
                if (new T_TaxRateSetting().TaxNoExist_ForUpdate(currentTaxNo, trsId))
                {
                    strRet = "{\"result\":\"error\",\"message\":\"修改失败，此税号已经使用\"}";
                }
                else
                {
                    //检验此品名是否已经使用
                    if (new T_TaxRateSetting().CategoryNameExist_ForUpdate(currentCategoryName, trsId))
                    {
                        strRet = "{\"result\":\"error\",\"message\":\"修改失败，此品名已经使用\"}";
                    }
                    else
                    {
                        M_TaxRateSetting m_TaxRateSetting = new M_TaxRateSetting();
                        m_TaxRateSetting.TaxNo = currentTaxNo;
                        m_TaxRateSetting.CargoName = currentCategoryName;
                        m_TaxRateSetting.Unit = txtItemUnit;
                        m_TaxRateSetting.FullValue = txtItemFullValue;
                        m_TaxRateSetting.TaxRate = Convert.ToDouble(txtItemTaxRate);
                        m_TaxRateSetting.trsID = Convert.ToInt32(trsId);
                        if (new T_TaxRateSetting().UpdateItem(m_TaxRateSetting))
                        {
                            strRet = "{\"result\":\"ok\",\"message\":\"修改成功\"}";
                        }
                        else
                        {
                            strRet = "{\"result\":\"error\",\"message\":\"修改失败\"}";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                strRet = "{\"result\":\"error\",\"message\":\"修改失败,原因:" + ex.Message + "\"}";
            }

            return strRet;
        }

        [HttpPost]
        public string DeleteItem(string trsId)
        {
            string strRet = "{\"result\":\"error\",\"message\":\"删除失败，原因未知\"}";

            trsId = Server.UrlDecode(trsId);
           
            try
            {
                M_TaxRateSetting m_TaxRateSetting = new M_TaxRateSetting();
                m_TaxRateSetting.trsID = Convert.ToInt32(trsId );
               
                if (new T_TaxRateSetting().DeleteItem(m_TaxRateSetting))
                {
                    strRet = "{\"result\":\"ok\",\"message\":\"删除成功\"}";
                }
                else
                {
                    strRet = "{\"result\":\"error\",\"message\":\"删除失败\"}";
                }
            }
            catch (Exception ex)
            {
                strRet = "{\"result\":\"error\",\"message\":\"删除失败,原因:" + ex.Message + "\"}";
            }

            return strRet;
        }

        [HttpPost]
        public string LoadComboxJSON_CargoNameByTaxNO(string type,string TaxNO)
        {
            string strResult = "";
            string cargoName = "";
            StringBuilder sb = new StringBuilder("");
            TaxNO = Server.UrlDecode(TaxNO);

            DataTable dt = new DataTable();
            sb.Append("[");
            sb.Append("{");
            if (TaxNO == "")
            {
                cargoName = "";
            }
            else
            {
                DataSet dsTaxRateSetting = new T_TaxRateSetting().GetCargoNameByTaxNO(TaxNO);
                if (dsTaxRateSetting != null)
                {
                    DataTable dtTaxRateSetting = dsTaxRateSetting.Tables[0];
                    if (dtTaxRateSetting!=null && dtTaxRateSetting.Rows.Count>0)
                    {
                        switch (type)
                        {
                            case "0":
                                cargoName = dtTaxRateSetting.Rows[0]["CargoName"].ToString();
                                break;
                            case "1":
                                cargoName = dtTaxRateSetting.Rows[0]["TaxRate"].ToString();
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
            sb.AppendFormat("\"id\":\"{0}\",\"text\":\"{1}\"", cargoName, cargoName);
            sb.Append("}");

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
