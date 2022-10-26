using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MouseState
{
    construct = 0,
    move = 1,
    destroy = 2
}

public class Mouse : MonoBehaviour
{
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

        if (state == MouseState.construct)
        {
            if (isConstructing)
            {
                poutreTemp.GetComponent<Poutre>().updatePoutreTemp();

                if (Input.GetMouseButtonUp(0))
                {
                    RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
                    Accroche accB;

                    if (hit && hit.collider.CompareTag("Accroche"))
                       accB = hit.collider.GetComponent<Accroche>();
                    else
                    {
                        GameObject newAcc = Instantiate(prefabAccMov);
                        Vector3 newPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                        newPos.z = 0;
                        newAcc.transform.position = newPos;
                        accB = newAcc.GetComponent<Accroche>();
                    }

                    poutreTemp.GetComponent<Poutre>().initPoutre(accB);
                    isConstructing = false;
                }

            }
            else if (Input.GetMouseButtonDown(0))
            {
                RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

                if (hit && hit.collider.CompareTag("Accroche"))
                {
                    Debug.Log("Accroche");
                    poutreTemp = Instantiate(prefabPoutre);
                    poutreTemp.GetComponent<Poutre>().accA = hit.collider.GetComponent<Accroche>();
                    poutreTemp.GetComponent<Poutre>().updatePoutreTemp();
                    isConstructing = true;
                }
            }
            
        }
    }

    public void setState(int newState)
    {
        hammerCursor.SetActive(false);
        handCursor.SetActive(false);
        trashCursor.SetActive(false);

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

            default:
                Debug.Log("/!\\ Mouse state inconnu ! /!\\");
                break;
        }
    }


}
