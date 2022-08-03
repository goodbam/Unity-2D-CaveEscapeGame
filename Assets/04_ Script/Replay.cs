using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Replay : MonoBehaviour
{
    bool rePlayGame = false;

    public void ReplayGameSwich()
    {
        rePlayGame = true;
        if (rePlayGame)
        {
            FindObjectOfType<GameSession>().ResetGameSession();
        }
    }
}
