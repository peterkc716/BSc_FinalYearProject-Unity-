using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UpgradeManager : MonoBehaviour {

    public GameObject upgradeTarget = null;
    int damage, range, rateoffire, money;
    public GameObject upgradePanel;
    public Text NText, FPText, RofText, RText, FPLText, RofLText, RLText;
    public Button DUpgrade, RofUpgrade, RUpgrade;
    public bool upgradeConnected = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	    if(upgradeTarget != null)
        {
            upgradeConnected = true;
            upgradePanel.SetActive(true);
        }
	}

    public void GetData()
    {

    }

    public void Close()
    {
        upgradeTarget = null;
        upgradeConnected = false;
        upgradePanel.SetActive(false);
    }
}
