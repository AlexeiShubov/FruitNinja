using System;
using UnityEngine;

public static class GameEvents
{
    public static Action<SpecialBlock> OnSpecialBlockIsCutting;
    public static Action<int, Transform> OnCuttingFruitEvent;
    public static Action<bool> OnLossGameEvent;
    public static Action<Transform> OnBonusLifeEvent;
    public static Action OnDamageEvent;
    public static Action OnMenuSceneLoad;
    public static Action OnSceneClear;
    public static Action OnBlockSwipe;
}
