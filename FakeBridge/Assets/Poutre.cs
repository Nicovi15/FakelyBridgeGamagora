using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
[RequireComponent(typeof(PolygonCollider2D))]
public class Poutre : MonoBehaviour
{
    public Accroche accA, accB;
    public float maxSize;

    public Gradient validColor;
    public Gradient nonValidColor;
    LineRenderer LR;
    PolygonCollider2D collid;

    private void Awake()
    {
        LR = GetComponent<LineRenderer>();
        collid = GetComponent<PolygonCollider2D>();
        LR.positionCount = 2;
        if (accA != null && accB != null)
            initPoutre(accA, accB);
    }

    public void initPoutre(Accroche accA, Accroche accB)
    {
        this.accA = accA;
        this.accB = accB;

        this.accA.poutres.Add(this);
        this.accB.poutres.Add(this);
        updateVisu();
    }

    public void initPoutre(Accroche accB)
    {
        this.accB = accB;
        this.accA.poutres.Add(this);
        this.accB.poutres.Add(this);
        updateVisu();
    }


    // Start is called before the first frame update
    void Start()
    {
        //updateVisu();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void updateVisu()
    {
        LR.SetPosition(0, accA.transform.position);
        LR.SetPosition(1, accB.transform.position);
        if (getSize() > maxSize)
            LR.colorGradient = nonValidColor;
        else
            LR.colorGradient = validColor;

        Vector3 d = (accB.transform.position - accA.transform.position).normalized;
        Vector3 p = Vector3.Cross(d, Vector3.forward).normalized;
        Vector2[] t = { accA.transform.position - p * LR.endWidth / 2.0f + d * accA.transform.localScale.x / 2.0f, accA.transform.position + p * LR.endWidth / 2.0f + d * accA.transform.localScale.x / 2.0f,
                        accB.transform.position + p * LR.endWidth / 2.0f - d * accB.transform.localScale.x / 2.0f, accB.transform.position - p * LR.endWidth / 2.0f - d * accB.transform.localScale.x / 2.0f};
        collid.points = t;
    }

    public float getSize()
    {
        return Vector3.Distance(accA.transform.position, accB.transform.position);
    }

    private void OnMouseEnter()
    {
        Debug.Log("Hey Poutre");
    }

    public void updatePoutreTemp()
    {
        Vector3 newPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        newPos.z = 0;

        LR.SetPosition(0, accA.transform.position);
        LR.SetPosition(1, newPos);
        if (Vector3.Distance(accA.transform.position, newPos) > maxSize)
            LR.colorGradient = nonValidColor;
        else
            LR.colorGradient = validColor;
    }
}
