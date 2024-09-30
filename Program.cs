using Tundra;
using TextFile;


namespace Tundra
{
    class Program
    {
        static void Main(string[] args)
        {
            string fileName = "C:\\Users\\USER\\Desktop\\GitHub\\Tundra\\input.txt";

            TextFileReader reader = new TextFileReader(fileName);
            int numPrey = int.Parse(reader.ReadLine().Split(' ')[0]);

            int turn = 1;
            Random random = new Random();

            List<Colony> colonies = ReadColoniesFromFile(fileName, turn);

            Console.WriteLine("Initial input: ");
            foreach (Colony colony in colonies)
            {
                Console.WriteLine(colony);
            }

            while (true)
            {
                Console.WriteLine($"\nTurn {turn}:");

                foreach (Colony colony in colonies)
                {
                    colony.Reproduce(turn);
                }

                foreach (PredatorColony predator in colonies.OfType<PredatorColony>())
                {
                    var preyColonies = colonies.OfType<PreyColony>().ToList();
                    PreyColony targetColony = preyColonies[random.Next(preyColonies.Count)];
                    predator.Attack(targetColony);
                }

                int conditionsMet = 0;
                foreach (Colony colony in colonies)
                {
                    if (IsExtinct(colony) || IsQuadrupled(colony))
                        conditionsMet++;
                }

                foreach (Colony colony in colonies)
                {
                    Console.WriteLine(colony);
                }

                if (conditionsMet == numPrey)
                    break;

                turn++;

            }
        }

        static List<Colony> ReadColoniesFromFile(string fileName, int turnCount)
        {
            List<Colony> colonies = new List<Colony>();

            TextFileReader reader = new TextFileReader(fileName);

            string firstLine = reader.ReadLine();
            int numPrey, numPredators;
            numPrey = int.Parse(firstLine.Split(' ')[0]);
            numPredators = int.Parse(firstLine.Split(' ')[1]);

            for (int i = 0; i < numPrey + numPredators; i++)
            {
                string line = reader.ReadLine();
                string[] data = line.Split(' ');

                string name = data[0];
                char species = char.ToLower(data[1][0]);
                int population = int.Parse(data[2]);

                if (species == 'l' || species == 'h' || species == 'g')
                {
                    colonies.Add(new PreyColony(name, species, population));
                }
                else if (species == 'o' || species == 'f' || species == 'w')
                {
                    colonies.Add(new PredatorColony(name, species, population));
                }
            }

            return colonies;
        }

        static bool IsExtinct(Colony colony)
        {

            if (colony is PreyColony preyColony && preyColony.Population <= 0)
            {
                return true;

            }
            else
            {
                return false;
            }
        }

        static bool IsQuadrupled(Colony colony)
        {
            if (colony is PreyColony preyColony && preyColony.Population >= preyColony.StartingPopulation * 4)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

}
