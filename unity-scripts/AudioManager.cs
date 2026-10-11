using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("Mixer Groups")]
    public AudioMixerGroup menuGroup;
    public AudioMixerGroup gameGroup;
    public AudioMixerGroup pauseGroup;
    public AudioMixerGroup sfxGroup;

    [Header("Music Clips")]
    public AudioClip menuMusic;
    public AudioClip gameMusic;
    public AudioClip pauseMusic;

    [Header("SFX Clips")]
    public AudioClip clickSFX;
    public AudioClip collisionSFX;
    public AudioClip goalSFX;
    public AudioClip wonSFX;
    public AudioClip lostSFX;

    private AudioSource musicSource;
    private AudioSource sfxSource;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            // Nastavimo dva AudioSource
            musicSource = gameObject.AddComponent<AudioSource>();
            musicSource.loop = true;
            musicSource.playOnAwake = false;

            sfxSource = gameObject.AddComponent<AudioSource>();
            sfxSource.loop = false;
            sfxSource.playOnAwake = false;

            // Pripni jih na mixer
            musicSource.outputAudioMixerGroup = menuGroup; // sprva menu
            sfxSource.outputAudioMixerGroup = sfxGroup;
        }
        else
            Destroy(gameObject);
    }

    /// <summary>
    /// Predvaja glasbo (loop) v izbranem mixer groupu.
    /// </summary>
    public void PlayMusic(AudioClip clip, AudioMixerGroup group)
    {
        musicSource.outputAudioMixerGroup = group;
        if (musicSource.clip == clip) return;
        musicSource.clip = clip;
        musicSource.Play();
    }

    /// <summary>
    /// Predvaja enkraten SFX.
    /// </summary>
    public void PlaySFX(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }
}
