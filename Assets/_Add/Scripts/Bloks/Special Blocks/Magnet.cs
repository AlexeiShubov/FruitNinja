using UnityEngine;

public class Magnet : SpecialBlock
{
    [SerializeField] private MagneticField _magneticField;

    public void DoMagneticFieldEffect(BlocksController blocksController)
    {
        var newMagnetField = Instantiate(_magneticField, transform.position, Quaternion.identity, transform.parent);
        newMagnetField.Init(blocksController);
    }
}
