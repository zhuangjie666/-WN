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
       [Description("销售排名报表-按客户")]
    public class SalerRankingListReportByCust : SysReportBaseService
    {
           string filterContidion = string.Empty;
           string ShuoHuo = "F_PAEZ_ShouHuo";
           string ShuoHuoCondition = "";
           string agent4ProvinceID = "F_PAEZ_AgentProvince";
           string countryCustomerID = "F_PAEZ_COUNTRYCUST";
           string cityCustomerID = "F_PAEZ_CITYCUST";
           string agent4ProvinceCondiotn = "";
           string countryCustomerCondiotn = "";
           string cityCustomerCondiotn = "";
           string saleGroup = "F_PAEZ_saleGroup";
           string saleGroupCondition = "";
           string customerCondition;
           string customerID = "F_PAEZ_Customer";
        public override void Initialize()
        {

            this.ReportProperty.ReportName = new LocaleValue("销售排名报表-客户", this.Context.UserLocale.LCID);
            this.ReportProperty.IdentityFieldName = "FIDENTITYID";
            List<DecimalControlField> list = new List<DecimalControlField>();

            list.Add(new DecimalControlField
            {
                ByDecimalControlFieldName = "JANSALENO",
                DecimalControlFieldName = "JinDu"
            });

            list.Add(new DecimalControlField
            {
                ByDecimalControlFieldName = "JANSALEAMT",
                DecimalControlFieldName = "JinDu"
            });

            list.Add(new DecimalControlField
            {
                ByDecimalControlFieldName = "JANZENGPINNO",
                DecimalControlFieldName = "JinDu"
            });

            list.Add(new DecimalControlField
            {
                ByDecimalControlFieldName = "JANZENGPINAMT",
                DecimalControlFieldName = "JinDu"
            });

            list.Add(new DecimalControlField
            {
                ByDecimalControlFieldName = "JANSUMTOTAL",
                DecimalControlFieldName = "JinDu"
            });

            list.Add(new DecimalControlField
            {
                ByDecimalControlFieldName = "JANSUMAMT",
                DecimalControlFieldName = "JinDu"
            });

            list.Add(new DecimalControlField
            {
                ByDecimalControlFieldName = "JANNOSENDNO",
                DecimalControlFieldName = "JinDu"
            });

            list.Add(new DecimalControlField
            {
                ByDecimalControlFieldName = "JANNOSENDAMT",
                DecimalControlFieldName = "JinDu"
            });

            list.Add(new DecimalControlField
            {
                ByDecimalControlFieldName = "FEBSALENO",
                DecimalControlFieldName = "JinDu"
            });

            list.Add(new DecimalControlField
            {
                ByDecimalControlFieldName = "FEBSALEAMT",
                DecimalControlFieldName = "JinDu"
            });

            list.Add(new DecimalControlField
            {
                ByDecimalControlFieldName = "FEBZENGPINNO",
                DecimalControlFieldName = "JinDu"
            });

            list.Add(new DecimalControlField
            {
                ByDecimalControlFieldName = "FEBZENGPINAMT",
                DecimalControlFieldName = "JinDu"
            });

            list.Add(new DecimalControlField
            {
                ByDecimalControlFieldName = "FEBSUMTOTAL",
                DecimalControlFieldName = "JinDu"
            });

            list.Add(new DecimalControlField
            {
                ByDecimalControlFieldName = "FEBSUMAMT",
                DecimalControlFieldName = "JinDu"
            });

            list.Add(new DecimalControlField
            {
                ByDecimalControlFieldName = "FEBNOSENDNO",
                DecimalControlFieldName = "JinDu"
            });

            list.Add(new DecimalControlField
            {
                ByDecimalControlFieldName = "FEBNOSENDAMT",
                DecimalControlFieldName = "JinDu"
            });

            list.Add(new DecimalControlField
            {
                ByDecimalControlFieldName = "MARSALENO",
                DecimalControlFieldName = "JinDu"
            });

            list.Add(new DecimalControlField
            {
                ByDecimalControlFieldName = "MARSALEAMT",
                DecimalControlFieldName = "JinDu"
            });

            list.Add(new DecimalControlField
            {
                ByDecimalControlFieldName = "MARZENGPINNO",
                DecimalControlFieldName = "JinDu"
            });

            list.Add(new DecimalControlField
            {
                ByDecimalControlFieldName = "MARZENGPINAMT",
                DecimalControlFieldName = "JinDu"
            });

            list.Add(new DecimalControlField
            {
                ByDecimalControlFieldName = "MARSUMTOTAL",
                DecimalControlFieldName = "JinDu"
            });

            list.Add(new DecimalControlField
            {
                ByDecimalControlFieldName = "MARSUMAMT",
                DecimalControlFieldName = "JinDu"
            });

            list.Add(new DecimalControlField
            {
                ByDecimalControlFieldName = "MARNOSENDNO",
                DecimalControlFieldName = "JinDu"
            });

            list.Add(new DecimalControlField
            {
                ByDecimalControlFieldName = "MARNOSENDAMT",
                DecimalControlFieldName = "JinDu"
            });

            list.Add(new DecimalControlField
            {
                ByDecimalControlFieldName = "APRSALENO",
                DecimalControlFieldName = "JinDu"
            });

            list.Add(new DecimalControlField
            {
                ByDecimalControlFieldName = "APRSALEAMT",
                DecimalControlFieldName = "JinDu"
            });

            list.Add(new DecimalControlField
            {
                ByDecimalControlFieldName = "APRZENGPINNO",
                DecimalControlFieldName = "JinDu"
            });

            list.Add(new DecimalControlField
            {
                ByDecimalControlFieldName = "APRZENGPINAMT",
                DecimalControlFieldName = "JinDu"
            });

            list.Add(new DecimalControlField
            {
                ByDecimalControlFieldName = "APRSUMTOTAL",
                DecimalControlFieldName = "JinDu"
            });

            list.Add(new DecimalControlField
            {
                ByDecimalControlFieldName = "APRSUMAMT",
                DecimalControlFieldName = "JinDu"
            });

            list.Add(new DecimalControlField
            {
                ByDecimalControlFieldName = "APRNOSENDNO",
                DecimalControlFieldName = "JinDu"
            });

            list.Add(new DecimalControlField
            {
                ByDecimalControlFieldName = "APRNOSENDAMT",
                DecimalControlFieldName = "JinDu"
            });

            list.Add(new DecimalControlField
            {
                ByDecimalControlFieldName = "MAYSALENO",
                DecimalControlFieldName = "JinDu"
            });

            list.Add(new DecimalControlField
            {
                ByDecimalControlFieldName = "MAYSALEAMT",
                DecimalControlFieldName = "JinDu"
            });

            list.Add(new DecimalControlField
            {
                ByDecimalControlFieldName = "MAYZENGPINNO",
                DecimalControlFieldName = "JinDu"
            });

            list.Add(new DecimalControlField
            {
                ByDecimalControlFieldName = "MAYZENGPINAMT",
                DecimalControlFieldName = "JinDu"
            });

            list.Add(new DecimalControlField
            {
                ByDecimalControlFieldName = "MAYSUMTOTAL",
                DecimalControlFieldName = "JinDu"
            });

            list.Add(new DecimalControlField
            {
                ByDecimalControlFieldName = "MAYSUMAMT",
                DecimalControlFieldName = "JinDu"
            });

            list.Add(new DecimalControlField
            {
                ByDecimalControlFieldName = "MAYNOSENDNO",
                DecimalControlFieldName = "JinDu"
            });

            list.Add(new DecimalControlField
            {
                ByDecimalControlFieldName = "MAYNOSENDAMT",
                DecimalControlFieldName = "JinDu"
            });

            list.Add(new DecimalControlField
            {
                ByDecimalControlFieldName = "JUNSALENO",
                DecimalControlFieldName = "JinDu"
            });

            list.Add(new DecimalControlField
            {
                ByDecimalControlFieldName = "JUNSALEAMT",
                DecimalControlFieldName = "JinDu"
            });

            list.Add(new DecimalControlField
            {
                ByDecimalControlFieldName = "JUNZENGPINNO",
                DecimalControlFieldName = "JinDu"
            });

            list.Add(new DecimalControlField
            {
                ByDecimalControlFieldName = "JUNZENGPINAMT",
                DecimalControlFieldName = "JinDu"
            });

            list.Add(new DecimalControlField
            {
                ByDecimalControlFieldName = "JUNSUMTOTAL",
                DecimalControlFieldName = "JinDu"
            });

            list.Add(new DecimalControlField
            {
                ByDecimalControlFieldName = "JUNSUMAMT",
                DecimalControlFieldName = "JinDu"
            });

            list.Add(new DecimalControlField
            {
                ByDecimalControlFieldName = "JUNNOSENDNO",
                DecimalControlFieldName = "JinDu"
            });

            list.Add(new DecimalControlField
            {
                ByDecimalControlFieldName = "JUNNOSENDAMT",
                DecimalControlFieldName = "JinDu"
            });

            list.Add(new DecimalControlField
            {
                ByDecimalControlFieldName = "JULSALENO",
                DecimalControlFieldName = "JinDu"
            });

            list.Add(new DecimalControlField
            {
                ByDecimalControlFieldName = "JULSALEAMT",
                DecimalControlFieldName = "JinDu"
            });

            list.Add(new DecimalControlField
            {
                ByDecimalControlFieldName = "JULZENGPINNO",
                DecimalControlFieldName = "JinDu"
            });

            list.Add(new DecimalControlField
            {
                ByDecimalControlFieldName = "JULZENGPINAMT",
                DecimalControlFieldName = "JinDu"
            });

            list.Add(new DecimalControlField
            {
                ByDecimalControlFieldName = "JULSUMTOTAL",
                DecimalControlFieldName = "JinDu"
            });

            list.Add(new DecimalControlField
            {
                ByDecimalControlFieldName = "JULSUMAMT",
                DecimalControlFieldName = "JinDu"
            });

            list.Add(new DecimalControlField
            {
                ByDecimalControlFieldName = "JULNOSENDNO",
                DecimalControlFieldName = "JinDu"
            });

            list.Add(new DecimalControlField
            {
                ByDecimalControlFieldName = "JULNOSENDAMT",
                DecimalControlFieldName = "JinDu"
            });

            list.Add(new DecimalControlField
            {
                ByDecimalControlFieldName = "AUGSALENO",
                DecimalControlFieldName = "JinDu"
            });

            list.Add(new DecimalControlField
            {
                ByDecimalControlFieldName = "AUGSALEAMT",
                DecimalControlFieldName = "JinDu"
            });

            list.Add(new DecimalControlField
            {
                ByDecimalControlFieldName = "AUGZENGPINNO",
                DecimalControlFieldName = "JinDu"
            });

            list.Add(new DecimalControlField
            {
                ByDecimalControlFieldName = "AUGZENGPINAMT",
                DecimalControlFieldName = "JinDu"
            });

            list.Add(new DecimalControlField
            {
                ByDecimalControlFieldName = "AUGSUMTOTAL",
                DecimalControlFieldName = "JinDu"
            });

            list.Add(new DecimalControlField
            {
                ByDecimalControlFieldName = "AUGSUMAMT",
                DecimalControlFieldName = "JinDu"
            });

            list.Add(new DecimalControlField
            {
                ByDecimalControlFieldName = "AUGNOSENDNO",
                DecimalControlFieldName = "JinDu"
            });

            list.Add(new DecimalControlField
            {
                ByDecimalControlFieldName = "AUGNOSENDAMT",
                DecimalControlFieldName = "JinDu"
            });

            list.Add(new DecimalControlField
            {
                ByDecimalControlFieldName = "SEPSALENO",
                DecimalControlFieldName = "JinDu"
            });

            list.Add(new DecimalControlField
            {
                ByDecimalControlFieldName = "SEPSALEAMT",
                DecimalControlFieldName = "JinDu"
            });

            list.Add(new DecimalControlField
            {
                ByDecimalControlFieldName = "SEPZENGPINNO",
                DecimalControlFieldName = "JinDu"
            });

            list.Add(new DecimalControlField
            {
                ByDecimalControlFieldName = "SEPZENGPINAMT",
                DecimalControlFieldName = "JinDu"
            });

            list.Add(new DecimalControlField
            {
                ByDecimalControlFieldName = "SEPSUMTOTAL",
                DecimalControlFieldName = "JinDu"
            });

            list.Add(new DecimalControlField
            {
                ByDecimalControlFieldName = "SEPSUMAMT",
                DecimalControlFieldName = "JinDu"
            });

            list.Add(new DecimalControlField
            {
                ByDecimalControlFieldName = "SEPNOSENDNO",
                DecimalControlFieldName = "JinDu"
            });

            list.Add(new DecimalControlField
            {
                ByDecimalControlFieldName = "SEPNOSENDAMT",
                DecimalControlFieldName = "JinDu"
            });


            list.Add(new DecimalControlField
            {
                ByDecimalControlFieldName = "OCTSALENO",
                DecimalControlFieldName = "JinDu"
            });

            list.Add(new DecimalControlField
            {
                ByDecimalControlFieldName = "OCTSALEAMT",
                DecimalControlFieldName = "JinDu"
            });

            list.Add(new DecimalControlField
            {
                ByDecimalControlFieldName = "OCTZENGPINNO",
                DecimalControlFieldName = "JinDu"
            });

            list.Add(new DecimalControlField
            {
                ByDecimalControlFieldName = "OCTZENGPINAMT",
                DecimalControlFieldName = "JinDu"
            });

            list.Add(new DecimalControlField
            {
                ByDecimalControlFieldName = "OCTSUMTOTAL",
                DecimalControlFieldName = "JinDu"
            });

            list.Add(new DecimalControlField
            {
                ByDecimalControlFieldName = "OCTSUMAMT",
                DecimalControlFieldName = "JinDu"
            });

            list.Add(new DecimalControlField
            {
                ByDecimalControlFieldName = "OCTNOSENDNO",
                DecimalControlFieldName = "JinDu"
            });

            list.Add(new DecimalControlField
            {
                ByDecimalControlFieldName = "OCTNOSENDAMT",
                DecimalControlFieldName = "JinDu"
            });

            list.Add(new DecimalControlField
            {
                ByDecimalControlFieldName = "NOVSALENO",
                DecimalControlFieldName = "JinDu"
            });

            list.Add(new DecimalControlField
            {
                ByDecimalControlFieldName = "NOVSALEAMT",
                DecimalControlFieldName = "JinDu"
            });

            list.Add(new DecimalControlField
            {
                ByDecimalControlFieldName = "NOVZENGPINNO",
                DecimalControlFieldName = "JinDu"
            });

            list.Add(new DecimalControlField
            {
                ByDecimalControlFieldName = "NOVZENGPINAMT",
                DecimalControlFieldName = "JinDu"
            });

            list.Add(new DecimalControlField
            {
                ByDecimalControlFieldName = "NOVSUMTOTAL",
                DecimalControlFieldName = "JinDu"
            });

            list.Add(new DecimalControlField
            {
                ByDecimalControlFieldName = "NOVSUMAMT",
                DecimalControlFieldName = "JinDu"
            });

            list.Add(new DecimalControlField
            {
                ByDecimalControlFieldName = "NOVNOSENDNO",
                DecimalControlFieldName = "JinDu"
            });

            list.Add(new DecimalControlField
            {
                ByDecimalControlFieldName = "NOVNOSENDAMT",
                DecimalControlFieldName = "JinDu"
            });

            list.Add(new DecimalControlField
            {
                ByDecimalControlFieldName = "DECSALENO",
                DecimalControlFieldName = "JinDu"
            });

            list.Add(new DecimalControlField
            {
                ByDecimalControlFieldName = "DECSALEAMT",
                DecimalControlFieldName = "JinDu"
            });

            list.Add(new DecimalControlField
            {
                ByDecimalControlFieldName = "DECZENGPINNO",
                DecimalControlFieldName = "JinDu"
            });

            list.Add(new DecimalControlField
            {
                ByDecimalControlFieldName = "DECZENGPINAMT",
                DecimalControlFieldName = "JinDu"
            });

            list.Add(new DecimalControlField
            {
                ByDecimalControlFieldName = "DECSUMTOTAL",
                DecimalControlFieldName = "JinDu"
            });

            list.Add(new DecimalControlField
            {
                ByDecimalControlFieldName = "DECSUMAMT",
                DecimalControlFieldName = "JinDu"
            });

            list.Add(new DecimalControlField
            {
                ByDecimalControlFieldName = "DECNOSENDNO",
                DecimalControlFieldName = "JinDu"
            });

            list.Add(new DecimalControlField
            {
                ByDecimalControlFieldName = "DECNOSENDAMT",
                DecimalControlFieldName = "JinDu"
            });

            list.Add(new DecimalControlField
            {
                ByDecimalControlFieldName = "ALLYEARSALENO",
                DecimalControlFieldName = "JinDu"
            });

            list.Add(new DecimalControlField
            {
                ByDecimalControlFieldName = "ALLYEARSALEAMT",
                DecimalControlFieldName = "JinDu"
            });

            list.Add(new DecimalControlField
            {
                ByDecimalControlFieldName = "ALLYEARZENGPINNO",
                DecimalControlFieldName = "JinDu"
            });

            list.Add(new DecimalControlField
            {
                ByDecimalControlFieldName = "ALLYEARZENGPINAMT",
                DecimalControlFieldName = "JinDu"
            });

            list.Add(new DecimalControlField
            {
                ByDecimalControlFieldName = "ALLYEARSUMTOTAL",
                DecimalControlFieldName = "JinDu"
            });

            list.Add(new DecimalControlField
            {
                ByDecimalControlFieldName = "ALLYEARSUMAMT",
                DecimalControlFieldName = "JinDu"
            });

            list.Add(new DecimalControlField
            {
                ByDecimalControlFieldName = "ALLYEARNOSENDNO",
                DecimalControlFieldName = "JinDu"
            });

            list.Add(new DecimalControlField
            {
                ByDecimalControlFieldName = "ALLYEARNOSENDAMT",
                DecimalControlFieldName = "JinDu"
            });

            list.Add(new DecimalControlField
            {
                ByDecimalControlFieldName = "FPRICE",
                DecimalControlFieldName = "JinDu"
            });
            this.ReportProperty.DecimalControlFieldList = list;
        }
        public override void BuilderReportSqlAndTempTable(IRptParams filter, string tableName)
        {
            string executeSQL = string.Empty;

            SQLStaticStatements sqlAllDetail = new SQLStaticStatements();
            string searchCondition = string.Empty;
            string sqlAll = sqlAllDetail.returnSQL4SalerRankListReportByCust(searchCondition);

            DynamicObject customFilter = filter.FilterParameter.CustomFilter;
            string selectedYear = Convert.ToString(customFilter["F_PAEZ_selectedYear"]); //必选

            Dictionary<string, string> conditionMap = getOptionConditionFilter(filter);

            foreach (KeyValuePair<string, string> kv in conditionMap)
            {
                if (kv.Key.Equals(ShuoHuo))
                {
                    ShuoHuoCondition = Convert.ToString(kv.Value);
                    searchCondition = " AND T6.FCUSTID = " + ShuoHuoCondition;
                    sqlAll = sqlAllDetail.returnSQL4SalerRankListReportByCust(searchCondition);
                }

                if (kv.Key.Equals(agent4ProvinceID))
                {
                    agent4ProvinceCondiotn = Convert.ToString(kv.Value);
                    searchCondition = searchCondition + " AND T1.F_PAEZ_provinceCus = " + agent4ProvinceCondiotn;
                    sqlAll = sqlAllDetail.returnSQL4SalerRankListReportByCust(searchCondition);
                }

                if (kv.Key.Equals(cityCustomerID))
                {
                    cityCustomerCondiotn = Convert.ToString(kv.Value);
                    searchCondition = searchCondition + " AND T1.F_PAEZ_CITYCUS = " + cityCustomerCondiotn;
                    sqlAll = sqlAllDetail.returnSQL4SalerRankListReportByCust(searchCondition);
                }

                if (kv.Key.Equals(countryCustomerID))
                {
                    countryCustomerCondiotn = Convert.ToString(kv.Value);
                    searchCondition = searchCondition + " AND T1.F_PAEZ_COUNTRYCUS = " + countryCustomerCondiotn;
                    sqlAll = sqlAllDetail.returnSQL4SalerRankListReportByCust(searchCondition);
                }

                if (kv.Key.Equals(customerID))
                {
                    customerCondition = Convert.ToString(kv.Value);
                    searchCondition = searchCondition + "  AND T1.FCUSTID = " + customerCondition;
                    sqlAll = sqlAllDetail.returnSQL4SalerRankListReportByCust(searchCondition);
                }


                if (kv.Key.Equals(saleGroup))
                {
                    saleGroupCondition = Convert.ToString(kv.Value);
                    searchCondition = searchCondition + " AND T1.FSaleGroupId = " + saleGroupCondition;
                    sqlAll = sqlAllDetail.returnSQL4SalerRankListReportByCust(searchCondition);
                }

            }

            executeSQL = string.Format(sqlAll, tableName, selectedYear);
            DBUtils.Execute(this.Context, executeSQL);


            ////全年总销售,以及金额的更新
            string updateALLYearSQL = string.Format(sqlAllDetail.returnSQL4UpdateAllYear(), tableName);
            DBUtils.Execute(this.Context, updateALLYearSQL);


        }

        public override List<Kingdee.BOS.Core.Report.SummaryField> GetSummaryColumnInfo(IRptParams filter)
        {
            List<Kingdee.BOS.Core.Report.SummaryField> summarys = new List<Kingdee.BOS.Core.Report.SummaryField>();
            summarys.Add(new SummaryField("JANSALENO", BOS.Core.Enums.BOSEnums.Enu_SummaryType.SUM));
            summarys.Add(new SummaryField("JANSALEAMT", BOS.Core.Enums.BOSEnums.Enu_SummaryType.SUM));
            summarys.Add(new SummaryField("JANZENGPINNO", BOS.Core.Enums.BOSEnums.Enu_SummaryType.SUM));
            summarys.Add(new SummaryField("JANZENGPINAMT", BOS.Core.Enums.BOSEnums.Enu_SummaryType.SUM));
            summarys.Add(new SummaryField("JANSUMTOTAL", BOS.Core.Enums.BOSEnums.Enu_SummaryType.SUM));
            summarys.Add(new SummaryField("JANSUMAMT", BOS.Core.Enums.BOSEnums.Enu_SummaryType.SUM));
            summarys.Add(new SummaryField("JANNOSENDNO", BOS.Core.Enums.BOSEnums.Enu_SummaryType.SUM));
            summarys.Add(new SummaryField("JANNOSENDAMT", BOS.Core.Enums.BOSEnums.Enu_SummaryType.SUM));

            summarys.Add(new SummaryField("FEBSALENO", BOS.Core.Enums.BOSEnums.Enu_SummaryType.SUM));
            summarys.Add(new SummaryField("FEBSALEAMT", BOS.Core.Enums.BOSEnums.Enu_SummaryType.SUM));
            summarys.Add(new SummaryField("FEBZENGPINNO", BOS.Core.Enums.BOSEnums.Enu_SummaryType.SUM));
            summarys.Add(new SummaryField("FEBZENGPINAMT", BOS.Core.Enums.BOSEnums.Enu_SummaryType.SUM));
            summarys.Add(new SummaryField("FEBSUMTOTAL", BOS.Core.Enums.BOSEnums.Enu_SummaryType.SUM));
            summarys.Add(new SummaryField("FEBSUMAMT", BOS.Core.Enums.BOSEnums.Enu_SummaryType.SUM));
            summarys.Add(new SummaryField("FEBNOSENDNO", BOS.Core.Enums.BOSEnums.Enu_SummaryType.SUM));
            summarys.Add(new SummaryField("FEBNOSENDAMT", BOS.Core.Enums.BOSEnums.Enu_SummaryType.SUM));

            summarys.Add(new SummaryField("MARSALENO", BOS.Core.Enums.BOSEnums.Enu_SummaryType.SUM));
            summarys.Add(new SummaryField("MARSALEAMT", BOS.Core.Enums.BOSEnums.Enu_SummaryType.SUM));
            summarys.Add(new SummaryField("MARZENGPINNO", BOS.Core.Enums.BOSEnums.Enu_SummaryType.SUM));
            summarys.Add(new SummaryField("MARZENGPINAMT", BOS.Core.Enums.BOSEnums.Enu_SummaryType.SUM));
            summarys.Add(new SummaryField("MARSUMTOTAL", BOS.Core.Enums.BOSEnums.Enu_SummaryType.SUM));
            summarys.Add(new SummaryField("MARSUMAMT", BOS.Core.Enums.BOSEnums.Enu_SummaryType.SUM));
            summarys.Add(new SummaryField("MARNOSENDNO", BOS.Core.Enums.BOSEnums.Enu_SummaryType.SUM));
            summarys.Add(new SummaryField("MARNOSENDAMT", BOS.Core.Enums.BOSEnums.Enu_SummaryType.SUM));

            summarys.Add(new SummaryField("APRSALENO", BOS.Core.Enums.BOSEnums.Enu_SummaryType.SUM));
            summarys.Add(new SummaryField("APRSALEAMT", BOS.Core.Enums.BOSEnums.Enu_SummaryType.SUM));
            summarys.Add(new SummaryField("APRZENGPINNO", BOS.Core.Enums.BOSEnums.Enu_SummaryType.SUM));
            summarys.Add(new SummaryField("APRZENGPINAMT", BOS.Core.Enums.BOSEnums.Enu_SummaryType.SUM));
            summarys.Add(new SummaryField("APRSUMTOTAL", BOS.Core.Enums.BOSEnums.Enu_SummaryType.SUM));
            summarys.Add(new SummaryField("APRSUMAMT", BOS.Core.Enums.BOSEnums.Enu_SummaryType.SUM));
            summarys.Add(new SummaryField("APRNOSENDNO", BOS.Core.Enums.BOSEnums.Enu_SummaryType.SUM));
            summarys.Add(new SummaryField("APRNOSENDAMT", BOS.Core.Enums.BOSEnums.Enu_SummaryType.SUM));

            summarys.Add(new SummaryField("MAYSALENO", BOS.Core.Enums.BOSEnums.Enu_SummaryType.SUM));
            summarys.Add(new SummaryField("MAYSALEAMT", BOS.Core.Enums.BOSEnums.Enu_SummaryType.SUM));
            summarys.Add(new SummaryField("MAYZENGPINNO", BOS.Core.Enums.BOSEnums.Enu_SummaryType.SUM));
            summarys.Add(new SummaryField("MAYZENGPINAMT", BOS.Core.Enums.BOSEnums.Enu_SummaryType.SUM));
            summarys.Add(new SummaryField("MAYSUMTOTAL", BOS.Core.Enums.BOSEnums.Enu_SummaryType.SUM));
            summarys.Add(new SummaryField("MAYSUMAMT", BOS.Core.Enums.BOSEnums.Enu_SummaryType.SUM));
            summarys.Add(new SummaryField("MAYNOSENDNO", BOS.Core.Enums.BOSEnums.Enu_SummaryType.SUM));
            summarys.Add(new SummaryField("MAYNOSENDAMT", BOS.Core.Enums.BOSEnums.Enu_SummaryType.SUM));

            summarys.Add(new SummaryField("JUNSALENO", BOS.Core.Enums.BOSEnums.Enu_SummaryType.SUM));
            summarys.Add(new SummaryField("JUNSALEAMT", BOS.Core.Enums.BOSEnums.Enu_SummaryType.SUM));
            summarys.Add(new SummaryField("JUNZENGPINNO", BOS.Core.Enums.BOSEnums.Enu_SummaryType.SUM));
            summarys.Add(new SummaryField("JUNZENGPINAMT", BOS.Core.Enums.BOSEnums.Enu_SummaryType.SUM));
            summarys.Add(new SummaryField("JUNSUMTOTAL", BOS.Core.Enums.BOSEnums.Enu_SummaryType.SUM));
            summarys.Add(new SummaryField("JUNSUMAMT", BOS.Core.Enums.BOSEnums.Enu_SummaryType.SUM));
            summarys.Add(new SummaryField("JUNNOSENDNO", BOS.Core.Enums.BOSEnums.Enu_SummaryType.SUM));
            summarys.Add(new SummaryField("JUNNOSENDAMT", BOS.Core.Enums.BOSEnums.Enu_SummaryType.SUM));

            summarys.Add(new SummaryField("JULSALENO", BOS.Core.Enums.BOSEnums.Enu_SummaryType.SUM));
            summarys.Add(new SummaryField("JULSALEAMT", BOS.Core.Enums.BOSEnums.Enu_SummaryType.SUM));
            summarys.Add(new SummaryField("JULZENGPINNO", BOS.Core.Enums.BOSEnums.Enu_SummaryType.SUM));
            summarys.Add(new SummaryField("JULZENGPINAMT", BOS.Core.Enums.BOSEnums.Enu_SummaryType.SUM));
            summarys.Add(new SummaryField("JULSUMTOTAL", BOS.Core.Enums.BOSEnums.Enu_SummaryType.SUM));
            summarys.Add(new SummaryField("JULSUMAMT", BOS.Core.Enums.BOSEnums.Enu_SummaryType.SUM));
            summarys.Add(new SummaryField("JULNOSENDNO", BOS.Core.Enums.BOSEnums.Enu_SummaryType.SUM));
            summarys.Add(new SummaryField("JULNOSENDAMT", BOS.Core.Enums.BOSEnums.Enu_SummaryType.SUM));

            summarys.Add(new SummaryField("AUGSALENO", BOS.Core.Enums.BOSEnums.Enu_SummaryType.SUM));
            summarys.Add(new SummaryField("AUGSALEAMT", BOS.Core.Enums.BOSEnums.Enu_SummaryType.SUM));
            summarys.Add(new SummaryField("AUGZENGPINNO", BOS.Core.Enums.BOSEnums.Enu_SummaryType.SUM));
            summarys.Add(new SummaryField("AUGZENGPINAMT", BOS.Core.Enums.BOSEnums.Enu_SummaryType.SUM));
            summarys.Add(new SummaryField("AUGSUMTOTAL", BOS.Core.Enums.BOSEnums.Enu_SummaryType.SUM));
            summarys.Add(new SummaryField("AUGSUMAMT", BOS.Core.Enums.BOSEnums.Enu_SummaryType.SUM));
            summarys.Add(new SummaryField("AUGNOSENDNO", BOS.Core.Enums.BOSEnums.Enu_SummaryType.SUM));
            summarys.Add(new SummaryField("AUGNOSENDAMT", BOS.Core.Enums.BOSEnums.Enu_SummaryType.SUM));

            summarys.Add(new SummaryField("SEPSALENO", BOS.Core.Enums.BOSEnums.Enu_SummaryType.SUM));
            summarys.Add(new SummaryField("SEPSALEAMT", BOS.Core.Enums.BOSEnums.Enu_SummaryType.SUM));
            summarys.Add(new SummaryField("SEPZENGPINNO", BOS.Core.Enums.BOSEnums.Enu_SummaryType.SUM));
            summarys.Add(new SummaryField("SEPZENGPINAMT", BOS.Core.Enums.BOSEnums.Enu_SummaryType.SUM));
            summarys.Add(new SummaryField("SEPSUMTOTAL", BOS.Core.Enums.BOSEnums.Enu_SummaryType.SUM));
            summarys.Add(new SummaryField("SEPSUMAMT", BOS.Core.Enums.BOSEnums.Enu_SummaryType.SUM));
            summarys.Add(new SummaryField("SEPNOSENDNO", BOS.Core.Enums.BOSEnums.Enu_SummaryType.SUM));
            summarys.Add(new SummaryField("SEPNOSENDAMT", BOS.Core.Enums.BOSEnums.Enu_SummaryType.SUM));

            summarys.Add(new SummaryField("OCTSALENO", BOS.Core.Enums.BOSEnums.Enu_SummaryType.SUM));
            summarys.Add(new SummaryField("OCTSALEAMT", BOS.Core.Enums.BOSEnums.Enu_SummaryType.SUM));
            summarys.Add(new SummaryField("OCTZENGPINNO", BOS.Core.Enums.BOSEnums.Enu_SummaryType.SUM));
            summarys.Add(new SummaryField("OCTZENGPINAMT", BOS.Core.Enums.BOSEnums.Enu_SummaryType.SUM));
            summarys.Add(new SummaryField("OCTSUMTOTAL", BOS.Core.Enums.BOSEnums.Enu_SummaryType.SUM));
            summarys.Add(new SummaryField("OCTSUMAMT", BOS.Core.Enums.BOSEnums.Enu_SummaryType.SUM));
            summarys.Add(new SummaryField("OCTNOSENDNO", BOS.Core.Enums.BOSEnums.Enu_SummaryType.SUM));
            summarys.Add(new SummaryField("OCTNOSENDAMT", BOS.Core.Enums.BOSEnums.Enu_SummaryType.SUM));

            summarys.Add(new SummaryField("NOVSALENO", BOS.Core.Enums.BOSEnums.Enu_SummaryType.SUM));
            summarys.Add(new SummaryField("NOVSALEAMT", BOS.Core.Enums.BOSEnums.Enu_SummaryType.SUM));
            summarys.Add(new SummaryField("NOVZENGPINNO", BOS.Core.Enums.BOSEnums.Enu_SummaryType.SUM));
            summarys.Add(new SummaryField("NOVZENGPINAMT", BOS.Core.Enums.BOSEnums.Enu_SummaryType.SUM));
            summarys.Add(new SummaryField("NOVSUMTOTAL", BOS.Core.Enums.BOSEnums.Enu_SummaryType.SUM));
            summarys.Add(new SummaryField("NOVSUMAMT", BOS.Core.Enums.BOSEnums.Enu_SummaryType.SUM));
            summarys.Add(new SummaryField("NOVNOSENDNO", BOS.Core.Enums.BOSEnums.Enu_SummaryType.SUM));
            summarys.Add(new SummaryField("NOVNOSENDAMT", BOS.Core.Enums.BOSEnums.Enu_SummaryType.SUM));

            summarys.Add(new SummaryField("DECSALENO", BOS.Core.Enums.BOSEnums.Enu_SummaryType.SUM));
            summarys.Add(new SummaryField("DECSALEAMT", BOS.Core.Enums.BOSEnums.Enu_SummaryType.SUM));
            summarys.Add(new SummaryField("DECZENGPINNO", BOS.Core.Enums.BOSEnums.Enu_SummaryType.SUM));
            summarys.Add(new SummaryField("DECZENGPINAMT", BOS.Core.Enums.BOSEnums.Enu_SummaryType.SUM));
            summarys.Add(new SummaryField("DECSUMTOTAL", BOS.Core.Enums.BOSEnums.Enu_SummaryType.SUM));
            summarys.Add(new SummaryField("DECSUMAMT", BOS.Core.Enums.BOSEnums.Enu_SummaryType.SUM));
            summarys.Add(new SummaryField("DECNOSENDNO", BOS.Core.Enums.BOSEnums.Enu_SummaryType.SUM));
            summarys.Add(new SummaryField("DECNOSENDAMT", BOS.Core.Enums.BOSEnums.Enu_SummaryType.SUM));

            summarys.Add(new SummaryField("ALLYEARSALENO", BOS.Core.Enums.BOSEnums.Enu_SummaryType.SUM));
            summarys.Add(new SummaryField("ALLYEARSALEAMT", BOS.Core.Enums.BOSEnums.Enu_SummaryType.SUM));
            summarys.Add(new SummaryField("ALLYEARZENGPINNO", BOS.Core.Enums.BOSEnums.Enu_SummaryType.SUM));
            summarys.Add(new SummaryField("ALLYEARZENGPINAMT", BOS.Core.Enums.BOSEnums.Enu_SummaryType.SUM));
            summarys.Add(new SummaryField("ALLYEARSUMTOTAL", BOS.Core.Enums.BOSEnums.Enu_SummaryType.SUM));
            summarys.Add(new SummaryField("ALLYEARSUMAMT", BOS.Core.Enums.BOSEnums.Enu_SummaryType.SUM));
            summarys.Add(new SummaryField("ALLYEARNOSENDNO", BOS.Core.Enums.BOSEnums.Enu_SummaryType.SUM));
            summarys.Add(new SummaryField("ALLYEARNOSENDAMT", BOS.Core.Enums.BOSEnums.Enu_SummaryType.SUM));


            return summarys;
        }

        public override ReportTitles GetReportTitles(IRptParams filter)
        {
            ReportTitles titles = new ReportTitles();
            DynamicObject customFilter = filter.FilterParameter.CustomFilter;
            string selectedYear = Convert.ToString(customFilter["F_PAEZ_selectedYear"]);
            titles.AddTitle("F_PAEZ_saleGroup", "");
            titles.AddTitle("F_PAEZ_AgentProvince", "");
            titles.AddTitle("F_PAEZ_ShouHuo","");
            if (customFilter["F_PAEZ_saleGroup"] != null)
            {
                DynamicObject fmaterialObject = customFilter["F_PAEZ_saleGroup"] as DynamicObject;
                titles.AddTitle("F_PAEZ_saleGroup", Convert.ToString(fmaterialObject["name"]));
            }
            if (customFilter["F_PAEZ_AgentProvince"] != null)
            {
                DynamicObject fmaterialObject = customFilter["F_PAEZ_AgentProvince"] as DynamicObject;
                titles.AddTitle("F_PAEZ_AgentProvince", Convert.ToString(fmaterialObject["name"]));
            }

            if (customFilter["F_PAEZ_CITYCUST"] != null)
            {
                DynamicObject fmaterialObject = customFilter["F_PAEZ_CITYCUST"] as DynamicObject;
                titles.AddTitle("F_PAEZ_CITYCUST", Convert.ToString(fmaterialObject["name"]));
            }
            if (customFilter["F_PAEZ_COUNTRYCUST"] != null)
            {
                DynamicObject fmaterialObject = customFilter["F_PAEZ_COUNTRYCUST"] as DynamicObject;
                titles.AddTitle("F_PAEZ_COUNTRYCUST", Convert.ToString(fmaterialObject["name"]));
            }
            if (customFilter["F_PAEZ_Customer"] != null)
            {
                DynamicObject fmaterialObject = customFilter["F_PAEZ_customer"] as DynamicObject;
                titles.AddTitle("F_PAEZ_customer", Convert.ToString(fmaterialObject["name"]));
            }

            if (customFilter["F_PAEZ_ShouHuo"] != null)
            {
                DynamicObject fmaterialObject = customFilter["F_PAEZ_ShouHuo"] as DynamicObject;
                titles.AddTitle("F_PAEZ_ShouHuo", Convert.ToString(fmaterialObject["name"]));
            }
            titles.AddTitle("F_PAEZ_selectedYear", selectedYear);
            return titles;
        }


        private Dictionary<string, string> getOptionConditionFilter(IRptParams filter)
        {
            string ShouHuo = " ";
            string agent4Province = " "; // 非必选
            string saleGroup = " ";
            string countryCustomer = "";
            string cityCustomer = "";
            string customer = "";
            Dictionary<string, string> conditionMap = new Dictionary<string, string>();
            DynamicObject customFilter = filter.FilterParameter.CustomFilter;
            if(!int.Equals(0,Convert.ToInt32(customFilter["F_PAEZ_ShouHuo_Id"]))){
                ShouHuo = Convert.ToString(customFilter["F_PAEZ_ShouHuo_Id"]);
            }
            if(!int.Equals(0,Convert.ToInt32(customFilter["F_PAEZ_AgentProvince_Id"]))){
                agent4Province = Convert.ToString(customFilter["F_PAEZ_AgentProvince_Id"]);
            }
            if (!int.Equals(0, Convert.ToInt32(customFilter["F_PAEZ_saleGroup_Id"]))) {
                 saleGroup = Convert.ToString(customFilter["F_PAEZ_saleGroup_Id"]);//非必选
            }
            if (!int.Equals(0, Convert.ToInt32(customFilter["F_PAEZ_COUNTRYCUST_Id"])))
            {
                countryCustomer = Convert.ToString(customFilter["F_PAEZ_COUNTRYCUST_Id"]);//非必选
            }
            if (!int.Equals(0, Convert.ToInt32(customFilter["F_PAEZ_CITYCUST_Id"])))
            {
                cityCustomer = Convert.ToString(customFilter["F_PAEZ_CITYCUST_Id"]);//非必选
            }
            if (!int.Equals(0, Convert.ToInt32(customFilter["F_PAEZ_Customer_Id"])))
            {
                customer = Convert.ToString(customFilter["F_PAEZ_Customer_Id"]);
            }


            if (!string.IsNullOrWhiteSpace(agent4Province)) 
            {
                conditionMap.Add("F_PAEZ_agent4Province", agent4Province);
            }
            if (!string.IsNullOrWhiteSpace(ShouHuo))
            {
                conditionMap.Add("F_PAEZ_ShouHuo", ShouHuo);
            }
            if (!string.IsNullOrWhiteSpace(saleGroup))
            {
                conditionMap.Add("F_PAEZ_saleGroup", saleGroup);
            }
            if (!string.IsNullOrWhiteSpace(cityCustomer))
            {
                conditionMap.Add("F_PAEZ_CITYCUST", cityCustomer);
            }
            if (!string.IsNullOrWhiteSpace(countryCustomer))
            {
                conditionMap.Add("F_PAEZ_COUNTRYCUST", countryCustomer);
            }
            if (!string.IsNullOrWhiteSpace(customer))
            {
                conditionMap.Add("F_PAEZ_Customer", customer);
            }

            return conditionMap;
            
        }

        public override ReportHeader GetReportHeaders(IRptParams filter)
        {
            ReportHeader header = new ReportHeader();

            header.AddChild("PROVINCECUST", new LocaleValue("省级", this.Context.UserLocale.LCID));
            header.AddChild("CITYCUST", new LocaleValue("市级", this.Context.UserLocale.LCID));
            header.AddChild("COUNTRYCUST", new LocaleValue("县级", this.Context.UserLocale.LCID));
            header.AddChild("SHOUHUOCUST", new LocaleValue("收货人", this.Context.UserLocale.LCID));
            header.AddChild("SHXSZU", new LocaleValue("收货人销售组", this.Context.UserLocale.LCID));
            header.AddChild("JIAMENGRIQI", new LocaleValue("加盟日期", this.Context.UserLocale.LCID));
            header.AddChild("KEHU", new LocaleValue("客户", this.Context.UserLocale.LCID));


            header.AddChild("JANSALENO", new LocaleValue("1月销售数量", this.Context.UserLocale.LCID), SqlStorageType.SqlDecimal);
            header.AddChild("JANSALEAMT", new LocaleValue("1月销售金额", this.Context.UserLocale.LCID), SqlStorageType.SqlDecimal);
            header.AddChild("JANZENGPINNO", new LocaleValue("1月赠品数量", this.Context.UserLocale.LCID), SqlStorageType.SqlDecimal);
            header.AddChild("JANZENGPINAMT", new LocaleValue("1月赠品金额", this.Context.UserLocale.LCID), SqlStorageType.SqlDecimal);
            header.AddChild("JANSUMTOTAL", new LocaleValue("1月合计数量", this.Context.UserLocale.LCID), SqlStorageType.SqlDecimal);
            header.AddChild("JANSUMAMT", new LocaleValue("1月合计金额", this.Context.UserLocale.LCID), SqlStorageType.SqlDecimal);
            header.AddChild("JANNOSENDNO", new LocaleValue("1月未发数量", this.Context.UserLocale.LCID), SqlStorageType.SqlDecimal);
            header.AddChild("JANNOSENDAMT", new LocaleValue("1月未发金额", this.Context.UserLocale.LCID), SqlStorageType.SqlDecimal);

            header.AddChild("FEBSALENO", new LocaleValue("2月销售数量", this.Context.UserLocale.LCID), SqlStorageType.SqlDecimal);
            header.AddChild("FEBSALEAMT", new LocaleValue("2月销售金额", this.Context.UserLocale.LCID), SqlStorageType.SqlDecimal);
            header.AddChild("FEBZENGPINNO", new LocaleValue("2月赠品数量", this.Context.UserLocale.LCID), SqlStorageType.SqlDecimal);
            header.AddChild("FEBZENGPINAMT", new LocaleValue("2月赠品金额", this.Context.UserLocale.LCID), SqlStorageType.SqlDecimal);
            header.AddChild("FEBSUMTOTAL", new LocaleValue("2月合计数量", this.Context.UserLocale.LCID), SqlStorageType.SqlDecimal);
            header.AddChild("FEBSUMAMT", new LocaleValue("2月合计金额", this.Context.UserLocale.LCID), SqlStorageType.SqlDecimal);
            header.AddChild("FEBNOSENDNO", new LocaleValue("2月未发数量", this.Context.UserLocale.LCID), SqlStorageType.SqlDecimal);
            header.AddChild("FEBNOSENDAMT", new LocaleValue("2月未发金额", this.Context.UserLocale.LCID), SqlStorageType.SqlDecimal);

            header.AddChild("MARSALENO", new LocaleValue("3月销售数量", this.Context.UserLocale.LCID), SqlStorageType.SqlDecimal);
            header.AddChild("MARSALEAMT", new LocaleValue("3月销售金额", this.Context.UserLocale.LCID), SqlStorageType.SqlDecimal);
            header.AddChild("MARZENGPINNO", new LocaleValue("3月赠品数量", this.Context.UserLocale.LCID), SqlStorageType.SqlDecimal);
            header.AddChild("MARZENGPINAMT", new LocaleValue("3月赠品金额", this.Context.UserLocale.LCID), SqlStorageType.SqlDecimal);
            header.AddChild("MARSUMTOTAL", new LocaleValue("3月合计数量", this.Context.UserLocale.LCID), SqlStorageType.SqlDecimal);
            header.AddChild("MARSUMAMT", new LocaleValue("3月合计金额", this.Context.UserLocale.LCID), SqlStorageType.SqlDecimal);
            header.AddChild("MARNOSENDNO", new LocaleValue("3月未发数量", this.Context.UserLocale.LCID), SqlStorageType.SqlDecimal);
            header.AddChild("MARNOSENDAMT", new LocaleValue("3月未发金额", this.Context.UserLocale.LCID), SqlStorageType.SqlDecimal);

            header.AddChild("APRSALENO", new LocaleValue("4月销售数量", this.Context.UserLocale.LCID), SqlStorageType.SqlDecimal);
            header.AddChild("APRSALEAMT", new LocaleValue("4月销售金额", this.Context.UserLocale.LCID), SqlStorageType.SqlDecimal);
            header.AddChild("APRZENGPINNO", new LocaleValue("4月赠品数量", this.Context.UserLocale.LCID), SqlStorageType.SqlDecimal);
            header.AddChild("APRZENGPINAMT", new LocaleValue("4月赠品金额", this.Context.UserLocale.LCID), SqlStorageType.SqlDecimal);
            header.AddChild("APRSUMTOTAL", new LocaleValue("4月合计数量", this.Context.UserLocale.LCID), SqlStorageType.SqlDecimal);
            header.AddChild("APRSUMAMT", new LocaleValue("4月合计金额", this.Context.UserLocale.LCID), SqlStorageType.SqlDecimal);
            header.AddChild("APRNOSENDNO", new LocaleValue("4月未发数量", this.Context.UserLocale.LCID), SqlStorageType.SqlDecimal);
            header.AddChild("APRNOSENDAMT", new LocaleValue("4月未发金额", this.Context.UserLocale.LCID), SqlStorageType.SqlDecimal);

            header.AddChild("MAYSALENO", new LocaleValue("5月销售数量", this.Context.UserLocale.LCID), SqlStorageType.SqlDecimal);
            header.AddChild("MAYSALEAMT", new LocaleValue("5月销售金额", this.Context.UserLocale.LCID), SqlStorageType.SqlDecimal);
            header.AddChild("MAYZENGPINNO", new LocaleValue("5月赠品数量", this.Context.UserLocale.LCID), SqlStorageType.SqlDecimal);
            header.AddChild("MAYZENGPINAMT", new LocaleValue("5月赠品金额", this.Context.UserLocale.LCID), SqlStorageType.SqlDecimal);
            header.AddChild("MAYSUMTOTAL", new LocaleValue("5月合计数量", this.Context.UserLocale.LCID), SqlStorageType.SqlDecimal);
            header.AddChild("MAYSUMAMT", new LocaleValue("5月合计金额", this.Context.UserLocale.LCID), SqlStorageType.SqlDecimal);
            header.AddChild("MAYNOSENDNO", new LocaleValue("5月未发数量", this.Context.UserLocale.LCID), SqlStorageType.SqlDecimal);
            header.AddChild("MAYNOSENDAMT", new LocaleValue("5月未发金额", this.Context.UserLocale.LCID), SqlStorageType.SqlDecimal);

            header.AddChild("JUNSALENO", new LocaleValue("6月销售数量", this.Context.UserLocale.LCID), SqlStorageType.SqlDecimal);
            header.AddChild("JUNSALEAMT", new LocaleValue("6月销售金额", this.Context.UserLocale.LCID), SqlStorageType.SqlDecimal);
            header.AddChild("JUNZENGPINNO", new LocaleValue("6月赠品数量", this.Context.UserLocale.LCID), SqlStorageType.SqlDecimal);
            header.AddChild("JUNZENGPINAMT", new LocaleValue("6月赠品金额", this.Context.UserLocale.LCID), SqlStorageType.SqlDecimal);
            header.AddChild("JUNSUMTOTAL", new LocaleValue("6月合计数量", this.Context.UserLocale.LCID), SqlStorageType.SqlDecimal);
            header.AddChild("JUNSUMAMT", new LocaleValue("6月合计金额", this.Context.UserLocale.LCID), SqlStorageType.SqlDecimal);
            header.AddChild("JUNNOSENDNO", new LocaleValue("6月未发数量", this.Context.UserLocale.LCID), SqlStorageType.SqlDecimal);
            header.AddChild("JUNNOSENDAMT", new LocaleValue("6月未发金额", this.Context.UserLocale.LCID), SqlStorageType.SqlDecimal);

            header.AddChild("JULSALENO", new LocaleValue("7月销售数量", this.Context.UserLocale.LCID), SqlStorageType.SqlDecimal);
            header.AddChild("JULSALEAMT", new LocaleValue("7月销售金额", this.Context.UserLocale.LCID), SqlStorageType.SqlDecimal);
            header.AddChild("JULZENGPINNO", new LocaleValue("7月赠品数量", this.Context.UserLocale.LCID), SqlStorageType.SqlDecimal);
            header.AddChild("JULZENGPINAMT", new LocaleValue("7月赠品金额", this.Context.UserLocale.LCID), SqlStorageType.SqlDecimal);
            header.AddChild("JULSUMTOTAL", new LocaleValue("7月合计数量", this.Context.UserLocale.LCID), SqlStorageType.SqlDecimal);
            header.AddChild("JULSUMAMT", new LocaleValue("7月合计金额", this.Context.UserLocale.LCID), SqlStorageType.SqlDecimal);
            header.AddChild("JULNOSENDNO", new LocaleValue("7月未发数量", this.Context.UserLocale.LCID), SqlStorageType.SqlDecimal);
            header.AddChild("JULNOSENDAMT", new LocaleValue("7月未发金额", this.Context.UserLocale.LCID), SqlStorageType.SqlDecimal);

            header.AddChild("AUGSALENO", new LocaleValue("8月销售数量", this.Context.UserLocale.LCID), SqlStorageType.SqlDecimal);
            header.AddChild("AUGSALEAMT", new LocaleValue("8月销售金额", this.Context.UserLocale.LCID), SqlStorageType.SqlDecimal);
            header.AddChild("AUGZENGPINNO", new LocaleValue("8月赠品数量", this.Context.UserLocale.LCID), SqlStorageType.SqlDecimal);
            header.AddChild("AUGZENGPINAMT", new LocaleValue("8月赠品金额", this.Context.UserLocale.LCID), SqlStorageType.SqlDecimal);
            header.AddChild("AUGSUMTOTAL", new LocaleValue("8月合计数量", this.Context.UserLocale.LCID), SqlStorageType.SqlDecimal);
            header.AddChild("AUGSUMAMT", new LocaleValue("8月合计金额", this.Context.UserLocale.LCID), SqlStorageType.SqlDecimal);
            header.AddChild("AUGNOSENDNO", new LocaleValue("8月未发数量", this.Context.UserLocale.LCID), SqlStorageType.SqlDecimal);
            header.AddChild("AUGNOSENDAMT", new LocaleValue("8月未发金额", this.Context.UserLocale.LCID), SqlStorageType.SqlDecimal);

            header.AddChild("SEPSALENO", new LocaleValue("9月销售数量", this.Context.UserLocale.LCID), SqlStorageType.SqlDecimal);
            header.AddChild("SEPSALEAMT", new LocaleValue("9月销售金额", this.Context.UserLocale.LCID), SqlStorageType.SqlDecimal);
            header.AddChild("SEPZENGPINNO", new LocaleValue("9月赠品数量", this.Context.UserLocale.LCID), SqlStorageType.SqlDecimal);
            header.AddChild("SEPZENGPINAMT", new LocaleValue("9月赠品金额", this.Context.UserLocale.LCID), SqlStorageType.SqlDecimal);
            header.AddChild("SEPSUMTOTAL", new LocaleValue("9月合计数量", this.Context.UserLocale.LCID), SqlStorageType.SqlDecimal);
            header.AddChild("SEPSUMAMT", new LocaleValue("9月合计金额", this.Context.UserLocale.LCID), SqlStorageType.SqlDecimal);
            header.AddChild("SEPNOSENDNO", new LocaleValue("9月未发数量", this.Context.UserLocale.LCID), SqlStorageType.SqlDecimal);
            header.AddChild("SEPNOSENDAMT", new LocaleValue("9月未发金额", this.Context.UserLocale.LCID), SqlStorageType.SqlDecimal);

            header.AddChild("OCTSALENO", new LocaleValue("10月销售数量", this.Context.UserLocale.LCID), SqlStorageType.SqlDecimal);
            header.AddChild("OCTSALEAMT", new LocaleValue("10月销售金额", this.Context.UserLocale.LCID), SqlStorageType.SqlDecimal);
            header.AddChild("OCTZENGPINNO", new LocaleValue("10月赠品数量", this.Context.UserLocale.LCID), SqlStorageType.SqlDecimal);
            header.AddChild("OCTZENGPINAMT", new LocaleValue("10月赠品金额", this.Context.UserLocale.LCID), SqlStorageType.SqlDecimal);
            header.AddChild("OCTSUMTOTAL", new LocaleValue("10月合计数量", this.Context.UserLocale.LCID), SqlStorageType.SqlDecimal);
            header.AddChild("OCTSUMAMT", new LocaleValue("10月合计金额", this.Context.UserLocale.LCID), SqlStorageType.SqlDecimal);
            header.AddChild("OCTNOSENDNO", new LocaleValue("10月未发数量", this.Context.UserLocale.LCID), SqlStorageType.SqlDecimal);
            header.AddChild("OCTNOSENDAMT", new LocaleValue("10月未发金额", this.Context.UserLocale.LCID), SqlStorageType.SqlDecimal);

            header.AddChild("NOVSALENO", new LocaleValue("11月销售数量", this.Context.UserLocale.LCID), SqlStorageType.SqlDecimal);
            header.AddChild("NOVSALEAMT", new LocaleValue("11月销售金额", this.Context.UserLocale.LCID), SqlStorageType.SqlDecimal);
            header.AddChild("NOVZENGPINNO", new LocaleValue("11月赠品数量", this.Context.UserLocale.LCID), SqlStorageType.SqlDecimal);
            header.AddChild("NOVZENGPINAMT", new LocaleValue("11月赠品金额", this.Context.UserLocale.LCID), SqlStorageType.SqlDecimal);
            header.AddChild("NOVSUMTOTAL", new LocaleValue("11月合计数量", this.Context.UserLocale.LCID), SqlStorageType.SqlDecimal);
            header.AddChild("NOVSUMAMT", new LocaleValue("11月合计金额", this.Context.UserLocale.LCID), SqlStorageType.SqlDecimal);
            header.AddChild("NOVNOSENDNO", new LocaleValue("11月未发数量", this.Context.UserLocale.LCID), SqlStorageType.SqlDecimal);
            header.AddChild("NOVNOSENDAMT", new LocaleValue("11月未发金额", this.Context.UserLocale.LCID), SqlStorageType.SqlDecimal);

            header.AddChild("DECSALENO", new LocaleValue("12月销售数量", this.Context.UserLocale.LCID), SqlStorageType.SqlDecimal);
            header.AddChild("DECSALEAMT", new LocaleValue("12月销售金额", this.Context.UserLocale.LCID), SqlStorageType.SqlDecimal);
            header.AddChild("DECZENGPINNO", new LocaleValue("12月赠品数量", this.Context.UserLocale.LCID), SqlStorageType.SqlDecimal);
            header.AddChild("DECZENGPINAMT", new LocaleValue("12月赠品金额", this.Context.UserLocale.LCID), SqlStorageType.SqlDecimal);
            header.AddChild("DECSUMTOTAL", new LocaleValue("12月合计数量", this.Context.UserLocale.LCID), SqlStorageType.SqlDecimal);
            header.AddChild("DECSUMAMT", new LocaleValue("12月合计金额", this.Context.UserLocale.LCID), SqlStorageType.SqlDecimal);
            header.AddChild("DECNOSENDNO", new LocaleValue("12月未发数量", this.Context.UserLocale.LCID), SqlStorageType.SqlDecimal);
            header.AddChild("DECNOSENDAMT", new LocaleValue("12月未发金额", this.Context.UserLocale.LCID), SqlStorageType.SqlDecimal);

            header.AddChild("ALLYEARSALENO", new LocaleValue("全年销售总数量", this.Context.UserLocale.LCID), SqlStorageType.SqlDecimal);
            header.AddChild("ALLYEARSALEAMT", new LocaleValue("全年销售总金额", this.Context.UserLocale.LCID), SqlStorageType.SqlDecimal);
            header.AddChild("ALLYEARZENGPINNO", new LocaleValue("全年促销总数量", this.Context.UserLocale.LCID), SqlStorageType.SqlDecimal);
            header.AddChild("ALLYEARZENGPINAMT", new LocaleValue("全年促销总金额", this.Context.UserLocale.LCID), SqlStorageType.SqlDecimal);
            header.AddChild("ALLYEARSUMTOTAL", new LocaleValue("全年合计总数量", this.Context.UserLocale.LCID), SqlStorageType.SqlDecimal);
            header.AddChild("ALLYEARSUMAMT", new LocaleValue("全年合计总金额", this.Context.UserLocale.LCID), SqlStorageType.SqlDecimal);
            header.AddChild("ALLYEARNOSENDNO", new LocaleValue("全年未发总数量", this.Context.UserLocale.LCID), SqlStorageType.SqlDecimal);
            header.AddChild("ALLYEARNOSENDAMT", new LocaleValue("全年未发总金额", this.Context.UserLocale.LCID), SqlStorageType.SqlDecimal);


            header.AddChild("MOBILEPHONE", new LocaleValue("地址", this.Context.UserLocale.LCID));
            header.AddChild("ADDRESS", new LocaleValue("电话", this.Context.UserLocale.LCID));

            return header;
        }

    }
}
