using System.Collections.Generic;
using UnityEngine;

public class GameDifficultyController
{
    private const float ROTATE_DIRECTION_CHANCE_SPEED = 0.5f;
    private const float ROTATE_DIRECTION_SPEED = -0.9f;
    
    private const float LVL_HARD_STEP = 0.1f;
    private const float MAX_LVL_HARD = 2f;
    
    private readonly float _standardMoveSpeedY;

    public Stack<float> RotateSpeeds { get; private set; }
    public Stack<Vector2> MoveSpeedPack  { get; }
    public Stack<SpawnZone> SpawnZones { get; }

    public GameDifficultyController(Stack<SpawnZone>spawnZones, float lvlHard)
    {
        var newLvlHard = lvlHard < MAX_LVL_HARD ? lvlHard + LVL_HARD_STEP : lvlHard;
        
        SpawnZones = spawnZones;
        RotateSpeeds = GetRotateSpeed(new Stack<SpawnZone>(spawnZones));
        MoveSpeedPack = SetMoveSpeed(new Stack<SpawnZone>(spawnZones), newLvlHard);
    }
    
    private Stack<float> GetRotateSpeed(Stack<SpawnZone> spawnZones)
    {
        RotateSpeeds = SetRotateSpeed(spawnZones);

        return RotateSpeeds;
    }

    private Stack<Vector2> SetMoveSpeed(Stack<SpawnZone> spawnZones, float lvlHard )
    {
        var moveSpeedPack = new Stack<Vector2>();

        while (spawnZones.Count > 0)
        {
            var moveSpeed = new Vector2(spawnZones.Peek().StartMoveSpeedBlock.x, spawnZones.Pop().StartMoveSpeedBlock.y + lvlHard);

            moveSpeedPack.Push(moveSpeed);
        }
        
        return moveSpeedPack;
    }

    private Stack<float> SetRotateSpeed(Stack<SpawnZone> spawnZones)
    {
        var rotateSpeedPack = new Stack<float>();

        while(spawnZones.Count > 0)
        {
            var rotateSpeed = spawnZones.Peek().RotationSpeed/spawnZones.Pop().StartMoveSpeedBlock.y;
            
            if (Random.value < ROTATE_DIRECTION_CHANCE_SPEED)
            {
                rotateSpeed *= Random.Range(-ROTATE_DIRECTION_CHANCE_SPEED, ROTATE_DIRECTION_SPEED) * Random.Range(ROTATE_DIRECTION_CHANCE_SPEED, MAX_LVL_HARD);
            }
            
            rotateSpeedPack.Push(rotateSpeed);
        }

        return rotateSpeedPack;
    }
}
