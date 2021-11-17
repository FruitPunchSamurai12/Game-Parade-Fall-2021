using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Feather : MonoBehaviour
{
    [SerializeField]
    float rotateSpeed = 5f;
    [SerializeField]
    float floatSpeed = 5f;
    [SerializeField]
    float floatHeight = 2f;
    [SerializeField]
    float colorAffectionDistance = 100f;

    float defaultY = 0f;

    new Transform transform;
    new MeshRenderer renderer;

    private void Awake()
    {
        transform = base.transform;
        renderer = GetComponentInChildren<MeshRenderer>();
    }

    private void Start()
    {
        defaultY = transform.position.y;
        ColorCorrection();
    }

    private void Update()
    {
        FloatAndRotate();
    }

    void FloatAndRotate ()
    {
        transform.Rotate(0f, rotateSpeed, 0f);

        var newY = defaultY + Mathf.Sin(Time.time * floatSpeed) * floatHeight;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }

    void ColorCorrection ()
    {
        var distance = Vector3.Distance(transform.position, GameManager.Instance.ExitPoint.position);
        if (distance < colorAffectionDistance)
        {
            var newColor = new Color(0f, (colorAffectionDistance - distance) * 1f / colorAffectionDistance, 0f);
            renderer.material.color = newColor;
        }   
    }
}
