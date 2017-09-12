using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Tower : MonoBehaviour
{

    Transform turretTransform;

    public double range = 10f;
    public GameObject bulletPrefab;

    public int cost = 5;
    public int income = 0;
    public int damagelevel = 1;
    public int roflevel = 1;
    public int rangelevel = 1;

    public double fireCooldown = 0.5;
    double fireCooldownLeft = 0;

    public float damage = 1;
    public float radius = 0;

    bool connectUpgrade = false;

    public Button FPupgrade, Rofupgrade, Rupgrade;

    bool upgrade = false;
    bool FPupgradeS = false;
    bool RofupgradeS = false;
    bool RupgradeS = false;

    public int FPupgradeC = 15;
    public int RofupgradeC = 2;
    public int RupgradeC = 15;

    public double TowerID = 0;


    // Use this for initialization
    void Start()
    {
        TowerID = GameObject.FindObjectOfType<ScoreManager>().TowerCount;
        turretTransform = transform.Find("Turret");
        GameObject.FindObjectOfType<ScoreManager>().income += income;
        MointorManager mm = GameObject.FindObjectOfType<MointorManager>();
        if (income == 0)
        {
            mm.AddTower(TowerID, damage, fireCooldown, range, radius, income);
        }
    }

    // Update is called once per frame
    void Update()
    {
        Enemy[] enemies = GameObject.FindObjectsOfType<Enemy>();

        Enemy nearestEnemy = null;
        float dist = Mathf.Infinity;

        foreach (Enemy e in enemies)
        {
            float d = Vector3.Distance(this.transform.position, e.transform.position);
            if (nearestEnemy == null || d < dist)
            {
                nearestEnemy = e;
                dist = d;
            }
        }

        if (nearestEnemy == null)
        {
            Debug.Log("No enemy");
            return;
        }

        Vector3 direction = nearestEnemy.transform.position - this.transform.position;

        Quaternion lookRotate = Quaternion.LookRotation(direction);

        turretTransform.rotation = Quaternion.Euler(0, lookRotate.eulerAngles.y, 0);

        fireCooldownLeft -= Time.deltaTime;
        if(fireCooldownLeft <= 0 && direction.magnitude <= range)
        {
            fireCooldownLeft = fireCooldown;
            Shoot(nearestEnemy);
        }

        if (connectUpgrade == true)
        {
            ManageUpgrade();
        }
    }

    void Shoot(Enemy e)
    {
        GameObject bulletPF = (GameObject)Instantiate(bulletPrefab, this.transform.position, this.transform.rotation);

        Bullet b = bulletPF.GetComponent<Bullet>();
        b.target = e.transform;
        b.damage = damage;
        b.radius = radius;
    }

    void OnMouseUp()
    {
        Debug.Log("Tower clicked.");
        Debug.Log(GetInstanceID());

        UpgradeManager um = GameObject.FindObjectOfType<UpgradeManager>();
        um.upgradeTarget = this.gameObject;
        if (radius > 0)
        {
            um.NText.text = "Explody Tower";
            um.FPText.text = "Fire Power: " + damage.ToString();
            um.FPLText.text = "Level: " + damagelevel.ToString() + " (" + FPupgradeC + ")";
            um.RofText.text = "Rate of Fire: " + fireCooldown.ToString() + " s/shot";
            um.RofLText.text = "Level: " + roflevel.ToString() + " (" + RofupgradeC + ")";
            um.RText.text = "Range: " + range.ToString();
            um.RLText.text = "Level: " + rangelevel.ToString() + " (" + RupgradeC + ")";
        }
        else if (income > 0)
        {
            um.NText.text = "Money Farm";
            um.FPText.text = "Income: " + income.ToString() + "/s";
            um.FPLText.text = "Level: " + damagelevel.ToString();
            um.RofText.text = "Rate of Fire: N/A";
            um.RofLText.text = "Level: N/A";
            um.RText.text = "Range: N/A";
            um.RLText.text = "Level: N/A";

        }
        else
        {
            um.NText.text = "Sniper Tower";
            um.FPText.text = "Fire Power: " + damage.ToString();
            um.FPLText.text = "Level: " + damagelevel.ToString() + " (" + FPupgradeC + ")";
            um.RofText.text = "Rate of Fire: " + fireCooldown.ToString() + " s/shot";
            um.RofLText.text = "Level: " + roflevel.ToString() + " (" + RofupgradeC + ")";
            um.RText.text = "Range: " + range.ToString();
            um.RLText.text = "Level: " + rangelevel.ToString() + " (" + RupgradeC + ")";
        }
        connectUpgrade = true;
    }

    void ManageUpgrade()
    {
        UpgradeManager um = GameObject.FindObjectOfType<UpgradeManager>();
        if (um.upgradeTarget == this.gameObject)
        {
            if (FPupgrade == null)
            {
                if (FPupgradeS == false)
                {
                    FPupgrade = um.DUpgrade;
                    FPupgrade.onClick.RemoveAllListeners();
                    FPupgrade.onClick.AddListener(FireUpgrade);
                    FPupgradeS = true;
                }
            }
            if (Rofupgrade == null)
            {
                if (RofupgradeS == false)
                {
                    Rofupgrade = um.RofUpgrade;
                    Rofupgrade.onClick.RemoveAllListeners();
                    Rofupgrade.onClick.AddListener(RateUpgrade);
                    RofupgradeS = true;
                }
            }
            if (Rupgrade == null)
            {
                if (RupgradeS == false)
                {
                    Rupgrade = um.RUpgrade;
                    Rupgrade.onClick.RemoveAllListeners();
                    Rupgrade.onClick.AddListener(RangeUpgrade);
                    RupgradeS = true;
                }
            }

        }
        if (um.upgradeConnected == false)
        {
            connectUpgrade = false;
            if (FPupgrade != null)
            {
                FPupgrade.onClick.RemoveAllListeners();
                Rofupgrade.onClick.RemoveAllListeners();
                Rupgrade.onClick.RemoveAllListeners();
            }
            FPupgradeS = false;
            FPupgrade = null;
            RofupgradeS = false;
            Rofupgrade = null;
            RupgradeS = false;
            Rupgrade = null;
        }
    }

    void FireUpgrade()
    {
        UpgradeManager um = GameObject.FindObjectOfType<UpgradeManager>();
        ScoreManager sm = GameObject.FindObjectOfType<ScoreManager>();
        MointorManager mm = GameObject.FindObjectOfType<MointorManager>();
        if (sm.money < FPupgradeC)
        {
            Debug.Log("Not enough money!");
            return;
        }
        else
        {
            upgrade = true;
        }

        if (damagelevel < 5 && income == 0)
        {
            if (upgrade == true)
            {
                damagelevel += 1;
                damage += 1;
                um.FPText.text = "Fire Power: " + damage.ToString();
                um.FPLText.text = "Level: " + damagelevel.ToString() + " (" + FPupgradeC + ")";
                sm.money -= FPupgradeC;
                mm.UpdateTower(TowerID, damage, fireCooldown, range);
                upgrade = false;
            }
        }
        else if (damagelevel < 5 && income > 0)
        {
            if (upgrade == true)
            {
                /*damagelevel += 1;
                income += 1;
                um.FPText.text = "Income: " + income.ToString() + "/s";
                um.FPLText.text = "Level: " + damagelevel.ToString();*/
                upgrade = false;
            }
        }
    }

    void RateUpgrade()
    {
        UpgradeManager um = GameObject.FindObjectOfType<UpgradeManager>();
        ScoreManager sm = GameObject.FindObjectOfType<ScoreManager>();
        MointorManager mm = GameObject.FindObjectOfType<MointorManager>();
        if (sm.money < RofupgradeC)
        {
            Debug.Log("Not enough money!");
            return;
        }
        else
        {
            upgrade = true;
        }
        if (roflevel < 5 && income == 0)
        {
            if (upgrade == true)
            {
                roflevel += 1;
                fireCooldown -= 0.2;
                um.RofText.text = "Rate of Fire: " + fireCooldown.ToString() + " s/shot";
                um.RofLText.text = "Level: " + roflevel.ToString() + " (" + RofupgradeC + ")";
                sm.money -= RofupgradeC;
                mm.UpdateTower(TowerID, damage, fireCooldown, range);
                upgrade = false;
            }
        }
        else if (roflevel < 5 && income > 0)
        {
            if (upgrade == true)
            {
                upgrade = false;
            }
        }
    }

    void RangeUpgrade()
    {
        UpgradeManager um = GameObject.FindObjectOfType<UpgradeManager>();
        ScoreManager sm = GameObject.FindObjectOfType<ScoreManager>();
        MointorManager mm = GameObject.FindObjectOfType<MointorManager>();
        if (sm.money < RupgradeC)
        {
            Debug.Log("Not enough money!");
            return;
        }
        else
        {
            upgrade = true;
        }
        if (rangelevel < 4 && income == 0)
        {
            if (upgrade == true)
            {
                rangelevel += 1;
                range += 5;
                um.RText.text = "Range: " + range.ToString();
                um.RLText.text = "Level: " + rangelevel.ToString() + " (" + RupgradeC + ")";
                sm.money -= RupgradeC;
                mm.UpdateTower(TowerID, damage, fireCooldown, range);
                upgrade = false;
            }
        }
        else if (rangelevel < 5 && income > 0)
        {
            if (upgrade == true)
            {
                upgrade = false;
            }
        }
    }
}
