using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolItem : MonoBehaviour
{

    // De object pool
    private ObjectPool m_objectPool;

    // De pool die in connectie staat met de object pool.
    public ObjectPool MyPool { set { m_objectPool = value; } }


    /// <summary>
    /// Hierbij word dus het object op de juiste positie met rotatie gezet en de 
    /// activatie opgeroeden als een eigen gemaakte start variand.
    /// </summary>
    /// <param name="_pos"></param>
    /// <param name="_rotation"></param>
    /// <param name="_parent"></param>
    public void Iniatalize(Vector3 _pos, Quaternion _rotation, Transform _parent)
    {
        transform.position = _pos;
        transform.rotation = _rotation;
        transform.parent = _parent;

        Activate();
    }

    protected virtual void Activate() { }

    protected virtual void Deactivate() { }

    /// <summary>
    /// Zet dit object inactief en terug in de pool waar het thuis hoort.
    /// </summary>
    public void ReturnToPool()
    {
        Deactivate();
        m_objectPool.ReturnPooledObject(this);
    }

}
