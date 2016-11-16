using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using UsefulThings;

public class PlayerController : MonoBehaviour
{

    public Transform[] points;
    public GameObject gameOverEffect;
    public GameObject animatedVisual;
    public GameObject stillVisual;
    
    private float progress;
    private bool gameOver;
    private bool moving;

    private AudioClip recording;

    void Start()
    {
        recording = new AudioClip();
        if (Microphone.devices.Length > 0)
        {
            recording = Microphone.Start(Microphone.devices[0], true, 1, 44100);
        }
        else
        {
            Debug.LogError("No microphones found");
        }
    }

    void Update()
    {
        if (gameOver) return;
        
        float[] data = new float[44100];
        recording.GetData(data, 0);

        int micPosition = Microphone.GetPosition(Microphone.devices[0]);
        float maxAmplitude = 0;
        for (int i = 0; i < 44100; i++)
        {
            if ((44100 + micPosition - i) % 44100 < 4410)
            {
                if (Mathf.Abs(data[i]) > maxAmplitude)
                {
                    maxAmplitude = Mathf.Abs(data[i]);
                }
            }
        }

        moving = maxAmplitude > 0.4f || Input.GetKey(KeyCode.G);

        animatedVisual.SetActive(moving);
        stillVisual.SetActive(!moving);
        if (moving)
        {
            progress += 0.05f * Time.deltaTime;
        }

        transform.position = GetPosition();
    }

    private Vector3 GetPosition()
    {
        return points[0].position * (1 - progress) + points[1].position * progress;
    }

    private void OnTriggerStay()
    {
        if (!moving) return;

        if (!gameOver)
        {
            SfxManager.PlaySfx(0);
            gameOver = true;
            gameOverEffect.SetActive(true);
        }
    }
}