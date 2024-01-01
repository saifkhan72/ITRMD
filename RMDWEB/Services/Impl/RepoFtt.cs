using Microsoft.EntityFrameworkCore;
using RMDWEB.Data;
using RMDWEB.Interface;
using RMDWEB.Models;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace RMDWEB.Impl
{
    public class RepoFtt : InterfaceFtt
    {
        private ApplicationDbContext dbconn = new ApplicationDbContext();

        List<FTTDocumentfile> InterfaceFtt.AllDocumentFileByFttId(int TID)
        {
            return dbconn.FTTDocumentfile.Include(s => s.StatusTbl).Where(a => a.TID==TID).ToList();
        }

        List<FTTTransaction> InterfaceFtt.AllFttTransaction()
        {
            return dbconn.FTTTransaction.Include(s=>s.StatusTbl).Include(c=>c.CurrencyTbl).ToList();
        }

        List<FTTTransaction> InterfaceFtt.AllFttTransactionByBank(int BankId)
        {
             return dbconn.FTTTransaction.Include(s=>s.StatusTbl).Include(c=>c.CurrencyTbl).Where(a => a.BankId==BankId).ToList();
        }

        List<FttTransactionLog> InterfaceFtt.AllFttTransactionLog()
        {
            return dbconn.FttTransactionLog.ToList();
        }

        List<FttTransactionLog> InterfaceFtt.AllFttTransactionLogByBank(int DepartmentId)
        {

            return dbconn.FttTransactionLog.Where(a => a.DepartmentId==DepartmentId).ToList();
        }

        FTTTransaction InterfaceFtt.Change(FTTTransaction tbl)
        {

            FTTTransaction t = new FTTTransaction();

            if(tbl.TID!=0)
            {
                t=dbconn.FTTTransaction.Single(a => a.TID==tbl.TID);
                if(t.UserId!=tbl.UserId)
                {
                    return t;
                }
            }
            t.BankLetterId=tbl.BankLetterId;
            t.TTDate=tbl.TTDate;
            t.TTNumber=tbl.TTNumber;
            t.TTAmount=tbl.TTAmount;
            t.StatusId=tbl.StatusId;
            t.UserId=tbl.UserId;
            t.Created=DateTime.Now;
            t.Updated=DateTime.Now;
            t.CreatedBy=tbl.CreatedBy;
            t.UpdatedBy=tbl.UpdatedBy;
            t.InvoiceContractNo=tbl.InvoiceContractNo;
            t.SenderName=tbl.SenderName;
            t.BenBank=tbl.BenBank;
            t.PurposeTransaction=tbl.PurposeTransaction;
            t.BenCountry=tbl.BenCountry;
            t.BenCompany=tbl.BenCompany;
            t.CurrencyId=tbl.CurrencyId;
            t.Comment=tbl.Comment;
            t.DepartmentId=tbl.DepartmentId;
            t.BankId=tbl.BankId;
            if(tbl.TID!=0)
            {
                dbconn.Entry(t).State=Microsoft.EntityFrameworkCore.EntityState.Modified;
            }
            else
            {
                dbconn.Entry(t).State=Microsoft.EntityFrameworkCore.EntityState.Added;
            }
            dbconn.SaveChanges();

            FttTransactionLog t2 = new FttTransactionLog();
            t2.TID=t.TID;
            t2.BankLetterId=t.BankLetterId;
            t2.TTDate=t.TTDate;
            t2.TTNumber=t.TTNumber;
            t2.TTAmount=t.TTAmount;
            t2.StatusId=t.StatusId;
            t2.UserId=t.UserId;
            t2.Created=DateTime.Now;
            t2.Updated=DateTime.Now;
            t2.CreatedBy=t.CreatedBy.Value;
            t2.UpdatedBy=t.UpdatedBy.Value;
            t2.InvoiceContractNo=t.InvoiceContractNo;
            t2.SenderName=t.SenderName;
            t2.BenBank=t.BenBank;
            t2.PurposeTransaction=t.PurposeTransaction;
            t2.BenCountry=t.BenCountry;
            t2.CurrencyId=t.CurrencyId;
            t2.BenCompany=t.BenCompany;
            t2.Comment=t.Comment;
            t2.DepartmentId=t.DepartmentId;
            dbconn.Entry(t2).State=Microsoft.EntityFrameworkCore.EntityState.Added;
            dbconn.SaveChanges();
            return t;




        }

        FTTTransaction InterfaceFtt.singelFttTransaction(int id)
        {
            return dbconn.FTTTransaction.Include(s=>s.StatusTbl).SingleOrDefault(a => a.TID==id);
        }

        FTTTransaction InterfaceFtt.singelFttTransactionByBankId(int id, int BankId)
        {
            return dbconn.FTTTransaction.SingleOrDefault(a => a.BankId != null && a.BankId==BankId&&a.TID==id);
        }


        FTTDocumentfile InterfaceFtt.singelFttDocument(int id)
        {
            return dbconn.FTTDocumentfile.SingleOrDefault(a => a.DocId==id);
        }


        int InterfaceFtt.DeleteFttDocument(FTTDocumentfile dl, int Status)
        {

            if(Status==1)
            {
                if(dl.StatusTbl.Name =="New")
                {
                    dbconn.Entry(dl).State=Microsoft.EntityFrameworkCore.EntityState.Deleted;
                    dbconn.SaveChanges();
                }
            }
            else if(Status==2)
            {
                dl.StatusId=dbconn.StatusTbl.Single(a => a.Name=="Active").StatusId;
                dbconn.Entry(dl).State=Microsoft.EntityFrameworkCore.EntityState.Modified;
                dbconn.SaveChanges();

            }

            return 1;
        }


        FTTDocumentfile InterfaceFtt.changeFttDocumentFile(FTTDocumentfile data)
        {
            FTTDocumentfile d = new FTTDocumentfile();
            if(data.DocId!=0)
            {
                d=dbconn.FTTDocumentfile.Single(a => a.DocId==data.DocId);
            }
            d.Title=data.Title;
            d.Created=DateTime.Now;
            d.Updated=DateTime.Now;
            d.CreatedBy=data.CreatedBy;
            d.UpdatedBy=data.UpdatedBy;
            d.StatusId=data.StatusId;
            d.TID=data.TID;
            d.DocImage=data.DocImage;
            d.DocImagePath=data.DocImagePath;
            if(data.DocId==0)
            {
                dbconn.Entry(d).State=Microsoft.EntityFrameworkCore.EntityState.Added;
            }
            else
            {
                dbconn.Entry(d).State=Microsoft.EntityFrameworkCore.EntityState.Modified;
            }
            dbconn.SaveChanges();

            FTTDocumentfileLog dl = new FTTDocumentfileLog();
            dl.DocId=d.DocId;
            dl.TID=d.TID;
            dl.Title=d.Title;
            dl.Created=DateTime.Now;
            dl.Updated=DateTime.Now;
            dl.CreatedBy=d.CreatedBy;
            dl.UpdatedBy=d.UpdatedBy;
            dl.StatusId=d.StatusId;
            dl.DocImage=d.DocImage;
            dl.DocImagePath=d.DocImagePath;
            dbconn.Entry(dl).State=Microsoft.EntityFrameworkCore.EntityState.Added;
            dbconn.SaveChanges();
            return d;


        }

        FTTTransaction InterfaceFtt.changeFttStatus(FTTTransaction data,string ? userId)
        {


            data.StatusTbl = dbconn.StatusTbl.Single(a => a.StatusId == data.StatusId);
            data.Updated=DateTime.Now;
            data.UpdatedBy=data.UpdatedBy;
            data.StatusId=data.StatusId;
            dbconn.Entry(data).State=Microsoft.EntityFrameworkCore.EntityState.Modified;
            dbconn.SaveChanges();


            FttTransactionLog t2 = new FttTransactionLog();
            t2.TID=data.TID;
            t2.BankLetterId=data.BankLetterId;
            t2.TTDate=data.TTDate;
            t2.TTNumber=data.TTNumber;
            t2.TTAmount=data.TTAmount;
            t2.StatusId=data.StatusId;
            t2.UserId=data.UserId;
            t2.Created=data.Created.Value;
            t2.Updated=DateTime.Now;
            t2.CreatedBy=data.CreatedBy.Value;
            t2.UpdatedBy=data.UpdatedBy;
            t2.InvoiceContractNo=data.InvoiceContractNo;
            t2.SenderName=data.SenderName;
            t2.BenBank=data.BenBank;
            t2.PurposeTransaction=data.PurposeTransaction;
            t2.BenCountry=data.BenCountry;
            t2.BenCompany=data.BenCompany;
            t2.Comment=data.Comment;
            t2.CurrencyId=data.CurrencyId;
            dbconn.Entry(t2).State=Microsoft.EntityFrameworkCore.EntityState.Added;

            FTTComment cmt = new FTTComment();
            cmt.TID=data.TID;
            cmt.StatusId=data.StatusId;
            cmt.comment=data.StatusTbl.Name;
            cmt.Created=DateTime.Now;
            cmt.UserId=userId;
            dbconn.Entry(cmt).State=Microsoft.EntityFrameworkCore.EntityState.Added;

            dbconn.SaveChanges();

            return data;
        }

        FTTComment InterfaceFtt.changeComment(FTTComment data)
        {
            FTTComment c = new FTTComment();
            if(data.fttcommentid!=0)
            {
                data=dbconn.FTTComment.Single(a => a.fttcommentid==data.fttcommentid);
            }
            c.UserId=data.UserId;
            c.Created=DateTime.Now;
            c.comment=data.comment;
            c.TID=data.TID;
            c.StatusId=data.StatusId;
            if(data.fttcommentid!=0)
            {
                dbconn.Entry(c).State=Microsoft.EntityFrameworkCore.EntityState.Modified;
            }
            else
            {
                dbconn.Entry(c).State=Microsoft.EntityFrameworkCore.EntityState.Added;
            }
            dbconn.SaveChanges();
            return c;



        }

        List<FTTComment> InterfaceFtt.AllFttComment(int TID)
        {
            return dbconn.FTTComment.Where(a => a.TID==TID).Include(x=>x.StatusTbl).ToList();
        }

        List<FTTDocumentfile> InterfaceFtt.AllFttDocument(int TID)
        {
            return dbconn.FTTDocumentfile.Include(s=>s.StatusTbl).Where(a => a.TID==TID).ToList();
        }
    }
}
