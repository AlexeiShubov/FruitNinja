using UnityEngine;

public class Bomb : SpecialBlock
{
    protected override void GetEffect(BlocksController blocksController)
    {
        foreach (var block in blocksController.ActiveBlocks)
        {
            if (block != this && !block.IsCutting && Vector2.Distance(transform.position, block.transform.position) < 5f 
                && Vector2.Distance(transform.position, block.transform.position) > 0.01f)
            {
                block.CurrentMovable.MoveDirection = Explosion(block.transform, transform, 30f);
            }
        }
            
        GameEvents.OnDamageEvent?.Invoke();
    }
    
    private Vector2 Explosion(Transform block, Transform bomb, float force)
    {
        var currentDirection = block.position - bomb.position;
        var currentDistanceToBlock = currentDirection.magnitude;
        var resultForce = force / currentDistanceToBlock;
        
        var result = currentDirection.normalized * resultForce;

        return result;
    }
}
