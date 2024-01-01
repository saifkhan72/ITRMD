using RMDWEB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMDWEB.Interface
{
    public interface InterfaceFtt
    {
        List<FTTTransaction> AllFttTransaction();
        List<FttTransactionLog> AllFttTransactionLog();
        List<FTTTransaction> AllFttTransactionByBank(int DepartmentId);
        List<FttTransactionLog> AllFttTransactionLogByBank(int DepartmentId);
        FTTTransaction singelFttTransaction(int id);
        FTTTransaction Change(FTTTransaction data);
        List<FTTDocumentfile> AllDocumentFileByFttId(int TID);
        FTTDocumentfile singelFttDocument(int id);
        FTTTransaction singelFttTransactionByBankId(int id, int BankId);
        int DeleteFttDocument(FTTDocumentfile data, int Status);
        FTTDocumentfile changeFttDocumentFile(FTTDocumentfile data);
        FTTTransaction changeFttStatus(FTTTransaction data,string ?userid);

        FTTComment changeComment(FTTComment data);

        List<FTTComment> AllFttComment(int TID);
        List<FTTDocumentfile> AllFttDocument(int TID);


    }
}
