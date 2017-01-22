using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneRoot : MonoBehaviour {
    protected void Awake()
    {
        if (SceneManager.GetActiveScene().buildIndex != 0)
        {
            // load the root scene to make testing faster
            SceneManager.LoadScene(0, LoadSceneMode.Single);
        }
    }
}
