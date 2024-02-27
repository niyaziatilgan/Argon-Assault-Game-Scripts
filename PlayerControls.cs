using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    [Header("General Setup Settings")]
    [Tooltip("Player Ship'e verdiðimiz hýz deðeri ")] [SerializeField] float controlSpeed = 10f;
    [Tooltip("X eksenindeki hýz deðeri ")] [SerializeField] float xRange = 10f;
    [Tooltip("Y eksenindeki hýz deðeri ")] [SerializeField] float yRange = 5f;

    [Header("Lazerler")]
    [Tooltip("Oyuncudaki lazerler")] [SerializeField] GameObject[] lasers;

    [Header("Ekrana Göre Döndürme")]
    [SerializeField] float positionEgimFactor = -2f;
    [SerializeField] float positionYalpalamaFactor = -1.5f;

    [Header("Oyuncuya Göre Döndürme")]
    [SerializeField] float controlEgimFactor = -10f;
    [SerializeField] float controlYuvarlanmaFactor = -15f;


    float xThrow;
    float yThrow;

    void Update()
    {
        ProcessTranslation();
        ProcessRotation();
        ProcessFiring();
    }


    void ProcessRotation()
    {
        float egimDueToPosition = transform.localPosition.y * positionEgimFactor;
        float egimDueToControlThrow = yThrow * controlEgimFactor;

        float egim = egimDueToPosition + egimDueToControlThrow;
        float yalpalama = transform.localPosition.x * positionYalpalamaFactor;
        float yuvarlanma = xThrow * controlYuvarlanmaFactor;
        transform.localRotation = Quaternion.Euler (egim, yalpalama, yuvarlanma);
    }

     void ProcessTranslation()
    {
         xThrow = Input.GetAxis("Horizontal");
         yThrow = Input.GetAxis("Vertical");

        float xOffset = xThrow * Time.deltaTime * controlSpeed;
        float rawXPos = transform.localPosition.x + xOffset;
        float clampedXPos = Mathf.Clamp(rawXPos, -xRange, xRange);

        float yOffset = yThrow * Time.deltaTime * controlSpeed;
        float rawYPos = transform.localPosition.y + yOffset;
        float clampedYPos = Mathf.Clamp(rawYPos, -yRange, yRange);

        transform.localPosition = new Vector3(clampedXPos, clampedYPos, transform.localPosition.z);
    }

    void ProcessFiring()
    {
        if(Input.GetButton("Fire1"))
        {
            SetLazersActive(true); 
        }
        else
        {
            SetLazersActive(false);
        }
    }

     void SetLazersActive(bool isActive)
    {
       foreach (GameObject laser in lasers)
        {
            var emissionModule = laser.GetComponent<ParticleSystem>().emission;
            emissionModule.enabled = isActive;
        }
    }

}
