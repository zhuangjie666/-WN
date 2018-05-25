using Kingdee.BOS;
using Kingdee.BOS.App.Data;
using Kingdee.BOS.Core.Bill;
using Kingdee.BOS.Core.DynamicForm;
using Kingdee.BOS.Core.DynamicForm.PlugIn.Args;
using Kingdee.BOS.Core.Metadata;
using Kingdee.BOS.Core.Report;
using Kingdee.BOS.Core.Report.PlugIn;
using Kingdee.BOS.Core.Report.PlugIn.Args;
using Kingdee.BOS.Core.SqlBuilder;
using Kingdee.BOS.Orm.DataEntity;
using Kingdee.BOS.ServiceHelper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using Kingdee.BOS.Core.CommonFilter;

namespace Kingdee.K3.WEINA.SCM.PlugIn.service
{
    [Description("双击链接到对应单元格的报表 - 库存明细报表 未出库明细 在途明细报表等")]
    public class CellDoubleClickLinkUtil : AbstractSysReportPlugIn
    {
        private const string notOut = "PAEZ_NoOutDetailReport";
        private const string onTheWay = "PAEZ_OnTheWayDetailReport";
        private const string stockDetail = "PAEZ_StockDetailReport";
        private const string inStockDetail = "PAEZ_InStockDetailReport";
        private const string onTheWayColumnName = "onTheWay";
        private string notOutColumnName = "";
        private string inStockColumnName = "PRODINSTOCKNO";

        public override void EntityRowDoubleClick(EntityRowClickEventArgs e)
        {
            base.EntityRowDoubleClick(e);
        }

        public override void CellDbClick(CellEventArgs Args)
        {
            Args.Cancel = true;
            int rowSeq = Args.CellRowIndex-1;     // 行序号，以1开始

            string ColumnNameKey = Args.Header.FieldName;//单元格的列名
            // 根据当前所选的单据编号字段名，确定需要打开的单据类型
            string formId = string.Empty;
            //准备所有的仓库未出标示
            notOutColumnName = buildNoOutFlg(ColumnNameKey);
 
            // 双击的是普通单元格，不需要显示单据,否则根据所双击的单元格进行二级报表的展示
            this.showSubReport(ColumnNameKey, rowSeq);
        }

        private string buildNoOutFlg(string ColumnNameKey)
        {
            List<string> notOutColumnNameList = new List<string>();
            GetSubStockNameUtils stockNameObject = new GetSubStockNameUtils();
           List<DynamicStockObject> stockObject =  stockNameObject.getStockID(this.Context);
            foreach(DynamicStockObject dso in stockObject){
                notOutColumnNameList.Add(Convert.ToString(dso.ckName) + "notouttoal");
            }
            foreach (string columnName in notOutColumnNameList)
            {
                if (columnName.Equals(ColumnNameKey))
                {
                    notOutColumnName = columnName;
                }
            }
            return notOutColumnName;
        }
        //取单元格的字段作为可双击单元格进行跳转窗口
        private void showSubReport(string ColumnNameKey, int rowSeq)
        {
            string formId = string.Empty;

            if (ColumnNameKey.Equals(inStockColumnName))
            {
                // 入库明细报表
                formId = inStockDetail;
                showStockDetailReport(ColumnNameKey, rowSeq, formId);
            }

            if (ColumnNameKey.Equals(notOutColumnName))
            {
                // 未出明细报表
                 formId = notOut;
                showStockDetailReport(ColumnNameKey, rowSeq, formId);
            }

            if (ColumnNameKey.Equals(onTheWayColumnName))
            {
                // 在途明细报表
                 formId = onTheWay;
                showStockDetailReport(ColumnNameKey, rowSeq, formId);
            }

            //库存明细报表

            List<DynamicStockObject> fldKeyList = new GetSubStockNameUtils().getStockID(this.Context);
            DynamicStockObject ckchengpin = new DynamicStockObject();
            ckchengpin.ckName = "CK002";
            ckchengpin.StockName = "成品仓";
            ckchengpin.stockid = 0;
            fldKeyList.Add(ckchengpin);
            foreach (DynamicStockObject ckname in fldKeyList)
            {
                if (ColumnNameKey.Equals(ckname.ckName))
                {
                     formId = stockDetail;
                    showStockDetailReport(ColumnNameKey, rowSeq, formId);
                }
            }

            if (string.IsNullOrWhiteSpace(formId)) {
                return;
            }

          
        }

        private void showStockDetailReport(string fldKey, int rowSeq, string formId)
        {
            SysReportShowParameter showParam = new SysReportShowParameter();

            Dictionary<string, object> fmaterial = this.GetFilterKeyValue(fldKey, rowSeq,formId);
            switch (formId)
            {
                case stockDetail:
                    showParam.CustomParams[fldKey] = Convert.ToString(fmaterial["ckname"]);
                    showParam.CustomParams[fldKey] = Convert.ToString(fmaterial["material"]);
                    break;
                case notOut:
                    showParam.CustomParams[fldKey] = Convert.ToString(fmaterial["material"]);
                    //showParam.CustomParams[fldKey] = fmaterial["ckname"];
                    break;
                case onTheWay:
                    showParam.CustomParams[fldKey] = Convert.ToString(fmaterial["material"]);
                    break;
                case inStockDetail:
                    showParam.CustomComplexParams[fldKey] = Convert.ToString(fmaterial["material"]);
                    showParam.CustomComplexParams[fldKey] = fmaterial["condition"];
                    break;

            }
            showParam.FormId = formId;
            showParam.IsShowFilter = false;
            this.View.ShowForm(showParam);
          
        }


        private Dictionary<string, object> GetFilterKeyValue(string fldKey, int rowIndex, string formId)
        {
            // 使用行索引，到报表数据源中自行获取行
            // 报表显示的数据源
            DataTable dt = ((ISysReportModel)this.View.Model).DataSource;
            if (dt.Rows.Count == 0 || rowIndex >= dt.Rows.Count)
            {
                return null;
            }
            DataRow currRow = dt.Rows[rowIndex];
            string fmaterial = Convert.ToString(currRow["FMaterialId"]);
            conditionEntry conditionEntry=null;
            if (formId.Equals(inStockDetail)) {
                string condition = Convert.ToString(currRow["condition"]);
                decimal qichu = Convert.ToDecimal(currRow["CHUQIKUCUN"]);
                
                conditionEntry = new conditionEntry();
                conditionEntry.material = fmaterial;
                conditionEntry.startDate = condition.Substring(0, 10);
                conditionEntry.endDate = condition.Substring(11, 10);
                conditionEntry.qichu = qichu;
                conditionEntry.cangku = "CK002";
            }
           
            string ckname = Convert.ToString(currRow[fldKey]);



            Dictionary<string, object> fmaterialMap = new Dictionary<string, object>();
            fmaterialMap.Add("material", fmaterial);
            fmaterialMap.Add("ckname", ckname);
            fmaterialMap.Add("condition", conditionEntry);

            return fmaterialMap;
        }
    }
}