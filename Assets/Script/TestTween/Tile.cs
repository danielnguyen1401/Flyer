using DG.Tweening;
using DG.Tweening.Core;
using UnityEngine;

public class Tile : MonoBehaviour
{
    void Start()
    {
    }

    void Update()
    {
    }

    void OnMouseDown()
    {
        Vector3 pos = transform.position;
        pos.y += 0.5f;
        Pawn.instance.transform.DOMove(pos, 0.5f).SetEase(Ease.OutQuart).OnComplete(ArrivalTile);
    }

    void ArrivalTile()
    {
        //        Debug.Log("arrival");
        Vector3[] pos = new Vector3[4];
        pos[0] = transform.position;
        pos[1] = transform.position + Vector3.right * 5;
        pos[2] = transform.position - Vector3.up * 2;
        pos[3] = transform.position + Vector3.up * 0.5f;
//        pos.y += 0.5f;
//        Pawn.instance.transform.DOJump(pos, 1, 1, 0.3f).SetLoops(3);
        Pawn.instance.transform.DOPath(pos, 2f, PathType.CatmullRom);

        Sequence myS = DOTween.Sequence();
//        myS.Append()
        


    }
}
