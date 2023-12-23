public class MagneticFieldMode
{
    private readonly BlocksController _blocksController;

    public MagneticFieldMode(BlocksController blocksController)
    {
        _blocksController = blocksController;
    }

    public void DoMagneticFieldEffect(Magnet magnet)
    {
        magnet.DoMagneticFieldEffect(_blocksController);
    }
}
