using UnityEngine;
using System.Collections;

public class TowerSpot : MonoBehaviour {

    void OnMouseUp()
    {
        Debug.Log("TowerSpot clicked.");

        BuildingManager bm = GameObject.FindObjectOfType<BuildingManager>();
        if(bm.selectedTower != null)
        {
            ScoreManager sm = GameObject.FindObjectOfType<ScoreManager>();
            if(sm.money < bm.selectedTower.GetComponent<Tower>().cost)
            {
                Debug.Log("Not enought money");
                return;
            }

            sm.money -= bm.selectedTower.GetComponent<Tower>().cost;

            Instantiate(bm.selectedTower, transform.parent.position, transform.parent.rotation);
            sm.TowerCount += 1;
            Destroy(transform.parent.gameObject);
        }
    }
}
