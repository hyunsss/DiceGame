using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }
    [Header("BGM")]
    public AudioClip MainmenuAudioClip;
    public AudioClip GamePlayAudioClip;
    public float MainBgmVolume;
    public float GameBgmVolume;
    public AudioSource BgmPlayer;

    [Header("SFX")]
    public List<AudioClip> SfxClips = new List<AudioClip>();
    public float sfxVolume;
    public int channels;
    AudioSource[] sfxPlayers;  
    int channelIndex;

    public enum Sfx_Dic { Click, LevelUp, TakeDamage, Attack, SearchItem, FindItem, Rocket, DropItem, PlayerTakeDamage }

    void Init() {
        GameObject bgmObject = new GameObject("BgmPlayer");
        bgmObject.transform.parent = transform;
        BgmPlayer = bgmObject.AddComponent<AudioSource>();
        BgmPlayer.volume = MainBgmVolume;
        BgmPlayer.playOnAwake = false;
        BgmPlayer.loop = true;

        GameObject sfxObject = new GameObject("SfxPlayer");
        sfxObject.transform.parent = transform;
        sfxPlayers = new AudioSource[channels];

        for(int i = 0; i < sfxPlayers.Length; i++) {
            sfxPlayers[i] = sfxObject.AddComponent<AudioSource>();
            sfxPlayers[i].playOnAwake = false;
            sfxPlayers[i].volume = sfxVolume;
        }
    }
    private void Awake() {

        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }

        Init();
    }
    
    /*
    여러개의 채널을 만들어 두고 빈곳에 clip을 할당하여 오디오 재생 
    */
    public void PlaySfx(Sfx_Dic sfx_dir) {
        for (int i = 0; i < sfxPlayers.Length; i++) {
            int loopindex = (i + channelIndex) % sfxPlayers.Length;

            if(sfxPlayers[loopindex].isPlaying) continue;

            channelIndex = loopindex;
            sfxPlayers[loopindex].clip = SfxClips[(int)sfx_dir];
            sfxPlayers[loopindex].Play();

            if(sfx_dir == Sfx_Dic.Rocket || sfx_dir == Sfx_Dic.PlayerTakeDamage || sfx_dir == Sfx_Dic.Attack) {
                sfxPlayers[loopindex].volume = 0.05f;
            } else {
                sfxPlayers[loopindex].volume = 0.4f;
            }
            break;
        }
    }
}
