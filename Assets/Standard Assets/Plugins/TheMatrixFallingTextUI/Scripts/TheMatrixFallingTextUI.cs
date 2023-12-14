using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class TheMatrixFallingTextUI : MonoBehaviour {
    public Font fontName;
    public int fontSize=11;

    public int textWidth=14;
    public int textHeight=12;

    public string matrixText = "0123456789ABCDEF";

    public float textDownSpeed = 30f;
    public float textFadeOutSpeed = 0.02f;

    [HideInInspector]
    public RectTransform rootTransform;
    string[] matrixSplits;

    public Color Color = Color.black;
    //GameObject[] textObjects;
    List<MatrixColumnItem> columnItems = new List<MatrixColumnItem>();

    public bool isFullScreen = false;
    public bool isRandomY = true;

	// Use this for initialization
	void Start () {

        Image img = GetComponent<Image>();
        if (img!=null)
        {
            img.color = Color.black;
        }
        
        matrixSplits = new string[matrixText.Length];
        for(int i=0;i<matrixText.Length;i++)
        {
            matrixSplits[i] = matrixText.Substring(i, 1);
        }

        rootTransform = gameObject.GetComponent<RectTransform>();


        MakeTextControls();

    }

    void MakeTextControls()
    {
        int stepw =0;
        int steph = 0;
        int w = 0;
        int h = 0;

        if (isFullScreen)
        {
            stepw = Screen.width / textWidth + 1;
            steph = Screen.height / textHeight + 1;

            w = Screen.width;
            h = Screen.height;
        }
        else
        {
            stepw = (int)((rootTransform.offsetMax.x - rootTransform.offsetMin.x) / textWidth) + 1;
            steph = (int)((rootTransform.offsetMax.y - rootTransform.offsetMin.y) / textHeight) + 1;
            w = (int)(rootTransform.offsetMax.x - rootTransform.offsetMin.x);
            h = (int)(rootTransform.offsetMax.y - rootTransform.offsetMin.y);
        }

        for (int x=0;x<stepw;x++)
        {
            MatrixColumnItem columnItem = new MatrixColumnItem();
            columnItem.Start(this, matrixSplits, steph, x, w, h, Color);

            columnItems.Add(columnItem);

        }

    }
	
	// Update is called once per frame
	void Update () {
	    foreach(MatrixColumnItem item in columnItems)
        {
            item.Update();
        }
	}
}
