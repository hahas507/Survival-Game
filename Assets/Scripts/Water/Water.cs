using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour
{
    [SerializeField]
    private float waterDrag;

    private float originDrag;

    [SerializeField]
    private Color waterColor;

    [SerializeField]
    private float waterFogDensity;

    [SerializeField]
    private Color waterNightColor;

    [SerializeField]
    private float waterNightFogDensity;

    private Color originWaterColor;
    private float originFogDensity;

    [SerializeField]
    private Color originNightColor;

    [SerializeField]
    private float originNightFogDensity;

    [SerializeField]
    private string sound_WaterOut;

    [SerializeField]
    private string sound_WaterIn;

    [SerializeField]
    private string sound_WaterBreath;

    [SerializeField]
    private float breatheTime;

    private float currentBreatheTime;

    private void Start()
    {
        originWaterColor = RenderSettings.fogColor;
        originFogDensity = RenderSettings.fogDensity;

        originDrag = 0;
    }

    private void Update()
    {
        if (GameManager.isWater)
        {
            currentBreatheTime += Time.deltaTime;
            if (currentBreatheTime >= breatheTime)
            {
                SoundManager.instance.PlaySE(sound_WaterBreath);
                currentBreatheTime = 0;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            GetInWater(other);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            GetOutWater(other);
        }
    }

    private void GetInWater(Collider _player)
    {
        SoundManager.instance.PlaySE(sound_WaterIn);
        GameManager.isWater = true;
        _player.transform.GetComponent<Rigidbody>().drag = waterDrag;
        if (!GameManager.isNight)
        {
            RenderSettings.fogColor = waterColor;
            RenderSettings.fogDensity = waterFogDensity;
        }
        else
        {
            RenderSettings.fogColor = waterNightColor;
            RenderSettings.fogDensity = waterNightFogDensity;
        }
    }

    private void GetOutWater(Collider _player)
    {
        SoundManager.instance.PlaySE(sound_WaterOut);
        if (GameManager.isWater)
        {
            GameManager.isWater = false;
            _player.transform.GetComponent<Rigidbody>().drag = originDrag;

            if (!GameManager.isNight)
            {
                RenderSettings.fogColor = originWaterColor;
                RenderSettings.fogDensity = originFogDensity;
            }
            else
            {
                RenderSettings.fogColor = originNightColor;
                RenderSettings.fogDensity = originNightFogDensity;
            }
        }
    }
}