using UnityEngine;
using UnityEngine.UI;

public class NeuralNetworkDisplay : MonoBehaviour
{
    //display for the Neural Network


    NeuralNetwork neuralNetwork; // The neural network to build the display after
    public bool neuralNetworkCurrentlyDisplayed;//so there is no attempt to work with gameobjects that are not existing

    //prefabs
    public GameObject dot;
    public GameObject line;
    public GameObject canvas;

    //the objects that are in the display
    public GameObject[] neurons;
    public GameObject[] connections;

    public GameObject nnBackground;
    public float canvasMaxX, canvasMaxY, canvasMinX, canvasMinY; // boundries for display
    int cSize; // the ammount of connections
    int nSize; // the ammount of neurons


    // for the loops 
    Neuron dummy;
    Neuron curr;
    Connection currConn;
    int counter; //for the slow load



    public void ChangeNN(NeuralNetwork newNN)
    {
        if (neuralNetwork != null)
        {
            DeleteNNDisplay();
        }
        neuralNetwork = newNN;
    }

    public void CreateNNDisplay()
    {
        //calculate space for NN
        float xPos = nnBackground.GetComponent<Transform>().position.x;
        float yPos = nnBackground.GetComponent<Transform>().position.y;
        float width = nnBackground.GetComponent<RectTransform>().sizeDelta[0];
        float heighth = nnBackground.GetComponent<RectTransform>().sizeDelta[1];

        canvasMaxX = xPos + (width / 2);
        canvasMinX = xPos + (width / 2);
        canvasMaxY = yPos + (heighth / 2);
        canvasMinY = yPos - (heighth / 2);


        dummy = neuralNetwork.GetDummyHead();
        cSize = 0;
        nSize = 0;

        //this is to get the sizes
        while (dummy != null)
        {
            curr = dummy;
            while (curr != null)
            {
                currConn = curr.GetRightConnections().GetHead();
                while (currConn != null)
                {
                    cSize++;
                    currConn = currConn.GetNext(true);
                }
                nSize++;
                curr = curr.GetBelow();
            }
            dummy = dummy.GetNext();
        }


        dummy = neuralNetwork.GetDummyHead();
        neurons = new GameObject[nSize];
        connections = new GameObject[cSize];

        cSize = 0;
        nSize = 0;


        while (dummy != null)
        {
            curr = dummy;
            while (curr != null)
            {
                currConn = curr.GetRightConnections().GetHead();
                while (currConn != null)
                {
                    //create connection
                    GameObject connection = Instantiate(line);
                    connections[cSize] = connection;

                    Vector3 leftPos = FindSpotVector(currConn.GetLeftNeuron());
                    Vector3 rightPos = FindSpotVector(currConn.GetRightNeuron());

                    ConfigureLine(connection, leftPos, rightPos);
                    connections[cSize].transform.SetParent(canvas.transform, false);


                    cSize++;
                    currConn = currConn.GetNext(true);
                }

                //create neuron
                GameObject neuron = Instantiate(dot);
                neurons[nSize] = neuron;
                neurons[nSize].transform.position = FindSpotVector(curr);
                neurons[nSize].transform.SetParent(canvas.transform, false);


                nSize++;

                curr = curr.GetBelow();

            }
            dummy = dummy.GetNext();
        }
    }

    public void SetupCreateNNDisplay()
    {
        float xPos = nnBackground.GetComponent<Transform>().localPosition.x;
        float yPos = nnBackground.GetComponent<Transform>().localPosition.y;
        //Debug.Log("x : " + xPos + ":::: y : " + yPos);
        float width = nnBackground.GetComponent<RectTransform>().sizeDelta.x * nnBackground.GetComponent<Transform>().localScale.x;
        float heighth = nnBackground.GetComponent<RectTransform>().sizeDelta.y * nnBackground.GetComponent<Transform>().localScale.y;
        //Debug.Log("width : " + width + ":::: heigth : " + heighth);
        canvasMaxX = (xPos + (width / 2)) * 0.9f;
        canvasMinX = (xPos - (width / 2)) * 0.9f;
        canvasMaxY = (yPos + (heighth / 2)) * 0.9f;
        canvasMinY = (yPos - (heighth / 2)) * 0.9f;

        // just gets the size and the (dummy, curr, and currConn for the loops)
        dummy = neuralNetwork.GetDummyHead();
        cSize = 0;
        nSize = 0;

        //this is to get the sizes
        while (dummy != null)
        {
            curr = dummy;
            while (curr != null)
            {
                currConn = curr.GetRightConnections().GetHead();
                while (currConn != null)
                {
                    cSize++;
                    currConn = currConn.GetNext(true);
                }
                nSize++;
                curr = curr.GetBelow();
            }
            dummy = dummy.GetNext();
        }


        dummy = neuralNetwork.GetDummyHead();
        curr = dummy;
        neurons = new GameObject[nSize];
        connections = new GameObject[cSize];

        cSize = 0;
        nSize = 0;
    }
    public bool CreateNNDisplay(int amount)
    {
        //currConn walks to the end
        //when it reaches the end curr walks forward once
        //when curr reaches the end dummy walks forward once
        //when dummy reaches the end then we are done

        while (amount > 0)
        {
            if (currConn == null)
            {
                if (curr == null)
                {
                    if (dummy.GetNext() != null)
                    {
                        dummy = dummy.GetNext();//move dummy forward
                        curr = dummy;//move curr with it
                        currConn = curr.GetRightConnections().GetHead();//move connection with it
                    }
                    else
                    {
                        return true; //we have reached the end
                    }
                }
                else
                {
                    //create the neuron then move forward
                    GameObject neuron = Instantiate(dot);
                    neurons[nSize] = neuron;
                    neurons[nSize].transform.position = FindSpotVector(curr);
                    neurons[nSize].transform.SetParent(canvas.transform, false);
                    neurons[nSize].GetComponent<Image>().color = FindColor(curr.GetValue());
                    nSize++;

                    curr = curr.GetBelow();//move forward
                    if (curr != null)
                        if (curr.GetRightConnections().GetLength() != 0)
                            currConn = curr.GetRightConnections().GetHead();//move connection with it
                }
            }
            else
            {
                //make connection and move currConn forward


                //create connection
                GameObject connection = Instantiate(line);
                connections[cSize] = connection;

                Vector3 leftPos = FindSpotVector(currConn.GetLeftNeuron());
                Vector3 rightPos = FindSpotVector(currConn.GetRightNeuron());

                ConfigureLine(connection, leftPos, rightPos);
                connections[cSize].transform.SetParent(canvas.transform, false);

                connections[cSize].GetComponent<Image>().color = FindColor(currConn.GetValue());//the color

                cSize++; //where to insert the connection
                currConn = currConn.GetNext(true);//move to next currConn
            }

            amount--;
        }
        return false;// if we made it through the loop then we are not done
    }

    public Vector3 FindSpotVector(Neuron n1)
    {
        //coll x
        //row -y
        float xPos = n1.GetCollumn();
        float yPos = n1.GetRow() + 1;

        float yRange = Mathf.Abs(canvasMaxY - canvasMinY);
        float yDistance;
        if (neuralNetwork.GetSize()[n1.GetCollumn()] == 0)
        {
            yDistance = yRange / (neuralNetwork.GetSize()[n1.GetCollumn()] + 1);
        }
        else
        {
            yDistance = yRange / neuralNetwork.GetSize()[n1.GetCollumn()];
        }
        yPos = canvasMaxY - (yPos * yDistance);

        float xRange = Mathf.Abs(canvasMaxX - canvasMinX);
        float xDistance;
        if ((neuralNetwork.GetSize().Length - 1) == 0)
        {
            xDistance = xRange / (neuralNetwork.GetSize().Length);
        }
        else
        {
            xDistance = xRange / (neuralNetwork.GetSize().Length - 1);
        }
        xPos = canvasMinX + (xPos * xDistance);

        return new Vector3(xPos, yPos, 0);
    }
    public void DeleteNNDisplay()
    {
        if (neuralNetwork.GetSize().Length == 0)
        {
            return;
        }
        Neuron dummy = neuralNetwork.GetDummyHead();
        Neuron curr;
        Connection currConn;
        nSize = 0;
        cSize = 0;
        while (dummy != null)
        {
            curr = dummy;
            while (curr != null)
            {
                currConn = curr.GetRightConnections().GetHead();
                while (currConn != null)
                {
                    Destroy(connections[cSize]);

                    cSize++;
                    currConn = currConn.GetNext(true);
                }
                Destroy(neurons[nSize]);

                nSize++;
                curr = curr.GetBelow();
            }
            dummy = dummy.GetNext();
        }
    }
    public void NNactivated()
    {
        Neuron dummy = neuralNetwork.GetDummyHead();
        Neuron curr;
        Connection currConn;
        nSize = 0;
        cSize = 0;
        while (dummy != null)
        {
            curr = dummy;
            while (curr != null)
            {
                currConn = curr.GetRightConnections().GetHead();
                while (currConn != null)
                {
                    connections[cSize].GetComponent<Image>().color = FindColor(currConn.GetValue());

                    cSize++;
                    currConn = currConn.GetNext(true);
                }
                neurons[nSize].GetComponent<Image>().color = FindColor(curr.GetValue());

                nSize++;
                curr = curr.GetBelow();
            }
            dummy = dummy.GetNext();
        }
    }
    public void NNactivatedFull()
    {
        Neuron dummy = neuralNetwork.GetDummyHead();
        Neuron curr;
        Connection currConn;
        nSize = 0;
        cSize = 0;
        while (dummy != null)
        {
            curr = dummy;
            while (curr != null)
            {
                currConn = curr.GetRightConnections().GetHead();
                while (currConn != null)
                {
                    connections[cSize].GetComponent<Image>().color = FindColor(currConn.GetValue() * currConn.GetLeftNeuron().GetValue());

                    cSize++;
                    currConn = currConn.GetNext(true);
                }
                neurons[nSize].GetComponent<Image>().color = FindColorNeuron(curr.GetValue());

                nSize++;
                curr = curr.GetBelow();
            }
            dummy = dummy.GetNext();
        }
    }

    public void ConfigureLine(GameObject line, Vector3 spot1, Vector3 spot2)
    {
        // makes the line connect the given dots
        //distance between two points formula
        float distX = Mathf.Pow(spot1.x - spot2.x, 2);
        float distY = Mathf.Pow(spot1.y - spot2.y, 2);
        float distance = Mathf.Sqrt(distX + distY);// distance is also the hypotonuse of the right triangle created with these points

        float angle = Mathf.Asin(Mathf.Abs(spot1.y - spot2.y) / distance);
        line.transform.localScale = new Vector3(distance, 3f, 1);
        line.transform.position = new Vector3((spot1.x + spot2.x) / 2, (spot1.y + spot2.y) / 2, 0);
        if (spot1.y > spot2.y)
        {
            line.transform.eulerAngles = new Vector3(0, 0, 360 - (angle * 58));
        }
        else
        {
            line.transform.eulerAngles = new Vector3(0, 0, angle * 58);
        }
    }
    public Color FindColor(float num)
    {
        //creates a color from the number (gradient)
        if (num < 0f)
        {
            num = Mathf.Abs(num);
            float red = num * 100;

            //float red = num * 127.5f;
            //return new Color((127.5f - red) / 255, (127.5f - red) / 255, (127.5f - red) / 255);

            //return new Color((155 + red) / 255, (155 - red) / 255, (155 - red) / 255);

            //return new Color((155 + red) / 255, 155f / 255f, 155f / 255f, (155f + red) / 255);

            return new Color((155 + red) / 255, 155f / 255f, 155f / 255f, (num * 40f) / 255);


        }
        else
        {
            float green = num * 100;

            //float green = num * 127.5f;
            //return new Color((127.5f + green) / 255, (127.5f + green) / 255, (127.5f + green) / 255);

            //return new Color((155 - green) / 255, (155 + green) / 255, (155 - green) / 255);

            //return new Color(155f / 255f, (155 + green) / 255f, 155f / 255f, (155 + green) / 255f);

            return new Color(155f / 255f, (155 + green) / 255f, 155f / 255f, (num * 40f) / 255);


        }
    }
    public Color FindColorNeuron(float num)
    {
        //creates a color from the number (gradient)
        if (num < 0f)
        {
            num = Mathf.Abs(num);
            float red = num * 100;

            return new Color((155 + red) / 255, 155f / 255f, 155f / 255f);
        }
        else
        {
            float green = num * 100;

            return new Color(155f / 255f, (155 + green) / 255f, 155f / 255f);
        }
    }
}
