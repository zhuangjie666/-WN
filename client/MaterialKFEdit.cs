using Kingdee.BOS.App.Data;
using Kingdee.BOS.Core.Bill.PlugIn;
using Kingdee.BOS.Orm.DataEntity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kingdee.K3.WEINA.SCM.PlugIn.client
{
    [Description("物料编辑界面")]
    public class MaterialKFEdit : AbstractBillPlugIn
    {
        
        public override void AfterBindData(EventArgs e)
        {
            base.AfterBindData(e);
            //通过物料ID去查看是否有审核销售订单下的销售订单，有则最大销量不能编辑 ，否则可以编辑。
            //if (this.View.IdList.CurrentId.id != null)
            //{
            //    int idCount = queryPurInStock();
            //    if (idCount > 0)
            //    {
            //        this.View.GetControl("F_PAEZ_maxSaleQty").Enabled = false;
            //        this.View.GetControl("F_PAEZ_isLimitSaleQty").Enabled = false;
            //        this.View.GetControl("F_PAEZ_maxSaleQty").Enabled = false;
                    
            //    }
            //    else
            //    {
            //        this.View.GetControl("F_PAEZ_maxSaleQty").Enabled = true;
            //        this.View.GetControl("F_PAEZ_isLimitSaleQty").Enabled = true;
            //        this.View.GetControl("F_PAEZ_maxSaleQty").Enabled = true;
                    
            //    }
            //}
        }
        private int queryPurInStock()
        {
            string sql = string.Format(@"SELECT DISTINCT a.FID FROM T_SAL_ORDER a inner join T_SAL_ORDERENTRY b  on a.fid =b.fid  WHERE b.FMATERIALID ='{0}' AND a.FDOCUMENTSTATUS in ('B','C') ", this.View.IdList.CurrentId.id);
            DynamicObjectCollection saleOrderIDs = DBUtils.ExecuteDynamicObject(this.Context, sql);
            return saleOrderIDs.Count;
        }
    }
}
