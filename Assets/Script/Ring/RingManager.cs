using System.Collections.Generic;
using UnityEngine;

public class RingManager : MonoBehaviour
{
    public List<Transform> rings = new List<Transform>();

    public Material activeRingMat;
    public Material inactiveRingMat;
    public Material finalRingMat;

    int currentRing = 0;

    void Start()
    {
        foreach (Transform r in transform)
        {
            rings.Add(r);
            r.GetComponent<MeshRenderer>().material = inactiveRingMat;
        }

        if (rings.Count == 0) return; // check if there is no ring child

        // active the first ring
        rings[currentRing].GetComponent<MeshRenderer>().material = activeRingMat;
        rings[currentRing].GetComponent<Ring>().ActiveRing();
    }

    void Update()
    {
    }

    public void NextRing()
    {
        currentRing++;

        if (currentRing == rings.Count) // check if Victory
        {
            Victory();
            return;
        }
        
        // show the final material
        if (currentRing == rings.Count - 1)
            rings[currentRing].GetComponent<MeshRenderer>().material = finalRingMat;
        else
            rings[currentRing].GetComponent<MeshRenderer>().material = activeRingMat;

        rings[currentRing].GetComponent<Ring>().ActiveRing();
    }

    void Victory()
    {
        //        FindObjectOfType<GameScene>().CompleteLevel();
    }
}
