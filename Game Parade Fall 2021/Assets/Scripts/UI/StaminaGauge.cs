using UnityEngine;
using UnityEngine.UI;

public class StaminaGauge : MonoBehaviour
{
    PlayerMovement movement;
    new RectTransform transform;
    Image sprite;

    private void Awake()
    {
        transform = (RectTransform) base.transform;
        sprite = GetComponent<Image>();
        movement = GameObject.FindWithTag("Player").GetComponent<PlayerMovement>();
    }

    private void Start()
    {
        
    }

    private void Update()
    {
        var newX = 0.5f - ((100f - movement.CurrentStamina) * 0.00425f);
        transform.anchorMax = new Vector2(newX, transform.anchorMax.y);

        if (newX < 0.11875f) sprite.color = Color.red;
        else if (newX < 0.2375f) sprite.color = Color.yellow;
        else sprite.color = Color.green;
    }
}
