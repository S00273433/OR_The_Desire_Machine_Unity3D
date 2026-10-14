using UnityEngine;

public class TowerRotate : MonoBehaviour
{
    [SerializeField] private Transform BoxTransform;
    
    
    [SerializeField] private float RotationSpeed = 2f;
    
    void LateUpdate()
    {
        BoxTransform.Rotate(Vector3.up, RotationSpeed * Time.deltaTime);
        
    }
}
