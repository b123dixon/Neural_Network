using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Connection
{

    //connection acts as the node for the singly linked list of ConnectionList
    //this class is used by Neurons to keep track of the connections
    //this node will always be part of two ConnectionLists

    //these are what the connection is meant to hold
    private float value;
    private float totalDesiredChange;
    private Neuron leftNeuron, rightNeuron;
    //next for the seperate connectionLists that the one node is part of 
    private Connection leftNext, rightNext;

    public Connection()
    {
        value = 0;
        totalDesiredChange = 0;
        leftNext = null;
        rightNext = null;
    }
    public float GetTotalDesiredChange()
    {
        return totalDesiredChange;
    }
    public void SetTotalDesiredChange(float newTotalDesiredChange)
    {
        totalDesiredChange = newTotalDesiredChange;
    }
    public float GetValue()
    {
        return value;
    }
    public void SetValue(float newValue)
    {
        value = newValue;
    }
    public Neuron GetRightNeuron()
    {
        return rightNeuron;
    }
    public void SetRightNeuron(Neuron newNeuron)
    {
        rightNeuron = newNeuron;
    }
    public Neuron GetLeftNeuron()
    {
        return leftNeuron;
    }
    public void SetLeftNeuron(Neuron newNeuron)
    {
        leftNeuron = newNeuron;
    }
    public Connection GetNext(bool left)
    {
        if (left)
            return leftNext;
        else
            return rightNext;
    }
    public void SetNext(bool left, Connection newNext)
    {
        if (left)
            leftNext = newNext;
        else
            rightNext = newNext;
    }
    public int CompareTo(Connection compareMe)
    {
        if (compareMe == null)
        {
            Debug.Log("Connection Compared To Null");
            return 1;
        }
        int num1 = leftNeuron.GetCollumn(), num2 = compareMe.GetLeftNeuron().GetCollumn();
        if (num1 > num2)
        {
            return 1;
        }
        if (num1 < num2)
        {
            return -1;
        }

        num1 = leftNeuron.GetRow();
        num2 = compareMe.GetLeftNeuron().GetRow();
        if (num1 > num2)
        {
            return 1;
        }
        if (num1 < num2)
        {
            return -1;
        }

        num1 = rightNeuron.GetCollumn();
        num2 = compareMe.GetRightNeuron().GetCollumn();
        if (num1 > num2)
        {
            return 1;
        }
        if (num1 < num2)
        {
            return -1;
        }

        num1 = rightNeuron.GetRow();
        num2 = compareMe.GetRightNeuron().GetRow();
        if (num1 > num2)
        {
            return 1;
        }
        if (num1 < num2)
        {
            return -1;
        }

        //this should never happen
        return 0;

    }
}

