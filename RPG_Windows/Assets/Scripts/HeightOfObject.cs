using System;
using UnityEngine;

public class HeightOfObject : MonoBehaviour
{
    [SerializeField] private float selfHeight = 0;
    [SerializeField] private float correspondHeight = 0;
    [SerializeField] private bool noEntry = false;



    public float GetCorrespondHeight() {
        return correspondHeight;
    }

    public float GetSelfHeight() {
        return selfHeight;
    }

    public bool GetNoEntry() {
        return noEntry;
    }
}
