using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MointorManager : MonoBehaviour {

    public static int Row = 20;
    public static int Col = 20;
    private double[,] TowerData = new double [Row,Col];

    public Text ALLTNT;
    public Text ETNT;
    public Text STNT;
    public Text STCPT;
    public Text ETCPT;
    public Text ATCPT;
    public Text ATEKT;
    public Text ETEKT;
    public Text STEKT;
    public Text EEWCPT;

    public int STN = 0;
    public int ETN = 0;
    public int ALLTN = 0;
    public double STCP = 0;
    public double ETCP = 0;
    public double ATCP = 0;
    public double ATEK = 0;
    public double ETEK = 0;
    public double STEK = 0;
    //public double EEWCP = 1;

    // Use this for initialization
    void Start () {
        STNT.text = STN.ToString();
        ETNT.text = ETN.ToString();
        ALLTNT.text = ALLTN.ToString();
        STCPT.text = STCP.ToString();
        ETCPT.text = ETCP.ToString();
        ATCPT.text = ATCP.ToString();
        ATEKT.text = ATEK.ToString();
        ETEKT.text = ETEK.ToString();
        STEKT.text = STEK.ToString();
        //EEWCPT.text = EEWCP.ToString();
    }
	
	// Update is called once per frame
	void Update () {
        CalcalateSTCP();
        CalcalateETCP();
        ATCP = STCP + ETCP;
        ATCPT.text = ATCP.ToString();
        CalcalateSTEK();
        CalcalateETEK();
        ATEK = STEK + ETEK;
        ATEKT.text = ATEK.ToString() + " / s";
    }

    public void AddTower (double id, double damage, double rateoffire, double range , double radius, double income)
    {
        int i = (int)id;
        TowerData[i,0] = id;
        TowerData[i, 1] = damage;
        TowerData[i, 2] = rateoffire;
        TowerData[i, 3] = range;
        TowerData[i, 4] = radius;
        TowerData[i, 5] = income;
        CalcalateTowerCP(i);
        if (radius > 0)
        {
            ETN += 1;
            ETNT.text = ETN.ToString();
            ALLTN += 1;
            ALLTNT.text = ALLTN.ToString();
        } else
        {
            STN += 1;
            STNT.text = STN.ToString();
            ALLTN += 1;
            ALLTNT.text = ALLTN.ToString();
        }
    }

    public void UpdateTower (double id, double damage, double rateoffire, double range)
    {
        int i = (int)id;
        TowerData[i, 1] = damage;
        TowerData[i, 2] = rateoffire;
        TowerData[i, 3] = range;
        double radius = TowerData[i, 4];
        CalcalateTowerCP(i);
        /*if (radius > 0)
        {
            ETN += 1;
            ETNT.text = ETN.ToString();
            ALLTN += 1;
            ALLTNT.text = ALLTN.ToString();
        }
        else
        {
            STN += 1;
            STNT.text = STN.ToString();
            ALLTN += 1;
            ALLTNT.text = ALLTN.ToString();
        }*/
    }

    void CalcalateTowerCP(double id)
    {
        int i = (int)id;
        double damage = TowerData[i, 1];
        double ROF = TowerData[i, 2];
        double range = TowerData[i, 3];
        double radius = TowerData[i, 4];

        double TowerCP = 0;
        double TowerCPR = 0;

        if(radius > 0)
        {
            TowerCP = (damage * radius) * (((range *2) / 10) / ROF);
            TowerCPR = Mathf.Round((float)TowerCP * 100) / 100;
            TowerData[i, 6] = TowerCPR;
        }
        else
        {
            TowerCP = (damage) * (((range * 2) / 10) / ROF);
            TowerCPR = Mathf.Round((float)TowerCP * 100) / 100;
            TowerData[i, 6] = TowerCPR;
        }
    }

    void CalcalateSTCP()
    {
        double TempCP = 0;
        for(int i = 0; i< TowerData.GetLength(0); i++)
        {
            if (TowerData[i, 0] != 0)
            {
                if(TowerData[i,4] == 0)
                {
                    TempCP += TowerData[i, 6];
                }
            }
            STCP = TempCP;
            STCPT.text = STCP.ToString();
        }  
    }

    void CalcalateETCP()
    {
        double TempCP = 0;
        for (int i = 0; i < TowerData.GetLength(0); i++)
        {
            if (TowerData[i, 0] != 0)
            {
                if (TowerData[i, 4] != 0)
                {
                    TempCP += TowerData[i, 6];
                }
            }
            ETCP = TempCP;
            ETCPT.text = ETCP.ToString();
        }
    }

    void CalcalateSTEK()
    {
        double TempEK = 0;
        double TempEKS = 0;
        double second = 60;
        double Temp = 0;
        double TempEKR = 0;
        for (int i = 0; i < TowerData.GetLength(0); i++)
        {
            if (TowerData[i, 0] != 0)
            {
                if (TowerData[i, 4] == 0)
                {
                    TempEK += TowerData[i, 2];
                    Temp = TempEK * second;
                    TempEKS += second / Temp;
                    TempEKR = Mathf.Round((float)TempEKS * 100) / 100;
                    TempEK = 0;
                }
            }
            STEK = TempEKR;
            STEKT.text = STEK.ToString() + " / s";
        }
    }

    void CalcalateETEK()
    {
        double TempEK = 0;
        double TempEKS = 0;
        double second = 60;
        double Temp = 0;
        double TempEKR = 0;
        for (int i = 0; i < TowerData.GetLength(0); i++)
        {
            if (TowerData[i, 0] != 0)
            {
                if (TowerData[i, 4] != 0)
                {
                    TempEK += TowerData[i, 2];
                    Temp = TempEK * second;
                    TempEKS += second / Temp * TowerData[i, 4];
                    TempEKR = Mathf.Round((float)TempEKS * 100) / 100;
                    TempEK = 0;
                }
            }
            ETEK = TempEKR;
            ETEKT.text = ETEK.ToString() + " / s";
        }
    }

    public void CalcaulateEnemyWave()
    {

    }
}
