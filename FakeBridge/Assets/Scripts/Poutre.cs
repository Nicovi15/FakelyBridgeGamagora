using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
[RequireComponent(typeof(PolygonCollider2D))]
public class Poutre : MonoBehaviour
{

    [Header("Physics settings")]
    public float maxSize;
    public float defaultSize;
    public float coefK;

    [Header("Accroche settings")]
    [SerializeField]
    GameObject poutrePrefab;
    [SerializeField]
    GameObject accMovePrefab;
    public Accroche accA, accB;

    [Header("Display settings")]
    public Gradient validColor;
    public Gradient validColorHighlight;
    public Gradient nonValidColor;
    public Gradient nonValidColorHighlight;
    public Gradient playModeTint;


    public bool isValid = false;
    public bool isHighlighted = false;

    bool constructPhase = false;
    LineRenderer LR;
    PolygonCollider2D collid;

    private void Awake()
    {
        GameObject.Find("GameManager").GetComponent<GameManager>().addPoutre(this);
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

        this.accA.addPoutre(this);
        this.accB.addPoutre(this);
        updateVisu();
    }

    public void initPoutre(Accroche accB)
    {
        this.accB = accB;
        this.accA.addPoutre(this);
        this.accB.addPoutre(this);
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

        isValid = !(getSize() > maxSize);
        updateColor();

        Vector3 d = (accB.transform.position - accA.transform.position).normalized;
        Vector3 p = Vector3.Cross(d, Vector3.forward).normalized;
        Vector2[] t = { accA.transform.position - p * LR.endWidth / 2.0f + d * accA.transform.localScale.x / 2.0f, accA.transform.position + p * LR.endWidth / 2.0f + d * accA.transform.localScale.x / 2.0f,
                        accB.transform.position + p * LR.endWidth / 2.0f - d * accB.transform.localScale.x / 2.0f, accB.transform.position - p * LR.endWidth / 2.0f - d * accB.transform.localScale.x / 2.0f};
        collid.points = t;
    }

    void updateColor()
    {
        if (constructPhase)
        {
            LR.material.color = Color.HSVToRGB(0.5f - (getSize() / maxSize) * 0.5f, 1.0f, 1.0f);
        }
        else
        {
            if (isHighlighted)
            {
                if (isValid)
                    LR.colorGradient = validColorHighlight;
                else
                    LR.colorGradient = nonValidColorHighlight;
            }
            else
            {
                if (isValid)
                    LR.colorGradient = validColor;
                else
                    LR.colorGradient = nonValidColor;
            }
        }
        
    }

    public float getSize()
    {
        return Vector3.Distance(accA.transform.position, accB.transform.position);
    }

    private void OnMouseEnter()
    {
        isHighlighted = true;
        updateColor();
    }

    private void OnMouseExit()
    {
        isHighlighted = false;
        updateColor();
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

    public void delete()
    {
        GameObject.Find("GameManager").GetComponent<GameManager>().poutres.Remove(this);
        accA.deletePoutre(this);
        if(accB)
            accB.deletePoutre(this);
        if(this.gameObject)
            Destroy(this.gameObject);
        
    }

    private void OnMouseDown()
    {
        switch (GameObject.Find("Mouse").GetComponent<Mouse>().state)
        {
            case MouseState.construct:
                break;

            case MouseState.destroy:
                delete();
                break;

            default:
                break;
        }
    }

    public void saveDefaultSize()
    {
        defaultSize = getSize();
        LR.colorGradient = playModeTint;
        constructPhase = true;
    }

    public Vector3 getForce(Accroche a)
    {
        float d = coefK * (defaultSize - getSize());
        if(a == accA)
            return -(accB.transform.position - accA.transform.position).normalized * d;
        else
            return -(accA.transform.position - accB.transform.position).normalized * d;
    }
}
