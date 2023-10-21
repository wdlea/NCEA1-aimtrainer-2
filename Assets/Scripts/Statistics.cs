using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Statistics
{
    public static int Score {get; private set; }
    public static int TargetsSpawned {get; private set; }
    public static int TargetsHit {get; private set; }
    public static int ShotsFired {get; private set;}
    public static int HitRate => TargetsHit/TargetsSpawned * 100;
    public static int Accuracy => TargetsHit/ShotsFired * 100;
    public static int SurvivalTime => (int)Time.timeSinceLevelLoad;
}
