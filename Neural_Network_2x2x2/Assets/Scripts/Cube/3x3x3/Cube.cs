using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube
{
    // these colors just make it easier to visualise what is going on
    int GREEN = 1, BLUE = 2, RED = 3, ORANGE = 4, WHITE = 5, YELLOW = 6;
    // these hold all the pieces that can move on a rubik's cube
    public Piece[] pieces = new Piece[20];

    // creates a cube in its solved state
    public Cube()
    {
        pieces[00] = new PieceEdge(GREEN, RED);
        pieces[01] = new PieceEdge(GREEN, ORANGE);
        pieces[02] = new PieceEdge(GREEN, WHITE);
        pieces[03] = new PieceEdge(GREEN, YELLOW);
        pieces[04] = new PieceEdge(BLUE, RED);
        pieces[05] = new PieceEdge(BLUE, ORANGE);
        pieces[06] = new PieceEdge(BLUE, WHITE);
        pieces[07] = new PieceEdge(BLUE, YELLOW);
        pieces[08] = new PieceEdge(RED, WHITE);
        pieces[09] = new PieceEdge(RED, YELLOW);
        pieces[10] = new PieceEdge(ORANGE, WHITE);
        pieces[11] = new PieceEdge(ORANGE, YELLOW);
        pieces[12] = new PieceCorner(GREEN, RED, WHITE);
        pieces[13] = new PieceCorner(GREEN, RED, YELLOW);
        pieces[14] = new PieceCorner(GREEN, ORANGE, WHITE);
        pieces[15] = new PieceCorner(GREEN, ORANGE, YELLOW);
        pieces[16] = new PieceCorner(BLUE, RED, WHITE);
        pieces[17] = new PieceCorner(BLUE, RED, YELLOW);
        pieces[18] = new PieceCorner(BLUE, ORANGE, WHITE);
        pieces[19] = new PieceCorner(BLUE, ORANGE, YELLOW);
    }

    //Scrambles the cube by performing n random moves where n is the integer given > 0
    public void Scramble(int numOfMoves)
    {
        if (numOfMoves <= 0)
            return;
        int x;
        for (x = 0; x < numOfMoves; x++)
        {
            turn(Random.Range(1, 13));
        }
    }

    //resets cube to solved state
    public void Reset()
    {
        pieces[00] = new PieceEdge(GREEN, RED);
        pieces[01] = new PieceEdge(GREEN, ORANGE);
        pieces[02] = new PieceEdge(GREEN, WHITE);
        pieces[03] = new PieceEdge(GREEN, YELLOW);
        pieces[04] = new PieceEdge(BLUE, RED);
        pieces[05] = new PieceEdge(BLUE, ORANGE);
        pieces[06] = new PieceEdge(BLUE, WHITE);
        pieces[07] = new PieceEdge(BLUE, YELLOW);
        pieces[08] = new PieceEdge(RED, WHITE);
        pieces[09] = new PieceEdge(RED, YELLOW);
        pieces[10] = new PieceEdge(ORANGE, WHITE);
        pieces[11] = new PieceEdge(ORANGE, YELLOW);
        pieces[12] = new PieceCorner(GREEN, RED, WHITE);
        pieces[13] = new PieceCorner(GREEN, RED, YELLOW);
        pieces[14] = new PieceCorner(GREEN, ORANGE, WHITE);
        pieces[15] = new PieceCorner(GREEN, ORANGE, YELLOW);
        pieces[16] = new PieceCorner(BLUE, RED, WHITE);
        pieces[17] = new PieceCorner(BLUE, RED, YELLOW);
        pieces[18] = new PieceCorner(BLUE, ORANGE, WHITE);
        pieces[19] = new PieceCorner(BLUE, ORANGE, YELLOW);
    }

    //turn will turn a side besed off of an input of int range 1-12
    public void turn(int side)
    {
        if (side == 0)
            Debug.Log("Bruh");
        // these are just used to save the pieces that will move to help turn
        Piece edge1 = null, edge2 = null, edge3 = null, edge4 = null;
        Piece corner1 = null, corner2 = null, corner3 = null, corner4 = null;

        //Whatever the color is the turn method will move that side clockwise unless the color has a +6 that will invert the turn
        // and the comments after each piece just show what piece is being grabbed
        switch (side)
        {
            case 1:
                edge1 = pieces[0];//gr
                edge2 = pieces[2];//gw
                edge3 = pieces[1];//go
                edge4 = pieces[3];//gy
                corner1 = pieces[12];//grw
                corner2 = pieces[14];//gow
                corner3 = pieces[15];//goy
                corner4 = pieces[13];//gry
                break;
            case 2:
                edge1 = pieces[4];//br
                edge2 = pieces[7];//by
                edge3 = pieces[5];//bo
                edge4 = pieces[6];//bw
                corner1 = pieces[16];//brw
                corner2 = pieces[17];//bry
                corner3 = pieces[19];//boy
                corner4 = pieces[18];//bow
                break;
            case 3:
                edge1 = pieces[0];//gr
                edge2 = pieces[9];//ry
                edge3 = pieces[4];//br
                edge4 = pieces[8];//rw
                corner1 = pieces[12];//grw
                corner2 = pieces[13];//gry
                corner3 = pieces[17];//bry
                corner4 = pieces[16];//brw
                break;
            case 4:
                edge1 = pieces[1];
                edge2 = pieces[10];
                edge3 = pieces[5];
                edge4 = pieces[11];
                corner1 = pieces[14];
                corner2 = pieces[18];
                corner3 = pieces[19];
                corner4 = pieces[15];
                break;
            case 5:
                edge1 = pieces[2];//gw
                edge2 = pieces[8];//rw
                edge3 = pieces[6];//bw
                edge4 = pieces[10];//ow
                corner1 = pieces[14];//gow
                corner2 = pieces[12];//grw
                corner3 = pieces[16];//brw
                corner4 = pieces[18];//bow
                break;
            case 6:
                edge1 = pieces[3];//gy
                edge2 = pieces[11];//oy
                edge3 = pieces[7];//by
                edge4 = pieces[9];//ry
                corner1 = pieces[13];//gry
                corner2 = pieces[15];//goy
                corner3 = pieces[19];//boy
                corner4 = pieces[17];//bry
                break;
            case 7:// inverted
                edge1 = pieces[0];//gr
                edge2 = pieces[3];//gy
                edge3 = pieces[1];//go
                edge4 = pieces[2];//gw
                corner1 = pieces[12];//grw
                corner2 = pieces[13];//gry
                corner3 = pieces[15];//goy
                corner4 = pieces[14];//gow
                break;
            case 8:
                edge1 = pieces[4];//br
                edge2 = pieces[6];//bw
                edge3 = pieces[5];//bo
                edge4 = pieces[7];//by
                corner1 = pieces[16];//brw
                corner2 = pieces[18];//bow
                corner3 = pieces[19];//boy
                corner4 = pieces[17];//bry
                break;
            case 9:
                edge1 = pieces[0];//gr
                edge2 = pieces[8];//rw
                edge3 = pieces[4];//br
                edge4 = pieces[9];//ry
                corner1 = pieces[12];//grw
                corner2 = pieces[16];//brw
                corner3 = pieces[17];//bry
                corner4 = pieces[13];//gry
                break;
            case 10:
                edge1 = pieces[1];
                edge2 = pieces[11];
                edge3 = pieces[5];
                edge4 = pieces[10];
                corner1 = pieces[14];
                corner2 = pieces[15];
                corner3 = pieces[19];
                corner4 = pieces[18];
                break;
            case 11:
                edge1 = pieces[2];//gw
                edge2 = pieces[10];//ow
                edge3 = pieces[6];//bw
                edge4 = pieces[8];//rw
                corner1 = pieces[14];//gow
                corner2 = pieces[18];//bow
                corner3 = pieces[16];//brw
                corner4 = pieces[12];//grw
                break;
            case 12:
                edge1 = pieces[3];//gy
                edge2 = pieces[9];//ry
                edge3 = pieces[7];//by
                edge4 = pieces[11];//oy
                corner1 = pieces[13];//gry
                corner2 = pieces[17];//bry
                corner3 = pieces[19];//boy
                corner4 = pieces[15];//goy
                break;
        }

        // this part simplifies the side from 1-12 to 1-6
        side = side % 6;
        if (side == 0)
            side = 6;

        // this goes through all the pieces that need change and runs the turned function 
        Piece pieceSave = new PieceEdge(edge1.GetColor(1), edge1.GetColor(2), edge1.GetColor(3), edge1.GetColor(4));
        edge1.Turned(edge2, side);
        edge2.Turned(edge3, side);
        edge3.Turned(edge4, side);
        edge4.Turned(pieceSave, side);
        pieceSave = new PieceCorner(corner1.GetColor(1), corner1.GetColor(2), corner1.GetColor(3), corner1.GetColor(4), corner1.GetColor(5), corner1.GetColor(6));
        corner1.Turned(corner2, side);
        corner2.Turned(corner3, side);
        corner3.Turned(corner4, side);
        corner4.Turned(pieceSave, side);

    }

    //returns a float from 0-1 representing the percent of the 48 movable tiles that are on the correct side
    public float getScoreTile()
    {

        float score = 0;
        int x;
        for (x = 0; x < 20; x++)
        {
            score += pieces[x].GetScoreTile();
        }
        score = score / 48f;//
        return score;
    }

    //returns a float from 0-1 representing the percent of movable pieces that are in their correct location
    public float getScorePiece()
    {
        float score = 0;
        int x;
        for (x = 0; x < 20; x++)
        {
            if (pieces[x].GetScoreTile() == 1)
                score++;
        }
        score = score / 20;
        return score;
    }

    public float[] getInputs()
    {
        float[] inputs = new float[48];
        inputs[0] = pieces[0].GetColor(0);
        int x, y = 0;
        for (x = 0; x < 20; x++)
        {
            if (x < 12)
            {
                inputs[y] = (float)pieces[x].GetColor(0);
                y++;
                inputs[y] = (float)pieces[x].GetColor(1);
                y++;
            }
            else
            {
                inputs[y] = (float)pieces[x].GetColor(0);
                y++;
                inputs[y] = (float)pieces[x].GetColor(1);
                y++;
                inputs[y] = (float)pieces[x].GetColor(2);
                y++;
            }
        }
        return inputs;
    }
}