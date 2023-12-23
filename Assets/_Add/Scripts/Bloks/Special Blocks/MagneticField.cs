using System.Collections;
using UnityEngine;

public class MagneticField : MonoBehaviour
{
    [SerializeField] private float _force;
    [SerializeField] private float _magnetismTime;
    [SerializeField] private float _magnetismDistantion;
    
    private BlocksController _blocksController;

    public void Init(BlocksController blocksController)
    {
        _blocksController = blocksController;
        
        StartCoroutine(StartDrop());
        StartCoroutine(DoMagneticFieldEffect());
    }

    private IEnumerator DoMagneticFieldEffect()
    {
        while (true)
        {
            foreach (var block in _blocksController.ActiveBlocks)
            {
                if (block is Bomb || block is Brick || Vector2.Distance(block.transform.position, transform.position) >
                    _magnetismDistantion) continue;

                if (block != null)
                {
                    block.transform.position = Vector2.MoveTowards
                    (block.transform.position,
                        transform.position,
                        Mathf.Abs(block.CurrentMovable.MoveDirection.y * block.CurrentMovable.MoveDirection.x) * _force * Time.deltaTime);
                }
            }

            yield return null;
        }
    }

    private IEnumerator StartDrop()
    {
        yield return new WaitForSeconds(_magnetismTime);

        Destroy(gameObject);
    }
}
