using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Model;

namespace SQLDAL
{
    public class T_WayBillWeight
    {
        /// <summary>
        /// 查询指定总运单号是否已经在计费重量表中
        /// </summary>
        /// <param name="wbID"></param>
        /// <returns></returns>
        public Boolean ExistInWayBillWeight(string wbID)
        {
            Boolean bExist = false;

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(0) from WayBillWeight");
            strSql.Append(" where wbw_wbID=" + wbID);
            DataSet ds = DBUtility.SqlServerHelper.Query(strSql.ToString());

            if (int.Parse(ds.Tables[0].Rows[0][0].ToString()) > 0)
            {
                bExist = true;
            }

            return bExist;
        }

        /// <summary>
        /// 添加计费重量
        /// </summary>
        /// <param name="m_WayBillWeight"></param>
        /// <returns></returns>
        public Boolean AddWayBillWeight(M_WayBillWeight m_WayBillWeight)
        {
            Boolean bOK = false;
            StringBuilder strSql = new StringBuilder();
            try
            {
                strSql.AppendFormat(@" insert into WayBillWeight(wbw_wbID,ActualWeight)
                                                        values({0},{1})", m_WayBillWeight.wbw_wbID, m_WayBillWeight.ActualWeight);

                if (DBUtility.SqlServerHelper.ExecuteSql(strSql.ToString()) >= 1)
                {
                    bOK = true;
                }
            }
            catch (Exception ex)
            {
                bOK = false;
            }

            return bOK;
        }

        /// <summary>
        /// 修改计费重量
        /// </summary>
        /// <param name="m_WayBillWeight"></param>
        /// <returns></returns>
        public Boolean UpdateWayBillWeight(M_WayBillWeight m_WayBillWeight)
        {
            Boolean bOK = false;
            StringBuilder strSql = new StringBuilder();
            try
            {
                strSql.AppendFormat(@" update WayBillWeight set ActualWeight={1} where wbw_wbID={0}", m_WayBillWeight.wbw_wbID, m_WayBillWeight.ActualWeight);

                if (DBUtility.SqlServerHelper.ExecuteSql(strSql.ToString()) >= 1)
                {
                    bOK = true;
                }
            }
            catch (Exception ex)
            {
                bOK = false;
            }

            return bOK;
        }

        /// <summary>
        /// 获取计费重量
        /// </summary>
        /// <returns></returns>
        public string getWeightForCompute(string wbID)
        {
            string strRet = "0.00";

            DataSet ds = null;
            DataTable dt = null;

            try
            {
                ds = DBUtility.SqlServerHelper.Query("select top 1 * from WayBillWeight where wbw_wbID=" + wbID);
                if (ds != null)
                {
                    dt = ds.Tables[0];
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        strRet = dt.Rows[0]["ActualWeight"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                strRet = "0.00";
            }

            if (strRet == "0.00")
            {
                try
                {
                    ds = new T_WayBill().getWayBillInfo(wbID);
                    if (ds != null)
                    {
                        dt = ds.Tables[0];
                        if (dt != null && dt.Rows.Count > 0)
                        {
                            strRet = dt.Rows[0]["wbTotalWeight"].ToString();
                        }
                    }
                }
                catch (Exception ex)
                {
                    strRet = "0.00";
                }
            }
            return strRet;
        }
    }
}
