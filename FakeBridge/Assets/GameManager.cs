using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public List<Accroche> accs = new List<Accroche>();
    public List<Poutre> poutres = new List<Poutre>();

    public bool constructPhase = true;


    /* TO DO
     * 
     * Phase construct - phase jeu
     * Check des poutres non valides et destruction
     * Raccourci clavier pour les states de la souris
     * 
     * 
     * 
     * 
     * 
     * 
     * 
     * 
     * 
     */

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void addAcc(Accroche a)
    {
        if (!accs.Contains(a))
            accs.Add(a);
    }

    public void addPoutre(Poutre p)
    {
        if (!poutres.Contains(p))
            poutres.Add(p);
    }

}
