using UnityEngine;

[CreateAssetMenu(fileName = "LevellingData", menuName = "Scriptable Objects/LevellingData")]
public class LevellingData : ScriptableObject
{
    [SerializeField] private AnimationCurve Curve;

    public int GetRequiredXp(int level)
    {
        return (int)Curve.Evaluate(level);
    }
}
