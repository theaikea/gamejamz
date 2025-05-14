using UnityEngine;

public class SpinObject : MonoBehaviour

{
    // Update is called once per frame
    void Update()
    {
        transform.Rotate (new Vector3 (0, 0, 40) * Time.deltaTime);
    }
}