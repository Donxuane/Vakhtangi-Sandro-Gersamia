using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

    public class ExpenseLimits
    {
     
        [Key]
        public int Id { get; set; }
        [Required]
        public int UserId { get; set; }
        [Required]
        public int LimitPeriod { get; set; }
        [Required]
        public string LimitExpenseType { get; set;}
        [Required]
        public decimal LimitAmount { get; set;}
}
