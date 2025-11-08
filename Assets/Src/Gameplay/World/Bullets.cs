using System.Collections;
using UnityEngine;
using UnityEngine.U2D;
using Utilities.ObjectPool;

public class Bullets : MonoBehaviour, IPoolableObject
{
    private Coroutine _movement;
    public bool IsActive
    {
        get => gameObject.activeSelf;
        set => gameObject.SetActive(value);
    }
    
    public void Reset()
    {
        //Resetear valores
        IsActive = false;
        if (_movement != null)
        {
            StopCoroutine(_movement);
            _movement = null;
        }
    }

    public void Shot(Vector3 from, Vector3 to, float dur, IPoolManager pool)
    {
        transform.position = from;
        if (_movement != null)
        {
            StopCoroutine(_movement);
            _movement = StartCoroutine(BulletMovement(to, dur, pool));
        }
    }

    private IEnumerator BulletMovement(Vector3 target, float dur, IPoolManager pool)
    {
        Vector3 start = transform.position;
        float time = 0f;
        while (time < 1f)
        {
            time+= Time.deltaTime/Mathf.Max(0.0001f, dur);
            transform.position = Vector3.Lerp(start, target, time);
            yield return null;
        }
        pool.Release(this);
    }


    /*// Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }*/
}
