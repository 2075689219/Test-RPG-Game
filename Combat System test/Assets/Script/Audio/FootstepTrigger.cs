using UnityEngine;

public class FootstepTrigger : MonoBehaviour
{
    //[SerializeField] private string footstepSoundName; // 音效名称
    private bool hasPlayed = false; // 防止一次落地重复播放

    private void OnTriggerEnter(Collider other)
    {
        // 确保只有接触地面时播放音效
        if (other.gameObject.layer == LayerMask.NameToLayer("Obstacles") && !hasPlayed)
        {
            AudioManager.Instance.PlayRandomFromGroup("Footstep");
            hasPlayed = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // 离开地面后重置
        if (other.gameObject.layer == LayerMask.NameToLayer("Obstacles"))
        {
            hasPlayed = false;
        }
    }
}
