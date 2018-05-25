using Kingdee.BOS.Core.Bill.PlugIn;
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
    [Description("销售增值税专用发票 编辑界面")]
    public class SaleSicKFEdit : AbstractBillPlugIn
    {
        
        public override void BarItemClick(BOS.Core.DynamicForm.PlugIn.Args.BarItemClickEventArgs e)
        {
            base.BarItemClick(e);
            switch (e.BarItemKey)
            {
                case "tbAmountAllocation":
                    //点击“金额分摊”功能后，重新计算销售增值税专用发票的单据体分录“含税单价”，含税单价=原价*折扣率；
                    EntryEntity entryEntity = this.View.BusinessInfo.GetEntryEntity("FSALESICENTRY");
                    DynamicObjectCollection rows = this.View.Model.GetEntityDataObject(entryEntity);
                    decimal disaccount = Convert.ToDecimal(this.View.Model.GetValue("F_PAEZ_disaccount"));//折扣率
                    int rowCount=rows.Count;
                    for (int i = 0; i < rowCount;i++ )
                    {
                        decimal origPrice = Convert.ToDecimal(this.View.Model.GetValue("F_PAEZ_origPrice",i)); //原价
                        //5、	金额分摊时，要把折扣率和折扣额清零，否则会造成折上折
                        this.View.Model.SetValue("FENTRYDISCOUNTRATE", 0, i);//折扣率%
                        this.View.Model.SetValue("FDISCOUNTAMOUNTFOR", 0, i);//折扣额

                        this.View.Model.SetValue("FAUXTAXPRICE", origPrice * disaccount, i);//含税单价=原价*折扣率
                        
                    }                   
                    return;

            }
        }
    }
}
