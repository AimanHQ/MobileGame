using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Goldmetal.UndeadSurvivor
{
    public class Spawner : MonoBehaviour
    {
        public Transform[] spawnPoint; // Array of spawn points
        public SpawnData[] spawnData; // Array of spawn data for different levels
        public float levelTime; // Time for each level

        int level; // Current level
        float timer; // Timer to track spawn intervals

        void Awake()
        {
            // Get all child Transforms, including this object's Transform
            spawnPoint = GetComponentsInChildren<Transform>();
            // Calculate level time based on max game time and number of levels
            levelTime = GameManager.instance.maxGameTime / spawnData.Length;
        }

        void Update()
        {
            // If the game is not live, return
            if (!GameManager.instance.isLive)
                return;

            // Increment the timer by the time elapsed since the last frame
            timer += Time.deltaTime;
            // Determine the current level based on game time and level time
            level = Mathf.Min(Mathf.FloorToInt(GameManager.instance.gameTime / levelTime), spawnData.Length - 1);

            // Check if it's time to spawn an enemy
            if (timer > spawnData[level].spawnTime) {
                timer = 0; // Reset the timer
                Spawn(); // Spawn an enemy
            }
        }

        void Spawn()
        {
            // Get an enemy from the pool
            GameObject enemy = GameManager.instance.pool.Get(0);
            // Set the enemy's position to a random spawn point
            enemy.transform.position = spawnPoint[Random.Range(1, spawnPoint.Length)].position;
            // Initialize the enemy with the current level's spawn data
            enemy.GetComponent<Enemy>().Init(spawnData[level]);
        }
    }

    // Class to hold spawn data for different levels
    [System.Serializable]
    public class SpawnData
    {
        public float spawnTime; // Time between spawns
        public int spriteType; // Type of sprite to use
        public int health; // Health of the enemy
        public float speed; // Speed of the enemy
    }
}
