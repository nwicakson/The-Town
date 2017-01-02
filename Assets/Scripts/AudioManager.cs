using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour {

    public AudioSource backgroundMusic;
    public AudioSource soundEffect;
    private static AudioManager instance = null;
    private float lowPitchRange = .95f, highPitchRange = 1.05f;

    // Use this for initialization
    void Start () {
	
	}

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update () {
    }

    public void PlayBackground(AudioClip clip)
    {
        backgroundMusic.clip = clip;
        backgroundMusic.Play();
    }

    public void PlaySoundEffect(AudioClip clip)
    {
        float randomPitch = Random.Range(lowPitchRange, highPitchRange);
        soundEffect.pitch = randomPitch;
        soundEffect.clip = clip;
        soundEffect.Play();
    }
}
