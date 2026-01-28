namespace Dierentuin.Models
{
    using Microsoft.EntityFrameworkCore;
    using System.ComponentModel.DataAnnotations;

    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public required string Name { get; set; }

        // Relationship with Animals
        public List<Animal> Animals { get; set; } = new List<Animal>();
    }




}



