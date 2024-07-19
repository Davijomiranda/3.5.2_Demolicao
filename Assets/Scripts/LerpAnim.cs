using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LerpAnim : MonoBehaviour
{
    public Vector2 posIni;
    public Vector2 posFinish;

    public bool active = false;
    public bool up = false;

    public float speed;

    RectTransform rect;

    private void Start()
    {
        rect = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (active)
        {
            if (!up)
            {
                rect.localPosition = Vector2.Lerp(rect.localPosition, posFinish, speed * Time.deltaTime);
                if (Vector2.Distance(rect.localPosition, posFinish) < 0.01f)
                {
                    //active = false;
                    Debug.Log("Passou aqui");
                    up = true;
                }
            }
            else
            {
                rect.localPosition = Vector2.Lerp(rect.localPosition, posIni, speed * Time.deltaTime);
                if (Vector2.Distance(rect.localPosition, posIni) < 0.01f)
                {
                    active = false;
                    up = false;
                }
            }
        }
    }

    public void Button()
    {
        active = true;
    }
    private void voltaButton()
    {
        active = true;
    }
}
