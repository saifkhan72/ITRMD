using RMDWEB.Models;

namespace RMDWEB.Services.Interface
{
    public interface InterfaceCurrency
    {
        List<CurrencyTbl> AllCurrency();
        CurrencyTbl singelCurrency(int id);

    }
}
