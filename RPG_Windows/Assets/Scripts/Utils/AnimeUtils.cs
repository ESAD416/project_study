using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal static class AnimeUtils
{
    public static float GetAnimateClipTime(Animator animator, string animeClipName) {
        float attackClipTime = 0f;
        RuntimeAnimatorController ac = animator.runtimeAnimatorController;

        for(int i = 0; i < ac.animationClips.Length; i++) {
            if( ac.animationClips[i].name == animeClipName)        //If it has the same name as your clip
            {
                attackClipTime = ac.animationClips[i].length;
                break;
            }
        }

        Debug.Log("SetAnimateClipTime attackClipTime: "+attackClipTime);
        return attackClipTime;
    }

    public static void ActivateAnimatorLayer(Animator animator, string layerName) {
        for(int i = 0 ; i < animator.layerCount; i++) {
            animator.SetLayerWeight(i, 0);
        }

        animator.SetLayerWeight(animator.GetLayerIndex(layerName), 1);
    }

    public static void SetAnimateFloatPara(Animator animator, Dictionary<string, float> paraDict) {
        if(paraDict != null) {
            foreach(KeyValuePair<string, float> entry in paraDict) {
                animator.SetFloat(entry.Key, entry.Value);
            }
        }
    }
}
