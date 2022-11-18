using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Scene Objects")]
    public List<Accroche> accs = new List<Accroche>();
    public List<Poutre> poutres = new List<Poutre>();

    [Header("Physics Settings")]
    public int coefDt;
    public float gravity;

    [Header("Input")]
    [SerializeField]
    Mouse mouse;

    [Header("Canvas")]
    [SerializeField]
    GameObject canvasConstruct;

    [SerializeField]
    GameObject canvasPlay;

    

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
        if (constructPhase)
            return;

        float dT = Time.deltaTime / ((float)coefDt);

        for (int i = 0; i < coefDt; i++)
        {
            // Calcul de l'acceleration
            foreach (Accroche a in accs)
                a.AccUpdate(a.mass * gravity * Vector3.down);

            // Mis a jour de la vitesse et de la position
            foreach (Accroche a in accs)
                a.updatePhysics(dT);

            // Suppression des poutres non valides
            for (int p = poutres.Count - 1; p >= 0; p--)
                if (!poutres[p].isValid)
                    poutres[p].delete();
        }
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
            if (!poutres[i].isValid)
                poutres[i].delete();
            else
                poutres[i].saveDefaultSize();
        }
    }

    public void Reset()
    {
        SceneManager.LoadScene(0);
    }

}
