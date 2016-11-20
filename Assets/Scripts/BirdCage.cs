using UnityEngine;
using System.Collections;

using UsefulThings;

public class BirdCage : MonoBehaviour {

    public float health;
    public float damage { get; set; }
    private float lastDamage;
	
	void Update () {
        if (damage - lastDamage > 0.2f)
        {
            lastDamage = damage;
            SfxManager.PlaySfx(3);
        }
	    if (damage >= health)
        {
            SfxManager.PlaySfx(4);
            Destroy(gameObject);
        }
	}

    void OnTriggerEnter()
    {
        SfxManager.PlaySfx(3);
    }
}
