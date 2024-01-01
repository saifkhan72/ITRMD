using RMDWEB.Models;

namespace RMDWEB.Services.Interface
{
    public interface InterfaceDepartment
    {
        List<DepartmentTbl> AllDepartments();
        DepartmentTbl singleDepartment(int id);
        DepartmentTbl changeDepartment(DepartmentTbl data);
        DepartmentTbl single(int id);
        Boolean Delete(DepartmentTbl department);
        List<StatusTbl> AllStatus();


    }
}
