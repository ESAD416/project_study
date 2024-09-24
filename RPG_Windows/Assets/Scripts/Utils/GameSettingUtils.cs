using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public static class GameSettingUtils
{

    public static void ScreenSet(SettingData settingData, int resolutionType, int displayType, int fpsType, int vsType) {
        // 解析度 & 螢幕模式
        if(settingData.screen_Resolution != resolutionType || settingData.screen_Display != displayType) {
            settingData.screen_Resolution = resolutionType;
            settingData.screen_Display =  displayType;

            Vector2Int resolution = Vector2Int.zero;
            float MainUI_Scaler = 1f;
            switch(resolutionType) {
                case 0:  
                    resolution = new Vector2Int(3084, 2160); 
                    MainUI_Scaler = 1f;
                    break;
                case 1: 
                    resolution = new Vector2Int(2560, 1440); 
                    MainUI_Scaler = 0.67f;
                    break;
                case 2: 
                    resolution = new Vector2Int(1920, 1080); 
                    MainUI_Scaler = 0.5f;
                    break;
                case 3: 
                    resolution = new Vector2Int(1280, 720); 
                    MainUI_Scaler = 0.33335f;
                    break;
                
            }
            Debug.Log("MainUI_Scaler: " + MainUI_Scaler);
            settingData.uiScalerReverse = 1f / MainUI_Scaler;
            Debug.Log("uiScalerReverse: " + settingData.uiScalerReverse);

            FullScreenMode fullScreen = FullScreenMode.FullScreenWindow;            //  全螢幕
            switch(displayType) {
                case 1: fullScreen = FullScreenMode.Windowed; break;                //  視窗模式
                case 2: fullScreen = FullScreenMode.ExclusiveFullScreen; break;     //  無邊框視窗
            }

            // 螢幕設定
            Screen.SetResolution(resolution.x, resolution.y, fullScreen);
        }

        // FPS設定
        if(settingData.screen_FPS != fpsType) {
            settingData.screen_FPS = fpsType;

            int fps = 60;
            switch(fpsType) {
                case 1: 
                    fps = 30; 
                    break;
            }

            // 幀數設定
            Application.targetFrameRate = fps;
        }

        if(settingData.screen_VerticalSynchronization != vsType) {
            settingData.screen_VerticalSynchronization = vsType;

            QualitySettings.vSyncCount = vsType;    // 0 = off, 1 = on, 2 = every second frame
        }
    }

    public static void AudioSet(SettingData settingData, float bgm, float se, float speech) {
        if(settingData.audio_Volume_BGM != bgm) settingData.audio_Volume_BGM = bgm;
        if(settingData.audio_Volume_SE != se) settingData.audio_Volume_SE = se;
        if(settingData.audio_Volume_Speech != speech) settingData.audio_Volume_Speech = speech;
    }

    // public static int GetGameSetting(SettingData settingData, int type) {
    //     int result = 0;

    //     switch(type) {
    //         case 0: 
    //             result = settingData.screen_Resolution;
    //             break;
    //         case 1: 
    //             result = settingData.screen_Display;
    //             break;
    //         case 2: 
    //             result = settingData.screen_FPS;
    //             break;
    //         case 3: 
    //             result = settingData.screen_VerticalSynchronization;
    //             break;

    //         case 4: 
    //             result = settingData.audio_Volume_BGM;
    //             break;
    //         case 5: 
    //             result = settingData.audio_Volume_SE;
    //             break;
    //         case 6: 
    //             result = settingData.audio_Volume_Speech;
    //             break;
    //     }

    //     return result;
    // }

}

