using Microsoft.EntityFrameworkCore;
using RMDWEB.Data;
using RMDWEB.Models;
using RMDWEB.Services.Interface;

namespace RMDWEB.Services.Impl
{
    public class RepoDepartment : InterfaceDepartment
    {
        private ApplicationDbContext dbconn = new ApplicationDbContext();

        List<DepartmentTbl> InterfaceDepartment.AllDepartments()
        {
            return dbconn.DepartmentTbl.ToList();
        }

        List<StatusTbl> InterfaceDepartment.AllStatus()
        {
            return dbconn.StatusTbl.ToList();
        }

        DepartmentTbl InterfaceDepartment.changeDepartment(DepartmentTbl data)
        {
            if (data.DepartmentId == 0)
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

        bool InterfaceDepartment.Delete(DepartmentTbl department)
        {
            if(department != null)
            {
                dbconn.Remove(department);
                dbconn.SaveChanges();
            }
            else
            {
                return false;
            }
            return true;
        }

        DepartmentTbl InterfaceDepartment.single(int id)
        {
            var department =  dbconn.DepartmentTbl.SingleOrDefault(a => a.DepartmentId == id);
            return department;
        }

        DepartmentTbl InterfaceDepartment.singleDepartment(int id)
        {
            return dbconn.DepartmentTbl.Single(a => a.DepartmentId == id);
        }
    }
}
