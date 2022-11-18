using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class Arm : MonoBehaviour
{
    [SerializeField]
    List<Transform> joints;
    [SerializeField]
    List<float> lengths = new List<float>();

    [SerializeField]
    Transform target;

    public float minDistance;
    public int maxIteration;

    LineRenderer LR;
    Vector3 start, end;
    


    private void Awake()
    {
        LR = GetComponent<LineRenderer>();
        updateLR();

        for(int i = 0; i < joints.Count - 1; i++)
        {
            lengths.Add((joints[i + 1].position - joints[i].position).magnitude);
        }
    }

    void updateLR()
    {
        LR.positionCount = joints.Count;
        int i = 0;
        foreach(Transform j in joints)
        {
            LR.SetPosition(i, j.position);
            i++;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Vector3.Distance(joints[joints.Count - 1].position, target.position) > minDistance)
            Fabrik();
    }

    void Fabrik()
    {
        start = joints[joints.Count - 1].position;
        end = target.position;
        for(int i = 0; i < maxIteration; i++)
            iterate();
        updateLR();
    }

    void iterate()
    {
        for (int i = 0; i < joints.Count; i++)
        {
            if (i == 0)
                joints[i].position = end;
            else
                joints[i].position = (joints[i].position - joints[i - 1].position).normalized * lengths[i - 1] + joints[i - 1].position;
        }

        for (int i = joints.Count - 1; i >= 0; i--)
        {
            if(i == joints.Count - 1)
                joints[i].position = start; 
            else
                joints[i].position = (joints[i].position - joints[i + 1].position).normalized * lengths[i] + joints[i + 1].position;
        }
    }
}
