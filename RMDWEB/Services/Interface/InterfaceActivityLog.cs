using RMDWEB.Models;

namespace RMDWEB.Services.Interface
{
    public interface InterfaceActivityLog
    {
        List<ActivityLog> AllLog();

        void Add(ActivityLog data);
    }
}
