using RMDWEB.Data;
using RMDWEB.Models;
using RMDWEB.Services.Interface;

namespace RMDWEB.Services.Impl
{
    public class RepoActivityLog : InterfaceActivityLog
    {
        private ApplicationDbContext dbconn = new ApplicationDbContext();

        void InterfaceActivityLog.Add(ActivityLog data)
        {
            ActivityLog row = new()
            {
                Activity = data.Activity,
                ActivityDate = data.ActivityDate,
                TableName = data.TableName,
                UserName= data.UserName,
                Detail= data.Detail
            };

            dbconn.Entry(row).State = Microsoft.EntityFrameworkCore.EntityState.Added;
            dbconn.SaveChanges();
        }

        List<ActivityLog> InterfaceActivityLog.AllLog()
        {
            return dbconn.ActivityLog.ToList();
        }
    }
}
