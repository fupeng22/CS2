using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace SQLDAL
{
    public class T_TaxSheetItems
    {
        /// <summary>
        /// 新增项目
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool addTaxSheetItems(Model.M_TaxSheetItems model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into TaxSheetItems");
            strSql.Append(" (tssKind,tssItem,tssFormItemName,tssFormItemValue,tssFormItemMemo)");
            strSql.Append(" values (");
            strSql.Append("@tssKind,@tssItem,@tssFormItemName,@tssFormItemValue,@tssFormItemMemo)");

            SqlParameter[] parameters = {
                   
                    new SqlParameter("@tssKind",SqlDbType.NVarChar),
                    new SqlParameter("@tssItem", SqlDbType.NVarChar),
                    new SqlParameter("@tssFormItemName", SqlDbType.NVarChar), 
                    new SqlParameter("@tssFormItemValue", SqlDbType.NVarChar),
                    new SqlParameter("@tssFormItemMemo", SqlDbType.NVarChar)
            };
            parameters[0].Value = model.tssKind;
            parameters[1].Value = model.tssItem;
            parameters[2].Value = model.tssFormItemName;
            parameters[3].Value = model.tssFormItemValue;
            parameters[4].Value = model.tssFormItemMemo;

            if (DBUtility.SqlServerHelper.ExecuteSql(strSql.ToString(), parameters) >= 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 修改项目
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool updateTaxSheetItems(Model.M_TaxSheetItems model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update TaxSheetItems set tssFormItemName=@tssFormItemName,tssFormItemValue=@tssFormItemValue where tssKind=@tssKind and tssItem=@tssItem");

            SqlParameter[] parameters = {
                   
                    new SqlParameter("@tssKind",SqlDbType.NVarChar),
                    new SqlParameter("@tssItem", SqlDbType.NVarChar),
                    new SqlParameter("@tssFormItemName", SqlDbType.NVarChar), 
                    new SqlParameter("@tssFormItemValue", SqlDbType.NVarChar),
            };
            parameters[0].Value = model.tssKind;
            parameters[1].Value = model.tssItem;
            parameters[2].Value = model.tssFormItemName;
            parameters[3].Value = model.tssFormItemValue;

            if (DBUtility.SqlServerHelper.ExecuteSql(strSql.ToString(), parameters) >= 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 修改项目
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool deleTaxSheetItems(Model.M_TaxSheetItems model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from TaxSheetItems where tssKind=@tssKind and tssItem=@tssItem");

            SqlParameter[] parameters = {
                   
                    new SqlParameter("@tssKind",SqlDbType.NVarChar),
                    new SqlParameter("@tssItem", SqlDbType.NVarChar)
            };
            parameters[0].Value = model.tssKind;
            parameters[1].Value = model.tssItem;

            if (DBUtility.SqlServerHelper.ExecuteSql(strSql.ToString(), parameters) >= 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 根据条件获取税表信息
        /// </summary>
        /// <param name="strKind"></param>
        /// <param name="strItem"></param>
        /// <returns></returns>
        public DataSet GetTaxSheetItemsInfo(string strKind)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"select * from TaxSheetItems where tssKind='" + strKind + "'");
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

        /// <summary>
        /// 根据条件获取税表信息
        /// </summary>
        /// <param name="strKind"></param>
        /// <param name="strItem"></param>
        /// <returns></returns>
        public DataSet GetTaxSheetItemsInfo(string strKind, string strItem)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"select * from TaxSheetItems where tssKind='" + strKind + "' and tssItem='" + strItem + "'");
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
