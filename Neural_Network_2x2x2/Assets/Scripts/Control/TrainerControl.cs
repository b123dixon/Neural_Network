using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TrainerControl : MonoBehaviour
{
    bool training = false;
    int activeID = 0;


    public Canvas canvas;
    public int loadAmm;
    public GameObject loadText;
    public GameObject display;
    NeuralNetwork nnDisplay;
    NeuralNetwork nn;
    NeuralNetwork nn0;
    NeuralNetwork nn1;
    NeuralNetwork nn2_0, nn2_1;
    NeuralNetwork nn3;
    NeuralNetwork nn4_0, nn4_1, nn4_2;
    NeuralNetwork nn5;
    NeuralNetwork nn6_0, nn6_1, nn6_2, nn6_3, nn6_4;
    NeuralNetwork nnCheckpoint;
    int nnCP = 147;
    int count = 0;
    int counter;

    //for slow loads
    bool building;
    int buildPercent;
    int buildAmmount;
    bool loading;
    int index;



    public InputField loadSpeedInput;
    private int loadSpeed;

    public string scene;

    public float oldResWidth = 1280, oldResHeighth = 960;
    public float learningRate;
    bool train;

    void Start()
    {
        counter = 0;
        //creates cube

        nn1 = new NeuralNetwork();
        display.GetComponent<NeuralNetworkDisplay>().ChangeNN(nn1);
        Vector2 canvasSizer = canvas.GetComponent<RectTransform>().sizeDelta;
        oldResWidth = canvasSizer[0];
        oldResHeighth = canvasSizer[1];
        PlayerPrefs.DeleteAll();
        Debug.Log("PlayerPrefs Cleared");
    }

    void Update()
    {
        Vector2 canvasSize = canvas.GetComponent<RectTransform>().sizeDelta;
        if (loading)
        {
            nnDisplay = GetComponent<NeuralNetworkSave>().LoadNeuralNetwork(index, loadSpeed);

            if (nnDisplay != null)//done
            {
                loading = false;
                display.GetComponent<NeuralNetworkDisplay>().ChangeNN(nnDisplay);
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
        else if (building)
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
        else if (training)
        {
            Debug.Log("Training " + activeID);
            switch (activeID)
            {
                case 2220:
                    TrainNN0(nn0);
                    this.GetComponent<NeuralNetworkSave>().ToStringNN(nn0, activeID);
                    break;
                case 2221:
                    TrainNN1(nnDisplay);
                    break;
                case 22220:
                    TrainNN2_0(nnDisplay);
                    break;
                case 22221:
                    TrainNN2_1(nnDisplay);
                    break;
                case 2223:
                    TrainNN3(nnDisplay);
                    break;
                case 22240:
                    TrainNN4_0(nnDisplay);
                    break;
                case 22241:
                    TrainNN4_1(nnDisplay);
                    break;
                case 22242:
                    TrainNN4_2(nnDisplay);
                    break;
                case 2225:
                    TrainNN5(nnDisplay);
                    break;
                case 22260:
                    TrainNN6_0(nnDisplay);
                    break;
                case 22261:
                    TrainNN6_1(nnDisplay);
                    break;
                case 22262:
                    TrainNN6_2(nnDisplay);
                    break;
                case 22263:
                    TrainNN6_3(nnDisplay);
                    break;
                case 22264:
                    TrainNN6_4(nnDisplay);
                    break;
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
    }

    public void TrainCurrent()
    {
        if (training)
        {
            training = false;
            //toggle train button color
        }
        else
        {
            if (activeID != 0 && activeID != 222)
            {
                training = true;
            }
        }
    }
    public void RandomizeConnections()
    {
        //activeId randomize connections
    }
    public void SetConnectionsConstant()
    {
        //activeId set to all 0
    }
    public void Merge()
    {
        //merge into one nn 
    }
    //
    //update loadNN




    public float[] NN0Out(int num)
    {
        float[] output = new float[] { 0.4f, 0.4f, 0.4f, 0.4f, 0.4f, 0.4f };

        if (num == 11 || num == 12)
        {
            //Debug.Log("nn0-0");
            output[0] = 1;
        }
        if (num == 0 || num == 3 || num == 4 || num == 5)
        {
            //Debug.Log("nn0-1");
            output[1] = 1;
        }
        if (num == 1 || num == 17)
        {
            //Debug.Log("nn0-2");
            output[2] = 1;
        }
        if (num == 15 || num == 18 || num == 19 || num == 20)
        {
            // Debug.Log("nn0-3");
            output[3] = 1;
        }
        if (num == 10 || num == 13 || num == 14)
        {
            //Debug.Log("nn0-4");
            output[4] = 1;
        }
        if (num == 2 || num == 7 || num == 8 || num == 9 || num == 16)
        {
            //Debug.Log("nn0-5");
            output[5] = 1;
        }
        return output;
    }
    public void TrainNN0(NeuralNetwork network)
    {
        int rightChoices = 0;
        float[] inputs = new float[154];
        float[] outputs = new float[] { 0.4f, 0.4f, 0.4f, 0.4f, 0.4f, 0.4f };
        float biggest4diff = 0, biggest1diff = 0;
        int x;
        for (x = 0; x < 154; x++)
        {
            inputs[x] = 0;
        }

        for (x = 0; x < 21; x++)
        {
            //if(x != 6)
            {
                inputs[x + 42] = 1;// set the right one to one
                network.RunInputs(inputs);
                network.BackPropegate(outputs);

                if (outputs[0] == 0.4f)
                {
                    float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 0).GetValue() - 0.4f;
                    if (tempDiff > biggest4diff)
                        biggest4diff = tempDiff;
                }
                if (outputs[1] == 0.4f)
                {
                    float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 1).GetValue() - 0.4f;
                    if (tempDiff > biggest4diff)
                        biggest4diff = tempDiff;
                }
                if (outputs[2] == 0.4f)
                {
                    float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 2).GetValue() - 0.4f;
                    if (tempDiff > biggest4diff)
                        biggest4diff = tempDiff;
                }
                if (outputs[3] == 0.4f)
                {
                    float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 3).GetValue() - 0.4f;
                    if (tempDiff > biggest4diff)
                        biggest4diff = tempDiff;
                }
                if (outputs[4] == 0.4f)
                {
                    float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 4).GetValue() - 0.4f;
                    if (tempDiff > biggest4diff)
                        biggest4diff = tempDiff;
                }
                if (outputs[5] == 0.4f)
                {
                    float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 5).GetValue() - 0.4f;
                    if (tempDiff > biggest4diff)
                        biggest4diff = tempDiff;
                }
                inputs[147] = 1; // set the cp on
                outputs = NN0Out(x);//all with the checkpoint
                network.RunInputs(inputs);
                network.BackPropegate(outputs);

                if (outputs[0] == 0.4f)
                {
                    float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 0).GetValue() - 0.4f;
                    if (tempDiff > biggest4diff)
                        biggest4diff = tempDiff;
                }
                if (outputs[1] == 0.4f)
                {
                    float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 1).GetValue() - 0.4f;
                    if (tempDiff > biggest4diff)
                        biggest4diff = tempDiff;
                }
                if (outputs[2] == 0.4f)
                {
                    float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 2).GetValue() - 0.4f;
                    if (tempDiff > biggest4diff)
                        biggest4diff = tempDiff;
                }
                if (outputs[3] == 0.4f)
                {
                    float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 3).GetValue() - 0.4f;
                    if (tempDiff > biggest4diff)
                        biggest4diff = tempDiff;
                }
                if (outputs[4] == 0.4f)
                {
                    float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 4).GetValue() - 0.4f;
                    if (tempDiff > biggest4diff)
                        biggest4diff = tempDiff;
                }
                if (outputs[5] == 0.4f)
                {
                    float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 5).GetValue() - 0.4f;
                    if (tempDiff > biggest4diff)
                        biggest4diff = tempDiff;
                }

                if (outputs[0] == 1)
                {
                    float tempDiff = 1 - network.GetNeuron(network.GetSize().Length - 1, 0).GetValue();
                    if (tempDiff > biggest1diff)
                        biggest1diff = tempDiff;
                }
                if (outputs[1] == 1)
                {
                    float tempDiff = 1 - network.GetNeuron(network.GetSize().Length - 1, 1).GetValue();
                    if (tempDiff > biggest1diff)
                        biggest1diff = tempDiff;
                }
                if (outputs[2] == 1)
                {
                    float tempDiff = 1 - network.GetNeuron(network.GetSize().Length - 1, 2).GetValue();
                    if (tempDiff > biggest1diff)
                        biggest1diff = tempDiff;
                }
                if (outputs[3] == 1)
                {
                    float tempDiff = 1 - network.GetNeuron(network.GetSize().Length - 1, 3).GetValue();
                    if (tempDiff > biggest1diff)
                        biggest1diff = tempDiff;
                }
                if (outputs[4] == 1)
                {
                    float tempDiff = 1 - network.GetNeuron(network.GetSize().Length - 1, 4).GetValue();
                    if (tempDiff > biggest1diff)
                        biggest1diff = tempDiff;
                }
                if (outputs[5] == 1)
                {
                    float tempDiff = 1 - network.GetNeuron(network.GetSize().Length - 1, 5).GetValue();
                    if (tempDiff > biggest1diff)
                        biggest1diff = tempDiff;
                }

                //correctly chosen
                if (outputs[0] == 1 && network.GetOutput() == 0 ||
                    outputs[1] == 1 && network.GetOutput() == 1 ||
                    outputs[2] == 1 && network.GetOutput() == 2 ||
                    outputs[3] == 1 && network.GetOutput() == 3 ||
                    outputs[4] == 1 && network.GetOutput() == 4 ||
                    outputs[5] == 1 && network.GetOutput() == 5)
                    rightChoices++;
                else
                {
                    if (outputs[0] == 1)
                        Debug.Log("CorrectChoice-0|| actual choice-" + network.GetOutput());
                    if (outputs[1] == 1)
                        Debug.Log("CorrectChoice-1|| actual choice-" + network.GetOutput());
                    if (outputs[2] == 1)
                        Debug.Log("CorrectChoice-2|| actual choice-" + network.GetOutput());
                    if (outputs[3] == 1)
                        Debug.Log("CorrectChoice-3|| actual choice-" + network.GetOutput());
                    if (outputs[4] == 1)
                        Debug.Log("CorrectChoice-4|| actual choice-" + network.GetOutput());
                    if (outputs[5] == 1)
                        Debug.Log("CorrectChoice-5|| actual choice-" + network.GetOutput());
                }

                inputs[147] = 0; //reset
                outputs = new float[] { 0.4f, 0.4f, 0.4f, 0.4f, 0.4f, 0.4f };
                inputs[x + 42] = 0;
            }
        }
        Debug.Log("1.0:" + (1 - biggest1diff) + "|||0.4:" + (0.4 + biggest4diff));
        Debug.Log("out of 20 n0 correctly chose : " + rightChoices);
        network.AddDesiredChanges(learningRate);
    }
    public void MakeGoodNN0()
    {
        nn0 = new NeuralNetwork();
        nn0.AddCollumn(0);
        nn0.AddCollumn(1);
        nn0.AddCollumn(2);
        int x;
        for (x = 0; x < 155; x++)//154 + bias node
        {
            nn0.AddNeuron(0, 0);
        }
        for (x = 0; x < 13; x++)
        {
            nn0.AddNeuron(1, 0);
        }
        for (x = 0; x < 6; x++)
        {
            nn0.AddNeuron(2, 0);
        }
        nn0.DeleteNeuron(1, 12);
        nn0.ConnectNeuronToCollumn(0, 154, 1);//biases
        nn0.ConnectNeuronToCollumn(0, 147, 1);//biases
        for (x = 42; x < 63; x++)
        {
            if (x != 2 * 21 + 6)
                nn0.ConnectNeuronToCollumn(0, x, 1);
        }
        nn0.AddNeuron(1, 12);
        nn0.ConnectCollumnToCollumn(1, 2);
    }

    public float[] NN1Out(int num)
    {
        float[] output = new float[] { 0.4f, 0.4f, 0.4f, 0.4f, 0.4f, 0.4f };

        if (num == 13 || num == 18 || num == 19 || num == 20)
        {
            output[0] = 1;
        }
        if (num == 1 || num == 2 || num == 5 || num == 10)
        {
            output[1] = 1;
        }
        if (num == 9 || num == 15 || num == 16 || num == 17)
        {
            output[3] = 1;
        }
        if (num == 3 || num == 4 || num == 11 || num == 12 || num == 14)
        {
            output[4] = 1;
        }
        return output;
    }
    public void TrainNN1(NeuralNetwork network)
    {
        float[] inputs = new float[154];
        float[] outputs = new float[] { 0.4f, 0.4f, 0.4f, 0.4f, 0.4f, 0.4f };
        int rightChoices = 0;
        float biggest4diff = 0, biggest1diff = 0;
        int x;
        for (x = 0; x < 154; x++)
        {
            inputs[x] = 0;
        }

        for (x = 0; x < 21; x++)
        {
            if (x != 0 && x != 6 && x != 7) // && x != 8)  need one for the zeros
            {
                inputs[x] = 1;// set the right one to one
                network.RunInputs(inputs);
                network.BackPropegate(outputs);
                inputs[148] = 1; // set the cp on
                outputs = NN1Out(x);//all with the checkpoint

                if (outputs[0] == 0.4f)
                {
                    float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 0).GetValue() - 0.4f;
                    if (tempDiff > biggest4diff)
                        biggest4diff = tempDiff;
                }
                if (outputs[1] == 0.4f)
                {
                    float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 1).GetValue() - 0.4f;
                    if (tempDiff > biggest4diff)
                        biggest4diff = tempDiff;
                }
                if (outputs[2] == 0.4f)
                {
                    float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 2).GetValue() - 0.4f;
                    if (tempDiff > biggest4diff)
                        biggest4diff = tempDiff;
                }
                if (outputs[3] == 0.4f)
                {
                    float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 3).GetValue() - 0.4f;
                    if (tempDiff > biggest4diff)
                        biggest4diff = tempDiff;
                }
                if (outputs[4] == 0.4f)
                {
                    float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 4).GetValue() - 0.4f;
                    if (tempDiff > biggest4diff)
                        biggest4diff = tempDiff;
                }
                if (outputs[5] == 0.4f)
                {
                    float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 5).GetValue() - 0.4f;
                    if (tempDiff > biggest4diff)
                        biggest4diff = tempDiff;
                }

                network.RunInputs(inputs);
                network.BackPropegate(outputs);

                if (outputs[0] == 0.4f)
                {
                    float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 0).GetValue() - 0.4f;
                    if (tempDiff > biggest4diff)
                        biggest4diff = tempDiff;
                }
                if (outputs[1] == 0.4f)
                {
                    float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 1).GetValue() - 0.4f;
                    if (tempDiff > biggest4diff)
                        biggest4diff = tempDiff;
                }
                if (outputs[2] == 0.4f)
                {
                    float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 2).GetValue() - 0.4f;
                    if (tempDiff > biggest4diff)
                        biggest4diff = tempDiff;
                }
                if (outputs[3] == 0.4f)
                {
                    float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 3).GetValue() - 0.4f;
                    if (tempDiff > biggest4diff)
                        biggest4diff = tempDiff;
                }
                if (outputs[4] == 0.4f)
                {
                    float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 4).GetValue() - 0.4f;
                    if (tempDiff > biggest4diff)
                        biggest4diff = tempDiff;
                }
                if (outputs[5] == 0.4f)
                {
                    float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 5).GetValue() - 0.4f;
                    if (tempDiff > biggest4diff)
                        biggest4diff = tempDiff;
                }

                if (outputs[0] == 1)
                {
                    float tempDiff = 1 - network.GetNeuron(network.GetSize().Length - 1, 0).GetValue();
                    if (tempDiff > biggest1diff)
                        biggest1diff = tempDiff;
                }
                if (outputs[1] == 1)
                {
                    float tempDiff = 1 - network.GetNeuron(network.GetSize().Length - 1, 1).GetValue();
                    if (tempDiff > biggest1diff)
                        biggest1diff = tempDiff;
                }
                if (outputs[2] == 1)
                {
                    float tempDiff = 1 - network.GetNeuron(network.GetSize().Length - 1, 2).GetValue();
                    if (tempDiff > biggest1diff)
                        biggest1diff = tempDiff;
                }
                if (outputs[3] == 1)
                {
                    float tempDiff = 1 - network.GetNeuron(network.GetSize().Length - 1, 3).GetValue();
                    if (tempDiff > biggest1diff)
                        biggest1diff = tempDiff;
                }
                if (outputs[4] == 1)
                {
                    float tempDiff = 1 - network.GetNeuron(network.GetSize().Length - 1, 4).GetValue();
                    if (tempDiff > biggest1diff)
                        biggest1diff = tempDiff;
                }
                if (outputs[5] == 1)
                {
                    float tempDiff = 1 - network.GetNeuron(network.GetSize().Length - 1, 5).GetValue();
                    if (tempDiff > biggest1diff)
                        biggest1diff = tempDiff;
                }

                if (outputs[0] == 1 && network.GetOutput() == 0 ||
                    outputs[1] == 1 && network.GetOutput() == 1 ||
                    outputs[2] == 1 && network.GetOutput() == 2 ||
                    outputs[3] == 1 && network.GetOutput() == 3 ||
                    outputs[4] == 1 && network.GetOutput() == 4 ||
                    outputs[5] == 1 && network.GetOutput() == 5)
                    rightChoices++;
                else
                {
                    if (outputs[0] == 1)
                        Debug.Log("CorrectChoice-0|| actual choice-" + network.GetOutput());
                    else if (outputs[1] == 1)
                        Debug.Log("CorrectChoice-1|| actual choice-" + network.GetOutput());
                    else if (outputs[2] == 1)
                        Debug.Log("CorrectChoice-2|| actual choice-" + network.GetOutput());
                    else if (outputs[3] == 1)
                        Debug.Log("CorrectChoice-3|| actual choice-" + network.GetOutput());
                    else if (outputs[4] == 1)
                        Debug.Log("CorrectChoice-4|| actual choice-" + network.GetOutput());
                    else if (outputs[5] == 1)
                        Debug.Log("CorrectChoice-5|| actual choice-" + network.GetOutput());
                }


                //Debug.Log("pp" + x + "p" + network.getOutput());
                inputs[148] = 0; //reset
                outputs = NN1Out(6);
                inputs[x] = 0;
            }
        }
        Debug.Log("1.0:" + (1 - biggest1diff) + "|||0.4:" + (0.4 + biggest4diff));
        Debug.Log("out of 17 n1 correctly chose : " + rightChoices);
        network.AddDesiredChanges(0.15f);
    }
    public void MakeGoodNN1()
    {
        nn1 = new NeuralNetwork();
        nn1.AddCollumn(0);
        nn1.AddCollumn(1);
        nn1.AddCollumn(2);
        int x;
        for (x = 0; x < 155; x++)//154 + bias node
        {
            nn1.AddNeuron(0, 0);
        }
        for (x = 0; x < 9; x++)
        {
            nn1.AddNeuron(1, 0);
        }
        for (x = 0; x < 6; x++)
        {
            nn1.AddNeuron(2, 0);
        }
        nn1.DeleteNeuron(1, 8);
        nn1.ConnectNeuronToCollumn(0, 154, 1);//biases
        nn1.ConnectNeuronToCollumn(0, 148, 1);//biases
        for (x = 0; x < 21; x++)
        {
            if (x != 0 && x != 6 && x != 7 && x != 8)
                nn1.ConnectNeuronToCollumn(0, x, 1);
        }
        nn1.AddNeuron(1, 8);
        nn1.ConnectCollumnToCollumn(1, 2);
        /*
        nn1.deleteNeuron(1, 8);
        nn1.connectNeuronToCollumn(0, 154, 1);//biases
        nn1.connectNeuronToCollumn(0, 147, 1);//cp
        nn1.addNeuron(1, 8);
        nn1.connectNeuronToCollumn(1, 8, 2);


        //b
        nn1.addConnection(0, 13, 1, 0, Random.Range(-1f, 1f));
        nn1.addConnection(0, 18, 1, 0, Random.Range(-1f, 1f));
        nn1.addConnection(0, 19, 1, 0, Random.Range(-1f, 1f));
        nn1.addConnection(0, 20, 1, 0, Random.Range(-1f, 1f));
        nn1.addConnection(0, 13, 1, 1, Random.Range(-1f, 1f));
        nn1.addConnection(0, 18, 1, 1, Random.Range(-1f, 1f));
        nn1.addConnection(0, 19, 1, 1, Random.Range(-1f, 1f));
        nn1.addConnection(0, 20, 1, 1, Random.Range(-1f, 1f));
        //r
        nn1.addConnection(0, 0, 1, 2, Random.Range(-1f, 1f));
        nn1.addConnection(0, 2, 1, 2, Random.Range(-1f, 1f));
        nn1.addConnection(0, 0, 1, 3, Random.Range(-1f, 1f));
        nn1.addConnection(0, 2, 1, 3, Random.Range(-1f, 1f));
        //b'
        nn1.addConnection(0, 9, 1, 4, Random.Range(-1f, 1f));
        nn1.addConnection(0, 15, 1, 4, Random.Range(-1f, 1f));
        nn1.addConnection(0, 16, 1, 4, Random.Range(-1f, 1f));
        nn1.addConnection(0, 17, 1, 4, Random.Range(-1f, 1f));
        nn1.addConnection(0, 9, 1, 5, Random.Range(-1f, 1f));
        nn1.addConnection(0, 15, 1, 5, Random.Range(-1f, 1f));
        nn1.addConnection(0, 16, 1, 5, Random.Range(-1f, 1f));
        nn1.addConnection(0, 17, 1, 5, Random.Range(-1f, 1f));
        //r'
        nn1.addConnection(0, 3, 1, 6, Random.Range(-1f, 1f));
        nn1.addConnection(0, 4, 1, 6, Random.Range(-1f, 1f));
        nn1.addConnection(0, 10, 1, 6, Random.Range(-1f, 1f));
        nn1.addConnection(0, 11, 1, 6, Random.Range(-1f, 1f));
        nn1.addConnection(0, 12, 1, 6, Random.Range(-1f, 1f));
        nn1.addConnection(0, 14, 1, 6, Random.Range(-1f, 1f));
        nn1.addConnection(0, 0, 1, 6, Random.Range(-1f, 1f));
        nn1.addConnection(0, 3, 1, 7, Random.Range(-1f, 1f));
        nn1.addConnection(0, 4, 1, 7, Random.Range(-1f, 1f));
        nn1.addConnection(0, 10, 1, 7, Random.Range(-1f, 1f));
        nn1.addConnection(0, 11, 1, 7, Random.Range(-1f, 1f));
        nn1.addConnection(0, 12, 1, 7, Random.Range(-1f, 1f));
        nn1.addConnection(0, 14, 1, 7, Random.Range(-1f, 1f));
        nn1.addConnection(0, 0, 1, 7, Random.Range(-1f, 1f));



        //finaks
        nn1.addConnection(1, 0, 2, 0, Random.Range(-1f, 1f));
        nn1.addConnection(1, 2, 2, 1, Random.Range(-1f, 1f));
        nn1.addConnection(1, 4, 2, 3, Random.Range(-1f, 1f));
        nn1.addConnection(1, 6, 2, 4, Random.Range(-1f, 1f));
        nn1.addConnection(1, 1, 2, 0, Random.Range(-1f, 1f));
        nn1.addConnection(1, 3, 2, 1, Random.Range(-1f, 1f));
        nn1.addConnection(1, 5, 2, 3, Random.Range(-1f, 1f));
        nn1.addConnection(1, 7, 2, 4, Random.Range(-1f, 1f));
        */
    }

    public float[] NN2_0Out(int num1)
    {
        float[] output = new float[] { 0.4f, 0.4f, 0.4f, 0.4f, 0.4f, 0.4f };

        //b
        if (num1 == 10 || num1 == 15 || num1 == 16 || num1 == 17 || num1 == 18 || num1 == 20)
        {
            output[0] = 1;
        }
        //b'
        if (num1 == 9 || num1 == 11 || num1 == 14)
        {
            output[3] = 1;
        }
        //r'
        if (num1 == 4 || num1 == 5 || num1 == 12 || num1 == 13 || num1 == 19)
        {
            output[4] = 1;
        }
        return output;
    }
    public void TrainNN2_0(NeuralNetwork network)
    {
        float[] inputs = new float[154];
        float[] outputs = new float[6];
        int x;
        int rightChoices = 0;
        float biggest4diff = 0, biggest1diff = 0;
        for (x = 0; x < 154; x++)
        {
            inputs[x] = 0;
        }

        for (x = 0; x < 21; x++)
        {
            if (x != 0 && x != 1 && x != 2 && x != 3 && x != 6 && x != 7) // && x != 8)  need one for the zeros
            {
                //cp 
                //helper 
                //pos

                //cp off helper off
                inputs[149] = 0; // turn the cp off
                inputs[0] = 0;

                //grw down train
                inputs[x + 21] = 1;// set the right one to one
                outputs = new float[] { 0.4f, 0.4f, 0.4f, 0.4f, 0.4f, 0.4f };
                network.RunInputs(inputs);
                network.BackPropegate(outputs);

                if (outputs[0] == 0.4f)
                {
                    float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 0).GetValue() - 0.4f;
                    if (tempDiff > biggest4diff)
                        biggest4diff = tempDiff;
                }
                if (outputs[1] == 0.4f)
                {
                    float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 1).GetValue() - 0.4f;
                    if (tempDiff > biggest4diff)
                        biggest4diff = tempDiff;
                }
                if (outputs[2] == 0.4f)
                {
                    float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 2).GetValue() - 0.4f;
                    if (tempDiff > biggest4diff)
                        biggest4diff = tempDiff;
                }
                if (outputs[3] == 0.4f)
                {
                    float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 3).GetValue() - 0.4f;
                    if (tempDiff > biggest4diff)
                        biggest4diff = tempDiff;
                }
                if (outputs[4] == 0.4f)
                {
                    float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 4).GetValue() - 0.4f;
                    if (tempDiff > biggest4diff)
                        biggest4diff = tempDiff;
                }
                if (outputs[5] == 0.4f)
                {
                    float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 5).GetValue() - 0.4f;
                    if (tempDiff > biggest4diff)
                        biggest4diff = tempDiff;
                }

                inputs[149] = 1; // turn the cp on

                network.RunInputs(inputs);
                network.BackPropegate(outputs);

                if (outputs[0] == 0.4f)
                {
                    float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 0).GetValue() - 0.4f;
                    if (tempDiff > biggest4diff)
                        biggest4diff = tempDiff;
                }
                if (outputs[1] == 0.4f)
                {
                    float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 1).GetValue() - 0.4f;
                    if (tempDiff > biggest4diff)
                        biggest4diff = tempDiff;
                }
                if (outputs[2] == 0.4f)
                {
                    float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 2).GetValue() - 0.4f;
                    if (tempDiff > biggest4diff)
                        biggest4diff = tempDiff;
                }
                if (outputs[3] == 0.4f)
                {
                    float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 3).GetValue() - 0.4f;
                    if (tempDiff > biggest4diff)
                        biggest4diff = tempDiff;
                }
                if (outputs[4] == 0.4f)
                {
                    float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 4).GetValue() - 0.4f;
                    if (tempDiff > biggest4diff)
                        biggest4diff = tempDiff;
                }
                if (outputs[5] == 0.4f)
                {
                    float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 5).GetValue() - 0.4f;
                    if (tempDiff > biggest4diff)
                        biggest4diff = tempDiff;
                }

                inputs[149] = 0; // turn the cp off
                inputs[0] = 1;

                network.RunInputs(inputs);
                network.BackPropegate(outputs);

                if (outputs[0] == 0.4f)
                {
                    float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 0).GetValue() - 0.4f;
                    if (tempDiff > biggest4diff)
                        biggest4diff = tempDiff;
                }
                if (outputs[1] == 0.4f)
                {
                    float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 1).GetValue() - 0.4f;
                    if (tempDiff > biggest4diff)
                        biggest4diff = tempDiff;
                }
                if (outputs[2] == 0.4f)
                {
                    float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 2).GetValue() - 0.4f;
                    if (tempDiff > biggest4diff)
                        biggest4diff = tempDiff;
                }
                if (outputs[3] == 0.4f)
                {
                    float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 3).GetValue() - 0.4f;
                    if (tempDiff > biggest4diff)
                        biggest4diff = tempDiff;
                }
                if (outputs[4] == 0.4f)
                {
                    float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 4).GetValue() - 0.4f;
                    if (tempDiff > biggest4diff)
                        biggest4diff = tempDiff;
                }
                if (outputs[5] == 0.4f)
                {
                    float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 5).GetValue() - 0.4f;
                    if (tempDiff > biggest4diff)
                        biggest4diff = tempDiff;
                }

                inputs[149] = 1;

                outputs = NN2_0Out(x);
                network.RunInputs(inputs);
                network.BackPropegate(outputs);
                if (outputs[0] == 0.4f)
                {
                    float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 0).GetValue() - 0.4f;
                    if (tempDiff > biggest4diff)
                        biggest4diff = tempDiff;
                }
                if (outputs[1] == 0.4f)
                {
                    float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 1).GetValue() - 0.4f;
                    if (tempDiff > biggest4diff)
                        biggest4diff = tempDiff;
                }
                if (outputs[2] == 0.4f)
                {
                    float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 2).GetValue() - 0.4f;
                    if (tempDiff > biggest4diff)
                        biggest4diff = tempDiff;
                }
                if (outputs[3] == 0.4f)
                {
                    float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 3).GetValue() - 0.4f;
                    if (tempDiff > biggest4diff)
                        biggest4diff = tempDiff;
                }
                if (outputs[4] == 0.4f)
                {
                    float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 4).GetValue() - 0.4f;
                    if (tempDiff > biggest4diff)
                        biggest4diff = tempDiff;
                }
                if (outputs[5] == 0.4f)
                {
                    float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 5).GetValue() - 0.4f;
                    if (tempDiff > biggest4diff)
                        biggest4diff = tempDiff;
                }

                if (outputs[0] == 1)
                {
                    float tempDiff = 1 - network.GetNeuron(network.GetSize().Length - 1, 0).GetValue();
                    if (tempDiff > biggest1diff)
                        biggest1diff = tempDiff;
                }
                if (outputs[1] == 1)
                {
                    float tempDiff = 1 - network.GetNeuron(network.GetSize().Length - 1, 1).GetValue();
                    if (tempDiff > biggest1diff)
                        biggest1diff = tempDiff;
                }
                if (outputs[2] == 1)
                {
                    float tempDiff = 1 - network.GetNeuron(network.GetSize().Length - 1, 2).GetValue();
                    if (tempDiff > biggest1diff)
                        biggest1diff = tempDiff;
                }
                if (outputs[3] == 1)
                {
                    float tempDiff = 1 - network.GetNeuron(network.GetSize().Length - 1, 3).GetValue();
                    if (tempDiff > biggest1diff)
                        biggest1diff = tempDiff;
                }
                if (outputs[4] == 1)
                {
                    float tempDiff = 1 - network.GetNeuron(network.GetSize().Length - 1, 4).GetValue();
                    if (tempDiff > biggest1diff)
                        biggest1diff = tempDiff;
                }
                if (outputs[5] == 1)
                {
                    float tempDiff = 1 - network.GetNeuron(network.GetSize().Length - 1, 5).GetValue();
                    if (tempDiff > biggest1diff)
                        biggest1diff = tempDiff;
                }


                if (outputs[0] == 1 && network.GetOutput() == 0 ||
                    outputs[1] == 1 && network.GetOutput() == 1 ||
                    outputs[2] == 1 && network.GetOutput() == 2 ||
                    outputs[3] == 1 && network.GetOutput() == 3 ||
                    outputs[4] == 1 && network.GetOutput() == 4 ||
                    outputs[5] == 1 && network.GetOutput() == 5)
                    rightChoices++;
                else
                {
                    if (outputs[0] == 1)
                        Debug.Log("CorrectChoiceDown-0|| actual choice-" + network.GetOutput());
                    else if (outputs[1] == 1)
                        Debug.Log("CorrectChoiceDown-1|| actual choice-" + network.GetOutput());
                    else if (outputs[2] == 1)
                        Debug.Log("CorrectChoiceDown-2|| actual choice-" + network.GetOutput());
                    else if (outputs[3] == 1)
                        Debug.Log("CorrectChoiceDown-3|| actual choice-" + network.GetOutput());
                    else if (outputs[4] == 1)
                        Debug.Log("CorrectChoiceDown-4|| actual choice-" + network.GetOutput());
                    else if (outputs[5] == 1)
                        Debug.Log("CorrectChoiceDown-5|| actual choice-" + network.GetOutput());
                }
                inputs[x + 21] = 0;
            }
        }
        Debug.Log("1.0:" + (1 - biggest1diff) + "|||0.4:" + (0.4 + biggest4diff));
        Debug.Log("out of 14 n2_0 correctly chose : " + rightChoices + "-----------------------------------");
        network.AddDesiredChanges(0.15f);
    }
    public void MakeGoodNN2_0()
    {
        nn2_0 = new NeuralNetwork();
        nn2_0.AddCollumn(0);
        nn2_0.AddCollumn(1);
        nn2_0.AddCollumn(2);
        int x;
        for (x = 0; x < 155; x++)//154 + bias node
        {
            nn2_0.AddNeuron(0, 0);
        }
        for (x = 0; x < 13; x++)
        {
            nn2_0.AddNeuron(1, 0);
        }
        for (x = 0; x < 6; x++)
        {
            nn2_0.AddNeuron(2, 0);
        }
        nn2_0.DeleteNeuron(1, 12);
        for (x = 0; x < 21; x++)
        {
            if (x != 0 && x != 1 && x != 2 && x != 3 && x != 6 && x != 7 && x != 8)
                nn2_0.ConnectNeuronToCollumn(0, x + 21, 1);
        }
        nn2_0.AddCollumn(1);
        nn2_0.AddNeuron(1, 0);
        nn2_0.AddNeuron(1, 1);

        nn2_0.AddCollumn(2);
        nn2_0.AddNeuron(2, 0);

        nn2_0.ConnectNeuronToCollumn(0, 0, 1);
        nn2_0.ConnectNeuronToCollumn(0, 149, 1);
        nn2_0.ConnectNeuronToCollumn(0, 154, 1);
        nn2_0.ConnectNeuronToCollumn(2, 0, 1);
        nn2_0.ConnectNeuronToCollumn(2, 0, 3);
        nn2_0.AddNeuron(1, 2);
        nn2_0.AddConnection(1, 2, 2, 0, 1);
        nn2_0.AddNeuron(2, 1);
        nn2_0.ConnectNeuronToCollumn(2, 1, 3);
        nn2_0.DeleteConnection(1, 1, 2, 1);


        nn2_0.ConnectNeuronToCollumn(1, 1, 2);//the one helper

        nn2_0.AddNeuron(3, 12);
        nn2_0.ConnectCollumnToCollumn(3, 4);

    }

    public float[] NN2_1Out(int num1)
    {
        float[] output = new float[] { 0.4f, 0.4f, 0.4f, 0.4f, 0.4f, 0.4f };

        //b
        if (num1 == 9 || num1 == 11 || num1 == 12 || num1 == 13 || num1 == 19)
        {
            output[0] = 1;
        }
        //r
        if (num1 == 14 || num1 == 15 || num1 == 16 || num1 == 17)
        {
            output[1] = 1;
        }
        //b'
        if (num1 == 10)
        {
            output[3] = 1;
        }
        return output;
    }
    public void TrainNN2_1(NeuralNetwork network)
    {
        float[] inputs = new float[154];
        float[] outputs = new float[6];
        int x;
        int rightChoices = 0;
        float biggest4diff = 0, biggest1diff = 0;
        for (x = 0; x < 154; x++)
        {
            inputs[x] = 0;
        }

        for (x = 0; x < 21; x++)
        {
            if (x != 0 && x != 1 && x != 2 && x != 3 && x != 6 && x != 7 && x != 8 && x != 4 && x != 5 && x != 18) // && x != 20) need one for the zeros
            {
                //cp 
                //helper 
                //pos

                //cp off helper off
                inputs[149] = 0; // turn the cp off
                inputs[5] = 0;

                //grw down train
                inputs[x + 21] = 1;// set the right one to one
                outputs = new float[] { 0.4f, 0.4f, 0.4f, 0.4f, 0.4f, 0.4f };
                network.RunInputs(inputs);
                network.BackPropegate(outputs);

                if (outputs[0] == 0.4f)
                {
                    float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 0).GetValue() - 0.4f;
                    if (tempDiff > biggest4diff)
                        biggest4diff = tempDiff;
                }
                if (outputs[1] == 0.4f)
                {
                    float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 1).GetValue() - 0.4f;
                    if (tempDiff > biggest4diff)
                        biggest4diff = tempDiff;
                }
                if (outputs[2] == 0.4f)
                {
                    float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 2).GetValue() - 0.4f;
                    if (tempDiff > biggest4diff)
                        biggest4diff = tempDiff;
                }
                if (outputs[3] == 0.4f)
                {
                    float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 3).GetValue() - 0.4f;
                    if (tempDiff > biggest4diff)
                        biggest4diff = tempDiff;
                }
                if (outputs[4] == 0.4f)
                {
                    float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 4).GetValue() - 0.4f;
                    if (tempDiff > biggest4diff)
                        biggest4diff = tempDiff;
                }
                if (outputs[5] == 0.4f)
                {
                    float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 5).GetValue() - 0.4f;
                    if (tempDiff > biggest4diff)
                        biggest4diff = tempDiff;
                }

                inputs[149] = 1; // turn the cp on

                network.RunInputs(inputs);
                network.BackPropegate(outputs);

                if (outputs[0] == 0.4f)
                {
                    float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 0).GetValue() - 0.4f;
                    if (tempDiff > biggest4diff)
                        biggest4diff = tempDiff;
                }
                if (outputs[1] == 0.4f)
                {
                    float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 1).GetValue() - 0.4f;
                    if (tempDiff > biggest4diff)
                        biggest4diff = tempDiff;
                }
                if (outputs[2] == 0.4f)
                {
                    float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 2).GetValue() - 0.4f;
                    if (tempDiff > biggest4diff)
                        biggest4diff = tempDiff;
                }
                if (outputs[3] == 0.4f)
                {
                    float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 3).GetValue() - 0.4f;
                    if (tempDiff > biggest4diff)
                        biggest4diff = tempDiff;
                }
                if (outputs[4] == 0.4f)
                {
                    float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 4).GetValue() - 0.4f;
                    if (tempDiff > biggest4diff)
                        biggest4diff = tempDiff;
                }
                if (outputs[5] == 0.4f)
                {
                    float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 5).GetValue() - 0.4f;
                    if (tempDiff > biggest4diff)
                        biggest4diff = tempDiff;
                }

                inputs[149] = 0; // turn the cp off
                inputs[5] = 1;

                network.RunInputs(inputs);
                network.BackPropegate(outputs);

                if (outputs[0] == 0.4f)
                {
                    float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 0).GetValue() - 0.4f;
                    if (tempDiff > biggest4diff)
                        biggest4diff = tempDiff;
                }
                if (outputs[1] == 0.4f)
                {
                    float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 1).GetValue() - 0.4f;
                    if (tempDiff > biggest4diff)
                        biggest4diff = tempDiff;
                }
                if (outputs[2] == 0.4f)
                {
                    float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 2).GetValue() - 0.4f;
                    if (tempDiff > biggest4diff)
                        biggest4diff = tempDiff;
                }
                if (outputs[3] == 0.4f)
                {
                    float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 3).GetValue() - 0.4f;
                    if (tempDiff > biggest4diff)
                        biggest4diff = tempDiff;
                }
                if (outputs[4] == 0.4f)
                {
                    float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 4).GetValue() - 0.4f;
                    if (tempDiff > biggest4diff)
                        biggest4diff = tempDiff;
                }
                if (outputs[5] == 0.4f)
                {
                    float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 5).GetValue() - 0.4f;
                    if (tempDiff > biggest4diff)
                        biggest4diff = tempDiff;
                }

                inputs[149] = 1;

                outputs = NN2_1Out(x);
                network.RunInputs(inputs);
                network.BackPropegate(outputs);

                if (outputs[0] == 0.4f)
                {
                    float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 0).GetValue() - 0.4f;
                    if (tempDiff > biggest4diff)
                        biggest4diff = tempDiff;
                }
                if (outputs[1] == 0.4f)
                {
                    float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 1).GetValue() - 0.4f;
                    if (tempDiff > biggest4diff)
                        biggest4diff = tempDiff;
                }
                if (outputs[2] == 0.4f)
                {
                    float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 2).GetValue() - 0.4f;
                    if (tempDiff > biggest4diff)
                        biggest4diff = tempDiff;
                }
                if (outputs[3] == 0.4f)
                {
                    float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 3).GetValue() - 0.4f;
                    if (tempDiff > biggest4diff)
                        biggest4diff = tempDiff;
                }
                if (outputs[4] == 0.4f)
                {
                    float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 4).GetValue() - 0.4f;
                    if (tempDiff > biggest4diff)
                        biggest4diff = tempDiff;
                }
                if (outputs[5] == 0.4f)
                {
                    float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 5).GetValue() - 0.4f;
                    if (tempDiff > biggest4diff)
                        biggest4diff = tempDiff;
                }

                if (outputs[0] == 1)
                {
                    float tempDiff = 1 - network.GetNeuron(network.GetSize().Length - 1, 0).GetValue();
                    if (tempDiff > biggest1diff)
                        biggest1diff = tempDiff;
                }
                if (outputs[1] == 1)
                {
                    float tempDiff = 1 - network.GetNeuron(network.GetSize().Length - 1, 1).GetValue();
                    if (tempDiff > biggest1diff)
                        biggest1diff = tempDiff;
                }
                if (outputs[2] == 1)
                {
                    float tempDiff = 1 - network.GetNeuron(network.GetSize().Length - 1, 2).GetValue();
                    if (tempDiff > biggest1diff)
                        biggest1diff = tempDiff;
                }
                if (outputs[3] == 1)
                {
                    float tempDiff = 1 - network.GetNeuron(network.GetSize().Length - 1, 3).GetValue();
                    if (tempDiff > biggest1diff)
                        biggest1diff = tempDiff;
                }
                if (outputs[4] == 1)
                {
                    float tempDiff = 1 - network.GetNeuron(network.GetSize().Length - 1, 4).GetValue();
                    if (tempDiff > biggest1diff)
                        biggest1diff = tempDiff;
                }
                if (outputs[5] == 1)
                {
                    float tempDiff = 1 - network.GetNeuron(network.GetSize().Length - 1, 5).GetValue();
                    if (tempDiff > biggest1diff)
                        biggest1diff = tempDiff;
                }

                if (outputs[0] == 1 && network.GetOutput() == 0 ||
                    outputs[1] == 1 && network.GetOutput() == 1 ||
                    outputs[2] == 1 && network.GetOutput() == 2 ||
                    outputs[3] == 1 && network.GetOutput() == 3 ||
                    outputs[4] == 1 && network.GetOutput() == 4 ||
                    outputs[5] == 1 && network.GetOutput() == 5)
                    rightChoices++;
                else
                {
                    if (outputs[0] == 1)
                        Debug.Log("CorrectChoiceDown-0|| actual choice-" + network.GetOutput());
                    else if (outputs[1] == 1)
                        Debug.Log("CorrectChoiceDown-1|| actual choice-" + network.GetOutput());
                    else if (outputs[2] == 1)
                        Debug.Log("CorrectChoiceDown-2|| actual choice-" + network.GetOutput());
                    else if (outputs[3] == 1)
                        Debug.Log("CorrectChoiceDown-3|| actual choice-" + network.GetOutput());
                    else if (outputs[4] == 1)
                        Debug.Log("CorrectChoiceDown-4|| actual choice-" + network.GetOutput());
                    else if (outputs[5] == 1)
                        Debug.Log("CorrectChoiceDown-5|| actual choice-" + network.GetOutput());
                }
                inputs[x + 21] = 0;
            }
        }
        Debug.Log("1.0:" + (1 - biggest1diff) + "|||0.4:" + (0.4 + biggest4diff));
        Debug.Log("out of 10 n2_1 correctly chose : " + rightChoices + "-----------------------------------");
        network.AddDesiredChanges(0.15f);
    }
    public void MakeGoodNN2_1()
    {
        nn2_1 = new NeuralNetwork();
        nn2_1.AddCollumn(0);
        nn2_1.AddCollumn(1);
        nn2_1.AddCollumn(2);
        int x;
        for (x = 0; x < 155; x++)//154 + bias node
        {
            nn2_1.AddNeuron(0, 0);
        }
        for (x = 0; x < 13; x++)
        {
            nn2_1.AddNeuron(1, 0);
        }
        for (x = 0; x < 6; x++)
        {
            nn2_1.AddNeuron(2, 0);
        }
        nn2_1.DeleteNeuron(1, 12);
        for (x = 0; x < 21; x++)
        {
            if (x != 0 && x != 1 && x != 2 && x != 3 && x != 6 && x != 7 && x != 8 && x != 4 && x != 5 && x != 18 && x != 20)
                nn2_1.ConnectNeuronToCollumn(0, x + 21, 1);
        }
        nn2_1.AddCollumn(1);
        nn2_1.AddNeuron(1, 0);
        nn2_1.AddNeuron(1, 1);

        nn2_1.AddCollumn(2);
        nn2_1.AddNeuron(2, 0);

        nn2_1.ConnectNeuronToCollumn(0, 5, 1);
        nn2_1.ConnectNeuronToCollumn(0, 149, 1);
        nn2_1.ConnectNeuronToCollumn(0, 154, 1);
        nn2_1.ConnectNeuronToCollumn(2, 0, 1);
        nn2_1.ConnectNeuronToCollumn(2, 0, 3);
        nn2_1.AddNeuron(1, 2);
        nn2_1.AddConnection(1, 2, 2, 0, 1);
        nn2_1.AddNeuron(2, 1);
        nn2_1.ConnectNeuronToCollumn(2, 1, 3);
        nn2_1.DeleteConnection(1, 1, 2, 1);


        nn2_1.ConnectNeuronToCollumn(1, 1, 2);//the one helper

        nn2_1.AddNeuron(3, 12);
        nn2_1.ConnectCollumnToCollumn(3, 4);

    }

    public void TrainNN3(NeuralNetwork network)
    {
        float[] inputs = new float[154];
        float[] outputs;
        int correctChoices = 0;
        float biggest4diff = 0, biggest1diff = 0;
        int x;
        for (x = 0; x < 154; x++)
        {
            inputs[x] = 0;
        }

        inputs[150] = 0; // set the cp off
        outputs = new float[] { 0.4f, 0.4f, 0.4f, 0.4f, 0.4f, 0.4f };
        network.RunInputs(inputs);
        network.BackPropegate(outputs);

        if (outputs[0] == 0.4f)
        {
            float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 0).GetValue() - 0.4f;
            if (tempDiff > biggest4diff)
                biggest4diff = tempDiff;
        }
        if (outputs[1] == 0.4f)
        {
            float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 1).GetValue() - 0.4f;
            if (tempDiff > biggest4diff)
                biggest4diff = tempDiff;
        }
        if (outputs[2] == 0.4f)
        {
            float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 2).GetValue() - 0.4f;
            if (tempDiff > biggest4diff)
                biggest4diff = tempDiff;
        }
        if (outputs[3] == 0.4f)
        {
            float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 3).GetValue() - 0.4f;
            if (tempDiff > biggest4diff)
                biggest4diff = tempDiff;
        }
        if (outputs[4] == 0.4f)
        {
            float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 4).GetValue() - 0.4f;
            if (tempDiff > biggest4diff)
                biggest4diff = tempDiff;
        }
        if (outputs[5] == 0.4f)
        {
            float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 5).GetValue() - 0.4f;
            if (tempDiff > biggest4diff)
                biggest4diff = tempDiff;
        }

        inputs[150] = 1; // set the cp on
        outputs = new float[] { 1f, 0.4f, 0.4f, 0.4f, 0.4f, 0.4f };
        network.RunInputs(inputs);
        network.BackPropegate(outputs);
        {
            if (outputs[0] == 0.4f)
            {
                float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 0).GetValue() - 0.4f;
                if (tempDiff > biggest4diff)
                    biggest4diff = tempDiff;
            }
            if (outputs[1] == 0.4f)
            {
                float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 1).GetValue() - 0.4f;
                if (tempDiff > biggest4diff)
                    biggest4diff = tempDiff;
            }
            if (outputs[2] == 0.4f)
            {
                float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 2).GetValue() - 0.4f;
                if (tempDiff > biggest4diff)
                    biggest4diff = tempDiff;
            }
            if (outputs[3] == 0.4f)
            {
                float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 3).GetValue() - 0.4f;
                if (tempDiff > biggest4diff)
                    biggest4diff = tempDiff;
            }
            if (outputs[4] == 0.4f)
            {
                float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 4).GetValue() - 0.4f;
                if (tempDiff > biggest4diff)
                    biggest4diff = tempDiff;
            }
            if (outputs[5] == 0.4f)
            {
                float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 5).GetValue() - 0.4f;
                if (tempDiff > biggest4diff)
                    biggest4diff = tempDiff;
            }
        }
        {
            if (outputs[0] == 1)
            {
                float tempDiff = 1 - network.GetNeuron(network.GetSize().Length - 1, 0).GetValue();
                if (tempDiff > biggest1diff)
                    biggest1diff = tempDiff;
            }
            if (outputs[1] == 1)
            {
                float tempDiff = 1 - network.GetNeuron(network.GetSize().Length - 1, 1).GetValue();
                if (tempDiff > biggest1diff)
                    biggest1diff = tempDiff;
            }
            if (outputs[2] == 1)
            {
                float tempDiff = 1 - network.GetNeuron(network.GetSize().Length - 1, 2).GetValue();
                if (tempDiff > biggest1diff)
                    biggest1diff = tempDiff;
            }
            if (outputs[3] == 1)
            {
                float tempDiff = 1 - network.GetNeuron(network.GetSize().Length - 1, 3).GetValue();
                if (tempDiff > biggest1diff)
                    biggest1diff = tempDiff;
            }
            if (outputs[4] == 1)
            {
                float tempDiff = 1 - network.GetNeuron(network.GetSize().Length - 1, 4).GetValue();
                if (tempDiff > biggest1diff)
                    biggest1diff = tempDiff;
            }
            if (outputs[5] == 1)
            {
                float tempDiff = 1 - network.GetNeuron(network.GetSize().Length - 1, 5).GetValue();
                if (tempDiff > biggest1diff)
                    biggest1diff = tempDiff;
            }
        }

        if (network.GetOutput() == 0)
            correctChoices++;

        Debug.Log("1.0:" + (1 - biggest1diff) + "|||0.4:" + (0.4 + biggest4diff));
        Debug.Log("out of 1 n3 correctly chose : " + correctChoices);
        network.AddDesiredChanges(0.15f);

    }
    public void MakeGoodNN3()
    {
        nn3 = new NeuralNetwork();
        nn3.AddCollumn(0);
        nn3.AddCollumn(1);
        nn3.AddCollumn(2);
        int x;
        for (x = 0; x < 155; x++)//154 + bias node
        {
            nn3.AddNeuron(0, 0);
        }
        for (x = 0; x < 3; x++)
        {
            nn3.AddNeuron(1, 0);
        }
        for (x = 0; x < 6; x++)
        {
            nn3.AddNeuron(2, 0);
        }
        nn3.DeleteNeuron(1, 2);
        nn3.ConnectNeuronToCollumn(0, 154, 1);//biases
        nn3.ConnectNeuronToCollumn(0, 150, 1);//biases
        nn3.AddNeuron(1, 2);
        nn3.ConnectCollumnToCollumn(1, 2);
    }


    public float[] NN4_0Out(int num1, int num2)
    {
        float[] output = new float[] { 0.4f, 0.4f, 0.4f, 0.4f, 0.4f, 0.4f };

        if (num1 == 0 && num2 == 15)
        {
            output[0] = 1;
        }
        if (num1 == 0 && num2 == 7)
        {
            output[1] = 1;
        }
        if ((num1 == 0 && num2 == 18) || (num1 == 10 && num2 == 7))
        {
            output[2] = 1;
        }
        if ((num1 == 7 && num2 == 18) || (num1 == 11 && num2 == 7))
        {
            output[3] = 1;
        }
        if (num1 == 13 && num2 == 7)
        {
            output[4] = 1;
        }
        if (num1 == 7 && num2 == 15)
        {
            output[5] = 1;
        }

        return output;
    }
    public void TrainNN4_0(NeuralNetwork network)
    {
        float[] inputs = new float[154];
        float[] outputs = new float[] { 0.4f, 0.4f, 0.4f, 0.4f, 0.4f, 0.4f };
        int rightChoices = 0;
        float biggest4diff = 0, biggest1diff = 0;
        int x, y;
        int[] piece0spots = new int[] { 0, 7, 10, 11, 13, 1 };//1's are here to give it an example where nothing is happening
        int[] piece6spots = new int[] { 7, 15, 18, 1 };
        for (x = 0; x < 154; x++)
        {
            inputs[x] = 0;
        }

        for (x = 0; x < 6; x++)
        {
            for (y = 0; y < 4; y++)
            {
                outputs = new float[] { 0.4f, 0.4f, 0.4f, 0.4f, 0.4f, 0.4f };

                inputs[151] = 0; // set the cp off
                inputs[piece0spots[x]] = 1;
                inputs[6 * 21 + piece6spots[y]] = 1;

                network.RunInputs(inputs);
                network.BackPropegate(outputs);

                {
                    if (outputs[0] == 0.4f)
                    {
                        float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 0).GetValue() - 0.4f;
                        if (tempDiff > biggest4diff)
                            biggest4diff = tempDiff;
                    }
                    if (outputs[1] == 0.4f)
                    {
                        float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 1).GetValue() - 0.4f;
                        if (tempDiff > biggest4diff)
                            biggest4diff = tempDiff;
                    }
                    if (outputs[2] == 0.4f)
                    {
                        float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 2).GetValue() - 0.4f;
                        if (tempDiff > biggest4diff)
                            biggest4diff = tempDiff;
                    }
                    if (outputs[3] == 0.4f)
                    {
                        float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 3).GetValue() - 0.4f;
                        if (tempDiff > biggest4diff)
                            biggest4diff = tempDiff;
                    }
                    if (outputs[4] == 0.4f)
                    {
                        float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 4).GetValue() - 0.4f;
                        if (tempDiff > biggest4diff)
                            biggest4diff = tempDiff;
                    }
                    if (outputs[5] == 0.4f)
                    {
                        float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 5).GetValue() - 0.4f;
                        if (tempDiff > biggest4diff)
                            biggest4diff = tempDiff;
                    }
                }

                inputs[151] = 1; // set the cp on

                outputs = NN4_0Out(piece0spots[x], piece6spots[y]);//all with the checkpoint

                network.RunInputs(inputs);
                network.BackPropegate(outputs);

                {
                    if (outputs[0] == 0.4f)
                    {
                        float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 0).GetValue() - 0.4f;
                        if (tempDiff > biggest4diff)
                            biggest4diff = tempDiff;
                    }
                    if (outputs[1] == 0.4f)
                    {
                        float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 1).GetValue() - 0.4f;
                        if (tempDiff > biggest4diff)
                            biggest4diff = tempDiff;
                    }
                    if (outputs[2] == 0.4f)
                    {
                        float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 2).GetValue() - 0.4f;
                        if (tempDiff > biggest4diff)
                            biggest4diff = tempDiff;
                    }
                    if (outputs[3] == 0.4f)
                    {
                        float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 3).GetValue() - 0.4f;
                        if (tempDiff > biggest4diff)
                            biggest4diff = tempDiff;
                    }
                    if (outputs[4] == 0.4f)
                    {
                        float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 4).GetValue() - 0.4f;
                        if (tempDiff > biggest4diff)
                            biggest4diff = tempDiff;
                    }
                    if (outputs[5] == 0.4f)
                    {
                        float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 5).GetValue() - 0.4f;
                        if (tempDiff > biggest4diff)
                            biggest4diff = tempDiff;
                    }
                }
                {
                    if (outputs[0] == 1)
                    {
                        float tempDiff = 1 - network.GetNeuron(network.GetSize().Length - 1, 0).GetValue();
                        if (tempDiff > biggest1diff)
                            biggest1diff = tempDiff;
                    }
                    if (outputs[1] == 1)
                    {
                        float tempDiff = 1 - network.GetNeuron(network.GetSize().Length - 1, 1).GetValue();
                        if (tempDiff > biggest1diff)
                            biggest1diff = tempDiff;
                    }
                    if (outputs[2] == 1)
                    {
                        float tempDiff = 1 - network.GetNeuron(network.GetSize().Length - 1, 2).GetValue();
                        if (tempDiff > biggest1diff)
                            biggest1diff = tempDiff;
                    }
                    if (outputs[3] == 1)
                    {
                        float tempDiff = 1 - network.GetNeuron(network.GetSize().Length - 1, 3).GetValue();
                        if (tempDiff > biggest1diff)
                            biggest1diff = tempDiff;
                    }
                    if (outputs[4] == 1)
                    {
                        float tempDiff = 1 - network.GetNeuron(network.GetSize().Length - 1, 4).GetValue();
                        if (tempDiff > biggest1diff)
                            biggest1diff = tempDiff;
                    }
                    if (outputs[5] == 1)
                    {
                        float tempDiff = 1 - network.GetNeuron(network.GetSize().Length - 1, 5).GetValue();
                        if (tempDiff > biggest1diff)
                            biggest1diff = tempDiff;
                    }
                }

                if (outputs[0] == 1 && network.GetOutput() == 0 ||
                    outputs[1] == 1 && network.GetOutput() == 1 ||
                    outputs[2] == 1 && network.GetOutput() == 2 ||
                    outputs[3] == 1 && network.GetOutput() == 3 ||
                    outputs[4] == 1 && network.GetOutput() == 4 ||
                    outputs[5] == 1 && network.GetOutput() == 5)
                    rightChoices++;
                else
                {
                    if (outputs[0] == 1)
                        Debug.Log("CorrectChoice4-0|| actual choice-" + network.GetOutput() + ":::" + piece0spots[x] + ", " + piece6spots[y]);
                    else if (outputs[1] == 1)
                        Debug.Log("CorrectChoice4-1|| actual choice-" + network.GetOutput() + ":::" + piece0spots[x] + ", " + piece6spots[y]);
                    else if (outputs[2] == 1)
                        Debug.Log("CorrectChoice4-2|| actual choice-" + network.GetOutput() + ":::" + piece0spots[x] + ", " + piece6spots[y]);
                    else if (outputs[3] == 1)
                        Debug.Log("CorrectChoice4-3|| actual choice-" + network.GetOutput() + ":::" + piece0spots[x] + ", " + piece6spots[y]);
                    else if (outputs[4] == 1)
                        Debug.Log("CorrectChoice4-4|| actual choice-" + network.GetOutput() + ":::" + piece0spots[x] + ", " + piece6spots[y]);
                    else if (outputs[5] == 1)
                        Debug.Log("CorrectChoice4-5|| actual choice-" + network.GetOutput() + ":::" + piece0spots[x] + ", " + piece6spots[y]);
                }

                inputs[piece0spots[x]] = 0;
                inputs[6 * 21 + piece6spots[y]] = 0;
            }
        }
        Debug.Log("out of 8 n4_0 correctly chose : " + rightChoices);
        Debug.Log("1.0:" + (1 - biggest1diff) + "|||0.4:" + (0.4 + biggest4diff));
        network.AddDesiredChanges(0.10f);
    }
    public void MakeGoodNN4_0()
    {
        nn4_0 = new NeuralNetwork();
        nn4_0.AddCollumn(0);
        nn4_0.AddCollumn(1);
        nn4_0.AddCollumn(2);
        int x;
        for (x = 0; x < 155; x++)//154 + bias node
        {
            nn4_0.AddNeuron(0, 0);
        }
        for (x = 0; x < 13; x++)
        {
            nn4_0.AddNeuron(1, 0);
        }
        for (x = 0; x < 6; x++)
        {
            nn4_0.AddNeuron(2, 0);
        }
        nn4_0.DeleteNeuron(1, 12);
        {
            nn4_0.ConnectNeuronToCollumn(0, 0, 1);
            nn4_0.ConnectNeuronToCollumn(0, 7, 1);
            nn4_0.ConnectNeuronToCollumn(0, 10, 1);
            nn4_0.ConnectNeuronToCollumn(0, 11, 1);
            nn4_0.ConnectNeuronToCollumn(0, 13, 1);

            nn4_0.ConnectNeuronToCollumn(0, 6 * 21 + 7, 1);
            nn4_0.ConnectNeuronToCollumn(0, 6 * 21 + 15, 1);
            nn4_0.ConnectNeuronToCollumn(0, 6 * 21 + 18, 1);

            nn4_0.ConnectNeuronToCollumn(0, 151, 1);
            nn4_0.ConnectNeuronToCollumn(0, 154, 1);
        }
        nn4_0.AddNeuron(1, 12);
        nn4_0.ConnectCollumnToCollumn(1, 2);
    }

    public float[] NN4_1Out(int num1, int num2)
    {
        float[] output = new float[] { 0.4f, 0.4f, 0.4f, 0.4f, 0.4f, 0.4f };

        if (num1 == 0 && num2 == 16)
        {
            output[0] = 1;
        }
        if (num1 == 0 && num2 == 8)
        {
            output[1] = 1;
        }
        if ((num1 == 0 && num2 == 19) || (num1 == 10 && num2 == 8))
        {
            output[2] = 1;
        }
        if ((num1 == 7 && num2 == 19) || (num1 == 11 && num2 == 8))
        {
            output[3] = 1;
        }
        if (num1 == 13 && num2 == 8)
        {
            output[4] = 1;
        }
        if (num1 == 7 && num2 == 16)
        {
            output[5] = 1;
        }

        return output;
    }
    public void TrainNN4_1(NeuralNetwork network)
    {
        float[] inputs = new float[154];
        float[] outputs = new float[] { 0.4f, 0.4f, 0.4f, 0.4f, 0.4f, 0.4f };
        int rightChoices = 0;
        float biggest4diff = 0, biggest1diff = 0;
        int x, y;
        int[] piece0spots = new int[] { 0, 7, 10, 11, 13, 1 };//1's are here to give it an example where nothing is happening
        int[] piece6spots = new int[] { 8, 16, 19, 1 };
        for (x = 0; x < 154; x++)
        {
            inputs[x] = 0;
        }

        for (x = 0; x < 6; x++)
        {
            for (y = 0; y < 4; y++)
            {
                outputs = new float[] { 0.4f, 0.4f, 0.4f, 0.4f, 0.4f, 0.4f };

                inputs[151] = 0; // set the cp off
                inputs[piece0spots[x]] = 1;
                inputs[6 * 21 + piece6spots[y]] = 1;

                network.RunInputs(inputs);
                network.BackPropegate(outputs);

                {
                    if (outputs[0] == 0.4f)
                    {
                        float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 0).GetValue() - 0.4f;
                        if (tempDiff > biggest4diff)
                            biggest4diff = tempDiff;
                    }
                    if (outputs[1] == 0.4f)
                    {
                        float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 1).GetValue() - 0.4f;
                        if (tempDiff > biggest4diff)
                            biggest4diff = tempDiff;
                    }
                    if (outputs[2] == 0.4f)
                    {
                        float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 2).GetValue() - 0.4f;
                        if (tempDiff > biggest4diff)
                            biggest4diff = tempDiff;
                    }
                    if (outputs[3] == 0.4f)
                    {
                        float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 3).GetValue() - 0.4f;
                        if (tempDiff > biggest4diff)
                            biggest4diff = tempDiff;
                    }
                    if (outputs[4] == 0.4f)
                    {
                        float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 4).GetValue() - 0.4f;
                        if (tempDiff > biggest4diff)
                            biggest4diff = tempDiff;
                    }
                    if (outputs[5] == 0.4f)
                    {
                        float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 5).GetValue() - 0.4f;
                        if (tempDiff > biggest4diff)
                            biggest4diff = tempDiff;
                    }
                }

                inputs[151] = 1; // set the cp on
                outputs = NN4_1Out(piece0spots[x], piece6spots[y]);//all with the checkpoint
                network.RunInputs(inputs);
                network.BackPropegate(outputs);


                {
                    if (outputs[0] == 0.4f)
                    {
                        float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 0).GetValue() - 0.4f;
                        if (tempDiff > biggest4diff)
                            biggest4diff = tempDiff;
                    }
                    if (outputs[1] == 0.4f)
                    {
                        float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 1).GetValue() - 0.4f;
                        if (tempDiff > biggest4diff)
                            biggest4diff = tempDiff;
                    }
                    if (outputs[2] == 0.4f)
                    {
                        float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 2).GetValue() - 0.4f;
                        if (tempDiff > biggest4diff)
                            biggest4diff = tempDiff;
                    }
                    if (outputs[3] == 0.4f)
                    {
                        float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 3).GetValue() - 0.4f;
                        if (tempDiff > biggest4diff)
                            biggest4diff = tempDiff;
                    }
                    if (outputs[4] == 0.4f)
                    {
                        float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 4).GetValue() - 0.4f;
                        if (tempDiff > biggest4diff)
                            biggest4diff = tempDiff;
                    }
                    if (outputs[5] == 0.4f)
                    {
                        float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 5).GetValue() - 0.4f;
                        if (tempDiff > biggest4diff)
                            biggest4diff = tempDiff;
                    }
                }
                {
                    if (outputs[0] == 1)
                    {
                        float tempDiff = 1 - network.GetNeuron(network.GetSize().Length - 1, 0).GetValue();
                        if (tempDiff > biggest1diff)
                            biggest1diff = tempDiff;
                        Debug.Log(network.GetNeuron(network.GetSize().Length - 1, 0).GetValue());
                    }
                    if (outputs[1] == 1)
                    {
                        float tempDiff = 1 - network.GetNeuron(network.GetSize().Length - 1, 1).GetValue();
                        if (tempDiff > biggest1diff)
                            biggest1diff = tempDiff;
                        Debug.Log(network.GetNeuron(network.GetSize().Length - 1, 1).GetValue());
                    }
                    if (outputs[2] == 1)
                    {
                        float tempDiff = 1 - network.GetNeuron(network.GetSize().Length - 1, 2).GetValue();
                        if (tempDiff > biggest1diff)
                            biggest1diff = tempDiff;
                        Debug.Log(network.GetNeuron(network.GetSize().Length - 1, 2).GetValue());
                    }
                    if (outputs[3] == 1)
                    {
                        float tempDiff = 1 - network.GetNeuron(network.GetSize().Length - 1, 3).GetValue();
                        if (tempDiff > biggest1diff)
                            biggest1diff = tempDiff;
                        Debug.Log(network.GetNeuron(network.GetSize().Length - 1, 3).GetValue());
                    }
                    if (outputs[4] == 1)
                    {
                        float tempDiff = 1 - network.GetNeuron(network.GetSize().Length - 1, 4).GetValue();
                        if (tempDiff > biggest1diff)
                            biggest1diff = tempDiff;
                        Debug.Log(network.GetNeuron(network.GetSize().Length - 1, 4).GetValue());
                    }
                    if (outputs[5] == 1)
                    {
                        float tempDiff = 1 - network.GetNeuron(network.GetSize().Length - 1, 5).GetValue();
                        if (tempDiff > biggest1diff)
                            biggest1diff = tempDiff;
                        Debug.Log(network.GetNeuron(network.GetSize().Length - 1, 5).GetValue());
                    }
                }

                if (outputs[0] == 1 && network.GetOutput() == 0 ||
                    outputs[1] == 1 && network.GetOutput() == 1 ||
                    outputs[2] == 1 && network.GetOutput() == 2 ||
                    outputs[3] == 1 && network.GetOutput() == 3 ||
                    outputs[4] == 1 && network.GetOutput() == 4 ||
                    outputs[5] == 1 && network.GetOutput() == 5)
                    rightChoices++;
                else
                {
                    if (outputs[0] == 1)
                        Debug.Log("CorrectChoice4-0|| actual choice-" + network.GetOutput() + ":::" + piece0spots[x] + ", " + piece6spots[y]);
                    else if (outputs[1] == 1)
                        Debug.Log("CorrectChoice4-1|| actual choice-" + network.GetOutput() + ":::" + piece0spots[x] + ", " + piece6spots[y]);
                    else if (outputs[2] == 1)
                        Debug.Log("CorrectChoice4-2|| actual choice-" + network.GetOutput() + ":::" + piece0spots[x] + ", " + piece6spots[y]);
                    else if (outputs[3] == 1)
                        Debug.Log("CorrectChoice4-3|| actual choice-" + network.GetOutput() + ":::" + piece0spots[x] + ", " + piece6spots[y]);
                    else if (outputs[4] == 1)
                        Debug.Log("CorrectChoice4-4|| actual choice-" + network.GetOutput() + ":::" + piece0spots[x] + ", " + piece6spots[y]);
                    else if (outputs[5] == 1)
                        Debug.Log("CorrectChoice4-5|| actual choice-" + network.GetOutput() + ":::" + piece0spots[x] + ", " + piece6spots[y]);
                }
                inputs[piece0spots[x]] = 0;
                inputs[6 * 21 + piece6spots[y]] = 0;
            }
        }
        Debug.Log("1.0:" + (1 - biggest1diff) + "|||0.4:" + (0.4 + biggest4diff));
        Debug.Log("out of 8 n4_1 correctly chose : " + rightChoices);
        network.AddDesiredChanges(0.10f);
    }
    public void MakeGoodNN4_1()
    {
        nn4_1 = new NeuralNetwork();
        nn4_1.AddCollumn(0);
        nn4_1.AddCollumn(1);
        nn4_1.AddCollumn(2);
        int x;
        for (x = 0; x < 155; x++)//154 + bias node
        {
            nn4_1.AddNeuron(0, 0);
        }
        for (x = 0; x < 13; x++)
        {
            nn4_1.AddNeuron(1, 0);
        }
        for (x = 0; x < 6; x++)
        {
            nn4_1.AddNeuron(2, 0);
        }
        nn4_1.DeleteNeuron(1, 12);
        {
            nn4_1.ConnectNeuronToCollumn(0, 0, 1);
            nn4_1.ConnectNeuronToCollumn(0, 7, 1);
            nn4_1.ConnectNeuronToCollumn(0, 10, 1);
            nn4_1.ConnectNeuronToCollumn(0, 11, 1);
            nn4_1.ConnectNeuronToCollumn(0, 13, 1);

            nn4_1.ConnectNeuronToCollumn(0, 6 * 21 + 8, 1);
            nn4_1.ConnectNeuronToCollumn(0, 6 * 21 + 16, 1);
            nn4_1.ConnectNeuronToCollumn(0, 6 * 21 + 19, 1);

            nn4_1.ConnectNeuronToCollumn(0, 151, 1);
            nn4_1.ConnectNeuronToCollumn(0, 154, 1);
        }
        nn4_1.AddNeuron(1, 12);
        nn4_1.ConnectCollumnToCollumn(1, 2);
    }

    public float[] NN4_2Out(int num1, int num2)
    {
        float[] output = new float[] { 0.4f, 0.4f, 0.4f, 0.4f, 0.4f, 0.4f };

        if (num1 == 0 && num2 == 17)
        {
            output[0] = 1;
        }
        if (num1 == 0 && num2 == 6)
        {
            output[1] = 1;
        }
        if ((num1 == 0 && num2 == 20) || (num1 == 10 && num2 == 6))
        {
            output[2] = 1;
        }
        if ((num1 == 7 && num2 == 20) || (num1 == 11 && num2 == 6))
        {
            output[3] = 1;
        }
        if (num1 == 13 && num2 == 6)
        {
            output[4] = 1;
        }
        if (num1 == 7 && num2 == 17)
        {
            output[5] = 1;
        }

        return output;
    }
    public void TrainNN4_2(NeuralNetwork network)
    {
        float[] inputs = new float[154];
        float[] outputs = new float[] { 0.4f, 0.4f, 0.4f, 0.4f, 0.4f, 0.4f };
        int rightChoices = 0;
        float biggest4diff = 0, biggest1diff = 0;
        int x, y;
        int[] piece0spots = new int[] { 0, 7, 10, 11, 13, 1 };//1's are here to give it an example where nothing is happening
        int[] piece6spots = new int[] { 6, 17, 20, 1 };
        for (x = 0; x < 154; x++)
        {
            inputs[x] = 0;
        }

        for (x = 0; x < 6; x++)
        {
            for (y = 0; y < 4; y++)
            {
                outputs = new float[] { 0.4f, 0.4f, 0.4f, 0.4f, 0.4f, 0.4f };

                inputs[151] = 0; // set the cp off
                inputs[piece0spots[x]] = 1;
                inputs[6 * 21 + piece6spots[y]] = 1;

                network.RunInputs(inputs);
                network.BackPropegate(outputs);

                {
                    if (outputs[0] == 0.4f)
                    {
                        float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 0).GetValue() - 0.4f;
                        if (tempDiff > biggest4diff)
                            biggest4diff = tempDiff;
                    }
                    if (outputs[1] == 0.4f)
                    {
                        float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 1).GetValue() - 0.4f;
                        if (tempDiff > biggest4diff)
                            biggest4diff = tempDiff;
                    }
                    if (outputs[2] == 0.4f)
                    {
                        float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 2).GetValue() - 0.4f;
                        if (tempDiff > biggest4diff)
                            biggest4diff = tempDiff;
                    }
                    if (outputs[3] == 0.4f)
                    {
                        float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 3).GetValue() - 0.4f;
                        if (tempDiff > biggest4diff)
                            biggest4diff = tempDiff;
                    }
                    if (outputs[4] == 0.4f)
                    {
                        float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 4).GetValue() - 0.4f;
                        if (tempDiff > biggest4diff)
                            biggest4diff = tempDiff;
                    }
                    if (outputs[5] == 0.4f)
                    {
                        float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 5).GetValue() - 0.4f;
                        if (tempDiff > biggest4diff)
                            biggest4diff = tempDiff;
                    }
                }

                outputs = NN4_2Out(piece0spots[x], piece6spots[y]);//all with the checkpoint
                inputs[151] = 1; // set the cp on
                network.RunInputs(inputs);
                network.BackPropegate(outputs);

                {
                    if (outputs[0] == 0.4f)
                    {
                        float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 0).GetValue() - 0.4f;
                        if (tempDiff > biggest4diff)
                            biggest4diff = tempDiff;
                    }
                    if (outputs[1] == 0.4f)
                    {
                        float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 1).GetValue() - 0.4f;
                        if (tempDiff > biggest4diff)
                            biggest4diff = tempDiff;
                    }
                    if (outputs[2] == 0.4f)
                    {
                        float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 2).GetValue() - 0.4f;
                        if (tempDiff > biggest4diff)
                            biggest4diff = tempDiff;
                    }
                    if (outputs[3] == 0.4f)
                    {
                        float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 3).GetValue() - 0.4f;
                        if (tempDiff > biggest4diff)
                            biggest4diff = tempDiff;
                    }
                    if (outputs[4] == 0.4f)
                    {
                        float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 4).GetValue() - 0.4f;
                        if (tempDiff > biggest4diff)
                            biggest4diff = tempDiff;
                    }
                    if (outputs[5] == 0.4f)
                    {
                        float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 5).GetValue() - 0.4f;
                        if (tempDiff > biggest4diff)
                            biggest4diff = tempDiff;
                    }
                }
                {
                    if (outputs[0] == 1)
                    {
                        float tempDiff = 1 - network.GetNeuron(network.GetSize().Length - 1, 0).GetValue();
                        if (tempDiff > biggest1diff)
                            biggest1diff = tempDiff;
                    }
                    if (outputs[1] == 1)
                    {
                        float tempDiff = 1 - network.GetNeuron(network.GetSize().Length - 1, 1).GetValue();
                        if (tempDiff > biggest1diff)
                            biggest1diff = tempDiff;
                    }
                    if (outputs[2] == 1)
                    {
                        float tempDiff = 1 - network.GetNeuron(network.GetSize().Length - 1, 2).GetValue();
                        if (tempDiff > biggest1diff)
                            biggest1diff = tempDiff;
                    }
                    if (outputs[3] == 1)
                    {
                        float tempDiff = 1 - network.GetNeuron(network.GetSize().Length - 1, 3).GetValue();
                        if (tempDiff > biggest1diff)
                            biggest1diff = tempDiff;
                    }
                    if (outputs[4] == 1)
                    {
                        float tempDiff = 1 - network.GetNeuron(network.GetSize().Length - 1, 4).GetValue();
                        if (tempDiff > biggest1diff)
                            biggest1diff = tempDiff;
                    }
                    if (outputs[5] == 1)
                    {
                        float tempDiff = 1 - network.GetNeuron(network.GetSize().Length - 1, 5).GetValue();
                        if (tempDiff > biggest1diff)
                            biggest1diff = tempDiff;
                    }
                }


                if (outputs[0] == 1 && network.GetOutput() == 0 ||
                    outputs[1] == 1 && network.GetOutput() == 1 ||
                    outputs[2] == 1 && network.GetOutput() == 2 ||
                    outputs[3] == 1 && network.GetOutput() == 3 ||
                    outputs[4] == 1 && network.GetOutput() == 4 ||
                    outputs[5] == 1 && network.GetOutput() == 5)
                    rightChoices++;
                else
                {
                    if (outputs[0] == 1)
                        Debug.Log("CorrectChoice4-0|| actual choice-" + network.GetOutput() + ":::" + piece0spots[x] + ", " + piece6spots[y]);
                    else if (outputs[1] == 1)
                        Debug.Log("CorrectChoice4-1|| actual choice-" + network.GetOutput() + ":::" + piece0spots[x] + ", " + piece6spots[y]);
                    else if (outputs[2] == 1)
                        Debug.Log("CorrectChoice4-2|| actual choice-" + network.GetOutput() + ":::" + piece0spots[x] + ", " + piece6spots[y]);
                    else if (outputs[3] == 1)
                        Debug.Log("CorrectChoice4-3|| actual choice-" + network.GetOutput() + ":::" + piece0spots[x] + ", " + piece6spots[y]);
                    else if (outputs[4] == 1)
                        Debug.Log("CorrectChoice4-4|| actual choice-" + network.GetOutput() + ":::" + piece0spots[x] + ", " + piece6spots[y]);
                    else if (outputs[5] == 1)
                        Debug.Log("CorrectChoice4-5|| actual choice-" + network.GetOutput() + ":::" + piece0spots[x] + ", " + piece6spots[y]);
                }

                inputs[piece0spots[x]] = 0;
                inputs[6 * 21 + piece6spots[y]] = 0;
            }
        }
        Debug.Log("1.0:" + (1 - biggest1diff) + "|||0.4:" + (0.4 + biggest4diff));
        Debug.Log("out of 8 n4_2 correctly chose : " + rightChoices);
        network.AddDesiredChanges(0.10f);
    }
    public void MakeGoodNN4_2()
    {
        nn4_2 = new NeuralNetwork();
        nn4_2.AddCollumn(0);
        nn4_2.AddCollumn(1);
        nn4_2.AddCollumn(2);
        int x;
        for (x = 0; x < 155; x++)//154 + bias node
        {
            nn4_2.AddNeuron(0, 0);
        }
        for (x = 0; x < 13; x++)
        {
            nn4_2.AddNeuron(1, 0);
        }
        for (x = 0; x < 6; x++)
        {
            nn4_2.AddNeuron(2, 0);
        }
        nn4_2.DeleteNeuron(1, 12);
        {
            nn4_2.ConnectNeuronToCollumn(0, 0, 1);
            nn4_2.ConnectNeuronToCollumn(0, 7, 1);
            nn4_2.ConnectNeuronToCollumn(0, 10, 1);
            nn4_2.ConnectNeuronToCollumn(0, 11, 1);
            nn4_2.ConnectNeuronToCollumn(0, 13, 1);

            nn4_2.ConnectNeuronToCollumn(0, 6 * 21 + 6, 1);
            nn4_2.ConnectNeuronToCollumn(0, 6 * 21 + 17, 1);
            nn4_2.ConnectNeuronToCollumn(0, 6 * 21 + 20, 1);

            nn4_2.ConnectNeuronToCollumn(0, 151, 1);
            nn4_2.ConnectNeuronToCollumn(0, 154, 1);
        }
        nn4_2.AddNeuron(1, 12);
        nn4_2.ConnectCollumnToCollumn(1, 2);
    }

    public float[] NN5_0Out(int num0, int num1, int num2)
    {
        float[] output = new float[] { 0.4f, 0.4f, 0.4f, 0.4f, 0.4f, 0.4f };

        //b
        if ((num0 == 5 && num1 == 17) ||
            (num0 == 5 && num1 == 19) ||
            (num0 == 7 && num1 == 3 && num2 == 16) ||
            (num0 == 7 && num1 == 10) ||
            (num0 == 7 && num1 == 14))
        {
            output[0] = 1;
        }
        //r
        if (num0 == 5 && num1 == 14)
        {
            output[1] = 1;
        }
        //w
        if ((num0 == 0 && num1 == 3) ||
            (num0 == 5 && num1 == 8))
        {
            output[2] = 1;
        }
        //r'
        if ((num0 == 0 && num1 == 8) ||
            (num0 == 7 && num1 == 3 && num2 == 20) ||
            (num0 == 7 && num1 == 17 && num2 == 11))
        {
            output[4] = 1;
        }
        //w'
        if (num0 == 7 && num1 == 17 && num2 == 0)
        {
            output[5] = 1;
        }
        return output;
    }
    public void TrainNN5(NeuralNetwork network)
    {
        float[] inputs = new float[154];
        float[] outputs = new float[6];
        int x, y, z;
        int rightChoices = 0; //keeps track of right choices to display in debug.log
        int[] pieces0 = { 0, 5, 7, 13 }, pieces1 = { 3, 8, 10, 14, 17, 19, 13 }, pieces2 = { 0, 11, 16, 20, 13 };
        int[] combos0 = { 00, 07, 07, 07, 07, 07, 07, 00, 05, 05, 05, 05 },
              combos1 = { 03, 03, 03, 14, 10, 17, 17, 08, 08, 17, 19, 14 },
              combos2 = { 06, 16, 20, 20, 13, 11, 00, 10, 01, 06, 06, 06 };// these represent the combinations that matter for solving
        for (x = 0; x < 154; x++)
        {
            inputs[x] = 0;
        }
        float biggest4diff = 0, biggest1diff = 0;
        for (x = 0; x < combos0.Length; x++)
        {
            inputs[152] = 1;
            inputs[0 + combos0[x]] = 1;
            inputs[21 + combos1[x]] = 1;
            inputs[42 + combos2[x]] = 1;
            outputs = NN5_0Out(combos0[x], combos1[x], combos2[x]);

            network.RunInputs(inputs);
            network.BackPropegate(outputs);

            {
                if (outputs[0] == 0.4f)
                {
                    float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 0).GetValue() - 0.4f;
                    if (tempDiff > biggest4diff)
                        biggest4diff = tempDiff;
                }
                if (outputs[1] == 0.4f)
                {
                    float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 1).GetValue() - 0.4f;
                    if (tempDiff > biggest4diff)
                        biggest4diff = tempDiff;
                }
                if (outputs[2] == 0.4f)
                {
                    float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 2).GetValue() - 0.4f;
                    if (tempDiff > biggest4diff)
                        biggest4diff = tempDiff;
                }
                if (outputs[3] == 0.4f)
                {
                    float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 3).GetValue() - 0.4f;
                    if (tempDiff > biggest4diff)
                        biggest4diff = tempDiff;
                }
                if (outputs[4] == 0.4f)
                {
                    float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 4).GetValue() - 0.4f;
                    if (tempDiff > biggest4diff)
                        biggest4diff = tempDiff;
                }
                if (outputs[5] == 0.4f)
                {
                    float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 5).GetValue() - 0.4f;
                    if (tempDiff > biggest4diff)
                        biggest4diff = tempDiff;
                }
            }
            {
                if (outputs[0] == 1)
                {
                    float tempDiff = 1 - network.GetNeuron(network.GetSize().Length - 1, 0).GetValue();
                    if (tempDiff > biggest1diff)
                        biggest1diff = tempDiff;
                }
                if (outputs[1] == 1)
                {
                    float tempDiff = 1 - network.GetNeuron(network.GetSize().Length - 1, 1).GetValue();
                    if (tempDiff > biggest1diff)
                        biggest1diff = tempDiff;
                }
                if (outputs[2] == 1)
                {
                    float tempDiff = 1 - network.GetNeuron(network.GetSize().Length - 1, 2).GetValue();
                    if (tempDiff > biggest1diff)
                        biggest1diff = tempDiff;
                }
                if (outputs[3] == 1)
                {
                    float tempDiff = 1 - network.GetNeuron(network.GetSize().Length - 1, 3).GetValue();
                    if (tempDiff > biggest1diff)
                        biggest1diff = tempDiff;
                }
                if (outputs[4] == 1)
                {
                    float tempDiff = 1 - network.GetNeuron(network.GetSize().Length - 1, 4).GetValue();
                    if (tempDiff > biggest1diff)
                        biggest1diff = tempDiff;
                }
                if (outputs[5] == 1)
                {
                    float tempDiff = 1 - network.GetNeuron(network.GetSize().Length - 1, 5).GetValue();
                    if (tempDiff > biggest1diff)
                        biggest1diff = tempDiff;
                }
            }

            if (outputs[0] == 1 && network.GetOutput() == 0 ||
                                    outputs[1] == 1 && network.GetOutput() == 1 ||
                                    outputs[2] == 1 && network.GetOutput() == 2 ||
                                    outputs[3] == 1 && network.GetOutput() == 3 ||
                                    outputs[4] == 1 && network.GetOutput() == 4 ||
                                    outputs[5] == 1 && network.GetOutput() == 5)
                rightChoices++;
            else
            {
                if (outputs[0] == 1)
                    Debug.Log("CorrectChoice4-0|| actual choice-" + network.GetOutput() + ":::");
                else if (outputs[1] == 1)
                    Debug.Log("CorrectChoice4-1|| actual choice-" + network.GetOutput() + ":::");
                else if (outputs[2] == 1)
                    Debug.Log("CorrectChoice4-2|| actual choice-" + network.GetOutput() + ":::");
                else if (outputs[3] == 1)
                    Debug.Log("CorrectChoice4-3|| actual choice-" + network.GetOutput() + ":::");
                else if (outputs[4] == 1)
                    Debug.Log("CorrectChoice4-4|| actual choice-" + network.GetOutput() + ":::");
                else if (outputs[5] == 1)
                    Debug.Log("CorrectChoice4-5|| actual choice-" + network.GetOutput() + ":::");
            }

            inputs[152] = 0;
            inputs[00 + combos0[x]] = 0;
            inputs[21 + combos1[x]] = 0;
            inputs[42 + combos2[x]] = 0;
        }


        Debug.Log("Ontime -- 1.0:" + (1 - biggest1diff) + "|||0.4:" + (0.4 + biggest4diff));
        biggest4diff = 0;
        biggest1diff = 0;
        //network.AddDesiredChanges(0.01f);

        for (x = 0; x < 4; x++)  //for (x = 0; x < -1; x++)
        {
            for (y = 0; y < 7; y++)
            {
                for (z = 0; z < 5; z++)
                {

                    inputs[00 + pieces0[x]] = 1;
                    inputs[21 + pieces1[y]] = 1;
                    inputs[42 + pieces2[z]] = 1;
                    inputs[152] = 0;

                    outputs = new float[] { 0.4f, 0.4f, 0.4f, 0.4f, 0.4f, 0.4f };

                    network.RunInputs(inputs);
                    network.BackPropegate(outputs);

                    {
                        if (outputs[0] == 0.4f)
                        {
                            float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 0).GetValue() - 0.4f;
                            if (tempDiff > biggest4diff)
                                biggest4diff = tempDiff;
                        }
                        if (outputs[1] == 0.4f)
                        {
                            float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 1).GetValue() - 0.4f;
                            if (tempDiff > biggest4diff)
                                biggest4diff = tempDiff;
                        }
                        if (outputs[2] == 0.4f)
                        {
                            float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 2).GetValue() - 0.4f;
                            if (tempDiff > biggest4diff)
                                biggest4diff = tempDiff;
                        }
                        if (outputs[3] == 0.4f)
                        {
                            float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 3).GetValue() - 0.4f;
                            if (tempDiff > biggest4diff)
                                biggest4diff = tempDiff;
                        }
                        if (outputs[4] == 0.4f)
                        {
                            float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 4).GetValue() - 0.4f;
                            if (tempDiff > biggest4diff)
                                biggest4diff = tempDiff;
                        }
                        if (outputs[5] == 0.4f)
                        {
                            float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 5).GetValue() - 0.4f;
                            if (tempDiff > biggest4diff)
                                biggest4diff = tempDiff;
                        }
                    }
                    inputs[00 + pieces0[x]] = 0;
                    inputs[21 + pieces1[y]] = 0;
                    inputs[42 + pieces2[z]] = 0;
                    inputs[152] = 0;
                }
            }
        }

        Debug.Log("Offtime -- 1.0:" + (1 - biggest1diff) + "|||0.4:" + (0.4 + biggest4diff));
        Debug.Log("out of 12 n5_0 correctly chose : " + rightChoices + "-----------------------------------");
        network.AddDesiredChanges(0.01f);
    }
    public void MakeGoodNN5_0()
    {
        nn5 = new NeuralNetwork();
        nn5.AddCollumn(0);
        nn5.AddCollumn(1);
        nn5.AddCollumn(2);
        nn5.AddCollumn(3);
        int x;
        for (x = 0; x < 155; x++)//3 + bias node
        {
            nn5.AddNeuron(0, 0);
        }
        for (x = 0; x < 25; x++)
        {
            nn5.AddNeuron(1, 0);
        }
        for (x = 0; x < 25; x++)
        {
            nn5.AddNeuron(2, 0);
        }
        for (x = 0; x < 6; x++)
        {
            nn5.AddNeuron(3, 0);
        }

        for (x = 0; x < 12; x++)
        {
            int p0 = 0, p1 = 0, choice = 0;
            switch (x)
            {
                case 0:
                    p0 = 0;
                    p1 = 21 + 3;
                    choice = 2;
                    break;
                case 1:
                    p0 = 0;
                    p1 = 21 + 8;
                    choice = 4;
                    break;
                case 2:
                    p0 = 5;
                    p1 = 21 + 8;
                    choice = 2;
                    break;
                case 3:
                    p0 = 5;
                    p1 = 21 + 14;
                    choice = 1;
                    break;
                case 4:
                    p0 = 5;
                    p1 = 21 + 17;
                    choice = 0;
                    break;
                case 5:
                    p0 = 5;
                    p1 = 21 + 19;
                    choice = 0;
                    break;
                case 6:
                    p0 = 21 + 3;
                    p1 = 42 + 16;
                    choice = 0;
                    break;
                case 7:
                    p0 = 21 + 3;
                    p1 = 42 + 20;
                    choice = 4;
                    break;
                case 8:
                    p0 = 7;
                    p1 = 21 + 10;
                    choice = 0;
                    break;
                case 9:
                    p0 = 7;
                    p1 = 21 + 14;
                    choice = 0;
                    break;
                case 10:
                    p0 = 21 + 17;
                    p1 = 42 + 0;
                    choice = 5;
                    break;
                case 11:
                    p0 = 21 + 17;
                    p1 = 42 + 11;
                    choice = 4;
                    break;
            }


            //to layer 1
            //node 0 
            nn5.AddConnection(0, p0, 1, x * 2, -0.383487f);
            nn5.AddConnection(0, p0, 1, x * 2 + 1, 4.895471f);
            //node 2
            nn5.AddConnection(0, 152, 1, x * 2, -0.3173361f);
            nn5.AddConnection(0, 152, 1, x * 2 + 1, 4.944991f);
            //node 3
            nn5.AddConnection(0, 154, 1, x * 2, 0.9871098f);
            nn5.AddConnection(0, 154, 1, x * 2 + 1, -7.647758f);

            //to layer 2
            //node 1
            nn5.AddConnection(0, p1, 2, x * 2, -5.810718f);
            nn5.AddConnection(0, p1, 2, x * 2 + 1, -0.2540283f);


            nn5.AddConnection(1, x * 2, 2, x * 2, -2.227202f);
            nn5.AddConnection(1, x * 2, 2, x * 2 + 1, -1.523751f);
            nn5.AddConnection(1, x * 2 + 1, 2, x * 2, 8.22331f);
            nn5.AddConnection(1, x * 2 + 1, 2, x * 2 + 1, 6.760274f);
            nn5.AddConnection(1, 24, 2, x * 2, -4.273445f);
            nn5.AddConnection(1, 24, 2, x * 2 + 1, -3.444222f);

            //to layer 3 output white
            nn5.AddConnection(2, x * 2, 3, choice, -6.172615f);
            nn5.AddConnection(2, x * 2 + 1, 3, choice, 6.331503f);
            nn5.AddConnection(2, 24, 3, choice, -0.4769176f);
        }


        //biases only blue prime
        nn5.AddConnection(2, 24, 3, 3, -0.4054653f);
    }
    public void MakeGoodNN5()
    {
        nn5 = new NeuralNetwork();
        nn5.AddCollumn(0);
        nn5.AddCollumn(1);
        nn5.AddCollumn(2);
        nn5.AddCollumn(3);
        int x;
        for (x = 0; x < 155; x++)//3 + bias node
        {
            nn5.AddNeuron(0, 0);
        }
        for (x = 0; x < 25; x++)
        {
            nn5.AddNeuron(1, 0);
        }
        for (x = 0; x < 25; x++)
        {
            nn5.AddNeuron(2, 0);
        }
        for (x = 0; x < 6; x++)
        {
            nn5.AddNeuron(3, 0);
        }

        for (x = 0; x < 12; x++)
        {
            int p0 = 0, p1 = 0, choice = 0;
            switch (x)
            {
                case 0:
                    p0 = 0;
                    p1 = 21 + 3;
                    choice = 2;
                    break;
                case 1:
                    p0 = 0;
                    p1 = 21 + 8;
                    choice = 4;
                    break;
                case 2:
                    p0 = 5;
                    p1 = 21 + 8;
                    choice = 2;
                    break;
                case 3:
                    p0 = 5;
                    p1 = 21 + 14;
                    choice = 1;
                    break;
                case 4:
                    p0 = 5;
                    p1 = 21 + 17;
                    choice = 0;
                    break;
                case 5:
                    p0 = 5;
                    p1 = 21 + 19;
                    choice = 0;
                    break;
                case 6:
                    p0 = 21 + 3;
                    p1 = 42 + 16;
                    choice = 0;
                    break;
                case 7:
                    p0 = 21 + 3;
                    p1 = 42 + 20;
                    choice = 4;
                    break;
                case 8:
                    p0 = 7;
                    p1 = 21 + 10;
                    choice = 0;
                    break;
                case 9:
                    p0 = 7;
                    p1 = 21 + 14;
                    choice = 0;
                    break;
                case 10:
                    p0 = 21 + 17;
                    p1 = 42 + 0;
                    choice = 5;
                    break;
                case 11:
                    p0 = 21 + 17;
                    p1 = 42 + 11;
                    choice = 4;
                    break;
            }


            //to layer 1
            //node 0 
            nn5.AddConnection(0, p0, 1, x * 2, Random.Range(-1f, 1f));
            nn5.AddConnection(0, p0, 1, x * 2 + 1, Random.Range(-1f, 1f));
            //node 2
            nn5.AddConnection(0, 152, 1, x * 2, Random.Range(-1f, 1f));
            nn5.AddConnection(0, 152, 1, x * 2 + 1, Random.Range(-1f, 1f));
            //node 3
            nn5.AddConnection(0, 154, 1, x * 2, Random.Range(-1f, 1f));
            nn5.AddConnection(0, 154, 1, x * 2 + 1, Random.Range(-1f, 1f));

            //to layer 2
            //node 1
            nn5.AddConnection(0, p1, 2, x * 2, Random.Range(-1f, 1f));
            nn5.AddConnection(0, p1, 2, x * 2 + 1, Random.Range(-1f, 1f));


            nn5.AddConnection(1, x * 2, 2, x * 2, Random.Range(-1f, 1f));
            nn5.AddConnection(1, x * 2, 2, x * 2 + 1, Random.Range(-1f, 1f));
            nn5.AddConnection(1, x * 2 + 1, 2, x * 2, Random.Range(-1f, 1f));
            nn5.AddConnection(1, x * 2 + 1, 2, x * 2 + 1, Random.Range(-1f, 1f));
            nn5.AddConnection(1, 24, 2, x * 2, Random.Range(-1f, 1f));
            nn5.AddConnection(1, 24, 2, x * 2 + 1, Random.Range(-1f, 1f));

            //to layer 3 output white
            nn5.AddConnection(2, x * 2, 3, choice, Random.Range(-1f, 1f));
            nn5.AddConnection(2, x * 2 + 1, 3, choice, Random.Range(-1f, 1f));
            nn5.AddConnection(2, 24, 3, choice, Random.Range(-1f, 1f));
        }


        //biases only blue prime
        nn5.AddConnection(2, 24, 3, 3, Random.Range(-1f, 1f));
    }


    public float[] NN5_1Out(int num0, int num1, int num2)
    {
        float[] output = new float[] { 0.4f, 0.4f, 0.4f, 0.4f, 0.4f, 0.4f };

        //b
        if ((num0 == 5 && num1 == 17) || (num0 == 5 && num1 == 19) || (num0 == 7 && num1 == 3 && num2 == 16)
             || (num0 == 7 && num1 == 10) || (num0 == 7 && num1 == 14))
        {
            output[0] = 1;
        }
        //r
        if (num0 == 5 && num1 == 14)
        {
            output[1] = 1;
        }
        //w
        if ((num0 == 0 && num1 == 3) || (num0 == 5 && num1 == 8))
        {
            output[2] = 1;
        }
        //r'
        if ((num0 == 0 && num1 == 8) || (num0 == 7 && num1 == 3 && num2 == 20))
        {
            output[4] = 1;
        }
        //w'
        if (num0 == 7 && num1 == 17 && num2 == 11)
        {
            output[5] = 1;
        }
        return output;
    }
    public void TrainNN5_1(NeuralNetwork network)
    {
        float[] inputs = new float[154];
        float[] outputs = new float[6];
        int x, y, z;
        int rightChoices = 0;
        int[] pieces0 = { 0, 5, 7, 13 }, pieces1 = { 3, 8, 10, 14, 17, 19, 13 }, pieces2 = { 0, 11, 16, 20, 13 };
        float biggest4diff = 0, biggest1diff = 0;
        for (x = 0; x < 154; x++)
        {
            inputs[x] = 0;
        }
        for (x = 0; x < 4; x++)
        {
            for (y = 0; y < pieces1.Length; y++)
            {
                for (z = 0; z < 4; z++)
                {

                    inputs[00 + pieces0[x]] = 1;
                    inputs[21 + pieces1[y]] = 1;
                    inputs[42 + pieces2[z]] = 1;
                    inputs[152] = 0;

                    outputs = new float[] { 0.4f, 0.4f, 0.4f, 0.4f, 0.4f, 0.4f };

                    network.RunInputs(inputs);
                    network.BackPropegate(outputs);

                    {
                        if (outputs[0] == 0.4f)
                        {
                            float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 0).GetValue() - 0.4f;
                            if (tempDiff > biggest4diff)
                                biggest4diff = tempDiff;
                        }
                        if (outputs[1] == 0.4f)
                        {
                            float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 1).GetValue() - 0.4f;
                            if (tempDiff > biggest4diff)
                                biggest4diff = tempDiff;
                        }
                        if (outputs[2] == 0.4f)
                        {
                            float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 2).GetValue() - 0.4f;
                            if (tempDiff > biggest4diff)
                                biggest4diff = tempDiff;
                        }
                        if (outputs[3] == 0.4f)
                        {
                            float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 3).GetValue() - 0.4f;
                            if (tempDiff > biggest4diff)
                                biggest4diff = tempDiff;
                        }
                        if (outputs[4] == 0.4f)
                        {
                            float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 4).GetValue() - 0.4f;
                            if (tempDiff > biggest4diff)
                                biggest4diff = tempDiff;
                        }
                        if (outputs[5] == 0.4f)
                        {
                            float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 5).GetValue() - 0.4f;
                            if (tempDiff > biggest4diff)
                                biggest4diff = tempDiff;
                        }
                    }

                    inputs[152] = 1;

                    outputs = NN5_0Out(pieces0[x], pieces1[y], pieces2[z]);

                    network.RunInputs(inputs);
                    network.BackPropegate(outputs);

                    {
                        if (outputs[0] == 0.4f)
                        {
                            float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 0).GetValue() - 0.4f;
                            if (tempDiff > biggest4diff)
                                biggest4diff = tempDiff;
                        }
                        if (outputs[1] == 0.4f)
                        {
                            float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 1).GetValue() - 0.4f;
                            if (tempDiff > biggest4diff)
                                biggest4diff = tempDiff;
                        }
                        if (outputs[2] == 0.4f)
                        {
                            float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 2).GetValue() - 0.4f;
                            if (tempDiff > biggest4diff)
                                biggest4diff = tempDiff;
                        }
                        if (outputs[3] == 0.4f)
                        {
                            float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 3).GetValue() - 0.4f;
                            if (tempDiff > biggest4diff)
                                biggest4diff = tempDiff;
                        }
                        if (outputs[4] == 0.4f)
                        {
                            float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 4).GetValue() - 0.4f;
                            if (tempDiff > biggest4diff)
                                biggest4diff = tempDiff;
                        }
                        if (outputs[5] == 0.4f)
                        {
                            float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 5).GetValue() - 0.4f;
                            if (tempDiff > biggest4diff)
                                biggest4diff = tempDiff;
                        }
                    }
                    {
                        if (outputs[0] == 1)
                        {
                            float tempDiff = 1 - network.GetNeuron(network.GetSize().Length - 1, 0).GetValue();
                            if (tempDiff > biggest1diff)
                                biggest1diff = tempDiff;
                        }
                        if (outputs[1] == 1)
                        {
                            float tempDiff = 1 - network.GetNeuron(network.GetSize().Length - 1, 1).GetValue();
                            if (tempDiff > biggest1diff)
                                biggest1diff = tempDiff;
                        }
                        if (outputs[2] == 1)
                        {
                            float tempDiff = 1 - network.GetNeuron(network.GetSize().Length - 1, 2).GetValue();
                            if (tempDiff > biggest1diff)
                                biggest1diff = tempDiff;
                        }
                        if (outputs[3] == 1)
                        {
                            float tempDiff = 1 - network.GetNeuron(network.GetSize().Length - 1, 3).GetValue();
                            if (tempDiff > biggest1diff)
                                biggest1diff = tempDiff;
                        }
                        if (outputs[4] == 1)
                        {
                            float tempDiff = 1 - network.GetNeuron(network.GetSize().Length - 1, 4).GetValue();
                            if (tempDiff > biggest1diff)
                                biggest1diff = tempDiff;
                        }
                        if (outputs[5] == 1)
                        {
                            float tempDiff = 1 - network.GetNeuron(network.GetSize().Length - 1, 5).GetValue();
                            if (tempDiff > biggest1diff)
                                biggest1diff = tempDiff;
                        }
                    }

                    if (outputs[0] == 1 && network.GetOutput() == 0 ||
                                            outputs[1] == 1 && network.GetOutput() == 1 ||
                                            outputs[2] == 1 && network.GetOutput() == 2 ||
                                            outputs[3] == 1 && network.GetOutput() == 3 ||
                                            outputs[4] == 1 && network.GetOutput() == 4 ||
                                            outputs[5] == 1 && network.GetOutput() == 5)
                        rightChoices++;
                    else
                    {
                        if (outputs[0] == 1)
                            Debug.Log("CorrectChoice4-0|| actual choice-" + network.GetOutput() + ":::");
                        else if (outputs[1] == 1)
                            Debug.Log("CorrectChoice4-1|| actual choice-" + network.GetOutput() + ":::");
                        else if (outputs[2] == 1)
                            Debug.Log("CorrectChoice4-2|| actual choice-" + network.GetOutput() + ":::");
                        else if (outputs[3] == 1)
                            Debug.Log("CorrectChoice4-3|| actual choice-" + network.GetOutput() + ":::");
                        else if (outputs[4] == 1)
                            Debug.Log("CorrectChoice4-4|| actual choice-" + network.GetOutput() + ":::");
                        else if (outputs[5] == 1)
                            Debug.Log("CorrectChoice4-5|| actual choice-" + network.GetOutput() + ":::");
                    }
                }
                inputs[00 + pieces0[x]] = 0;
                inputs[21 + pieces1[y]] = 0;
                Debug.Log(z);
                Debug.Log(pieces2.Length);

                Debug.Log(21 + pieces2[z]);
                inputs[42 + pieces2[z]] = 0;
                inputs[152] = 0;
            }
        }

        Debug.Log("1.0:" + (1 - biggest1diff) + "|||0.4:" + (0.4 + biggest4diff));
        Debug.Log("out of 14 n5_0 correctly chose : " + rightChoices + "-----------------------------------");
        network.AddDesiredChanges(0.15f);
    }
    public void MakeGoodNN5_1()
    {
        nn5 = new NeuralNetwork();
        nn5.AddCollumn(0);
        nn5.AddCollumn(1);
        nn5.AddCollumn(2);
        int x;
        for (x = 0; x < 155; x++)//154 + bias node
        {
            nn5.AddNeuron(0, 0);
        }
        for (x = 0; x < 13; x++)
        {
            nn5.AddNeuron(1, 0);
        }
        for (x = 0; x < 6; x++)
        {
            nn5.AddNeuron(2, 0);
        }
        nn5.DeleteNeuron(1, 12);
        nn5.ConnectNeuronToCollumn(0, 154, 1);//biases
        nn5.ConnectNeuronToCollumn(0, 152, 1);//biases
        for (x = 0; x < 21; x++)
        {
            if (x == 0 || x == 5 || x == 7)
                nn5.ConnectNeuronToCollumn(0, x, 1);
            if (x == 3 || x == 8 || x == 14 || x == 19 || x == 10 || x == 17)
                nn5.ConnectNeuronToCollumn(0, 21 + x, 1);
            if (x == 0 || x == 11 || x == 16 || x == 20)
                nn5.ConnectNeuronToCollumn(0, 42 + x, 1);
        }
        nn5.AddNeuron(1, 12);
        nn5.ConnectCollumnToCollumn(1, 2);

    }

    public float[] NN6_0Out(int num0, int num1, int num2)
    {
        float[] output = new float[] { 0.4f, 0.4f, 0.4f, 0.4f, 0.4f, 0.4f };

        if (num0 == 12 || num0 == 13 || num0 == 14 || num1 == 12 || num1 == 13 || num1 == 14)
        {
            output[0] = 1;
        }
        if (num0 == 9 || num0 == 10 || num0 == 11 || num1 == 9 || num1 == 10 || num1 == 11)
        {
            output[2] = 1;
        }
        if (num2 == 15 || num2 == 16 || num2 == 17)
        {
            output[3] = 1;
        }
        if (num2 == 9 || num2 == 10 || num2 == 11)
        {
            output[4] = 1;
        }
        return output;
    }
    public void TrainNN6_0(NeuralNetwork network)
    {
        int rightChoices = 0;
        float[] inputs = new float[154];
        float[] outputs = new float[] { 0.4f, 0.4f, 0.4f, 0.4f, 0.4f, 0.4f };
        float biggest4diff = 0, biggest1diff = 0;
        int[] pieces0 = { 9, 10, 11, 12, 13, 14, 1 };
        int[] pieces1 = { 9, 10, 11, 12, 13, 14, 1 };
        int[] pieces2 = { 9, 10, 11, 15, 16, 17, 1 };
        int x;
        for (x = 0; x < 154; x++)
        {
            inputs[x] = 0;
        }

        for (x = 0; x < 7; x++)
        {
            inputs[pieces0[x]] = 1;// set the right one to one
            network.RunInputs(inputs);
            network.BackPropegate(outputs);
            {
                if (outputs[0] == 0.4f)
                {
                    float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 0).GetValue() - 0.4f;
                    if (tempDiff > biggest4diff)
                        biggest4diff = tempDiff;
                }
                if (outputs[1] == 0.4f)
                {
                    float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 1).GetValue() - 0.4f;
                    if (tempDiff > biggest4diff)
                        biggest4diff = tempDiff;
                }
                if (outputs[2] == 0.4f)
                {
                    float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 2).GetValue() - 0.4f;
                    if (tempDiff > biggest4diff)
                        biggest4diff = tempDiff;
                }
                if (outputs[3] == 0.4f)
                {
                    float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 3).GetValue() - 0.4f;
                    if (tempDiff > biggest4diff)
                        biggest4diff = tempDiff;
                }
                if (outputs[4] == 0.4f)
                {
                    float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 4).GetValue() - 0.4f;
                    if (tempDiff > biggest4diff)
                        biggest4diff = tempDiff;
                }
                if (outputs[5] == 0.4f)
                {
                    float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 5).GetValue() - 0.4f;
                    if (tempDiff > biggest4diff)
                        biggest4diff = tempDiff;
                }
            }
            inputs[153] = 1; // set the cp on
            outputs = NN6_0Out(pieces0[x], 0, 0);//all with the checkpoint
            network.RunInputs(inputs);
            network.BackPropegate(outputs);

            {
                if (outputs[0] == 0.4f)
                {
                    float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 0).GetValue() - 0.4f;
                    if (tempDiff > biggest4diff)
                        biggest4diff = tempDiff;
                }
                if (outputs[1] == 0.4f)
                {
                    float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 1).GetValue() - 0.4f;
                    if (tempDiff > biggest4diff)
                        biggest4diff = tempDiff;
                }
                if (outputs[2] == 0.4f)
                {
                    float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 2).GetValue() - 0.4f;
                    if (tempDiff > biggest4diff)
                        biggest4diff = tempDiff;
                }
                if (outputs[3] == 0.4f)
                {
                    float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 3).GetValue() - 0.4f;
                    if (tempDiff > biggest4diff)
                        biggest4diff = tempDiff;
                }
                if (outputs[4] == 0.4f)
                {
                    float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 4).GetValue() - 0.4f;
                    if (tempDiff > biggest4diff)
                        biggest4diff = tempDiff;
                }
                if (outputs[5] == 0.4f)
                {
                    float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 5).GetValue() - 0.4f;
                    if (tempDiff > biggest4diff)
                        biggest4diff = tempDiff;
                }
                if (outputs[0] == 1)
                {
                    float tempDiff = 1 - network.GetNeuron(network.GetSize().Length - 1, 0).GetValue();
                    if (tempDiff > biggest1diff)
                        biggest1diff = tempDiff;
                }
                if (outputs[1] == 1)
                {
                    float tempDiff = 1 - network.GetNeuron(network.GetSize().Length - 1, 1).GetValue();
                    if (tempDiff > biggest1diff)
                        biggest1diff = tempDiff;
                }
                if (outputs[2] == 1)
                {
                    float tempDiff = 1 - network.GetNeuron(network.GetSize().Length - 1, 2).GetValue();
                    if (tempDiff > biggest1diff)
                        biggest1diff = tempDiff;
                }
                if (outputs[3] == 1)
                {
                    float tempDiff = 1 - network.GetNeuron(network.GetSize().Length - 1, 3).GetValue();
                    if (tempDiff > biggest1diff)
                        biggest1diff = tempDiff;
                }
                if (outputs[4] == 1)
                {
                    float tempDiff = 1 - network.GetNeuron(network.GetSize().Length - 1, 4).GetValue();
                    if (tempDiff > biggest1diff)
                        biggest1diff = tempDiff;
                }
                if (outputs[5] == 1)
                {
                    float tempDiff = 1 - network.GetNeuron(network.GetSize().Length - 1, 5).GetValue();
                    if (tempDiff > biggest1diff)
                        biggest1diff = tempDiff;
                }
            }
            //correctly chosen
            if (outputs[0] == 1 && network.GetOutput() == 0 ||
                outputs[1] == 1 && network.GetOutput() == 1 ||
                outputs[2] == 1 && network.GetOutput() == 2 ||
                outputs[3] == 1 && network.GetOutput() == 3 ||
                outputs[4] == 1 && network.GetOutput() == 4 ||
                outputs[5] == 1 && network.GetOutput() == 5)
                rightChoices++;
            else
            {
                if (outputs[0] == 1)
                    Debug.Log("CorrectChoice-0|| actual choice-" + network.GetOutput());
                if (outputs[1] == 1)
                    Debug.Log("CorrectChoice-1|| actual choice-" + network.GetOutput());
                if (outputs[2] == 1)
                    Debug.Log("CorrectChoice-2|| actual choice-" + network.GetOutput());
                if (outputs[3] == 1)
                    Debug.Log("CorrectChoice-3|| actual choice-" + network.GetOutput());
                if (outputs[4] == 1)
                    Debug.Log("CorrectChoice-4|| actual choice-" + network.GetOutput());
                if (outputs[5] == 1)
                    Debug.Log("CorrectChoice-5|| actual choice-" + network.GetOutput());
            }

            inputs[153] = 0; //reset
            outputs = new float[] { 0.4f, 0.4f, 0.4f, 0.4f, 0.4f, 0.4f };
            inputs[pieces0[x]] = 0;
        }
        for (x = 0; x < 0; x++)
        {
            inputs[21 + pieces1[x]] = 1;// set the right one to one
            network.RunInputs(inputs);
            network.BackPropegate(outputs);
            {
                if (outputs[0] == 0.4f)
                {
                    float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 0).GetValue() - 0.4f;
                    if (tempDiff > biggest4diff)
                        biggest4diff = tempDiff;
                }
                if (outputs[1] == 0.4f)
                {
                    float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 1).GetValue() - 0.4f;
                    if (tempDiff > biggest4diff)
                        biggest4diff = tempDiff;
                }
                if (outputs[2] == 0.4f)
                {
                    float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 2).GetValue() - 0.4f;
                    if (tempDiff > biggest4diff)
                        biggest4diff = tempDiff;
                }
                if (outputs[3] == 0.4f)
                {
                    float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 3).GetValue() - 0.4f;
                    if (tempDiff > biggest4diff)
                        biggest4diff = tempDiff;
                }
                if (outputs[4] == 0.4f)
                {
                    float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 4).GetValue() - 0.4f;
                    if (tempDiff > biggest4diff)
                        biggest4diff = tempDiff;
                }
                if (outputs[5] == 0.4f)
                {
                    float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 5).GetValue() - 0.4f;
                    if (tempDiff > biggest4diff)
                        biggest4diff = tempDiff;
                }
            }
            inputs[153] = 1; // set the cp on
            outputs = NN6_0Out(0, 21 + pieces1[x], 0);//all with the checkpoint
            network.RunInputs(inputs);
            network.BackPropegate(outputs);

            {
                if (outputs[0] == 0.4f)
                {
                    float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 0).GetValue() - 0.4f;
                    if (tempDiff > biggest4diff)
                        biggest4diff = tempDiff;
                }
                if (outputs[1] == 0.4f)
                {
                    float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 1).GetValue() - 0.4f;
                    if (tempDiff > biggest4diff)
                        biggest4diff = tempDiff;
                }
                if (outputs[2] == 0.4f)
                {
                    float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 2).GetValue() - 0.4f;
                    if (tempDiff > biggest4diff)
                        biggest4diff = tempDiff;
                }
                if (outputs[3] == 0.4f)
                {
                    float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 3).GetValue() - 0.4f;
                    if (tempDiff > biggest4diff)
                        biggest4diff = tempDiff;
                }
                if (outputs[4] == 0.4f)
                {
                    float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 4).GetValue() - 0.4f;
                    if (tempDiff > biggest4diff)
                        biggest4diff = tempDiff;
                }
                if (outputs[5] == 0.4f)
                {
                    float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 5).GetValue() - 0.4f;
                    if (tempDiff > biggest4diff)
                        biggest4diff = tempDiff;
                }
                if (outputs[0] == 1)
                {
                    float tempDiff = 1 - network.GetNeuron(network.GetSize().Length - 1, 0).GetValue();
                    if (tempDiff > biggest1diff)
                        biggest1diff = tempDiff;
                }
                if (outputs[1] == 1)
                {
                    float tempDiff = 1 - network.GetNeuron(network.GetSize().Length - 1, 1).GetValue();
                    if (tempDiff > biggest1diff)
                        biggest1diff = tempDiff;
                }
                if (outputs[2] == 1)
                {
                    float tempDiff = 1 - network.GetNeuron(network.GetSize().Length - 1, 2).GetValue();
                    if (tempDiff > biggest1diff)
                        biggest1diff = tempDiff;
                }
                if (outputs[3] == 1)
                {
                    float tempDiff = 1 - network.GetNeuron(network.GetSize().Length - 1, 3).GetValue();
                    if (tempDiff > biggest1diff)
                        biggest1diff = tempDiff;
                }
                if (outputs[4] == 1)
                {
                    float tempDiff = 1 - network.GetNeuron(network.GetSize().Length - 1, 4).GetValue();
                    if (tempDiff > biggest1diff)
                        biggest1diff = tempDiff;
                }
                if (outputs[5] == 1)
                {
                    float tempDiff = 1 - network.GetNeuron(network.GetSize().Length - 1, 5).GetValue();
                    if (tempDiff > biggest1diff)
                        biggest1diff = tempDiff;
                }
            }
            //correctly chosen
            if (outputs[0] == 1 && network.GetOutput() == 0 ||
                outputs[1] == 1 && network.GetOutput() == 1 ||
                outputs[2] == 1 && network.GetOutput() == 2 ||
                outputs[3] == 1 && network.GetOutput() == 3 ||
                outputs[4] == 1 && network.GetOutput() == 4 ||
                outputs[5] == 1 && network.GetOutput() == 5)
                rightChoices++;
            else
            {
                if (outputs[0] == 1)
                    Debug.Log("CorrectChoice-0|| actual choice-" + network.GetOutput());
                if (outputs[1] == 1)
                    Debug.Log("CorrectChoice-1|| actual choice-" + network.GetOutput());
                if (outputs[2] == 1)
                    Debug.Log("CorrectChoice-2|| actual choice-" + network.GetOutput());
                if (outputs[3] == 1)
                    Debug.Log("CorrectChoice-3|| actual choice-" + network.GetOutput());
                if (outputs[4] == 1)
                    Debug.Log("CorrectChoice-4|| actual choice-" + network.GetOutput());
                if (outputs[5] == 1)
                    Debug.Log("CorrectChoice-5|| actual choice-" + network.GetOutput());
            }

            inputs[153] = 0; //reset
            outputs = new float[] { 0.4f, 0.4f, 0.4f, 0.4f, 0.4f, 0.4f };
            inputs[21 + pieces1[x]] = 0;
        }
        for (x = 0; x < 0; x++)

        {
            inputs[42 + pieces2[x]] = 1;// set the right one to one
            network.RunInputs(inputs);
            network.BackPropegate(outputs);
            {
                if (outputs[0] == 0.4f)
                {
                    float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 0).GetValue() - 0.4f;
                    if (tempDiff > biggest4diff)
                        biggest4diff = tempDiff;
                }
                if (outputs[1] == 0.4f)
                {
                    float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 1).GetValue() - 0.4f;
                    if (tempDiff > biggest4diff)
                        biggest4diff = tempDiff;
                }
                if (outputs[2] == 0.4f)
                {
                    float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 2).GetValue() - 0.4f;
                    if (tempDiff > biggest4diff)
                        biggest4diff = tempDiff;
                }
                if (outputs[3] == 0.4f)
                {
                    float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 3).GetValue() - 0.4f;
                    if (tempDiff > biggest4diff)
                        biggest4diff = tempDiff;
                }
                if (outputs[4] == 0.4f)
                {
                    float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 4).GetValue() - 0.4f;
                    if (tempDiff > biggest4diff)
                        biggest4diff = tempDiff;
                }
                if (outputs[5] == 0.4f)
                {
                    float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 5).GetValue() - 0.4f;
                    if (tempDiff > biggest4diff)
                        biggest4diff = tempDiff;
                }
            }
            inputs[153] = 1; // set the cp on
            outputs = NN6_0Out(0, 0, 42 + pieces2[x]);//all with the checkpoint
            network.RunInputs(inputs);
            network.BackPropegate(outputs);

            {
                if (outputs[0] == 0.4f)
                {
                    float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 0).GetValue() - 0.4f;
                    if (tempDiff > biggest4diff)
                        biggest4diff = tempDiff;
                }
                if (outputs[1] == 0.4f)
                {
                    float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 1).GetValue() - 0.4f;
                    if (tempDiff > biggest4diff)
                        biggest4diff = tempDiff;
                }
                if (outputs[2] == 0.4f)
                {
                    float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 2).GetValue() - 0.4f;
                    if (tempDiff > biggest4diff)
                        biggest4diff = tempDiff;
                }
                if (outputs[3] == 0.4f)
                {
                    float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 3).GetValue() - 0.4f;
                    if (tempDiff > biggest4diff)
                        biggest4diff = tempDiff;
                }
                if (outputs[4] == 0.4f)
                {
                    float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 4).GetValue() - 0.4f;
                    if (tempDiff > biggest4diff)
                        biggest4diff = tempDiff;
                }
                if (outputs[5] == 0.4f)
                {
                    float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 5).GetValue() - 0.4f;
                    if (tempDiff > biggest4diff)
                        biggest4diff = tempDiff;
                }
                if (outputs[0] == 1)
                {
                    float tempDiff = 1 - network.GetNeuron(network.GetSize().Length - 1, 0).GetValue();
                    if (tempDiff > biggest1diff)
                        biggest1diff = tempDiff;
                }
                if (outputs[1] == 1)
                {
                    float tempDiff = 1 - network.GetNeuron(network.GetSize().Length - 1, 1).GetValue();
                    if (tempDiff > biggest1diff)
                        biggest1diff = tempDiff;
                }
                if (outputs[2] == 1)
                {
                    float tempDiff = 1 - network.GetNeuron(network.GetSize().Length - 1, 2).GetValue();
                    if (tempDiff > biggest1diff)
                        biggest1diff = tempDiff;
                }
                if (outputs[3] == 1)
                {
                    float tempDiff = 1 - network.GetNeuron(network.GetSize().Length - 1, 3).GetValue();
                    if (tempDiff > biggest1diff)
                        biggest1diff = tempDiff;
                }
                if (outputs[4] == 1)
                {
                    float tempDiff = 1 - network.GetNeuron(network.GetSize().Length - 1, 4).GetValue();
                    if (tempDiff > biggest1diff)
                        biggest1diff = tempDiff;
                }
                if (outputs[5] == 1)
                {
                    float tempDiff = 1 - network.GetNeuron(network.GetSize().Length - 1, 5).GetValue();
                    if (tempDiff > biggest1diff)
                        biggest1diff = tempDiff;
                }
            }
            //correctly chosen
            if (outputs[0] == 1 && network.GetOutput() == 0 ||
                outputs[1] == 1 && network.GetOutput() == 1 ||
                outputs[2] == 1 && network.GetOutput() == 2 ||
                outputs[3] == 1 && network.GetOutput() == 3 ||
                outputs[4] == 1 && network.GetOutput() == 4 ||
                outputs[5] == 1 && network.GetOutput() == 5)
                rightChoices++;
            else
            {
                if (outputs[0] == 1)
                    Debug.Log("CorrectChoice-0|| actual choice-" + network.GetOutput());
                if (outputs[1] == 1)
                    Debug.Log("CorrectChoice-1|| actual choice-" + network.GetOutput());
                if (outputs[2] == 1)
                    Debug.Log("CorrectChoice-2|| actual choice-" + network.GetOutput());
                if (outputs[3] == 1)
                    Debug.Log("CorrectChoice-3|| actual choice-" + network.GetOutput());
                if (outputs[4] == 1)
                    Debug.Log("CorrectChoice-4|| actual choice-" + network.GetOutput());
                if (outputs[5] == 1)
                    Debug.Log("CorrectChoice-5|| actual choice-" + network.GetOutput());
            }

            inputs[153] = 0; //reset
            outputs = new float[] { 0.4f, 0.4f, 0.4f, 0.4f, 0.4f, 0.4f };
            inputs[42 + pieces2[x]] = 0;
        }

        Debug.Log("1.0:" + (1 - biggest1diff) + "|||0.4:" + (0.4 + biggest4diff));
        Debug.Log("out of 18 n6_0 correctly chose : " + rightChoices);
        network.AddDesiredChanges(0.15f);
    }
    public void MakeGoodNN6_0()
    {
        nn6_0 = new NeuralNetwork();
        nn6_0.AddCollumn(0);
        nn6_0.AddCollumn(1);
        nn6_0.AddCollumn(2);
        int x;
        for (x = 0; x < 155; x++)//154 + bias node
        {
            nn6_0.AddNeuron(0, 0);
        }
        for (x = 0; x < 13; x++)
        {
            nn6_0.AddNeuron(1, 0);
        }
        for (x = 0; x < 6; x++)
        {
            nn6_0.AddNeuron(2, 0);
        }
        nn6_0.DeleteNeuron(1, 12);
        nn6_0.ConnectNeuronToCollumn(0, 154, 1);//biases
        nn6_0.ConnectNeuronToCollumn(0, 153, 1);//biases
        for (x = 0; x < 21; x++)
        {
            if (x == 9 || x == 10 || x == 11 || x == 12 || x == 13 || x == 14)
                nn6_0.ConnectNeuronToCollumn(0, 0 + x, 1);
            if (x == 9 || x == 10 || x == 11 || x == 12 || x == 13 || x == 14)
                nn6_0.ConnectNeuronToCollumn(0, 21 + x, 1);
            if (x == 9 || x == 10 || x == 11 || x == 15 || x == 16 || x == 17)
                nn6_0.ConnectNeuronToCollumn(0, 42 + x, 1);
        }
        nn6_0.AddNeuron(1, 12);
        nn6_0.ConnectCollumnToCollumn(1, 2);
    }

    public float[] NN6_1Out(int num0, int num1, int num2)
    {
        float[] output = new float[] { 0.4f, 0.4f, 0.4f, 0.4f, 0.4f, 0.4f };

        if (num0 == 12 || num0 == 13 || num0 == 14 || num1 == 12 || num1 == 13 || num1 == 14)
        {
            output[0] = 1;
        }
        if (num0 == 9 || num0 == 10 || num0 == 11 || num1 == 9 || num1 == 10 || num1 == 11)
        {
            output[2] = 1;
        }
        if (num2 == 15 || num2 == 16 || num2 == 17)
        {
            output[3] = 1;
        }
        if (num2 == 9 || num2 == 10 || num2 == 11)
        {
            output[4] = 1;
        }
        return output;
    }
    public void TrainNN6_1(NeuralNetwork network)
    {
        int rightChoices = 0;
        float[] inputs = new float[154];
        float[] outputs = new float[] { 0.4f, 0.4f, 0.4f, 0.4f, 0.4f, 0.4f };
        float biggest4diff = 0, biggest1diff = 0;
        int[] pieces0 = { 9, 10, 11, 12, 13, 14, 1 };
        int[] pieces1 = { 9, 10, 11, 12, 13, 14, 1 };
        int[] pieces2 = { 9, 10, 11, 15, 16, 17, 1 };
        int x;
        for (x = 0; x < 154; x++)
        {
            inputs[x] = 0;
        }

        for (x = 0; x < 0; x++)
        {
            inputs[pieces0[x]] = 1;// set the right one to one
            network.RunInputs(inputs);
            network.BackPropegate(outputs);
            {
                if (outputs[0] == 0.4f)
                {
                    float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 0).GetValue() - 0.4f;
                    if (tempDiff > biggest4diff)
                        biggest4diff = tempDiff;
                }
                if (outputs[1] == 0.4f)
                {
                    float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 1).GetValue() - 0.4f;
                    if (tempDiff > biggest4diff)
                        biggest4diff = tempDiff;
                }
                if (outputs[2] == 0.4f)
                {
                    float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 2).GetValue() - 0.4f;
                    if (tempDiff > biggest4diff)
                        biggest4diff = tempDiff;
                }
                if (outputs[3] == 0.4f)
                {
                    float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 3).GetValue() - 0.4f;
                    if (tempDiff > biggest4diff)
                        biggest4diff = tempDiff;
                }
                if (outputs[4] == 0.4f)
                {
                    float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 4).GetValue() - 0.4f;
                    if (tempDiff > biggest4diff)
                        biggest4diff = tempDiff;
                }
                if (outputs[5] == 0.4f)
                {
                    float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 5).GetValue() - 0.4f;
                    if (tempDiff > biggest4diff)
                        biggest4diff = tempDiff;
                }
            }
            inputs[153] = 1; // set the cp on
            outputs = NN6_1Out(pieces0[x], 0, 0);//all with the checkpoint
            network.RunInputs(inputs);
            network.BackPropegate(outputs);

            {
                if (outputs[0] == 0.4f)
                {
                    float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 0).GetValue() - 0.4f;
                    if (tempDiff > biggest4diff)
                        biggest4diff = tempDiff;
                }
                if (outputs[1] == 0.4f)
                {
                    float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 1).GetValue() - 0.4f;
                    if (tempDiff > biggest4diff)
                        biggest4diff = tempDiff;
                }
                if (outputs[2] == 0.4f)
                {
                    float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 2).GetValue() - 0.4f;
                    if (tempDiff > biggest4diff)
                        biggest4diff = tempDiff;
                }
                if (outputs[3] == 0.4f)
                {
                    float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 3).GetValue() - 0.4f;
                    if (tempDiff > biggest4diff)
                        biggest4diff = tempDiff;
                }
                if (outputs[4] == 0.4f)
                {
                    float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 4).GetValue() - 0.4f;
                    if (tempDiff > biggest4diff)
                        biggest4diff = tempDiff;
                }
                if (outputs[5] == 0.4f)
                {
                    float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 5).GetValue() - 0.4f;
                    if (tempDiff > biggest4diff)
                        biggest4diff = tempDiff;
                }
                if (outputs[0] == 1)
                {
                    float tempDiff = 1 - network.GetNeuron(network.GetSize().Length - 1, 0).GetValue();
                    if (tempDiff > biggest1diff)
                        biggest1diff = tempDiff;
                }
                if (outputs[1] == 1)
                {
                    float tempDiff = 1 - network.GetNeuron(network.GetSize().Length - 1, 1).GetValue();
                    if (tempDiff > biggest1diff)
                        biggest1diff = tempDiff;
                }
                if (outputs[2] == 1)
                {
                    float tempDiff = 1 - network.GetNeuron(network.GetSize().Length - 1, 2).GetValue();
                    if (tempDiff > biggest1diff)
                        biggest1diff = tempDiff;
                }
                if (outputs[3] == 1)
                {
                    float tempDiff = 1 - network.GetNeuron(network.GetSize().Length - 1, 3).GetValue();
                    if (tempDiff > biggest1diff)
                        biggest1diff = tempDiff;
                }
                if (outputs[4] == 1)
                {
                    float tempDiff = 1 - network.GetNeuron(network.GetSize().Length - 1, 4).GetValue();
                    if (tempDiff > biggest1diff)
                        biggest1diff = tempDiff;
                }
                if (outputs[5] == 1)
                {
                    float tempDiff = 1 - network.GetNeuron(network.GetSize().Length - 1, 5).GetValue();
                    if (tempDiff > biggest1diff)
                        biggest1diff = tempDiff;
                }
            }
            //correctly chosen
            if (outputs[0] == 1 && network.GetOutput() == 0 ||
                outputs[1] == 1 && network.GetOutput() == 1 ||
                outputs[2] == 1 && network.GetOutput() == 2 ||
                outputs[3] == 1 && network.GetOutput() == 3 ||
                outputs[4] == 1 && network.GetOutput() == 4 ||
                outputs[5] == 1 && network.GetOutput() == 5)
                rightChoices++;
            else
            {
                if (outputs[0] == 1)
                    Debug.Log("CorrectChoice-0|| actual choice-" + network.GetOutput());
                if (outputs[1] == 1)
                    Debug.Log("CorrectChoice-1|| actual choice-" + network.GetOutput());
                if (outputs[2] == 1)
                    Debug.Log("CorrectChoice-2|| actual choice-" + network.GetOutput());
                if (outputs[3] == 1)
                    Debug.Log("CorrectChoice-3|| actual choice-" + network.GetOutput());
                if (outputs[4] == 1)
                    Debug.Log("CorrectChoice-4|| actual choice-" + network.GetOutput());
                if (outputs[5] == 1)
                    Debug.Log("CorrectChoice-5|| actual choice-" + network.GetOutput());
            }

            inputs[153] = 0; //reset
            outputs = new float[] { 0.4f, 0.4f, 0.4f, 0.4f, 0.4f, 0.4f };
            inputs[pieces0[x]] = 0;
        }
        for (x = 0; x < 7; x++)
        {
            inputs[21 + pieces1[x]] = 1;// set the right one to one
            network.RunInputs(inputs);
            network.BackPropegate(outputs);
            {
                if (outputs[0] == 0.4f)
                {
                    float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 0).GetValue() - 0.4f;
                    if (tempDiff > biggest4diff)
                        biggest4diff = tempDiff;
                }
                if (outputs[1] == 0.4f)
                {
                    float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 1).GetValue() - 0.4f;
                    if (tempDiff > biggest4diff)
                        biggest4diff = tempDiff;
                }
                if (outputs[2] == 0.4f)
                {
                    float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 2).GetValue() - 0.4f;
                    if (tempDiff > biggest4diff)
                        biggest4diff = tempDiff;
                }
                if (outputs[3] == 0.4f)
                {
                    float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 3).GetValue() - 0.4f;
                    if (tempDiff > biggest4diff)
                        biggest4diff = tempDiff;
                }
                if (outputs[4] == 0.4f)
                {
                    float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 4).GetValue() - 0.4f;
                    if (tempDiff > biggest4diff)
                        biggest4diff = tempDiff;
                }
                if (outputs[5] == 0.4f)
                {
                    float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 5).GetValue() - 0.4f;
                    if (tempDiff > biggest4diff)
                        biggest4diff = tempDiff;
                }
            }
            inputs[153] = 1; // set the cp on
            outputs = NN6_1Out(0, pieces1[x], 0);//all with the checkpoint
            network.RunInputs(inputs);
            network.BackPropegate(outputs);

            {
                if (outputs[0] == 0.4f)
                {
                    float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 0).GetValue() - 0.4f;
                    if (tempDiff > biggest4diff)
                        biggest4diff = tempDiff;
                }
                if (outputs[1] == 0.4f)
                {
                    float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 1).GetValue() - 0.4f;
                    if (tempDiff > biggest4diff)
                        biggest4diff = tempDiff;
                }
                if (outputs[2] == 0.4f)
                {
                    float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 2).GetValue() - 0.4f;
                    if (tempDiff > biggest4diff)
                        biggest4diff = tempDiff;
                }
                if (outputs[3] == 0.4f)
                {
                    float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 3).GetValue() - 0.4f;
                    if (tempDiff > biggest4diff)
                        biggest4diff = tempDiff;
                }
                if (outputs[4] == 0.4f)
                {
                    float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 4).GetValue() - 0.4f;
                    if (tempDiff > biggest4diff)
                        biggest4diff = tempDiff;
                }
                if (outputs[5] == 0.4f)
                {
                    float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 5).GetValue() - 0.4f;
                    if (tempDiff > biggest4diff)
                        biggest4diff = tempDiff;
                }
                if (outputs[0] == 1)
                {
                    float tempDiff = 1 - network.GetNeuron(network.GetSize().Length - 1, 0).GetValue();
                    if (tempDiff > biggest1diff)
                        biggest1diff = tempDiff;
                }
                if (outputs[1] == 1)
                {
                    float tempDiff = 1 - network.GetNeuron(network.GetSize().Length - 1, 1).GetValue();
                    if (tempDiff > biggest1diff)
                        biggest1diff = tempDiff;
                }
                if (outputs[2] == 1)
                {
                    float tempDiff = 1 - network.GetNeuron(network.GetSize().Length - 1, 2).GetValue();
                    if (tempDiff > biggest1diff)
                        biggest1diff = tempDiff;
                }
                if (outputs[3] == 1)
                {
                    float tempDiff = 1 - network.GetNeuron(network.GetSize().Length - 1, 3).GetValue();
                    if (tempDiff > biggest1diff)
                        biggest1diff = tempDiff;
                }
                if (outputs[4] == 1)
                {
                    float tempDiff = 1 - network.GetNeuron(network.GetSize().Length - 1, 4).GetValue();
                    if (tempDiff > biggest1diff)
                        biggest1diff = tempDiff;
                }
                if (outputs[5] == 1)
                {
                    float tempDiff = 1 - network.GetNeuron(network.GetSize().Length - 1, 5).GetValue();
                    if (tempDiff > biggest1diff)
                        biggest1diff = tempDiff;
                }
            }
            //correctly chosen
            if (outputs[0] == 1 && network.GetOutput() == 0 ||
                outputs[1] == 1 && network.GetOutput() == 1 ||
                outputs[2] == 1 && network.GetOutput() == 2 ||
                outputs[3] == 1 && network.GetOutput() == 3 ||
                outputs[4] == 1 && network.GetOutput() == 4 ||
                outputs[5] == 1 && network.GetOutput() == 5)
                rightChoices++;
            else
            {
                if (outputs[0] == 1)
                    Debug.Log("CorrectChoice-0|| actual choice-" + network.GetOutput());
                if (outputs[1] == 1)
                    Debug.Log("CorrectChoice-1|| actual choice-" + network.GetOutput());
                if (outputs[2] == 1)
                    Debug.Log("CorrectChoice-2|| actual choice-" + network.GetOutput());
                if (outputs[3] == 1)
                    Debug.Log("CorrectChoice-3|| actual choice-" + network.GetOutput());
                if (outputs[4] == 1)
                    Debug.Log("CorrectChoice-4|| actual choice-" + network.GetOutput());
                if (outputs[5] == 1)
                    Debug.Log("CorrectChoice-5|| actual choice-" + network.GetOutput());
            }

            inputs[153] = 0; //reset
            outputs = new float[] { 0.4f, 0.4f, 0.4f, 0.4f, 0.4f, 0.4f };
            inputs[21 + pieces1[x]] = 0;
        }
        for (x = 0; x < 0; x++)

        {
            inputs[42 + pieces2[x]] = 1;// set the right one to one
            network.RunInputs(inputs);
            network.BackPropegate(outputs);
            {
                if (outputs[0] == 0.4f)
                {
                    float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 0).GetValue() - 0.4f;
                    if (tempDiff > biggest4diff)
                        biggest4diff = tempDiff;
                }
                if (outputs[1] == 0.4f)
                {
                    float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 1).GetValue() - 0.4f;
                    if (tempDiff > biggest4diff)
                        biggest4diff = tempDiff;
                }
                if (outputs[2] == 0.4f)
                {
                    float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 2).GetValue() - 0.4f;
                    if (tempDiff > biggest4diff)
                        biggest4diff = tempDiff;
                }
                if (outputs[3] == 0.4f)
                {
                    float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 3).GetValue() - 0.4f;
                    if (tempDiff > biggest4diff)
                        biggest4diff = tempDiff;
                }
                if (outputs[4] == 0.4f)
                {
                    float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 4).GetValue() - 0.4f;
                    if (tempDiff > biggest4diff)
                        biggest4diff = tempDiff;
                }
                if (outputs[5] == 0.4f)
                {
                    float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 5).GetValue() - 0.4f;
                    if (tempDiff > biggest4diff)
                        biggest4diff = tempDiff;
                }
            }
            inputs[153] = 1; // set the cp on
            outputs = NN6_0Out(0, 0, 42 + pieces2[x]);//all with the checkpoint
            network.RunInputs(inputs);
            network.BackPropegate(outputs);

            {
                if (outputs[0] == 0.4f)
                {
                    float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 0).GetValue() - 0.4f;
                    if (tempDiff > biggest4diff)
                        biggest4diff = tempDiff;
                }
                if (outputs[1] == 0.4f)
                {
                    float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 1).GetValue() - 0.4f;
                    if (tempDiff > biggest4diff)
                        biggest4diff = tempDiff;
                }
                if (outputs[2] == 0.4f)
                {
                    float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 2).GetValue() - 0.4f;
                    if (tempDiff > biggest4diff)
                        biggest4diff = tempDiff;
                }
                if (outputs[3] == 0.4f)
                {
                    float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 3).GetValue() - 0.4f;
                    if (tempDiff > biggest4diff)
                        biggest4diff = tempDiff;
                }
                if (outputs[4] == 0.4f)
                {
                    float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 4).GetValue() - 0.4f;
                    if (tempDiff > biggest4diff)
                        biggest4diff = tempDiff;
                }
                if (outputs[5] == 0.4f)
                {
                    float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 5).GetValue() - 0.4f;
                    if (tempDiff > biggest4diff)
                        biggest4diff = tempDiff;
                }
                if (outputs[0] == 1)
                {
                    float tempDiff = 1 - network.GetNeuron(network.GetSize().Length - 1, 0).GetValue();
                    if (tempDiff > biggest1diff)
                        biggest1diff = tempDiff;
                }
                if (outputs[1] == 1)
                {
                    float tempDiff = 1 - network.GetNeuron(network.GetSize().Length - 1, 1).GetValue();
                    if (tempDiff > biggest1diff)
                        biggest1diff = tempDiff;
                }
                if (outputs[2] == 1)
                {
                    float tempDiff = 1 - network.GetNeuron(network.GetSize().Length - 1, 2).GetValue();
                    if (tempDiff > biggest1diff)
                        biggest1diff = tempDiff;
                }
                if (outputs[3] == 1)
                {
                    float tempDiff = 1 - network.GetNeuron(network.GetSize().Length - 1, 3).GetValue();
                    if (tempDiff > biggest1diff)
                        biggest1diff = tempDiff;
                }
                if (outputs[4] == 1)
                {
                    float tempDiff = 1 - network.GetNeuron(network.GetSize().Length - 1, 4).GetValue();
                    if (tempDiff > biggest1diff)
                        biggest1diff = tempDiff;
                }
                if (outputs[5] == 1)
                {
                    float tempDiff = 1 - network.GetNeuron(network.GetSize().Length - 1, 5).GetValue();
                    if (tempDiff > biggest1diff)
                        biggest1diff = tempDiff;
                }
            }
            //correctly chosen
            if (outputs[0] == 1 && network.GetOutput() == 0 ||
                outputs[1] == 1 && network.GetOutput() == 1 ||
                outputs[2] == 1 && network.GetOutput() == 2 ||
                outputs[3] == 1 && network.GetOutput() == 3 ||
                outputs[4] == 1 && network.GetOutput() == 4 ||
                outputs[5] == 1 && network.GetOutput() == 5)
                rightChoices++;
            else
            {
                if (outputs[0] == 1)
                    Debug.Log("CorrectChoice-0|| actual choice-" + network.GetOutput());
                if (outputs[1] == 1)
                    Debug.Log("CorrectChoice-1|| actual choice-" + network.GetOutput());
                if (outputs[2] == 1)
                    Debug.Log("CorrectChoice-2|| actual choice-" + network.GetOutput());
                if (outputs[3] == 1)
                    Debug.Log("CorrectChoice-3|| actual choice-" + network.GetOutput());
                if (outputs[4] == 1)
                    Debug.Log("CorrectChoice-4|| actual choice-" + network.GetOutput());
                if (outputs[5] == 1)
                    Debug.Log("CorrectChoice-5|| actual choice-" + network.GetOutput());
            }

            inputs[153] = 0; //reset
            outputs = new float[] { 0.4f, 0.4f, 0.4f, 0.4f, 0.4f, 0.4f };
            inputs[42 + pieces2[x]] = 0;
        }

        Debug.Log("1.0:" + (1 - biggest1diff) + "|||0.4:" + (0.4 + biggest4diff));
        Debug.Log("out of 18 n6_1 correctly chose : " + rightChoices);
        network.AddDesiredChanges(0.15f);
    }
    public void MakeGoodNN6_1()
    {
        nn6_1 = new NeuralNetwork();
        nn6_1.AddCollumn(0);
        nn6_1.AddCollumn(1);
        nn6_1.AddCollumn(2);
        int x;
        for (x = 0; x < 155; x++)//154 + bias node
        {
            nn6_1.AddNeuron(0, 0);
        }
        for (x = 0; x < 13; x++)
        {
            nn6_1.AddNeuron(1, 0);
        }
        for (x = 0; x < 6; x++)
        {
            nn6_1.AddNeuron(2, 0);
        }
        nn6_1.DeleteNeuron(1, 12);
        nn6_1.ConnectNeuronToCollumn(0, 154, 1);//biases
        nn6_1.ConnectNeuronToCollumn(0, 153, 1);//biases
        for (x = 0; x < 21; x++)
        {
            if (x == 9 || x == 10 || x == 11 || x == 12 || x == 13 || x == 14)
                nn6_1.ConnectNeuronToCollumn(0, 0 + x, 1);
            if (x == 9 || x == 10 || x == 11 || x == 12 || x == 13 || x == 14)
                nn6_1.ConnectNeuronToCollumn(0, 21 + x, 1);
            if (x == 9 || x == 10 || x == 11 || x == 15 || x == 16 || x == 17)
                nn6_1.ConnectNeuronToCollumn(0, 42 + x, 1);
        }
        nn6_1.AddNeuron(1, 12);
        nn6_1.ConnectCollumnToCollumn(1, 2);
    }

    public float[] NN6_2Out(int num0, int num1, int num2)
    {
        float[] output = new float[] { 0.4f, 0.4f, 0.4f, 0.4f, 0.4f, 0.4f };

        if (num0 == 12 || num0 == 13 || num0 == 14 || num1 == 12 || num1 == 13 || num1 == 14)
        {
            output[0] = 1;
        }
        if (num0 == 9 || num0 == 10 || num0 == 11 || num1 == 9 || num1 == 10 || num1 == 11)
        {
            output[2] = 1;
        }
        if (num2 == 15 || num2 == 16 || num2 == 17)
        {
            output[3] = 1;
        }
        if (num2 == 9 || num2 == 10 || num2 == 11)
        {
            output[4] = 1;
        }
        return output;
    }
    public void TrainNN6_2(NeuralNetwork network)
    {
        int rightChoices = 0;
        float[] inputs = new float[154];
        float[] outputs = new float[] { 0.4f, 0.4f, 0.4f, 0.4f, 0.4f, 0.4f };
        float biggest4diff = 0, biggest1diff = 0;
        int[] pieces0 = { 9, 10, 11, 12, 13, 14, 1 };
        int[] pieces1 = { 9, 10, 11, 12, 13, 14, 1 };
        int[] pieces2 = { 9, 10, 11, 15, 16, 17, 1 };
        int x;
        for (x = 0; x < 154; x++)
        {
            inputs[x] = 0;
        }

        for (x = 0; x < 0; x++)
        {
            inputs[pieces0[x]] = 1;// set the right one to one
            network.RunInputs(inputs);
            network.BackPropegate(outputs);
            {
                if (outputs[0] == 0.4f)
                {
                    float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 0).GetValue() - 0.4f;
                    if (tempDiff > biggest4diff)
                        biggest4diff = tempDiff;
                }
                if (outputs[1] == 0.4f)
                {
                    float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 1).GetValue() - 0.4f;
                    if (tempDiff > biggest4diff)
                        biggest4diff = tempDiff;
                }
                if (outputs[2] == 0.4f)
                {
                    float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 2).GetValue() - 0.4f;
                    if (tempDiff > biggest4diff)
                        biggest4diff = tempDiff;
                }
                if (outputs[3] == 0.4f)
                {
                    float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 3).GetValue() - 0.4f;
                    if (tempDiff > biggest4diff)
                        biggest4diff = tempDiff;
                }
                if (outputs[4] == 0.4f)
                {
                    float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 4).GetValue() - 0.4f;
                    if (tempDiff > biggest4diff)
                        biggest4diff = tempDiff;
                }
                if (outputs[5] == 0.4f)
                {
                    float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 5).GetValue() - 0.4f;
                    if (tempDiff > biggest4diff)
                        biggest4diff = tempDiff;
                }
            }
            inputs[153] = 1; // set the cp on
            outputs = NN6_1Out(pieces0[x], 0, 0);//all with the checkpoint
            network.RunInputs(inputs);
            network.BackPropegate(outputs);

            {
                if (outputs[0] == 0.4f)
                {
                    float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 0).GetValue() - 0.4f;
                    if (tempDiff > biggest4diff)
                        biggest4diff = tempDiff;
                }
                if (outputs[1] == 0.4f)
                {
                    float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 1).GetValue() - 0.4f;
                    if (tempDiff > biggest4diff)
                        biggest4diff = tempDiff;
                }
                if (outputs[2] == 0.4f)
                {
                    float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 2).GetValue() - 0.4f;
                    if (tempDiff > biggest4diff)
                        biggest4diff = tempDiff;
                }
                if (outputs[3] == 0.4f)
                {
                    float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 3).GetValue() - 0.4f;
                    if (tempDiff > biggest4diff)
                        biggest4diff = tempDiff;
                }
                if (outputs[4] == 0.4f)
                {
                    float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 4).GetValue() - 0.4f;
                    if (tempDiff > biggest4diff)
                        biggest4diff = tempDiff;
                }
                if (outputs[5] == 0.4f)
                {
                    float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 5).GetValue() - 0.4f;
                    if (tempDiff > biggest4diff)
                        biggest4diff = tempDiff;
                }
                if (outputs[0] == 1)
                {
                    float tempDiff = 1 - network.GetNeuron(network.GetSize().Length - 1, 0).GetValue();
                    if (tempDiff > biggest1diff)
                        biggest1diff = tempDiff;
                }
                if (outputs[1] == 1)
                {
                    float tempDiff = 1 - network.GetNeuron(network.GetSize().Length - 1, 1).GetValue();
                    if (tempDiff > biggest1diff)
                        biggest1diff = tempDiff;
                }
                if (outputs[2] == 1)
                {
                    float tempDiff = 1 - network.GetNeuron(network.GetSize().Length - 1, 2).GetValue();
                    if (tempDiff > biggest1diff)
                        biggest1diff = tempDiff;
                }
                if (outputs[3] == 1)
                {
                    float tempDiff = 1 - network.GetNeuron(network.GetSize().Length - 1, 3).GetValue();
                    if (tempDiff > biggest1diff)
                        biggest1diff = tempDiff;
                }
                if (outputs[4] == 1)
                {
                    float tempDiff = 1 - network.GetNeuron(network.GetSize().Length - 1, 4).GetValue();
                    if (tempDiff > biggest1diff)
                        biggest1diff = tempDiff;
                }
                if (outputs[5] == 1)
                {
                    float tempDiff = 1 - network.GetNeuron(network.GetSize().Length - 1, 5).GetValue();
                    if (tempDiff > biggest1diff)
                        biggest1diff = tempDiff;
                }
            }
            //correctly chosen
            if (outputs[0] == 1 && network.GetOutput() == 0 ||
                outputs[1] == 1 && network.GetOutput() == 1 ||
                outputs[2] == 1 && network.GetOutput() == 2 ||
                outputs[3] == 1 && network.GetOutput() == 3 ||
                outputs[4] == 1 && network.GetOutput() == 4 ||
                outputs[5] == 1 && network.GetOutput() == 5)
                rightChoices++;
            else
            {
                if (outputs[0] == 1)
                    Debug.Log("CorrectChoice-0|| actual choice-" + network.GetOutput());
                if (outputs[1] == 1)
                    Debug.Log("CorrectChoice-1|| actual choice-" + network.GetOutput());
                if (outputs[2] == 1)
                    Debug.Log("CorrectChoice-2|| actual choice-" + network.GetOutput());
                if (outputs[3] == 1)
                    Debug.Log("CorrectChoice-3|| actual choice-" + network.GetOutput());
                if (outputs[4] == 1)
                    Debug.Log("CorrectChoice-4|| actual choice-" + network.GetOutput());
                if (outputs[5] == 1)
                    Debug.Log("CorrectChoice-5|| actual choice-" + network.GetOutput());
            }

            inputs[153] = 0; //reset
            outputs = new float[] { 0.4f, 0.4f, 0.4f, 0.4f, 0.4f, 0.4f };
            inputs[pieces0[x]] = 0;
        }
        for (x = 0; x < 0; x++)
        {
            inputs[21 + pieces1[x]] = 1;// set the right one to one
            network.RunInputs(inputs);
            network.BackPropegate(outputs);
            {
                if (outputs[0] == 0.4f)
                {
                    float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 0).GetValue() - 0.4f;
                    if (tempDiff > biggest4diff)
                        biggest4diff = tempDiff;
                }
                if (outputs[1] == 0.4f)
                {
                    float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 1).GetValue() - 0.4f;
                    if (tempDiff > biggest4diff)
                        biggest4diff = tempDiff;
                }
                if (outputs[2] == 0.4f)
                {
                    float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 2).GetValue() - 0.4f;
                    if (tempDiff > biggest4diff)
                        biggest4diff = tempDiff;
                }
                if (outputs[3] == 0.4f)
                {
                    float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 3).GetValue() - 0.4f;
                    if (tempDiff > biggest4diff)
                        biggest4diff = tempDiff;
                }
                if (outputs[4] == 0.4f)
                {
                    float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 4).GetValue() - 0.4f;
                    if (tempDiff > biggest4diff)
                        biggest4diff = tempDiff;
                }
                if (outputs[5] == 0.4f)
                {
                    float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 5).GetValue() - 0.4f;
                    if (tempDiff > biggest4diff)
                        biggest4diff = tempDiff;
                }
            }
            inputs[153] = 1; // set the cp on
            outputs = NN6_0Out(0, 21 + pieces1[x], 0);//all with the checkpoint
            network.RunInputs(inputs);
            network.BackPropegate(outputs);

            {
                if (outputs[0] == 0.4f)
                {
                    float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 0).GetValue() - 0.4f;
                    if (tempDiff > biggest4diff)
                        biggest4diff = tempDiff;
                }
                if (outputs[1] == 0.4f)
                {
                    float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 1).GetValue() - 0.4f;
                    if (tempDiff > biggest4diff)
                        biggest4diff = tempDiff;
                }
                if (outputs[2] == 0.4f)
                {
                    float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 2).GetValue() - 0.4f;
                    if (tempDiff > biggest4diff)
                        biggest4diff = tempDiff;
                }
                if (outputs[3] == 0.4f)
                {
                    float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 3).GetValue() - 0.4f;
                    if (tempDiff > biggest4diff)
                        biggest4diff = tempDiff;
                }
                if (outputs[4] == 0.4f)
                {
                    float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 4).GetValue() - 0.4f;
                    if (tempDiff > biggest4diff)
                        biggest4diff = tempDiff;
                }
                if (outputs[5] == 0.4f)
                {
                    float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 5).GetValue() - 0.4f;
                    if (tempDiff > biggest4diff)
                        biggest4diff = tempDiff;
                }
                if (outputs[0] == 1)
                {
                    float tempDiff = 1 - network.GetNeuron(network.GetSize().Length - 1, 0).GetValue();
                    if (tempDiff > biggest1diff)
                        biggest1diff = tempDiff;
                }
                if (outputs[1] == 1)
                {
                    float tempDiff = 1 - network.GetNeuron(network.GetSize().Length - 1, 1).GetValue();
                    if (tempDiff > biggest1diff)
                        biggest1diff = tempDiff;
                }
                if (outputs[2] == 1)
                {
                    float tempDiff = 1 - network.GetNeuron(network.GetSize().Length - 1, 2).GetValue();
                    if (tempDiff > biggest1diff)
                        biggest1diff = tempDiff;
                }
                if (outputs[3] == 1)
                {
                    float tempDiff = 1 - network.GetNeuron(network.GetSize().Length - 1, 3).GetValue();
                    if (tempDiff > biggest1diff)
                        biggest1diff = tempDiff;
                }
                if (outputs[4] == 1)
                {
                    float tempDiff = 1 - network.GetNeuron(network.GetSize().Length - 1, 4).GetValue();
                    if (tempDiff > biggest1diff)
                        biggest1diff = tempDiff;
                }
                if (outputs[5] == 1)
                {
                    float tempDiff = 1 - network.GetNeuron(network.GetSize().Length - 1, 5).GetValue();
                    if (tempDiff > biggest1diff)
                        biggest1diff = tempDiff;
                }
            }
            //correctly chosen
            if (outputs[0] == 1 && network.GetOutput() == 0 ||
                outputs[1] == 1 && network.GetOutput() == 1 ||
                outputs[2] == 1 && network.GetOutput() == 2 ||
                outputs[3] == 1 && network.GetOutput() == 3 ||
                outputs[4] == 1 && network.GetOutput() == 4 ||
                outputs[5] == 1 && network.GetOutput() == 5)
                rightChoices++;
            else
            {
                if (outputs[0] == 1)
                    Debug.Log("CorrectChoice-0|| actual choice-" + network.GetOutput());
                if (outputs[1] == 1)
                    Debug.Log("CorrectChoice-1|| actual choice-" + network.GetOutput());
                if (outputs[2] == 1)
                    Debug.Log("CorrectChoice-2|| actual choice-" + network.GetOutput());
                if (outputs[3] == 1)
                    Debug.Log("CorrectChoice-3|| actual choice-" + network.GetOutput());
                if (outputs[4] == 1)
                    Debug.Log("CorrectChoice-4|| actual choice-" + network.GetOutput());
                if (outputs[5] == 1)
                    Debug.Log("CorrectChoice-5|| actual choice-" + network.GetOutput());
            }

            inputs[153] = 0; //reset
            outputs = new float[] { 0.4f, 0.4f, 0.4f, 0.4f, 0.4f, 0.4f };
            inputs[21 + pieces1[x]] = 0;
        }
        for (x = 0; x < 7; x++)

        {
            inputs[42 + pieces2[x]] = 1;// set the right one to one
            network.RunInputs(inputs);
            network.BackPropegate(outputs);
            {
                if (outputs[0] == 0.4f)
                {
                    float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 0).GetValue() - 0.4f;
                    if (tempDiff > biggest4diff)
                        biggest4diff = tempDiff;
                }
                if (outputs[1] == 0.4f)
                {
                    float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 1).GetValue() - 0.4f;
                    if (tempDiff > biggest4diff)
                        biggest4diff = tempDiff;
                }
                if (outputs[2] == 0.4f)
                {
                    float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 2).GetValue() - 0.4f;
                    if (tempDiff > biggest4diff)
                        biggest4diff = tempDiff;
                }
                if (outputs[3] == 0.4f)
                {
                    float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 3).GetValue() - 0.4f;
                    if (tempDiff > biggest4diff)
                        biggest4diff = tempDiff;
                }
                if (outputs[4] == 0.4f)
                {
                    float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 4).GetValue() - 0.4f;
                    if (tempDiff > biggest4diff)
                        biggest4diff = tempDiff;
                }
                if (outputs[5] == 0.4f)
                {
                    float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 5).GetValue() - 0.4f;
                    if (tempDiff > biggest4diff)
                        biggest4diff = tempDiff;
                }
            }
            inputs[153] = 1; // set the cp on
            outputs = NN6_2Out(0, 0, pieces2[x]);//all with the checkpoint
            network.RunInputs(inputs);
            network.BackPropegate(outputs);

            {
                if (outputs[0] == 0.4f)
                {
                    float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 0).GetValue() - 0.4f;
                    if (tempDiff > biggest4diff)
                        biggest4diff = tempDiff;
                }
                if (outputs[1] == 0.4f)
                {
                    float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 1).GetValue() - 0.4f;
                    if (tempDiff > biggest4diff)
                        biggest4diff = tempDiff;
                }
                if (outputs[2] == 0.4f)
                {
                    float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 2).GetValue() - 0.4f;
                    if (tempDiff > biggest4diff)
                        biggest4diff = tempDiff;
                }
                if (outputs[3] == 0.4f)
                {
                    float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 3).GetValue() - 0.4f;
                    if (tempDiff > biggest4diff)
                        biggest4diff = tempDiff;
                }
                if (outputs[4] == 0.4f)
                {
                    float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 4).GetValue() - 0.4f;
                    if (tempDiff > biggest4diff)
                        biggest4diff = tempDiff;
                }
                if (outputs[5] == 0.4f)
                {
                    float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 5).GetValue() - 0.4f;
                    if (tempDiff > biggest4diff)
                        biggest4diff = tempDiff;
                }
                if (outputs[0] == 1)
                {
                    float tempDiff = 1 - network.GetNeuron(network.GetSize().Length - 1, 0).GetValue();
                    if (tempDiff > biggest1diff)
                        biggest1diff = tempDiff;
                }
                if (outputs[1] == 1)
                {
                    float tempDiff = 1 - network.GetNeuron(network.GetSize().Length - 1, 1).GetValue();
                    if (tempDiff > biggest1diff)
                        biggest1diff = tempDiff;
                }
                if (outputs[2] == 1)
                {
                    float tempDiff = 1 - network.GetNeuron(network.GetSize().Length - 1, 2).GetValue();
                    if (tempDiff > biggest1diff)
                        biggest1diff = tempDiff;
                }
                if (outputs[3] == 1)
                {
                    float tempDiff = 1 - network.GetNeuron(network.GetSize().Length - 1, 3).GetValue();
                    if (tempDiff > biggest1diff)
                        biggest1diff = tempDiff;
                }
                if (outputs[4] == 1)
                {
                    float tempDiff = 1 - network.GetNeuron(network.GetSize().Length - 1, 4).GetValue();
                    if (tempDiff > biggest1diff)
                        biggest1diff = tempDiff;
                }
                if (outputs[5] == 1)
                {
                    float tempDiff = 1 - network.GetNeuron(network.GetSize().Length - 1, 5).GetValue();
                    if (tempDiff > biggest1diff)
                        biggest1diff = tempDiff;
                }
            }
            //correctly chosen
            if (outputs[0] == 1 && network.GetOutput() == 0 ||
                outputs[1] == 1 && network.GetOutput() == 1 ||
                outputs[2] == 1 && network.GetOutput() == 2 ||
                outputs[3] == 1 && network.GetOutput() == 3 ||
                outputs[4] == 1 && network.GetOutput() == 4 ||
                outputs[5] == 1 && network.GetOutput() == 5)
                rightChoices++;
            else
            {
                if (outputs[0] == 1)
                    Debug.Log("CorrectChoice-0|| actual choice-" + network.GetOutput());
                if (outputs[1] == 1)
                    Debug.Log("CorrectChoice-1|| actual choice-" + network.GetOutput());
                if (outputs[2] == 1)
                    Debug.Log("CorrectChoice-2|| actual choice-" + network.GetOutput());
                if (outputs[3] == 1)
                    Debug.Log("CorrectChoice-3|| actual choice-" + network.GetOutput());
                if (outputs[4] == 1)
                    Debug.Log("CorrectChoice-4|| actual choice-" + network.GetOutput());
                if (outputs[5] == 1)
                    Debug.Log("CorrectChoice-5|| actual choice-" + network.GetOutput());
            }

            inputs[153] = 0; //reset
            outputs = new float[] { 0.4f, 0.4f, 0.4f, 0.4f, 0.4f, 0.4f };
            inputs[42 + pieces2[x]] = 0;
        }

        Debug.Log("1.0:" + (1 - biggest1diff) + "|||0.4:" + (0.4 + biggest4diff));
        Debug.Log("out of 18 n6_2 correctly chose : " + rightChoices);
        network.AddDesiredChanges(0.45f);
    }
    public void MakeGoodNN6_2()
    {
        nn6_2 = new NeuralNetwork();
        nn6_2.AddCollumn(0);
        nn6_2.AddCollumn(1);
        nn6_2.AddCollumn(2);
        int x;
        for (x = 0; x < 155; x++)//154 + bias node
        {
            nn6_2.AddNeuron(0, 0);
        }
        for (x = 0; x < 13; x++)
        {
            nn6_2.AddNeuron(1, 0);
        }
        for (x = 0; x < 6; x++)
        {
            nn6_2.AddNeuron(2, 0);
        }
        nn6_2.DeleteNeuron(1, 12);
        nn6_2.ConnectNeuronToCollumn(0, 154, 1);//biases
        nn6_2.ConnectNeuronToCollumn(0, 153, 1);//biases
        for (x = 0; x < 21; x++)
        {
            if (x == 9 || x == 10 || x == 11 || x == 12 || x == 13 || x == 14)
                nn6_2.ConnectNeuronToCollumn(0, 0 + x, 1);
            if (x == 9 || x == 10 || x == 11 || x == 12 || x == 13 || x == 14)
                nn6_2.ConnectNeuronToCollumn(0, 21 + x, 1);
            if (x == 9 || x == 10 || x == 11 || x == 15 || x == 16 || x == 17)
                nn6_2.ConnectNeuronToCollumn(0, 42 + x, 1);
        }
        nn6_2.AddNeuron(1, 12);
        nn6_2.ConnectCollumnToCollumn(1, 2);
    }

    public void TrainNN6_3(NeuralNetwork network)
    {
        int rightChoices = 0;
        float[] inputs = new float[155];
        float[] outputs = new float[] { 0.4f, 0.4f, 0.4f, 0.4f, 0.4f, 0.4f };
        float biggest4diff = 0, biggest1diff = 0;
        int[] piece0spots = { 0, 1, 2 };
        int[] piece3spots = { 3, 4, 5, 6 };
        int x, y;
        for (x = 0; x < 154; x++)
        {
            inputs[x] = 0;
        }
        inputs[154] = 1;

        for (x = 0; x < 3; x++)
        {
            for (y = 0; y < 4; y++)
            {
                outputs = new float[] { 0.4f, 0.4f, 0.4f, 0.4f, 0.4f, 0.4f };

                inputs[piece0spots[x]] = 0;
                inputs[21 * piece3spots[y] + 9] = 0;
                inputs[153] = 0;
                network.RunInputs(inputs);
                network.BackPropegate(outputs);

                {
                    if (outputs[0] == 0.4f)
                    {
                        float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 0).GetValue() - 0.4f;
                        if (tempDiff > biggest4diff)
                            biggest4diff = tempDiff;
                    }
                    if (outputs[1] == 0.4f)
                    {
                        float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 1).GetValue() - 0.4f;
                        if (tempDiff > biggest4diff)
                            biggest4diff = tempDiff;
                    }
                    if (outputs[2] == 0.4f)
                    {
                        float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 2).GetValue() - 0.4f;
                        if (tempDiff > biggest4diff)
                            biggest4diff = tempDiff;
                    }
                    if (outputs[3] == 0.4f)
                    {
                        float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 3).GetValue() - 0.4f;
                        if (tempDiff > biggest4diff)
                            biggest4diff = tempDiff;
                    }
                    if (outputs[4] == 0.4f)
                    {
                        float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 4).GetValue() - 0.4f;
                        if (tempDiff > biggest4diff)
                            biggest4diff = tempDiff;
                    }
                    if (outputs[5] == 0.4f)
                    {
                        float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 5).GetValue() - 0.4f;
                        if (tempDiff > biggest4diff)
                            biggest4diff = tempDiff;
                    }
                }

                outputs = new float[] { 0.4f, 0.4f, 0.4f, 0.4f, 0.4f, 0.4f };

                inputs[piece0spots[x]] = 1;
                inputs[21 * piece3spots[y] + 9] = 0;
                inputs[153] = 0;
                network.RunInputs(inputs);
                network.BackPropegate(outputs);

                {
                    if (outputs[0] == 0.4f)
                    {
                        float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 0).GetValue() - 0.4f;
                        if (tempDiff > biggest4diff)
                            biggest4diff = tempDiff;
                    }
                    if (outputs[1] == 0.4f)
                    {
                        float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 1).GetValue() - 0.4f;
                        if (tempDiff > biggest4diff)
                            biggest4diff = tempDiff;
                    }
                    if (outputs[2] == 0.4f)
                    {
                        float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 2).GetValue() - 0.4f;
                        if (tempDiff > biggest4diff)
                            biggest4diff = tempDiff;
                    }
                    if (outputs[3] == 0.4f)
                    {
                        float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 3).GetValue() - 0.4f;
                        if (tempDiff > biggest4diff)
                            biggest4diff = tempDiff;
                    }
                    if (outputs[4] == 0.4f)
                    {
                        float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 4).GetValue() - 0.4f;
                        if (tempDiff > biggest4diff)
                            biggest4diff = tempDiff;
                    }
                    if (outputs[5] == 0.4f)
                    {
                        float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 5).GetValue() - 0.4f;
                        if (tempDiff > biggest4diff)
                            biggest4diff = tempDiff;
                    }
                }

                outputs = new float[] { 0.4f, 0.4f, 0.4f, 0.4f, 0.4f, 0.4f };

                inputs[piece0spots[x]] = 0;
                inputs[21 * piece3spots[y] + 9] = 1;
                inputs[153] = 0;
                network.RunInputs(inputs);
                network.BackPropegate(outputs);

                {
                    if (outputs[0] == 0.4f)
                    {
                        float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 0).GetValue() - 0.4f;
                        if (tempDiff > biggest4diff)
                            biggest4diff = tempDiff;
                    }
                    if (outputs[1] == 0.4f)
                    {
                        float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 1).GetValue() - 0.4f;
                        if (tempDiff > biggest4diff)
                            biggest4diff = tempDiff;
                    }
                    if (outputs[2] == 0.4f)
                    {
                        float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 2).GetValue() - 0.4f;
                        if (tempDiff > biggest4diff)
                            biggest4diff = tempDiff;
                    }
                    if (outputs[3] == 0.4f)
                    {
                        float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 3).GetValue() - 0.4f;
                        if (tempDiff > biggest4diff)
                            biggest4diff = tempDiff;
                    }
                    if (outputs[4] == 0.4f)
                    {
                        float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 4).GetValue() - 0.4f;
                        if (tempDiff > biggest4diff)
                            biggest4diff = tempDiff;
                    }
                    if (outputs[5] == 0.4f)
                    {
                        float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 5).GetValue() - 0.4f;
                        if (tempDiff > biggest4diff)
                            biggest4diff = tempDiff;
                    }
                }

                outputs = new float[] { 0.4f, 0.4f, 0.4f, 0.4f, 0.4f, 0.4f };

                inputs[piece0spots[x]] = 1;
                inputs[21 * piece3spots[y] + 9] = 1;
                inputs[153] = 0;
                network.RunInputs(inputs);
                network.BackPropegate(outputs);

                {
                    if (outputs[0] == 0.4f)
                    {
                        float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 0).GetValue() - 0.4f;
                        if (tempDiff > biggest4diff)
                            biggest4diff = tempDiff;
                    }
                    if (outputs[1] == 0.4f)
                    {
                        float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 1).GetValue() - 0.4f;
                        if (tempDiff > biggest4diff)
                            biggest4diff = tempDiff;
                    }
                    if (outputs[2] == 0.4f)
                    {
                        float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 2).GetValue() - 0.4f;
                        if (tempDiff > biggest4diff)
                            biggest4diff = tempDiff;
                    }
                    if (outputs[3] == 0.4f)
                    {
                        float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 3).GetValue() - 0.4f;
                        if (tempDiff > biggest4diff)
                            biggest4diff = tempDiff;
                    }
                    if (outputs[4] == 0.4f)
                    {
                        float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 4).GetValue() - 0.4f;
                        if (tempDiff > biggest4diff)
                            biggest4diff = tempDiff;
                    }
                    if (outputs[5] == 0.4f)
                    {
                        float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 5).GetValue() - 0.4f;
                        if (tempDiff > biggest4diff)
                            biggest4diff = tempDiff;
                    }
                }

                outputs = new float[] { 0.4f, 0.4f, 0.4f, 0.4f, 0.4f, 0.4f };

                inputs[piece0spots[x]] = 0;
                inputs[21 * piece3spots[y] + 9] = 0;
                inputs[153] = 1;
                network.RunInputs(inputs);
                network.BackPropegate(outputs);

                {
                    if (outputs[0] == 0.4f)
                    {
                        float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 0).GetValue() - 0.4f;
                        if (tempDiff > biggest4diff)
                            biggest4diff = tempDiff;
                    }
                    if (outputs[1] == 0.4f)
                    {
                        float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 1).GetValue() - 0.4f;
                        if (tempDiff > biggest4diff)
                            biggest4diff = tempDiff;
                    }
                    if (outputs[2] == 0.4f)
                    {
                        float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 2).GetValue() - 0.4f;
                        if (tempDiff > biggest4diff)
                            biggest4diff = tempDiff;
                    }
                    if (outputs[3] == 0.4f)
                    {
                        float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 3).GetValue() - 0.4f;
                        if (tempDiff > biggest4diff)
                            biggest4diff = tempDiff;
                    }
                    if (outputs[4] == 0.4f)
                    {
                        float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 4).GetValue() - 0.4f;
                        if (tempDiff > biggest4diff)
                            biggest4diff = tempDiff;
                    }
                    if (outputs[5] == 0.4f)
                    {
                        float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 5).GetValue() - 0.4f;
                        if (tempDiff > biggest4diff)
                            biggest4diff = tempDiff;
                    }
                }
                {
                    if (outputs[0] == 1)
                    {
                        float tempDiff = 1 - network.GetNeuron(network.GetSize().Length - 1, 0).GetValue();
                        if (tempDiff > biggest1diff)
                            biggest1diff = tempDiff;
                    }
                    if (outputs[1] == 1)
                    {
                        float tempDiff = 1 - network.GetNeuron(network.GetSize().Length - 1, 1).GetValue();
                        if (tempDiff > biggest1diff)
                            biggest1diff = tempDiff;
                    }
                    if (outputs[2] == 1)
                    {
                        float tempDiff = 1 - network.GetNeuron(network.GetSize().Length - 1, 2).GetValue();
                        if (tempDiff > biggest1diff)
                            biggest1diff = tempDiff;
                    }
                    if (outputs[3] == 1)
                    {
                        float tempDiff = 1 - network.GetNeuron(network.GetSize().Length - 1, 3).GetValue();
                        if (tempDiff > biggest1diff)
                            biggest1diff = tempDiff;
                    }
                    if (outputs[4] == 1)
                    {
                        float tempDiff = 1 - network.GetNeuron(network.GetSize().Length - 1, 4).GetValue();
                        if (tempDiff > biggest1diff)
                            biggest1diff = tempDiff;
                    }
                    if (outputs[5] == 1)
                    {
                        float tempDiff = 1 - network.GetNeuron(network.GetSize().Length - 1, 5).GetValue();
                        if (tempDiff > biggest1diff)
                            biggest1diff = tempDiff;
                    }
                }

                outputs = new float[] { 0.4f, 0.4f, 0.4f, 0.4f, 1f, 0.4f };

                inputs[piece0spots[x]] = 1;
                inputs[21 * piece3spots[y] + 9] = 0;
                inputs[153] = 1;
                network.RunInputs(inputs);
                network.BackPropegate(outputs);

                {
                    if (outputs[0] == 0.4f)
                    {
                        float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 0).GetValue() - 0.4f;
                        if (tempDiff > biggest4diff)
                            biggest4diff = tempDiff;
                    }
                    if (outputs[1] == 0.4f)
                    {
                        float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 1).GetValue() - 0.4f;
                        if (tempDiff > biggest4diff)
                            biggest4diff = tempDiff;
                    }
                    if (outputs[2] == 0.4f)
                    {
                        float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 2).GetValue() - 0.4f;
                        if (tempDiff > biggest4diff)
                            biggest4diff = tempDiff;
                    }
                    if (outputs[3] == 0.4f)
                    {
                        float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 3).GetValue() - 0.4f;
                        if (tempDiff > biggest4diff)
                            biggest4diff = tempDiff;
                    }
                    if (outputs[4] == 0.4f)
                    {
                        float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 4).GetValue() - 0.4f;
                        if (tempDiff > biggest4diff)
                            biggest4diff = tempDiff;
                    }
                    if (outputs[5] == 0.4f)
                    {
                        float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 5).GetValue() - 0.4f;
                        if (tempDiff > biggest4diff)
                            biggest4diff = tempDiff;
                    }
                }
                {
                    if (outputs[0] == 1)
                    {
                        float tempDiff = 1 - network.GetNeuron(network.GetSize().Length - 1, 0).GetValue();
                        if (tempDiff > biggest1diff)
                            biggest1diff = tempDiff;
                    }
                    if (outputs[1] == 1)
                    {
                        float tempDiff = 1 - network.GetNeuron(network.GetSize().Length - 1, 1).GetValue();
                        if (tempDiff > biggest1diff)
                            biggest1diff = tempDiff;
                    }
                    if (outputs[2] == 1)
                    {
                        float tempDiff = 1 - network.GetNeuron(network.GetSize().Length - 1, 2).GetValue();
                        if (tempDiff > biggest1diff)
                            biggest1diff = tempDiff;
                    }
                    if (outputs[3] == 1)
                    {
                        float tempDiff = 1 - network.GetNeuron(network.GetSize().Length - 1, 3).GetValue();
                        if (tempDiff > biggest1diff)
                            biggest1diff = tempDiff;
                    }
                    if (outputs[4] == 1)
                    {
                        float tempDiff = 1 - network.GetNeuron(network.GetSize().Length - 1, 4).GetValue();
                        if (tempDiff > biggest1diff)
                            biggest1diff = tempDiff;
                    }
                    if (outputs[5] == 1)
                    {
                        float tempDiff = 1 - network.GetNeuron(network.GetSize().Length - 1, 5).GetValue();
                        if (tempDiff > biggest1diff)
                            biggest1diff = tempDiff;
                    }
                }

                outputs = new float[] { 0.4f, 0.4f, 0.4f, 0.4f, 0.4f, 0.4f };

                inputs[piece0spots[x]] = 0;
                inputs[21 * piece3spots[y] + 9] = 1;
                inputs[153] = 1;
                network.RunInputs(inputs);
                network.BackPropegate(outputs);

                {
                    if (outputs[0] == 0.4f)
                    {
                        float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 0).GetValue() - 0.4f;
                        if (tempDiff > biggest4diff)
                            biggest4diff = tempDiff;
                    }
                    if (outputs[1] == 0.4f)
                    {
                        float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 1).GetValue() - 0.4f;
                        if (tempDiff > biggest4diff)
                            biggest4diff = tempDiff;
                    }
                    if (outputs[2] == 0.4f)
                    {
                        float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 2).GetValue() - 0.4f;
                        if (tempDiff > biggest4diff)
                            biggest4diff = tempDiff;
                    }
                    if (outputs[3] == 0.4f)
                    {
                        float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 3).GetValue() - 0.4f;
                        if (tempDiff > biggest4diff)
                            biggest4diff = tempDiff;
                    }
                    if (outputs[4] == 0.4f)
                    {
                        float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 4).GetValue() - 0.4f;
                        if (tempDiff > biggest4diff)
                            biggest4diff = tempDiff;
                    }
                    if (outputs[5] == 0.4f)
                    {
                        float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 5).GetValue() - 0.4f;
                        if (tempDiff > biggest4diff)
                            biggest4diff = tempDiff;
                    }
                }
                {
                    if (outputs[0] == 1)
                    {
                        float tempDiff = 1 - network.GetNeuron(network.GetSize().Length - 1, 0).GetValue();
                        if (tempDiff > biggest1diff)
                            biggest1diff = tempDiff;
                    }
                    if (outputs[1] == 1)
                    {
                        float tempDiff = 1 - network.GetNeuron(network.GetSize().Length - 1, 1).GetValue();
                        if (tempDiff > biggest1diff)
                            biggest1diff = tempDiff;
                    }
                    if (outputs[2] == 1)
                    {
                        float tempDiff = 1 - network.GetNeuron(network.GetSize().Length - 1, 2).GetValue();
                        if (tempDiff > biggest1diff)
                            biggest1diff = tempDiff;
                    }
                    if (outputs[3] == 1)
                    {
                        float tempDiff = 1 - network.GetNeuron(network.GetSize().Length - 1, 3).GetValue();
                        if (tempDiff > biggest1diff)
                            biggest1diff = tempDiff;
                    }
                    if (outputs[4] == 1)
                    {
                        float tempDiff = 1 - network.GetNeuron(network.GetSize().Length - 1, 4).GetValue();
                        if (tempDiff > biggest1diff)
                            biggest1diff = tempDiff;
                    }
                    if (outputs[5] == 1)
                    {
                        float tempDiff = 1 - network.GetNeuron(network.GetSize().Length - 1, 5).GetValue();
                        if (tempDiff > biggest1diff)
                            biggest1diff = tempDiff;
                    }
                }

                outputs = new float[] { 0.4f, 0.4f, 0.4f, 1f, 0.4f, 0.4f };

                inputs[piece0spots[x]] = 1;
                inputs[21 * piece3spots[y] + 9] = 1;
                inputs[153] = 1;



                network.RunInputs(inputs);
                network.BackPropegate(outputs);

                Debug.Log("(" + network.GetNeuron(network.GetSize().Length - 1, 0).GetValue() + ", "
                                + network.GetNeuron(network.GetSize().Length - 1, 1).GetValue() + ", "
                                + network.GetNeuron(network.GetSize().Length - 1, 2).GetValue() + ", "
                                + network.GetNeuron(network.GetSize().Length - 1, 3).GetValue() + ", "
                                + network.GetNeuron(network.GetSize().Length - 1, 4).GetValue() + ", "
                                + network.GetNeuron(network.GetSize().Length - 1, 5).GetValue() + ")"); ;

                {
                    if (outputs[0] == 0.4f)
                    {
                        float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 0).GetValue() - 0.4f;
                        if (tempDiff > biggest4diff)
                            biggest4diff = tempDiff;
                    }
                    if (outputs[1] == 0.4f)
                    {
                        float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 1).GetValue() - 0.4f;
                        if (tempDiff > biggest4diff)
                            biggest4diff = tempDiff;
                    }
                    if (outputs[2] == 0.4f)
                    {
                        float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 2).GetValue() - 0.4f;
                        if (tempDiff > biggest4diff)
                            biggest4diff = tempDiff;
                    }
                    if (outputs[3] == 0.4f)
                    {
                        float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 3).GetValue() - 0.4f;
                        if (tempDiff > biggest4diff)
                            biggest4diff = tempDiff;
                    }
                    if (outputs[4] == 0.4f)
                    {
                        float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 4).GetValue() - 0.4f;
                        if (tempDiff > biggest4diff)
                            biggest4diff = tempDiff;
                    }
                    if (outputs[5] == 0.4f)
                    {
                        float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 5).GetValue() - 0.4f;
                        if (tempDiff > biggest4diff)
                            biggest4diff = tempDiff;
                    }
                }
                {
                    if (outputs[0] == 1)
                    {
                        float tempDiff = 1 - network.GetNeuron(network.GetSize().Length - 1, 0).GetValue();
                        if (tempDiff > biggest1diff)
                            biggest1diff = tempDiff;
                    }
                    if (outputs[1] == 1)
                    {
                        float tempDiff = 1 - network.GetNeuron(network.GetSize().Length - 1, 1).GetValue();
                        if (tempDiff > biggest1diff)
                            biggest1diff = tempDiff;
                    }
                    if (outputs[2] == 1)
                    {
                        float tempDiff = 1 - network.GetNeuron(network.GetSize().Length - 1, 2).GetValue();
                        if (tempDiff > biggest1diff)
                            biggest1diff = tempDiff;
                    }
                    if (outputs[3] == 1)
                    {
                        float tempDiff = 1 - network.GetNeuron(network.GetSize().Length - 1, 3).GetValue();
                        if (tempDiff > biggest1diff)
                            biggest1diff = tempDiff;
                    }
                    if (outputs[4] == 1)
                    {
                        float tempDiff = 1 - network.GetNeuron(network.GetSize().Length - 1, 4).GetValue();
                        if (tempDiff > biggest1diff)
                            biggest1diff = tempDiff;
                    }
                    if (outputs[5] == 1)
                    {
                        float tempDiff = 1 - network.GetNeuron(network.GetSize().Length - 1, 5).GetValue();
                        if (tempDiff > biggest1diff)
                            biggest1diff = tempDiff;
                    }
                }

                inputs[piece0spots[x]] = 0;
                inputs[21 * piece3spots[y] + 9] = 0;
                inputs[153] = 0;
                outputs = new float[] { 0.4f, 0.4f, 0.4f, 0.4f, 0.4f, 0.4f };
            }
        }


        Debug.Log("1.0:" + (1 - biggest1diff) + "|||0.4:" + (0.4 + biggest4diff));
        Debug.Log("out of 1 n6_3 correctly chose : " + rightChoices);
        network.AddDesiredChanges(0.99f);

        /*
        Debug.Log("c0010--" + nn6_3.GetConnection(0, 0, 1, 0).GetValue());
        Debug.Log("c0011--" + nn6_3.GetConnection(0, 0, 1, 1).GetValue());
        Debug.Log("c0210--" + nn6_3.GetConnection(0, 153, 1, 0).GetValue());
        Debug.Log("c0211--" + nn6_3.GetConnection(0, 153, 1, 1).GetValue());
        Debug.Log("c0310--" + nn6_3.GetConnection(0, 154, 1, 0).GetValue());
        Debug.Log("c0311--" + nn6_3.GetConnection(0, 154, 1, 1).GetValue());

        Debug.Log("c1020--" + nn6_3.GetConnection(1, 0, 2, 0).GetValue());
        Debug.Log("c1021--" + nn6_3.GetConnection(1, 0, 2, 1).GetValue());
        Debug.Log("c1120--" + nn6_3.GetConnection(1, 1, 2, 0).GetValue());
        Debug.Log("c1121--" + nn6_3.GetConnection(1, 1, 2, 1).GetValue());
        Debug.Log("c1220--" + nn6_3.GetConnection(1, 2, 2, 0).GetValue());
        Debug.Log("c1221--" + nn6_3.GetConnection(1, 2, 2, 1).GetValue());

        Debug.Log("c0120--" + nn6_3.GetConnection(0, 73, 2, 0).GetValue());
        Debug.Log("c0121--" + nn6_3.GetConnection(0, 73, 2, 1).GetValue());

        Debug.Log("c2033--" + nn6_3.GetConnection(2, 0, 3, 3).GetValue());
        Debug.Log("c2133--" + nn6_3.GetConnection(2, 1, 3, 3).GetValue());
        Debug.Log("c2233--" + nn6_3.GetConnection(2, 2, 3, 3).GetValue());

        Debug.Log("c2230--" + nn6_3.GetConnection(2, 2, 3, 0).GetValue());
        Debug.Log("c2231--" + nn6_3.GetConnection(2, 2, 3, 1).GetValue());
        Debug.Log("c2232--" + nn6_3.GetConnection(2, 2, 3, 2).GetValue());
        Debug.Log("c2234--" + nn6_3.GetConnection(2, 2, 3, 4).GetValue());
        Debug.Log("c2235--" + nn6_3.GetConnection(2, 2, 3, 5).GetValue());
        */
    }
    public void MakeGoodNN6_3()
    {
        nn6_3 = new NeuralNetwork();
        nn6_3.AddCollumn(0);
        nn6_3.AddCollumn(1);
        nn6_3.AddCollumn(2);
        nn6_3.AddCollumn(3);
        int x;
        for (x = 0; x < 155; x++)//3 + bias node
        {
            nn6_3.AddNeuron(0, 0);
        }
        for (x = 0; x < 3; x++)
        {
            nn6_3.AddNeuron(1, 0);
        }
        for (x = 0; x < 3; x++)
        {
            nn6_3.AddNeuron(2, 0);
        }
        for (x = 0; x < 6; x++)
        {
            nn6_3.AddNeuron(3, 0);
        }

        //to layer 1
        //node 0 
        nn6_3.AddConnection(0, 0, 1, 0, Random.Range(-1f, 1f));
        nn6_3.AddConnection(0, 0, 1, 1, Random.Range(-1f, 1f));
        nn6_3.AddConnection(0, 1, 1, 0, Random.Range(-1f, 1f));
        nn6_3.AddConnection(0, 1, 1, 1, Random.Range(-1f, 1f));
        nn6_3.AddConnection(0, 2, 1, 0, Random.Range(-1f, 1f));
        nn6_3.AddConnection(0, 2, 1, 1, Random.Range(-1f, 1f));
        //node 2
        nn6_3.AddConnection(0, 153, 1, 0, Random.Range(-1f, 1f));///
        nn6_3.AddConnection(0, 153, 1, 1, Random.Range(-1f, 1f));///
            //node 3
        nn6_3.AddConnection(0, 154, 1, 0, Random.Range(-1f, 1f));///
        nn6_3.AddConnection(0, 154, 1, 1, Random.Range(-1f, 1f));///

        //to layer 2
        nn6_3.AddConnection(1, 0, 2, 0, Random.Range(-1f, 1f));
        nn6_3.AddConnection(1, 0, 2, 1, Random.Range(-1f, 1f));
        nn6_3.AddConnection(1, 1, 2, 0, Random.Range(-1f, 1f));
        nn6_3.AddConnection(1, 1, 2, 1, Random.Range(-1f, 1f));
        nn6_3.AddConnection(1, 2, 2, 0, Random.Range(-1f, 1f));
        nn6_3.AddConnection(1, 2, 2, 1, Random.Range(-1f, 1f));

        //node 1
        nn6_3.AddConnection(0, 63 + 9, 2, 0, Random.Range(-1f, 1f));
        nn6_3.AddConnection(0, 63 + 9, 2, 1, Random.Range(-1f, 1f));
        nn6_3.AddConnection(0, 84 + 9, 2, 0, Random.Range(-1f, 1f));
        nn6_3.AddConnection(0, 84 + 9, 2, 1, Random.Range(-1f, 1f));
        nn6_3.AddConnection(0, 105 + 9, 2, 0, Random.Range(-1f, 1f));
        nn6_3.AddConnection(0, 105 + 9, 2, 1, Random.Range(-1f, 1f));
        nn6_3.AddConnection(0, 126 + 9, 2, 0, Random.Range(-1f, 1f));
        nn6_3.AddConnection(0, 126 + 9, 2, 1, Random.Range(-1f, 1f));

        //to layer 3
        nn6_3.AddConnection(2, 0, 3, 3, Random.Range(-1f, 1f));
        nn6_3.AddConnection(2, 1, 3, 3, Random.Range(-1f, 1f));
        nn6_3.AddConnection(2, 2, 3, 3, Random.Range(-1f, 1f));
        nn6_3.AddConnection(2, 0, 3, 4, Random.Range(-1f, 1f));
        nn6_3.AddConnection(2, 1, 3, 4, Random.Range(-1f, 1f));
        nn6_3.AddConnection(2, 2, 3, 4, Random.Range(-1f, 1f));

        nn6_3.AddConnection(2, 2, 3, 0, Random.Range(-1f, 1f));
        nn6_3.AddConnection(2, 2, 3, 1, Random.Range(-1f, 1f));
        nn6_3.AddConnection(2, 2, 3, 2, Random.Range(-1f, 1f));
        nn6_3.AddConnection(2, 2, 3, 5, Random.Range(-1f, 1f));
        /*
        //to layer 1
        //node 0 
        nn6_3.AddConnection(0, 0, 1, 0, -0.383487f);
        nn6_3.AddConnection(0, 0, 1, 1, 4.895471f);
        nn6_3.AddConnection(0, 1, 1, 0, -0.383487f);
        nn6_3.AddConnection(0, 1, 1, 1, 4.895471f);
        nn6_3.AddConnection(0, 2, 1, 0, -0.383487f);
        nn6_3.AddConnection(0, 2, 1, 1, 4.895471f);
        //node 2
        nn6_3.AddConnection(0, 153, 1, 0, -0.3173361f);///
        nn6_3.AddConnection(0, 153, 1, 1, 4.944991f);///
            //node 3
        nn6_3.AddConnection(0, 154, 1, 0, 0.9871098f);///
        nn6_3.AddConnection(0, 154, 1, 1, -7.647758f);///

        //to layer 2
        nn6_3.AddConnection(1, 0, 2, 0, -2.227202f);
        nn6_3.AddConnection(1, 0, 2, 1, -1.523751f);
        nn6_3.AddConnection(1, 1, 2, 0, 8.22331f);
        nn6_3.AddConnection(1, 1, 2, 1, 6.760274f);
        nn6_3.AddConnection(1, 2, 2, 0, -4.273445f);
        nn6_3.AddConnection(1, 2, 2, 1, -3.444222f);

        //node 1
        nn6_3.AddConnection(0, 63 + 9, 2, 0, -5.810718f);
        nn6_3.AddConnection(0, 63 + 9, 2, 1, -0.2540283f);
        nn6_3.AddConnection(0, 84 + 9, 2, 0, -5.810718f);
        nn6_3.AddConnection(0, 84 + 9, 2, 1, -0.2540283f);
        nn6_3.AddConnection(0, 105 + 9, 2, 0, -5.810718f);
        nn6_3.AddConnection(0, 105 + 9, 2, 1, -0.2540283f);
        nn6_3.AddConnection(0, 126 + 9, 2, 0, -5.810718f);
        nn6_3.AddConnection(0, 126 + 9, 2, 1, -0.2540283f);

        //to layer 3
        nn6_3.AddConnection(2, 0, 3, 3, -6.172615f);
        nn6_3.AddConnection(2, 1, 3, 3, 6.331503f);
        nn6_3.AddConnection(2, 2, 3, 3, -0.4769176f);
        nn6_3.AddConnection(2, 0, 3, 4, 6.25519f);
        nn6_3.AddConnection(2, 1, 3, 4, -0.126948f);
        nn6_3.AddConnection(2, 2, 3, 4, -0.4178526f);

        nn6_3.AddConnection(2, 2, 3, 0, -0.4054653f);
        nn6_3.AddConnection(2, 2, 3, 1, -0.4054653f);
        nn6_3.AddConnection(2, 2, 3, 2, -0.4054653f);
        nn6_3.AddConnection(2, 2, 3, 5, -0.4054653f);
        */




    }

    public void TrainNN6_4(NeuralNetwork network)
    {
        int rightChoices = 0;
        float[] inputs = new float[155];
        float[] outputs = new float[] { 0.4f, 0.4f, 0.4f, 0.4f, 0.4f, 0.4f };
        float biggest4diff = 0, biggest1diff = 0;
        int[] piece0spots = { 0, 1, 2 };
        int[] piece3spots = { 3, 4, 5, 6 };
        int x, y;
        for (x = 0; x < 154; x++)
        {
            inputs[x] = 0;
        }
        inputs[154] = 1;

        for (x = 0; x < 1; x++)
        {
            for (y = 0; y < 1; y++)
            {
                outputs = new float[] { 0.4f, 0.4f, 0.4f, 0.4f, 0.4f, 0.4f };

                inputs[piece0spots[x]] = 0;
                inputs[73] = 0;
                inputs[153] = 0;
                network.RunInputs(inputs);
                network.BackPropegate(outputs);

                {
                    if (outputs[0] == 0.4f)
                    {
                        float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 0).GetValue() - 0.4f;
                        if (tempDiff > biggest4diff)
                            biggest4diff = tempDiff;
                    }
                    if (outputs[1] == 0.4f)
                    {
                        float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 1).GetValue() - 0.4f;
                        if (tempDiff > biggest4diff)
                            biggest4diff = tempDiff;
                    }
                    if (outputs[2] == 0.4f)
                    {
                        float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 2).GetValue() - 0.4f;
                        if (tempDiff > biggest4diff)
                            biggest4diff = tempDiff;
                    }
                    if (outputs[3] == 0.4f)
                    {
                        float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 3).GetValue() - 0.4f;
                        if (tempDiff > biggest4diff)
                            biggest4diff = tempDiff;
                    }
                    if (outputs[4] == 0.4f)
                    {
                        float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 4).GetValue() - 0.4f;
                        if (tempDiff > biggest4diff)
                            biggest4diff = tempDiff;
                    }
                    if (outputs[5] == 0.4f)
                    {
                        float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 5).GetValue() - 0.4f;
                        if (tempDiff > biggest4diff)
                            biggest4diff = tempDiff;
                    }
                }

                outputs = new float[] { 0.4f, 0.4f, 0.4f, 0.4f, 0.4f, 0.4f };

                inputs[piece0spots[x]] = 1;
                inputs[73] = 0;
                inputs[153] = 0;
                network.RunInputs(inputs);
                network.BackPropegate(outputs);

                {
                    if (outputs[0] == 0.4f)
                    {
                        float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 0).GetValue() - 0.4f;
                        if (tempDiff > biggest4diff)
                            biggest4diff = tempDiff;
                    }
                    if (outputs[1] == 0.4f)
                    {
                        float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 1).GetValue() - 0.4f;
                        if (tempDiff > biggest4diff)
                            biggest4diff = tempDiff;
                    }
                    if (outputs[2] == 0.4f)
                    {
                        float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 2).GetValue() - 0.4f;
                        if (tempDiff > biggest4diff)
                            biggest4diff = tempDiff;
                    }
                    if (outputs[3] == 0.4f)
                    {
                        float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 3).GetValue() - 0.4f;
                        if (tempDiff > biggest4diff)
                            biggest4diff = tempDiff;
                    }
                    if (outputs[4] == 0.4f)
                    {
                        float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 4).GetValue() - 0.4f;
                        if (tempDiff > biggest4diff)
                            biggest4diff = tempDiff;
                    }
                    if (outputs[5] == 0.4f)
                    {
                        float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 5).GetValue() - 0.4f;
                        if (tempDiff > biggest4diff)
                            biggest4diff = tempDiff;
                    }
                }

                outputs = new float[] { 0.4f, 0.4f, 0.4f, 0.4f, 0.4f, 0.4f };

                inputs[piece0spots[x]] = 0;
                inputs[73] = 1;
                inputs[153] = 0;
                network.RunInputs(inputs);
                network.BackPropegate(outputs);

                {
                    if (outputs[0] == 0.4f)
                    {
                        float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 0).GetValue() - 0.4f;
                        if (tempDiff > biggest4diff)
                            biggest4diff = tempDiff;
                    }
                    if (outputs[1] == 0.4f)
                    {
                        float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 1).GetValue() - 0.4f;
                        if (tempDiff > biggest4diff)
                            biggest4diff = tempDiff;
                    }
                    if (outputs[2] == 0.4f)
                    {
                        float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 2).GetValue() - 0.4f;
                        if (tempDiff > biggest4diff)
                            biggest4diff = tempDiff;
                    }
                    if (outputs[3] == 0.4f)
                    {
                        float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 3).GetValue() - 0.4f;
                        if (tempDiff > biggest4diff)
                            biggest4diff = tempDiff;
                    }
                    if (outputs[4] == 0.4f)
                    {
                        float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 4).GetValue() - 0.4f;
                        if (tempDiff > biggest4diff)
                            biggest4diff = tempDiff;
                    }
                    if (outputs[5] == 0.4f)
                    {
                        float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 5).GetValue() - 0.4f;
                        if (tempDiff > biggest4diff)
                            biggest4diff = tempDiff;
                    }
                }

                outputs = new float[] { 0.4f, 0.4f, 0.4f, 0.4f, 0.4f, 0.4f };

                inputs[piece0spots[x]] = 1;
                inputs[73] = 1;
                inputs[153] = 0;
                network.RunInputs(inputs);
                network.BackPropegate(outputs);

                {
                    if (outputs[0] == 0.4f)
                    {
                        float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 0).GetValue() - 0.4f;
                        if (tempDiff > biggest4diff)
                            biggest4diff = tempDiff;
                    }
                    if (outputs[1] == 0.4f)
                    {
                        float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 1).GetValue() - 0.4f;
                        if (tempDiff > biggest4diff)
                            biggest4diff = tempDiff;
                    }
                    if (outputs[2] == 0.4f)
                    {
                        float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 2).GetValue() - 0.4f;
                        if (tempDiff > biggest4diff)
                            biggest4diff = tempDiff;
                    }
                    if (outputs[3] == 0.4f)
                    {
                        float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 3).GetValue() - 0.4f;
                        if (tempDiff > biggest4diff)
                            biggest4diff = tempDiff;
                    }
                    if (outputs[4] == 0.4f)
                    {
                        float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 4).GetValue() - 0.4f;
                        if (tempDiff > biggest4diff)
                            biggest4diff = tempDiff;
                    }
                    if (outputs[5] == 0.4f)
                    {
                        float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 5).GetValue() - 0.4f;
                        if (tempDiff > biggest4diff)
                            biggest4diff = tempDiff;
                    }
                }

                outputs = new float[] { 0.4f, 0.4f, 0.4f, 0.4f, 0.4f, 0.4f };

                inputs[piece0spots[x]] = 0;
                inputs[73] = 0;
                inputs[153] = 1;
                network.RunInputs(inputs);
                network.BackPropegate(outputs);

                {
                    if (outputs[0] == 0.4f)
                    {
                        float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 0).GetValue() - 0.4f;
                        if (tempDiff > biggest4diff)
                            biggest4diff = tempDiff;
                    }
                    if (outputs[1] == 0.4f)
                    {
                        float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 1).GetValue() - 0.4f;
                        if (tempDiff > biggest4diff)
                            biggest4diff = tempDiff;
                    }
                    if (outputs[2] == 0.4f)
                    {
                        float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 2).GetValue() - 0.4f;
                        if (tempDiff > biggest4diff)
                            biggest4diff = tempDiff;
                    }
                    if (outputs[3] == 0.4f)
                    {
                        float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 3).GetValue() - 0.4f;
                        if (tempDiff > biggest4diff)
                            biggest4diff = tempDiff;
                    }
                    if (outputs[4] == 0.4f)
                    {
                        float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 4).GetValue() - 0.4f;
                        if (tempDiff > biggest4diff)
                            biggest4diff = tempDiff;
                    }
                    if (outputs[5] == 0.4f)
                    {
                        float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 5).GetValue() - 0.4f;
                        if (tempDiff > biggest4diff)
                            biggest4diff = tempDiff;
                    }
                }
                {
                    if (outputs[0] == 1)
                    {
                        float tempDiff = 1 - network.GetNeuron(network.GetSize().Length - 1, 0).GetValue();
                        if (tempDiff > biggest1diff)
                            biggest1diff = tempDiff;
                    }
                    if (outputs[1] == 1)
                    {
                        float tempDiff = 1 - network.GetNeuron(network.GetSize().Length - 1, 1).GetValue();
                        if (tempDiff > biggest1diff)
                            biggest1diff = tempDiff;
                    }
                    if (outputs[2] == 1)
                    {
                        float tempDiff = 1 - network.GetNeuron(network.GetSize().Length - 1, 2).GetValue();
                        if (tempDiff > biggest1diff)
                            biggest1diff = tempDiff;
                    }
                    if (outputs[3] == 1)
                    {
                        float tempDiff = 1 - network.GetNeuron(network.GetSize().Length - 1, 3).GetValue();
                        if (tempDiff > biggest1diff)
                            biggest1diff = tempDiff;
                    }
                    if (outputs[4] == 1)
                    {
                        float tempDiff = 1 - network.GetNeuron(network.GetSize().Length - 1, 4).GetValue();
                        if (tempDiff > biggest1diff)
                            biggest1diff = tempDiff;
                    }
                    if (outputs[5] == 1)
                    {
                        float tempDiff = 1 - network.GetNeuron(network.GetSize().Length - 1, 5).GetValue();
                        if (tempDiff > biggest1diff)
                            biggest1diff = tempDiff;
                    }
                }

                outputs = new float[] { 0.4f, 0.4f, 0.4f, 0.4f, 1f, 0.4f };

                inputs[piece0spots[x]] = 1;
                inputs[73] = 0;
                inputs[153] = 1;
                network.RunInputs(inputs);
                network.BackPropegate(outputs);

                {
                    if (outputs[0] == 0.4f)
                    {
                        float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 0).GetValue() - 0.4f;
                        if (tempDiff > biggest4diff)
                            biggest4diff = tempDiff;
                    }
                    if (outputs[1] == 0.4f)
                    {
                        float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 1).GetValue() - 0.4f;
                        if (tempDiff > biggest4diff)
                            biggest4diff = tempDiff;
                    }
                    if (outputs[2] == 0.4f)
                    {
                        float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 2).GetValue() - 0.4f;
                        if (tempDiff > biggest4diff)
                            biggest4diff = tempDiff;
                    }
                    if (outputs[3] == 0.4f)
                    {
                        float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 3).GetValue() - 0.4f;
                        if (tempDiff > biggest4diff)
                            biggest4diff = tempDiff;
                    }
                    if (outputs[4] == 0.4f)
                    {
                        float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 4).GetValue() - 0.4f;
                        if (tempDiff > biggest4diff)
                            biggest4diff = tempDiff;
                    }
                    if (outputs[5] == 0.4f)
                    {
                        float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 5).GetValue() - 0.4f;
                        if (tempDiff > biggest4diff)
                            biggest4diff = tempDiff;
                    }
                }
                {
                    if (outputs[0] == 1)
                    {
                        float tempDiff = 1 - network.GetNeuron(network.GetSize().Length - 1, 0).GetValue();
                        if (tempDiff > biggest1diff)
                            biggest1diff = tempDiff;
                    }
                    if (outputs[1] == 1)
                    {
                        float tempDiff = 1 - network.GetNeuron(network.GetSize().Length - 1, 1).GetValue();
                        if (tempDiff > biggest1diff)
                            biggest1diff = tempDiff;
                    }
                    if (outputs[2] == 1)
                    {
                        float tempDiff = 1 - network.GetNeuron(network.GetSize().Length - 1, 2).GetValue();
                        if (tempDiff > biggest1diff)
                            biggest1diff = tempDiff;
                    }
                    if (outputs[3] == 1)
                    {
                        float tempDiff = 1 - network.GetNeuron(network.GetSize().Length - 1, 3).GetValue();
                        if (tempDiff > biggest1diff)
                            biggest1diff = tempDiff;
                    }
                    if (outputs[4] == 1)
                    {
                        float tempDiff = 1 - network.GetNeuron(network.GetSize().Length - 1, 4).GetValue();
                        if (tempDiff > biggest1diff)
                            biggest1diff = tempDiff;
                    }
                    if (outputs[5] == 1)
                    {
                        float tempDiff = 1 - network.GetNeuron(network.GetSize().Length - 1, 5).GetValue();
                        if (tempDiff > biggest1diff)
                            biggest1diff = tempDiff;
                    }
                }

                outputs = new float[] { 0.4f, 0.4f, 0.4f, 0.4f, 0.4f, 0.4f };

                inputs[piece0spots[x]] = 0;
                inputs[73] = 1;
                inputs[153] = 1;
                network.RunInputs(inputs);
                network.BackPropegate(outputs);

                {
                    if (outputs[0] == 0.4f)
                    {
                        float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 0).GetValue() - 0.4f;
                        if (tempDiff > biggest4diff)
                            biggest4diff = tempDiff;
                    }
                    if (outputs[1] == 0.4f)
                    {
                        float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 1).GetValue() - 0.4f;
                        if (tempDiff > biggest4diff)
                            biggest4diff = tempDiff;
                    }
                    if (outputs[2] == 0.4f)
                    {
                        float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 2).GetValue() - 0.4f;
                        if (tempDiff > biggest4diff)
                            biggest4diff = tempDiff;
                    }
                    if (outputs[3] == 0.4f)
                    {
                        float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 3).GetValue() - 0.4f;
                        if (tempDiff > biggest4diff)
                            biggest4diff = tempDiff;
                    }
                    if (outputs[4] == 0.4f)
                    {
                        float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 4).GetValue() - 0.4f;
                        if (tempDiff > biggest4diff)
                            biggest4diff = tempDiff;
                    }
                    if (outputs[5] == 0.4f)
                    {
                        float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 5).GetValue() - 0.4f;
                        if (tempDiff > biggest4diff)
                            biggest4diff = tempDiff;
                    }
                }
                {
                    if (outputs[0] == 1)
                    {
                        float tempDiff = 1 - network.GetNeuron(network.GetSize().Length - 1, 0).GetValue();
                        if (tempDiff > biggest1diff)
                            biggest1diff = tempDiff;
                    }
                    if (outputs[1] == 1)
                    {
                        float tempDiff = 1 - network.GetNeuron(network.GetSize().Length - 1, 1).GetValue();
                        if (tempDiff > biggest1diff)
                            biggest1diff = tempDiff;
                    }
                    if (outputs[2] == 1)
                    {
                        float tempDiff = 1 - network.GetNeuron(network.GetSize().Length - 1, 2).GetValue();
                        if (tempDiff > biggest1diff)
                            biggest1diff = tempDiff;
                    }
                    if (outputs[3] == 1)
                    {
                        float tempDiff = 1 - network.GetNeuron(network.GetSize().Length - 1, 3).GetValue();
                        if (tempDiff > biggest1diff)
                            biggest1diff = tempDiff;
                    }
                    if (outputs[4] == 1)
                    {
                        float tempDiff = 1 - network.GetNeuron(network.GetSize().Length - 1, 4).GetValue();
                        if (tempDiff > biggest1diff)
                            biggest1diff = tempDiff;
                    }
                    if (outputs[5] == 1)
                    {
                        float tempDiff = 1 - network.GetNeuron(network.GetSize().Length - 1, 5).GetValue();
                        if (tempDiff > biggest1diff)
                            biggest1diff = tempDiff;
                    }
                }

                outputs = new float[] { 0.4f, 0.4f, 0.4f, 1f, 0.4f, 0.4f };

                inputs[piece0spots[x]] = 1;
                inputs[73] = 1;
                inputs[153] = 1;



                network.RunInputs(inputs);
                network.BackPropegate(outputs);

                Debug.Log("(" + network.GetNeuron(network.GetSize().Length - 1, 0).GetValue() + ", "
                                + network.GetNeuron(network.GetSize().Length - 1, 1).GetValue() + ", "
                                + network.GetNeuron(network.GetSize().Length - 1, 2).GetValue() + ", "
                                + network.GetNeuron(network.GetSize().Length - 1, 3).GetValue() + ", "
                                + network.GetNeuron(network.GetSize().Length - 1, 4).GetValue() + ", "
                                + network.GetNeuron(network.GetSize().Length - 1, 5).GetValue() + ")"); ;

                {
                    if (outputs[0] == 0.4f)
                    {
                        float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 0).GetValue() - 0.4f;
                        if (tempDiff > biggest4diff)
                            biggest4diff = tempDiff;
                    }
                    if (outputs[1] == 0.4f)
                    {
                        float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 1).GetValue() - 0.4f;
                        if (tempDiff > biggest4diff)
                            biggest4diff = tempDiff;
                    }
                    if (outputs[2] == 0.4f)
                    {
                        float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 2).GetValue() - 0.4f;
                        if (tempDiff > biggest4diff)
                            biggest4diff = tempDiff;
                    }
                    if (outputs[3] == 0.4f)
                    {
                        float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 3).GetValue() - 0.4f;
                        if (tempDiff > biggest4diff)
                            biggest4diff = tempDiff;
                    }
                    if (outputs[4] == 0.4f)
                    {
                        float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 4).GetValue() - 0.4f;
                        if (tempDiff > biggest4diff)
                            biggest4diff = tempDiff;
                    }
                    if (outputs[5] == 0.4f)
                    {
                        float tempDiff = network.GetNeuron(network.GetSize().Length - 1, 5).GetValue() - 0.4f;
                        if (tempDiff > biggest4diff)
                            biggest4diff = tempDiff;
                    }
                }
                {
                    if (outputs[0] == 1)
                    {
                        float tempDiff = 1 - network.GetNeuron(network.GetSize().Length - 1, 0).GetValue();
                        if (tempDiff > biggest1diff)
                            biggest1diff = tempDiff;
                    }
                    if (outputs[1] == 1)
                    {
                        float tempDiff = 1 - network.GetNeuron(network.GetSize().Length - 1, 1).GetValue();
                        if (tempDiff > biggest1diff)
                            biggest1diff = tempDiff;
                    }
                    if (outputs[2] == 1)
                    {
                        float tempDiff = 1 - network.GetNeuron(network.GetSize().Length - 1, 2).GetValue();
                        if (tempDiff > biggest1diff)
                            biggest1diff = tempDiff;
                    }
                    if (outputs[3] == 1)
                    {
                        float tempDiff = 1 - network.GetNeuron(network.GetSize().Length - 1, 3).GetValue();
                        if (tempDiff > biggest1diff)
                            biggest1diff = tempDiff;
                    }
                    if (outputs[4] == 1)
                    {
                        float tempDiff = 1 - network.GetNeuron(network.GetSize().Length - 1, 4).GetValue();
                        if (tempDiff > biggest1diff)
                            biggest1diff = tempDiff;
                    }
                    if (outputs[5] == 1)
                    {
                        float tempDiff = 1 - network.GetNeuron(network.GetSize().Length - 1, 5).GetValue();
                        if (tempDiff > biggest1diff)
                            biggest1diff = tempDiff;
                    }
                }
                outputs = new float[] { 0.4f, 0.4f, 0.4f, 0.4f, 0.4f, 0.4f };
            }
        }
        Debug.Log(nn6_4.GetConnection(0, 0, 1, 0).GetValue() + "(0, 0, 1, 0)");
        Debug.Log(nn6_4.GetConnection(0, 0, 1, 1).GetValue() + "(0, 0, 1, 1)");
        Debug.Log(nn6_4.GetConnection(0, 153, 1, 0).GetValue() + "(0, 153, 1, 0)");
        Debug.Log(nn6_4.GetConnection(0, 153, 1, 1).GetValue() + "(0, 153, 1, 1)");
        Debug.Log(nn6_4.GetConnection(0, 154, 1, 0).GetValue() + "(0, 154, 1, 0)");
        Debug.Log(nn6_4.GetConnection(0, 154, 1, 1).GetValue() + "(0, 154, 1, 1)");

        Debug.Log(nn6_4.GetConnection(1, 0, 2, 0).GetValue() + "(1, 0, 2, 0)");
        Debug.Log(nn6_4.GetConnection(1, 0, 2, 1).GetValue() + "(1, 0, 2, 1)");
        Debug.Log(nn6_4.GetConnection(1, 1, 2, 0).GetValue() + "(1, 1, 2, 0)");
        Debug.Log(nn6_4.GetConnection(1, 1, 2, 1).GetValue() + "(1, 1, 2, 1)");
        Debug.Log(nn6_4.GetConnection(1, 2, 2, 0).GetValue() + "(1, 2, 2, 0)");
        Debug.Log(nn6_4.GetConnection(1, 2, 2, 1).GetValue() + "(1, 2, 2, 1)");

        Debug.Log(nn6_4.GetConnection(0, 73, 2, 0).GetValue() + "(0, 73, 2, 0)");
        Debug.Log(nn6_4.GetConnection(0, 73, 2, 1).GetValue() + "(0, 73, 2, 1)");

        Debug.Log(nn6_4.GetConnection(2, 0, 3, 3).GetValue() + "(2, 0, 3, 3)");
        Debug.Log(nn6_4.GetConnection(2, 1, 3, 3).GetValue() + "(2, 1, 3, 3)");
        Debug.Log(nn6_4.GetConnection(2, 2, 3, 3).GetValue() + "(2, 2, 3, 3)");

        Debug.Log(nn6_4.GetConnection(2, 0, 3, 4).GetValue() + "(2, 0, 3, 4)");
        Debug.Log(nn6_4.GetConnection(2, 1, 3, 4).GetValue() + "(2, 1, 3, 4)");
        Debug.Log(nn6_4.GetConnection(2, 2, 3, 4).GetValue() + "(2, 2, 3, 4)");

        Debug.Log(nn6_4.GetConnection(2, 2, 3, 0).GetValue() + "(2, 2, 3, 0)");
        Debug.Log(nn6_4.GetConnection(2, 2, 3, 1).GetValue() + "(2, 2, 3, 1)");
        Debug.Log(nn6_4.GetConnection(2, 2, 3, 2).GetValue() + "(2, 2, 3, 2)");
        Debug.Log(nn6_4.GetConnection(2, 2, 3, 5).GetValue() + "(2, 2, 3, 5)");


        Debug.Log("1.0:" + (1 - biggest1diff) + "|||0.4:" + (0.4 + biggest4diff));
        Debug.Log("out of 1 n6_3 correctly chose : " + rightChoices);
        network.AddDesiredChanges(0.99f);
    }
    public void MakeGoodNN6_4()
    {
        nn6_4 = new NeuralNetwork();
        nn6_4.AddCollumn(0);
        nn6_4.AddCollumn(1);
        nn6_4.AddCollumn(2);
        nn6_4.AddCollumn(3);
        int x;
        for (x = 0; x < 155; x++)//3 + bias node
        {
            nn6_4.AddNeuron(0, 0);
        }
        for (x = 0; x < 3; x++)
        {
            nn6_4.AddNeuron(1, 0);
        }
        for (x = 0; x < 3; x++)
        {
            nn6_4.AddNeuron(2, 0);
        }
        for (x = 0; x < 6; x++)
        {
            nn6_4.AddNeuron(3, 0);
        }
        nn6_4.AddConnection(0, 0, 1, 0, Random.Range(-1f, 1f));
        nn6_4.AddConnection(0, 0, 1, 1, Random.Range(-1f, 1f));
        nn6_4.AddConnection(0, 153, 1, 0, Random.Range(-1f, 1f));
        nn6_4.AddConnection(0, 153, 1, 1, Random.Range(-1f, 1f));
        nn6_4.AddConnection(0, 154, 1, 0, Random.Range(-1f, 1f));
        nn6_4.AddConnection(0, 154, 1, 1, Random.Range(-1f, 1f));

        nn6_4.AddConnection(1, 0, 2, 0, Random.Range(-1f, 1f));
        nn6_4.AddConnection(1, 0, 2, 1, Random.Range(-1f, 1f));
        nn6_4.AddConnection(1, 1, 2, 0, Random.Range(-1f, 1f));
        nn6_4.AddConnection(1, 1, 2, 1, Random.Range(-1f, 1f));
        nn6_4.AddConnection(1, 2, 2, 0, Random.Range(-1f, 1f));
        nn6_4.AddConnection(1, 2, 2, 1, Random.Range(-1f, 1f));

        //the second and with p3,p4,p5,p6 : 10
        nn6_4.AddConnection(0, 63 + 10, 2, 0, Random.Range(-1f, 1f));
        nn6_4.AddConnection(0, 63 + 10, 2, 1, Random.Range(-1f, 1f));


        nn6_4.AddConnection(2, 0, 3, 3, Random.Range(-1f, 1f));
        nn6_4.AddConnection(2, 1, 3, 3, Random.Range(-1f, 1f));
        nn6_4.AddConnection(2, 2, 3, 3, Random.Range(-1f, 1f));

        nn6_4.AddConnection(2, 0, 3, 4, Random.Range(-1f, 1f));
        nn6_4.AddConnection(2, 1, 3, 4, Random.Range(-1f, 1f));
        nn6_4.AddConnection(2, 2, 3, 4, Random.Range(-1f, 1f));


        nn6_4.AddConnection(2, 2, 3, 0, Random.Range(-1f, 1f));
        nn6_4.AddConnection(2, 2, 3, 1, Random.Range(-1f, 1f));
        nn6_4.AddConnection(2, 2, 3, 2, Random.Range(-1f, 1f));
        nn6_4.AddConnection(2, 2, 3, 5, Random.Range(-1f, 1f));
    }

    //4 and 5 should be the same as 3

    public void MakeCheckpointNN()
    {
        nnCheckpoint = new NeuralNetwork();
        nnCheckpoint.AddCollumn(0);
        nnCheckpoint.AddCollumn(1);
        nnCheckpoint.AddCollumn(2);
        nnCheckpoint.AddCollumn(3);
        nnCheckpoint.AddCollumn(4);
        int x;
        for (x = 0; x < 155; x++)//3 + bias node
        {
            nnCheckpoint.AddNeuron(0, 0);
        }
        for (x = 0; x < 13; x++)
        {
            nnCheckpoint.AddNeuron(4, 0);
        }
        //cp0
        nnCheckpoint.AddConnection(0, 0 + 0, 4, 6, 10 / 7.5f);
        nnCheckpoint.AddConnection(0, 21 + 3, 4, 6, 10 / 7.5f);
        nnCheckpoint.AddConnection(0, 42 + 6, 4, 6, 10 / 7.5f);
        nnCheckpoint.AddConnection(0, 63 + 9, 4, 6, 10 / 7.5f);
        nnCheckpoint.AddConnection(0, 84 + 12, 4, 6, 10 / 7.5f);
        nnCheckpoint.AddConnection(0, 105 + 15, 4, 6, 10 / 7.5f);
        nnCheckpoint.AddConnection(0, 126 + 18, 4, 6, 10 / 7.5f);
        nnCheckpoint.AddConnection(0, 153, 4, 6, 10 / 7.5f);//cp6
        //cp1
        nnCheckpoint.AddConnection(0, 42 + 6, 4, 7, 10 / 1.5f);
        nnCheckpoint.AddConnection(0, 147, 4, 7, 10 / 1.5f);//cp0
        //cp2
        nnCheckpoint.AddConnection(0, 42 + 6, 4, 8, 10 / 2.5f);
        nnCheckpoint.AddConnection(0, 0, 4, 8, 10 / 2.5f);
        nnCheckpoint.AddConnection(0, 148, 4, 8, 10 / 2.5f);//cp1
        //cp3
        nnCheckpoint.AddConnection(0, 42 + 6, 4, 9, 10 / 3.5f);
        nnCheckpoint.AddConnection(0, 0, 4, 9, 10 / 3.5f);
        nnCheckpoint.AddConnection(0, 21 + 3, 4, 9, 10 / 3.5f);
        nnCheckpoint.AddConnection(0, 149, 4, 9, 10 / 3.5f);//cp2
        //cp4
        nnCheckpoint.AddConnection(0, 42 + 6, 4, 10, 10 / 4.5f);
        nnCheckpoint.AddConnection(0, 0, 4, 10, 10 / 4.5f);
        nnCheckpoint.AddConnection(0, 21 + 3, 4, 10, 10 / 4.5f);
        nnCheckpoint.AddConnection(0, 126 + 18, 4, 10, 10 / 4.5f);//orientaion doesn't matter
        nnCheckpoint.AddConnection(0, 126 + 19, 4, 10, 10 / 4.5f);//orientaion doesn't matter
        nnCheckpoint.AddConnection(0, 126 + 20, 4, 10, 10 / 4.5f);//orientaion doesn't matter
        nnCheckpoint.AddConnection(0, 150, 4, 10, 10 / 4.5f);//cp3
        //cp5
        nnCheckpoint.AddConnection(0, 42 + 6, 4, 11, 10 / 5.5f);
        nnCheckpoint.AddConnection(0, 0, 4, 11, 10 / 5.5f);
        nnCheckpoint.AddConnection(0, 21 + 3, 4, 11, 10 / 5.5f);
        nnCheckpoint.AddConnection(0, 126 + 18, 4, 11, 10 / 5.5f);//orientaion doesn't matter
        nnCheckpoint.AddConnection(0, 126 + 19, 4, 11, 10 / 5.5f);//orientaion doesn't matter
        nnCheckpoint.AddConnection(0, 126 + 20, 4, 11, 10 / 5.5f);//orientaion doesn't matter
        nnCheckpoint.AddConnection(0, 105 + 15, 4, 11, 10 / 5.5f);//orientaion doesn't matter
        nnCheckpoint.AddConnection(0, 105 + 16, 4, 11, 10 / 5.5f);//orientaion doesn't matter
        nnCheckpoint.AddConnection(0, 105 + 17, 4, 11, 10 / 5.5f);//orientaion doesn't matter
        nnCheckpoint.AddConnection(0, 151, 4, 11, 10 / 5.5f);//cp4
        //cp6
        nnCheckpoint.AddConnection(0, 42 + 6, 4, 12, 10 / 7.5f);
        nnCheckpoint.AddConnection(0, 0, 4, 12, 10 / 7.5f);
        nnCheckpoint.AddConnection(0, 21 + 3, 4, 12, 10 / 7.5f);
        nnCheckpoint.AddConnection(0, 63 + 9, 4, 12, 10 / 7.5f);//orientaion doesn't matter
        nnCheckpoint.AddConnection(0, 63 + 10, 4, 12, 10 / 7.5f);//orientaion doesn't matter
        nnCheckpoint.AddConnection(0, 63 + 11, 4, 12, 10 / 7.5f);//orientaion doesn't matter
        nnCheckpoint.AddConnection(0, 84 + 12, 4, 12, 10 / 7.5f);//orientaion doesn't matter
        nnCheckpoint.AddConnection(0, 84 + 13, 4, 12, 10 / 7.5f);//orientaion doesn't matter
        nnCheckpoint.AddConnection(0, 84 + 14, 4, 12, 10 / 7.5f);//orientaion doesn't matter
        nnCheckpoint.AddConnection(0, 105 + 15, 4, 12, 10 / 7.5f);//orientaion doesn't matter
        nnCheckpoint.AddConnection(0, 105 + 16, 4, 12, 10 / 7.5f);//orientaion doesn't matter
        nnCheckpoint.AddConnection(0, 105 + 17, 4, 12, 10 / 7.5f);//orientaion doesn't matter
        nnCheckpoint.AddConnection(0, 126 + 18, 4, 12, 10 / 7.5f);//orientaion doesn't matter
        nnCheckpoint.AddConnection(0, 126 + 19, 4, 12, 10 / 7.5f);//orientaion doesn't matter
        nnCheckpoint.AddConnection(0, 126 + 20, 4, 12, 10 / 7.5f);//orientaion doesn't matter

        //offset set
        nnCheckpoint.AddConnection(0, 126 + 12, 4, 12, 10 / 7.5f);//orientaion doesn't matter
        nnCheckpoint.AddConnection(0, 126 + 13, 4, 12, 10 / 7.5f);//orientaion doesn't matter
        nnCheckpoint.AddConnection(0, 126 + 14, 4, 12, 10 / 7.5f);//orientaion doesn't matter
        nnCheckpoint.AddConnection(0, 105 + 18, 4, 12, 10 / 7.5f);//orientaion doesn't matter
        nnCheckpoint.AddConnection(0, 105 + 19, 4, 12, 10 / 7.5f);//orientaion doesn't matter
        nnCheckpoint.AddConnection(0, 105 + 20, 4, 12, 10 / 7.5f);//orientaion doesn't matter
        nnCheckpoint.AddConnection(0, 63 + 15, 4, 12, 10 / 7.5f);//orientaion doesn't matter
        nnCheckpoint.AddConnection(0, 63 + 16, 4, 12, 10 / 7.5f);//orientaion doesn't matter
        nnCheckpoint.AddConnection(0, 63 + 17, 4, 12, 10 / 7.5f);//orientaion doesn't matter
        nnCheckpoint.AddConnection(0, 84 + 9, 4, 12, 10 / 7.5f);//orientaion doesn't matter
        nnCheckpoint.AddConnection(0, 84 + 10, 4, 12, 10 / 7.5f);//orientaion doesn't matter
        nnCheckpoint.AddConnection(0, 84 + 11, 4, 12, 10 / 7.5f);//orientaion doesn't matter

        nnCheckpoint.AddConnection(0, 152, 4, 12, 10 / 7.5f);//cp5
    }



    public void ChangeScene(string newScene)
    {
        SceneManager.LoadScene(newScene);
    }
    public void LoadNN(int loadIndex)
    {
        //loads from player prefs or makes new one
        training = false;
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
            nnDisplay = GetComponent<Holder>().GetNeuralNetworkTrained(loadIndex);
            if (nnDisplay == null)
            {
                nnDisplay = MakeGoodNN(loadIndex);
                string str = this.GetComponent<NeuralNetworkSave>().ToStringNN(nnDisplay, loadIndex);
                PlayerPrefs.SetString("" + loadIndex, str);
            }
            building = true;
            buildPercent = 0;
            display.GetComponent<NeuralNetworkDisplay>().ChangeNN(nnDisplay);
            display.GetComponent<NeuralNetworkDisplay>().SetupCreateNNDisplay();
            buildAmmount = nnDisplay.TrueSize();
        }
    }
    public NeuralNetwork MakeGoodNN(int index)
    {
        switch (index)
        {
            case 2220:
                activeID = 2220;
                MakeGoodNN0();
                return nn0;
            case 2221:
                activeID = 2221;
                MakeGoodNN1();
                return nn1;
            case 22220:
                activeID = 22220;
                MakeGoodNN2_0();
                return nn2_0;
            case 22221:
                activeID = 22221;
                MakeGoodNN2_1();
                return nn2_1;
            case 2223:
                activeID = 2223;
                MakeGoodNN3();
                return nn3;
            case 22240:
                activeID = 22240;
                MakeGoodNN4_0();
                return nn4_0;
            case 22241:
                activeID = 22241;
                MakeGoodNN4_1();
                return nn4_1;
            case 22242:
                activeID = 22242;
                MakeGoodNN4_2();
                return nn4_2;
            case 2225:
                activeID = 2225;
                MakeGoodNN5();
                return nn5;
            case 22260:
                activeID = 22260;
                MakeGoodNN6_0();
                return nn6_0;
            case 22261:
                activeID = 22261;
                MakeGoodNN6_1();
                return nn6_1;
            case 22262:
                activeID = 22262;
                MakeGoodNN6_2();
                return nn6_2;
            case 22263:
                activeID = 22263;
                MakeGoodNN6_3();
                return nn6_3;
            case 22264:
                activeID = 22264;
                MakeGoodNN6_4();
                return nn6_4;
            case 222000:
                activeID = 0;
                MakeCheckpointNN();
                return nnCheckpoint;
        }
        Debug.Log("MakeGoodNN " + index + " isnull");
        return null;
    }
}

