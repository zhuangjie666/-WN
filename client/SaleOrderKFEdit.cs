using Kingdee.BOS;
using Kingdee.BOS.App.Data;
using Kingdee.BOS.Core.Bill.PlugIn;
using Kingdee.BOS.Core.DynamicForm;
using Kingdee.BOS.Core.Metadata.EntityElement;
using Kingdee.BOS.Orm.DataEntity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kingdee.K3.WEINA.SCM.PlugIn.client
{
    [Description("销售订单编辑界面")]
    public class SaleOrderKFEdit : AbstractBillPlugIn
    {
        public override void BeforeSave(BOS.Core.Bill.PlugIn.Args.BeforeSaveEventArgs e)
        {
            base.BeforeSave(e);
            //销售订单的“当前组织”、“己审核”的销售订单的当前物料的数量   和 该单据物料之和大于最大销量 不允许提交当前销售订单
            EntryEntity entryEntity = this.View.BusinessInfo.GetEntryEntity("FSaleOrderEntry");
            DynamicObjectCollection rows = this.View.Model.GetEntityDataObject(entryEntity);
            IOperationResult operaRst = new OperationResult();
            operaRst.IsSuccess = false;
            operaRst.CustomMessageModel = K3DisplayerModel.Create(this.Context, "行号~|~校验信息");
            operaRst.CustomMessageModel.FieldAppearances[0].Width = new LocaleValue("100", this.Context.UserLocale.LCID);
            operaRst.CustomMessageModel.FieldAppearances[1].Width = new LocaleValue("100", this.Context.UserLocale.LCID);
            operaRst.CustomMessageModel.FieldAppearances[2].Width = new LocaleValue("300", this.Context.UserLocale.LCID);

            foreach (var row in rows)
            {
                DynamicObject material = row["MaterialID"] as DynamicObject;
                decimal qty = Convert.ToDecimal(row["Qty"]);
                decimal maxQty = Convert.ToDecimal(material["F_PAEZ_maxSaleQty"]);
                int seq = Convert.ToInt32(row["Seq"]);
                if (null == material)
                {
                    return;
                }

                string sql = string.Format(@"SELECT sum(b.FQTY) qty FROM T_SAL_ORDER a inner join T_SAL_ORDERENTRY b  on a.fid =b.fid  WHERE b.FMATERIALID ='{0}' AND a.FDOCUMENTSTATUS  in ('B','C') ", material["Id"]);
                DynamicObjectCollection saleOrderCol = DBUtils.ExecuteDynamicObject(this.Context, sql);
                if (saleOrderCol.Count > 0)
                {
                    qty = qty + Convert.ToDecimal(saleOrderCol[0]["qty"]);
                }

                if (qty > maxQty)
                {
                    operaRst.CustomMessageModel.AddMessage(string.Format("{0}~|~{1}", seq, "大于此物料的最大销量，不允许提交当前销售订单"));
                }
            }

            if (operaRst.CustomMessageModel.Messages.Count() > 0)
            {
                e.Cancel = true;

                this.View.ShowK3Displayer(operaRst.CustomMessageModel);
            }

        }
       
    }
}
