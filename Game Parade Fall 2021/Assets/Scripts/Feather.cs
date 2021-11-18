using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Feather : PooledMonoBehaviour
{
    [SerializeField]
    float rotateSpeed = 5f;
    [SerializeField]
    float floatSpeed = 5f;
    [SerializeField]
    float floatHeight = 2f;
    [SerializeField]
    float colorAffectionDistance = 100f;
    [SerializeField]
    float alphaReductionStrength = 0.05f;
    [SerializeField]
    float alphaReductionDistance = 25f;

    float defaultY = 0f;

    new Transform transform;
    new MeshRenderer renderer;

    private void Awake()
    {
        transform = base.transform;
        renderer = GetComponentInChildren<MeshRenderer>();
    }

    private void OnEnable()
    {
        defaultY = transform.position.y;  
        SetColor();
    }

    private void Update()
    {
        FloatAndRotate();
        AlphaCorrection();
    }



    void SetColor()
    {
        var distance = Vector3.Distance(transform.position, GameManager.Instance.ExitPoint.position);
        if (distance < colorAffectionDistance)
        {
            var newColor = new Color(0f, (colorAffectionDistance - distance) * 1f / colorAffectionDistance, 0f);
            for (int i = 0; i < renderer.materials.Length; i++) renderer.materials[i].color = newColor;
        }
    }

    void FloatAndRotate ()
    {
        transform.Rotate(0f, rotateSpeed, 0f);

        var newY = defaultY + Mathf.Sin(Time.time * floatSpeed) * floatHeight;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }

    void AlphaCorrection ()
    {
        var distance = Vector3.Distance(transform.position, GameManager.Instance.PlayerTransform.position);
        if (distance > alphaReductionDistance)
        {
            var newAlpha = 1f - (distance - alphaReductionDistance) * alphaReductionStrength;
            var newColor = renderer.materials[1].color;
            newColor.a = newAlpha;
            renderer.materials[1].color = newColor;
        }
    }
}
