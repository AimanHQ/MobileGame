using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Goldmetal.UndeadSurvivor
{
    // The Result class manages the display of game result titles (win/lose).
    public class Result : MonoBehaviour
    {
        // Public array to hold references to the title GameObjects.
        public GameObject[] titles;

        // Method to display the lose title.
        public void Lose()
        {
            // Activate the first title (assuming it's the lose title).
            titles[0].SetActive(true);
        }

        // Method to display the win title.
        public void Win()
        {
            // Activate the second title (assuming it's the win title).
            titles[1].SetActive(true);
        }
    }
}
