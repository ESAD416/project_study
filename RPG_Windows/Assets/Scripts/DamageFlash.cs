using System;
using System.Collections;
using System.Collections.Generic;
using Fungus;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class DamageFlash : MonoBehaviour
{
    [SerializeField] private Color flashColor;
    [SerializeField] private float flashDuration;

    [SerializeField] private SpriteRenderer[] spriteRenderers;
    private Material[] materials;

    private Coroutine flasherCoroutine;
    
    private void Awake() {
        InitMaterials();
    }

    // Start is called before the first frame update
    private void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        
    }

    private void InitMaterials() {
       materials = new Material[spriteRenderers.Length];
       for(int i = 0; i < spriteRenderers.Length; i++) {
           materials[i] = spriteRenderers[i].material;
       }
    }

    public void CallDamageFlasher() {
        if(flasherCoroutine != null) StopCoroutine(flasherCoroutine);
        flasherCoroutine = StartCoroutine(DamagerFlasher());
    }

    public IEnumerator DamagerFlasher() {
        SetFlashColor();

        float currentFlashAmout = 0f;
        float elaspedTime = 0f;

        while(elaspedTime < flashDuration) {
            elaspedTime += Time.deltaTime;

            currentFlashAmout = Mathf.Lerp(1f, 0f, elaspedTime / flashDuration);
            SetFlashAmout(currentFlashAmout);
            yield return null;
        }
    }

    private void SetFlashColor() {
        foreach(var material in materials) {
            material.SetColor("_FlashColor", flashColor);
        }
    }

    private void SetFlashAmout(float flashAmout) {
        foreach(var material in materials) {
            material.SetFloat("_FloatAmout", flashAmout);
        }
    }
}
