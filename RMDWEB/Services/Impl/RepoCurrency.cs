using RMDWEB.Data;
using RMDWEB.Models;
using RMDWEB.Services.Interface;

namespace RMDWEB.Services.Impl
{
    public class RepoCurrency:InterfaceCurrency
    {
        private ApplicationDbContext dbconn = new ApplicationDbContext();

        List<CurrencyTbl> InterfaceCurrency.AllCurrency()
        {
            return dbconn.CurrencyTbl.ToList();
        }

        CurrencyTbl InterfaceCurrency.singelCurrency(int id)
        {
            return dbconn.CurrencyTbl.Single(a => a.CurrencyId == id);
        }
    }
}
