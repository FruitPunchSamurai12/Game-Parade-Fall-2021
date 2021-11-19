using System.Collections;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public void Shake (float magnitude = 1f, float duration = 1f) {
        StartCoroutine(StartShake(magnitude, duration));
    }

    IEnumerator StartShake (float magnitude, float duration) {

        Vector3 originPosition = transform.localPosition;

        float elapsed = 0f;

        while (elapsed < duration) {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;
            transform.localPosition = new Vector3(x, y, originPosition.z);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = originPosition;
    }
}
