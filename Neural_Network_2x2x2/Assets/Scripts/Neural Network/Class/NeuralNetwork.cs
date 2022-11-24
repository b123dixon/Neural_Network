using UnityEngine;

public class NeuralNetwork
{
    //the Neural Network is a 2d array of Neuron Objects with dummy head nodes
    //the neural network is a feed forward neural network that has bias nodes on all but the output layer

    private int[] size;//length is the number of collumns and size[x] is the size of that for that collumn
    Neuron dummyHead;// dummy headNode for the Neural Network


    public NeuralNetwork()
    {
        size = new int[0];
    }
    public int[] GetSize()
    {
        return size;
    }
    public Neuron GetDummyHead()
    {
        return dummyHead;
    }


    //these are for the actual learning and running of the neural network
    public void RunInputs(float[] inputs)
    {
        //this is the forward propegation of the neural network

        //checks to make sure the input layer will match the inputs
        if (inputs.Length != size[0] && inputs.Length != size[0] - 1)//minus one for the bias
        {
            Debug.Log("invalid input size for runInputs");
            return;
        }
        //we have to first set the neurons to 0
        //set all non bias and non input nodes to 0
        int x;
        Neuron dummy = dummyHead;
        while (dummy != null)
        {
            Neuron curr = dummy.GetBelow();
            while (curr != null)
            {
                if (dummy.GetCollumn() != 0)
                {
                    if (curr.GetLeftConnections().GetLength() != 0)//this is done for the bias nodes
                        curr.SetValue(0);
                }
                curr = curr.GetBelow();
            }
            dummy = dummy.GetNext();
        }


        //the actual forward pass
        x = 0;
        dummy = dummyHead;
        while (dummy != null)
        {
            Neuron curr = dummy.GetBelow();
            while (curr != null)
            {
                if (dummy.GetCollumn() == 0)//input layer
                {
                    if (x < inputs.Length)
                    {
                        curr.SetValue(inputs[x]);
                    }
                    curr.Pass();
                    x++;
                }
                else if (dummy.GetCollumn() == size.Length - 1)//output layer
                {
                    curr.SquishSigmoid();
                }
                else//hidden layer
                {
                    if (curr.GetLeftConnections().GetLength() != 0)//this is done for the bias nodes
                        curr.SquishSigmoid();
                    curr.Pass();
                }
                curr = curr.GetBelow();
            }
            dummy = dummy.GetNext();
        }
    }
    public void RunInputsCubeCP(float[] inputs)
    {
        //same as RunInputs but different squish for output layer for the cubes checkpoints
        if (inputs.Length != size[0] && inputs.Length != size[0] - 1)
        {
            Debug.Log("invalid input size for runInputs");
            return;
        }


        //we have to first set the neurons to 0
        int x;
        Neuron dummy = dummyHead;
        while (dummy != null)
        {
            Neuron curr = dummy.GetBelow();
            while (curr != null)
            {
                if (dummy.GetCollumn() != 0)
                {
                    if (curr.GetLeftConnections().GetLength() != 0)//this is done for the bias nodes
                        curr.SetValue(0);
                }
                curr = curr.GetBelow();
            }
            dummy = dummy.GetNext();
        }


        //the actual forward pass
        x = 0;
        dummy = dummyHead;
        while (dummy != null)
        {
            Neuron curr = dummy.GetBelow();
            while (curr != null)
            {
                if (dummy.GetCollumn() == 0)//input layer
                {

                    if (x < inputs.Length)
                    {
                        curr.SetValue(inputs[x]);
                    }
                    curr.Pass();
                    x++;
                }
                else if (dummy.GetCollumn() == size.Length - 1)//output layer
                {
                    //this is for the checkpointing system of the neural network with the cube
                    if (curr.GetRow() < 6)
                        curr.SquishSigmoid();
                    else
                    {
                        curr.Squish10();
                    }
                }
                else//hidden layer
                {
                    if (curr.GetLeftConnections().GetLength() != 0)//this is done for the bias nodes
                        curr.SquishSigmoid();
                    curr.Pass();
                }
                curr = curr.GetBelow();
            }
            dummy = dummy.GetNext();
        }
    }
    public int GetOutput()
    {
        if (size.Length == 0)
        {
            Debug.Log("tried to get output of empty neural network");
            return -1;
        }
        Neuron curr = dummyHead;
        while (curr.GetNext() != null)
        {
            curr = curr.GetNext();
        }
        curr = curr.GetBelow();
        if (curr == null)
        {
            Debug.Log("tried to get output of neural network with missing output layer");
            return -1;
        }
        Neuron biggest = curr;
        while (curr != null)
        {
            if (curr.GetValue() > biggest.GetValue())
            {
                biggest = curr;
            }
            curr = curr.GetBelow();
        }
        return biggest.GetRow();
    }
    public void BackPropegate(float[] desiredOutputs)
    {
        //normal back propegation based on the matt mazur example
        float totalError = 0;
        int z;
        for (z = 0; z < desiredOutputs.Length; z++)
        {
            totalError += Mathf.Abs(desiredOutputs[z] - GetNeuron(size.Length - 1, z).GetValue());
        }
        //Debug.Log(totalError);
        //check to make sure outputs are the right size
        if (desiredOutputs.Length != size[size.Length - 1])
        {
            Debug.Log("invalid desireOutputs size for backPropegate");
            return;
        }
        //need to go backwards
        Neuron dummy = dummyHead;
        int count = 0;
        while (dummy.GetNext() != null)
        {
            count++;
            dummy = dummy.GetNext();
        }
        int x;
        for (x = count; x >= 1; x--) //we dont need to do the input layer
        {
            Neuron curr = GetNeuron(x, 0);
            if (curr != null)
            {
                //the output layer
                if (curr.GetCollumn() == size.Length - 1)
                {
                    if (curr.GetCollumn() == size.Length - 1)
                    {
                        while (curr != null)
                        {
                            float dTotalE_dOut = -(desiredOutputs[curr.GetRow()] - curr.GetValue());
                            float dOut_dNet = curr.GetValue() * (1 - curr.GetValue());
                            curr.SetDTotalE_dNet(dTotalE_dOut * dOut_dNet);

                            Connection currConn = curr.GetLeftConnections().GetHead();
                            while (currConn != null)// change this and 165 for the matt mazur way to ...while (currConn.getNext(false) != null)
                            {
                                float dNet_dConnection = currConn.GetLeftNeuron().GetValue(); // needs to be the output from h1
                                float dTotalE_dConnection = dTotalE_dOut * dOut_dNet * dNet_dConnection;
                                currConn.SetTotalDesiredChange(currConn.GetTotalDesiredChange() + dTotalE_dConnection);
                                currConn = currConn.GetNext(false);//right connections go through the lefts for  curr
                            }
                            curr = curr.GetBelow();
                        }
                    }
                }
                //hidden layers
                else
                {
                    if (curr.GetCollumn() != 0)
                    {
                        while (curr.GetBelow() != null)
                        {
                            //go through and grab old values
                            Connection currConn = curr.GetRightConnections().GetHead();
                            float dTotalE_dOut = 0;
                            while (currConn != null)
                            {
                                dTotalE_dOut += currConn.GetRightNeuron().GetDTotalE_dNet() * currConn.GetValue();// needs to be the just the weight of the neuron
                                currConn = currConn.GetNext(true);//right connections go through the lefts for  curr
                            }

                            float dOut_dNet = curr.GetValue() * (1 - curr.GetValue());


                            curr.SetDTotalE_dNet(dTotalE_dOut * dOut_dNet);

                            //do the backPass process here
                            currConn = curr.GetLeftConnections().GetHead();
                            while (currConn != null)
                            {
                                float dNet_dConnection = currConn.GetLeftNeuron().GetValue();
                                float dTotalE_dConnection = dTotalE_dOut * dOut_dNet * dNet_dConnection;
                                currConn.SetTotalDesiredChange(currConn.GetTotalDesiredChange() + dTotalE_dConnection);

                                currConn = currConn.GetNext(false);//right connections go through the lefts for  curr
                            }
                            curr = curr.GetBelow();
                        }
                    }

                }
            }
        }
    }
    public void AddDesiredChanges(float learningRate)
    {
        // each node holds its own desired changes that it needs to implement just multiplied by the learning rate
        Neuron dummy = dummyHead;
        Neuron curr;
        Connection currConn;
        while (dummy.GetNext() != null)
        {
            curr = dummy.GetBelow();
            while (curr != null)
            {
                currConn = curr.GetRightConnections().GetHead();
                while (currConn != null)
                {
                    currConn.SetValue(currConn.GetValue() - currConn.GetTotalDesiredChange() * learningRate);
                    currConn.SetTotalDesiredChange(0);
                    currConn = currConn.GetNext(true);
                }
                curr = curr.GetBelow();
            }
            dummy = dummy.GetNext();
        }
    }


    //these are all the basic methods for changing and accessing certain parts of the neural network
    public void DeleteCollumn(int index)
    {
        //first check to make sure the collumnn exists
        Neuron dummy = GetNeuron(index, -1), prev = GetNeuron(index - 1, -1);
        if (dummy == null)
        {
            Debug.Log("no plz");
            return;
        }


        //now we have to clear all neurons below individually so that they can delete their connections properly
        Neuron curr = dummy.GetBelow();
        while (curr != null)
        {
            DeleteNeuron(index, 0);
            curr = curr.GetBelow();
        }
        //remove the collumn
        if (index == 0)
        {
            dummyHead = dummyHead.GetNext();
        }
        else
        {
            prev.SetNext(prev.GetNext().GetNext());
        }

        //update other all neurons with higher indices down to their new index
        dummy = prev.GetNext();

        while (dummy != null)
        {
            curr = dummy;
            while (curr != null)
            {
                curr.SetCollumn(curr.GetCollumn() - 1);
                curr = curr.GetBelow();
            }
            dummy = dummy.GetNext();
        }
        size = DecreaseSize(size, index);
    }
    public void AddCollumn(int index)
    {
        if (index > size.Length || index < 0)
        {
            Debug.Log("tried to addCollumn with invalid index");
            return;
        }


        //adding the collumn
        Neuron dummy = new Neuron(0, index, -1);
        Neuron curr;

        if (index == 0)
        {
            dummy.SetNext(dummyHead);
            dummyHead = dummy;
            curr = dummyHead.GetNext();
        }
        else if (index == size.Length)
        {
            curr = dummyHead;
            //we grab the dummy before where we will insert which is the last dummy
            while (curr.GetNext() != null)
            {
                curr = curr.GetNext();
            }
            curr.SetNext(dummy);
            size = IncreaseSize(size, index);
            return; // because the collumn will have none following it to change
        }
        else
        {
            //this chunk makes sure we have the dummy before the spot we will insert into
            curr = dummyHead;
            int x;
            for (x = 0; x < index - 1; x++)
            {
                curr = curr.GetNext();
            }

            dummy.SetNext(curr.GetNext());

            curr.SetNext(dummy);
            curr = dummy.GetNext();
        }
        //changing the collumns indexes that follow
        while (curr != null)
        {
            int newCol = curr.GetCollumn() + 1;
            Neuron rowCurr = curr;
            while (rowCurr != null)
            {
                rowCurr.SetCollumn(newCol);
                rowCurr = rowCurr.GetBelow();
            }
            curr = curr.GetNext();
        }
        size = IncreaseSize(size, index);
    }
    private int[] IncreaseSize(int[] oldSize, int index)
    {
        int x;
        int[] newSize = new int[oldSize.Length + 1];
        for (x = 0; x < (oldSize.Length + 1); x++)
        {
            if (x < index)
            {
                newSize[x] = oldSize[x];
            }
            else if (x > index)
            {
                newSize[x] = oldSize[x - 1];
            }
            else
            {
                newSize[x] = 0;
            }
        }
        return newSize;

    }
    private int[] DecreaseSize(int[] oldSize, int index)
    {
        if (oldSize.Length == 0)
            return oldSize;
        int x;
        int[] newSize = new int[oldSize.Length - 1];
        for (x = 0; x < (oldSize.Length - 1); x++)
        {
            if (x < index)
            {
                newSize[x] = oldSize[x];
            }
            else
            {
                newSize[x] = oldSize[x + 1];
            }
        }
        return newSize;
    }
    public void AddNeuron(int coll, int row)
    {
        //check to not double insert
        Neuron above = GetNeuron(coll, row - 1);
        Neuron insert = new Neuron(1f, coll, row);
        //check to make sure this is a valid index
        if (row > size[coll] || row < 0)
        {
            Debug.Log("tried to insert neuron out of range of collumn");
            return;
        }

        insert.SetBelow(above.GetBelow());
        above.SetBelow(insert);

        Neuron curr = insert.GetBelow();

        //change the size
        size[coll] = size[coll] + 1;

        while (curr != null)
        {
            curr.SetRow(curr.GetRow() + 1);
            curr = curr.GetBelow();
        }


    }
    public void DeleteNeuron(int coll, int row)
    {
        Neuron curr = GetNeuron(coll, row);
        if (curr == null)
        {
            Debug.Log("tried to delete nonexisting neuron");
            return;
        }
        Neuron prev = GetNeuron(coll, row - 1);

        //first we clear all connections
        curr.DeleteAllConnections();

        //remove the neuron from the neural network
        prev.SetBelow(curr.GetBelow());
        //change the size
        size[coll] = size[coll] - 1;

        // now we have to change the indices of the Neurons below
        curr = curr.GetBelow();
        while (curr != null)
        {
            curr.SetRow(curr.GetRow() - 1);
            curr = curr.GetBelow();
        }

    }
    public Neuron GetNeuron(int coll, int row)
    {
        int x;
        Neuron curr = dummyHead;
        for (x = 0; x < coll; x++)
        {
            curr = curr.GetNext();
            if (curr == null)
            {
                Debug.Log("coll fail");
                return curr;
            }
        }
        for (x = -1; x < row; x++)//-1 index for dummy nodes
        {
            curr = curr.GetBelow();
            if (curr == null)
            {
                Debug.Log("row fail");
                return curr;
            }
        }
        return curr;
    }
    public void AddConnection(int coll1, int row1, int coll2, int row2, float value)
    {
        Neuron n1, n2;
        n1 = GetNeuron(coll1, row1);
        n2 = GetNeuron(coll2, row2);

        if (n1 == null || n2 == null)
        {
            Debug.Log("tried to add connection connecting to null neuron");
            return;
        }
        if (n1.GetRow() == -1 || n2.GetRow() == -1)
        {
            Debug.Log("tried to add connection connecting to a dummy");
            return;
        }
        if (n1.GetCollumn() == n2.GetCollumn())
        {
            Debug.Log("tried to add connection connecting two neurons in the same collumn");
            return;
        }
        Connection c1 = GetConnection(coll1, row1, coll2, row2);
        if (c1 != null)
        {
            Debug.Log("tried to add connection but a connection exists here");
            return;
        }


        if (n1.GetCollumn() > n2.GetCollumn())
            n2.InsertConnection(n1, value);
        else
            n1.InsertConnection(n2, value);
    }
    public void DeleteConnection(int coll1, int row1, int coll2, int row2)
    {
        Neuron n1, n2;
        n1 = GetNeuron(coll1, row1);
        n2 = GetNeuron(coll2, row2);

        if (n1 == null || n2 == null)
        {
            Debug.Log("tried to delete connection connecting to null neuron");
            return;
        }
        n1.DeleteConnection(n2);
    }
    public Connection GetConnection(int coll1, int row1, int coll2, int row2)
    {
        Neuron n1, n2;
        n1 = GetNeuron(coll1, row1);
        n2 = GetNeuron(coll2, row2);

        if (n1 == null || n2 == null)
        {
            Debug.Log("tried to get connection connecting to null neuron");
            return null;
        }
        return n1.GetConnection(n2);
    }
    public void ConnectNeuronToCollumn(int coll1, int row1, int coll2, float value)
    {
        Neuron n1 = GetNeuron(coll1, row1);
        Neuron n2 = GetNeuron(coll2, 0);
        if (n1 == null)
        {
            Debug.Log("Tried to connect neuron to collumn where neuron is null");
        }
        if (n2 == null)
        {
            Debug.Log("Tried to connect neuron to collumn where collumn is null");
        }
        if (coll1 < coll2)
        {
            while (n2.GetBelow() != null)
            {
                AddConnection(coll1, row1, coll2, n2.GetRow(), value);

                n2 = n2.GetBelow();
            }
        }
        else if (coll1 < coll2)
        {
            while (n2 != null)
            {
                AddConnection(coll1, row1, coll2, n2.GetRow(), value);

                n2 = n2.GetBelow();
            }
        }
        else
        {
            Debug.Log("Tried to connect neuron to collumn to same collumn");
            return;
        }
    }
    public void ConnectNeuronToCollumn(int coll1, int row1, int coll2)
    {
        Neuron n1 = GetNeuron(coll1, row1);
        Neuron n2 = GetNeuron(coll2, 0);
        if (n1 == null)
        {
            Debug.Log("Tried to connect neuron to collumn where neuron is null");
        }
        if (n2 == null)
        {
            Debug.Log("Tried to connect neuron to collumn where collumn is null");
        }
        if (coll1 < coll2)
        {
            while (n2.GetBelow() != null)
            {
                AddConnection(coll1, row1, coll2, n2.GetRow(), Random.Range(-1f, 1f));

                n2 = n2.GetBelow();
            }
        }
        else if (coll1 < coll2)
        {
            while (n2 != null)
            {
                AddConnection(coll1, row1, coll2, n2.GetRow(), Random.Range(-1f, 1f));

                n2 = n2.GetBelow();
            }
        }
        else
        {
            Debug.Log("Tried to connect neuron to collumn to same collumn");
            return;
        }
    }
    public void UnconnectNeuronToCollumn(int coll1, int row1, int coll2)
    {
        Neuron n1 = GetNeuron(coll1, row1);
        Neuron n2 = GetNeuron(coll2, 0);
        if (n1 == null)
        {
            Debug.Log("Tried to connect neuron to collumn where neuron is null");
        }
        if (n2 == null)
        {
            Debug.Log("Tried to connect neuron to collumn where collumn is null");
        }
        if (coll1 > coll2)
        {
            while (n2.GetBelow() != null)
            {
                DeleteConnection(coll1, row1, coll2, n2.GetRow());
                n2 = n2.GetBelow();
            }
        }
        else if (coll1 < coll2)
        {
            while (n2 != null)
            {
                DeleteConnection(coll1, row1, coll2, n2.GetRow());
                n2 = n2.GetBelow();
            }
        }
        else
        {
            Debug.Log("Tried to connect neuron to collumn to same collumn");
            return;
        }
    }
    public void ConnectCollumnToCollumn(int coll1, int coll2, float value)
    {
        Neuron n1;
        Neuron n2;
        if (coll1 > coll2)
        {
            n1 = GetNeuron(coll2, 0);
            n2 = GetNeuron(coll1, 0);
        }
        else if (coll1 < coll2)
        {
            n1 = GetNeuron(coll1, 0);
            n2 = GetNeuron(coll2, 0);
        }
        else
        {
            return;
        }

        if (n1 == null || n2 == null)
        {
            Debug.Log("Tried to connect neuron to collumn where collumn is null");
        }

        Neuron n2helper;
        while (n1 != null)
        {
            n2helper = n2;
            if (coll2 == size.Length - 1)// for the bias nodes in input and hidden layers
            {
                while (n2helper != null)
                {
                    AddConnection(n1.GetCollumn(), n1.GetRow(), n2helper.GetCollumn(), n2helper.GetRow(), value);
                    n2helper = n2helper.GetBelow();
                }
            }
            else
            {
                while (n2helper.GetBelow() != null)
                {
                    AddConnection(n1.GetCollumn(), n1.GetRow(), n2helper.GetCollumn(), n2helper.GetRow(), value);
                    n2helper = n2helper.GetBelow();
                }
            }
            n1 = n1.GetBelow();
        }
    }
    public void ConnectCollumnToCollumn(int coll1, int coll2)
    {
        Neuron n1;
        Neuron n2;
        if (coll1 > coll2)
        {
            n1 = GetNeuron(coll2, 0);
            n2 = GetNeuron(coll1, 0);
        }
        else if (coll1 < coll2)
        {
            n1 = GetNeuron(coll1, 0);
            n2 = GetNeuron(coll2, 0);
        }
        else
        {
            return;
        }

        if (n1 == null || n2 == null)
        {
            Debug.Log("Tried to connect neuron to collumn where collumn is null");
        }

        Neuron n2helper;
        while (n1 != null)
        {
            n2helper = n2;
            if (coll2 == size.Length - 1)// for the bias nodes in input and hidden layers
            {
                while (n2helper != null)
                {
                    AddConnection(n1.GetCollumn(), n1.GetRow(), n2helper.GetCollumn(), n2helper.GetRow(), Random.Range(-1f, 1f));
                    n2helper = n2helper.GetBelow();
                }
            }
            else
            {
                while (n2helper.GetBelow() != null)
                {
                    AddConnection(n1.GetCollumn(), n1.GetRow(), n2helper.GetCollumn(), n2helper.GetRow(), Random.Range(-1f, 1f));
                    n2helper = n2helper.GetBelow();
                }
            }
            n1 = n1.GetBelow();
        }
    }
    public void UnconnectCollumnToCollumn(int coll1, int coll2)
    {
        Neuron n1;
        Neuron n2;
        if (coll1 > coll2)
        {
            n1 = GetNeuron(coll2, 0);
            n2 = GetNeuron(coll1, 0);
        }
        else if (coll1 < coll2)
        {
            n1 = GetNeuron(coll1, 0);
            n2 = GetNeuron(coll2, 0);
        }
        else
        {
            return;
        }

        if (n1 == null || n2 == null)
        {
            Debug.Log("Tried to unconnect neuron to collumn where collumn is null");
        }
        Neuron n2helper;
        if (coll1 > coll2)
        {
            while (n1.GetBelow() != null)
            {
                n2helper = n2;
                while (n2helper != null)
                {
                    DeleteConnection(n1.GetCollumn(), n1.GetRow(), n2helper.GetCollumn(), n2helper.GetRow());
                    n2helper = n2helper.GetBelow();
                }
                n1 = n1.GetBelow();
            }
        }
    }

    public void AddNetwork(NeuralNetwork nn)//don't connect input to output neurons for this method
    {
        //merges 2 nn with same input and output size

        //do some checks on the compatabity
        //make lengths the same
        //add the neurons
        //add the connections

        if (nn == null)
            return;
        int[] otherSize = nn.GetSize();
        if (otherSize.Length < 2)
            return;
        if (otherSize[0] != size[0] || otherSize[otherSize.Length - 1] != size[size.Length - 1])
            return;
        //input and output are same on a viable nn

        while (otherSize.Length > size.Length)
        {
            AddCollumn(size.Length - 1);
        }
        while (otherSize.Length < size.Length)
        {
            nn.AddCollumn(otherSize.Length - 1);
            otherSize = nn.GetSize();
        }
        //lengths are the same


        int collumn;
        for (collumn = 1; collumn < size.Length - 1; collumn++)
        {
            Neuron otherCurr;
            if (otherSize[collumn] > 0)
            {
                otherCurr = nn.GetNeuron(collumn, 0);
            }
            else
            {
                otherCurr = null;
            }

            while (otherCurr != null)
            {
                AddNeuron(collumn, size[collumn]);
                otherCurr = otherCurr.GetBelow();
            }
        }
        //all the neurons are added





        for (collumn = 0; collumn < size.Length; collumn++)//hidden layer leftNeurons
        {
            Neuron otherCurr;
            if (otherSize[collumn] > 0)
            {
                otherCurr = nn.GetNeuron(collumn, 0);
            }
            else
            {
                otherCurr = null;
            }
            while (otherCurr != null)
            {
                ConnectionList otherConnections = otherCurr.GetRightConnections();
                Connection currConn = otherConnections.GetHead();
                while (currConn != null)
                {
                    int row1;
                    if (collumn == 0)
                    {
                        row1 = currConn.GetLeftNeuron().GetRow();
                    }
                    else
                    {
                        row1 = (size[collumn] - otherSize[collumn]) + currConn.GetLeftNeuron().GetRow();
                    }
                    int coll2 = currConn.GetRightNeuron().GetCollumn();
                    int row2;

                    if (coll2 == size.Length - 1)
                    {
                        row2 = currConn.GetRightNeuron().GetRow();
                    }
                    else
                    {
                        row2 = (size[coll2] - otherSize[coll2]) + currConn.GetRightNeuron().GetRow();
                    }
                    AddConnection(collumn, row1, coll2, row2, currConn.GetValue());
                    currConn = currConn.GetNext(true);
                }
                otherCurr = otherCurr.GetBelow();
            }
        }
    }


    public int TrueSize()
    {
        //just for finding the number of things in a nueral network
        int count = 0;
        Neuron dummy = dummyHead;

        //the actual forward pass
        while (dummy != null)
        {
            Neuron curr = dummy.GetBelow();
            while (curr != null)
            {
                count += curr.GetRightConnections().GetLength();
                curr = curr.GetBelow();
                count++;
            }
            dummy = dummy.GetNext();
            count++;
        }
        return count;
    }
}
