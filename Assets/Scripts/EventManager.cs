using System;
using UnityEngine;

public static class EventManager
{
    public static Action OnExplode;
    public static Action OnSendBoss;
    public static Action OnGameOver;
    public static Action OnWaveComplete;
    public static Action OnLevelComplete;
    public static Action<int> OnEnemyGone;
    public static Action<Transform> OnDestroyTower;
    public static Action<Transform> OnRemoveBoxFromTower;
    public static Action<Transform> ONBoxReachToSling;
    public static Action<Transform> OnBoxLeaveSling;
    public static Action<Transform,int> OnEnemyMerge;
    public static Action<Transform,int> OnBoxTouchGround;
    public static Action<Transform, Transform> OnNewBoxToTower;

    public static void TriggerExplode()
    {
        OnExplode.Invoke();
    }

    public static void TriggerRemoveBoxFromTower(Transform box)
    {
        OnRemoveBoxFromTower.Invoke(box);
    }

    public static void TriggerEnemyGone(int number)
    {
        OnEnemyGone.Invoke(number);
    }

    public static void TriggerGameOver()
    {
        OnGameOver.Invoke();
    }

    public static void TriggerWaveComplete()
    {
        OnWaveComplete.Invoke();
    }

    public static void TriggerLevelComplete()
    {
        OnLevelComplete.Invoke();
    }

    public static void TriggerSendBoss()
    {
        OnSendBoss.Invoke();
    }
    
    public static void TriggerEnemyMerge(Transform transform, int index)
    {
        OnEnemyMerge.Invoke(transform, index);
    }
    
    public static void TriggerBoxReachToSling(Transform transform)
    {
        ONBoxReachToSling.Invoke(transform);
    }

    public static void TriggerBoxLeaveSling(Transform transform)
    {
        OnBoxLeaveSling.Invoke(transform);
    }

    public static void TriggerBoxTouchGround(Transform transform, int towerNumber)
    {
        OnBoxTouchGround.Invoke(transform, towerNumber);
    }

    public static void TriggerNewBoxToTower(Transform transform1, Transform transform2)
    {
        OnNewBoxToTower.Invoke(transform1,transform2);
    }
    public static void TriggerDestroyTower(Transform transform)
    {
        OnDestroyTower.Invoke(transform);
    }
}
