using System.Collections;
using UnityEngine;

public class CameraFollow : MonoBehaviour 
{
	[SerializeField] [Range(0f, 1f)] 
	float followSmoothness = 0.2f;

	Vector3 offset;

	
	new Transform transform;
	Transform player;

	void Awake ()
	{
        transform = base.transform;
	}

	void Start () 
	{
		player = GameManager.Instance.PlayerTransform;
		offset = transform.position - player.position;
	}
	
	void LateUpdate () 
	{
        if(player==null)
            player = GameManager.Instance.PlayerTransform;
        transform.position = Vector3.Lerp(transform.position, player.position + offset, 1 - followSmoothness);
	}
}
