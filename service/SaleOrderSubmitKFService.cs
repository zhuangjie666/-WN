using Kingdee.BOS;
using Kingdee.BOS.App.Data;
using Kingdee.BOS.Core.DynamicForm;
using Kingdee.BOS.Core.DynamicForm.PlugIn;
using Kingdee.BOS.Core.Validation;
using Kingdee.BOS.Orm.DataEntity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kingdee.K3.WEINA.SCM.PlugIn.service
{
    [Description("销售订单提交")]
    public class SaleOrderSubmitKFService : AbstractOperationServicePlugIn
    {
        public override void OnPreparePropertys(BOS.Core.DynamicForm.PlugIn.Args.PreparePropertysEventArgs e)
        {
            base.OnPreparePropertys(e);
            e.FieldKeys.Add("FSaleOrderEntry_Link");
          

        }
        public override void OnAddValidators(BOS.Core.DynamicForm.PlugIn.Args.AddValidatorsEventArgs e)
        {
            base.OnAddValidators(e);
            //new自定义校验器

            OrderSubmitValidator orderSubmitValidator = new OrderSubmitValidator();
            //传入要校验的单据体唯一标识
            orderSubmitValidator.EntityKey = "FSaleOrderEntry";
            //将校验器对象添加到当前e.Validators中
            e.Validators.Add(orderSubmitValidator);
        }

    }
    public class OrderSubmitValidator : AbstractValidator
    {
        public override void Validate(BOS.Core.ExtendedDataEntity[] dataEntities, ValidateContext validateContext, BOS.Context ctx)
        {
            Dictionary<string, decimal> dic = new Dictionary<string, decimal>();
            foreach (var row in dataEntities)
            {
                DynamicObject material = row["MaterialID"] as DynamicObject;
                decimal qty = Convert.ToDecimal(row["Qty"]);
                decimal maxQty = Convert.ToDecimal(material["F_PAEZ_maxSaleQty"]);
                Boolean isLimitSaleQty = Convert.ToBoolean(material["F_PAEZ_isLimitSaleQty"]);
                DateTime startDate=  Convert.ToDateTime(material["F_PAEZ_startDate"]);
                int seq = Convert.ToInt32(row["Seq"]);
                if (null == material || !isLimitSaleQty || startDate > DateTime.Now || Convert.ToString( startDate)=="0001-01-01 00:00:00")
                {
                    continue;
                }
                //用字典记录下上面物料的数量   最大销量是这个单据上所有数量 + 其它已经审核 + 其它审核中 的数量
                string number = material["Number"].ToString();
                if (dic.ContainsKey(number))
                {
                    decimal qty1 = dic[number];
                    dic.Remove(number);
                    dic.Add(number, qty1 + qty);
                }
                else
                {
                    dic.Add(number, qty);   
                }

                string sql = string.Format(@"SELECT sum(b.FQTY) qty FROM T_SAL_ORDER a inner join T_SAL_ORDERENTRY b  on a.fid =b.fid  WHERE b.FMATERIALID ='{0}' AND a.FDOCUMENTSTATUS in ('B','C') and a.FDATE>='{1}'", material["Id"], startDate.Date);
                DynamicObjectCollection saleOrderCol = DBUtils.ExecuteDynamicObject(this.Context, sql);
                if (saleOrderCol.Count > 0)
                {
                    qty = dic[number] + Convert.ToDecimal(saleOrderCol[0]["qty"]);
                }
                if (qty > maxQty)
                {
                    ValidationErrorInfo errinfo = new ValidationErrorInfo(
                    "FSeq", //单据体序号标识
                    Convert.ToString(row.DataEntity["Id"]),
                    row.DataEntityIndex,
                    Convert.ToInt32(row["Id"]),
                    "SubmitValidator",//
                    "序号为" + seq + "这行数量大于此物料的最大销量，不允许提交当前销售订单！请更换其他产品.", //提示信息内容
                    "校验失败", //提示信息标题
                    ErrorLevel.Error);

                    //添加错误信息到validateContext中
                    validateContext.AddError(row, errinfo);
                }
                  
            }
        }
    }
}
