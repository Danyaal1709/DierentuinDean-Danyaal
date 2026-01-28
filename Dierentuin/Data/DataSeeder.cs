using Bogus;
using Dierentuin.Data;
using Dierentuin.Models;
using System.Linq;

namespace Dierentuin.Data
{
    // Seeds the database with initial test data
    public class DataSeeder
    {
        // Constants to avoid magic numbers
        private const int BASE_ENCLOSURE_SIZE = 1000;
        private const int ENCLOSURE_SIZE_INCREMENT = 500;
        private const int BASE_SPACE_REQUIREMENT = 10;
        private const int SPACE_REQUIREMENT_INCREMENT = 10;

        private readonly DierentuinContext _context;

        public DataSeeder(DierentuinContext context)
        {
            _context = context;
        }

        // Seeds the database with categories, zoos, enclosures, and animals
        public void Seed()
        {
            // Clear existing data
            ClearExistingData();

            // Seed categories
            var categories = GenerateUniqueEntities<Category>(new[] { "Mammals", "Birds", "Reptiles", "Amphibians", "Fish" }, (name, index) => new Category { Name = $"{name} {index + 1}" });
            _context.Category.AddRange(categories);
            _context.SaveChanges();

            // Seed zoos
            var zoos = GenerateUniqueEntities<Zoo>(new[] { "Zoo A", "Zoo B", "Zoo C" }, (name, index) => new Zoo { Name = $"{name} {index + 1}" });
            _context.Zoo.AddRange(zoos);
            _context.SaveChanges();

            // Seed enclosures
            var faker = new Faker();
            var habitatTypes = new[] { HabitatTypeEnum.Forest, HabitatTypeEnum.Desert, HabitatTypeEnum.Grassland, HabitatTypeEnum.Wetland, HabitatTypeEnum.Marine };
            var enclosures = GenerateUniqueEntities<Enclosure>(new[] { "Savannah", "Rainforest", "Desert", "Aquarium", "Aviary" }, (name, index) => new Enclosure
            {
                Name = $"{name} {index + 1}",
                Climate = faker.PickRandom<ClimateEnum>(),
                HabitatType = faker.PickRandom(habitatTypes),
                SecurityLevel = faker.PickRandom<SecurityLevelEnum>(),
                Size = BASE_ENCLOSURE_SIZE + index * ENCLOSURE_SIZE_INCREMENT,
                ZooId = zoos[index % zoos.Count].Id
            });
            _context.Enclosure.AddRange(enclosures);
            _context.SaveChanges();

            // Seed animals
            var animalNames = new[] { "Lion", "Elephant", "Tiger", "Bear", "Wolf", "Eagle", "Shark", "Dolphin", "Giraffe", "Zebra", "Rhino", "Hippo", "Crocodile", "Snake", "Monkey" };
            var speciesPrefixes = new[] { "Panthera", "Loxodonta", "Panthera", "Ursus", "Canis", "Aquila", "Carcharodon", "Delphinus", "Giraffa", "Equus", "Ceratotherium", "Hippopotamus", "Crocodylus", "Python", "Macaca" };
            
            var animals = animalNames.Select((name, index) => new Animal
            {
                Name = $"{name} {index + 1}",
                Species = $"{speciesPrefixes[index]} {faker.Lorem.Word()}",
                CategoryId = categories[index % categories.Count].Id,
                Size = faker.PickRandom<SizeEnum>(),
                DietaryClass = faker.PickRandom<DietaryClassEnum>(),
                ActivityPattern = faker.PickRandom<ActivityPatternEnum>(),
                Prey = string.Join(", ", faker.Lorem.Words(faker.Random.Int(1, 3))),
                EnclosureId = enclosures[index % enclosures.Count].Id,
                SpaceRequirement = BASE_SPACE_REQUIREMENT + faker.Random.Int(0, 50) * SPACE_REQUIREMENT_INCREMENT,
                SecurityRequirement = faker.PickRandom<SecurityLevelEnum>()
            }).ToList();
            
            _context.Animal.AddRange(animals);
            _context.SaveChanges();
        }

        // Clears existing data from database
        private void ClearExistingData()
        {
            try
            {
                // Check if database exists and can connect
                if (!_context.Database.CanConnect())
                {
                    return; // Database doesn't exist yet, nothing to clear
                }

                // Check if tables exist before trying to clear them
                try
                {
                    _context.Animal.RemoveRange(_context.Animal);
                    _context.Category.RemoveRange(_context.Category);
                    _context.Enclosure.RemoveRange(_context.Enclosure);
                    _context.Zoo.RemoveRange(_context.Zoo);
                    _context.SaveChanges();
                }
                catch
                {
                    // Tables might not exist yet, which is fine
                }
            }
            catch
            {
                // If clearing fails, continue anyway - database might be new
            }
        }

        // Helper method to generate unique entities
        private List<T> GenerateUniqueEntities<T>(string[] names, Func<string, int, T> createEntity) where T : class
        {
            return names.Select((name, index) => createEntity(name, index)).ToList();
        }
    }
}
