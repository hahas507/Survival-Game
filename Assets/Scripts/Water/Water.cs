using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    [SerializeField]
    private float totalOxygen;

    private float currentOxygen;

    private float temp;

    [SerializeField]
    private GameObject go_BaseUI;

    [SerializeField]
    private Text text_total_Oxygen;

    [SerializeField]
    private Text text_currentOxygen;

    [SerializeField]
    private Image gauge;

    private StatusController thePlayerStat;

    private void Start()
    {
        originWaterColor = RenderSettings.fogColor;
        originFogDensity = RenderSettings.fogDensity;

        originDrag = 0;

        thePlayerStat = FindObjectOfType<StatusController>();
        currentOxygen = totalOxygen;
        text_total_Oxygen.text = totalOxygen.ToString();
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

        DecreaseOxygen();
    }

    private void DecreaseOxygen()
    {
        if (GameManager.isWater)
        {
            currentOxygen -= Time.deltaTime;
            text_currentOxygen.text = Mathf.RoundToInt(currentOxygen).ToString();
            gauge.fillAmount = currentOxygen / totalOxygen;

            if (currentOxygen <= 0)
            {
                temp += Time.deltaTime;
                if (temp >= 1)
                {
                    thePlayerStat.DecreaseHP(1);
                    temp = 0;
                }
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

        go_BaseUI.SetActive(true);

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
        go_BaseUI.SetActive(false);
        if (GameManager.isWater)
        {
            currentOxygen = totalOxygen;
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