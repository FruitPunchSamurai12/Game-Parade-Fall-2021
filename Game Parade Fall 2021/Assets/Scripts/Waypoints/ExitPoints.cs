using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[RequireComponent(typeof(BoxCollider))]
public class ExitPoints : MonoBehaviour
{
    BoxCollider _col;
    bool _triggered = false;

    private void OnValidate()
    {
        _col = GetComponent<BoxCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_triggered) return;
        if(other.CompareTag("Player"))
        {
            _triggered = true;
            GameManager.Instance.BirdGotToTheEnd();
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Handles.color = Color.magenta;
        Handles.DrawWireCube(transform.position, transform.rotation * _col.size);
    }
#endif
}
