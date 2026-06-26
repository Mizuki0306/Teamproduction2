using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public bool GameClearJudge = false;
    public string NextSceneName;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (GameClearJudge == true)
        {
            if (!string.IsNullOrEmpty(NextSceneName))
            {
                SceneManager.LoadScene(NextSceneName);
            }
            else
            {
                Debug.LogWarning("遷移先の nextSceneName がインスペクターで設定されていません！");
            }
        }
    }
}
