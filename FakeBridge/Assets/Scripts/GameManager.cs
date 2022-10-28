using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    Mouse mouse;

    [SerializeField]
    GameObject canvasConstruct;

    [SerializeField]
    GameObject canvasPlay;

    public List<Accroche> accs = new List<Accroche>();
    public List<Poutre> poutres = new List<Poutre>();

    public bool constructPhase = true;


    /* TO DO
     * 
     * Ajout option de creer une accroche a l'intersection de 2 poutres
     * Ajout option de fusion de points d'accroches
     * et autres options d'ergonomie
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

    public void setPlayMode()
    {
        mouse.setState(3);
        canvasConstruct.SetActive(false);
        canvasPlay.SetActive(true);
        constructPhase = false;

        for (int i = poutres.Count - 1; i >= 0; i--)
        {
            if(!poutres[i].isValid)
                poutres[i].delete();
        }
    }

    public void Reset()
    {
        SceneManager.LoadScene(0);
    }

}
