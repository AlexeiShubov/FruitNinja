public class Heart : SpecialBlock
{
    protected override void GetEffect(BlocksController blocksController)
    {
        GameEvents.OnBonusLifeEvent.Invoke(transform);
    }
}