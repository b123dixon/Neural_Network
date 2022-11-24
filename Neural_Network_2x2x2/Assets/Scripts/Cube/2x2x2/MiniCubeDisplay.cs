using UnityEngine;

public class MiniCubeDisplay : MonoBehaviour
{
    public MiniCube miniCube;
    public GameObject control;
    public bool cubeShown = false;

    public GameObject CenterHelper;
    public GameObject Center;
    private float centerAngleX = 0, centerAngleY = 0, centerAngleZ = 0;
    public GameObject tile;
    GameObject[,] greenTiles = new GameObject[2, 2];
    GameObject[,] blueTiles = new GameObject[2, 2];
    GameObject[,] redTiles = new GameObject[2, 2];
    GameObject[,] orangeTiles = new GameObject[2, 2];
    GameObject[,] whiteTiles = new GameObject[2, 2];
    GameObject[,] yellowTiles = new GameObject[2, 2];

    Color green = new Color(0, 1, 0);
    Color blue = new Color(0, 0, 1);
    Color red = new Color(1, 0, 0);
    Color orange = new Color(1, 0.5f, 0);
    Color white = new Color(1, 1, 1);
    Color yellow = new Color(1, 1, 0);
    Color black = new Color(0, 0, 0);
    // Start is called before the first frame update
    public void ShowCube()
    {
        ChangeColor(Center, 0);
        int x, y;
        if (cubeShown)
        {
            DeleteCube();
        }
        for (x = 0; x < 2; x++)
        {
            for (y = 0; y < 2; y++)
            {
                greenTiles[x, y] = Instantiate(tile);
                greenTiles[x, y].transform.localScale = new Vector3(0.4f, 0.4f, 0.2666667f);
                greenTiles[x, y].transform.position = new Vector3(-0.25f + (x * 0.5f), -0.25f + (y * 0.5f), 0.379f);
                ChangeColor(greenTiles[x, y], 1);
                greenTiles[x, y].transform.SetParent(Center.transform, false);
            }
        }
        for (x = 0; x < 2; x++)
        {
            for (y = 0; y < 2; y++)
            {
                blueTiles[x, y] = Instantiate(tile);
                blueTiles[x, y].transform.localScale = new Vector3(0.4f, 0.4f, 0.2666667f);
                blueTiles[x, y].transform.position = new Vector3(-0.25f + (x * 0.5f), -0.25f + (y * 0.5f), -0.379f);
                ChangeColor(blueTiles[x, y], 2);
                blueTiles[x, y].transform.SetParent(Center.transform, false);
            }
        }

        for (x = 0; x < 2; x++)
        {
            for (y = 0; y < 2; y++)
            {
                redTiles[x, y] = Instantiate(tile);
                redTiles[x, y].transform.localScale = new Vector3(0.4f, 0.2666667f, 0.4f);
                redTiles[x, y].transform.position = new Vector3(-0.25f + (x * 0.5f), 0.379f, -0.25f + (y * 0.5f));
                ChangeColor(redTiles[x, y], 3);
                redTiles[x, y].transform.SetParent(Center.transform, false);
            }
        }
        for (x = 0; x < 2; x++)
        {
            for (y = 0; y < 2; y++)
            {
                orangeTiles[x, y] = Instantiate(tile);
                orangeTiles[x, y].transform.localScale = new Vector3(0.4f, 0.2666667f, 0.4f);
                orangeTiles[x, y].transform.position = new Vector3(-0.25f + (x * 0.5f), -0.379f, -0.25f + (y * 0.5f));
                ChangeColor(orangeTiles[x, y], 4);
                orangeTiles[x, y].transform.SetParent(Center.transform, false);
            }
        }
        for (x = 0; x < 2; x++)
        {
            for (y = 0; y < 2; y++)
            {
                whiteTiles[x, y] = Instantiate(tile);
                whiteTiles[x, y].transform.localScale = new Vector3(0.2666667f, 0.4f, 0.4f);
                whiteTiles[x, y].transform.position = new Vector3(0.379f, -0.25f + (x * 0.5f), -0.25f + (y * 0.5f));
                ChangeColor(whiteTiles[x, y], 5);
                whiteTiles[x, y].transform.SetParent(Center.transform, false);
            }
        }
        for (x = 0; x < 2; x++)
        {
            for (y = 0; y < 2; y++)
            {
                yellowTiles[x, y] = Instantiate(tile);
                yellowTiles[x, y].transform.localScale = new Vector3(0.2666667f, 0.4f, 0.4f);
                yellowTiles[x, y].transform.position = new Vector3(-0.379f, -0.25f + (x * 0.5f), -0.25f + (y * 0.5f));
                ChangeColor(yellowTiles[x, y], 6);
                yellowTiles[x, y].transform.SetParent(Center.transform, false);
            }
        }
        cubeShown = true;
    }
    public void DeleteCube()
    {
        int x, y;
        if (!cubeShown)
        {
            return;
        }
        for (x = 0; x < 2; x++)
        {
            for (y = 0; y < 2; y++)
            {
                Destroy(greenTiles[x, y]);
            }
        }
        for (x = 0; x < 2; x++)
        {
            for (y = 0; y < 2; y++)
            {
                Destroy(blueTiles[x, y]);
            }
        }

        for (x = 0; x < 2; x++)
        {
            for (y = 0; y < 2; y++)
            {
                Destroy(redTiles[x, y]);
            }
        }
        for (x = 0; x < 2; x++)
        {
            for (y = 0; y < 2; y++)
            {
                Destroy(orangeTiles[x, y]);
            }
        }
        for (x = 0; x < 2; x++)
        {
            for (y = 0; y < 2; y++)
            {
                Destroy(whiteTiles[x, y]);
            }
        }
        for (x = 0; x < 2; x++)
        {
            for (y = 0; y < 2; y++)
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
        ChangeColor(greenTiles[0, 0], miniCube.pieces[3].GetColorFromMid(1));//green orange yellow
        ChangeColor(greenTiles[1, 0], miniCube.pieces[2].GetColorFromMid(1));//green orange white
        ChangeColor(greenTiles[1, 1], miniCube.pieces[0].GetColorFromMid(1));//green red white
        ChangeColor(greenTiles[0, 1], miniCube.pieces[1].GetColorFromMid(1));//green red yellow
        //blue side all good
        //corners
        ChangeColor(blueTiles[0, 0], miniCube.pieces[7].GetColorFromMid(2));//blue orange yellow
        ChangeColor(blueTiles[0, 1], miniCube.pieces[5].GetColorFromMid(2));//blue red yellow
        ChangeColor(blueTiles[1, 1], miniCube.pieces[4].GetColorFromMid(2));//blue red white
        ChangeColor(blueTiles[1, 0], miniCube.pieces[6].GetColorFromMid(2));//blue orange white
        //red side all good
        //corners
        ChangeColor(redTiles[1, 1], miniCube.pieces[0].GetColorFromMid(3));//red green white
        ChangeColor(redTiles[1, 0], miniCube.pieces[4].GetColorFromMid(3));//red blue white
        ChangeColor(redTiles[0, 0], miniCube.pieces[5].GetColorFromMid(3));//red blue yellow
        ChangeColor(redTiles[0, 1], miniCube.pieces[1].GetColorFromMid(3));//red green yellow
        //orange side all good
        //corners
        ChangeColor(orangeTiles[1, 1], miniCube.pieces[2].GetColorFromMid(4));//orange green white
        ChangeColor(orangeTiles[1, 0], miniCube.pieces[6].GetColorFromMid(4));//orange blue white
        ChangeColor(orangeTiles[0, 0], miniCube.pieces[7].GetColorFromMid(4));//orange blue yellow
        ChangeColor(orangeTiles[0, 1], miniCube.pieces[3].GetColorFromMid(4));//orange green yellow
        //middle
        //white side all good
        //corners
        ChangeColor(whiteTiles[1, 1], miniCube.pieces[0].GetColorFromMid(5));//white green red
        ChangeColor(whiteTiles[0, 1], miniCube.pieces[2].GetColorFromMid(5));//white green orange
        ChangeColor(whiteTiles[1, 0], miniCube.pieces[4].GetColorFromMid(5));//white blue red
        ChangeColor(whiteTiles[0, 0], miniCube.pieces[6].GetColorFromMid(5));//white blue orange
        //yellow side all good
        //corners
        ChangeColor(yellowTiles[1, 1], miniCube.pieces[1].GetColorFromMid(6));//yellow green red
        ChangeColor(yellowTiles[0, 1], miniCube.pieces[3].GetColorFromMid(6));//yellow green orange
        ChangeColor(yellowTiles[1, 0], miniCube.pieces[5].GetColorFromMid(6));//yellow blue red
        ChangeColor(yellowTiles[0, 0], miniCube.pieces[7].GetColorFromMid(6));//yellow blue orange
    }
    private void ChangeColor(GameObject tileGuy, int colorNum)
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
    public void SetCube(MiniCube newCube)
    {
        miniCube = newCube;
    }
    public void RotateCube(int input)
    {
        // calculate upAngle

        // six options up down left right clockwise and counter clockwise 1234 
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
        CenterHelper.transform.eulerAngles = new Vector3(0, 0, 0);
        Center.transform.eulerAngles = new Vector3(centerAngleX, centerAngleY, centerAngleZ);
        //Center.transform.localEulerAngles = new Vector3(centerAngleX, centerAngleY, centerAngleZ);
    }
}
