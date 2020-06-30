using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerBehaviour : NetworkBehaviour
{
    Player player = null;

    public Color color;

    public GameObject settlementPrefab;
    public GameObject roadPrefab;
    public GameObject cityPrefab;

    List<GameObject> settlements = new List<GameObject>();
    List<GameObject> cities = new List<GameObject>();
    List<GameObject> roads = new List<GameObject>();

    // Start is called before the first frame update
    public override void OnStartClient()
    {
        FindObjectOfType<GameManager>().AddPlayer(gameObject);

        for (int i = 0; i < 5; i++)
        {
            settlements.Add(Instantiate(settlementPrefab, transform));
        }
        for (int i = 0; i < 5; i++)
        {
            cities.Add(Instantiate(cityPrefab, transform));
        }
        for (int i = 0; i < 15; i++)
        {
            roads.Add(Instantiate(roadPrefab, transform));
        }
    }

    public void SetPlayer(Player player)
    {
        this.player = player;
    }

    // Update is called once per frame
    void Update()
    {
        if(player != null)
        {
            // Update materials to this player's color
            settlementPrefab.GetComponent<Renderer>().sharedMaterial.SetColor("_Color", color);
            roadPrefab.GetComponent<Renderer>().sharedMaterial.SetColor("_Color", color);
            cityPrefab.GetComponent<Renderer>().sharedMaterial.SetColor("_Color", color);

            float spacing = 0;
            // Render the items in settlements, cities and roads
            for (int i = 0; i < player.storeSettlementNum; i++)
            {
                settlements[i].SetActive(true);
                settlements[i].transform.localPosition = new Vector3(spacing, 0, 0);
                spacing += 1.4f;
            }
            for(int i = player.storeSettlementNum; i < 5; i++)
            {
                settlements[i].SetActive(false);
            }

            spacing = 0;
            for (int i = 0; i < player.storeCityNum; i++)
            {
                cities[i].SetActive(true);
                cities[i].transform.localPosition = new Vector3(spacing, 0, 3);
                spacing += 1.4f;
            }
            for (int i = player.storeCityNum; i < 5; i++)
            {
                cities[i].SetActive(false);
            }

            spacing = 0;
            for (int i = 0; i < player.storeRoadNum; i++)
            {
                roads[i].SetActive(true);
                roads[i].transform.localPosition = new Vector3(spacing, 0, 5);
                spacing += 0.8f;
            }
            for (int i = player.storeRoadNum; i < 15; i++)
            {
                roads[i].SetActive(false);
            }
        }
    }
}
