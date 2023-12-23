public class Ice : SpecialBlock
{
    public void CancelIceEffect(BlocksController blocksController)
    {
        foreach (var block in blocksController.ActiveBlocks)
        {
            if (!block.IsCutting && block.FrozenAnimator != null)
            {
                block.FrozenAnimator.CrossFade("UnFrozen", 0);
            }
            
            block.CurrentMovable.MoveDirection /= 0.3f;
            block.CurrentMovable.CurrentGravity /= 0.1f;
            block.IsFrozen = false;
        }
    }

    public void DoIceEffect(BlocksController blocksController)
    {
        foreach (var block in blocksController.ActiveBlocks)
        {
            if (block.IsFrozen) continue;

            if (!block.IsCutting && block.FrozenAnimator != null)
            {
                block.FrozenAnimator.CrossFade("Frozen", 0);
            }
            
            block.CurrentMovable.MoveDirection *= 0.3f;
            block.CurrentMovable.CurrentGravity *= 0.1f;
            block.IsFrozen = true;
        }
    }
}
