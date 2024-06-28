using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Constant
{
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
