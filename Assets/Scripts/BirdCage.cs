using UnityEngine;
using System.Collections;

public class BirdCage : MonoBehaviour {

    public float health;
    public float damage { get; set; }
	
	void Update () {
	    if (damage >= health)
        {
            Destroy(gameObject);
        }
	}
}
