using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Mako.State
{

    public class MenuController : MonoBehaviour
    {
        public static MenuController instance;
        public GameObject sideWindow = null;
        [SerializeField] private LoadingScreen _loadingScreen;
        [SerializeField] private CoroutinePerformer _coroutinePerformer;
        private void Awake()
        {
            //Singleton method
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);

            }
            else if (instance != this)
            {
                Destroy(instance.gameObject);
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
        }
        public void EnableGameObject(GameObject obj)
        {
            bool toEnable = false;
            toEnable = obj.activeInHierarchy ? false : true;
            obj.SetActive(toEnable);
            if (toEnable == false)
            {
                sideWindow = null;
            }
            else
            {
                if (sideWindow != null)
                {
                    sideWindow.SetActive(false);
                }
                sideWindow = obj;
            }
        }
        public void RestartScene()
        {
            Time.timeScale = 1;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        public void RestartSceneAsync()
        {
            Time.timeScale = 1;
            StartCoroutine(LoadAsync(SceneManager.GetActiveScene().buildIndex));
        }
        public void GoToScene(int sceneIndex)
        {
            Time.timeScale = 1;
            SceneManager.LoadScene(sceneIndex);
        }
        public void GoToSceneAsync(int sceneIndex)
        {
            Time.timeScale = 1;
            StartCoroutine(LoadAsync(sceneIndex));
        }

        public IEnumerator LoadAsync(int sceneIndex)
        {
            AsyncOperation waitLoading = SceneManager.LoadSceneAsync(sceneIndex);
            yield return new WaitUntil(() => waitLoading.isDone);
        }
        public void TransitToNewScene(int index)
        {
            _coroutinePerformer.StartCoroutine(ProcessSwitchScene(index));
        }
        private IEnumerator ProcessSwitchScene(int sceneIndex)
        {
            _loadingScreen.Show();
            _loadingScreen.ShowMessage("Loading...");

            yield return LoadAsync(sceneIndex);

            _loadingScreen.Hide();
        }

        public void GoToScene(string sceneName)
        {
            Time.timeScale = 1;
            SceneManager.LoadScene(sceneName);
        }

        public void Quit()
        {
            Application.Quit();
        }
    }
}