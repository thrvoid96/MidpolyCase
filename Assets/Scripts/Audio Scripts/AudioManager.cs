using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The audio manager
/// </summary>
public static class AudioManager
{
    static AudioSource audioSource;
 

    static Dictionary<AudioClipName, AudioClip> audioClips =
        new Dictionary<AudioClipName, AudioClip>();

    public static void Initialize(AudioSource source)
    {
        audioSource = source;
        

        if (!audioClips.ContainsKey(AudioClipName.BreakOut))
        {
        audioClips.Add(AudioClipName.BreakOut,
        Resources.Load<AudioClip>("Sounds/BreakOut"));
        }

        if (!audioClips.ContainsKey(AudioClipName.BridgeGold))
        {
            audioClips.Add(AudioClipName.BridgeGold,
            Resources.Load<AudioClip>("Sounds/BridgeGold"));
        }
        
        if (!audioClips.ContainsKey(AudioClipName.Failed))
        {
            audioClips.Add(AudioClipName.Failed,
            Resources.Load<AudioClip>("Sounds/Failed"));
        }


        if (!audioClips.ContainsKey(AudioClipName.GoldFlight))
        {
            audioClips.Add(AudioClipName.GoldFlight,
            Resources.Load<AudioClip>("Sounds/GoldFlight"));
        }

        if (!audioClips.ContainsKey(AudioClipName.PrizeChest))
        {
            audioClips.Add(AudioClipName.PrizeChest,
            Resources.Load<AudioClip>("Sounds/PrizeChest"));
        }

        if (!audioClips.ContainsKey(AudioClipName.Upgrade))
        {
            audioClips.Add(AudioClipName.Upgrade,
            Resources.Load<AudioClip>("Sounds/Upgrade"));
        }

        if (!audioClips.ContainsKey(AudioClipName.ReachFinishCircle))
        {
            audioClips.Add(AudioClipName.ReachFinishCircle,
            Resources.Load<AudioClip>("Sounds/ReachFinishCircle"));
        }

        if (!audioClips.ContainsKey(AudioClipName.TubeCollect))
        {
            audioClips.Add(AudioClipName.TubeCollect,
            Resources.Load<AudioClip>("Sounds/TubeCollect"));
        }

        if (!audioClips.ContainsKey(AudioClipName.VictoryPanelMovingCoin))
        {
            audioClips.Add(AudioClipName.VictoryPanelMovingCoin,
            Resources.Load<AudioClip>("Sounds/VictoryPanelMovingCoin"));
        }

        if (!audioClips.ContainsKey(AudioClipName.VictoryPanelOpen))
        {
            audioClips.Add(AudioClipName.VictoryPanelOpen,
            Resources.Load<AudioClip>("Sounds/VictoryPanelOpen"));
        }

    }
    public static void Play(AudioClipName name)
    {
        audioSource.PlayOneShot(audioClips[name], GameManager.Sound);
    }

    public static void ClearSound()
    {
        audioClips.Clear();
        Initialize(audioSource);
    }

 
}
