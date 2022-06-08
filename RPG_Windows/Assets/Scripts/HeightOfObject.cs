using System;
using UnityEngine;

public class HeightOfObject : MonoBehaviour
{
    [SerializeField] private float selfHeight = 0;
    [SerializeField] private float correspondHeight = 0;



    public float GetCorrespondHeight() {
        return correspondHeight;
    }

    public float GetSelfHeight() {
        return selfHeight;
    }
}
