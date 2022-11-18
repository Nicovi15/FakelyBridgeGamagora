using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Accroche : MonoBehaviour
{
    [Header("Physics Settings")]
    public float mass;
    public Vector3 acceleration;
    public Vector3 vitesse;
    public float dissipation;

    [Header("Poutres")]
    public List<Poutre> poutres = new List<Poutre>();
    protected Mouse mouse;
    protected SpriteRenderer SR;

    [Header("Color settings")]
    [SerializeField]
    protected Color normalColor;

    [SerializeField]
    protected Color highlightColor;

    // Start is called before the first frame update
    public void Start()
    {
        //GameObject.Find("GameManager").GetComponent<GameManager>().addAcc(this);
        mouse = GameObject.Find("Mouse").GetComponent<Mouse>();
        SR = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseEnter()
    {
        SR.color = highlightColor;
    }

    private void OnMouseExit()
    {
        SR.color = normalColor;
    }

    protected void OnMouseDrag()
    {
        switch (mouse.state)
        {
            case MouseState.construct:
                constructAcc();
                break;

            case MouseState.move:
                moveAcc();
                break;

            case MouseState.destroy:
                destroyAcc();
                break;
        }
    }
    protected virtual void constructAcc() { }
    protected virtual void moveAcc() { }
    protected virtual void destroyAcc() { }

    public virtual void AccUpdate(Vector3 gravity){
        acceleration = gravity;
        foreach(Poutre p in poutres)
            acceleration += p.getForce(this);

        acceleration /= mass;
    }


    public virtual void deletePoutre(Poutre p)
    {
        poutres.Remove(p);
        if (poutres.Count <= 0)
        {
            GameObject.Find("GameManager").GetComponent<GameManager>().accs.Remove(this);
            Destroy(this.gameObject);
        }
            
    }


    protected void deleteAcc()
    {
        for(int i = poutres.Count - 1; i >= 0; i--)
            poutres[i].delete();
        GameObject.Find("GameManager").GetComponent<GameManager>().accs.Remove(this);
    }

    public void addPoutre(Poutre p)
    {
        if (!poutres.Contains(p))
            poutres.Add(p);
    }

    public virtual void updatePhysics()
    {
        vitesse += Time.deltaTime * acceleration;
        transform.position += vitesse * Time.deltaTime;
        foreach (Poutre p in poutres)
            p.updateVisu();
    }

    public virtual void updatePhysics(float dT)
    {
        vitesse += dT * acceleration - vitesse * dissipation;
        transform.position += vitesse * dT;
        foreach (Poutre p in poutres)
            p.updateVisu();
    }

}
