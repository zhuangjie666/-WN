using Kingdee.BOS;
using Kingdee.BOS.App.Data;
using Kingdee.BOS.Contracts.Report;
using Kingdee.BOS.Core.Report;
using Kingdee.BOS.Core.Report.PlugIn;
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
    [Description("成品仓库发货情况表")]
    public class EndProductionSendOutDetail : SysReportBaseService
    {
        List<DynamicStockObject> stockList = null;
        public override void Initialize()
        {

            this.ReportProperty.ReportName = new LocaleValue("销售排名报表-按产品", this.Context.UserLocale.LCID);
            this.ReportProperty.IdentityFieldName = "FIDENTITYID";
            List<DecimalControlField> list = new List<DecimalControlField>();

            list.Add(new DecimalControlField
            {
                ByDecimalControlFieldName = "DANJIA",
                DecimalControlFieldName = "JINDU"
            });
            GetSubStockNameUtils stocks = new GetSubStockNameUtils();
            stockList = stocks.getStockID(this.Context);
            foreach (DynamicStockObject stock in stockList)
            {
                list.Add(new DecimalControlField
                {
                    ByDecimalControlFieldName = stock.ckName,
                    DecimalControlFieldName = "JINDU"
                });
            }


            list.Add(new DecimalControlField
            {
                ByDecimalControlFieldName = "DIAOBOSUM",
                DecimalControlFieldName = "JINDU"
            });
            list.Add(new DecimalControlField
            {
                ByDecimalControlFieldName = "DIAOBOJINE",
                DecimalControlFieldName = "JINDU"
            });
            list.Add(new DecimalControlField
            {
                ByDecimalControlFieldName = "QIMOKUCUN",
                DecimalControlFieldName = "JINDU"
            });
            list.Add(new DecimalControlField
            {
                ByDecimalControlFieldName = "QIMOKUCUNJINE",
                DecimalControlFieldName = "JINDU"
            });




            this.ReportProperty.DecimalControlFieldList = list;
        }


        public override void BuilderReportSqlAndTempTable(IRptParams filter, string tableName)
        {
            DynamicObject customFilter = filter.FilterParameter.CustomFilter;
            string startDate = Convert.ToDateTime(customFilter["F_PAEZ_startDate"]).ToString("yyyy-MM-dd", System.Globalization.DateTimeFormatInfo.InvariantInfo);
            string endDate = Convert.ToDateTime(customFilter["F_PAEZ_endDate"]).ToString("yyyy-MM-dd", System.Globalization.DateTimeFormatInfo.InvariantInfo);
            //string startDate = Convert.ToString(customFilter["F_PAEZ_startDate"]).Substring(0,10).Replace("/","-");
            //string endDate = Convert.ToString(customFilter["F_PAEZ_endDate"]).Substring(0,10).Replace("/","-");
            string executeSQL = string.Empty;

            SQLStaticStatements sqlAllDetail = new SQLStaticStatements();
            string searchCondition = string.Empty;
            string sqlAll = sqlAllDetail.returnSQLEndProductionSendOutDetailReport(searchCondition);
            List<string> stockss = new List<string>();
           
     
            //stockList = stocks.getStockID(this.Context);
            foreach(DynamicStockObject stock in stockList){
                stockss.Add(stock.ckName);
            }
            executeSQL = string.Format(sqlAll, tableName, String.Join(", ", stockss.ToArray()),startDate,endDate);
            DBUtils.Execute(this.Context, executeSQL);

            // 调拨总数
            StringBuilder updateCK= new StringBuilder();
             
            foreach (string s in stockss) {
                updateCK.Append( "ISNULL(" + s + ",0) + ");
            }
            //string sqlsum = String.Join("+ ", stockss.ToArray());
            executeSQL = string.Format("UPDATE {0} SET DIAOBOSUM = {1}", tableName, updateCK.Remove(updateCK.Length-3,3));
            DBUtils.Execute(this.Context, executeSQL);
            
            //调拨金额
            executeSQL = string.Format("UPDATE {0} SET DIAOBOJINE = ISNULL(DIAOBOSUM*DANJIA, 0)", tableName);
            DBUtils.Execute(this.Context, executeSQL);

            //产品总数
            string prodsum = sqlAllDetail.returnSQL4ProdSum();
            executeSQL = string.Format(prodsum, tableName, startDate, endDate);
            DBUtils.Execute(this.Context, executeSQL);
            //期初库存
            string qichukucun = sqlAllDetail.returnSQL4qichukucun();
            executeSQL = string.Format(qichukucun, tableName, startDate);
            DBUtils.Execute(this.Context, executeSQL);
            //期末库存
            string qimokucun = sqlAllDetail.returnSQL4qimokucun();
            executeSQL = string.Format(qimokucun, tableName, endDate);
            DBUtils.Execute(this.Context, executeSQL);
            //期末库存金额
            executeSQL = string.Format("UPDATE {0} SET QIMOKUCUNJINE = ISNULL(QIMOKUCUN*DANJIA, 0)", tableName);
            DBUtils.Execute(this.Context, executeSQL);



        }

        public override List<Kingdee.BOS.Core.Report.SummaryField> GetSummaryColumnInfo(IRptParams filter)
        {
            List<Kingdee.BOS.Core.Report.SummaryField> summarys = new List<Kingdee.BOS.Core.Report.SummaryField>();
            summarys.Add(new SummaryField("CHUQIKUCUN", BOS.Core.Enums.BOSEnums.Enu_SummaryType.SUM));
            summarys.Add(new SummaryField("QIMOKUCUN", BOS.Core.Enums.BOSEnums.Enu_SummaryType.SUM));
            summarys.Add(new SummaryField("PRODINSTOCKNO", BOS.Core.Enums.BOSEnums.Enu_SummaryType.SUM));
            
            return summarys;
        }


        public override ReportHeader GetReportHeaders(IRptParams filter)
        {
            ReportHeader header = new ReportHeader();
            header.IsHyperlink = true;
            header.AddChild("FNUMBER", new LocaleValue("产品编码", this.Context.UserLocale.LCID));
            header.AddChild("FOLDNUMBER", new LocaleValue("旧编码", this.Context.UserLocale.LCID));
            header.AddChild("FNAME", new LocaleValue("产品名称", this.Context.UserLocale.LCID));
            header.AddChild("FHYMING", new LocaleValue("韩语名", this.Context.UserLocale.LCID));
            header.AddChild("DANJIA", new LocaleValue("单价", this.Context.UserLocale.LCID), SqlStorageType.SqlDecimal);
            header.AddChild("PRODINSTOCKNO", new LocaleValue("产品入库数量", this.Context.UserLocale.LCID));
            header.AddChild("CHUQIKUCUN", new LocaleValue("期初库存", this.Context.UserLocale.LCID));

            foreach(DynamicStockObject stock in stockList){
                header.AddChild(stock.ckName, new LocaleValue(stock.StockName, this.Context.UserLocale.LCID), SqlStorageType.SqlDecimal);
            }

            header.AddChild("DIAOBOSUM", new LocaleValue("调拨数量合计", this.Context.UserLocale.LCID), SqlStorageType.SqlDecimal);
            header.AddChild("DIAOBOJINE", new LocaleValue("调拨总金额", this.Context.UserLocale.LCID), SqlStorageType.SqlDecimal);
            header.AddChild("QIMOKUCUN", new LocaleValue("期末库存", this.Context.UserLocale.LCID), SqlStorageType.SqlDecimal);
            header.AddChild("QIMOKUCUNJINE", new LocaleValue("期末库存金额", this.Context.UserLocale.LCID), SqlStorageType.SqlDecimal);
            return header;
        }


    }
}