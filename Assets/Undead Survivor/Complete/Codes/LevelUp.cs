using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Goldmetal.UndeadSurvivor
{
    // The LevelUp class manages the level-up interface in the game.
    public class LevelUp : MonoBehaviour
    {
        // Private fields for internal use.
        RectTransform rect; // The RectTransform of the level-up interface.
        Item[] items; // Array of item components available for level-up.

        // Awake is called when the script instance is being loaded.
        void Awake()
        {
            // Initialize the RectTransform and item components.
            rect = GetComponent<RectTransform>();
            items = GetComponentsInChildren<Item>(true);
        }

        // Method to show the level-up interface.
        public void Show()
        {
            // Select the next items to be displayed.
            Next();
            // Set the scale to make the interface visible.
            rect.localScale = Vector3.one;
            // Stop the game and play level-up sound effects and background music.
            GameManager.instance.Stop();
            AudioManager.instance.PlaySfx(AudioManager.Sfx.LevelUp);
            AudioManager.instance.EffectBgm(true);
        }

        // Method to hide the level-up interface.
        public void Hide()
        {
            // Set the scale to zero to hide the interface.
            rect.localScale = Vector3.zero;
            // Resume the game and play selection sound effects and background music.
            GameManager.instance.Resume();
            AudioManager.instance.PlaySfx(AudioManager.Sfx.Select);
            AudioManager.instance.EffectBgm(false);
        }

        // Method to handle item selection.
        public void Select(int index)
        {
            // Call the OnClick method of the selected item.
            items[index].OnClick();
        }

        // Method to determine the next set of items to display.
        void Next()
        {
            // 1. Deactivate all items initially.
            foreach (Item item in items)
            {
                item.gameObject.SetActive(false);
            }

            // 2. Randomly select 3 different items to activate.
            int[] ran = new int[3];
            while (true)
            {
                ran[0] = Random.Range(0, items.Length);
                ran[1] = Random.Range(0, items.Length);
                ran[2] = Random.Range(0, items.Length);

                if (ran[0] != ran[1] && ran[1] != ran[2] && ran[0] != ran[2])
                    break;
            }

            for (int index = 0; index < ran.Length; index++)
            {
                Item ranItem = items[ran[index]];

                // 3. If the selected item's level is maxed out, activate a specific item.
                if (ranItem.level == ranItem.data.damages.Length)
                {
                    items[4].gameObject.SetActive(true); // Assuming item at index 4 is a placeholder for maxed-out items.
                }
                else
                {
                    ranItem.gameObject.SetActive(true);
                }
            }
        }
    }
}
