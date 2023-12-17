using UnityEngine;

public class HoverItem2 : MonoBehaviour
{
    public float floatSpeed = 1.5f; 
    public float floatAmplitude = 0.2f; 
    public Vector3 rotationSpeed = new Vector3(0, 45, 0);
    public bool itemRotation = true;

    private Vector3 initialPosition;

    void Start()
    {
        initialPosition = transform.position;
    }

   
    void Update()
    {
        if(itemRotation)
        {
            Vector3 newPosition = initialPosition + new Vector3(0, Mathf.Sin(Time.time * floatSpeed) * floatAmplitude, 0);

            transform.position = newPosition;
            transform.Rotate(rotationSpeed * Time.deltaTime);
        }
    }
}
