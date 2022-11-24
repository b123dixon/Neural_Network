using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Control : MonoBehaviour
{
    public GameObject display;
    public GameObject cubeDisplay;
    NeuralNetwork nn;

    public MiniCube mini;
    public Cube cube;

    //button hell
    public InputField scrambleAmmount;
    public InputField scrambleInstructions;
    public InputField connectionColl1;
    public InputField connectionRow1;
    public InputField connectionColl2;
    public InputField connectionRow2;
    public InputField connectionValue;
    public InputField neuronColl;
    public InputField neuronRow;
    public InputField dummyColl;
    public InputField neurToCollColl1;
    public InputField neurToCollRow1;
    public InputField neurToCollColl;
    public InputField collToCollColl1;
    public InputField collToCollColl2;
    void Start()
    {
        mini = new MiniCube();
        //cube = new Cube();
        //cubeDisplay.GetComponent<CubeDisplay>().setCube(cube);
        cubeDisplay.GetComponent<MiniCubeDisplay>().SetCube(mini);
        //cubeDisplay.GetComponent<CubeDisplay>().ShowCube();
        cubeDisplay.GetComponent<MiniCubeDisplay>().ShowCube();
    }

    void Update()
    {
        if (Input.GetKeyDown("1"))
        {
            nn = new NeuralNetwork();
            nn.AddCollumn(0);
            nn.AddCollumn(1);
            nn.AddCollumn(2);

            nn.AddNeuron(0, 0);
            nn.AddNeuron(0, 1);
            nn.AddNeuron(0, 2);
            nn.AddNeuron(1, 0);
            nn.AddNeuron(1, 1);
            nn.AddNeuron(2, 0);
            nn.AddNeuron(2, 1);

            nn.AddConnection(0, 0, 1, 0, 1);
            nn.AddConnection(0, 0, 1, 1, 2);
            nn.AddConnection(0, 0, 2, 0, 0);
            nn.AddConnection(0, 0, 2, 1, 1);

            nn.AddConnection(0, 1, 1, 0, 0.2f);
            nn.AddConnection(0, 1, 1, 1, -0.4f);
            nn.AddConnection(0, 1, 2, 0, 0.3f);
            nn.AddConnection(0, 1, 2, 1, -0.5f);

            nn.AddConnection(0, 2, 1, 0, 0.5f);
            nn.AddConnection(0, 2, 1, 1, 0.2f);
            nn.AddConnection(0, 2, 2, 0, -0.8f);
            nn.AddConnection(0, 2, 2, 1, 0.1f);

            nn.AddConnection(1, 0, 2, 0, 0.2f);
            nn.AddConnection(1, 0, 2, 1, 0.6f);
            nn.AddConnection(1, 1, 2, 0, -0.7f);
            nn.AddConnection(1, 1, 2, 1, 0.3f);

            display.GetComponent<NeuralNetworkDisplay>().ChangeNN(nn);
            display.GetComponent<NeuralNetworkDisplay>().CreateNNDisplay();
            display.GetComponent<NeuralNetworkDisplay>().NNactivated();

        }
        if (Input.GetKeyDown("2"))
        {
            display.GetComponent<NeuralNetworkDisplay>().DeleteNNDisplay();
        }
        if (Input.GetKeyDown("3"))
        {
            display.GetComponent<NeuralNetworkSave>().SaveNeuralNetwork(nn, 1);
            nn = new NeuralNetwork();
            nn = display.GetComponent<NeuralNetworkSave>().LoadNeuralNetwork(1); //problem here//////////////////
        }
        if (Input.GetKeyDown("4"))
        {
            nn = new NeuralNetwork();
            nn.AddCollumn(0);
            nn.AddCollumn(1);
            nn.AddCollumn(2);

            nn.AddNeuron(0, 0);
            nn.AddNeuron(0, 1);
            nn.AddNeuron(0, 2);

            nn.AddNeuron(1, 0);
            nn.AddNeuron(1, 1);
            nn.AddNeuron(1, 2);

            nn.AddNeuron(2, 0);
            nn.AddNeuron(2, 1);



            nn.AddConnection(0, 0, 1, 0, 0.15f);
            nn.AddConnection(0, 0, 1, 1, 0.25f);

            nn.AddConnection(0, 1, 1, 0, 0.2f);
            nn.AddConnection(0, 1, 1, 1, 0.3f);

            nn.AddConnection(0, 2, 1, 0, 0.35f);
            nn.AddConnection(0, 2, 1, 1, 0.35f);

            nn.AddConnection(1, 0, 2, 0, 0.4f);
            nn.AddConnection(1, 0, 2, 1, 0.5f);

            nn.AddConnection(1, 1, 2, 0, 0.45f);
            nn.AddConnection(1, 1, 2, 1, 0.55f);

            nn.AddConnection(1, 2, 2, 0, 0.6f);
            nn.AddConnection(1, 2, 2, 1, 0.6f);

            display.GetComponent<NeuralNetworkDisplay>().ChangeNN(nn);
            display.GetComponent<NeuralNetworkDisplay>().CreateNNDisplay();
            display.GetComponent<NeuralNetworkDisplay>().NNactivated();

            float[] inputs = new float[] { 0.05f, 0.1f, 1f };

            nn.RunInputs(inputs);


            /*Debug.Log(nn.getNeuron(0, 0).getValue());
            Debug.Log(nn.getNeuron(0, 1).getValue());
            Debug.Log(nn.getNeuron(0, 2).getValue());
            Debug.Log("||");
            Debug.Log(nn.getNeuron(1, 0).getValue());
            Debug.Log(nn.getNeuron(1, 1).getValue());
            Debug.Log(nn.getNeuron(1, 2).getValue());
            Debug.Log("||");
            Debug.Log(nn.getNeuron(2, 0).getValue());
            Debug.Log(nn.getNeuron(2, 1).getValue());*/
            display.GetComponent<NeuralNetworkDisplay>().NNactivated();


        }
        if (Input.GetKeyDown("5"))
        {
            float[] inputs = new float[] { 0.05f, 0.1f, 1f };

            nn.RunInputs(inputs);
            float[] desiredOutputs = new float[] { 0.01f, 0.99f };
            nn.BackPropegate(desiredOutputs);
        }
        if (Input.GetKeyDown("6"))
        {
            float[] inputs = new float[] { 0.05f, 0.1f, 1f };

            nn.RunInputs(inputs);
            Debug.Log(nn.GetNeuron(2, 0).GetValue());
            Debug.Log(nn.GetNeuron(2, 1).GetValue());

            nn.AddDesiredChanges(0.5f);

            inputs = new float[] { 0.05f, 0.1f, 1f };

            nn.RunInputs(inputs);
            Debug.Log(nn.GetNeuron(2, 0).GetValue());
            Debug.Log(nn.GetNeuron(2, 1).GetValue());

            display.GetComponent<NeuralNetworkDisplay>().ChangeNN(nn);
            display.GetComponent<NeuralNetworkDisplay>().CreateNNDisplay();
            display.GetComponent<NeuralNetworkDisplay>().NNactivated();

        }
        if (Input.GetKeyDown("7"))
        {
            Debug.Log(nn.GetConnection(1, 0, 2, 0).GetValue());
            Debug.Log(nn.GetConnection(1, 1, 2, 0).GetValue());
            Debug.Log(nn.GetConnection(1, 0, 2, 1).GetValue());
            Debug.Log(nn.GetConnection(1, 1, 2, 1).GetValue());
            Debug.Log("ll");
            Debug.Log(nn.GetConnection(0, 0, 1, 0).GetValue());
            Debug.Log(nn.GetConnection(0, 1, 1, 0).GetValue());
            Debug.Log(nn.GetConnection(0, 0, 1, 1).GetValue());
            Debug.Log(nn.GetConnection(0, 1, 1, 1).GetValue());

        }
        if (Input.GetKey("8"))
        {
            float[] inputs = new float[] { 0.05f, 0.1f, 1f };

            nn.RunInputs(inputs);
            float[] desiredOutputs = new float[] { 0.01f, 0.99f };
            nn.BackPropegate(desiredOutputs);

            nn.AddDesiredChanges(0.5f);

            inputs = new float[] { 0.05f, 0.1f, 1f };

            nn.RunInputs(inputs);
            Debug.Log(nn.GetNeuron(2, 0).GetValue());
            Debug.Log(nn.GetNeuron(2, 1).GetValue());

        }
        if (Input.GetKeyDown("9"))
        {
            Debug.Log(nn.GetConnection(1, 0, 2, 0).GetValue());
            Debug.Log(nn.GetConnection(1, 1, 2, 0).GetValue());
            Debug.Log(nn.GetConnection(1, 0, 2, 1).GetValue());
            Debug.Log(nn.GetConnection(1, 1, 2, 1).GetValue());
            Debug.Log("ll");
            Debug.Log(nn.GetConnection(0, 0, 1, 0).GetValue());
            Debug.Log(nn.GetConnection(0, 1, 1, 0).GetValue());
            Debug.Log(nn.GetConnection(0, 0, 1, 1).GetValue());
            Debug.Log(nn.GetConnection(0, 1, 1, 1).GetValue());

        }
        if (Input.GetKey("w"))
        {
            //cubeDisplay.GetComponent<CubeDisplay>().rotateCube(2);
            cubeDisplay.GetComponent<MiniCubeDisplay>().RotateCube(2);
        }
        if (Input.GetKey("s"))
        {
            //cubeDisplay.GetComponent<CubeDisplay>().rotateCube(1);
            cubeDisplay.GetComponent<MiniCubeDisplay>().RotateCube(1);
        }
        if (Input.GetKey("a"))
        {
            //cubeDisplay.GetComponent<CubeDisplay>().rotateCube(3);
            cubeDisplay.GetComponent<MiniCubeDisplay>().RotateCube(3);
        }
        if (Input.GetKey("d"))
        {
            //cubeDisplay.GetComponent<CubeDisplay>().rotateCube(4);
            cubeDisplay.GetComponent<MiniCubeDisplay>().RotateCube(4);
        }
        if (Input.GetKey("q"))
        {
            //cubeDisplay.GetComponent<CubeDisplay>().rotateCube(6);
            cubeDisplay.GetComponent<MiniCubeDisplay>().RotateCube(6);
        }
        if (Input.GetKey("e"))
        {
            //cubeDisplay.GetComponent<CubeDisplay>().rotateCube(5);
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

    public void ScrambleIns()
    {
        string str = scrambleInstructions.text;
        string move;
        int num;
        while (str != "")
        {
            move = str.Substring(0, 1);
            str.Remove(0, 1);

            bool worked = int.TryParse(move, out num);
            if (worked)
                mini.Turn(num);
        }
    }
    public void ScrambleAmm()
    {
        string str = scrambleAmmount.text;
        int ammount;
        bool worked = int.TryParse(str, out ammount);
        if (worked)
        {
            int x;
            for (x = 0; x < ammount; x++)
            {
                mini.Turn(Random.Range(1, 7));
            }
        }
    }
}
