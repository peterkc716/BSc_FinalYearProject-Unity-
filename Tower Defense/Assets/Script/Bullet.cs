using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {

    public float speed = 40f;
    public Transform target;
    public float damage = 1f;
    public float radius = 0;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	    if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        Vector3 direction = target.position - this.transform.localPosition;

        float framedist = speed * Time.deltaTime;

        if(direction.magnitude <= framedist)
        {
            BulletHit();
        } else
        {
            transform.Translate(direction.normalized * framedist, Space.World);
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            this.transform.rotation = Quaternion.Lerp(this.transform.rotation, targetRotation, Time.deltaTime * 5);
        }
	}

    void BulletHit()
    {
        if(radius == 0)
        {
            target.GetComponent<Enemy>().TakeDamage(damage);
        } else
        {
            Collider[] cols = Physics.OverlapSphere(transform.position, radius);

            foreach(Collider c in cols)
            {
                Enemy e = c.GetComponent<Enemy>();
                if(e != null)
                {
                    e.GetComponent<Enemy>().TakeDamage(damage);
                }
            }
        }

        Destroy(gameObject);
    }
}
