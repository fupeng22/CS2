using System;
using System.Data;
using System.Collections;
using System.Text;
using System.Data.SqlClient;
using DBUtility;
using Model;

namespace SQLDAL
{
    public class T_SubWayBill
    {
        public T_SubWayBill()
        {

        }
        //获取ID根据wbSerialNum
        public int GetSubWayBillID()
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select Max(wbID) from Waybill");

            DataSet ds = DBUtility.SqlServerHelper.Query(strSql.ToString());
            if (ds.Tables[0].Rows[0][0] != DBNull.Value)
            {
                return int.Parse(ds.Tables[0].Rows[0][0].ToString());
            }
            else
            {
                return 0;
            }
        }
        //获取放行件数
        public int GetReleseNum(int wbID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(swbID) from V_Distinct_SubWayBill");
            strSql.Append(" WHERE (swbNeedCheck != 3) and swb_wbID=" + wbID + "");
            //strSql.Append(" WHERE (swbNeedCheck = 0 or swbNeedCheck=2) and swb_wbID=" + wbID + "");


            DataSet ds = DBUtility.SqlServerHelper.Query(strSql.ToString());
            if (ds.Tables[0].Rows.Count > 0)
            {
                return int.Parse(ds.Tables[0].Rows[0][0].ToString());
            }
            else
            {
                return 0;
            }
        }
        //获取扣留件数
        public int GetSaveNum(int wbID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(swbID) from V_Distinct_SubWayBill");
            strSql.Append(" WHERE (swbNeedCheck = 3) and swb_wbID=" + wbID + "");


            DataSet ds = DBUtility.SqlServerHelper.Query(strSql.ToString());
            if (ds.Tables[0].Rows.Count > 0)
            {
                return int.Parse(ds.Tables[0].Rows[0][0].ToString());
            }
            else
            {
                return 0;
            }
        }

        //获取扣留件数
        public DataSet GetSave(int wbID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select swbID,swbSerialNum,swbDescription_CHN,swbNumber,swbWeight,swbActualWeight,swbValue,swbRecipients from V_Distinct_SubWayBill");
            strSql.Append(" WHERE (swbNeedCheck = 3) and swb_wbID=" + wbID + "");


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
        //新增
        public bool addSubWayBill(Model.M_SubWayBill model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into SubWaybill");
            strSql.Append(" (swb_wbID,swbSerialNum,swbDescription_CHN,swbDescription_ENG,swbNumber,swbWeight,swbValue,swbMonetary,swbRecipients,swbCustomsCategory,swbValueDetail,Sender,ReceiverIDCard,ReceiverPhone,EmailAddress,PickGoodsAgain)");
            strSql.Append(" values (");
            strSql.Append("@swb_wbID,@swbSerialNum,@swbDescription_CHN,@swbDescription_ENG,@swbNumber,@swbWeight,@swbValue,@swbMonetary,@swbRecipients,@swbCustomsCategory,@swbValueDetail,@Sender,@ReceiverIDCard,@ReceiverPhone,@EmailAddress,@PickGoodsAgain)");

            SqlParameter[] parameters = {
                   
                    new SqlParameter("@swb_wbID",SqlDbType.Int),
                    new SqlParameter("@swbSerialNum", SqlDbType.VarChar),
                    new SqlParameter("@swbDescription_CHN", SqlDbType.VarChar), 
                    new SqlParameter("@swbDescription_ENG", SqlDbType.VarChar),
                    new SqlParameter("@swbNumber", SqlDbType.Int),
                    new SqlParameter("@swbWeight", SqlDbType.Real),
                    new SqlParameter("@swbValue", SqlDbType.Real),
                    new SqlParameter("@swbMonetary", SqlDbType.VarChar),
                    new SqlParameter("@swbRecipients", SqlDbType.VarChar),
                    new SqlParameter("@swbCustomsCategory", SqlDbType.VarChar),
                    new SqlParameter("@swbValueDetail", SqlDbType.VarChar),
                    new SqlParameter("@Sender", SqlDbType.NVarChar),
                    new SqlParameter("@ReceiverIDCard", SqlDbType.NVarChar),
                    new SqlParameter("@ReceiverPhone", SqlDbType.NVarChar),
                    new SqlParameter("@EmailAddress", SqlDbType.NVarChar),
                    new SqlParameter("@PickGoodsAgain", SqlDbType.Int)
            };
            parameters[0].Value = model.Swb_wbID;
            parameters[1].Value = model.SwbSerialNum;
            parameters[2].Value = model.SwbDescription_CHN;
            parameters[3].Value = model.SwbDescription_ENG;
            parameters[4].Value = model.SwbNumber;
            parameters[5].Value = model.SwbWeight;
            parameters[6].Value = model.SwbValue;
            parameters[7].Value = model.SwbMonetary;
            parameters[8].Value = model.SwbRecipients;
            parameters[9].Value = model.SwbCustomsCategory;
            parameters[10].Value = model.swbValueDetail == null ? "" : model.swbValueDetail;
            parameters[11].Value = model.Sender;
            parameters[12].Value = model.ReceiverIDCard;
            parameters[13].Value = model.ReceiverPhone;
            parameters[14].Value = model.EmailAddress;
            parameters[15].Value = model.PickGoodsAgain;

            if (DBUtility.SqlServerHelper.ExecuteSql(strSql.ToString(), parameters) >= 1)
            {
                //更新库存表中先前插入的未预入库记录信息
                //new T_WayBillFlow().FillwbIDswbID(model.SwbSerialNum);

                return true;
            }
            else
            {
                return false;

            }

        }

        /// <summary>
        /// 修改分运单头信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool updateSubWayBill(Model.M_SubWayBill model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update SubWaybill set swb_wbID=@swb_wbID,swbSerialNum=@swbSerialNum,swbDescription_CHN=@swbDescription_CHN,swbDescription_ENG=@swbDescription_ENG,swbNumber=@swbNumber,swbWeight=@swbWeight,swbValue=@swbValue,swbMonetary=@swbMonetary,swbRecipients=@swbRecipients,swbCustomsCategory=@swbCustomsCategory,swbValueDetail=@swbValueDetail,Sender=@Sender,ReceiverIDCard=@ReceiverIDCard,ReceiverPhone=@ReceiverPhone,EmailAddress=@EmailAddress,PickGoodsAgain=@PickGoodsAgain where swbID=@swbID");

            SqlParameter[] parameters = {
                   
                    new SqlParameter("@swb_wbID",SqlDbType.Int),
                    new SqlParameter("@swbSerialNum", SqlDbType.VarChar),
                    new SqlParameter("@swbDescription_CHN", SqlDbType.VarChar), 
                    new SqlParameter("@swbDescription_ENG", SqlDbType.VarChar),
                    new SqlParameter("@swbNumber", SqlDbType.Int),
                    new SqlParameter("@swbWeight", SqlDbType.Real),
                    new SqlParameter("@swbValue", SqlDbType.Real),
                    new SqlParameter("@swbMonetary", SqlDbType.VarChar),
                    new SqlParameter("@swbRecipients", SqlDbType.VarChar),
                    new SqlParameter("@swbCustomsCategory", SqlDbType.VarChar),
                    new SqlParameter("@swbValueDetail", SqlDbType.VarChar),
                    new SqlParameter("@swbID", SqlDbType.Int),
                    new SqlParameter("@Sender", SqlDbType.NVarChar),
                    new SqlParameter("@ReceiverIDCard", SqlDbType.NVarChar),
                    new SqlParameter("@ReceiverPhone", SqlDbType.NVarChar),
                    new SqlParameter("@EmailAddress", SqlDbType.NVarChar),
                    new SqlParameter("@PickGoodsAgain", SqlDbType.Int)
            };

            parameters[0].Value = model.Swb_wbID;
            parameters[1].Value = model.SwbSerialNum;
            parameters[2].Value = model.SwbDescription_CHN;
            parameters[3].Value = model.SwbDescription_ENG;
            parameters[4].Value = model.SwbNumber;
            parameters[5].Value = model.SwbWeight;
            parameters[6].Value = model.SwbValue;
            parameters[7].Value = model.SwbMonetary;
            parameters[8].Value = model.SwbRecipients;
            parameters[9].Value = model.SwbCustomsCategory;
            parameters[10].Value = model.swbValueDetail == null ? "" : model.swbValueDetail;
            parameters[11].Value = model.SwbID;
            parameters[12].Value = model.Sender;
            parameters[13].Value = model.ReceiverIDCard;
            parameters[14].Value = model.ReceiverPhone;
            parameters[15].Value = model.EmailAddress;
            parameters[16].Value = model.PickGoodsAgain;

            if (DBUtility.SqlServerHelper.ExecuteSql(strSql.ToString(), parameters) >= 1)
            {
                //更新库存表中先前插入的未预入库记录信息
                //new T_WayBillFlow().FillwbIDswbID(model.SwbSerialNum);

                return true;
            }
            else
            {
                return false;

            }

        }

        //根据swSeralNum获取WayBill
        public DataSet GetSubWayBill(int wbID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from V_Distinct_SubWayBill");
            strSql.Append(" where swb_wbID=" + wbID + " and (swbNeedCheck=0 or swbNeedCheck=1)");
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


        //获取放行件数
        public int GetNeedCheckNum(int wbID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(swbID) from V_Distinct_SubWayBill");
            strSql.Append(" WHERE (swbNeedCheck = 1) and swb_wbID=" + wbID + "");


            DataSet ds = DBUtility.SqlServerHelper.Query(strSql.ToString());
            if (ds.Tables[0].Rows.Count > 0)
            {
                return int.Parse(ds.Tables[0].Rows[0][0].ToString());
            }
            else
            {
                return 0;
            }
        }


        //更新
        public bool upDateSwbNeedCheck(int swID, string strParm, int index, int swbNeedCheck)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update SubWaybill ");
            strSql.Append("set swbNeedCheck=@swbNeedCheck");
            //全部更新
            switch (index)
            {
                case 0:
                    strSql.Append(" where swb_wbID=" + swID + " ");
                    break;
                case -1:
                    strSql.Append(" where swb_wbID=" + swID + " ");
                    strSql.Append(" and swbID not in (" + strParm + ")");
                    break;
                case 1:
                    strSql.Append(" where swb_wbID=" + swID + " ");
                    strSql.Append(" and swbID  in (" + strParm + ")");
                    break;
                case 2:
                    strSql.Append(" where swbID  in (" + strParm + ")");
                    break;
            }
            SqlParameter[] parameters = {
                    new SqlParameter("@swbNeedCheck", SqlDbType.Int)
                                      };

            parameters[0].Value = swbNeedCheck;
            if (DBUtility.SqlServerHelper.ExecuteSql(strSql.ToString(), parameters) >= 1)
            {
                return true;
            }
            else
            {
                return false;
            }



        }

        //解控
        public bool outOfControlSubWayBill(string swbIds)
        {
            StringBuilder strSql_Reject = new StringBuilder();
            StringBuilder strSql_UnRelease = new StringBuilder();
            strSql_Reject.Append("update SubWaybill set swbNeedCheck=3 where swbID in (" + swbIds + ") and swbNeedCheck=99");
            strSql_UnRelease.Append("update SubWaybill set swbNeedCheck=0 where swbID in (" + swbIds + ") and swbNeedCheck=3");
            ArrayList alSQL = new ArrayList();
            alSQL.Add(strSql_UnRelease.ToString());
            alSQL.Add(strSql_Reject.ToString());
            try
            {
                DBUtility.SqlServerHelper.ExecuteSqlTran(alSQL);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        //获取称重总重量
        public double GetTotalActualWeight(int wbID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select SUM(swbActualWeight) from V_Distinct_SubWayBill");
            strSql.Append(" where swb_wbID=" + wbID + "");
            DataSet ds = DBUtility.SqlServerHelper.Query(strSql.ToString());
            if (ds.Tables[0].Rows[0][0] != DBNull.Value)
            {
                return double.Parse(ds.Tables[0].Rows[0][0].ToString());
            }
            else
            {
                return 0;
            }
        }

        //获取称重总重量
        public int GetActualSubNum(int wbID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(swbID) from V_Distinct_SubWayBill");
            strSql.Append(" where swb_wbID=" + wbID + "");
            DataSet ds = DBUtility.SqlServerHelper.Query(strSql.ToString());
            if (ds.Tables[0].Rows[0][0] != DBNull.Value)
            {
                return int.Parse(ds.Tables[0].Rows[0][0].ToString());
            }
            else
            {
                return 0;
            }
        }

        //获取实际放行数量
        public int GetActualReleseNum(int wbID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(swbNeedCheck) from V_Distinct_SubWayBill");
            strSql.Append(" where swb_wbID=" + wbID + " and (swbNeedCheck=0 or swbNeedCheck=2) and swbSortingTime is not null");
            DataSet ds = DBUtility.SqlServerHelper.Query(strSql.ToString());
            if (ds.Tables[0].Rows[0][0] != DBNull.Value)
            {
                return int.Parse(ds.Tables[0].Rows[0][0].ToString());
            }
            else
            {
                return 0;
            }
        }

        //获取实际扣留数量
        public int GetActualNotReleseNum(int wbID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(swbNeedCheck) from V_Distinct_SubWayBill");
            strSql.Append(" where swb_wbID=" + wbID + " and  swbNeedCheck=3 ");
            DataSet ds = DBUtility.SqlServerHelper.Query(strSql.ToString());
            if (ds.Tables[0].Rows[0][0] != DBNull.Value)
            {
                return int.Parse(ds.Tables[0].Rows[0][0].ToString());
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// 获取退货数量
        /// </summary>
        /// <param name="wbID"></param>
        /// <returns></returns>
        public int GetActualRejectNum(int wbID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(swbNeedCheck) from V_Distinct_SubWayBill");
            strSql.Append(" where swb_wbID=" + wbID + " and  swbNeedCheck=99 ");
            DataSet ds = DBUtility.SqlServerHelper.Query(strSql.ToString());
            if (ds.Tables[0].Rows[0][0] != DBNull.Value)
            {
                return int.Parse(ds.Tables[0].Rows[0][0].ToString());
            }
            else
            {
                return 0;
            }
        }

        //获取上机数量

        public int GetActualNotProNum(int wbID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(swb_wbID) from V_Distinct_SubWayBill");
            strSql.Append(" where swbSortingTime is null and swb_wbID=" + wbID + "");
            DataSet ds = DBUtility.SqlServerHelper.Query(strSql.ToString());
            if (ds.Tables[0].Rows[0][0] != DBNull.Value)
            {
                return int.Parse(ds.Tables[0].Rows[0][0].ToString());
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// 根据总运单ID获取其子运单信息
        /// </summary>
        /// <param name="wbID"></param>
        /// <returns></returns>
        public DataSet GetSubWayBillInfoBywbID(int wbID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat(@"select * from V_Distinct_SubWayBill where swb_wbID={0}", wbID);
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
        /// 获取所有子运单信息
        /// </summary>
        /// <param name="wbID"></param>
        /// <returns></returns>
        public DataSet GetAllSubWayBillInfo()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"select * from V_Distinct_SubWayBill");
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
        /// 根据总运单ID得到该总运单下的所有子运单的汇总信息，如：总重量、总数量、实际总重量
        /// </summary>
        /// <param name="wbID"></param>
        /// <returns></returns>
        public DataSet GetSubWayBillSumInfo(int wbID)
        {

            StringBuilder strSql = new StringBuilder();
            //            strSql.AppendFormat(@"select  swb_wbID,
            //                                SUM(case swbActualNumber when null then 0 else  swbActualNumber end) as swbTotalActualNumber,
            //                                COUNT(swbID) as swbTotalNumber,
            //                                SUM(case swbWeight when null then 0 else  cast(round(swbWeight,2) as  numeric(30,2)) end) as swbTotalWeight, 
            //                                SUM(case swbActualWeight when null then 0 else  cast(round(swbActualWeight,2) as  numeric(30,2)) end) as swbTotalActualWeight
            //                                from V_Distinct_SubWayBill  group by swb_wbID  having swb_wbID={0}", wbID);
            //            strSql.AppendFormat(@"select  wbID as swb_wbID,
            //                                SUM(case swbActualNumber when null then 0 else  swbActualNumber end) as swbTotalActualNumber,
            //                                COUNT(swbID) as swbTotalNumber,
            //                                SUM(case swbWeight when null then 0 else  cast(round(swbWeight,2) as  numeric(30,2)) end) as swbTotalWeight, 
            //                                SUM(case swbActualWeight when null then 0 else  cast(round(swbActualWeight,2) as  numeric(30,2)) end) as swbTotalActualWeight
            //                                from V_SubWayBill_SubWayBillDetail  group by wbID  having wbID={0}", wbID);
            //            strSql.AppendFormat(@"select  wbID as swb_wbID,
            //                                SUM(case swbActualNumber when null then 0 else  swbActualNumber end) as swbTotalActualNumber,
            //                                COUNT(swbID) as swbTotalNumber,
            //                                SUM(case swbWeight when null then 0 else  cast(round(swbWeight,2) as  numeric(30,2)) end) as swbTotalWeight, 
            //                                SUM(case swbActualWeight when null then 0 else  cast(round(swbActualWeight,2) as  numeric(30,2)) end) as swbTotalActualWeight
            //                                from V_SubWayBill_SubWayBillDetail  group by wbID  having wbID={0}", wbID);
            strSql.AppendFormat(@"  select a.swb_wbID,count(swbID) as swbTotalNumber,sum(a.swbTotalActualNumber) as swbTotalActualNumber,sum(a.swbTotalWeight) as swbTotalWeight,sum(a.swbTotalActualWeight) as swbTotalActualWeight
                                    from
                                    (
                                    select wbID as swb_wbID,swbID,
                                    SUM(case swbActualNumber when null then 0 else  swbActualNumber end) as swbTotalActualNumber,
                                    SUM(case swbWeight when null then 0 else  cast(round(swbWeight,2) as  numeric(30,2)) end) as swbTotalWeight,
                                    SUM(case swbActualWeight when null then 0 else  cast(round(swbActualWeight,2) as  numeric(30,2)) end) as swbTotalActualWeight
                                     from V_SubWayBill_SubWayBillDetail group by wbID,swbID
                                     ) a group by swb_wbID  having swb_wbID={0}", wbID);
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

        public Boolean TestExistSWBSerialNumInOtherWayBill(string wbSerialNum, string swbSerialNum)
        {
            Boolean bExist = false;
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat(@"select count(0) from V_WayBill_SubWayBill where swbSerialNum='{0}' and wbSerialNum<>'{1}'", swbSerialNum, wbSerialNum);
            DataSet ds = DBUtility.SqlServerHelper.Query(strSql.ToString());
            if (Convert.ToInt32(ds.Tables[0].Rows[0][0]) > 0)
            {
                bExist = true;
            }
            return bExist;
        }

        public Boolean TestExistSwbSerialNum_Create(int wbId, string swbSerialNum)
        {
            Boolean bExist = false;
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat(@"select count(0) from V_WayBill_SubWayBill where swbSerialNum='{0}' and wbId={1}", swbSerialNum, wbId.ToString());
            DataSet ds = DBUtility.SqlServerHelper.Query(strSql.ToString());
            if (Convert.ToInt32(ds.Tables[0].Rows[0][0]) > 0)
            {
                bExist = true;
            }
            return bExist;
        }

        public Boolean TestExistSwbSerialNum_Update(int swbId, int wbId, string swbSerialNum)
        {
            Boolean bExist = false;
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat(@"select count(0) from V_WayBill_SubWayBill where swbSerialNum='{0}' and wbId={1} and swbId<>{2}", swbSerialNum, wbId.ToString(), swbId.ToString());
            DataSet ds = DBUtility.SqlServerHelper.Query(strSql.ToString());
            if (Convert.ToInt32(ds.Tables[0].Rows[0][0]) > 0)
            {
                bExist = true;
            }
            return bExist;
        }

        //更新

        /// <summary>
        /// 删除指定ID的分运单信息
        /// </summary>
        /// <param name="swID"></param>
        /// <param name="strParm"></param>
        /// <param name="index"></param>
        /// <param name="swbNeedCheck"></param>
        /// <returns></returns>
        public bool DeleteSubWayBillByID(string swbIDs)
        {
            //StringBuilder strSql = new StringBuilder();
            //DBUtility.SqlServerHelper.ExecuteSqlTran();
            ArrayList alSQL = new ArrayList();
            alSQL.Add("delete from SubWaybill_Detail where swbd_swbID in (" + swbIDs + ")");
            alSQL.Add("delete from SubWaybill  where swbID in (" + swbIDs + ")");
            //strSql.Append("delete from SubWaybill  where swbID in (" + swbIDs + ")");

            //if (DBUtility.SqlServerHelper.ExecuteSqlTran(alSQL) >= 1)
            //{
            //    return true;
            //}
            //else
            //{
            //    return false;
            //}
            bool bOK = false;
            try
            {
                DBUtility.SqlServerHelper.ExecuteSqlTran(alSQL);
                bOK = true;
            }
            catch (Exception ex)
            {

            }
            return bOK;
        }

        /// <summary>
        /// 删除指定总运单ID的分运单信息
        /// </summary>
        /// <param name="swID"></param>
        /// <param name="strParm"></param>
        /// <param name="index"></param>
        /// <param name="swbNeedCheck"></param>
        /// <returns></returns>
        public bool DeleteSubWayBillByWBID(string wbIDs)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from SubWaybill  where swb_wbID in (" + wbIDs + ")");

            if (DBUtility.SqlServerHelper.ExecuteSql(strSql.ToString()) >= 1)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        public bool upDateSwbNeedCheck(string swbIds, string iNeedCheck)
        {
            StringBuilder strSql = new StringBuilder();
            switch (iNeedCheck)
            {
                case "2":
                    strSql.Append("update SubWaybill ");
                    strSql.Append("set ReleaseDate=getdate(),swbNeedCheck=" + iNeedCheck + " where swbId in (" + swbIds + ")");
                    break;
                case "3":
                    strSql.Append("update SubWaybill ");
                    strSql.Append("set DetainDate=getdate(),swbNeedCheck=" + iNeedCheck + " where swbId in (" + swbIds + ")");
                    break;
                case "4":
                    strSql.Append("update SubWaybill ");
                    strSql.Append("set InHandleDate=getdate(),swbNeedCheck=" + iNeedCheck + " where swbId in (" + swbIds + ")");
                    break;
                case "99":
                    strSql.Append("update SubWaybill ");
                    strSql.Append("set RejectDate=getdate(),swbNeedCheck=" + iNeedCheck + " where swbId in (" + swbIds + ")");
                    break;
                default:
                    break;
            }

            if (DBUtility.SqlServerHelper.ExecuteSql(strSql.ToString()) >= 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool RejectSubWayBill(string swbIds, string Operator)
        {
            bool bOK = false;
            ArrayList arrSQLList = new ArrayList();
            DataSet ds = DBUtility.SqlServerHelper.Query("select * from subwaybill where swbId in(" + swbIds + ")");
            if (ds != null)
            {
                DataTable dt = ds.Tables[0];
                if (dt != null && dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        arrSQLList.Add(string.Format(@"INSERT INTO RejectSubWayBill
                            ([swbID]
                            ,[swb_wbID]
                            ,[swbSerialNum]
                            ,[swbDescription_CHN]
                            ,[swbDescription_ENG]
                            ,[swbNumber]
                            ,[swbWeight]
                            ,[swbActualWeight]
                            ,[swbSortingTime]
                            ,[swbNeedCheck]
                            ,[swbXRayWebSiteIPAndPort]
                            ,[swbImageLocalPath]
                            ,[swbCheckDescription]
                            ,[swbDelFlag]
                            ,[swbValue]
                            ,[swbMonetary]
                            ,[swbRecipients]
                            ,[swbCustomsCategory]
                            ,[swbValueDetail]
                            ,[DetainDate]
                            ,[ReleaseDate]
                            ,[InHandleDate]
                            ,[swbActualNumber]
                            ,[RejectDate]
                            ,[Operator]
                            ,[mMemo])
                        VALUES
                            ({0}
                            ,{1}
                            ,'{2}'
                            ,'{3}'
                            ,'{4}'
                            ,{5}
                            ,{6}
                            ,{7}
                            ,'{8}'
                            ,{9}
                            ,'{10}'
                            ,'{11}'
                            ,'{12}'
                            ,{13}
                            ,{14}
                            ,'{15}'
                            ,'{16}'
                            ,'{17}'
                            ,'{18}'
                            ,'{19}'
                            ,'{20}'
                            ,'{21}'
                            ,{22}
                            ,getDate()
                            ,'{23}'
                            ,'')", dt.Rows[i]["swbID"] == DBNull.Value ? "-1" : dt.Rows[i]["swbID"].ToString()
                             , dt.Rows[i]["swb_wbID"] == DBNull.Value ? "-1" : dt.Rows[i]["swb_wbID"].ToString()
                             , dt.Rows[i]["swbSerialNum"] == DBNull.Value ? "" : dt.Rows[i]["swbSerialNum"].ToString()
                             , dt.Rows[i]["swbDescription_CHN"] == DBNull.Value ? "" : dt.Rows[i]["swbDescription_CHN"].ToString()
                             , dt.Rows[i]["swbDescription_ENG"] == DBNull.Value ? "" : dt.Rows[i]["swbDescription_ENG"].ToString()
                             , dt.Rows[i]["swbNumber"] == DBNull.Value ? "0" : dt.Rows[i]["swbNumber"].ToString()
                             , dt.Rows[i]["swbWeight"] == DBNull.Value ? "0" : dt.Rows[i]["swbWeight"].ToString()
                             , dt.Rows[i]["swbActualWeight"] == DBNull.Value ? "0" : dt.Rows[i]["swbActualWeight"].ToString()
                             , dt.Rows[i]["swbSortingTime"] == null ? "" : dt.Rows[i]["swbSortingTime"].ToString()
                             , dt.Rows[i]["swbNeedCheck"] == DBNull.Value ? "-1" : dt.Rows[i]["swbNeedCheck"].ToString()
                             , dt.Rows[i]["swbXRayWebSiteIPAndPort"] == DBNull.Value ? "" : dt.Rows[i]["swbXRayWebSiteIPAndPort"].ToString()
                             , dt.Rows[i]["swbImageLocalPath"] == DBNull.Value ? "" : dt.Rows[i]["swbImageLocalPath"].ToString()
                             , dt.Rows[i]["swbCheckDescription"] == DBNull.Value ? "" : dt.Rows[i]["swbCheckDescription"].ToString()
                             , dt.Rows[i]["swbDelFlag"] == DBNull.Value ? "-1" : dt.Rows[i]["swbDelFlag"].ToString()
                             , dt.Rows[i]["swbValue"] == DBNull.Value ? "0" : dt.Rows[i]["swbValue"].ToString()
                             , dt.Rows[i]["swbMonetary"] == DBNull.Value ? "" : dt.Rows[i]["swbMonetary"].ToString()
                             , dt.Rows[i]["swbRecipients"] == DBNull.Value ? "" : dt.Rows[i]["swbRecipients"].ToString()
                             , dt.Rows[i]["swbCustomsCategory"] == DBNull.Value ? "" : dt.Rows[i]["swbCustomsCategory"].ToString()
                             , dt.Rows[i]["swbValueDetail"] == DBNull.Value ? "" : dt.Rows[i]["swbValueDetail"].ToString()
                             , dt.Rows[i]["DetainDate"] == null ? "" : dt.Rows[i]["DetainDate"].ToString()
                             , dt.Rows[i]["ReleaseDate"] == null ? "" : dt.Rows[i]["ReleaseDate"].ToString()
                             , dt.Rows[i]["InHandleDate"] == null ? "" : dt.Rows[i]["InHandleDate"].ToString()
                             , dt.Rows[i]["swbActualNumber"] == DBNull.Value ? "0" : dt.Rows[i]["swbActualNumber"].ToString()
                             , Operator
                             ));
                    }
                    arrSQLList.Add("delete from subwaybill where swbId in(" + swbIds + ")");
                    SqlServerHelper.ExecuteSqlTran(arrSQLList);
                    bOK = true;
                }
            }
            return bOK;
        }

        /// <summary>
        /// 根据总运单号查询其所有退货分运单信息
        /// </summary>
        /// <param name="wbSerialNum"></param>
        /// <returns></returns>
        public DataSet getRejectSubWayBillInfo(string wbSerialNum)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat(@"select * from V_WayBill_RejectSubWayBill where wbSerialNum='{0}'", wbSerialNum);
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
        /// 根据总运单ID查询总运单子运单信息
        /// </summary>
        /// <param name="wbID"></param>
        /// <returns></returns>
        public DataSet getWayBill_SubWayBill(string wbID, int iNeedCheck)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat(@"select * from V_WayBill_SubWayBill where wbID={0}", wbID);
            switch (iNeedCheck)
            {
                case -1:
                    break;
                default:
                    strSql.Append(" and swbNeedCheck in (" + iNeedCheck.ToString() + ")");
                    break;
            }
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

        public DataSet getRejectSubWayBill(string wbID, string beginDT, string endDT)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat(@"select * from V_WayBill_SubWayBill where wbID={0} and swbNeedCheck=99 and convert(nvarchar(10),RejectDate,120)>='{1}' and convert(nvarchar(10),RejectDate,120)<='{2}'", wbID, Convert.ToDateTime(beginDT).ToString("yyyy-MM-dd"), Convert.ToDateTime(endDT).ToString("yyyy-MM-dd"));

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
        /// 根据分运单单号信息查询分运单信息
        /// </summary>
        /// <param name="strSerialNums"></param>
        /// <returns></returns>
        public DataSet getWayBill_SubWayBill(string wbID, string strSerialNums)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat(@"select * from V_WayBill_SubWayBill where  wbID={0} and swbSerialNum in ({1})", wbID, strSerialNums);

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

        public int IsPickGoodsAgain(string IDCard)
        {
            int iRet = 0;
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat("select count(0) from V_WayBill_SubWayBill where PickGoodsAgain=0 and ReceiverIDCard='" + IDCard + "' and (wbStorageDate>convert(varchar, DATEADD(day,-15,getdate()), 112) and wbStorageDate<convert(varchar, getdate(), 112) )");

            DataSet ds = DBUtility.SqlServerHelper.Query(strSql.ToString());
            if (Convert.ToInt32(ds.Tables[0].Rows[0][0]) > 0)
            {
                iRet = 1;
            }

            return iRet;
        }

        public string CreateCategoryByType(string iType)
        {
            string strRet = "";

            try
            {
                int typeIndex = -1;
                typeIndex = Convert.ToInt32(iType);
                switch (typeIndex)
                {
                    case 2:
                        strRet = "样品";
                        break;
                    case 3:
                        strRet = "KJ-3";
                        break;
                    case 4:
                        strRet = "D类";
                        break;
                    case 5:
                        strRet = "个人物品";
                        break;
                    case 6:
                        strRet = "分运行李";
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {

            }

            return strRet;
        }

        /// <summary>
        /// 自动重新计算税金合计值
        /// </summary>
        /// <param name="swbId"></param>
        /// <returns></returns>
        public bool updateSwbValue(string swbId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"update SubWaybill set swbValue=(select SUM(TaxValue) from SubWaybill_Detail where swbd_swbID=SubWaybill.swbID)
                            where swbID=" + swbId);

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
        ///更新税费审核信息
        /// </summary>
        /// <param name="swbId"></param>
        /// <param name="TaxValueCheck"></param>
        /// <param name="TaxValueCheckOperator"></param>
        /// <returns></returns>
        public bool updateTaxValueCheckInfo(string swbId, string TaxValueCheck, string TaxValueCheckOperator)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update SubWaybill set TaxValueCheck=@TaxValueCheck,TaxValueCheckOperator=@TaxValueCheckOperator where swbID=@swbID");

            SqlParameter[] parameters = {
                   
                    new SqlParameter("@TaxValueCheck",SqlDbType.Real),
                    new SqlParameter("@TaxValueCheckOperator", SqlDbType.NVarChar),
                    new SqlParameter("@swbID", SqlDbType.Int),
            };

            parameters[0].Value = Convert.ToDouble(TaxValueCheck);
            parameters[1].Value = TaxValueCheckOperator;
            parameters[2].Value = Convert.ToInt32(swbId);

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
        ///批量进行税费核准
        /// </summary>
        /// <param name="swbId"></param>
        /// <param name="TaxValueCheck"></param>
        /// <param name="TaxValueCheckOperator"></param>
        /// <returns></returns>
        public bool bakCheckTaxFee(string swbIds, string TaxValueCheckOperator)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update SubWaybill set TaxValueCheck=swbValue,TaxValueCheckOperator='" + TaxValueCheckOperator + "' where swbID in (" + swbIds + ")");

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
        /// 根据分运单ID获取其对应的身份证号上次提货信息
        /// </summary>
        /// <param name="wbID"></param>
        /// <param name="strSerialNums"></param>
        /// <returns></returns>
        public DataSet LoadLastPickGoodInfo(string strSwbId)
        {
            StringBuilder strSql = new StringBuilder();
            //strSql.AppendFormat(@"select top 1 * from V_WayBill_SubWayBill where ReceiverIDCard='{0}' and swbId>{1} order by swbId desc",IDCard,strSwbId);
            strSql.AppendFormat(@"select top 1 * from V_WayBill_SubWayBill where ReceiverIDCard in (select ReceiverIDCard from V_WayBill_SubWayBill where swbId={0}) and swbId<{0} order by swbId desc", strSwbId);

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
        /// 获取指定总运单的分运单中为进行税金核准的分运单信息
        /// </summary>
        /// <param name="wbId"></param>
        /// <returns></returns>
        public DataSet getAllTaxFeeUnCheckSubWayBillInfo(string wbId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat(@"select * from V_WayBill_SubWayBill where wbID={0} and TaxValueCheck=-1", wbId);

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
        /// 获取指定总运单号的子运单的核准税金之和
        /// </summary>
        /// <param name="wbId"></param>
        /// <returns></returns>
        public string getSumTaxFeeCheckValue(string wbId)
        {
            string strRet = "0.00";
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat(@"select sum(TaxValueCheck) from V_WayBill_SubWayBill where wbID={0}", wbId);

            DataSet ds = DBUtility.SqlServerHelper.Query(strSql.ToString());
            DataTable dt = null;
            if (ds != null)
            {
                dt = ds.Tables[0];
                if (dt != null && dt.Rows.Count > 0)
                {
                    strRet = Convert.ToDouble(dt.Rows[0][0].ToString()).ToString("0.00");
                }
            }
            return strRet;
        }
    }
}
