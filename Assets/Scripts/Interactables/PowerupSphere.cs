using UnityEngine;

public class PowerupSphere : MonoBehaviour
{
    [SerializeField] private Transform SphereTransform;
    [SerializeField] private float RotationSpeed = 2f;

    // Update is called once per frame
    void Update()
    {
        SphereTransform.Rotate(Vector3.right, RotationSpeed * Time.deltaTime);
        SphereTransform.Rotate(Vector3.down, RotationSpeed * Time.deltaTime);
    }
}
