using RMDWEB.Models;

namespace RMDWEB.Services.Interface
{
    public interface InterfaceBank
    {
        BankTbl single(int id);
        Boolean delete(BankTbl bank);
        List<BankTbl> AllBanks();
        BankTbl changeBank(BankTbl data);
        List<StatusTbl> AllStatus();
    }
}
