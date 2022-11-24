using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceCorner : Piece
{
    //color represents the color of the tile touching colorMid
    //this allows to know the position and orientation of the different pieces
    private int color1, color2, color3;
    private int color1Mid, color2Mid, color3Mid;

    //this will create a piece with matching color and colormids
    public PieceCorner(int newColor1, int newColor2, int newColor3)
    {
        color1 = newColor1;
        color2 = newColor2;
        color3 = newColor3;
        color1Mid = newColor1;
        color2Mid = newColor2;
        color3Mid = newColor3;
    }

    //this will create a piece with given color and colorMid
    public PieceCorner(int newColor1, int newColor2, int newColor3, int newMid1, int newMid2, int newMid3)
    {
        color1 = newColor1;
        color2 = newColor2;
        color3 = newColor3;
        color1Mid = newMid1;
        color2Mid = newMid2;
        color3Mid = newMid3;
    }

    // returns the color for the given number corrosponding with the colors in order that they are declared above
    override public int GetColor(int spot)
    {
        if (spot == 1)
        {
            return color1;
        }
        else if (spot == 2)
        {
            return color2;
        }
        else if (spot == 3)
        {
            return color3;
        }
        else if (spot == 4)
        {
            return color1Mid;
        }
        else if (spot == 5)
        {
            return color2Mid;
        }
        else if (spot == 6)
        {
            return color3Mid;
        }
        else return 0;
    }

    // returns the color for the given number corrosponding with the colors in order that they are declared above
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
        if (midColor == color3Mid)
        {
            return color3;
        }
        else return 0;
    }

    // updates the piece so that the colors represent their proper new color
    override public void Turned(Piece newPiece, int side)
    {
        // the large block of code follows this general idea to figure out which way to switch the colors
        // 1.) find a tile from this piece and newpiece that share a same colorMid
        // 2.) Check if that colorMid is the same as the side sent in
        // 3.) Finally find the other pair of tiles that share a same colorMid
        // the good match switches with its match and the bad match CANT switch with its match

        if (newPiece.GetColor(4) == color1Mid)
        {
            if (color1Mid == side)//this means it was the good match
            {
                color1 = newPiece.GetColor(1);
                if (newPiece.GetColor(5) == color2Mid || newPiece.GetColor(6) == color3Mid)// bad match
                {
                    color2 = newPiece.GetColor(3);
                    color3 = newPiece.GetColor(2);
                }
                else
                {
                    color2 = newPiece.GetColor(2);
                    color3 = newPiece.GetColor(3);
                }
            }
            else// we got the bad side
            {
                if (newPiece.GetColor(5) == color2Mid)// good match
                {
                    color2 = newPiece.GetColor(2);

                    color1 = newPiece.GetColor(3);
                    color3 = newPiece.GetColor(1);
                }
                else if (newPiece.GetColor(6) == color2Mid)// good match
                {
                    color2 = newPiece.GetColor(3);

                    color1 = newPiece.GetColor(2);
                    color3 = newPiece.GetColor(1);
                }
                else if (newPiece.GetColor(5) == color3Mid)// good match
                {
                    color3 = newPiece.GetColor(2);

                    color1 = newPiece.GetColor(3);
                    color2 = newPiece.GetColor(1);
                }
                else //(newPiece.gimmeColor(6) == color3Mid)// good match
                {
                    color3 = newPiece.GetColor(3);

                    color1 = newPiece.GetColor(2);
                    color2 = newPiece.GetColor(1);
                }
            }
        }
        else if (newPiece.GetColor(5) == color1Mid)
        {
            if (color1Mid == side)//this means it was the good match
            {
                color1 = newPiece.GetColor(2);
                if (newPiece.GetColor(4) == color2Mid || newPiece.GetColor(6) == color3Mid)// bad match
                {
                    color2 = newPiece.GetColor(3);
                    color3 = newPiece.GetColor(1);
                }
                else
                {
                    color2 = newPiece.GetColor(1);
                    color3 = newPiece.GetColor(3);
                }
            }
            else// bad match
            {
                if (newPiece.GetColor(4) == color2Mid)// good match
                {
                    color2 = newPiece.GetColor(1);

                    color1 = newPiece.GetColor(3);
                    color3 = newPiece.GetColor(2);
                }
                else if (newPiece.GetColor(6) == color2Mid)// good match
                {
                    color2 = newPiece.GetColor(3);

                    color1 = newPiece.GetColor(1);
                    color3 = newPiece.GetColor(2);
                }
                else if (newPiece.GetColor(4) == color3Mid)// good match
                {
                    color3 = newPiece.GetColor(1);

                    color1 = newPiece.GetColor(3);
                    color2 = newPiece.GetColor(2);
                }
                else if (newPiece.GetColor(6) == color3Mid)// good match
                {
                    color3 = newPiece.GetColor(3);

                    color1 = newPiece.GetColor(1);
                    color2 = newPiece.GetColor(2);
                }
            }
        }
        else if (newPiece.GetColor(6) == color1Mid)
        {
            if (color1Mid == side)// good match
            {
                color1 = newPiece.GetColor(3);
                if (newPiece.GetColor(5) == color2Mid || newPiece.GetColor(4) == color3Mid)// bad match
                {
                    color2 = newPiece.GetColor(1);
                    color3 = newPiece.GetColor(2);
                }
                else
                {
                    color2 = newPiece.GetColor(2);
                    color3 = newPiece.GetColor(1);
                }
            }
            else// bad match
            {
                if (newPiece.GetColor(4) == color2Mid)// good match
                {
                    color2 = newPiece.GetColor(1);

                    color1 = newPiece.GetColor(2);
                    color3 = newPiece.GetColor(3);
                }
                else if (newPiece.GetColor(5) == color2Mid)// good match
                {
                    color2 = newPiece.GetColor(2);

                    color1 = newPiece.GetColor(1);
                    color3 = newPiece.GetColor(3);
                }
                else if (newPiece.GetColor(4) == color3Mid)// good match
                {
                    color3 = newPiece.GetColor(1);

                    color1 = newPiece.GetColor(2);
                    color2 = newPiece.GetColor(3);
                }
                else if (newPiece.GetColor(5) == color3Mid)// good match
                {
                    color3 = newPiece.GetColor(2);

                    color1 = newPiece.GetColor(1);
                    color2 = newPiece.GetColor(3);
                }
            }
        }
        else if (newPiece.GetColor(4) == color2Mid)
        {
            if (color2Mid == side)// good match
            {
                color2 = newPiece.GetColor(1);
                if (newPiece.GetColor(5) == color3Mid)// bad match
                {
                    color3 = newPiece.GetColor(3);
                    color1 = newPiece.GetColor(2);
                }
                else
                {
                    color3 = newPiece.GetColor(2);
                    color1 = newPiece.GetColor(3);
                }
            }
            else// bad match
            {
                if (newPiece.GetColor(5) == color3Mid)// good match
                {
                    color3 = newPiece.GetColor(2);

                    color1 = newPiece.GetColor(1);
                    color2 = newPiece.GetColor(3);
                }
                else
                {
                    color3 = newPiece.GetColor(3);

                    color1 = newPiece.GetColor(1);
                    color2 = newPiece.GetColor(2);
                }
            }
        }
        else if (newPiece.GetColor(5) == color2Mid)
        {
            if (color2Mid == side)// good match
            {
                color2 = newPiece.GetColor(2);
                if (newPiece.GetColor(4) == color3Mid)// bad match
                {
                    color3 = newPiece.GetColor(3);
                    color1 = newPiece.GetColor(1);
                }
                else
                {
                    color3 = newPiece.GetColor(1);
                    color1 = newPiece.GetColor(3);
                }
            }
            else// bad match
            {
                if (newPiece.GetColor(4) == color3Mid)//good match
                {
                    color3 = newPiece.GetColor(1);

                    color1 = newPiece.GetColor(2);
                    color2 = newPiece.GetColor(3);
                }
                else
                {
                    color3 = newPiece.GetColor(3);

                    color1 = newPiece.GetColor(2);
                    color2 = newPiece.GetColor(1);
                }
            }
        }
        else if (newPiece.GetColor(6) == color2Mid)
        {
            if (color2Mid == side)//good match
            {
                color2 = newPiece.GetColor(3);
                if (newPiece.GetColor(4) == color3Mid)//bad match
                {
                    color3 = newPiece.GetColor(2);
                    color1 = newPiece.GetColor(1);
                }
                else
                {
                    color3 = newPiece.GetColor(1);
                    color1 = newPiece.GetColor(2);
                }
            }
            else//bad match
            {
                if (newPiece.GetColor(4) == color3Mid)//good match
                {
                    color3 = newPiece.GetColor(1);

                    color1 = newPiece.GetColor(3);
                    color2 = newPiece.GetColor(2);
                }
                else
                {
                    color3 = newPiece.GetColor(2);

                    color1 = newPiece.GetColor(3);
                    color2 = newPiece.GetColor(1);
                }
            }
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
        if (color3 == color3Mid)
        {
            score = score + 1f;
        }
        return score;
    }
}