namespace Tundra
{
    interface ColonyI
    {
        string Name { get; }
        char Species { get; }
        int Population { get; }
        int StartingPopulation { get; }

        void Reproduce(int currentTurn);
        string ToString();
    }

    abstract class Colony : ColonyI
    {
        public string Name { get; }
        public char Species { get; }
        public int Population { get; internal protected set; }
        public int StartingPopulation { get; }

        protected Colony(string name, char species, int population)
        {
            Name = name;
            Species = species;
            Population = population;
            StartingPopulation = population;
        }

        public abstract void Reproduce(int currentTurn);

        public override string ToString()
        {
            return $"{Name} ({Species}): {Population}";
        }
    }

    class PreyColony : Colony
    {
        public PreyColony(string name, char species, int population) : base(name, species, population) { }

        public override void Reproduce(int currentTurn)
        {
            switch (Species)
            {
                case 'l':
                    if (currentTurn % 2 == 0)
                    {
                        Population *= 2;
                    }
                    break;
                case 'h':
                    if (currentTurn % 2 == 0 && Population != 1)
                    {
                        Population *= 3 / 2;

                    }
                    else if (currentTurn % 4 == 0 && Population == 1)
                    {
                        Population *= 2;
                    }
                    break;
                case 'g':
                    if (currentTurn % 4 == 0)
                    {
                        Population *= 2;

                    }

                    break;
            }

        }

    }

    class PredatorColony : Colony
    {

        public PredatorColony(string name, char species, int population) : base(name, species, population) { }

        public override void Reproduce(int currentTurn)
        {
            if (currentTurn % 8 == 0)
            {
                Population += GetOffsprings();
            }

        }

        private int predatorsWithoutPrey = 0;

        public void Attack(Colony preyColony)
        {
            if (preyColony != null && preyColony is PreyColony)
            {
                int predators = Population;
                int preyPopulation = preyColony.Population;

                int decreaseTimes = 0;
                if (preyColony.Species == 'l')
                {
                    decreaseTimes = 4;
                }
                else
                {
                    decreaseTimes = 2;
                }

                if (preyPopulation >= predators * decreaseTimes)
                {
                    preyColony.Population -= predators*decreaseTimes;
                }
                else
                {
                    preyColony.Population -= (preyPopulation / decreaseTimes) * decreaseTimes;
                    predatorsWithoutPrey = (predators - (preyPopulation / decreaseTimes));
                    Population = (preyPopulation / decreaseTimes) + (predatorsWithoutPrey-(predatorsWithoutPrey / decreaseTimes));
                }
                
            }
        }

        private int GetOffsprings()
        {
            switch (Species)
            {
                case 'o': return Population / 4;
                case 'f': return (Population * 3) / 4;
                case 'w': return (Population / 4)*2;
                default: return 0;
            }
        }
    }
}
