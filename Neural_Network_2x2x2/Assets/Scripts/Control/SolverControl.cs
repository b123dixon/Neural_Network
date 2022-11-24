using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SolverControl : MonoBehaviour
{
    public Canvas canvas;// the canvas for the UI
    public int cubeCounter = 0;// this is the counter for how much to rest between moves when solving the cube
    //public int loadAmm;
    public GameObject loadText;
    public GameObject display;
    public GameObject cubeDisplay;
    NeuralNetwork nn0;
    int nnCP = 147;
    int count = 0;

    //for slow loads
    bool building;
    int buildPercent;
    int buildAmmount;
    bool loading;
    int index;

    public MiniCube mini;

    public InputField scrambleAmmount;
    public Toggle stopOnSolve;
    public Toggle displayActivation;
    private bool solving = false;
    public InputField loadSpeedInput;
    private int loadSpeed;

    public Slider solveSpeedSlider;
    int moves = 0;

    public string scene;

    public float oldResWidth = 1280, oldResHeighth = 960;


    void Start()
    {
        //creates cube
        mini = new MiniCube();
        cubeDisplay.GetComponent<MiniCubeDisplay>().SetCube(mini);
        cubeDisplay.GetComponent<MiniCubeDisplay>().ShowCube();
        int x;
        for (x = 0; x < 90; x++)
        {
            cubeDisplay.GetComponent<MiniCubeDisplay>().RotateCube(2);
        }
        Vector2 canvasSizer = canvas.GetComponent<RectTransform>().sizeDelta;
        oldResWidth = canvasSizer[0];
        oldResHeighth = canvasSizer[1];
    }

    void Update()
    {
        Vector2 canvasSize = canvas.GetComponent<RectTransform>().sizeDelta;


        if (scene == "solver")
        {
            loadSpeedInput.text = "4000";
            scrambleAmmount.text = "51";
            LoadNN(222);
            scene = "s";
        }
        if (scene == "solvertrained")
        {
            loadSpeedInput.text = "4000";
            scrambleAmmount.text = "51";
            LoadNNTrained(222);
            scene = "s";
        }
        if (scene == "s")
        {
            if (oldResWidth != canvasSize[0] || oldResHeighth != canvasSize[1])
            {
                Vector2 canvasSizer = canvas.GetComponent<RectTransform>().sizeDelta;
                oldResWidth = canvasSizer[0];
                oldResHeighth = canvasSizer[1];


                LoadNN(222);
                float[] inputs = mini.GetInputsFullPiece();
                inputs[nnCP] = 1;
                nn0.RunInputsCubeCP(inputs);
                display.GetComponent<NeuralNetworkDisplay>().NNactivatedFull();
            }
        }

        if (solving)
        {
            cubeCounter--;
            if (cubeCounter <= 0)
            {
                moves++;
                float[] inputs = mini.GetInputsFullPiece();
                inputs[nnCP] = 1;
                nn0.RunInputsCubeCP(inputs);
                if (displayActivation.isOn == true)
                {
                    display.GetComponent<NeuralNetworkDisplay>().NNactivatedFull();
                }

                int ouput = nn0.GetOutput();
                if (ouput < 6)
                {
                    mini.Turn(ouput + 1);
                }
                else
                {
                    nnCP = 147 + ouput - 6;
                    if (ouput == 6)
                    {
                        if (stopOnSolve.isOn == true)
                        {
                            solving = false;
                        }
                        else
                        {
                            count++;
                            Debug.Log("finished for the " + count + " time in " + moves + " moves");
                            ScrambleAmm();
                        }
                        moves = 0;
                    }
                }
                cubeDisplay.GetComponent<MiniCubeDisplay>().UpdateCube();
                //display.GetComponent<NeuralNetworkDisplay>().NNactivated();

                if (solveSpeedSlider != null)
                {
                    float speed = solveSpeedSlider.value;
                    speed--;
                    speed = -speed;

                    cubeCounter = (int)(speed * 100);
                }
                else cubeCounter = 20;
            }
        }
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
                //Debug.Log("Working on load:" + buildPercent + "/" + buildAmmount);
                loadText.GetComponent<Text>().text = ("Working on load:" + buildPercent + "/" + buildAmmount);
            }
        }
        if (building)
        {
            bool done = display.GetComponent<NeuralNetworkDisplay>().CreateNNDisplay(loadSpeed);
            if (done)
            {
                building = false;
                //Debug.Log("Working on build:" + buildPercent + "/" + buildAmmount);
                //Debug.Log("Load Complete");
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

        /**
        if(Input.GetKeyDown("p"))
        {
            MakeGoodNN5_0();
        }
        if (Input.GetKeyDown("l"))
        {
            nn5_0 = GetComponent<Holder>().GetNeuralNetwork(2225);
        }
        if (Input.GetKeyDown("o"))
        {
            if (train)
                train = false;
            else
                train = true;
        }
        if (train)
            TrainNN5_0(nn5_0);
        if (Input.GetKeyDown("i"))
        {
            this.gameObject.GetComponent<NeuralNetworkSave>().SaveNeuralNetwork(nn5_0, 2225);
            this.gameObject.GetComponent<NeuralNetworkSave>().ToString(2225);
        }
        

        if (Input.GetKeyDown("m"))
        {
            nn = GetComponent<Holder>().GetNeuralNetwork(2220);
            nn.AddNetwork(GetComponent<Holder>().GetNeuralNetwork(2221));
            nn.AddNetwork(GetComponent<Holder>().GetNeuralNetwork(22220));
            nn.AddNetwork(GetComponent<Holder>().GetNeuralNetwork(22221));
            nn.AddNetwork(GetComponent<Holder>().GetNeuralNetwork(2223));
            nn.AddNetwork(GetComponent<Holder>().GetNeuralNetwork(22240));
            nn.AddNetwork(GetComponent<Holder>().GetNeuralNetwork(22241));
            nn.AddNetwork(GetComponent<Holder>().GetNeuralNetwork(22242));
            nn.AddNetwork(GetComponent<Holder>().GetNeuralNetwork(2225));
            nn.AddNetwork(GetComponent<Holder>().GetNeuralNetwork(22260));
            nn.AddNetwork(GetComponent<Holder>().GetNeuralNetwork(22261));
            nn.AddNetwork(GetComponent<Holder>().GetNeuralNetwork(22262));
            nn.AddNetwork(GetComponent<Holder>().GetNeuralNetwork(22263));
            //nn.AddNetwork(GetComponent<Holder>().GetNeuralNetwork(22264));
            nn.AddNeuron(4, 6);
            nn.AddNeuron(4, 6);
            nn.AddNeuron(4, 6);
            nn.AddNeuron(4, 6);
            nn.AddNeuron(4, 6);
            nn.AddNeuron(4, 6);
            nn.AddNeuron(4, 6);
            nn.AddNetwork(GetComponent<Holder>().GetNeuralNetwork(222000));

            this.gameObject.GetComponent<NeuralNetworkSave>().SaveNeuralNetwork(nn,222);
            this.gameObject.GetComponent<NeuralNetworkSave>().ToString(222);
        }
        **/

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
    public void ChangeScene(string newScene)
    {
        SceneManager.LoadScene(newScene);
    }

    public void ResetCube()
    {
        mini.Reset();
        cubeDisplay.GetComponent<MiniCubeDisplay>().UpdateCube();
    }
    public void ScrambleAmm()
    {
        string str = scrambleAmmount.text;
        bool worked = int.TryParse(str, out int ammount);
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

    public void LoadNN(int loadIndex)
    {
        if (building == false && loading == false)
        {
            //gets the desired loadSpeed from the user if its invalid then it is changed to 10
            //if its  to low then set it to 1
            if (loadSpeedInput != null)
            {
                string str = loadSpeedInput.text;
                bool worked = int.TryParse(str, out int choice);
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

    public void LoadNNTrained(int loadIndex)
    {
        if (building == false && loading == false)
        {
            //gets the desired loadSpeed from the user if its invalid then it is changed to 10
            //if its  to low then set it to 1
            if (loadSpeedInput != null)
            {
                string str = loadSpeedInput.text;
                bool worked = int.TryParse(str, out int choice);
                if (worked)
                    loadSpeed = choice;
                else loadSpeed = 10;
                if (loadSpeed < 1)
                    loadSpeed = 1;
            }
            else loadSpeed = 10;


            display.GetComponent<NeuralNetworkDisplay>().ChangeNN(null);//why
            nn0 = GetComponent<Holder>().GetNeuralNetworkTrained(loadIndex);
            building = true;
            buildPercent = 0;
            solving = false;
            display.GetComponent<NeuralNetworkDisplay>().ChangeNN(nn0);
            display.GetComponent<NeuralNetworkDisplay>().SetupCreateNNDisplay();
            buildAmmount = nn0.TrueSize();
        }
    }
    public void Solve()
    {
        if (nn0 != null)
        {
            if (solving)
                solving = false;
            else
            {
                if (moves == 0)
                {
                    ScrambleAmm();
                    nnCP = 147;
                }
                solving = true;
            }
        }
    }
}


