using UnityEngine;
using UnityEngine.UI;

public class NotificationTextBehaviour : MonoBehaviour
{

    void Update()
    {
        Color color = GetComponent<Text>().color;
        if(color.a >= 0)
        {
            color.a = color.a - 10f*Time.deltaTime;
            GetComponent<Text>().color = color;
        }
    }

    public void DisplayText(string text)
    {
        Color color = GetComponent<Text>().color;
        color.a = 30f;

        GetComponent<Text>().color = color;
        GetComponent<Text>().text = text;
    }
}