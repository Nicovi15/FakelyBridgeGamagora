using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MouseState
{
    construct = 0,
    move = 1,
    destroy = 2,
    play = 3
}

public class Mouse : MonoBehaviour
{
    [SerializeField]
    GameManager GM;

    [Header("Cursor state")]
    public MouseState state;

    [Header("Cursor settings")]
    [SerializeField]
    GameObject mouseCursor;

    [SerializeField]
    GameObject hammerCursor;

    [SerializeField]
    GameObject handCursor;

    [SerializeField]
    GameObject trashCursor;

    [SerializeField]
    GameObject playCursor;

    bool isConstructing;
    GameObject poutreTemp;

    [SerializeField]
    GameObject prefabPoutre;

    [SerializeField]
    GameObject prefabAccMov;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        mouseCursor.transform.position = Input.mousePosition;

        if (!GM.constructPhase)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
            setState(0);
        if (Input.GetKeyDown(KeyCode.Alpha2))
            setState(1);
        if (Input.GetKeyDown(KeyCode.Alpha3))
            setState(2);
        if (Input.GetKeyDown(KeyCode.Return))
        {
            GM.setPlayMode();
            return;
        }
            

        if (state == MouseState.construct)
        {
            if (isConstructing)
            {
                poutreTemp.GetComponent<Poutre>().updatePoutreTemp();

                if (Input.GetMouseButtonDown(1))
                {
                    Destroy(poutreTemp);
                    isConstructing = false;
                }
                else if (Input.GetMouseButtonUp(0))
                {
                    RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
                    Accroche accB;

                    if (hit)
                    {
                        if(hit.collider.CompareTag("Accroche"))
                            accB = hit.collider.GetComponent<Accroche>();
                        else /*if (hit.collider.CompareTag("Poutre"))*/
                        {
                            Poutre currentPoutre = hit.collider.GetComponent<Poutre>();
                            GameObject newAcc = Instantiate(prefabAccMov);
                            Vector3 newPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                            newPos.z = 0;
                            newAcc.transform.position = newPos;

                            GameObject poutreA = Instantiate(prefabPoutre);
                            poutreA.GetComponent<Poutre>().initPoutre(currentPoutre.accA, newAcc.GetComponent<AccMovable>());

                            GameObject poutreB = Instantiate(prefabPoutre);
                            poutreB.GetComponent<Poutre>().initPoutre(currentPoutre.accB, newAcc.GetComponent<AccMovable>());

                            currentPoutre.delete();
                            accB = newAcc.GetComponent<Accroche>();
                        }
                    }
                    else
                    {
                        GameObject newAcc = Instantiate(prefabAccMov);
                        Vector3 newPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                        newPos.z = 0;
                        newAcc.transform.position = newPos;
                        accB = newAcc.GetComponent<Accroche>();
                    }

                    if(accB == poutreTemp.GetComponent<Poutre>().accA)
                        Destroy(poutreTemp);
                    else
                    {
                        bool valid = true;
                        foreach(Poutre p in accB.poutres)
                        {
                            if ((p.accA == poutreTemp.GetComponent<Poutre>().accA  && p.accB == accB) || 
                                (p.accA == accB && p.accB == poutreTemp.GetComponent<Poutre>().accA))
                            {
                                Destroy(poutreTemp);
                                valid = false;
                                break;
                            }
                        }
                        if(valid)
                            poutreTemp.GetComponent<Poutre>().initPoutre(accB);
                    }
                        

                    isConstructing = false;
                }

            }
            else if (Input.GetMouseButtonDown(0))
            {
                RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

                if (hit)
                {
                    if (hit.collider.CompareTag("Accroche"))
                    {
                        poutreTemp = Instantiate(prefabPoutre);
                        poutreTemp.GetComponent<Poutre>().accA = hit.collider.GetComponent<Accroche>();
                        poutreTemp.GetComponent<Poutre>().updatePoutreTemp();
                        isConstructing = true;
                    }
                    else if (hit.collider.CompareTag("Poutre"))
                    {
                        Poutre currentPoutre = hit.collider.GetComponent<Poutre>();
                        GameObject newAcc = Instantiate(prefabAccMov);
                        Vector3 newPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                        newPos.z = 0;
                        newAcc.transform.position = newPos;

                        GameObject poutreA = Instantiate(prefabPoutre);
                        poutreA.GetComponent<Poutre>().initPoutre(currentPoutre.accA, newAcc.GetComponent<AccMovable>());

                        GameObject poutreB = Instantiate(prefabPoutre);
                        poutreB.GetComponent<Poutre>().initPoutre(currentPoutre.accB, newAcc.GetComponent<AccMovable>());

                        currentPoutre.delete();
                    }
                    
                }
            }
            
        }
    }

    public void setState(int newState)
    {
        hammerCursor.SetActive(false);
        handCursor.SetActive(false);
        trashCursor.SetActive(false);
        playCursor.SetActive(false);

        switch (newState)
        {
            case 0:
                state = MouseState.construct;
                hammerCursor.SetActive(true);
                break;

            case 1:
                state = MouseState.move;
                handCursor.SetActive(true);
                break;

            case 2:
                state = MouseState.destroy;
                trashCursor.SetActive(true);
                break;

            case 3:
                state = MouseState.play;
                playCursor.SetActive(true);
                break;

            default:
                Debug.Log("/!\\ Mouse state inconnu ! /!\\");
                break;
        }
    }


}
