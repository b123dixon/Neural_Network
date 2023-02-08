using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CubeControl : MonoBehaviour
{
    //this really needs some tlc


    public Canvas canvas;
    public int cubeCounter = 0;
    public int loadAmm;
    public GameObject cubeDisplay;
    int count = 0;
    int counter;

    public MiniCube mini;
    public MiniCube mini2;

    //button hell
    public InputField scrambleAmmount;

    public float oldResWidth = 1280, oldResHeighth = 960;

    void Start()
    {
        counter = 0;
        //creates cube
        mini = new MiniCube();
        cubeDisplay.GetComponent<MiniCubeDisplay>().SetCube(mini);
        cubeDisplay.GetComponent<MiniCubeDisplay>().ShowCube();
        int x;
        for (x = 0; x < 90; x++)
        {
            cubeDisplay.GetComponent<MiniCubeDisplay>().RotateCube(2);
        }
    }
    void Update()
    {
        Vector2 canvasSize = canvas.GetComponent<RectTransform>().sizeDelta;
        // move cube
        if (Input.GetKey("w"))
        {
            cubeDisplay.GetComponent<MiniCubeDisplay>().RotateCube(2);
        }
        if (Input.GetKey("s"))
        {
            cubeDisplay.GetComponent<MiniCubeDisplay>().RotateCube(1);
        }
        if (Input.GetKey("a"))
        {
            cubeDisplay.GetComponent<MiniCubeDisplay>().RotateCube(3);
        }
        if (Input.GetKey("d"))
        {
            cubeDisplay.GetComponent<MiniCubeDisplay>().RotateCube(4);
        }
        if (Input.GetKey("q"))
        {
            cubeDisplay.GetComponent<MiniCubeDisplay>().RotateCube(6);
        }
        if (Input.GetKey("e"))
        {
            cubeDisplay.GetComponent<MiniCubeDisplay>().RotateCube(5);
        }
    }
    public void TurnMiniCube(int choice)
    {
        mini.Turn(choice);
        cubeDisplay.GetComponent<MiniCubeDisplay>().UpdateCube();
    }
    public void ResetCube()
    {
        mini.Reset();
        cubeDisplay.GetComponent<MiniCubeDisplay>().UpdateCube();
    }
    public void scrambleAmm()
    {
        string str = scrambleAmmount.text;
        int ammount;
        bool worked = int.TryParse(str, out ammount);
        if (worked)
        {
            if (ammount > 1000)
                ammount = 1000;
            int x;
            for (x = 0; x < ammount; x++)
            {
                mini.Turn(Random.Range(1, 7));
            }
        }
        cubeDisplay.GetComponent<MiniCubeDisplay>().UpdateCube();
    }
}