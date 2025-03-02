using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dto.Account
{
    public class AccountDto
    {
        public int Id { get; set; }
        public int SubLedgerId { get; set; }
        public string AccountNumber { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string ContactInfo { get; set; }
        public string Email { get; set; }
        public string PANNumber { get; set; }
        public string BackUpContact { get; set; }
        public decimal? CreditLimit { get; set; }
        public DateTime? CreditDays { get; set; }
        public int CompanyInfoId { get; set; }
        public bool IsTaxable { get; set; }
        public int UnitId { get; set; }
    }
}
