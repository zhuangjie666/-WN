using Kingdee.BOS;
using Kingdee.BOS.App.Data;
using Kingdee.BOS.Contracts.Report;
using Kingdee.BOS.Core.Report;
using Kingdee.BOS.Orm.DataEntity;
using Kingdee.BOS.ServiceHelper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kingdee.K3.WEINA.SCM.PlugIn.service
{
    [Description("动态库存表")]
    public class DynamicStockReport : SysReportBaseService
    {
        String tempTable = null;
        String tempTables = null;
        DynamicObjectCollection whCol = null;
        String fname;
        List<String> tempTableList = new List<string>();
        public override void Initialize()
        {
            // 支持分组汇总
            this.ReportProperty.ReportName = new LocaleValue("动态库存表", this.Context.UserLocale.LCID);
            //精度 格式化
            //     this.ReportProperty.DecimalControlFieldList.Add(new DecimalControlField() { ByDecimalControlFieldName = "CK010", DecimalControlFieldName = "famount_decimal" });
            this.ReportProperty.IdentityFieldName = "FIDENTITYID";
        }

        public override void BuilderReportSqlAndTempTable(IRptParams filter, string tableName)
        {

            DynamicObject ProdFilter = filter.FilterParameter.CustomFilter;
            //DynamicObject material =ProdFilter["F_PAEZ_Material"] as DynamicObject;
            //DynamicObject materialGroupobj = ProdFilter["F_PAEZ_BaseProperty"] as DynamicObject;

            int materialID = Convert.ToInt32(ProdFilter["F_PAEZ_Material_Id"]);
            string materialType = Convert.ToString(ProdFilter["F_PAEZ_ErpClsID"]);
            DynamicObjectCollection proCol = ProdFilter["F_PAEZ_BaseProperty"] as DynamicObjectCollection;
            string materialGroup = "";
            for (int i = 0; i < proCol.Count; i++)
            {
                if (i == 0)
                {
                    materialGroup = Convert.ToString(proCol[i]["F_PAEZ_BaseProperty_Id"]);
                }
                else
                {
                    materialGroup = materialGroup + "," + Convert.ToString(proCol[i]["F_PAEZ_BaseProperty_Id"]);
                }

            }
            // int materialGroup = Convert.ToInt32(ProdFilter["F_PAEZ_BaseProperty_Id"]);
            tempTables = "";
            tempTable = "";
            tempTableList = new List<string>();
            string whSql = " select a.FNUMBER from T_BD_STOCK a inner join T_BD_STOCKGROUP b on b.FID=a.FGROUP  WHERE b.fnumber = '02' OR a.FNUMBER IN ('CK002','CK010') order by a.fnumber ";
            whCol = DBServiceHelper.ExecuteDynamicObject(this.Context, whSql) as DynamicObjectCollection;
            for (int i = 0; i < whCol.Count; i++)
            {
                tempTable = tempTable + Convert.ToString(whCol[i]["FNUMBER"]) + ',';
                tempTableList.Add(Convert.ToString(whCol[i]["FNUMBER"]));
            }
            tempTables = tempTable.Substring(0, tempTable.Length - 1);
            //and a.FStockStatusId = 10000 库存状态可用
            string AllFilter = @"/*dialect*/
SELECT
	p.*, 

(
		SELECT
			SUM (a.FBaseQty)
		FROM
			T_STK_INVENTORY a
		INNER JOIN T_BD_STOCK b ON a.fstockid = b.fstockid  and b.FNUMBER IN('CK002','CK010') 
		WHERE
			a.FMaterialId = p.FMaterialId and  a.FStockStatusId = 10000
	) AS PRODTOTAL,
(
		SELECT
			SUM (a.FBaseQty)
		FROM
			T_STK_INVENTORY a
		INNER JOIN T_BD_STOCK b ON a.fstockid = b.fstockid
        inner join T_BD_STOCKGROUP c on c.FID=b.FGROUP  
		WHERE	a.FMaterialId = p.FMaterialId
		AND c.FNUMBER = '02'  and a.FStockStatusId = 10000
	) AS SUBSTOCKTOTAL,
(
		SELECT
			SUM (a.FBaseQty)
		FROM
			T_STK_INVENTORY a
		INNER JOIN T_BD_STOCK b ON a.fstockid = b.fstockid
		WHERE
			a.FMaterialId = p.FMaterialId
		AND b.FNUMBER IN('{2}') and a.FStockStatusId = 10004
	) AS onTheWay,
(
		SELECT
			case when SUM (a.FBaseQty) is null then 0 else  SUM (a.FBaseQty) end 
		FROM
			T_STK_INVENTORY a
		INNER JOIN T_BD_STOCK b ON a.fstockid = b.fstockid
		WHERE
			a.FMaterialId = p.FMaterialId
		AND b.FNUMBER IN('{2}') and a.FStockStatusId in (10000,10004)
	) AS TOTALALL,
(
		SELECT
            
		  case when	( SUM(c.FBASEREMAINOUTQTY) ) is null then 0 else  SUM(c.FBASEREMAINOUTQTY) end
		FROM
			T_SAL_ORDERENTRY a 
        INNER JOIN T_SAL_ORDER b ON a.FID=b.FID and b.FDOCUMENTSTATUS = 'C'
        LEFT JOIN T_SAL_ORDERENTRY_R c on a.FEntryID=c.FEntryID
		WHERE
			a.FMaterialId = p.FMaterialId  and c.FBASEREMAINOUTQTY>0
	) AS LEFTSALEORDERTOTAL,
0  AS LACKTOTAL,

(
        SELECT 
            SUM(c.FNOSTOCKINQTY)
        FROM
			T_PRD_MOENTRY a 
            inner JOIN T_PRD_MO b on a.fid =b.fid  and  b.FDOCUMENTSTATUS='C'
            inner JOIN T_PRD_MOENTRY_Q c on c.FEntryId= a.FEntryId
            inner JOIN T_PRD_MOENTRY_A d on d.FEntryId= a.FEntryId and d.FSTATUS in (2,3,4,5)
		WHERE
			a.FMaterialId = p.FMaterialId 
	) AS  PRODPLANTOTAL,
    0 AS PRODTOTALAMT,
    0 AS SUBSTOCKTOTALAMT,
    0 AS onTheWayAMT,
    0 AS TOTALALLAMT,
    0 AS LEFTSALEORDERTOTALAMT,
    0 AS LACKTOTALAMT,
";
            string others = "";
            for (int i = 0; i < whCol.Count; i++)
            {
                string othersql = "0 as  " + Convert.ToString(whCol[i]["FNUMBER"]) + "notouttoal, " +
                   " 0 AS " + Convert.ToString(whCol[i]["FNUMBER"]) + "notoutamt,  " +
                   " 0 AS " + Convert.ToString(whCol[i]["FNUMBER"]) + "amt,  ";
                othersql = string.Format(othersql);
                others = others + othersql;
            }


            string leftsql = " row_number() over (ORDER BY P.FMaterialId) as FIDENTITYID into {0} FROM ( SELECT  " +
                                                 "A.FMaterialId as FMaterialId, " +
                                                 "A.FNUMBER AS MATERIALFNUMBER," +
                                                  "A.foldnumber as foldnumber," +
                                                     "T5.FNAME as FMATERIALGROUP," +
                                                  "T4.fid as FMATERIALGROUPID," +
                                                  "t21.FNAME as FNAME,  " +
                                                  "A.F_kd_Text as FHANYU," +
                                                  "isnull(t6.FREFCOST,0) as FREFCOST," +
                                                  "t22.FErpClsID AS FErpClsID," +
                                                  "CASE t22.FErpClsID" +
                                                 " WHEN  '1' THEN '外购'" +
                                                 " WHEN  '2'  THEN '自制'" +
                                                 " WHEN '3'  THEN '委外'" +
                                                 " WHEN '9'  THEN '配置'" +
                                                 " WHEN '10'  THEN '资产'" +
                                                 " WHEN '4'  THEN '特征'" +
                                                 " WHEN '11'  THEN '费用'" +
                                                 " WHEN '5'  THEN '虚拟'" +
                                                 " WHEN '12'  THEN '模型'" +
                                                 " WHEN '6'  THEN '服务'" +
                                                 " WHEN '7'  THEN '一次性'" +
                                                 " ELSE '其他' END " +
                                                 " AS FErpClsName,	" +
                                                  "t26.FPrecision as  JinDu," +
                                                    "B.*  from T_BD_MATERIAL  A LEFT JOIN  (SELECT " +
                                                  "T1.FMaterialId as BFMaterialId, " +
                                                  "t3.fnumber as fnumber," +
                                                  "sum(t1.FBaseQty) as FBaseQty" +
                                             " FROM	  T_STK_INVENTORY T1 " +
                                              " INNER JOIN T_BD_MATERIAL t2 on T1.FMATERIALID = t2.FMATERIALID" +
                                              " LEFT JOIN T_BD_STOCK t3 on t1.fstockid=t3.fstockid" +
                                             " WHERE  T2.FFORBIDSTATUS = 'A'  and t2.FDocumentStatus='C' and t1.FStockStatusId = 10000 and ( T2.FNUMBER like 'P%' or T2.FNUMBER like 'S%' or T2.FNUMBER like 'Z%') " +
                                             " group by T1.FMaterialId,t3.fnumber,T1.FID" +
                                              "    ) t  " +
                                              " PIVOT (" +
                                              "       sum(FBaseQty) FOR fnumber IN({1}) ) AS B  ON  A.FMaterialId=B.BFMaterialId " +
                                              "INNER JOIN T_BD_MATERIAL_L t21 on t21.FMATERIALID = A.FMATERIALID  and t21.FLOCALEID=2052" +
                                              " INNER JOIN t_BD_MaterialBase t22 on t22.fmaterialID = A.FMATERIALID" +
                                              " INNER JOIN T_BD_UNIT t26 on t26.FUNITID = t22.FBASEUNITID" +
                                              " LEFT JOIN T_BD_MATERIALGROUP T4 ON A.FMATERIALGROUP=T4.FID" +
                                              " LEFT JOIN T_BD_MATERIALGROUP_L T5 ON T5.FID=T4.FID  and t5.FLOCALEID=2052" +
                                              " LEFT JOIN t_BD_MaterialStock T6 ON T6.FMATERIALID=A.FMATERIALID " +
                                              " WHERE  A.FFORBIDSTATUS = 'A'  and A.FDocumentStatus='C' and ( A.FNUMBER like 'P%' or A.FNUMBER like 'S%' or A.FNUMBER like 'Z%') ) AS p ";



            string sqlFilterAll = "";
            string sqlall = AllFilter + others + leftsql;
            if (materialID != 0 || materialType != "" || materialGroup != "")
            {
                sqlall = sqlall + " WHERE ";
            }

            if (materialID != 0)
            {
                sqlall = sqlall + "P.FMaterialId = {3}";
                sqlFilterAll = string.Format(sqlall, tableName, tempTables, string.Join("','", tempTableList), materialID);
            }

            if (materialType != "")
            {
                if (materialID != 0)
                {
                    sqlall = sqlall + "AND P.FErpClsID = '{4}'";
                    sqlFilterAll = string.Format(sqlall, tableName, tempTables, string.Join("','", tempTableList), materialID, materialType);
                }
                else
                {
                    sqlall = sqlall + " P.FErpClsID = '{3}'";
                    sqlFilterAll = string.Format(sqlall, tableName, tempTables, string.Join("','", tempTableList), materialType);
                }
            }

            if (materialGroup != "")
            {
                if (materialType != "")
                {
                    if (materialID != 0)
                    {
                        sqlall = sqlall + " AND P.FMATERIALGROUPID in ({5})";
                        sqlFilterAll = string.Format(sqlall, tableName, tempTables, string.Join("','", tempTableList), materialID, materialType, materialGroup);
                    }
                    else
                    {
                        sqlall = sqlall + " AND P.FMATERIALGROUPID in ({4})";
                        sqlFilterAll = string.Format(sqlall, tableName, tempTables, string.Join("','", tempTableList), materialType, materialGroup);
                    }
                }
                else
                    if (materialID != 0)
                    {
                        sqlall = sqlall + " AND  P.FMATERIALGROUPID in ({4})";
                        sqlFilterAll = string.Format(sqlall, tableName, tempTables, string.Join("','", tempTableList), materialID, materialGroup);
                    }
                    else
                    {
                        sqlall = sqlall + "  P.FMATERIALGROUPID in ({3})";
                        sqlFilterAll = string.Format(sqlall, tableName, tempTables, string.Join("','", tempTableList), materialGroup);
                    }
            }



            if (materialID == 0 && materialType == "" && materialGroup == "")
            {
                sqlFilterAll = string.Format(sqlall, tableName, tempTables, string.Join("','", tempTableList));
            }

            DBUtils.Execute(this.Context, sqlFilterAll);


            List<SqlObject> updateNotTotalList = new List<SqlObject>();
            List<SqlObject> updateNotTotalList2 = new List<SqlObject>();
            List<SqlObject> updateamtList = new List<SqlObject>();

            for (int i = 0; i < whCol.Count; i++)
            {
                string clonumName = Convert.ToString(whCol[i]["FNUMBER"]);
                if (clonumName == "CK002" || clonumName == "CK010")
                {
                    continue;
                }
                else
                { // 更新分公司 未出数量 字段  剩余未出数量（销售基本） FBASEREMAINOUTQTY   sum(a.FQty) - sum(c.FSTOCKOUTQTY)

                    string updateothersql = "merge into  {0}  temp  using ( SELECT a.FMaterialId, sum(c.FBASEREMAINOUTQTY) as qqty  FROM  T_SAL_ORDERENTRY a " +
                    " LEFT JOIN T_SAL_ORDER b ON a.FID=b.FID and b.FDOCUMENTSTATUS = 'C'  " +
                    " LEFT JOIN T_SAL_ORDERENTRY_R c ON a.FEntryID = c.FEntryID " +
                    " LEFT JOIN T_BD_STOCK  d  on b.F_KD_BASE =d.fstockid where   c.FBASEREMAINOUTQTY>0 and d.FNUMBER = '{1}'  " +
                    " group by  a.FMaterialId ) t2  on temp.FMaterialId=t2.FMaterialId" +
                    " when matched then update set temp." + Convert.ToString(whCol[i]["FNUMBER"]) + "notouttoal =  isnull(t2.qqty,0) ";
                    //"notouttoal = case when isnull(t2.qqty,0)-isnull(temp." + Convert.ToString(whCol[i]["FNUMBER"]) +
                    //"  ,0) >0 then isnull(t2.qqty,0)-isnull(temp." + Convert.ToString(whCol[i]["FNUMBER"]) + "  ,0) else 0 end ;";


                    string updatenotouttotalsql = string.Format(updateothersql, tableName, Convert.ToString(whCol[i]["FNUMBER"]));

                    updateNotTotalList.Add(new SqlObject(updatenotouttotalsql, new List<SqlParam>()));
                    //string updatenotouttotalsql2 = "update " + tableName + " set  " + Convert.ToString(whCol[i]["FNUMBER"]) + "notouttoal =isnull(" +
                    //    Convert.ToString(whCol[i]["FNUMBER"]) + "notouttoal,0)-isnull("+ Convert.ToString(whCol[i]["FNUMBER"])+
                    //    ",0) where isnull(" +Convert.ToString(whCol[i]["FNUMBER"]) + "notouttoal,0) >isnull("+ Convert.ToString(whCol[i]["FNUMBER"]) +",0) ;";
                    //updateNotTotalList2.Add(new SqlObject(updatenotouttotalsql2, new List<SqlParam>()));
                    // 更新分公司 未出金额 、库存金额 字段
                    string fengongsiamt = "update {0} " +
                            "set " + Convert.ToString(whCol[i]["FNUMBER"]) + "amt = ISNULL(" + Convert.ToString(whCol[i]["FNUMBER"]) + "* FREFCOST,0) ," +
                             Convert.ToString(whCol[i]["FNUMBER"]) + "notoutamt = ISNULL(" + Convert.ToString(whCol[i]["FNUMBER"]) + "notouttoal* FREFCOST,0) ; ";

                    string fengongsiamtsql = string.Format(fengongsiamt, tableName, Convert.ToString(whCol[i]["FNUMBER"]), tableName);
                    updateamtList.Add(new SqlObject(fengongsiamtsql, new List<SqlParam>()));

                }
            }
            DBUtils.ExecuteBatch(this.Context, updateNotTotalList);
            //DBUtils.ExecuteBatch(this.Context, updateNotTotalList2);
            DBUtils.ExecuteBatch(this.Context, updateamtList);
            //更新不足数量
            String updateTemp = "update  " + tableName + " set LACKTOTAL = LEFTSALEORDERTOTAL - TOTALALL   where  LEFTSALEORDERTOTAL>TOTALALL";
            DBUtils.Execute(this.Context, updateTemp);
            //更新金额
            string updateAmt = "update {0}  set PRODTOTALAMT=isnull(PRODTOTAL*FREFCOST,0) ," +
                               " SUBSTOCKTOTALAMT=isnull(SUBSTOCKTOTAL*FREFCOST,0) ," +
                               " onTheWayAMT=isnull(onTheWay*FREFCOST,0) ," +
                               " TOTALALLAMT=isnull(TOTALALL*FREFCOST,0) ," +
                               " LEFTSALEORDERTOTALAMT=isnull(LEFTSALEORDERTOTAL*FREFCOST,0) ," +
                                " LACKTOTALAMT=isnull(LACKTOTAL*FREFCOST,0) ;";
            updateAmt = string.Format(updateAmt, tableName);
            DBUtils.Execute(this.Context, updateAmt);
        }


        //public override ReportTitles GetReportTitles(IRptParams filter)
        //{
        //    ReportTitles titles = new ReportTitles();
        //    DynamicObject ProdFilter = filter.FilterParameter.CustomFilter;
        //    String materialID = ""; // Convert.ToString(ProdFilter["F_PAEZ_MaterialID"]);
        //    titles.AddTitle("F_PAEZ_MaterialID", materialID);
        //    return titles;
        //}

        /// <summary>
        /// 设置汇总列信息
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public override List<Kingdee.BOS.Core.Report.SummaryField> GetSummaryColumnInfo(IRptParams filter)
        {
            List<Kingdee.BOS.Core.Report.SummaryField> summarys = new List<Kingdee.BOS.Core.Report.SummaryField>();
            summarys.Add(new SummaryField("CK002", BOS.Core.Enums.BOSEnums.Enu_SummaryType.SUM));
            summarys.Add(new SummaryField("CK010", BOS.Core.Enums.BOSEnums.Enu_SummaryType.SUM));
            summarys.Add(new SummaryField("PRODTOTAL", BOS.Core.Enums.BOSEnums.Enu_SummaryType.SUM));
            summarys.Add(new SummaryField("PRODTOTALAMT", BOS.Core.Enums.BOSEnums.Enu_SummaryType.SUM));


            for (int i = 0; i < whCol.Count; i++)
            {
                string clonumName = Convert.ToString(whCol[i]["FNUMBER"]);
                if (clonumName == "CK002" || clonumName == "CK010")
                {
                    continue;
                }
                else
                {
                    string sqlFilter = string.Format(@" select T1.fname from T_BD_STOCK_L as T1 INNER JOIN T_BD_STOCK AS T2 ON T1.FSTOCKID=T2.FSTOCKID WHERE T2.FNUMBER = '{0}' ", clonumName);
                    using (IDataReader ReadData = DBUtils.ExecuteReader(this.Context, sqlFilter))
                    {
                        while (ReadData.Read())
                        {
                            fname = Convert.ToString(ReadData["FNAME"]);
                        }
                    }
                    summarys.Add(new SummaryField(clonumName, BOS.Core.Enums.BOSEnums.Enu_SummaryType.SUM));
                    summarys.Add(new SummaryField(clonumName + "amt", BOS.Core.Enums.BOSEnums.Enu_SummaryType.SUM));
                    summarys.Add(new SummaryField(clonumName + "notouttoal", BOS.Core.Enums.BOSEnums.Enu_SummaryType.SUM));
                    summarys.Add(new SummaryField(clonumName + "notoutamt", BOS.Core.Enums.BOSEnums.Enu_SummaryType.SUM));
                }
            }

            summarys.Add(new SummaryField("SUBSTOCKTOTAL", BOS.Core.Enums.BOSEnums.Enu_SummaryType.SUM));
            summarys.Add(new SummaryField("SUBSTOCKTOTALAMT", BOS.Core.Enums.BOSEnums.Enu_SummaryType.SUM));
            summarys.Add(new SummaryField("onTheWay", BOS.Core.Enums.BOSEnums.Enu_SummaryType.SUM));
            summarys.Add(new SummaryField("onTheWayAMT", BOS.Core.Enums.BOSEnums.Enu_SummaryType.SUM));

            summarys.Add(new SummaryField("TOTALALL", BOS.Core.Enums.BOSEnums.Enu_SummaryType.SUM));
            summarys.Add(new SummaryField("TOTALALLAMT", BOS.Core.Enums.BOSEnums.Enu_SummaryType.SUM));
            summarys.Add(new SummaryField("LEFTSALEORDERTOTAL", BOS.Core.Enums.BOSEnums.Enu_SummaryType.SUM));
            summarys.Add(new SummaryField("LEFTSALEORDERTOTALAMT", BOS.Core.Enums.BOSEnums.Enu_SummaryType.SUM));

            summarys.Add(new SummaryField("LACKTOTAL", BOS.Core.Enums.BOSEnums.Enu_SummaryType.SUM));
            summarys.Add(new SummaryField("LACKTOTALAMT", BOS.Core.Enums.BOSEnums.Enu_SummaryType.SUM));
            summarys.Add(new SummaryField("PRODPLANTOTAL", BOS.Core.Enums.BOSEnums.Enu_SummaryType.SUM));

            return summarys;
        }
        /// <summary>
        /// 构建动态列
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public override ReportHeader GetReportHeaders(IRptParams filter)
        {

            // TODO:fentryid,fid,fbaseunitqty,fmaterialid,fbomid,fqty
            // DynamicObject tempFileds = DBUtils.ExecuteReader()
            ReportHeader header = new ReportHeader();
            header.AddChild("MATERIALFNUMBER", new LocaleValue("编码", this.Context.UserLocale.LCID));
            header.AddChild("foldnumber", new LocaleValue("旧编码", this.Context.UserLocale.LCID));
            header.AddChild("FREFCOST", new LocaleValue("单价", this.Context.UserLocale.LCID), SqlStorageType.SqlDecimal);


            header.AddChild("FMATERIALGROUP", new LocaleValue("物料分组", this.Context.UserLocale.LCID));
            header.AddChild("FNAME", new LocaleValue("商品名称", this.Context.UserLocale.LCID));
            header.AddChild("FHANYU", new LocaleValue("韩语名", this.Context.UserLocale.LCID));
            header.AddChild("FErpClsName", new LocaleValue("物料属性", this.Context.UserLocale.LCID));



            //    OutPutWeight.AddChild("CK002", new LocaleValue("成品仓", this.Context.UserLocale.LCID));
            header.AddChild("CK002", new LocaleValue(Kingdee.BOS.Resource.ResManager.LoadKDString("成品仓", "002460030014698", Kingdee.BOS.Resource.SubSystemType.BOS), this.Context.UserLocale.LCID), SqlStorageType.SqlDecimal);
            header.AddChild("CK010", new LocaleValue("备用仓", this.Context.UserLocale.LCID), SqlStorageType.SqlDecimal);
            header.AddChild("PRODTOTAL", new LocaleValue("工厂库存合计", this.Context.UserLocale.LCID), SqlStorageType.SqlDecimal);
            header.AddChild("PRODTOTALAMT", new LocaleValue("工厂金额合计", this.Context.UserLocale.LCID), SqlStorageType.SqlDecimal);



            for (int i = 0; i < whCol.Count; i++)
            {
                string clonumName = Convert.ToString(whCol[i]["FNUMBER"]);
                if (clonumName == "CK002" || clonumName == "CK010")
                {
                    continue;
                }
                else
                {
                    string sqlFilter = string.Format(@" select T1.fname from T_BD_STOCK_L as T1 INNER JOIN T_BD_STOCK AS T2 ON T1.FSTOCKID=T2.FSTOCKID WHERE T2.FNUMBER = '{0}' ", clonumName);
                    using (IDataReader ReadData = DBUtils.ExecuteReader(this.Context, sqlFilter))
                    {
                        while (ReadData.Read())
                        {
                            fname = Convert.ToString(ReadData["FNAME"]);
                        }
                    }
                    header.IsHyperlink = true;
                    header.AddChild(clonumName, new LocaleValue(fname + "库存", this.Context.UserLocale.LCID), SqlStorageType.SqlDecimal);
                    header.AddChild(clonumName + "amt", new LocaleValue(fname + "库存金额", this.Context.UserLocale.LCID), SqlStorageType.SqlDecimal);
                    header.AddChild(clonumName + "notouttoal", new LocaleValue(fname + "未出数量", this.Context.UserLocale.LCID), SqlStorageType.SqlDecimal);
                    header.AddChild(clonumName + "notoutamt", new LocaleValue(fname + "未出金额", this.Context.UserLocale.LCID), SqlStorageType.SqlDecimal);
                }
            }

            header.AddChild("SUBSTOCKTOTAL", new LocaleValue("分公司合计", this.Context.UserLocale.LCID), SqlStorageType.SqlDecimal);
            header.AddChild("SUBSTOCKTOTALAMT", new LocaleValue("分公司金额", this.Context.UserLocale.LCID), SqlStorageType.SqlDecimal);
            header.AddChild("onTheWay", new LocaleValue("调拨在途", this.Context.UserLocale.LCID), SqlStorageType.SqlDecimal);
            header.AddChild("onTheWayAMT", new LocaleValue("调拨在途金额", this.Context.UserLocale.LCID), SqlStorageType.SqlDecimal);
            header.AddChild("TOTALALL", new LocaleValue("库存合计", this.Context.UserLocale.LCID), SqlStorageType.SqlDecimal);
            header.AddChild("TOTALALLAMT", new LocaleValue("库存合计金额", this.Context.UserLocale.LCID), SqlStorageType.SqlDecimal);
            header.AddChild("LEFTSALEORDERTOTAL", new LocaleValue("未出订单", this.Context.UserLocale.LCID), SqlStorageType.SqlDecimal);
            header.AddChild("LEFTSALEORDERTOTALAMT", new LocaleValue("未出订单金额", this.Context.UserLocale.LCID), SqlStorageType.SqlDecimal);
            header.AddChild("LACKTOTAL", new LocaleValue("不足数量", this.Context.UserLocale.LCID), SqlStorageType.SqlDecimal);
            header.AddChild("LACKTOTALAMT", new LocaleValue("不足数量金额", this.Context.UserLocale.LCID), SqlStorageType.SqlDecimal);
            header.AddChild("PRODPLANTOTAL", new LocaleValue("生产计划数量", this.Context.UserLocale.LCID), SqlStorageType.SqlDecimal);

            return header;
        }
    }



}


