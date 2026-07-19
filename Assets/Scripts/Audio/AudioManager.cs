using UnityEngine;

namespace LostMonkey.Audio
{
    /// <summary>
    /// Persistent, lightweight audio hub. Plays one-shot SFX and a single looping
    /// music track. Reachable via <see cref="Instance"/>. Attach to a GameObject
    /// with two AudioSources (one for SFX, one for music).
    /// </summary>
    [RequireComponent(typeof(AudioSource))]
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance { get; private set; }

        [Header("Sources")]
        [SerializeField] private AudioSource sfxSource;
        [SerializeField] private AudioSource musicSource;

        [Header("Default clips (optional)")]
        [SerializeField] private AudioClip jumpClip;
        [SerializeField] private AudioClip collectClip;
        [SerializeField] private AudioClip deathClip;
        [SerializeField] private AudioClip checkpointClip;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);

            if (sfxSource == null)
            {
                sfxSource = GetComponent<AudioSource>();
            }
        }

        public void PlaySfx(AudioClip clip, float volume = 1f)
        {
            if (clip != null && sfxSource != null)
            {
                sfxSource.PlayOneShot(clip, volume);
            }
        }

        public void PlayJump() => PlaySfx(jumpClip);
        public void PlayCollect() => PlaySfx(collectClip);
        public void PlayDeath() => PlaySfx(deathClip);
        public void PlayCheckpoint() => PlaySfx(checkpointClip);

        public void PlayMusic(AudioClip track, bool loop = true)
        {
            if (musicSource == null || track == null)
            {
                return;
            }
            musicSource.clip = track;
            musicSource.loop = loop;
            musicSource.Play();
        }

        public void SetMusicVolume(float volume)
        {
            if (musicSource != null)
            {
                musicSource.volume = Mathf.Clamp01(volume);
            }
        }
    }
}
