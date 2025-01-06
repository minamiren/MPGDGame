using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SoundType
{ 
    //For Player
    FOOTSTEP,
    JUMP,
    LAND,
    GUNPREPARE,
    GUNSHOOT,
    PLAYERHURT,

    //Whole Game
    ENEMIEDIE,
    GAMEOVER,
    GAMEWIN,
    REWARD,
    GAMESTART
}
[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioClip[] soundList;
    private static SoundManager instance;
    private AudioSource audioSource;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject); // Prevent duplicates
        }
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    public static void PlaySound(SoundType sound, float volume = 1f)
    {
        if (instance == null)
        {
            Debug.LogError("SoundManager instance not found!");
            return;
        }

        instance.audioSource.PlayOneShot(instance.soundList[(int)sound], volume);

    }
}
