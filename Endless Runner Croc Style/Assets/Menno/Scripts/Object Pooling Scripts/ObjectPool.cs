using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    #region Variables
    // Het object dat jij wil opslaan in de object pool.
    public GameObject PooledObject;
    
    // De Object Pool size van hoeveel er in opgeslagen kan worden.
    public int PoolSize = 5;

    // De Object pool zelf waar alle objecten inzitten die jij daar in wou
    // opgeslagen hebben.
    private Stack<PoolItem> m_objectPool;

    [Tooltip("Set this to true if you wanna expand the pool of objects if you are running out")]
    private bool m_autoExpand;

    [Tooltip("The amount of objects added to the pool when pool is empty")]
    private int m_expansionSize = 10;
    #endregion

    #region Create Object Pool and Fill it
    private void Awake()
    {
        // Hier create je de object pool.
        m_objectPool = new Stack<PoolItem>(PoolSize);

        // Hiermee vull je de object pool met de object die jij koos
        Expand(PoolSize);
    }
    #endregion

    #region Expand and Fill Pool
    /// <summary>
    /// Zorgt er voor dat de poolsize helemaal opgevuld word door mijn pooledobject
    /// tot dat de pool vol is
    /// </summary>
    /// <param name="_expansionSize"></param>
    private void Expand(int _expansionSize)
    {        
        for (int i = 0; i < _expansionSize; i++)
        {
            GameObject _newObject = Instantiate(PooledObject);
            PoolItem _item = _newObject.GetComponent<PoolItem>();
            _item.MyPool = this;
            ReturnPooledObject(_item);
        }
    }
    #endregion

    #region Get and Return Pooled Object
    /// <summary>
    /// Deze functie zorgt er voor dat de meest hoogste object in de pool er uit word gehaald actief word 
    /// en op de juiste positie word gezet met de juiste rotatie en dan doet wat dat object moet doen.
    /// </summary>
    /// <param name="_pos"></param>
    /// <param name="_rotation"></param>
    /// <param name="_parent"></param>
    /// <returns></returns>
    public GameObject GetPooledObject(Vector3 _pos, Quaternion _rotation, Transform _parent = null)
    {
        if (m_objectPool.Count == 0)
        {
            Debug.Log($"{name}: Pool is empty wait for the new objects to arrive");
            m_autoExpand = true;
            ExpandThePool();
        }

        PoolItem _item = m_objectPool.Pop();
        _item.Iniatalize(_pos, _rotation, _parent != null ? _parent : transform);
        _item.gameObject.SetActive(true);
        return _item.gameObject;

    }


    /// <summary>
    /// Zorgt er voor dat de objecten weer terug in de pool worden gestopt en 
    /// dan weer inactief worden gezet
    /// </summary>
    /// <param name="_item"></param>
    public void ReturnPooledObject(PoolItem _item)
    {
        if (!_item.gameObject.activeSelf) { return; }
        
        _item.transform.parent = transform;
        _item.gameObject.SetActive(false);

        m_objectPool.Push(_item);
    }
    #endregion

    #region AutoExpand Pool
    /// <summary>
    /// Deze functie zorgt er voor dat als de object pool leeg raakt hij eerst checkt of het zo is zo ja dan maakt 
    /// hij 10 extra pool objecten aan is de pool niet leeg dan stopt de functie zich zelf van de objecten aanmaken
    /// </summary>
    private void ExpandThePool()
    {
        if (m_objectPool.Count != 0)
        {
            m_autoExpand = false;
        }

        if (m_autoExpand == true)
        {
            for (int i = 0; i < m_expansionSize; i++)
            {
                GameObject _newObject = Instantiate(PooledObject);
                PoolItem _item = _newObject.GetComponent<PoolItem>();
                _item.MyPool = this;
                ReturnPooledObject(_item);
            }
        }        
    }
    #endregion
}
