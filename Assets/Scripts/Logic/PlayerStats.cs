namespace Logic
{
    [System.Serializable]
    public class PlayerStats
    {
        public int currentHp;
        public int maxHp = 50;
        
        public int power;

        public int baseMana = 4;

        public int startingArmor = 0;
        public int currentArmor = 0;
        
        public int drawAmountForTurn = 8;
        
        public PlayerStats()
        {
            currentHp = maxHp;
        }
    }
    
    public class StatBoosts
    {
        public int maxHp;
        public int power;
        public int bonusManaForCombat;
        public int startingArmor;
        public int bonusDrawAmountForTurn;
    }

    public class TempStatBoosts
    {
        public int Power;
        public int bonusManaForTurn;
        public int bonusDrawAmountForTurn;
    }
}
