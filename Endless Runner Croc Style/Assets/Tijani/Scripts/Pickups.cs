using UnityEngine;
using UnityEngine.Events;

public class Pickups : MonoBehaviour
{
    [SerializeField]
    private UnityEvent _pickUpEvent;
    public UnityEvent PickUpEvent => _pickUpEvent;

    public void MonkeyPickup(int monkeyMultiplier)
    {
        GameManager.Instance.MonkeyCount++;

        if (GameManager.Instance.MonkeyCount >= GameManager.Instance.MaxMonkeyCount)
        {
            GameManager.Instance.MonkeyCount = 0;
            GameManager.Instance.MaxMonkeyCount *= monkeyMultiplier;
            GameManager.Instance.PlayerHealth.CurrentHealth++;
            UIManager.Instance.UpdateHealth();
        }

        SoundManager.Instance.Play("MonkeyCollect");
        UIManager.Instance.UpdateMonkeyCount();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerController>())
        {
            PickUpEvent.Invoke();
            LevelGenerator.Instance.PickupPool.ReturnPooledObject(this.GetComponent<PoolItem>());
        }
    }
}