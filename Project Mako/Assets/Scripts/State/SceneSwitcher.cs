using UnityEngine;
using UnityEngine.SceneManagement;

namespace Mako.State
{
    public class SceneSwitcher : MonoBehaviour
    {
        public void GoToScene(int sceneIndex)
        {
            SceneManager.LoadScene(sceneIndex);
        }

        public void GoToScene(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
        }

        public void Quit()
        {
            Application.Quit();
        }
    }
}
