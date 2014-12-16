using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace SQLDAL
{
    public class T_PrintTaxSheetInfo
    {
        /// <summary>
        /// 新增项目
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool addPrintTaxSheetInfo(Model.M_PrintTaxSheetInfo model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into PrintTaxSheetInfo");
            strSql.Append(" (PrintDate,OrderNumber,Operator,mMemo)");
            strSql.Append(" values (");
            strSql.Append("@PrintDate,@OrderNumber,@Operator,@mMemo)");

            SqlParameter[] parameters = {
                   
                    new SqlParameter("@PrintDate",SqlDbType.DateTime),
                    new SqlParameter("@OrderNumber", SqlDbType.Int),
                    new SqlParameter("@Operator", SqlDbType.NVarChar), 
                    new SqlParameter("@mMemo", SqlDbType.NVarChar)
            };
            parameters[0].Value = model.PrintDate;
            parameters[1].Value = model.OrderNumber;
            parameters[2].Value = model.Operator;
            parameters[3].Value = model.mMemo;

            if (DBUtility.SqlServerHelper.ExecuteSql(strSql.ToString(), parameters) >= 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public string GetNextOrderNumber()
        {
            string strOrderNumber = "错误";
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"select count(0) from PrintTaxSheetInfo");
            DataSet ds = DBUtility.SqlServerHelper.Query(strSql.ToString());
            if (Convert.ToInt32(ds.Tables[0].Rows[0][0].ToString()) == 0)
            {
                strOrderNumber = "00001";
            }
            else
            {
                strSql = new StringBuilder("");
                strSql.Append(@"select max(ordernumber)+1 from PrintTaxSheetInfo");
                ds = DBUtility.SqlServerHelper.Query(strSql.ToString());
                strOrderNumber = Convert.ToInt32(ds.Tables[0].Rows[0][0].ToString()).ToString("00000"); ;
            }
            return strOrderNumber;
        }
    }
}
