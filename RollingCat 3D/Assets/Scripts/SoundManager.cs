using UnityEngine;

public class SoundManager : MSingleton<SoundManager>
{
    public AudioClip gameOverClip;
    public AudioClip victoryClip;
    public AudioClip collectClip;
    public AudioClip swipeClip;
    public AudioClip toggleClip;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        if (!PlayerPrefs.HasKey("Sound"))
            PlayerPrefs.SetInt("Sound", 1);
        
    }

    public void PlaySound(AudioClip audioClip)
    {
        if(PlayerPrefs.GetInt("Sound") == 1)
        {
            GameObject soundGameObject = new GameObject(audioClip.name);
            AudioSource audioSource = soundGameObject.AddComponent<AudioSource>();

            audioSource.PlayOneShot(audioClip);
            Destroy(soundGameObject, audioClip.length);
        }
    }

    public void ChangeSoundStatus()
    {
        PlayerPrefs.SetInt("Sound", PlayerPrefs.GetInt("Sound") == 0 ? 1 : 0);
    }
}