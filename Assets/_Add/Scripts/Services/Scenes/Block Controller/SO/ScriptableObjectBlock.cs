using UnityEngine;

[CreateAssetMenu(menuName = "Create a Block", fileName = "Block")]
public class ScriptableObjectBlock : ScriptableObject
{
    public Vector2 DefaultBlockScale;
    public Vector2 MaxBlockScale;
    public float ScaleBlockStep;
    
    [Header("Sprites")]
    public Sprite[] MainSpriteBlock;
    public Sprite[] LeftBlock;
    public Sprite[] RightBlock;
    public Sprite[] Shadow;
    public Sprite[] Splat;
}
