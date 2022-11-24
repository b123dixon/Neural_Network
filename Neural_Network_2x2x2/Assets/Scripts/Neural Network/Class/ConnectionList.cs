using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectionList
{
    //tested
    private int length;
    private Connection head;
    //left in regards to neuron
    private bool left; // this is because connection lists nodes are always part of 2 connection lists
    // and knowing if its the front or back one is important for certain methods

    public ConnectionList(bool newLeft)
    {
        length = 0;
        head = null;
        left = newLeft;
    }
    public int GetLength()
    {
        return length;
    }
    //this is the insert used by the neurons
    public Connection GetHead()
    {
        return head;
    }

    //we do the opposite of left in insert, delete, and getConnection as a connection to left will consider the nexts as to the right
    public void Insert(Connection conn)
    {
        if (conn == null)
            return;
        //0 length
        if (length == 0)
        {
            head = conn;
            length++;
            return;
        }

        Connection curr = head, backup = head;

        if (curr.CompareTo(conn) == 1)//front
        {
            conn.SetNext(!left, head);
            head = conn;
            length++;
            return;
        }
        else
        {
            curr = curr.GetNext(!left);
            while (curr != null)
            {
                if (curr.CompareTo(conn) == 1)
                {
                    conn.SetNext(!left, curr);
                    backup.SetNext(!left, conn);
                    length++;
                    return;
                }
                backup = curr;
                curr = curr.GetNext(!left);
            }
        }
        //back of list
        backup.SetNext(!left, conn);
        length++;
    }
    public void Delete(Connection conn)
    {
        if (conn == null)
            return;
        Connection curr = head, backup = head;
        if (length == 0)
            return;
        if (curr.CompareTo(conn) == 0)
        {
            head = curr.GetNext(!left);
            length--;
            return;
        }
        curr = curr.GetNext(!left);
        while (curr != null)
        {
            if (curr.CompareTo(conn) == 0)
            {
                backup.SetNext(!left, conn.GetNext(!left));
                length--;
                return;
            }
            backup = curr;
            curr = curr.GetNext(left);
        }
    }
    public Connection GetConnection(Neuron neuron)
    {
        if (neuron == null)
            return null;
        Connection curr = head;
        while (curr != null)
        {
            Neuron currNeuron;
            //we grab the oppposite side neuron
            if (left)
                currNeuron = curr.GetLeftNeuron();
            else
                currNeuron = curr.GetRightNeuron();
            if (currNeuron.GetCollumn() == neuron.GetCollumn() && currNeuron.GetRow() == neuron.GetRow())
            {
                return curr;
            }
            curr = curr.GetNext(!left);
        }
        return null;
    }
}
