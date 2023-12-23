using UnityEngine;

public class Brick : SpecialBlock
{
    [SerializeField] private Animator _animator;
    
    protected override void GetEffect(BlocksController blocksController)
    {
        if (Particle.Length > 0)
        {
            Particle[Random.Range(0, Particle.Length)].Play();
        }
        
        _animator.CrossFade("Brick", 0);

        GameEvents.OnBlockSwipe.Invoke();
    }
    
    public override void SetControllersMono(IMovable newMovable, IRotator newRotator, ICutting newCutting)
    {
        CurrentMovable = newMovable;
        CurrentRotator = newRotator;
        CurrentCutting = new BrickCutting(this);
    }
}
