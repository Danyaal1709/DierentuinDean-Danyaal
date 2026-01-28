namespace Dierentuin.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Collections.Generic;

    // Animal model class
    public class Animal
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public required string Name { get; set; }

        [Required]
        [MaxLength(100)]
        public required string Species { get; set; }

        // Relationship with Category (nullable)
        public int? CategoryId { get; set; }
        public Category? Category { get; set; }

        [Required]
        public SizeEnum Size { get; set; }

        [Required]
        public DietaryClassEnum DietaryClass { get; set; }

        [Required]
        public ActivityPatternEnum ActivityPattern { get; set; }

        [MaxLength(500)]
        public string? Prey { get; set; }

        // Relationship with Enclosure (nullable)
        public int? EnclosureId { get; set; }
        public Enclosure? Enclosure { get; set; }

        [Required]
        public double SpaceRequirement { get; set; }

        [Required]
        public SecurityLevelEnum SecurityRequirement { get; set; }

        // Returns action at sunrise based on activity pattern
        public string ActieSunrise()
        {
            return ActivityPattern switch
            {
                ActivityPatternEnum.Diurnal => "Wakes up",
                ActivityPatternEnum.Nocturnal => "Goes to sleep",
                ActivityPatternEnum.Cathemeral => "Always active",
                _ => "Unknown"
            };
        }

        // Returns action at sunset based on activity pattern
        public string ActieSunset()
        {
            return ActivityPattern switch
            {
                ActivityPatternEnum.Diurnal => "Goes to sleep",
                ActivityPatternEnum.Nocturnal => "Wakes up",
                ActivityPatternEnum.Cathemeral => "Always active",
                _ => "Unknown"
            };
        }

        // Returns what the animal eats during feeding time
        public string ActieFeedingTime()
        {
            if (DietaryClass == DietaryClassEnum.Carnivore && !string.IsNullOrEmpty(Prey))
            {
                return $"Eats other animals: {Prey}";
            }
            return $"Eats: {Prey}";
        }

        // Checks if all constraints are satisfied for this animal
        public List<string> CheckConstraints()
        {
            var constraints = new List<string>();

            // Check if animal has an enclosure
            if (Enclosure == null)
            {
                constraints.Add("❌ No enclosure assigned");
            }
            else
            {
                // Check space requirement
                var totalSpaceUsed = Enclosure.Animals.Sum(a => a.SpaceRequirement);
                if (totalSpaceUsed > Enclosure.Size)
                {
                    constraints.Add($"❌ Space constraint violated: {totalSpaceUsed:F2} m² used, but enclosure only has {Enclosure.Size:F2} m²");
                }
                else
                {
                    constraints.Add($"✅ Space requirement met: {totalSpaceUsed:F2} m² / {Enclosure.Size:F2} m²");
                }

                // Check security requirement
                if (SecurityRequirement > Enclosure.SecurityLevel)
                {
                    constraints.Add($"❌ Security constraint violated: Animal requires {SecurityRequirement}, but enclosure has {Enclosure.SecurityLevel}");
                }
                else
                {
                    constraints.Add($"✅ Security requirement met: {SecurityRequirement} <= {Enclosure.SecurityLevel}");
                }

                // Check if category is assigned (optional but good practice)
                if (Category == null)
                {
                    constraints.Add("⚠️ No category assigned (optional)");
                }
                else
                {
                    constraints.Add($"✅ Category assigned: {Category.Name}");
                }
            }

            return constraints;
        }
    }

    // Enum for Size
    public enum SizeEnum
    {
        Microscopic,
        VerySmall,
        Small,
        Medium,
        Large,
        VeryLarge
    }

    // Enum for Dietary Class
    public enum DietaryClassEnum
    {
        Carnivore,
        Herbivore,
        Omnivore,
        Insectivore,
        Piscivore
    }

    // Enum for Activity Pattern
    public enum ActivityPatternEnum
    {
        Diurnal,  // Active during the day
        Nocturnal, // Active during the night
        Cathemeral // Active at intervals throughout the day and night
    }

    // Enum for Security Level
    public enum SecurityLevelEnum
    {
        Low,
        Medium,
        High
    }
}

