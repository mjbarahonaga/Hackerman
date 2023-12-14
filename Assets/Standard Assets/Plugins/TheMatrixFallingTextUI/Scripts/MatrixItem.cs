using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MatrixItem
{
    TheMatrixFallingTextUI textUI;

    public Text txt=null;

    GameObject textObject;

    public void Start(TheMatrixFallingTextUI ui, string name,float x,float y,float w,float h)
    {
        textUI = ui;

        GameObject go = new GameObject();
        go.name = name;
        RectTransform rect = go.AddComponent<RectTransform>();
        //rect.parent = textUI.rootTransform;
        rect.SetParent(textUI.rootTransform);
        rect.anchorMin = new Vector2(0, 1);
        rect.anchorMax = new Vector2(0, 1);
        rect.offsetMin = new Vector2(0, 0);
        rect.offsetMax = new Vector2(textUI.textWidth, textUI.textHeight);
        rect.localPosition = new Vector3(x * textUI.textWidth - w / 2 + textUI.textWidth / 2, h - y * textUI.textHeight - h / 2 - textUI.textHeight / 2);

        txt = go.AddComponent<Text>();
        txt.text = (x + y).ToString();
        txt.font = textUI.fontName;
        txt.fontSize = textUI.fontSize;
        txt.verticalOverflow = VerticalWrapMode.Overflow;
        txt.horizontalOverflow = HorizontalWrapMode.Overflow;
        txt.alignment = TextAnchor.MiddleCenter;
        txt.color = Color.black;

        textObject = go;

    }

    // Update is called once per frame
    public void Update () {
        Color c = txt.color;
        if (c.r>0) c.r -= Time.deltaTime* textUI.textFadeOutSpeed;
        if (c.g>0) c.g -= Time.deltaTime* textUI.textFadeOutSpeed;
        if (c.b>0) c.b -= Time.deltaTime* textUI.textFadeOutSpeed;
        txt.color = c;
        if (c == Color.black)
            textObject.SetActive(false);
        else
            textObject.SetActive(true);

    }
}
