using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Parameter Storage/Setting")]
public class SettingData : ScriptableObject
{
    [Header("")]
    [Range(0f, 3f)] public int screen_Resolution;                               // 畫面: 解析度     (0 = 3084x2160[4k], 1 = 2560x1440[2k], 2 = 1920x1080[1080p], 3 = 1280x720[720p])
    public float uiScalerReverse;                                               // 畫面: UI縮放比例
    [Range(0f, 2f)] public int screen_Display;                                  // 畫面: 螢幕模式   (0 = 全螢幕, 1 = 視窗化, 2 = 無邊框視窗)
    [Range(0f, 1f)] public int screen_FPS;                                      // 畫面: 幀數       (0 = FPS30, 1 = FPS60)
    [Range(0f, 2f)] public int screen_VerticalSynchronization;                  // 畫面: 垂直同步   (0 = off, 1 = on, 2 = every second frame)

    [Header("")]
    [Range(0f, 10f)] public float audio_Volume_BGM;                             // 聲音: 背景音樂
    [Range(0f, 10f)] public float audio_Volume_SE;                              // 聲音: 音效
    [Range(0f, 10f)] public float audio_Volume_Speech;                          // 聲音: 語音



}
