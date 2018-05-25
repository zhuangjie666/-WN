using Kingdee.BOS;
using Kingdee.BOS.Core.CommonFilter.PlugIn;
using Kingdee.BOS.Core.DynamicForm.PlugIn;
using Kingdee.BOS.Core.DynamicForm.PlugIn.ControlModel;
using Kingdee.BOS.Core.Metadata;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kingdee.K3.WEINA.SCM.PlugIn.service
{
        [Description("设置动态下拉列表的值")]
    public class saleRankingListFilterPlugIn : AbstractCommonFilterPlugIn
    {
        	public override void AfterBindData(System.EventArgs e){
                SetStockComList();
            }

            private void SetStockComList()
            {
                ComboFieldEditor headComboEidtor = this.View.GetControl<ComboFieldEditor>("F_PAEZ_outStock");
                List<EnumItem> comboOptions = new List<EnumItem>();
                GetSubStockNameUtils subStock = new GetSubStockNameUtils();
                int i = 0;
                List<DynamicStockObject> fldKeyList = subStock.getStockID(base.Context);
                foreach (DynamicStockObject fldKey in fldKeyList)
                {
                     comboOptions.Add(new EnumItem() { Caption = new LocaleValue(fldKey.StockName, this.View.Context.UserLocale.LCID), EnumId = i.ToString(), Seq = i, Value = fldKey.ToString() });
                     i = i + 1;
                 }

                 headComboEidtor.SetComboItems(comboOptions);
     
            }

    }
}
