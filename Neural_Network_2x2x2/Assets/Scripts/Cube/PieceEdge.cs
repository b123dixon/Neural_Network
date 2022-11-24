using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceEdge : Piece
{
    //color represents the color of the tile touching colorMid
    //this allows to know the position and orientation of the different pieces
    private int color1, color2;
    private int color1Mid, color2Mid;

    //this will create a piece with matching color and colormids
    public PieceEdge(int newColor1, int newColor2)
    {
        color1 = newColor1;
        color2 = newColor2;
        color1Mid = newColor1;
        color2Mid = newColor2;
    }

    //this will create a piece with given color and colorMid
    public PieceEdge(int newColor1, int newColor2, int newMid1, int newMid2)
    {
        color1 = newColor1;
        color2 = newColor2;
        color1Mid = newMid1;
        color2Mid = newMid2;
    }

    // returns the color for the given number corrosponding with the colors in order that they are declared above
    override public int GetColor(int spotColor)
    {
        if (spotColor == 1)
        {
            return color1;
        }
        else if (spotColor == 2)
        {
            return color2;
        }
        else if (spotColor == 3)
        {
            return color1Mid;
        }
        else if (spotColor == 4)
        {
            return color2Mid;
        }
        else return 0;
    }

    //returns the color that corrosponds with the given midColor
    override public int GetColorFromMid(int midColor)
    {
        if (midColor == color1Mid)
        {
            return color1;
        }
        if (midColor == color2Mid)
        {
            return color2;
        }
        else return 0;
    }

    // updates the piece so that the colors represent their proper new color
    override public void Turned(Piece newPiece, int side)
    {
        //side isn't actually needed here in this method but is here because PieceCorner needs it
        if (newPiece.GetColor(3) == color1Mid || newPiece.GetColor(4) == color2Mid)
        {
            color1 = newPiece.GetColor(1);
            color2 = newPiece.GetColor(2);
        }
        if (newPiece.GetColor(3) == color2Mid || newPiece.GetColor(4) == color1Mid)
        {
            color1 = newPiece.GetColor(2);
            color2 = newPiece.GetColor(1);
        }
    }

    //returns the number of tiles that have color = colorMid
    override public float GetScoreTile()
    {
        float score = 0f;
        if (color1 == color1Mid)
        {
            score = score + 1f;
        }
        if (color2 == color2Mid)
        {
            score = score + 1f;
        }
        return score;
    }
}