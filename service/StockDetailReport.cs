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
    [Description("库存明细报表")]
    public class StockDetailReport : SysReportBaseService
    {
        string material;
        string ckName;
        public override void Initialize()
        {

            this.ReportProperty.ReportName = new LocaleValue("库存明细报表", this.Context.UserLocale.LCID);
            this.ReportProperty.IdentityFieldName = "FIDENTITYID";
            List<DecimalControlField> list = new List<DecimalControlField>();
            list.Add(new DecimalControlField
            {
                ByDecimalControlFieldName = "PRICE",
                DecimalControlFieldName = "JinDu"
            });
            list.Add(new DecimalControlField
            {
                ByDecimalControlFieldName = "INQTY",
                DecimalControlFieldName = "JinDu"
            });
            list.Add(new DecimalControlField
            {
                ByDecimalControlFieldName = "OUTQTY",
                DecimalControlFieldName = "JinDu"
            });
                        list.Add(new DecimalControlField
            {
                ByDecimalControlFieldName = "KCTOTAL",
                DecimalControlFieldName = "JinDu"
            });
            

            this.ReportProperty.DecimalControlFieldList = list;
        }
        public override void BuilderReportSqlAndTempTable(IRptParams filter, string tableName)
        {
            string cks = string.Empty;
            getFilterCondiftionFields(filter, base.Context,cks);
            SQLStaticStatements sqlAllDetail = new SQLStaticStatements();
            string sqlAll = sqlAllDetail.returnSQK4StockDetailReport();
            string executeSQL = string.Format(sqlAll, tableName,material, ckName);
            DBUtils.Execute(this.Context, executeSQL);

            string kctot = "UPDATE {0}  SET KCTOTAL = ISNULL(INQTY,0) - ISNULL(OUTQTY,0) ";
            string SelectSQL = "select FDATE,BILLNO FROM {0} ";
            //GetSubStockNameUtils stocks = new GetSubStockNameUtils();
            //List<DynamicStockObject> stocklist = stocks.getStockID(this.Context);
            //int stockid = 0;
            //foreach (DynamicStockObject s in stocklist) {
            //    if (s.ckName.Equals(ckName)) {
            //        stockid = s.stockid;
            //        break;
            //    }
            //}
            //using (IDataReader ReadData = DBUtils.ExecuteReader(this.Context, string.Format(SelectSQL, tableName)))
            //{
            //    while (ReadData.Read())
            //    {

            //      //  string endDateCondition = Convert.ToDateTime(ReadData["FDATE"]).ToString("yyyy-MM-dd HH:mm:ss", System.Globalization.DateTimeFormatInfo.InvariantInfo);
            //        string billno = Convert.ToString(ReadData["BILLNO"]);
            //        string update4KCQTY = sqlAllDetail.returnSQL4KUCUNDETAILQTY();
            //        executeSQL = string.Format(update4KCQTY, tableName, material, stockid, billno);
            //        DBUtils.Execute(this.Context, executeSQL);
            //    }
            //}
            string executeSQL1 = string.Format(kctot, tableName, material, ckName);
            DBUtils.Execute(this.Context, executeSQL1);
        }

        public override List<Kingdee.BOS.Core.Report.SummaryField> GetSummaryColumnInfo(IRptParams filter)
        {
            List<Kingdee.BOS.Core.Report.SummaryField> summarys = new List<Kingdee.BOS.Core.Report.SummaryField>();
            summarys.Add(new SummaryField("INQTY", BOS.Core.Enums.BOSEnums.Enu_SummaryType.SUM));
            summarys.Add(new SummaryField("OUTQTY", BOS.Core.Enums.BOSEnums.Enu_SummaryType.SUM));
            summarys.Add(new SummaryField("KCTOTAL", BOS.Core.Enums.BOSEnums.Enu_SummaryType.SUM));
            return summarys;
        }

        public override ReportHeader GetReportHeaders(IRptParams filter)
        {
            ReportHeader header = new ReportHeader();
            header.IsHyperlink = true;
            header.AddChild("CANGKUNAME", new LocaleValue("仓库", this.Context.UserLocale.LCID));
            header.AddChild("MATERIAL", new LocaleValue("产品编码", this.Context.UserLocale.LCID));
            header.AddChild("OLDNUMBER", new LocaleValue("旧编码", this.Context.UserLocale.LCID));
            header.AddChild("NAME", new LocaleValue("产品名称", this.Context.UserLocale.LCID));
            header.AddChild("FHYMING", new LocaleValue("韩语名", this.Context.UserLocale.LCID));
            header.AddChild("PRICE", new LocaleValue("单价", this.Context.UserLocale.LCID),SqlStorageType.SqlDecimal);
            header.AddChild("INQTY", new LocaleValue("入库", this.Context.UserLocale.LCID),SqlStorageType.SqlDecimal);
            header.AddChild("OUTQTY", new LocaleValue("出库", this.Context.UserLocale.LCID), SqlStorageType.SqlDecimal);
            header.AddChild("KCTOTAL", new LocaleValue("库存数量", this.Context.UserLocale.LCID),SqlStorageType.SqlDecimal);
            if (ckName.Equals("CK002"))
            {
                 header.AddChild("INSTOCKNAME", new LocaleValue("入库仓库", this.Context.UserLocale.LCID));
            }
            header.AddChild("BILLTYPE", new LocaleValue("单据类型", this.Context.UserLocale.LCID));
            header.AddChild("BILLNO", new LocaleValue("单据编号", this.Context.UserLocale.LCID));
            header.AddChild("FDATE", new LocaleValue("日期", this.Context.UserLocale.LCID));
            return header;
        }

        private void getFilterCondiftionFields(IRptParams filter, BOS.Context context,string cks)
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
            DynamicStockObject ckchengpin = new DynamicStockObject();
            ckchengpin.ckName = "CK002";
            ckchengpin.StockName = "成品仓";
            ckchengpin.stockid = 0;
            fldKeyList.Add(ckchengpin);
            foreach (DynamicStockObject fldKey in fldKeyList)
            {
                if (fromMainReportFlag)
                {
                    if (selectedCurrentRow.ContainsKey(fldKey.ckName))
                    {
                        material = Convert.ToString(selectedCurrentRow[fldKey.ckName]);
                        ckName = fldKey.ckName;
                    }
                    if (!string.IsNullOrWhiteSpace(material))
                    {
                        break;
                    }
                }
            }
        }




    }
}
