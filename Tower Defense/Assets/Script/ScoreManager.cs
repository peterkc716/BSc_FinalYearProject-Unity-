using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour {

    public int lives = 20;
    public int money = 100;
    public int income = 0;

    private float heldTime = 0.0f;

    public Text moneyText;
    public Text lifeText;
    public Text incomeText;
    public double TowerCount = 1;

    public void LoseLife(int l = 1)
    {
        lives -= 1;
        if(lives <= 0)
        {
            GameOver();
        }
    }

    public void GameOver()
    {
        Debug.Log("Game over");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        moneyText.text = "Money: $" + money.ToString();
        lifeText.text = "Lives: " + lives.ToString();
        incomeText.text = "Income: " + income.ToString() + "/s";

        heldTime += Time.deltaTime;
        if(heldTime > 1)
        {
            money += income;
            heldTime -= (int)heldTime;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
}
