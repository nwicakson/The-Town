using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MainMenuController : MonoBehaviour {

    public AudioClip sfx;
    private AudioManager soundManager;

    // Use this for initialization
    void Start () {
        soundManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Play()
    {
        soundManager.PlaySoundEffect(sfx);
        SceneManager.LoadScene("loading");
    }

    public void Exit()
    {
        soundManager.PlaySoundEffect(sfx);
        Application.Quit();
    }
}
