public class Fruit : AbstractBlock
{
    public override IRotator CurrentRotator { get; set; }
    public override IMovable CurrentMovable { get; set; }
    public override ICutting CurrentCutting { get; set; }
}
