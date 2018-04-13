using DG.Tweening;
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
//            Vector3 pos = transform.position;
//            pos.z -= 1f;    // pos.z += value -> error
//            transform.DOMove(pos, 0.1f);
            transform.DOShakeScale(2f, 10f);

            ringMan.NextRing();
            Destroy(gameObject, 1f);
        }
    }
}
