using Microsoft.EntityFrameworkCore;
using RMDWEB.Data;
using RMDWEB.Interface;
using RMDWEB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMDWEB.Impl
{
    public class RepoSystemConfig : InterfaceSystemConfig
    {

        private ApplicationDbContext dbconn = new ApplicationDbContext();


        List<StatusTbl> InterfaceSystemConfig.AllStatus()
        {
            return dbconn.StatusTbl.ToList();
        }

        List<BankTbl> InterfaceSystemConfig.AllBanks()
        {
            return dbconn.BankTbl.ToList();
        }

        List<DepartmentTbl> InterfaceSystemConfig.AllDepartments()
        {
            return dbconn.DepartmentTbl.ToList();
        }

        List<CurrencyTbl> InterfaceSystemConfig.AllCurrency()
        {
            return dbconn.CurrencyTbl.ToList();
        }

        CurrencyTbl InterfaceSystemConfig.singelCurrency(int id)
        {
            return dbconn.CurrencyTbl.Single(a => a.CurrencyId==id);
        }


        DepartmentTbl InterfaceSystemConfig.singleDepartment(int id)
        {
            return dbconn.DepartmentTbl.Single(a => a.DepartmentId==id);
        }


        StatusTbl InterfaceSystemConfig.singelStatus(int id)
        {
            return dbconn.StatusTbl.Single(a => a.StatusId==id);
        }

        BankTbl InterfaceSystemConfig.changeBank(BankTbl data)
        {
            if(data.BankId ==0)
            {
                dbconn.Entry(data).State=EntityState.Added;
            }
            else
            {
                dbconn.Entry(data).State=EntityState.Modified;
            }
            dbconn.SaveChanges();
            return data;
        }

        DepartmentTbl InterfaceSystemConfig.changeDepartment(DepartmentTbl data)
        {
            if(data.DepartmentId==0)
            {
                dbconn.Entry(data).State=EntityState.Added;
            }
            else
            {
                dbconn.Entry(data).State=EntityState.Modified;
            }
            dbconn.SaveChanges();
            return data;
        }

    }
}
