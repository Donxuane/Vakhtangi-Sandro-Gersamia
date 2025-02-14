using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetingExpense.Domain.Models
{
   public class User
    {
        [Key]
        public int Id { get; set; }
        [Length(2,50)]
        [Required]
        public string UserName { get; set; }
        [Length(2,50)]
        [Required]
        public string UserSurname { get; set; }
        [EmailAddress]
        [Required]
        public string Email { get; set; }
        [Length(8,255)]
        [Required]
        public string Password { get; set; }
        [Required]
        public DateTime RegisterDate { get; set; }
    
        public bool? Notifications { get; set; }


    }
}
