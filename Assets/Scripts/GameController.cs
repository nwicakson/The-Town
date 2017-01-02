using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameController : MonoBehaviour {

    public GameObject[] plot, hotel, house1, house2, house3, house4, player, profile;
    public Button[] button;
    public Text turn, money, dice, cityText, possessionText, upgrade, upgradePrice, payPrice;
    public GameObject dicePanel, buyPanel;
    private int playerTurn;
    private int[] playerMoney, playerPosition, possession, levelUpgrade;
    private bool[] checkUpgrade;
    private string[] upgradeText = { "Tanah", "Rumah 1", "Rumah 2", "Rumah 3", "Rumah 4", "Hotel" };
    private int[] upgradePriceText = { 100, 120, 130, 140, 150, 200 };
    private int[] payPriceText = { 80, 100, 110, 120, 130, 180 };
    private string[] city = { "Surabaya", "Malang", "Bandung", "Bukittinggi",
                              "Jakarta", "Palembang", "Semarang", "Irian Barat",
                              "Yogyakarta", "Ambon", "Bali", "Pontianak",
                              "Kupang", "Pekanbaru", "Kediri" };
    private int[] plotPrice = { 5000, 6000, 7000, 8000, 9000,
                                15000, 16000, 17000, 18000, 19000,
                                25000, 26000, 27000, 28000, 29000,
                                35000, 36000, 37000, 38000, 39000 };
    private Color32[] pion = { new Color32(130,130,0,255), new Color32(0,130,130,255) };
    private int win = 0;
    private bool[] lose;

	// Use this for initialization
	void Start () {
        DontDestroyOnLoad(this);
        playerMoney = new int[player.Length];
        playerPosition = new int[player.Length];
        lose = new bool[player.Length];
        possession = new int[plot.Length];
        levelUpgrade = new int[plot.Length];
        checkUpgrade = new bool[plot.Length];
        for (int i = 0; i < player.Length; i++)
        {
            playerMoney[i] = 150000;
            playerPosition[i] = 0;
            lose[i] = false;
        }
        for (int i = 0; i < plot.Length; i++)
        {
            possession[i] = 0;
            levelUpgrade[i] = -1;
            checkUpgrade[i] = false;
        }
        playerTurn = Random.Range(1, player.Length);
        money.text = playerMoney[playerTurn - 1].ToString();
        button[1].interactable = false;
        button[2].interactable = false;
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void PlayDice()
    {
        int step = Random.Range(1, 6);
        dice.text = step.ToString();
        Move(playerTurn - 1, step);
        CheckPossession(playerTurn - 1);
        button[0].interactable = false;
        button[1].interactable = true;
    }

    public void EndTurn()
    {
        dicePanel.SetActive(true);
        buyPanel.SetActive(false);
        do
        {
            playerTurn++;
            if (playerTurn > player.Length)
                playerTurn -= player.Length;
        }
        while (lose[playerTurn - 1]);

        string text;
        if (playerTurn == 1)
            text = "Pahlawan";
        else
            text = "Penjajah";
        turn.text = "Player " + playerTurn.ToString() + " - " + text;
        money.text = playerMoney[playerTurn - 1].ToString();
        for(int i=0;i<player.Length;i++)
        {
            if (playerTurn - 1 == i)
                profile[i].SetActive(true);
            else
                profile[i].SetActive(false);
        }
        dice.text = "0";
        button[0].interactable = true;
        button[1].interactable = false;
        button[2].interactable = false;
    }

    private void Move(int playerIndex, int step)
    {
        for(int i=0;i< step;i++)
        {
            playerPosition[playerIndex] ++;
            if (playerPosition[playerIndex] >= plot.Length)
            {
                playerPosition[playerIndex] -= plot.Length;
                playerMoney[playerIndex] += 5000;
                money.text = playerMoney[playerIndex].ToString();
            }
            player[playerIndex].transform.position = plot[playerPosition[playerIndex]].transform.position;
        }
    }

    private void CheckPossession(int playerIndex)
    {
        if (playerPosition[playerIndex] != 0)
        {
            dicePanel.SetActive(false);
            buyPanel.SetActive(true);

            int check = possession[playerPosition[playerIndex]];
            if (check == 0 || check == playerIndex + 1)
            {
                button[2].interactable = true;

                cityText.text = city[playerPosition[playerIndex] - 1];
                if (check == 0)
                    possessionText.text = "Milik : -";
                else
                {
                    checkUpgrade[playerPosition[playerIndex]] = true;
                    possessionText.text = "Milik : Player " + (playerIndex + 1);
                }

                int levelTemp = levelUpgrade[playerPosition[playerIndex]] + 1;
                int tempPrice;
                if (levelTemp <= 5)
                {
                    upgrade.text = upgradeText[levelTemp];

                    tempPrice = plotPrice[playerPosition[playerIndex]] * upgradePriceText[levelTemp] / 100;
                    upgradePrice.text = tempPrice.ToString();

                    tempPrice = plotPrice[playerPosition[playerIndex]] * payPriceText[levelTemp] / 100;
                    payPrice.text = tempPrice.ToString();
                }
                else
                {
                    upgrade.text = "Terbeli";
                    upgradePrice.text = "";
                    tempPrice = plotPrice[playerPosition[playerIndex]] * payPriceText[levelTemp - 1] / 100;
                    payPrice.text = tempPrice.ToString();
                    button[2].interactable = false;
                }

                if(playerMoney[playerIndex] - tempPrice < 0)
                    button[2].interactable = false;
            }
            else
            {
                button[2].interactable = false;
                cityText.text = city[playerPosition[playerIndex] - 1];
                possessionText.text = "Milik : Player " + possession[playerPosition[playerIndex]];
                upgrade.text = "";
                upgradePrice.text = "";
                int tempPrice = plotPrice[playerPosition[playerIndex]] * payPriceText[levelUpgrade[playerPosition[playerIndex]]] / 100;
                payPrice.text = tempPrice.ToString();
                tempPrice = plotPrice[playerPosition[playerIndex]] * payPriceText[levelUpgrade[playerPosition[playerIndex]]] / 100;

                playerMoney[check - 1] += tempPrice;
                playerMoney[playerIndex] -= tempPrice;
                money.text = playerMoney[playerIndex].ToString();
                if (playerMoney[playerIndex] <= 0)
                {
                    lose[playerIndex] = true;
                    int count = 0;
                    for (int i = 0; i < player.Length; i++)
                    {
                        if (lose[i])
                            count++;
                        else
                            win = i+1;
                    }
                    if(count == player.Length - 1)
                    {
                        SceneManager.LoadScene("win lose");
                    }
                }
                    
            }
        }
    }

    public void BuyUpgrade()
    {
        int levelTemp = levelUpgrade[playerPosition[playerTurn - 1]] + 1;
        if (checkUpgrade[playerPosition[playerTurn - 1]] == false && levelTemp == 0)
        {
            button[2].interactable = false;
            plot[playerPosition[playerTurn - 1]].GetComponent<Image>().color = pion[playerTurn - 1];
            possession[playerPosition[playerTurn - 1]] = playerTurn;
        }
        int tempPrice = plotPrice[playerPosition[playerTurn - 1]] * upgradePriceText[levelTemp] / 100;
        playerMoney[playerTurn - 1] -= tempPrice;
        money.text = playerMoney[playerTurn - 1].ToString();

        if (levelTemp == 1)
            house1[playerPosition[playerTurn - 1] - 1].SetActive(true);
        else if (levelTemp == 2)
            house2[playerPosition[playerTurn - 1] - 1].SetActive(true);
        else if (levelTemp == 3)
            house3[playerPosition[playerTurn - 1] - 1].SetActive(true);
        else if (levelTemp == 4)
            house4[playerPosition[playerTurn - 1] - 1].SetActive(true);

        if (levelTemp > 4)
        {
            upgrade.text = "Hotel";
            upgradePrice.text = "Terbeli";
            tempPrice = plotPrice[playerPosition[playerTurn - 1]] * payPriceText[levelTemp - 1] / 100;
            payPrice.text = tempPrice.ToString();
            button[2].interactable = false;
            house1[playerPosition[playerTurn - 1] - 1].SetActive(false);
            house2[playerPosition[playerTurn - 1] - 1].SetActive(false);
            house3[playerPosition[playerTurn - 1] - 1].SetActive(false);
            house4[playerPosition[playerTurn - 1] - 1].SetActive(false);
            hotel[playerPosition[playerTurn - 1] - 1].SetActive(true);
        }
        else
        {
            possessionText.text = "Milik : Player " + possession[playerPosition[playerTurn - 1]];

            levelUpgrade[playerPosition[playerTurn - 1]]++;
            levelTemp = levelUpgrade[playerPosition[playerTurn - 1]] + 1;
            upgrade.text = upgradeText[levelTemp];

            tempPrice = plotPrice[playerPosition[playerTurn - 1]] * upgradePriceText[levelTemp] / 100;
            upgradePrice.text = tempPrice.ToString();

            tempPrice = plotPrice[playerPosition[playerTurn - 1]] * payPriceText[levelTemp] / 100;
            payPrice.text = tempPrice.ToString();
        }

        if(playerMoney[playerTurn - 1] - tempPrice < 0)
            button[2].interactable = false;
    }

    public int getWin()
    {
        return win;
    }

    public int getMoneyWin()
    {
        return playerMoney[win - 1];
    }

    public void AddSound(AudioClip sfx)
    {
        AudioManager sound = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        sound.PlaySoundEffect(sfx);
    }
}
