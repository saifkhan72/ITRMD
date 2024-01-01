using RMDWEB.Data;
using RMDWEB.Models;

namespace RMDWEB.Services.Interface
{
    public interface InterfaceStatus
    {
        List<StatusTbl> AllStatus();
        StatusTbl singelStatus(int id);

    }
}
