using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

namespace Logic
{
    public class RunManager : MonoBehaviour
    {
        public static RunManager Instance;

        public int seed;
        
        public Run currentRun = new Run(1);
        public List<int> currentDeck = new List<int>();
        public List<int> itemList = new List<int>();
        
        private readonly int[] defaultDeck = new int[] {0, 0, 0, 0, 0, 1, 1, 1, 1, 1};

        private void Awake()
        {
            seed = Random.Range(0, 1000000000);
        }

        private void Start()
        {
            if (Instance != null)
                Destroy(gameObject);
            else
                Instance = this;
        
            DontDestroyOnLoad(this);
            
            currentDeck.AddRange(defaultDeck);
        }

        public void StartNewRun(int startingItem)
        {
            currentRun = new Run(seed);
            
            currentRun.deck = currentDeck.ToArray();
            
            AddItemToInventory(startingItem);

            SaveRun();
        
            SceneManager.LoadScene("Map");
        }

        public void SaveRun()
        {
            SaveLoadManager.SaveRun(currentRun);
        }

        public void LoadRun()
        {
            currentRun = SaveLoadManager.LoadRun();
            
            seed = currentRun.randomSeed;
            currentDeck.Clear();
            currentDeck.AddRange(currentRun.deck);

            SceneManager.LoadScene("Map");
        }

        public void AddCardToDeck(int newIndex)
        {
            currentDeck.Add(newIndex);
            currentRun.deck = currentDeck.ToArray();
        }
        
        public void AddItemToInventory(int newIndex)
        {
            itemList.Add(newIndex);
            currentRun.items = itemList.ToArray();
        }
        
    }
}
