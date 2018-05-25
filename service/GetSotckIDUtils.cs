using Kingdee.BOS.Orm.DataEntity;
using Kingdee.BOS.ServiceHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kingdee.K3.WEINA.SCM.PlugIn.service
{
    public class GetSubStockNameUtils
    {
        List<DynamicStockObject> fldKeyList = new List<DynamicStockObject>();


        //internal List<DynamicStockObject> getStockID(BOS.Context context, int id)
        //{
        //    DynamicObjectCollection whCol = null;
        //    string cknameSql = " select a.stockid from T_BD_STOCK a inner join T_BD_STOCKGROUP b on b.FID=a.FGROUP  WHERE b.fnumber = '02' order by a.fnumber ";
        //    whCol = DBServiceHelper.ExecuteDynamicObject(context, cknameSql) as DynamicObjectCollection;
        //    foreach (DynamicObject ckname in whCol)
        //    {
        //        fldKeyList.Add(Convert.ToString(ckname["STOCKID"]));
        //    }
        //    return fldKeyList;
        //    throw new NotImplementedException();
        //}

        internal List<DynamicStockObject> getStockID(BOS.Context context)
        {
            DynamicObjectCollection whCol = null;
            string cknameSql = " SELECT a.FSTOCKID AS STOCKID, a.FNUMBER AS FNUMBER ,c.FNAME AS FNAME FROM T_BD_STOCK a INNER JOIN T_BD_STOCKGROUP b ON b.FID = a.FGROUP  " +
                                                "LEFT JOIN T_BD_STOCK_L C ON C.FSTOCKID = a.FSTOCKID WHERE b.fnumber = '02' ORDER BY a.fnumber";
            whCol = DBServiceHelper.ExecuteDynamicObject(context, cknameSql) as DynamicObjectCollection;
            foreach (DynamicObject ckname in whCol)
            {
                DynamicStockObject stockObject = new DynamicStockObject();
                stockObject.stockid = Convert.ToInt32(ckname["STOCKID"]);
                stockObject.ckName = Convert.ToString(ckname["FNUMBER"]);
                stockObject.StockName = Convert.ToString(ckname["FNAME"]);
                fldKeyList.Add(stockObject);
            }
            return fldKeyList;
            throw new NotImplementedException();
        }

    }
}
