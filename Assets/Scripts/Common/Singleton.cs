using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Singleton : MonoBehaviour
{
    public static Singleton instance = null;

    public GameObject playerPrefab;

    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            StartCoroutine("LoadSceneAsync", "DebugScene");
        } else
        {
            Destroy(gameObject);
        }
    }

    IEnumerator LoadSceneAsync(string sceneToLoad)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneToLoad);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        //CurrentScene = sceneToLoad;

        //Todo: Instantiate the player.

        GameObject.Instantiate(playerPrefab, new Vector3(0f, 0f, 0f), Quaternion.identity);

        yield break;
    }
}
