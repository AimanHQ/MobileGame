using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Goldmetal.UndeadSurvivor
{
    public class GameManager : MonoBehaviour
    {
        // Singleton instance
        public static GameManager instance;

        [Header("# Game Control")]
        public bool isLive; // Is the game currently live
        public float gameTime; // Current game time
        public float maxGameTime = 2 * 10f; // Maximum game time

        [Header("# Player Info")]
        public int playerId; // Player's ID
        public float health; // Player's current health
        public float maxHealth = 100; // Player's maximum health
        public int level; // Player's current level
        public int kill; // Number of enemies killed
        public int exp; // Current experience points
        public int[] nextExp = { 3, 5, 10, 100, 150, 210, 280, 360, 450, 600 }; // Experience points needed for next level

        [Header("# Game Object")]
        public PoolManager pool; // Reference to the PoolManager
        public Player player; // Reference to the Player
        public LevelUp uiLevelUp; // Reference to the LevelUp UI
        public Result uiResult; // Reference to the Result UI
        public Transform uiJoy; // Reference to the UI joystick
        public GameObject enemyCleaner; // Reference to the enemy cleaner object

        void Awake()
        {
            // Set the singleton instance
            instance = this;
            // Set the target frame rate
            Application.targetFrameRate = 60;
        }

        // Start the game with the given player ID
        public void GameStart(int id)
        {
            playerId = id;
            health = maxHealth; // Reset player health

            // Activate player and UI elements
            player.gameObject.SetActive(true);
            uiLevelUp.Select(playerId % 2);
            Resume(); // Resume the game

            // Play background music and select sound effect
            AudioManager.instance.PlayBgm(true);
            AudioManager.instance.PlaySfx(AudioManager.Sfx.Select);
        }

        // Handle game over scenario
        public void GameOver()
        {
            StartCoroutine(GameOverRoutine());
        }

        IEnumerator GameOverRoutine()
        {
            isLive = false; // Set game to not live

            yield return new WaitForSeconds(0.5f);

            // Show game over UI and stop the game
            uiResult.gameObject.SetActive(true);
            uiResult.Lose();
            Stop();

            // Play game over sound effects
            AudioManager.instance.PlayBgm(false);
            AudioManager.instance.PlaySfx(AudioManager.Sfx.Lose);
        }

        // Handle game victory scenario
        public void GameVictory()
        {
            StartCoroutine(GameVictoryRoutine());
        }

        IEnumerator GameVictoryRoutine()
        {
            isLive = false; // Set game to not live
            enemyCleaner.SetActive(true); // Activate enemy cleaner

            yield return new WaitForSeconds(0.5f);

            // Show victory UI and stop the game
            uiResult.gameObject.SetActive(true);
            uiResult.Win();
            Stop();

            // Play victory sound effects
            AudioManager.instance.PlayBgm(false);
            AudioManager.instance.PlaySfx(AudioManager.Sfx.Win);
        }

        // Retry the game by reloading the scene
        public void GameRetry()
        {
            SceneManager.LoadScene(0);
        }

        // Quit the game application
        public void GameQuit()
        {
            Application.Quit();
        }

        void Update()
        {
            // If the game is not live, return
            if (!isLive)
                return;

            // Increment game time
            gameTime += Time.deltaTime;

            // Check if game time has exceeded the maximum game time
            if (gameTime > maxGameTime) {
                gameTime = maxGameTime;
                GameVictory(); // Trigger game victory
            }
        }

        // Grant experience points to the player
        public void GetExp()
        {
            if (!isLive)
                return;

            exp++; // Increment experience points

            // Check if player has reached the next level
            if (exp == nextExp[Mathf.Min(level, nextExp.Length - 1)]) {
                level++;
                exp = 0; // Reset experience points
                uiLevelUp.Show(); // Show level-up UI
            }
        }

        // Stop the game
        public void Stop()
        {
            isLive = false; // Set game to not live
            Time.timeScale = 0; // Pause the game
            uiJoy.localScale = Vector3.zero; // Hide the UI joystick
        }

        // Resume the game
        public void Resume()
        {
            isLive = true; // Set game to live
            Time.timeScale = 1; // Resume the game
            uiJoy.localScale = Vector3.one; // Show the UI joystick
        }
    }
}
