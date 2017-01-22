using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneRoot : MonoBehaviour {
    public string m_gameSceneName;
    protected void Awake()
    {
        if (SceneManager.GetActiveScene().buildIndex != 0)
        {
            // load the root scene to make testing faster
            SceneManager.LoadScene(m_gameSceneName, LoadSceneMode.Single);
        }
    }
}
