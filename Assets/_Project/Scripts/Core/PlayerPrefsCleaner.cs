using UnityEngine;

namespace _Project.Scripts.Core
{
    public class PlayerPrefsCleaner : MonoBehaviour
    {
        private void Start()
        {
            PlayerPrefs.DeleteAll();
            PlayerPrefs.Save();
            Debug.Log("PlayerPrefs очищено!");
        }
    }
}