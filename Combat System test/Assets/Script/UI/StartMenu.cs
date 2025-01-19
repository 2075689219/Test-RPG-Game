using UnityEngine;
using UnityEngine.UI;

public class StartMenu : MonoBehaviour
{
    public Animator cameraAnimator; // 用于控制相机的动画组件
    public GameObject startMenuUI; // 开始菜单的 UI 界面
    public GameObject player; // 第三人称主角对象
    public Camera mainCamera; // 主相机

    private void Start()
    {
        // 确保开始菜单在游戏开始时是可见的
        startMenuUI.SetActive(true);
    }

    public void OnStartButtonClick()
    {
        // 隐藏开始菜单
        startMenuUI.SetActive(false);
        // 播放相机动画
        cameraAnimator.SetTrigger("StartAnimation");
    }

    public void OnAnimationEnd()
    {
        // 当相机动画结束后，将相机切换到第三人称主角身上
        mainCamera.transform.SetParent(player.transform);
        mainCamera.transform.localPosition = new Vector3(0, 2, -5); // 根据需要调整相机的位置
        mainCamera.transform.localRotation = Quaternion.Euler(10, 0, 0); // 根据需要调整相机的旋转
    }

    public void OnExitButtonClick()
    {
        // 退出游戏
        Application.Quit();
    }
}