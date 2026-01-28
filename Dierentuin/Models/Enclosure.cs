namespace Dierentuin.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Collections.Generic;

    public class Enclosure
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public required string Name { get; set; }

        // Relationship with Animals
        public List<Animal> Animals { get; set; } = new List<Animal>();

        [Required]
        public ClimateEnum Climate { get; set; }

        [Required]
        public HabitatTypeEnum HabitatType { get; set; }

        [Required]
        public SecurityLevelEnum SecurityLevel { get; set; }

        [Required]
        public double Size { get; set; }

        // Relationship with Zoo (nullable)
        public int? ZooId { get; set; }
        public Zoo? Zoo { get; set; }

        // Method to get sunrise actions for all animals in this enclosure
        public List<string> ActieSunrise()
        {
            var actions = new List<string>();
            foreach (var animal in Animals)
            {
                actions.Add($"{animal.Name}: {animal.ActieSunrise()}");
            }
            return actions;
        }

        // Method to get sunset actions for all animals in this enclosure
        public List<string> ActieSunset()
        {
            var actions = new List<string>();
            foreach (var animal in Animals)
            {
                actions.Add($"{animal.Name}: {animal.ActieSunset()}");
            }
            return actions;
        }

        // Method to get feeding time actions for all animals in this enclosure
        // Carnivores eating other animals takes priority
        public List<string> ActieFeedingTime()
        {
            var actions = new List<string>();
            foreach (var animal in Animals)
            {
                // Check if carnivore and has prey animals in the same enclosure
                if (animal.DietaryClass == DietaryClassEnum.Carnivore && !string.IsNullOrEmpty(animal.Prey))
                {
                    // Check if any animals in enclosure match the prey
                    var preyAnimals = Animals.Where(a => animal.Prey.Contains(a.Name, StringComparison.OrdinalIgnoreCase)).ToList();
                    if (preyAnimals.Any())
                    {
                        actions.Add($"{animal.Name}: Eats other animals - {string.Join(", ", preyAnimals.Select(a => a.Name))}");
                    }
                    else
                    {
                        actions.Add($"{animal.Name}: {animal.ActieFeedingTime()}");
                    }
                }
                else
                {
                    actions.Add($"{animal.Name}: {animal.ActieFeedingTime()}");
                }
            }
            return actions;
        }

        // Method to check if all constraints are satisfied for this enclosure
        public List<string> CheckConstraints()
        {
            var constraints = new List<string>();

            // Check total space usage
            var totalSpaceUsed = Animals.Sum(a => a.SpaceRequirement);
            if (totalSpaceUsed > Size)
            {
                constraints.Add($"❌ Space constraint violated: {totalSpaceUsed:F2} m² used, but enclosure only has {Size:F2} m²");
            }
            else
            {
                constraints.Add($"✅ Space constraint met: {totalSpaceUsed:F2} m² / {Size:F2} m²");
            }

            // Check security levels for all animals
            var securityViolations = Animals.Where(a => a.SecurityRequirement > SecurityLevel).ToList();
            if (securityViolations.Any())
            {
                constraints.Add($"❌ Security constraint violated for: {string.Join(", ", securityViolations.Select(a => a.Name))}");
            }
            else
            {
                constraints.Add($"✅ Security constraints met: All animals' requirements satisfied");
            }

            // Check for carnivore conflicts (carnivores shouldn't be with their prey)
            var carnivores = Animals.Where(a => a.DietaryClass == DietaryClassEnum.Carnivore).ToList();
            foreach (var carnivore in carnivores)
            {
                if (!string.IsNullOrEmpty(carnivore.Prey))
                {
                    var preyInEnclosure = Animals.Where(a => carnivore.Prey.Contains(a.Name, StringComparison.OrdinalIgnoreCase)).ToList();
                    if (preyInEnclosure.Any())
                    {
                        constraints.Add($"⚠️ Warning: {carnivore.Name} (carnivore) is in same enclosure as potential prey: {string.Join(", ", preyInEnclosure.Select(a => a.Name))}");
                    }
                }
            }

            // Check if zoo is assigned
            if (Zoo == null)
            {
                constraints.Add("⚠️ No zoo assigned (optional)");
            }
            else
            {
                constraints.Add($"✅ Zoo assigned: {Zoo.Name}");
            }

            return constraints;
        }
    }

    // Enum for Climate
    public enum ClimateEnum
    {
        Tropical,
        Temperate,
        Arctic
    }

    // Enum for Habitat Type (Flags enum - can combine multiple values)
    [Flags]
    public enum HabitatTypeEnum
    {
        Forest = 1,
        Desert = 2,
        Grassland = 4,
        Wetland = 8,
        Marine = 16
    }

 
}


