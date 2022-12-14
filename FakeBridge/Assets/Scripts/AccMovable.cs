using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccMovable : Accroche
{
    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        GameObject.Find("GameManager").GetComponent<GameManager>().addAcc(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected override void moveAcc()
    {
        Vector3 oldPos = transform.position;
        Vector3 newPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        newPos.z = 0;
        transform.position = newPos;

        foreach (Poutre p in poutres)
            p.updateVisu();
    }

    private void OnMouseDown()
    {
        if(mouse.state == MouseState.destroy)
            deleteAcc();
    }

}
