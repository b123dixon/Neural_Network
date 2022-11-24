using UnityEngine;

public class NeuralNetworkSave : MonoBehaviour
{
    // this is used to save and load neural networks
    //n = neuron c = connection
    //player prefs: nSize, cSize, nRow, nColl, cLeftRow, cLeftColl, cRightRow, cRightColl, cValue
    //key is always "index|..."
    //also has a toString for saving not in player prefs faster and transferable saves
    //i had no idea how slow player prefs would be
    NeuralNetwork nn;
    string str;
    int nSize;
    int cSize;
    int nPos;
    int cPos;




    public void SaveNeuralNetwork(NeuralNetwork nn, int index)
    {
        string str = "" + index + "|";
        if (PlayerPrefs.HasKey(str + "nSize"))
        {
            DeleteNeuralNetwork(index);
        }
        Neuron dummy = nn.GetDummyHead();
        Neuron curr;
        int nSize = 0, cSize = 0; // to keep track of the two sizes
        while (dummy != null)
        {
            curr = dummy;
            while (curr != null)
            {
                //neuron
                PlayerPrefs.SetInt(str + "nRow" + nSize, curr.GetRow());
                PlayerPrefs.SetInt(str + "nColl" + nSize, curr.GetCollumn());

                //connection
                Connection currConn = curr.GetRightConnections().GetHead();
                while (currConn != null)
                {
                    PlayerPrefs.SetInt(str + "cLeftRow" + cSize, currConn.GetLeftNeuron().GetRow());
                    PlayerPrefs.SetInt(str + "cLeftColl" + cSize, currConn.GetLeftNeuron().GetCollumn());
                    PlayerPrefs.SetInt(str + "cRightRow" + cSize, currConn.GetRightNeuron().GetRow());
                    PlayerPrefs.SetInt(str + "cRightColl" + cSize, currConn.GetRightNeuron().GetCollumn());
                    PlayerPrefs.SetFloat(str + "cValue" + cSize, currConn.GetValue());
                    cSize++;
                    currConn = currConn.GetNext(true);
                }
                nSize++;
                curr = curr.GetBelow();
            }
            dummy = dummy.GetNext();
        }
        PlayerPrefs.SetInt(str + "cSize", cSize);
        PlayerPrefs.SetInt(str + "nSize", nSize);

    }
    public void DeleteNeuralNetwork(int index)
    {
        string str = "" + index + "|";
        if (!PlayerPrefs.HasKey(str + "nSize"))
        {
            return;
        }
        int nSize = PlayerPrefs.GetInt(str + "nSize");
        int cSize = PlayerPrefs.GetInt(str + "cSize");
        PlayerPrefs.DeleteKey(str + "nSize");
        PlayerPrefs.DeleteKey(str + "cSize");


        int x;
        for (x = 0; x < nSize; x++)
        {

        }
    }
    public NeuralNetwork LoadNeuralNetwork(int index)
    {
        NeuralNetwork nn = GetComponent<Holder>().GetNeuralNetwork(index);
        return nn;
    }

    public int PrepLoad(int index)
    {
        //gets ready for slow load
        //1 checks if it is real
        //2 grabs the sizes for neurons and connections
        //3 sets positions to 0
        //4 creates empty neural network
        str = "" + index + "|";
        if (!PlayerPrefs.HasKey(str + "nSize"))
        {
            return 0;// this nn doesnt exist
        }
        nSize = PlayerPrefs.GetInt(str + "nSize");
        cSize = PlayerPrefs.GetInt(str + "cSize");

        nPos = 0;
        cPos = 0;

        nn = new NeuralNetwork();
        return nSize + cSize; // the nn does exist if this returns 0
    }
    public NeuralNetwork LoadNeuralNetwork(int index, int amount)
    {
        //loads a chunk of the neural network by the given ammount

        while (amount > 0)// first neurons then connections
        {
            if (nPos < nSize)//neurons will be all loaded if this is false
            {
                int row = PlayerPrefs.GetInt(str + "nRow" + nPos);
                int coll = PlayerPrefs.GetInt(str + "nColl" + nPos);
                if (row == -1)
                {
                    nn.AddCollumn(coll);
                }
                else
                {
                    nn.AddNeuron(coll, row);
                }
                nPos++;
            }
            else if (cPos < cSize)//connections will all be done when this is false
            {
                int leftRow, leftColl, rightRow, rightColl;
                float value;
                leftRow = PlayerPrefs.GetInt(str + "cLeftRow" + cPos);
                leftColl = PlayerPrefs.GetInt(str + "cLeftColl" + cPos);
                rightRow = PlayerPrefs.GetInt(str + "cRightRow" + cPos);
                rightColl = PlayerPrefs.GetInt(str + "cRightColl" + cPos);
                value = PlayerPrefs.GetFloat(str + "cValue" + cPos);
                nn.AddConnection(leftColl, leftRow, rightColl, rightRow, value);
                cPos++;
            }
            else //yay, everything is done
            {
                return nn;
            }
            amount--;
        }
        //if we made it here then nn is not loaded 
        return null;
    }



    public void ToString(int index)
    {
        string str2 = "|";


        string str = "" + index + "|";
        if (!PlayerPrefs.HasKey(str + "nSize"))
        {
            return;
        }
        int nSize = PlayerPrefs.GetInt(str + "nSize");
        int cSize = PlayerPrefs.GetInt(str + "cSize");

        str2 = str2 + nSize + "|" + cSize + "|";

        //insert neurons
        int x;
        int row, coll;
        for (x = 0; x < nSize; x++)
        {
            row = PlayerPrefs.GetInt(str + "nRow" + x);
            coll = PlayerPrefs.GetInt(str + "nColl" + x);

            if (x != 0)
                str2 = str2 + "*";
            str2 = str2 + row + "_" + coll;
        }
        str2 = str2 + "|";
        //insert connections
        int leftRow, leftColl, rightRow, rightColl;
        float value;
        for (x = 0; x < cSize; x++)
        {


            leftRow = PlayerPrefs.GetInt(str + "cLeftRow" + x);
            leftColl = PlayerPrefs.GetInt(str + "cLeftColl" + x);
            rightRow = PlayerPrefs.GetInt(str + "cRightRow" + x);
            rightColl = PlayerPrefs.GetInt(str + "cRightColl" + x);
            value = PlayerPrefs.GetFloat(str + "cValue" + x);

            if (x != 0)
                str2 = str2 + "*";
            str2 = str2 + leftRow + "_" + leftColl + "_" + rightRow + "_" + rightColl + "_" + value;
        }
        str2 = str2 + "|";

        GUIUtility.systemCopyBuffer = str2;


    }
    public string ToStringNN(NeuralNetwork nn, int index)
    {

        str = "";
        nSize = 0;
        cSize = 0;
        string neuStr = "";
        string conStr = "";

        Neuron dummy = nn.GetDummyHead();
        Neuron curr;
        while (dummy != null)
        {
            curr = dummy;
            while (curr != null)
            {
                //neuron
                if (nSize != 0)
                    neuStr += "*";
                neuStr = neuStr + curr.GetRow() + "_" + curr.GetCollumn();


                //connection
                Connection currConn = curr.GetRightConnections().GetHead();
                while (currConn != null)
                {

                    if (cSize != 0)
                        conStr += "*";

                    conStr = conStr +
                        currConn.GetLeftNeuron().GetRow() + "_" +
                        currConn.GetLeftNeuron().GetCollumn() + "_" +
                        currConn.GetRightNeuron().GetRow() + "_" +
                        currConn.GetRightNeuron().GetCollumn() + "_" +
                        currConn.GetValue();
                    cSize++;
                    currConn = currConn.GetNext(true);
                }
                nSize++;
                curr = curr.GetBelow();
            }
            dummy = dummy.GetNext();
        }
        str = str + nSize + "|" + cSize + "|" + neuStr + "|" + conStr;
        return str;


    }
}
