using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour
{
    private AudioSource audioSource;
    private float progress;
    public Transform[] points;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (Microphone.devices.Length > 0)
        {
            audioSource.clip = Microphone.Start(Microphone.devices[0], true, 1, 44100);
            audioSource.Play();
        }
        else
        {
            Debug.LogError("No microphones found");
        }
    }

    void Update()
    {
        float[] spectrum = new float[1024];
        audioSource.GetSpectrumData(spectrum, 0, FFTWindow.Rectangular);

        for (int i = 1; i < spectrum.Length - 1; i++)
        {
            Debug.DrawLine(new Vector3(i - 1, spectrum[i] * 1000, 0), new Vector3(i, spectrum[i + 1] * 1000, 0), Color.red);
        }
        if (Mathf.Max(spectrum) > 0.009f)
        {
            progress += 0.05f * Time.deltaTime;
        }
        transform.position = GetPosition();
    }

    private Vector3 GetPosition()
    {
        return points[0].position * (1 - progress) + points[1].position * progress;
    }

    private void OnTriggerEnter()
    {
        Debug.Log("Game Over");
    }
}