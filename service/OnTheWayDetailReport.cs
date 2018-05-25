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
    [Description("在途明细报表")]
    public class OnTheWayDetailReport : SysReportBaseService
    {
        string material;
        string ckName;
        private const string onTheWayColumnName = "onTheWay";
        public override void Initialize()
        {

            this.ReportProperty.ReportName = new LocaleValue("在途明细报表", this.Context.UserLocale.LCID);
            this.ReportProperty.IdentityFieldName = "FIDENTITYID";
            List<DecimalControlField> list = new List<DecimalControlField>();
            list.Add(new DecimalControlField
            {
                ByDecimalControlFieldName = "PRICE",
                DecimalControlFieldName = "JINDU"
            });
            list.Add(new DecimalControlField
            {
                ByDecimalControlFieldName = "ONTHEWAYNO",
                DecimalControlFieldName = "JINDU"
            });
            this.ReportProperty.DecimalControlFieldList = list;

        }
        public override void BuilderReportSqlAndTempTable(IRptParams filter, string tableName)
        {
            string cks = string.Empty;
            getFilterCondiftionFields(filter, base.Context, cks);
            SQLStaticStatements sqlAllDetail = new SQLStaticStatements();
            string sqlAll = sqlAllDetail.returnSQK4OnTheWayDetailReport();
            string executeSQL = string.Format(sqlAll, tableName, material);
            DBUtils.Execute(this.Context, executeSQL);
        }

        public override List<Kingdee.BOS.Core.Report.SummaryField> GetSummaryColumnInfo(IRptParams filter)
        {
            List<Kingdee.BOS.Core.Report.SummaryField> summarys = new List<Kingdee.BOS.Core.Report.SummaryField>();
            summarys.Add(new SummaryField("ONTHEWAYNO", BOS.Core.Enums.BOSEnums.Enu_SummaryType.SUM));

            return summarys;
        }

        public override ReportHeader GetReportHeaders(IRptParams filter)
        {
            ReportHeader header = new ReportHeader();
            header.IsHyperlink = true;
            header.AddChild("FAHUOSTOCK", new LocaleValue("发货仓库", this.Context.UserLocale.LCID));
            header.AddChild("RUKUSTOCK", new LocaleValue("入库仓库", this.Context.UserLocale.LCID));
            header.AddChild("MATERIAL", new LocaleValue("产品编码", this.Context.UserLocale.LCID));
            header.AddChild("OLDNUMBER", new LocaleValue("旧编码", this.Context.UserLocale.LCID));
            header.AddChild("NAME", new LocaleValue("产品名称", this.Context.UserLocale.LCID));
            header.AddChild("FHYMING", new LocaleValue("韩语名", this.Context.UserLocale.LCID));
            header.AddChild("PRICE", new LocaleValue("单价", this.Context.UserLocale.LCID),SqlStorageType.SqlDecimal);
            header.AddChild("BILLNO", new LocaleValue("单据编号", this.Context.UserLocale.LCID));
            header.AddChild("FDATE", new LocaleValue("日期", this.Context.UserLocale.LCID));
            header.AddChild("ONTHEWAYNO", new LocaleValue("在途数量", this.Context.UserLocale.LCID), SqlStorageType.SqlDecimal);
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
            }

            foreach (KeyValuePair<string, object> materialMap in selectedCurrentRow)
            {
                if (materialMap.Key.Equals(onTheWayColumnName))
                {
                    material = Convert.ToString(materialMap.Value);
                }
            }

        }



        
    }
}
