using Microsoft.EntityFrameworkCore;
using RMDWEB.Models;
using System.ComponentModel.DataAnnotations;

namespace RMDWEB.Models
{

    public class FttDetails
    {
        public FTTTransaction FTTTransaction { get; set; }
        public IEnumerable<FTTDocumentfile> FTTDocumentfile { get; set; }
    }

    public class FttStatusView
    {
        public int StatusId { get; set; }
        public string Name { get; set; }
    }
    public class postFileUploadView
    {
        public IFormFile? file{ get; set; }
        public int? TID{ get; set; }
        public string? Title{ get; set; }
    }
    public class postCommentView
    {
        public int TID { get; set; }
        public string Comment { get; set; }
        public int StatusId { get; set; }
        public int fttcommentid { get; set; }
    }

    public class FttCommentView
    {
        public long fttcommentid { get; set; }
        public string comment { get; set; }
        public string Created { get; set; }
        public string UserId { get; set; }
        public String StatusId { get; set; }
        public long TID { get; set; }
    }

    public class FttDocumentView
    {
        public long id { get; set; }
        public string title { get; set; }
        public string path { get; set; }
        public string StatusId { get; set; }
    }

    public class PaginatedList<T> : List<T>
    {
        public int PageIndex { get; private set; }
        public int TotalPages { get; private set; }

        public PaginatedList(List<T> items, int count, int pageIndex, int pageSize)
        {
            PageIndex=pageIndex;
            TotalPages=(int)Math.Ceiling(count/(double)pageSize);

            this.AddRange(items);
        }

        public bool HasPreviousPage => PageIndex>1;

        public bool HasNextPage => PageIndex<TotalPages;

        public static async Task<PaginatedList<T>> CreateAsync(IQueryable<T> source, int pageIndex, int pageSize)
        {
            var count = await source.CountAsync();
            var items = await source.Skip((pageIndex-1)*pageSize).Take(pageSize).ToListAsync();
            return new PaginatedList<T>(items, count, pageIndex, pageSize);
        }

    }

    public class ManageUserRolesViewModel
    {
        public string RoleId { get; set; }
        public string RoleName { get; set; }
        public bool Selected { get; set; }
    }
    public class ApplicationUserView
    {
        public string UserName { get; set; }
    }


    public class getSelectItemsView
    {
        public int id { get; set; }
        public string name { get; set; }
    }



    public class FttTransactionView
    {
        public int TID { get; set; }
        public string Comment { get; set; }
        [Required]
        public int BankLetterId { get; set; }

        public DateTime TTDate { get; set; }
        public int TTNumber { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "{0:0.00}", ApplyFormatInEditMode = true)]
        public float TTAmount { get; set; }
        [Required]
        public string SenderName { get; set; }
        [Required]
        public string BenBank { get; set; }
        [Required]
        public string PurposeTransaction { get; set; }
        [Required]
        public string BenCountry { get; set; }
        [Required]
        public string BenCompany { get; set; }

        [Required]
        public int CurrencyId { get; set; }

        [Required]
        public string InvoiceContractNo { get; set; }

    }


}
