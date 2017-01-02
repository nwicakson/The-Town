using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class WinLoseController : MonoBehaviour {

    private GameController gameScript;
    public Text winText, moneyWinText;
    public GameObject[] karakter;

    // Use this for initialization
    void Start () {
        gameScript = GameObject.Find("Script").GetComponent<GameController>();
        if(gameScript.getWin() == 1)
            winText.text = "Player " + gameScript.getWin()  + " - Pahlawan Menang";
        else
            winText.text = "Player " + gameScript.getWin() + " - Penjajah Menang";
        moneyWinText.text = "Uang akhir : " + gameScript.getMoneyWin().ToString();
        for(int i=0;i<karakter.Length;i++)
        {
            if (gameScript.getWin() - 1 == i)
                karakter[i].SetActive(true);
            else
                karakter[i].SetActive(false);
        }   

    }
	
	// Update is called once per frame
	void Update () {
	    
	}

    public void Ok()
    {
        SceneManager.LoadScene("main menu");
    }
}
