using UnityEngine;
using System.Collections;

using UsefulThings;

public class ClosingDoor : MonoBehaviour {

    public Curve curve;
    public GameObject gameOverEffect;
    public AudioSource backgroundMusic;

    private float startTime;
    private Vector3 position;

	void Start () {
        startTime = Time.time;
        position = transform.position;
	}
	
	void Update () {
        float t = Time.time - startTime;
        position.y = curve.Evaluate(t);
        transform.position = position;

        if (t >= 40)
        {
            SfxManager.StopLoop(0);
            SfxManager.PlaySfx(6);
            gameOverEffect.SetActive(true);
            backgroundMusic.Stop();
            this.enabled = false;
        }
	}
}
