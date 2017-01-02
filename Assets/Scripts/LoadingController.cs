using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LoadingController : MonoBehaviour {

	// Use this for initialization
	void Start () {
        StartCoroutine(StartScene());
    }

    IEnumerator StartScene()
    {
        yield return new WaitForSeconds(.1f);
        SceneManager.LoadScene("monopoli");
    }
}
