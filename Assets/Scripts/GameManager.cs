using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public Camera mainCamera;
    public BoardBehaviour board;

    public Text inspectionText;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        board.SetupBoardGrid();
        board.RandomizeTileTypes();
        board.SetupChanceTokens();
        board.MoveRobberToDesert();
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        bool hitSuccess = Physics.Raycast(ray, out hit, 100f);

        if (hitSuccess)
        {
            if (hit.collider.gameObject.GetComponent<Inspectable>() != null)
            {
                inspectionText.text = hit.collider.gameObject.GetComponent<Inspectable>().getInspectionText();
            }
            else
            {
                inspectionText.text = "";
            }
        }
    }
}
