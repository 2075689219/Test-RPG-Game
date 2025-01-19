using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public MeeleFighter meeleFighter;
    public GameObject deathUI;
    private bool canRestart = false; // 用于标记是否可以重新开始游戏

    private void Start()
    {
        meeleFighter = GetComponent<MeeleFighter>();
    }

    void Update()
    {
        float health = meeleFighter.Health;
        if (health <= 0)
        {
            deathUI.SetActive(true);
            if (!canRestart)
            {
                StartCoroutine(WaitAndEnableRestart()); // 启动协程等待 2 秒
            }
            if (canRestart && Input.GetMouseButtonDown(0))
            {
                // 重新开始游戏，这里简单地重新加载当前场景
                UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
            }
        }
    }

    // 协程方法，用于等待 2 秒
    IEnumerator WaitAndEnableRestart()
    {
        yield return new WaitForSeconds(2f); // 等待 2 秒
        canRestart = true; // 2 秒后允许重新开始
    }
}