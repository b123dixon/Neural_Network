using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PieceControl : MonoBehaviour
{
    public Canvas canvas;
    public int loadAmm;
    public GameObject loadText;
    public GameObject display;
    NeuralNetwork nn0, nn1;
    int count = 0;
    int counter;

    //for slow loads
    bool building;
    int buildPercent;
    int buildAmmount;
    bool loading;
    int index;

    public MiniCube mini;
    public MiniCube mini2;

    private bool solving = false;
    public InputField loadSpeedInput;
    private int loadSpeed;

    public float oldResWidth = 1280, oldResHeighth = 960;

    bool train;

    void Start()
    {
        counter = 0;
        int x;

        nn1 = new NeuralNetwork();
        display.GetComponent<NeuralNetworkDisplay>().ChangeNN(nn1);
        Vector2 canvasSizer = canvas.GetComponent<RectTransform>().sizeDelta;
        oldResWidth = canvasSizer[0];
        oldResHeighth = canvasSizer[1];
    }

    void Update()
    {
        Vector2 canvasSize = canvas.GetComponent<RectTransform>().sizeDelta;

        if (loading)
        {
            nn0 = GetComponent<NeuralNetworkSave>().LoadNeuralNetwork(index, loadSpeed);

            if (nn0 != null)//done
            {
                loading = false;
                display.GetComponent<NeuralNetworkDisplay>().ChangeNN(nn0);
                display.GetComponent<NeuralNetworkDisplay>().SetupCreateNNDisplay();
                Debug.Log("Working on load:" + buildPercent + "/" + buildAmmount);
                Debug.Log("Load Complete");
                loadText.GetComponent<Text>().text = ("Load Complete");
                building = true;
                buildPercent = 0;

            }
            else//keep working
            {
                buildPercent += loadSpeed;
                if (buildPercent > buildAmmount)
                    buildPercent = buildAmmount;
                loadText.GetComponent<Text>().text = ("Working on load:" + buildPercent + "/" + buildAmmount);
            }
        }
        if (building)
        {
            bool done = display.GetComponent<NeuralNetworkDisplay>().CreateNNDisplay(loadSpeed);
            if (done)
            {
                building = false;
                loadText.GetComponent<Text>().text = ("Build Complete");
            }
            else
            {
                buildPercent += loadSpeed;
                if (buildPercent > buildAmmount)
                    buildPercent = buildAmmount;
                loadText.GetComponent<Text>().text = ("Working on build:" + buildPercent + "/" + buildAmmount);
            }
        }
    }
    public void LoadNN(int loadIndex)
    {
        if (building == false && loading == false)
        {
            //gets the desired loadSpeed from the user if its invalid then it is changed to 10
            //if its  to low then set it to 1
            if (loadSpeedInput != null)
            {
                string str = loadSpeedInput.text;
                int choice;
                bool worked = int.TryParse(str, out choice);
                if (worked)
                    loadSpeed = choice;
                else loadSpeed = 10;
                if (loadSpeed < 1)
                    loadSpeed = 1;
            }
            else loadSpeed = 10;


            display.GetComponent<NeuralNetworkDisplay>().ChangeNN(null);//why
            nn0 = GetComponent<Holder>().GetNeuralNetwork(loadIndex);
            building = true;
            buildPercent = 0;
            solving = false;
            display.GetComponent<NeuralNetworkDisplay>().ChangeNN(nn0);
            display.GetComponent<NeuralNetworkDisplay>().SetupCreateNNDisplay();
            buildAmmount = nn0.TrueSize();
        }
    }
}
