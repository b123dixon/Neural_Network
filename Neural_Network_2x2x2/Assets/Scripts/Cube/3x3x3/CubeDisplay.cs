using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeDisplay : MonoBehaviour
{
    public Cube cube;
    public GameObject control;
    public bool cubeShown = false;

    public GameObject CenterHelper;
    public GameObject Center;
    private float centerAngleX = 0, centerAngleY = 0, centerAngleZ = 0;
    public GameObject tile;
    GameObject[,] greenTiles = new GameObject[3, 3];
    GameObject[,] blueTiles = new GameObject[3, 3];
    GameObject[,] redTiles = new GameObject[3, 3];
    GameObject[,] orangeTiles = new GameObject[3, 3];
    GameObject[,] whiteTiles = new GameObject[3, 3];
    GameObject[,] yellowTiles = new GameObject[3, 3];

    Color green = new Color(0, 1, 0);
    Color blue = new Color(0, 0, 1);
    Color red = new Color(1, 0, 0);
    Color orange = new Color(1, 0.5f, 0);
    Color white = new Color(1, 1, 1);
    Color yellow = new Color(1, 1, 0);
    Color black = new Color(0, 0, 0);

    public void ShowCube()
    {
        changeColor(Center, 0);
        int x, y;
        if (cubeShown)
        {
            deleteCube();
        }
        for (x = 0; x < 3; x++)
        {
            for (y = 0; y < 3; y++)
            {
                greenTiles[x, y] = Instantiate(tile);
                greenTiles[x, y].transform.localScale = new Vector3(0.2666667f, 0.2666667f, 0.2666667f);
                greenTiles[x, y].transform.position = new Vector3((x - 1) * 0.3333333f, (y - 1) * 0.3333333f, 0.3833333f);
                changeColor(greenTiles[x, y], 1);
                greenTiles[x, y].transform.SetParent(Center.transform, false);
            }
        }
        for (x = 0; x < 3; x++)
        {
            for (y = 0; y < 3; y++)
            {
                blueTiles[x, y] = Instantiate(tile);
                blueTiles[x, y].transform.localScale = new Vector3(0.2666667f, 0.2666667f, 0.2666667f);
                blueTiles[x, y].transform.position = new Vector3((x - 1) * 0.3333333f, (y - 1) * 0.3333333f, -0.3833333f);
                changeColor(blueTiles[x, y], 2);
                blueTiles[x, y].transform.SetParent(Center.transform, false);
            }
        }

        for (x = 0; x < 3; x++)
        {
            for (y = 0; y < 3; y++)
            {
                redTiles[x, y] = Instantiate(tile);
                redTiles[x, y].transform.localScale = new Vector3(0.2666667f, 0.2666667f, 0.2666667f);
                redTiles[x, y].transform.position = new Vector3((x - 1) * 0.3333333f, 0.3833333f, (y - 1) * 0.3333333f);
                changeColor(redTiles[x, y], 3);
                redTiles[x, y].transform.SetParent(Center.transform, false);
            }
        }
        for (x = 0; x < 3; x++)
        {
            for (y = 0; y < 3; y++)
            {
                orangeTiles[x, y] = Instantiate(tile);
                orangeTiles[x, y].transform.localScale = new Vector3(0.2666667f, 0.2666667f, 0.2666667f);
                orangeTiles[x, y].transform.position = new Vector3((x - 1) * 0.3333333f, -0.3833333f, (y - 1) * 0.3333333f);
                changeColor(orangeTiles[x, y], 4);
                orangeTiles[x, y].transform.SetParent(Center.transform, false);
            }
        }
        for (x = 0; x < 3; x++)
        {
            for (y = 0; y < 3; y++)
            {
                whiteTiles[x, y] = Instantiate(tile);
                whiteTiles[x, y].transform.localScale = new Vector3(0.2666667f, 0.2666667f, 0.2666667f);
                whiteTiles[x, y].transform.position = new Vector3(0.3833333f, (x - 1) * 0.3333333f, (y - 1) * 0.3333333f);
                changeColor(whiteTiles[x, y], 5);
                whiteTiles[x, y].transform.SetParent(Center.transform, false);
            }
        }
        for (x = 0; x < 3; x++)
        {
            for (y = 0; y < 3; y++)
            {
                yellowTiles[x, y] = Instantiate(tile);
                yellowTiles[x, y].transform.localScale = new Vector3(0.2666667f, 0.2666667f, 0.2666667f);
                yellowTiles[x, y].transform.position = new Vector3(-0.3833333f, (x - 1) * 0.3333333f, (y - 1) * 0.3333333f);
                changeColor(yellowTiles[x, y], 6);
                yellowTiles[x, y].transform.SetParent(Center.transform, false);
            }
        }
        cubeShown = true;
    }
    public void solveCube()
    {

    }
    public void deleteCube()
    {
        int x, y;
        if (!cubeShown)
        {
            return;
        }
        for (x = 0; x < 3; x++)
        {
            for (y = 0; y < 3; y++)
            {
                Destroy(greenTiles[x, y]);
            }
        }
        for (x = 0; x < 3; x++)
        {
            for (y = 0; y < 3; y++)
            {
                Destroy(blueTiles[x, y]);
            }
        }

        for (x = 0; x < 3; x++)
        {
            for (y = 0; y < 3; y++)
            {
                Destroy(redTiles[x, y]);
            }
        }
        for (x = 0; x < 3; x++)
        {
            for (y = 0; y < 3; y++)
            {
                Destroy(orangeTiles[x, y]);
            }
        }
        for (x = 0; x < 3; x++)
        {
            for (y = 0; y < 3; y++)
            {
                Destroy(whiteTiles[x, y]);
            }
        }
        for (x = 0; x < 3; x++)
        {
            for (y = 0; y < 3; y++)
            {
                Destroy(yellowTiles[x, y]);
            }
        }
        cubeShown = false;
    }
    public void UpdateCube()
    {
        //green side all good
        //corners
        changeColor(greenTiles[0, 0], cube.pieces[15].GetColorFromMid(1));//green orange yellow
        changeColor(greenTiles[2, 0], cube.pieces[14].GetColorFromMid(1));//green orange white
        changeColor(greenTiles[2, 2], cube.pieces[12].GetColorFromMid(1));//green red white
        changeColor(greenTiles[0, 2], cube.pieces[13].GetColorFromMid(1));//green red yellow
        //middle
        changeColor(greenTiles[1, 1], 1);
        //edges
        changeColor(greenTiles[1, 0], cube.pieces[1].GetColorFromMid(1));//green orange
        changeColor(greenTiles[2, 1], cube.pieces[2].GetColorFromMid(1));//green white
        changeColor(greenTiles[1, 2], cube.pieces[0].GetColorFromMid(1));//green red
        changeColor(greenTiles[0, 1], cube.pieces[3].GetColorFromMid(1));//green yellow
        //blue side all good
        //corners
        changeColor(blueTiles[0, 0], cube.pieces[19].GetColorFromMid(2));//blue orange yellow
        changeColor(blueTiles[0, 2], cube.pieces[17].GetColorFromMid(2));//blue red yellow
        changeColor(blueTiles[2, 2], cube.pieces[16].GetColorFromMid(2));//blue red white
        changeColor(blueTiles[2, 0], cube.pieces[18].GetColorFromMid(2));//blue orange white
        //middle
        changeColor(blueTiles[1, 1], 2);
        //edges
        changeColor(blueTiles[1, 0], cube.pieces[5].GetColorFromMid(2));//blue orange 
        changeColor(blueTiles[0, 1], cube.pieces[7].GetColorFromMid(2));//blue yellow
        changeColor(blueTiles[2, 1], cube.pieces[6].GetColorFromMid(2));//blue red 
        changeColor(blueTiles[1, 2], cube.pieces[4].GetColorFromMid(2));//blue white
        //red side all good
        //corners
        changeColor(redTiles[2, 2], cube.pieces[12].GetColorFromMid(3));//red green white
        changeColor(redTiles[2, 0], cube.pieces[16].GetColorFromMid(3));//red blue white
        changeColor(redTiles[0, 0], cube.pieces[17].GetColorFromMid(3));//red blue yellow
        changeColor(redTiles[0, 2], cube.pieces[13].GetColorFromMid(3));//red green yellow
        //middle
        changeColor(redTiles[1, 1], 3);
        //edges
        changeColor(redTiles[1, 2], cube.pieces[0].GetColorFromMid(3));//red green
        changeColor(redTiles[2, 1], cube.pieces[8].GetColorFromMid(3));//red white
        changeColor(redTiles[1, 0], cube.pieces[4].GetColorFromMid(3));//red blue
        changeColor(redTiles[0, 1], cube.pieces[9].GetColorFromMid(3));//red yellow
        //orange side all good
        //corners
        changeColor(orangeTiles[2, 2], cube.pieces[14].GetColorFromMid(4));//orange green white
        changeColor(orangeTiles[2, 0], cube.pieces[18].GetColorFromMid(4));//orange blue white
        changeColor(orangeTiles[0, 0], cube.pieces[19].GetColorFromMid(4));//orange blue yellow
        changeColor(orangeTiles[0, 2], cube.pieces[15].GetColorFromMid(4));//orange green yellow
        //middle
        changeColor(orangeTiles[1, 1], 4);
        //edges
        changeColor(orangeTiles[1, 2], cube.pieces[1].GetColorFromMid(4));//orange green
        changeColor(orangeTiles[2, 1], cube.pieces[10].GetColorFromMid(4));//orange white
        changeColor(orangeTiles[1, 0], cube.pieces[5].GetColorFromMid(4));//orange blue
        changeColor(orangeTiles[0, 1], cube.pieces[11].GetColorFromMid(4));//orange yellow
        //white side all good
        //corners
        changeColor(whiteTiles[2, 2], cube.pieces[12].GetColorFromMid(5));//white green red
        changeColor(whiteTiles[0, 2], cube.pieces[14].GetColorFromMid(5));//white green orange
        changeColor(whiteTiles[2, 0], cube.pieces[16].GetColorFromMid(5));//white blue red
        changeColor(whiteTiles[0, 0], cube.pieces[18].GetColorFromMid(5));//white blue orange
        //middle
        changeColor(whiteTiles[1, 1], 5);
        //edges
        changeColor(whiteTiles[1, 2], cube.pieces[2].GetColorFromMid(5));//white green
        changeColor(whiteTiles[1, 0], cube.pieces[6].GetColorFromMid(5));//white blue
        changeColor(whiteTiles[2, 1], cube.pieces[8].GetColorFromMid(5));//white red
        changeColor(whiteTiles[0, 1], cube.pieces[10].GetColorFromMid(5));//white orange
        //yellow side all good
        //corners
        changeColor(yellowTiles[2, 2], cube.pieces[13].GetColorFromMid(6));//yellow green red
        changeColor(yellowTiles[0, 2], cube.pieces[15].GetColorFromMid(6));//yellow green orange
        changeColor(yellowTiles[2, 0], cube.pieces[17].GetColorFromMid(6));//yellow blue red
        changeColor(yellowTiles[0, 0], cube.pieces[19].GetColorFromMid(6));//yellow blue orange
        //middle
        changeColor(yellowTiles[1, 1], 6);
        //edges
        changeColor(yellowTiles[1, 2], cube.pieces[3].GetColorFromMid(6));//yellow green
        changeColor(yellowTiles[1, 0], cube.pieces[7].GetColorFromMid(6));//yellow blue
        changeColor(yellowTiles[2, 1], cube.pieces[9].GetColorFromMid(6));//yellow red
        changeColor(yellowTiles[0, 1], cube.pieces[11].GetColorFromMid(6));//yellow orange
    }
    private void changeColor(GameObject tileGuy, int colorNum)
    {
        switch (colorNum)
        {
            case 1:
                tileGuy.GetComponent<Renderer>().material.color = green;
                break;
            case 2:
                tileGuy.GetComponent<Renderer>().material.color = blue;
                break;
            case 3:
                tileGuy.GetComponent<Renderer>().material.color = red;
                break;
            case 4:
                tileGuy.GetComponent<Renderer>().material.color = orange;
                break;
            case 5:
                tileGuy.GetComponent<Renderer>().material.color = white;
                break;
            case 6:
                tileGuy.GetComponent<Renderer>().material.color = yellow;
                break;
            case 0:
                tileGuy.GetComponent<Renderer>().material.color = black;
                break;
        }
    }
    public void setCube(Cube newCube)
    {
        cube = newCube;
    }
    public void rotateCube(int input)
    {
        // calculate upAngle

        // four options up down left right 1234 
        if (input == 1)
        {
            CenterHelper.transform.eulerAngles = new Vector3(
                CenterHelper.transform.eulerAngles.x - 1
                , CenterHelper.transform.eulerAngles.y
                , CenterHelper.transform.eulerAngles.z);
        }
        if (input == 2)
        {
            CenterHelper.transform.eulerAngles = new Vector3(
                CenterHelper.transform.eulerAngles.x + 1
                , CenterHelper.transform.eulerAngles.y
                , CenterHelper.transform.eulerAngles.z);
        }
        if (input == 3)
        {
            CenterHelper.transform.eulerAngles = new Vector3(
                CenterHelper.transform.eulerAngles.x
                , CenterHelper.transform.eulerAngles.y + 1
                , CenterHelper.transform.eulerAngles.z);
        }
        if (input == 4)
        {
            CenterHelper.transform.eulerAngles = new Vector3(
                CenterHelper.transform.eulerAngles.x
                , CenterHelper.transform.eulerAngles.y - 1
                , CenterHelper.transform.eulerAngles.z);
        }
        if (input == 5)
        {
            CenterHelper.transform.eulerAngles = new Vector3(
                CenterHelper.transform.eulerAngles.x
                , CenterHelper.transform.eulerAngles.y
                , CenterHelper.transform.eulerAngles.z - 1);
        }
        if (input == 6)
        {
            CenterHelper.transform.eulerAngles = new Vector3(
                CenterHelper.transform.eulerAngles.x
                , CenterHelper.transform.eulerAngles.y
                , CenterHelper.transform.eulerAngles.z + 1);
        }
        centerAngleX = Center.transform.eulerAngles.x;
        centerAngleY = Center.transform.eulerAngles.y;
        centerAngleZ = Center.transform.eulerAngles.z;
        Debug.Log("XYZ" + centerAngleX + "," + centerAngleY + "," + centerAngleZ);
        CenterHelper.transform.eulerAngles = new Vector3(0, 0, 0);
        Center.transform.eulerAngles = new Vector3(centerAngleX, centerAngleY, centerAngleZ);
        //Center.transform.localEulerAngles = new Vector3(centerAngleX, centerAngleY, centerAngleZ);
    }
}