using Gameplay.Enemies;
using System.Collections;
using UnityEngine;
using UnityEngine.U2D;
using Utilities.ObjectPool;

public class Bullets : APoolableObject
{
    private Coroutine _movement;

    /**
    public bool IsActive
    {
        get => gameObject.activeSelf;
        set => gameObject.SetActive(value);
    }
    /**/ 
    //Just use IsActive from APoolableObject it's a public method

    /**/
    public override void Reset()
    {
        //Resetear valores
        if (_movement != null)
        {
            StopCoroutine(_movement);
            _movement = null;
        }
        //GameplayManager.Instance.poolManager.Release(this);
    }
    /**/

    public void Shot(Vector3 from, AEnemy enemy, float dur, IPoolManager pool)
    {
        Debug.Log("Estamos en el metodo SHOT DE LA BALA");
        transform.position = from;
        if (_movement != null)
        {
            StopCoroutine(_movement);
        }
        _movement = StartCoroutine(BulletMovement(enemy, dur, pool));
    }

    private IEnumerator BulletMovement(AEnemy enemy, float dur, IPoolManager pool)
    {
        Debug.Log("Estamos en la corutina");
        Vector3 start = transform.position;
        float time = 0f;
        while (time < 1f)
        {
            Vector3 position= enemy.transform.position;
            time+= Time.deltaTime/Mathf.Max(0.0001f, dur);
            transform.position = Vector3.Lerp(start, position, time);
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
