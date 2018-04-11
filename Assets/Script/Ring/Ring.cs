using UnityEngine;

public class Ring : MonoBehaviour
{
    private RingManager ringMan;
    private bool activeRing;

    void Start()
    {
        ringMan = transform.parent.GetComponent<RingManager>();
    }

    void Update()
    {
    }

    public void ActiveRing()
    {
        activeRing = true;
    }

    void OnTriggerEnter(Collider target)
    {
        if (activeRing && target.gameObject.CompareTag("Player"))
        {
            ringMan.NextRing();
            Destroy(gameObject, 4);
        }
    }
}
