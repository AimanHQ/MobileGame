using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Goldmetal.UndeadSurvivor
{
    // The HUD class manages different types of information displayed on the Heads-Up Display (HUD) in the game.
    public class HUD : MonoBehaviour
    {
        // Define the types of information that can be displayed on the HUD.
        public enum InfoType { Exp, Level, Kill, Time, Health }
        public InfoType type;

        // Private fields for the text and slider components.
        Text myText;
        Slider mySlider;

        // Awake is called when the script instance is being loaded.
        void Awake()
        {
            // Initialize the text and slider components.
            myText = GetComponent<Text>();
            mySlider = GetComponent<Slider>();
        }

        // LateUpdate is called once per frame, after all Update functions have been called.
        void LateUpdate()
        {
            // Update the HUD element based on the type of information.
            switch (type)
            {
                case InfoType.Exp:
                    // Update the experience bar.
                    float curExp = GameManager.instance.exp;
                    float maxExp = GameManager.instance.nextExp[Mathf.Min(GameManager.instance.level, GameManager.instance.nextExp.Length - 1)];
                    mySlider.value = curExp / maxExp;
                    break;
                case InfoType.Level:
                    // Update the level display.
                    myText.text = string.Format("Lv.{0:F0}", GameManager.instance.level);
                    break;
                case InfoType.Kill:
                    // Update the kill count display.
                    myText.text = string.Format("{0:F0}", GameManager.instance.kill);
                    break;
                case InfoType.Time:
                    // Update the game timer display.
                    float remainTime = GameManager.instance.maxGameTime - GameManager.instance.gameTime;
                    int min = Mathf.FloorToInt(remainTime / 60);
                    int sec = Mathf.FloorToInt(remainTime % 60);
                    myText.text = string.Format("{0:D2}:{1:D2}", min, sec);
                    break;
                case InfoType.Health:
                    // Update the health bar.
                    float curHealth = GameManager.instance.health;
                    float maxHealth = GameManager.instance.maxHealth;
                    mySlider.value = curHealth / maxHealth;
                    break;
            }
        }
    }
}
