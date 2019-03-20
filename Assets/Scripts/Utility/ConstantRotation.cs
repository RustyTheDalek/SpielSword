using UnityEngine;

public class ConstantRotation : MonoBehaviour
{
    public int m_RotationSpeed = 20;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.forward, m_RotationSpeed);
    }
}
