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
    [Description("入库明细表")]
    public class InStockDetail : SysReportBaseService
    {
        private string inStockColumnName = "PRODINSTOCKNO";
        string material = null;
        public override void Initialize()
        {
            this.ReportProperty.ReportName = new LocaleValue("入库明细表", this.Context.UserLocale.LCID);
            this.ReportProperty.IdentityFieldName = "FIDENTITYID";
            List<DecimalControlField> list = new List<DecimalControlField>();

            list.Add(new DecimalControlField
            {
                ByDecimalControlFieldName = "DANJIA",
                DecimalControlFieldName = "JINDU"
            });
            list.Add(new DecimalControlField
            {
                ByDecimalControlFieldName = "FQTY",
                DecimalControlFieldName = "JINDU"
            });
            list.Add(new DecimalControlField
            {
                ByDecimalControlFieldName = "KUCUNQTY",
                DecimalControlFieldName = "JINDU"
            });
            this.ReportProperty.DecimalControlFieldList = list;
        }

        public override void BuilderReportSqlAndTempTable(IRptParams filter, string tableName)
        { 
            string cks = string.Empty;
            conditionEntry condition = getFilterCondiftionFields(filter, base.Context, cks);
            string material = condition.material;
            string startDate = condition.startDate;
            string endDate = condition.endDate;
            decimal qichu = condition.qichu;
            string cangku = condition.cangku;
            
            string executeSQL = string.Empty;
            SQLStaticStatements sqlAllDetail = new SQLStaticStatements();
            string searchCondition = string.Empty;
            string sqlAll = sqlAllDetail.returnSQLInStockDetailReport();
            executeSQL = string.Format(sqlAll, tableName, material,cangku,startDate,endDate);
            DBUtils.Execute(this.Context, executeSQL);

            string SelectSQL = "select FDATE,BILLNO FROM {0} ";

            using (IDataReader ReadData = DBUtils.ExecuteReader(this.Context, string.Format(SelectSQL, tableName)))
            {
                while (ReadData.Read())
                {

                    string endDateCondition = Convert.ToDateTime(ReadData["FDATE"]).ToString("yyyy-MM-dd HH:mm:ss", System.Globalization.DateTimeFormatInfo.InvariantInfo);
                    string billno  = Convert.ToString(ReadData["BILLNO"]);
                    string update4KCQTY = sqlAllDetail.returnSQL4KUCUNQTY();
                    executeSQL = string.Format(update4KCQTY, tableName, startDate, endDateCondition, material, billno, qichu);
                    DBUtils.Execute(this.Context, executeSQL);
                }
            }
            


        }

        public override List<Kingdee.BOS.Core.Report.SummaryField> GetSummaryColumnInfo(IRptParams filter)
        {
            List<Kingdee.BOS.Core.Report.SummaryField> summarys = new List<Kingdee.BOS.Core.Report.SummaryField>();
            summarys.Add(new SummaryField("FQTY", BOS.Core.Enums.BOSEnums.Enu_SummaryType.SUM));
            summarys.Add(new SummaryField("KUCUNQTY", BOS.Core.Enums.BOSEnums.Enu_SummaryType.SUM));
            
            return summarys;
        }
        public override ReportHeader GetReportHeaders(IRptParams filter)
        {
            ReportHeader header = new ReportHeader();
            header.IsHyperlink = true;
            header.AddChild("MATERIAL", new LocaleValue("产品编码", this.Context.UserLocale.LCID));
            header.AddChild("OLDNUMBER", new LocaleValue("旧编码", this.Context.UserLocale.LCID));
            header.AddChild("NAME", new LocaleValue("产品名称", this.Context.UserLocale.LCID));
            header.AddChild("FHYMING", new LocaleValue("韩语名", this.Context.UserLocale.LCID));
            header.AddChild("PRICE", new LocaleValue("单价", this.Context.UserLocale.LCID), SqlStorageType.SqlDecimal);
            header.AddChild("FQTY", new LocaleValue("入库数量", this.Context.UserLocale.LCID), SqlStorageType.SqlDecimal);
            header.AddChild("KUCUNQTY", new LocaleValue("库存数量", this.Context.UserLocale.LCID), SqlStorageType.SqlDecimal);
            header.AddChild("BILLTYPE", new LocaleValue("单据类型", this.Context.UserLocale.LCID));
            header.AddChild("BILLNO", new LocaleValue("单据编号", this.Context.UserLocale.LCID));
            header.AddChild("FDATE", new LocaleValue("日期", this.Context.UserLocale.LCID));


            return header;
        }

        private conditionEntry getFilterCondiftionFields(IRptParams filter, BOS.Context context, string cks)
        {
            //定义是否从汇总表过来的过滤条件
            Boolean fromMainReportFlag = false;

            //用于定位明细的过滤条件map
            Dictionary<string, object> selectedCurrentRow = new Dictionary<string, object>();
            if (filter.CustomParams.Count > 0 && filter.CustomParams.ContainsKey("OpenParameter"))
            {
                selectedCurrentRow = (Dictionary<string, object>)filter.CustomParams["OpenParameter"];
                fromMainReportFlag = true;
            }

            List<DynamicStockObject> fldKeyList = new GetSubStockNameUtils().getStockID(context);
            foreach (KeyValuePair<string, object> materialMap in selectedCurrentRow)
            {
                if (materialMap.Key.Equals(inStockColumnName))
                {
                    return (conditionEntry)materialMap.Value;
                }                
            }
            return null;
        }


    }
}
