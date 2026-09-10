using UnityEngine;
using UnityEngine.SceneManagement;
using WordVista.Save;
using WordVista.Economy;

namespace WordVista.Core
{
    public class BootstrapManager : MonoBehaviour
    {
        private void Start()
        {
            // Initialize Core Singletons
            var save = SaveManager.Instance;
            var econ = EconomyManager.Instance;

            Debug.Log($"[Bootstrap] WordVista 3D Initialized. Highest Level: {save.Data.highestUnlockedLevel}, Coins: {save.Data.coins}");

            // Load MainMenu
            SceneManager.LoadScene("MainMenu");
        }
    }
}
