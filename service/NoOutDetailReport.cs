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
     [Description("未出明细报表")]
    public class NoOutDetailReport : SysReportBaseService
    {
         string ckName;
         string material;
         string ckid;
        public override void Initialize()
        {
            this.ReportProperty.ReportName = new LocaleValue("未出明细报表", this.Context.UserLocale.LCID);
            this.ReportProperty.IdentityFieldName = "FIDENTITYID";
            List<DecimalControlField> list = new List<DecimalControlField>();
            list.Add(new DecimalControlField
            {
                ByDecimalControlFieldName = "NOOUTNO",
                DecimalControlFieldName = "JINDU"
            });
            list.Add(new DecimalControlField
            {
                ByDecimalControlFieldName = "FPRICE",
                DecimalControlFieldName = "JINDU"
            });

            this.ReportProperty.DecimalControlFieldList = list;
        }
        public override void BuilderReportSqlAndTempTable(IRptParams filter, string tableName)
        {
            string cks = string.Empty;
            getFilterCondiftionFields(filter, base.Context, cks);
            SQLStaticStatements sqlAllDetail = new SQLStaticStatements();
            string sqlAll = sqlAllDetail.returnSQK4NoOutDetailReport();
            GetSubStockNameUtils ckNameObject = new GetSubStockNameUtils();
            List<DynamicStockObject> ckNameObj = ckNameObject.getStockID(this.Context);
            foreach (DynamicStockObject ck in ckNameObj) {
                if (ck.ckName.Equals(ckName.Substring(0,5))) { 
                        ckid=Convert.ToString(ck.stockid);
                }
            }
            string executeSQL = string.Format(sqlAll, tableName,ckid,material);
            DBUtils.Execute(this.Context, executeSQL);
        }
        public override List<Kingdee.BOS.Core.Report.SummaryField> GetSummaryColumnInfo(IRptParams filter)
        {
            List<Kingdee.BOS.Core.Report.SummaryField> summarys = new List<Kingdee.BOS.Core.Report.SummaryField>();
            summarys.Add(new SummaryField("NOOUTNO", BOS.Core.Enums.BOSEnums.Enu_SummaryType.SUM));

            return summarys;
        }
        public override ReportHeader GetReportHeaders(IRptParams filter)
        {
            ReportHeader header = new ReportHeader();
            header.IsHyperlink = true;
            header.AddChild("OUTSTOCK", new LocaleValue("仓库", this.Context.UserLocale.LCID));
            header.AddChild("PRODNO", new LocaleValue("产品编码", this.Context.UserLocale.LCID));
            header.AddChild("OLDNUMBER", new LocaleValue("旧编码", this.Context.UserLocale.LCID));
            header.AddChild("PRODNAME", new LocaleValue("产品名称", this.Context.UserLocale.LCID));
            header.AddChild("HYMING", new LocaleValue("韩语名", this.Context.UserLocale.LCID));
            header.AddChild("FPRICE", new LocaleValue("单价", this.Context.UserLocale.LCID),SqlStorageType.SqlDecimal);
            header.AddChild("BILLNO", new LocaleValue("订单编号", this.Context.UserLocale.LCID));
            header.AddChild("BILLDATE", new LocaleValue("订单日期", this.Context.UserLocale.LCID));
            header.AddChild("NOOUTNO", new LocaleValue("未出数量", this.Context.UserLocale.LCID), SqlStorageType.SqlDecimal);
            header.AddChild("SHOUHUO", new LocaleValue("收货人", this.Context.UserLocale.LCID));
            header.AddChild("COUNTRYCUST", new LocaleValue("县级客户", this.Context.UserLocale.LCID));
            header.AddChild("CITYCUST", new LocaleValue("市级客户", this.Context.UserLocale.LCID));
            header.AddChild("PROVINCECUST", new LocaleValue("省级客户", this.Context.UserLocale.LCID));
            return header;
        }

        private void getFilterCondiftionFields(IRptParams filter, BOS.Context context, string cks)
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
            foreach (DynamicStockObject fldKey in fldKeyList)
            {
                if (fromMainReportFlag)
                {
                    if (selectedCurrentRow.ContainsKey(fldKey.ckName + "notouttoal"))
                    {
                        material = Convert.ToString(selectedCurrentRow[fldKey.ckName + "notouttoal"]);
                        ckName = fldKey.ckName;
                    }
                }
            }
        }
     }
}
