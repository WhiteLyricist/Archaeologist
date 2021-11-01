using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SizeAdaptationBackground : MonoBehaviour
{
    private void Awake()
    {
            var newWidth = Screen.width * transform.localScale.y / Screen.height;

            transform.localScale = new Vector3(newWidth, transform.localScale.y, transform.localScale.z);

    }
}
