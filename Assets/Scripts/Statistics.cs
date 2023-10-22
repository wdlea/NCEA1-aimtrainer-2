using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Statistics
{
    public static int Score => TargetsHit - Misses;
    public static int TargetsSpawned {get; private set; }
    public static int TargetsHit {get; private set; }
    public static int Misses => ShotsFired - TargetsHit;
    public static int ShotsFired {get; private set;}
    public static int HitRate => TargetsSpawned > 0 ? TargetsHit/TargetsSpawned * 100 : 0;
    public static int Accuracy => TargetsSpawned > 0 ?  TargetsHit/ShotsFired * 100 : 0;
    public static int SurvivalTime => (int)Time.timeSinceLevelLoad;

    public static void OnHitTarget(){
        TargetsHit++;
    }
    public static void OnSpawnTarget(){
        TargetsSpawned++;
    }
    public static void OnShot(){
        ShotsFired++;
    }
}
