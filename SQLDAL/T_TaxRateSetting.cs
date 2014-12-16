using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model;
using System.Data.SqlClient;
using System.Data;

namespace SQLDAL
{
    public class T_TaxRateSetting
    {
        string strRet = "";

        public bool CreateCategory(M_TaxRateSetting m_TaxRateSetting)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into TaxRateSetting");
            strSql.Append(" (TaxNo,ParentTaxNo,CargoName,isLeaf)");
            strSql.Append(" values (");
            strSql.Append("@TaxNo,@ParentTaxNo,@CargoName,@isLeaf)");

            SqlParameter[] parameters = {
                    new SqlParameter("@TaxNo",SqlDbType.NVarChar),
                    new SqlParameter("@ParentTaxNo",SqlDbType.NVarChar ),
                    new SqlParameter("@CargoName", SqlDbType.NVarChar),
                    new SqlParameter("@isLeaf",SqlDbType.Int)
            };
            parameters[0].Value = m_TaxRateSetting.TaxNo;
            parameters[1].Value = m_TaxRateSetting.ParentTaxNo;
            parameters[2].Value = m_TaxRateSetting.CargoName;
            parameters[3].Value = 0;

            if (DBUtility.SqlServerHelper.ExecuteSql(strSql.ToString(), parameters) >= 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool UpdateCategory(M_TaxRateSetting m_TaxRateSetting)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update TaxRateSetting set TaxNo=@TaxNo,CargoName=@CargoName,isLeaf=@isLeaf where trsID=@trsID");

            SqlParameter[] parameters = {
                    new SqlParameter("@TaxNo",SqlDbType.NVarChar),
                    new SqlParameter("@CargoName", SqlDbType.NVarChar),
                    new SqlParameter("@isLeaf",SqlDbType.Int),
                    new SqlParameter("@trsID",SqlDbType.Int)
            };
            parameters[0].Value = m_TaxRateSetting.TaxNo;
            parameters[1].Value = m_TaxRateSetting.CargoName;
            parameters[2].Value = 0;
            parameters[3].Value = m_TaxRateSetting.trsID;

            if (DBUtility.SqlServerHelper.ExecuteSql(strSql.ToString(), parameters) >= 1)
            {
                return true;
            }
            else
            {
                return false;

            }
        }

        public bool CreateItem(M_TaxRateSetting m_TaxRateSetting)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into TaxRateSetting");
            strSql.Append(" (TaxNo,ParentTaxNo,CargoName,Unit,FullValue,TaxRate,isLeaf)");
            strSql.Append(" values (");
            strSql.Append("@TaxNo,@ParentTaxNo,@CargoName,@Unit,@FullValue,@TaxRate,@isLeaf)");

            SqlParameter[] parameters = {
                    new SqlParameter("@TaxNo",SqlDbType.NVarChar),
                    new SqlParameter("@ParentTaxNo",SqlDbType.NVarChar ),
                    new SqlParameter("@CargoName", SqlDbType.NVarChar),
                    new SqlParameter("@Unit", SqlDbType.NVarChar),
                    new SqlParameter("@FullValue", SqlDbType.NVarChar),
                    new SqlParameter("@TaxRate", SqlDbType.Decimal),
                    new SqlParameter("@isLeaf",SqlDbType.Int)
            };
            parameters[0].Value = m_TaxRateSetting.TaxNo;
            parameters[1].Value = m_TaxRateSetting.ParentTaxNo;
            parameters[2].Value = m_TaxRateSetting.CargoName;
            parameters[3].Value = m_TaxRateSetting.Unit;
            parameters[4].Value = m_TaxRateSetting.FullValue;
            parameters[5].Value = m_TaxRateSetting.TaxRate;
            parameters[6].Value = 1;

            if (DBUtility.SqlServerHelper.ExecuteSql(strSql.ToString(), parameters) >= 1)
            {
                return true;
            }
            else
            {
                return false;

            }
        }

        public bool UpdateItem(M_TaxRateSetting m_TaxRateSetting)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update TaxRateSetting set TaxNo=@TaxNo,CargoName=@CargoName,Unit=@Unit,FullValue=@FullValue,TaxRate=@TaxRate,isLeaf=@isLeaf where trsID=@trsID");

            SqlParameter[] parameters = {
                    new SqlParameter("@TaxNo",SqlDbType.NVarChar),
                    new SqlParameter("@CargoName", SqlDbType.NVarChar),
                    new SqlParameter("@Unit", SqlDbType.NVarChar),
                    new SqlParameter("@FullValue", SqlDbType.NVarChar),
                    new SqlParameter("@TaxRate", SqlDbType.Decimal),
                    new SqlParameter("@isLeaf",SqlDbType.Int),
                    new SqlParameter("@trsID",SqlDbType.Int)
            };
            parameters[0].Value = m_TaxRateSetting.TaxNo;
            parameters[1].Value = m_TaxRateSetting.CargoName;
            parameters[2].Value = m_TaxRateSetting.Unit;
            parameters[3].Value = m_TaxRateSetting.FullValue;
            parameters[4].Value = m_TaxRateSetting.TaxRate;
            parameters[5].Value = 1;
            parameters[6].Value = m_TaxRateSetting.trsID;

            if (DBUtility.SqlServerHelper.ExecuteSql(strSql.ToString(), parameters) >= 1)
            {
                return true;
            }
            else
            {
                return false;

            }
        }

        public bool TaxNoExist_ForCreate(string TaxNo)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from TaxRateSetting where TaxNo='" + TaxNo + "'");

            if (DBUtility.SqlServerHelper.Query(strSql.ToString()).Tables[0].Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool TaxNoExist_ForUpdate(string TaxNo, string trsId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from TaxRateSetting where TaxNo='" + TaxNo + "' and trsID<>" + trsId);

            if (DBUtility.SqlServerHelper.Query(strSql.ToString()).Tables[0].Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool CategoryNameExist_ForCreate(string CategoryName)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from TaxRateSetting where CargoName='" + CategoryName + "'");

            if (DBUtility.SqlServerHelper.Query(strSql.ToString()).Tables[0].Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool CategoryNameExist_ForUpdate(string CategoryName, string trsId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from TaxRateSetting where CargoName='" + CategoryName + "' and trsID<>" + trsId);

            if (DBUtility.SqlServerHelper.Query(strSql.ToString()).Tables[0].Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool DeleteItem(M_TaxRateSetting m_TaxRateSetting)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from TaxRateSetting where trsID=@trsID");

            SqlParameter[] parameters = {
                    new SqlParameter("@trsID",SqlDbType.Int)
            };
            parameters[0].Value = m_TaxRateSetting.trsID;

            if (DBUtility.SqlServerHelper.ExecuteSql(strSql.ToString(), parameters) >= 1)
            {
                return true;
            }
            else
            {
                return false;

            }
        }

        public DataSet GetCargoNameByTaxNO(string TaxNO)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  * from TaxRateSetting where TaxNo='" + TaxNO + "'");
            DataSet ds = DBUtility.SqlServerHelper.Query(strSql.ToString());
            return ds;
        }

        public bool IsMismatchCargoName(string cargoName, string TaxNO)
        {
            Boolean bMatch = false;
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(0) from TaxRateSetting where CargoName='" + cargoName + "' and TaxNo='" + TaxNO + "'");

            if (Convert.ToInt32(DBUtility.SqlServerHelper.Query(strSql.ToString()).Tables[0].Rows[0][0]) > 0)
            {
                bMatch = true;
            }

            return bMatch;
        }

        public bool IsBelowFullValue(string cargoValue, string TaxNO)
        {
            Boolean bBelow = false;
            DataSet ds = null;
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from TaxRateSetting where FullValue<>'另行确定' and TaxNo='" + TaxNO + "'");
            ds = DBUtility.SqlServerHelper.Query(strSql.ToString());

            if (ds != null)
            {
                DataTable dt = ds.Tables[0];
                if (dt != null && dt.Rows.Count > 0)
                {
                    if (Convert.ToDouble(dt.Rows[0]["FullValue"].ToString()) <= Convert.ToDouble(cargoValue))
                    {
                        bBelow = false;
                    }
                    else
                    {
                        bBelow = true;
                    }
                }
            }

            return bBelow;
        }

        public string GetCategoryByTaxNO(string TaxNO)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  * from TaxRateSetting where TaxNo='" + TaxNO + "'");
            DataSet ds = DBUtility.SqlServerHelper.Query(strSql.ToString());
            if (ds!=null)
            {
                DataTable dt = ds.Tables[0];
                if (dt!=null && dt.Rows.Count>0)
                {
                    if (dt.Rows[0]["ParentTaxNo"].ToString() == "top")
                    {
                        strRet = TaxNO;
                        return strRet;
                    }
                    else
                    {
                        GetCategoryByTaxNO(dt.Rows[0]["ParentTaxNo"].ToString());
                    }
                }
            }
            return strRet;
        }
    }
}
