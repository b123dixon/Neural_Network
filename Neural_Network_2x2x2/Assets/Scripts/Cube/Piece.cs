using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Piece
{
    //classic abstract class for Pieces
    public Piece()
    {

    }
    public abstract int GetColor(int num);
    public abstract void Turned(Piece newSpot, int side);
    public abstract int GetColorFromMid(int midColor);
    public abstract float GetScoreTile();
}
