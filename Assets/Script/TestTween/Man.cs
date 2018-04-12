using DG.Tweening;
using UnityEngine;

public class Man : MonoBehaviour
{
    float num;
    private float myValue;
    Material manMat;

    void Start()
    {
        //        DOTween.To(() => transform.position, newPos => transform.position = newPos, new Vector3(0, 0, 0), 3); // run one time can get the new position
        //        transform.position = Vector3.Lerp(transform.position, new Vector3(0, 0, 0), Time.deltaTime * 2);
//        transform.DOMove(Vector3.zero + Vector3.up, 3f).SetEase(Ease.Flash).OnComplete(Arrival);
        //        DOTween.To(() => myValue, x => myValue = x, 10, 3);
        manMat = gameObject.GetComponent<MeshRenderer>().material;


        // animation using sequence (can add many tween together)
        // sequence can wait between tweens
        Sequence mySequence = DOTween.Sequence();
        mySequence.Append(transform.DOMove(new Vector3(0, 0, 0), 1))
            .Append(transform.DORotate(new Vector3(0, 180, 0), 1)) // rotate
            .PrependInterval(1) // add 1 second to the beginning of sequence
            .Append(manMat.DOColor(Color.green, 2f))
            .Insert(mySequence.Duration(), transform.DOScale(3, mySequence.Duration())) // rescale
            .OnComplete(Arrival).OnStart(StartAni);
    }

    void Arrival()
    {
        Debug.Log("Done");
    }

    void StartAni()
    {
        Debug.Log("start s");
    }

    void Update()
    {
    }
}
