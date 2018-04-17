using DG.Tweening;
using UnityEngine;

public class Token : MonoBehaviour
{

    void Start()
    {
        transform.DORotate(new Vector3(0, 360, 180), 2, RotateMode.LocalAxisAdd).SetEase(Ease.Linear).SetLoops(-1, LoopType.Incremental);
    }


    void Update()
    {
    }

}
