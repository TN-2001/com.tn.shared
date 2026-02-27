using UnityEngine;

public class NoRollReference : MonoBehaviour
{
    void LateUpdate()
    {
        var euler = transform.eulerAngles;
        transform.eulerAngles = new Vector3(0f, euler.y, 0f);
    }
}
