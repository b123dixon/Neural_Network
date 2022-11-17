using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdenticalUI : MonoBehaviour
{
    //UI will always take up the same percentage of the screen in the same postition relative to the new screne size
    //was not happy with how unity changes ui sizes for this projects needs

    public Canvas canvas;//this is the canvas that the UI element is attatched to

    //this is the intended resolution
    float baseResWidth = 1280;
    float baseResHeighth = 960;

    //will be saved from the ui element on Start()
    float baseXPos;
    float baseYPos;
    float baseWidth;
    float baseHeighth;
    //these are how we tell if the screne size has changed
    float oldResWidth;
    float oldResHeighth;

    void Start()
    {
        SaveValues();
    }
    void Update()
    {
        //if the canvas size changes, then the UI changes with it
        Vector2 canvasSize = canvas.GetComponent<RectTransform>().sizeDelta;
        if (oldResWidth != canvasSize[0] || oldResHeighth != canvasSize[1])
            FixTransform();
    }

    public void SaveValues()
    {
        //saves the base UI transform elements to base changes off of later
        baseXPos = this.GetComponent<Transform>().localPosition.x;
        baseYPos = this.GetComponent<Transform>().localPosition.y;
        baseWidth = this.gameObject.GetComponent<RectTransform>().transform.localScale.x;
        baseHeighth = this.gameObject.GetComponent<RectTransform>().transform.localScale.y;
    }
    public void FixTransform()
    {
        //update old dimensions to be the new dimensions
        Vector2 canvasSize = canvas.GetComponent<RectTransform>().sizeDelta;
        oldResWidth = canvasSize[0];
        oldResHeighth = canvasSize[1];

        //find correct values for new canvas size
        float x, y;
        float xPos2, yPos2;
        x = baseWidth * (oldResWidth / baseResWidth);
        xPos2 = baseXPos * (oldResWidth / baseResWidth);
        y = baseHeighth * (oldResHeighth / baseResHeighth);
        yPos2 = baseYPos * (oldResHeighth / baseResHeighth);

        //update ui
        this.gameObject.GetComponent<RectTransform>().transform.localScale = new Vector3(x, y);
        this.gameObject.GetComponent<RectTransform>().transform.localPosition = new Vector3(xPos2, yPos2);

    }
}

