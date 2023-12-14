using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class MatrixColumnItem
{
    TheMatrixFallingTextUI textUI;

    List<MatrixItem> items = new List<MatrixItem>();

    int currentColumn = 0;
    int maxColumn = 0;

    string[] matrixSplits;

    float tick = 0;
    private Color _color = Color.green;
    // Use this for initialization
    public void Start(TheMatrixFallingTextUI ui, string[] spt, int step, float x, float w, float h, Color color)
    {
        textUI = ui;

        matrixSplits = spt;
        _color = color;
        for (int y = 0; y < step; y++)
        {
            MatrixItem item = new MatrixItem();
            item.Start(ui, "txt" + x.ToString() + "_" + y.ToString(), x, y, w, h);

            items.Add(item);
        }

        MakeNewRandomInfo();
    }

    void MakeNewRandomInfo()
    {
        currentColumn = 0;

        if (textUI.isRandomY)
            maxColumn = UnityEngine.Random.Range(10, items.Count - 1);
        else
            maxColumn = items.Count - 1;

        tick = 0;
    }

    // Update is called once per frame
    public void Update ()
    {
        tick += Time.deltaTime * textUI.textDownSpeed;
        if (tick >= 1)
        {
            items[currentColumn].txt.text = matrixSplits[UnityEngine.Random.Range(0, matrixSplits.Length)];
            items[currentColumn].txt.color = _color;

            currentColumn += 1;
            if (currentColumn >= maxColumn)
                MakeNewRandomInfo();

            tick = 0;
        }

        int length = items.Count;
        for (int i = 0; i < length; ++i)
        {
            items[i].Update();
        }
	}
}
