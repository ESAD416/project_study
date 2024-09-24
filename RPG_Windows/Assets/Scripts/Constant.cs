using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Constant
{
    public const int DEFAULT_SCREEN_RESOLUTION = 2;
    public const int DEFAULT_SCREEN_DISPLAY = 0;
    public const int DEFAULT_SCREEN_FPS = 0;
    public const int DEFAULT_SCREEN_VERTICALSYNCHRONIZATION  = 0;

    public const int DEFAULT_AUDIO_VOLUME_BGM = 4;
    public const int DEFAULT__AUDIO_VOLUME_SE = 5;
    public const int DEFAULT__AUDIO_VOLUME_SPEECH  = 6;

    public enum ControlDevice {
        Gamepad,
        KeyboardMouse,
        TouchScreen
    };

    public enum CharactorState {
        Idle, Move, Attack, Jump, Dodge, Hurt, Dead,
    }
    public enum EnemyState {
        Patrol, Pursuing, 
    }
    public enum BossState {
        BeforeStart, DuringBattle, AnimePerformance, BattleFinish
    }
    
}
