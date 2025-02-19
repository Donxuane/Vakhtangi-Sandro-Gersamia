using System.ComponentModel.DataAnnotations;

    public class ExpenseDetails
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public decimal ExpenseAmount { get; set; }
        [Required]
        public DateOnly ExpenseDate { get; set; }

    }
