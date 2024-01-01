using Microsoft.EntityFrameworkCore;
using RMDWEB.Data;
using RMDWEB.Models;
using RMDWEB.Services.Interface;

namespace RMDWEB.Services.Impl
{
    public class RepoBank : InterfaceBank
    {
        private ApplicationDbContext dbconn = new ApplicationDbContext();

        List<BankTbl> InterfaceBank.AllBanks()
        {
            return dbconn.BankTbl.ToList();
        }

        List<StatusTbl> InterfaceBank.AllStatus()
        {
            return dbconn.StatusTbl.ToList();
        }

        BankTbl InterfaceBank.changeBank(BankTbl data)
        {
            if (data.BankId == 0)
            {
                dbconn.Entry(data).State = EntityState.Added;
            }
            else
            {
                dbconn.Entry(data).State = EntityState.Modified;
            }
            dbconn.SaveChanges();
            return data;
        }

        bool InterfaceBank.delete(BankTbl bank)
        {
            if (bank != null)
            {
                try
                {
                    dbconn.Entry(bank).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
                    dbconn.SaveChanges();
                    return true;
                }
                catch
                {
                    return false; 
                }
               
            }
            return false;
        }


        BankTbl InterfaceBank.single(int id)
        {
            var bank = dbconn.BankTbl.SingleOrDefault(a => a.BankId == id);
            return bank;
        }


    }
}
