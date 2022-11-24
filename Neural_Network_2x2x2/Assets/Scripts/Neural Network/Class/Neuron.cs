using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Neuron
{
    //tested
    // Neuron acts as the Node for the singly linked list NeuronList

    private float dTotalE_dNet;
    private float value;
    private int row, collumn; // for position in network

    //singly linked list for going down the rows
    //the dummy head nodes only have other dummyhead nodes for forward and backward connections
    private Neuron below;
    private Neuron next;
    private ConnectionList leftConnections;
    private ConnectionList rightConnections;

    public Neuron(float newValue, int newCollumn, int newRow)
    {
        dTotalE_dNet = 0;
        value = newValue;
        row = newRow;
        collumn = newCollumn;
        rightConnections = new ConnectionList(false);
        leftConnections = new ConnectionList(true);
    }

    public float GetValue()
    {
        return value;
    }
    public void SetValue(float newValue)
    {
        value = newValue;
    }
    public float GetDTotalE_dNet()
    {
        return dTotalE_dNet;
    }
    public void SetDTotalE_dNet(float newDTotalE_dNet)
    {
        dTotalE_dNet = newDTotalE_dNet;
    }
    public int GetRow()
    {
        return row;
    }
    public void SetRow(int newRow)
    {
        row = newRow;
    }
    public int GetCollumn()
    {
        return collumn;
    }
    public void SetCollumn(int newCollumn)
    {
        collumn = newCollumn;
    }
    public Neuron GetBelow()
    {
        return below;
    }
    public void SetBelow(Neuron newBelow)
    {
        below = newBelow;
    }
    public Neuron GetNext()
    {
        return next;
    }
    public void SetNext(Neuron newNext)
    {
        next = newNext;
    }

    public void InsertConnection(Neuron n2, float value)
    {
        //check to make sure connection isnt to null or to this collumn
        if (n2 == null)
            return;
        if (n2.GetCollumn() == collumn)
            return;


        //check if connection exists
        Connection c1;
        if (n2.GetCollumn() < collumn)
            c1 = leftConnections.GetConnection(n2);
        else
            c1 = rightConnections.GetConnection(n2);
        if (c1 != null)
        {
            Debug.Log("tried to double insert");
            return;
        }

        //everything is ready for insertion 

        //create the connection
        c1 = new Connection();
        c1.SetValue(value);

        //connect everybody
        if (n2.GetCollumn() < collumn)
        {
            c1.SetLeftNeuron(n2);
            c1.SetRightNeuron(this);
            this.GetLeftConnections().Insert(c1);
            n2.GetRightConnections().Insert(c1);
        }
        else
        {
            c1.SetLeftNeuron(this);
            c1.SetRightNeuron(n2);
            n2.GetLeftConnections().Insert(c1);
            this.GetRightConnections().Insert(c1);
        }
    }
    public void DeleteConnection(Neuron n2, bool left)
    {
        Connection c1;
        //check if connection exists
        if (left)
            c1 = leftConnections.GetConnection(n2);
        else
            c1 = rightConnections.GetConnection(n2);
        if (c1 == null)
        {
            Debug.Log("tried to delete non existing connection");
            return;
        }

        if (left)
        {
            n2 = c1.GetLeftNeuron();
            n2.GetRightConnections().Delete(c1);
            GetLeftConnections().Delete(c1);
        }
        else
        {
            n2 = c1.GetRightNeuron();
            n2.GetLeftConnections().Delete(c1);
            GetRightConnections().Delete(c1);
        }
    }
    public void DeleteConnection(Neuron n2)
    {
        if (n2 == null)
        {
            Debug.Log("tried to insert connection with null neuron");
            return;
        }
        Connection c1;
        //check if connection exists
        //while also checking if it is in the same collumn
        if (n2.GetCollumn() < collumn)
        {
            c1 = leftConnections.GetConnection(n2);
        }
        else if (n2.GetCollumn() > collumn)
        {
            c1 = rightConnections.GetConnection(n2);
        }
        else
        {
            Debug.Log("Same Collumn insert");
            return;
        }
        if (c1 == null)
        {
            Debug.Log("tried to delete non existing connection");
            return;
        }

        //passed this point everything is good for deletion

        if (n2.GetCollumn() < collumn)
        {
            n2.GetRightConnections().Delete(c1);
            GetLeftConnections().Delete(c1);
        }
        else
        {
            n2.GetLeftConnections().Delete(c1);
            GetRightConnections().Delete(c1);
        }
    }
    public void DeleteAllConnections()
    {
        while (leftConnections.GetLength() != 0)
        {
            Connection c1 = leftConnections.GetHead();
            Neuron n1 = c1.GetLeftNeuron();
            n1.DeleteConnection(c1.GetRightNeuron(), false);
        }
        while (rightConnections.GetLength() != 0)
        {
            Connection c1 = rightConnections.GetHead();
            Neuron n1 = c1.GetRightNeuron();
            n1.DeleteConnection(c1.GetLeftNeuron(), true);
        }
    }

    public ConnectionList GetLeftConnections()
    {
        return leftConnections;
    }
    public ConnectionList GetRightConnections()
    {
        return rightConnections;
    }

    public Connection GetConnection(bool left, Neuron n2)
    {
        Connection conn;
        if (left)
        {
            conn = leftConnections.GetConnection(n2);
        }
        else
        {
            conn = rightConnections.GetConnection(n2);
        }
        return conn;
    }
    public Connection GetConnection(Neuron n2)
    {
        Connection conn = null;
        if (n2.GetCollumn() > collumn)
        {
            conn = rightConnections.GetConnection(n2);
        }
        else if (n2.GetCollumn() < collumn)
        {
            conn = leftConnections.GetConnection(n2);
        }
        return conn;
    }


    public void Pass()
    {
        Connection c1 = rightConnections.GetHead();
        while (c1 != null)
        {
            Neuron n1 = c1.GetRightNeuron();
            n1.SetValue(n1.GetValue() + c1.GetValue() * value);
            c1 = c1.GetNext(true);
        }
    }
    public void SquishSigmoid()
    {
        //sigmoid squish
        float e = 2.71828f; // this is an approximation 
        float newNum = 1 + Mathf.Pow(e, -value);
        value = 1 / newNum;
    }
    public void Squish10()
    {
        //if greater than 10 then output 1
        if (value >= 10)
            value = 1;
        else
            value = 0;
    }

}
