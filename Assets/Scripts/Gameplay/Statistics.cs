using UnityEngine;
/// <summary>
/// Keeps track of neccessary statistics
/// </summary>
public static class Statistics
{

    //calculate or store the various statistics
    public static int Score => TargetsHit - Misses;
    public static int TargetsSpawned {get; private set; }
    public static int TargetsHit {get; private set; }
    public static int Misses => ShotsFired - TargetsHit;
    public static int ShotsFired {get; private set;}
    public static int HitRate => TargetsSpawned > 0 ? (int)((float)TargetsHit/TargetsSpawned * 100f) : 0;
    public static int Accuracy => ShotsFired > 0 ? (int)((float)TargetsHit/ShotsFired * 100f) : 0;
    public static int SurvivalTime => (int)Time.timeSinceLevelLoad;

    //callbacks for updating some stats while not making their set public

    public static void OnHitTarget(){
        TargetsHit++;
    }
    public static void OnSpawnTarget(){
        TargetsSpawned++;
    }
    public static void OnShot(){
        Debug.Log("pew");
        ShotsFired++;
    }
}
