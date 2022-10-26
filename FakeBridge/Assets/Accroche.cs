using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Accroche : MonoBehaviour
{

    public List<Poutre> poutres = new List<Poutre>();
    protected Mouse mouse;

    // Start is called before the first frame update
    public void Start()
    {
        mouse = GameObject.Find("Mouse").GetComponent<Mouse>();
    }

    // Update is called once per frame
    void Update()
    {
        
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

}
