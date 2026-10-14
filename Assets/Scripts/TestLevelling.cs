using UnityEngine;

public class TestLevelling : MonoBehaviour
{
    [SerializeField] private LevellingData levellingData;

    private void Start()
    {
        Debug.Log(levellingData.GetRequiredXp(1));
        Debug.Log(levellingData.GetRequiredXp(10));
        Debug.Log(levellingData.GetRequiredXp(50));
        Debug.Log(levellingData.GetRequiredXp(100));
    }

}
