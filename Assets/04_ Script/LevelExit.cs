using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class LevelExit : MonoBehaviour
{
    [SerializeField] float levelLoadDelay = 1f;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            StartCoroutine(LoadNextLevel());
        }
    }

    IEnumerator LoadNextLevel()
    {
        // 코루틴을 사용할 떄는 항상 yield return new 먼저 붙여 줘야한다.
        yield return new WaitForSecondsRealtime(levelLoadDelay);
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex; // 현재 Scene의 인덱스 번호를 가져온다.
        int nextSceneIndex = currentSceneIndex + 1;

        // 다음 Scene Index가 현재 
        // 현재 몇개의 Scene이 있는지 가져온다.
        // 마지막 Scene라면 현재 Scene를 초기화한다.
        if (nextSceneIndex == SceneManager.sceneCountInBuildSettings)
        {
            nextSceneIndex = 1;
        }

        FindObjectOfType<ScenePersist>().ResetScenePersist();
        SceneManager.LoadScene(nextSceneIndex); // 다음 Scene을 불러온다.
    }
}
