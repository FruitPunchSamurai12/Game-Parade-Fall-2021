using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[RequireComponent(typeof(BoxCollider))]
public class ExitPoints : MonoBehaviour
{
    [SerializeField] GameObject[] activeExitPointObjects;
    [SerializeField] GameObject[] inactiveExitPointObjects;
    BoxCollider _col;
    bool _triggered = false;

    private void OnValidate()
    {
        _col = GetComponent<BoxCollider>();
    }

    public void ToggleActive(bool active)
    {
        if(active)
        {
            foreach (var go in activeExitPointObjects)
            {
                go.SetActive(true);
            }
            foreach (var go in inactiveExitPointObjects)
            {
                go.SetActive(false);
            }
            gameObject.SetActive(true);
        }
        else
        {
            foreach (var go in activeExitPointObjects)
            {
                go.SetActive(false);
            }
            foreach (var go in inactiveExitPointObjects)
            {
                go.SetActive(true);
            }
            gameObject.SetActive(false);
        }
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
