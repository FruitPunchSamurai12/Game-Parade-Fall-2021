using System;
using System.Collections;
using UnityEngine;

public class PooledMonoBehaviour : MonoBehaviour
{
    [SerializeField] int initialPoolSize = 50;

    public event Action<PooledMonoBehaviour> OnReturnToPool;

    public int InitialPoolSize { get { return initialPoolSize; } }

    public T Get<T>(bool enable = true) where T:PooledMonoBehaviour
    {
        var pool = Pool.GetPool(this);
        var pooledObject = pool.Get<T>();

        if(enable)
        {
            pooledObject.gameObject.SetActive(true);
        }

        return pooledObject;
    }

    public T Get<T>(Vector3 position,Quaternion rotation) where T : PooledMonoBehaviour
    {
        var pooledObject = Get<T>();
        pooledObject.transform.position = position;
        pooledObject.transform.rotation = rotation;

        return pooledObject;
    }

    public T Get<T>(Vector2 position, Quaternion rotation) where T : PooledMonoBehaviour
    {
        return Get<T>(new Vector3(position.x,position.y,0), rotation);
    }

    public void Get<T>(object p, Quaternion rotation)
    {
        throw new NotImplementedException();
    }

    protected virtual void OnDisable()
    {
        OnReturnToPool?.Invoke(this);
    }

    protected void ReturnToPool(float delay = 0)
    {
        StartCoroutine(ReturnToPoolAfterSeconds(delay));
    }

    private IEnumerator ReturnToPoolAfterSeconds(float delay)
    {
        yield return new WaitForSeconds(delay);
        gameObject.SetActive(false);
    }
}