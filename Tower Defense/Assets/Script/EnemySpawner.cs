using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour {

    float spawnCD = 1.5f;
    float spawnCDremain = 10;

    [System.Serializable]
    public class WaveComponent
    {
        public GameObject enemyPrefab;
        public int num;
        [System.NonSerialized]
        public int spawned = 0;
    }

    public WaveComponent[] waveComps;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        spawnCDremain -= Time.deltaTime;
        if(spawnCDremain < 0)
        {
            spawnCDremain = spawnCD;

            bool didSpawn = false;

            foreach(WaveComponent wc in waveComps)
            {
                if(wc.spawned < wc.num)
                {
                    wc.spawned++;
                    Instantiate(wc.enemyPrefab, this.transform.position, this.transform.rotation);

                    didSpawn = true;
                    break;
                }
            }

            if(didSpawn == false)
            {
                if(transform.parent.childCount > 1)
                {
                    transform.parent.GetChild(1).gameObject.SetActive(true);
                } else
                {
                    
                }
                Destroy(gameObject);
            }
        }
	}
}
