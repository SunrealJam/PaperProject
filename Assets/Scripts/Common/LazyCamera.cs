using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LazyCamera : MonoBehaviour
{
    public Transform lockOn;

    private void LateUpdate()
    {
        if (!lockOn)
        {
            lockOn = GameObject.FindWithTag("Player")?.transform;
        }

        if (lockOn)
        {
            transform.position = new Vector3(lockOn.position.x, lockOn.position.y + 2.6f, lockOn.position.z - 8f);
        }
    }
}
