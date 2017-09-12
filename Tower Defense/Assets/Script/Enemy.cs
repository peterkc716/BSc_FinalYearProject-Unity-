using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

    GameObject pathRoute;

    Transform targetPathNode;
    int pathNodeNo = 0;

    public float health = 1f;
    public int reward = 1;
    float speed = 10f;
    bool dead = false;

	// Use this for initialization
	void Start () {
        pathRoute = GameObject.Find("Path");
	}
	
    void NextPath()
    {
        if(pathNodeNo < pathRoute.transform.childCount)
        {
            targetPathNode = pathRoute.transform.GetChild(pathNodeNo);
            pathNodeNo++;
        } else
        {
            targetPathNode = null;
            ReachGoal();
        }
    }
	// Update is called once per frame
	void Update () {
	    if (targetPathNode == null)
        {
            NextPath();
            if(targetPathNode == null)
            {
                ReachGoal();
                return;
            }
        }

        Vector3 direction = targetPathNode.position - this.transform.localPosition;
        float frameDist = speed * Time.deltaTime;

        if (direction.magnitude <= frameDist)
        {
            targetPathNode = null;
        } else
        {
            transform.Translate(direction.normalized * frameDist, Space.World);
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            this.transform.rotation = Quaternion.Lerp(this.transform.rotation, targetRotation, Time.deltaTime * 5);
        }
	}

    void ReachGoal()
    {
        if (dead == false)
        {
            GameObject.FindObjectOfType<ScoreManager>().LoseLife();
            dead = true;
        }
        Destroy(gameObject);
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        if(health <= 0)
        {
            Dead();
        }
    }

    public void Dead()
    {
        GameObject.FindObjectOfType<ScoreManager>().money += reward;
        Destroy(gameObject);
    }
}
