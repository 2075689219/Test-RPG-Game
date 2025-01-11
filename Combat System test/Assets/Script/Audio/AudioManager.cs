using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct AudioData
{
    public string name;        // 自定义音频名称
    public AudioClip clip;     // 对应的音频文件
    public float defaultVolume ; // 默认音量 
    public string group;       // 分组名称（可选）
}

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [SerializeField] private List<AudioData> audioDataList; // 配置音效数据
    private Dictionary<string, AudioSource> audioSources;   // 自定义名称到 AudioSource 的映射
    private Dictionary<string, List<string>> audioGroups;   // 分组名到音效名称列表的映射

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 跨场景持久化
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        audioSources = new Dictionary<string, AudioSource>();
        audioGroups = new Dictionary<string, List<string>>();

        // 初始化音效数据
        foreach (var data in audioDataList)
        {
            AudioSource source = gameObject.AddComponent<AudioSource>();
            source.clip = data.clip;
            source.volume = data.defaultVolume > 0 ? data.defaultVolume : 1f; // 如果未设置，默认音量为 1
            audioSources.Add(data.name, source);

            // 添加到对应的分组
            if (!string.IsNullOrEmpty(data.group))
            {
                if (!audioGroups.ContainsKey(data.group))
                {
                    audioGroups[data.group] = new List<string>();
                }
                audioGroups[data.group].Add(data.name);
            }
        }
    }

    private void Start()
    {
        PreloadAudio(); // 预加载音频数据
    }

    private void PreloadAudio()
    {
        foreach (var data in audioDataList)
        {
            AudioClip preloadClip = data.clip;
            if (preloadClip.loadState != AudioDataLoadState.Loaded)
            {
                preloadClip.LoadAudioData();
            }
        }
    }

    // 播放音效
    public void Play(string audioName)
    {
        if (audioSources.TryGetValue(audioName, out AudioSource source))
        {
            source.Play();
        }
        else
        {
            Debug.LogWarning($"Audio with name {audioName} not found!");
        }
    }

    // 随机播放指定分组的音效
    public void PlayRandomFromGroup(string groupName)
    {
        if (audioGroups.TryGetValue(groupName, out List<string> group))
        {
            int randomIndex = Random.Range(0, group.Count);
            Play(group[randomIndex]);
        }
        else
        {
            Debug.LogWarning($"Audio group {groupName} not found!");
        }
    }

    // 设置音量
    public void SetVolume(string audioName, float volume)
    {
        if (audioSources.TryGetValue(audioName, out AudioSource source))
        {
            source.volume = Mathf.Clamp01(volume);
        }
    }

    // 停止音效
    public void Stop(string audioName)
    {
        if (audioSources.TryGetValue(audioName, out AudioSource source))
        {
            source.Stop();
        }
    }

    // 当面板数据变化时自动更新音量
    private void OnValidate()
    {
        if (audioSources != null)
        {
            foreach (var data in audioDataList)
            {
                if (audioSources.TryGetValue(data.name, out AudioSource source))
                {
                    source.volume = data.defaultVolume;
                }
            }
        }
    }
}
