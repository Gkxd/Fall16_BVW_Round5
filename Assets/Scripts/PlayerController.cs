﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using UsefulThings;

public class PlayerController : MonoBehaviour
{

    public Transform[] points;
    public GameObject gameOverEffect;
    public GameObject animatedVisual;
    public GameObject stillVisual;

    [Header("Cutscene References")]
    public GameObject spotLights;

    private float progress;
    private bool gameOver;
    private bool moving;

    private bool hasPlayedCutscene;
    private bool cutsceneActive;

    private AudioClip recording;

    private BirdCage birdCage;

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
        if (gameOver || cutsceneActive) return;
        
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

        if (birdCage == null)
        {
            moving = maxAmplitude > 0.4f || Input.GetKey(KeyCode.G);

            animatedVisual.SetActive(moving);
            stillVisual.SetActive(!moving);
            if (moving)
            {
                progress += 0.025f * Time.deltaTime;
            }
            if (progress >= 0.5f && !hasPlayedCutscene)
            {
                cutsceneActive = hasPlayedCutscene = true;
            }

            transform.position = GetPosition();
        }
        else
        {
            birdCage.damage += maxAmplitude * Time.deltaTime;
            if (Input.GetKeyDown(KeyCode.G))
            {
                birdCage.damage += 0.2f;
            }
        }
    }

    private IEnumerator Cutscene()
    {
        yield return new WaitForSeconds(1);
        spotLights.SetActive(false);

        yield return new WaitForSeconds(1);

        cutsceneActive = false;
        yield return null;
    }

    private Vector3 GetPosition()
    {
        if (progress < 0.5)
        {
            float p = progress * 2;
            return points[0].position * (1 - p) + points[1].position * p;
        }
        else
        {
            float p = (progress - 0.5f) * 2;
            return points[1].position * (1 - p) + points[0].position * p;
        }
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

    private void OnTriggerEnter(Collider c)
    {
        birdCage = c.gameObject.GetComponent<BirdCage>();
        moving = false;
    }
}