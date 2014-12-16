using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace SQLDAL
{
    public class T_SubWayBillDetail
    {
        /// <summary>
        /// 新增分运单明细
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool AddSubWayBillDetail(Model.M_SubWayBillDetail model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into SubWaybill_Detail");
            strSql.Append(" (swbd_swbID,swbDescription_CHN,swbDescription_ENG,swbNumber,swbWeight,swbValue,swbMonetary,swbRecipients,swbCustomsCategory,swbValueDetail,TaxNo,TaxRate,CategoryNo,mismatchCargoName,belowFullPrice,above1000,TaxValue)");
            strSql.Append(" values (");
            strSql.Append("@swbd_swbID,@swbDescription_CHN,@swbDescription_ENG,@swbNumber,@swbWeight,@swbValue,@swbMonetary,@swbRecipients,@swbCustomsCategory,@swbValueDetail,@TaxNo,@TaxRate,@CategoryNo,@mismatchCargoName,@belowFullPrice,@above1000,@TaxValue)");

            SqlParameter[] parameters = {
                   
                    new SqlParameter("@swbd_swbID",SqlDbType.Int),
                    new SqlParameter("@swbDescription_CHN", SqlDbType.VarChar), 
                    new SqlParameter("@swbDescription_ENG", SqlDbType.VarChar),
                    new SqlParameter("@swbNumber", SqlDbType.Int),
                    new SqlParameter("@swbWeight", SqlDbType.Real),
                    new SqlParameter("@swbValue", SqlDbType.Real),
                    new SqlParameter("@swbMonetary", SqlDbType.VarChar),
                    new SqlParameter("@swbRecipients", SqlDbType.VarChar),
                    new SqlParameter("@swbCustomsCategory", SqlDbType.VarChar),
                    new SqlParameter("@swbValueDetail", SqlDbType.VarChar),
                    new SqlParameter("@TaxNo", SqlDbType.VarChar),
                    new SqlParameter("@TaxRate", SqlDbType.Decimal),
                    new SqlParameter("@CategoryNo", SqlDbType.VarChar),
                    new SqlParameter("@mismatchCargoName", SqlDbType.Int),
                    new SqlParameter("@belowFullPrice", SqlDbType.Int),
                    new SqlParameter("@above1000", SqlDbType.Int),
                    new SqlParameter("@TaxValue", SqlDbType.Real)
            };

            parameters[0].Value = model.swbd_swbID;
            parameters[1].Value = model.SwbDescription_CHN;
            parameters[2].Value = model.SwbDescription_ENG;
            parameters[3].Value = model.SwbNumber;
            parameters[4].Value = model.SwbWeight;
            parameters[5].Value = model.SwbValue;
            parameters[6].Value = model.SwbMonetary;
            parameters[7].Value = model.SwbRecipients;
            parameters[8].Value = model.SwbCustomsCategory;
            parameters[9].Value = model.swbValueDetail == null ? "" : model.swbValueDetail;
            parameters[10].Value = model.TaxNo;
            parameters[11].Value = model.TaxRate;
            parameters[12].Value = model.CategoryNo;
            parameters[13].Value = model.mismatchCargoName;
            parameters[14].Value = model.belowFullPrice;
            parameters[15].Value = model.above1000;
            parameters[16].Value = model.TaxValue;

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
        /// 新增分运单明细
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool UpdateSubWayBillDetail(Model.M_SubWayBillDetail model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update SubWaybill_Detail set swbd_swbID=@swbd_swbID,swbDescription_CHN=@swbDescription_CHN,swbDescription_ENG=@swbDescription_ENG,swbNumber=@swbNumber,swbWeight=@swbWeight,swbValue=@swbValue,swbMonetary=@swbMonetary,swbRecipients=@swbRecipients,swbCustomsCategory=@swbCustomsCategory,swbValueDetail=@swbValueDetail,TaxNo=@TaxNo,TaxRate=@TaxRate,CategoryNo=@CategoryNo,mismatchCargoName=@mismatchCargoName,belowFullPrice=@belowFullPrice,above1000=@above1000,TaxValue=@TaxValue where swbdID=@swbdID");
           
            SqlParameter[] parameters = {
                   
                    new SqlParameter("@swbd_swbID",SqlDbType.Int),
                    new SqlParameter("@swbDescription_CHN", SqlDbType.VarChar), 
                    new SqlParameter("@swbDescription_ENG", SqlDbType.VarChar),
                    new SqlParameter("@swbNumber", SqlDbType.Int),
                    new SqlParameter("@swbWeight", SqlDbType.Real),
                    new SqlParameter("@swbValue", SqlDbType.Real),
                    new SqlParameter("@swbMonetary", SqlDbType.VarChar),
                    new SqlParameter("@swbRecipients", SqlDbType.VarChar),
                    new SqlParameter("@swbCustomsCategory", SqlDbType.VarChar),
                    new SqlParameter("@swbValueDetail", SqlDbType.VarChar),
                    new SqlParameter("@TaxNo", SqlDbType.VarChar),
                    new SqlParameter("@TaxRate", SqlDbType.Decimal),
                    new SqlParameter("@CategoryNo", SqlDbType.VarChar),
                    new SqlParameter("@swbdID", SqlDbType.Int),
                    new SqlParameter("@mismatchCargoName", SqlDbType.Int),
                    new SqlParameter("@belowFullPrice", SqlDbType.Int),
                    new SqlParameter("@above1000", SqlDbType.Int),
                    new SqlParameter("@TaxValue", SqlDbType.Real)
            };

            parameters[0].Value = model.swbd_swbID;
            parameters[1].Value = model.SwbDescription_CHN;
            parameters[2].Value = model.SwbDescription_ENG;
            parameters[3].Value = model.SwbNumber;
            parameters[4].Value = model.SwbWeight;
            parameters[5].Value = model.SwbValue;
            parameters[6].Value = model.SwbMonetary;
            parameters[7].Value = model.SwbRecipients;
            parameters[8].Value = model.SwbCustomsCategory;
            parameters[9].Value = model.swbValueDetail == null ? "" : model.swbValueDetail;
            parameters[10].Value = model.TaxNo;
            parameters[11].Value = model.TaxRate;
            parameters[12].Value = model.CategoryNo;
            parameters[13].Value = model.swbdID;
            parameters[14].Value = model.mismatchCargoName;
            parameters[15].Value = model.belowFullPrice;
            parameters[16].Value = model.above1000;
            parameters[17].Value = model.TaxValue;

            if (DBUtility.SqlServerHelper.ExecuteSql(strSql.ToString(), parameters) >= 1)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        public bool DeleteSubWayBillDetail(string ids)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from SubWaybill_Detail where swbdID in ("+ids+")");
           
            if (DBUtility.SqlServerHelper.ExecuteSql(strSql.ToString()) >= 1)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        /// <summary>
        /// 根据品名来获取对应的子运单序号
        /// </summary>
        /// <param name="swbDescription_CHN"></param>
        /// <param name="swbDescription_ENG"></param>
        /// <returns></returns>
        public string CreateSwbIds(string swbDescription_CHN, string swbDescription_ENG)
        {
            string strRet = "";
            string strWhereTemp = "";
            StringBuilder strSql = new StringBuilder("");
            StringBuilder sbSwbIds = new StringBuilder("");

            if (!(swbDescription_CHN=="" && swbDescription_ENG==""))
            {
                if (swbDescription_CHN != "")
                {
                    if (strWhereTemp != "")
                    {
                        strWhereTemp = strWhereTemp + " and (swbDescription_CHN like '%" + swbDescription_CHN + "%') ";
                    }
                    else
                    {
                        strWhereTemp = strWhereTemp + "  (swbDescription_CHN like '%" + swbDescription_CHN + "%') ";
                    }
                }

                if (swbDescription_ENG != "")
                {
                    if (strWhereTemp != "")
                    {
                        strWhereTemp = strWhereTemp + " and (swbDescription_ENG like '%" + swbDescription_ENG + "%') ";
                    }
                    else
                    {
                        strWhereTemp = strWhereTemp + "  (swbDescription_ENG like '%" + swbDescription_ENG + "%') ";
                    }
                }

                strSql.Append("select swbID from V_SubWayBill_SubWayBillDetail where ").Append(strWhereTemp);

                DataSet ds = DBUtility.SqlServerHelper.Query(strSql.ToString());
                if (ds!=null)
                {
                    DataTable dt = ds.Tables[0];
                    if (dt!=null && dt.Rows.Count>0)
                    {
                        sbSwbIds.Append("(");
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            sbSwbIds.Append(dt.Rows[i]["swbID"].ToString());
                            if (i != dt.Rows.Count - 1)
                            {
                                sbSwbIds.Append(",");
                            }
                        }
                        sbSwbIds.Append(")");
                        strRet = sbSwbIds.ToString();
                    }
                }
            }

            return strRet;
        }

        /// <summary>
        /// 修改查验结果信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool UpdateCheckResultInfo(Model.M_SubWayBillDetail model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update SubWaybill_Detail set CheckResult=@CheckResult,HandleSuggestion=@HandleSuggestion,CheckResultDescription=@CheckResultDescription,HandleSuggestionDescription=@HandleSuggestionDescription,CheckResultOperator=@CheckResultOperator where swbdID=@swbdID");

            SqlParameter[] parameters = {
                   
                    new SqlParameter("@CheckResult",SqlDbType.Int),
                    new SqlParameter("@HandleSuggestion", SqlDbType.Int), 
                    new SqlParameter("@CheckResultDescription", SqlDbType.NVarChar),
                    new SqlParameter("@HandleSuggestionDescription", SqlDbType.NVarChar),
                    new SqlParameter("@CheckResultOperator", SqlDbType.NVarChar),
                    new SqlParameter("@swbdID", SqlDbType.Int)
            };

            parameters[0].Value = model.CheckResult;
            parameters[1].Value = model.HandleSuggestion;
            parameters[2].Value = model.CheckResultDescription;
            parameters[3].Value = model.HandleSuggestionDescription;
            parameters[4].Value = model.CheckResultOperator;
            parameters[5].Value = model.swbdID;
           
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
        /// 修改审核结果
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool UpdateConfirmCheckStatus(Model.M_SubWayBillDetail model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update SubWaybill_Detail set IsConfirmCheck=@IsConfirmCheck,ConfirmCheckOperator=@ConfirmCheckOperator where swbdID=@swbdID");

            SqlParameter[] parameters = {
                   
                    new SqlParameter("@IsConfirmCheck",SqlDbType.Int),
                    new SqlParameter("@ConfirmCheckOperator", SqlDbType.NVarChar), 
                    new SqlParameter("@swbdID", SqlDbType.Int)
            };

            parameters[0].Value = model.IsConfirmCheck;
            parameters[1].Value = model.ConfirmCheckOperator;
            parameters[2].Value = model.swbdID;

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
        /// 修改审核结果
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool bakUpdateConfirmCheckStatus(string swbdIDs,string isConfirmCheck,string ConfirmCheckOperator)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat("update SubWaybill_Detail set IsConfirmCheck={0},ConfirmCheckOperator='{1}' where swbdID in ({2})",isConfirmCheck,ConfirmCheckOperator,swbdIDs);

            if (DBUtility.SqlServerHelper.ExecuteSql(strSql.ToString()) >= 1)
            {
                return true;
            }
            else
            {
                return false;
            }

        }


        public DataSet getSubWayBillDetialBySwbID(string swbID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat(@"select * from V_SubWayBill_SubWayBillDetail where swbID={0}", swbID);

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
        /// 根据主运单ID获取其待处理的分运单头ID字符串，如：123,125等
        /// </summary>
        /// <param name="wbID"></param>
        /// <returns></returns>
        public string getAllPendingSubWayBillsBywbID(string wbID)
        {
            StringBuilder sbRet = new StringBuilder("");
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat(@"select distinct swbID from V_SubWayBill_SubWayBillDetail where wbID={0} and (CheckResult<>-1 or HandleSuggestion<>-1)", wbID);

            DataSet ds = DBUtility.SqlServerHelper.Query(strSql.ToString());
            if (ds!=null)
            {
                DataTable dt = ds.Tables[0];
                if (dt!=null && dt.Rows.Count>0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (i != dt.Rows.Count - 1)
                        {
                            sbRet.Append(dt.Rows[i]["swbID"].ToString()+",");
                        }
                        else
                        {
                            sbRet.Append(dt.Rows[i]["swbID"].ToString());
                        }
                    }
                }
            }
            return sbRet.ToString();
        }
    }
}
