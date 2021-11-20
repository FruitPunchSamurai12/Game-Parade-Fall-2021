using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public int AreaID;
    public int CatNewAreaID;
    public event Action<Checkpoint> onCheckpointTrigger;

    BoxCollider _col;

    private void OnValidate()
    {
        _col = GetComponent<BoxCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            onCheckpointTrigger?.Invoke(this);
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        UnityEditor.Handles.DrawWireCube(transform.position, _col.bounds.size);
    }
#endif

}
