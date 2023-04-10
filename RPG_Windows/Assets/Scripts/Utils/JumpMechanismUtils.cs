using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class JumpMechanismUtils
{
    public enum JumpState {
        Ground,
        JumpUp,
        JumpDown
    }

    public static JumpState DetectedJumpState(Vector2 raycastStartPoint, Vector2 rayCastEndPosition, Vector2 rayCastDistance, float currHeight, bool isMoving, bool OnCollisioning = false) {
        RaycastHit2D[] hits = Physics2D.LinecastAll(raycastStartPoint, rayCastEndPosition, 1 << LayerMask.NameToLayer("HeightObj"));
        
        var heightManager = GameObject.FindObjectOfType(typeof(HeightManager)) as HeightManager;
        float altitudeVariation = 0f;
        bool jumpUp = false;
        bool jumpDown = false;

        if(hits.Length >= 1) {
            Debug.Log("DetectedJumpState hits.Length > 1");
            foreach(RaycastHit2D hit in hits) {
                Debug.Log("DetectedJumpState hits collider name: "+hit.collider.name);
                var heightObj = hit.collider.GetComponent<HeightOfObject>() as HeightOfObject;
                if(heightObj != null) {
                    float correspondHeight = heightObj.GetCorrespondHeight();
                    float selfHeight = heightObj.GetSelfHeight();
                    altitudeVariation = Math.Abs(currHeight - correspondHeight) ;

                    if(currHeight < correspondHeight && altitudeVariation > 0 && altitudeVariation <= 1) {
                        var angle = Vector2.Angle((Vector2)raycastStartPoint - rayCastEndPosition, hit.normal);
                        angle = 90.0f - Mathf.Abs(angle);
                        Debug.Log("DetectedJumpState linecast angle:"+angle);
                        if(angle >= 60f && 180f - angle >= 60f) {
                            jumpUp = true;
                        }
                    }  else if(currHeight >= correspondHeight) {
                        jumpDown = true;
                    }
                }
            }
        }

        if(jumpUp) {
            return JumpState.JumpUp;
        } else if(jumpDown) {
            return JumpState.JumpDown;
        }
        return JumpState.Ground;
    }
}
