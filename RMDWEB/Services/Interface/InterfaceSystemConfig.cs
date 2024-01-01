using RMDWEB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMDWEB.Interface
{
    public interface InterfaceSystemConfig
    {
        List<StatusTbl> AllStatus();
        List<BankTbl> AllBanks();

        List<CurrencyTbl> AllCurrency();
        List<DepartmentTbl> AllDepartments();
        CurrencyTbl singelCurrency(int id);
        StatusTbl singelStatus(int id);

        DepartmentTbl singleDepartment(int id);

        BankTbl changeBank(BankTbl data);
        DepartmentTbl changeDepartment(DepartmentTbl data);

 
    }
}
