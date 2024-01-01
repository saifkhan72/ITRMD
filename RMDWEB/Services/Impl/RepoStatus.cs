using RMDWEB.Data;
using RMDWEB.Models;
using RMDWEB.Services.Interface;

namespace RMDWEB.Services.Impl
{

    public class RepoStatus : InterfaceStatus
    {
        private ApplicationDbContext dbconn = new ApplicationDbContext();
        List<StatusTbl> InterfaceStatus.AllStatus()
        {
            return dbconn.StatusTbl.ToList();
        }

        StatusTbl InterfaceStatus.singelStatus(int id)
        {
            return dbconn.StatusTbl.Single(a => a.StatusId == id);
        }
    }
}
