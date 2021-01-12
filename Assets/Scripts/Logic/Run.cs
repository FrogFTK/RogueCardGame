namespace Logic
{
    [System.Serializable]
    public class Run
    {
        public int randomSeed;

        public PlayerStats stats;

        public int[] deck = new int[0];
        public int[] items = new int[0];
    
        public Run(int seed)
        {
            randomSeed = seed;
            stats = new PlayerStats();
        }

    }
}