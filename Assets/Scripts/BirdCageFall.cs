using UnityEngine;
using System.Collections;

using UsefulThings;

public class BirdCageFall : MonoBehaviour {

    public Curve curve;

    private float startTime;
    private Vector3 position;

	void Start () {
        startTime = Time.time;
        position = transform.position;
	}
	
	void Update () {
        float t = Time.time - startTime;
        position.y = curve.Evaluate(Time.time - startTime);
        transform.position = position;

        if (t > 1)
        {
            SfxManager.PlaySfx(2);
            this.enabled = false;
        }
	}
}
