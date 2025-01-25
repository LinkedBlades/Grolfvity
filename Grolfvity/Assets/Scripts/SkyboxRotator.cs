using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SkyboxRotator : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] float rotationSpeed = 1.0f;

    // Update is called once per frame
    void Update()
    {
        float rotation = Time.time * rotationSpeed;
        RenderSettings.skybox.SetFloat("_Rotation", rotation);
    }
}
