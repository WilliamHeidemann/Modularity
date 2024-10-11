using UnityEngine;
using UnityEngine.SceneManagement;

namespace Runtime.Components
{
    public class Exit : MonoBehaviour
    {
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.R)) SceneManager.LoadScene(0);
            if (Input.GetKeyDown(KeyCode.Escape)) Application.Quit();
        }
    }
}
