using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AnimeUtils
{
    #region 常數

    public const string PATH_UP = "PATH_UP";
    public const string PATH_DOWN = "PATH_DOWN";
    public const string PATH_LEFT = "PATH_LEFT";
    public const string PATH_RIGHT = "PATH_RIGHT";
    public const string PATH_UP_LEFT = "PATH_UP_LEFT";
    public const string PATH_UP_RIGHT = "PATH_UP_RIGHT";
    public const string PATH_DOWN_LEFT = "PATH_DOWN_LEFT";
    public const string PATH_DOWN_RIGHT = "PATH_DOWN_RIGHT";
    public const string NO_PATH = "NO_PATH";
    
    #endregion
    
    public static float GetAnimateClipTimeInRuntime(Animator animator, string animeClipName) {
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

    public static string DefineMovementPath(Vector3 movement) {
        Vector3 nomalizedMovement = movement.normalized;
        //Debug.Log("DefineMovementPath movement: "+movement);
        if(movement.x == 0 && movement.y > 0) {
            // Up
            return PATH_UP;
        } else if(nomalizedMovement.x == 0 && nomalizedMovement.y < 0) {
            // Down 
            return PATH_DOWN;
        } else if(nomalizedMovement.x < 0 && nomalizedMovement.y == 0) {
            // Left
            return PATH_LEFT;
        } else if(nomalizedMovement.x > 0 && nomalizedMovement.y == 0) {
            // Right
            return PATH_RIGHT;
        } else if(nomalizedMovement.x > 0 && nomalizedMovement.y > 0) {
            // UpRight
            return PATH_UP_RIGHT;
        } else if(nomalizedMovement.x < 0 && nomalizedMovement.y > 0) {
            // UpLeft
            return PATH_UP_LEFT;
        } else if(nomalizedMovement.x > 0 && nomalizedMovement.y < 0) {
            // DownRight
            return PATH_DOWN_RIGHT;
        } else if(nomalizedMovement.x < 0 && nomalizedMovement.y < 0) {
            // DownLeft
            return PATH_DOWN_LEFT;
        }

        return NO_PATH;
    }

    public static bool isRightForHorizontalAnimation(Vector3 movement, bool defaultIsRight = false) {
        string path = DefineMovementPath(movement);
        if(path.Equals(PATH_RIGHT) || path.Equals(PATH_UP_RIGHT) || path.Equals(PATH_DOWN_RIGHT)) {
            return true;
        } else if(path.Equals(PATH_UP) || path.Equals(PATH_DOWN)) {
            if(defaultIsRight) return true;
        }

        return false;
    }

    public static bool isLeftForHorizontalAnimation(Vector3 movement, bool defaultIsLeft = false) {
        string path = DefineMovementPath(movement);
        if(path.Equals(PATH_LEFT) || path.Equals(PATH_UP_LEFT) || path.Equals(PATH_DOWN_LEFT)) {
            return true;
        } else if(path.Equals(PATH_UP) || path.Equals(PATH_DOWN)) {
            if(defaultIsLeft) return true;
        }

        return false;
    }



}
