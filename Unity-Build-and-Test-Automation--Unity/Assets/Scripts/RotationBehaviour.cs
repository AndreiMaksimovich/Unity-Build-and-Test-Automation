using UnityEngine;

public class RotationBehaviour : MonoBehaviour
{
    
    
    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up, Time.deltaTime * 10f);    
    }
}
