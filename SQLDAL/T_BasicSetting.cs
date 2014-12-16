using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace SQLDAL
{
    public class T_BasicSetting
    {
        public DataSet LoadItemsByType(string type)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from BasicSetting");
            strSql.Append(" WHERE TypeName='"+type+"'");

            DataSet ds = DBUtility.SqlServerHelper.Query(strSql.ToString());
            if (ds.Tables[0].Rows.Count > 0)
            {
                return ds;
            }
            else
            {
                return null;
            }
        }
    }
}
