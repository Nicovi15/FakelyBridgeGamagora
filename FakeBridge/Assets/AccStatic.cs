using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccStatic : Accroche
{
    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void deletePoutre(Poutre p)
    {
        poutres.Remove(p);
    }

}
