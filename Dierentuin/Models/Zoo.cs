namespace Dierentuin.Models
{
    using Microsoft.EntityFrameworkCore;
    using System.ComponentModel.DataAnnotations;
    using System.Collections.Generic;

    public class Zoo
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public required string Name { get; set; }

        // Relationship with Enclosures
        public List<Enclosure> Enclosures { get; set; } = new List<Enclosure>();

        // Method to get sunrise actions for all animals in all enclosures
        public List<string> ActieSunrise()
        {
            var actions = new List<string>();
            foreach (var enclosure in Enclosures)
            {
                actions.AddRange(enclosure.ActieSunrise());
            }
            return actions;
        }

        // Method to get sunset actions for all animals in all enclosures
        public List<string> ActieSunset()
        {
            var actions = new List<string>();
            foreach (var enclosure in Enclosures)
            {
                actions.AddRange(enclosure.ActieSunset());
            }
            return actions;
        }

        // Method to get feeding time actions for all animals in all enclosures
        public List<string> ActieFeedingTime()
        {
            var actions = new List<string>();
            foreach (var enclosure in Enclosures)
            {
                actions.AddRange(enclosure.ActieFeedingTime());
            }
            return actions;
        }

        // Method to check if all constraints are satisfied for this zoo
        public List<string> CheckConstraints()
        {
            var constraints = new List<string>();
            var allValid = true;

            foreach (var enclosure in Enclosures)
            {
                var enclosureConstraints = enclosure.CheckConstraints();
                constraints.Add($"--- Enclosure: {enclosure.Name} ---");
                constraints.AddRange(enclosureConstraints);
                
                if (enclosureConstraints.Any(c => c.StartsWith("❌")))
                {
                    allValid = false;
                }
            }

            if (allValid && constraints.Any())
            {
                constraints.Insert(0, "✅ All constraints satisfied for this zoo!");
            }
            else if (!allValid)
            {
                constraints.Insert(0, "❌ Some constraints are violated!");
            }

            return constraints;
        }
    }



}
