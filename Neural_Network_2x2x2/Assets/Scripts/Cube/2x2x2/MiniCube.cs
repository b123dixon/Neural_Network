using UnityEngine;

public class MiniCube
{

    // these colors just make it easier to visualise what is going on
    int GREEN = 1, BLUE = 2, RED = 3, ORANGE = 4, WHITE = 5, YELLOW = 6;
    // these hold all the pieces that can move on a rubik's cube
    public Piece[] pieces = new Piece[8];
    public MiniCube()
    {
        //numbers after dots are adjusted index
        pieces[0] = new PieceCorner(GREEN, RED, WHITE);//0-20 ... 0
        pieces[1] = new PieceCorner(GREEN, RED, YELLOW);//21-41 ... 1
        pieces[2] = new PieceCorner(GREEN, ORANGE, WHITE);//42-62 ... 2
        pieces[3] = new PieceCorner(GREEN, ORANGE, YELLOW);//never moves ... null
        pieces[4] = new PieceCorner(BLUE, RED, WHITE);//63-83 ... 3
        pieces[5] = new PieceCorner(BLUE, RED, YELLOW);//84-104 ... 4
        pieces[6] = new PieceCorner(BLUE, ORANGE, WHITE);//105-125 ... 5
        pieces[7] = new PieceCorner(BLUE, ORANGE, YELLOW);//126-146 ... 6
    }
    public void Reset()
    {
        //resets cube into solved state
        pieces[0] = new PieceCorner(GREEN, RED, WHITE);
        pieces[1] = new PieceCorner(GREEN, RED, YELLOW);
        pieces[2] = new PieceCorner(GREEN, ORANGE, WHITE);
        pieces[3] = new PieceCorner(GREEN, ORANGE, YELLOW);
        pieces[4] = new PieceCorner(BLUE, RED, WHITE);
        pieces[5] = new PieceCorner(BLUE, RED, YELLOW);
        pieces[6] = new PieceCorner(BLUE, ORANGE, WHITE);
        pieces[7] = new PieceCorner(BLUE, ORANGE, YELLOW);
    }

    public void Turn(int side)
    {
        if (side <= 0 || side > 6)
        {
            Debug.Log("Invalid Side");
            return;
        }


        // these are just used to save the pieces that will move to help turn
        Piece corner1 = null, corner2 = null, corner3 = null, corner4 = null;

        //Whatever the color is the turn method will move that side clockwise unless the color has a +6 that will invert the turn
        // and the comments after each piece just show what piece is being grabbed
        switch (side)
        {
            case 1://blue
                corner1 = pieces[4];//brw
                corner2 = pieces[5];//bry
                corner3 = pieces[7];//boy
                corner4 = pieces[6];//bow
                break;
            case 2://red
                corner1 = pieces[0];//gro
                corner2 = pieces[1];//gry
                corner3 = pieces[5];//bry
                corner4 = pieces[4];//brw
                break;
            case 3://white
                corner1 = pieces[4];//brw
                corner2 = pieces[6];//bow
                corner3 = pieces[2];//gow
                corner4 = pieces[0];//grw
                break;
            case 4://blue *
                corner1 = pieces[6];//bow
                corner2 = pieces[7];//boy
                corner3 = pieces[5];//bry
                corner4 = pieces[4];//brw
                break;
            case 5://red *
                corner1 = pieces[4];//brw
                corner2 = pieces[5];//bry
                corner3 = pieces[1];//gry
                corner4 = pieces[0];//grw
                break;
            case 6://white *
                corner1 = pieces[0];//grw
                corner2 = pieces[2];//gow
                corner3 = pieces[6];//bow
                corner4 = pieces[4];//brw
                break;
        }

        // this part simplifies the side from 1-12 to 1-6
        side = side % 3;
        if (side == 0)
            side = 5;
        else if (side == 1)
            side = 2;
        else if (side == 2)
            side = 3;
        // this goes through all the pieces that need change and runs the turned function 
        Piece pieceSave = new PieceCorner(corner1.GetColor(1), corner1.GetColor(2), corner1.GetColor(3), corner1.GetColor(4), corner1.GetColor(5), corner1.GetColor(6));
        corner1.Turned(corner2, side);
        corner2.Turned(corner3, side);
        corner3.Turned(corner4, side);
        corner4.Turned(pieceSave, side);

    }


    public float[] GetInputsFullPiece()
    {
        float[] inputs = new float[147 + 7];
        //goy does not move so is not included in inputs
        //each piece has 21 options
        //split into 7 spots each with 3 orientations
        //7 extra for the steps
        int x;
        for (x = 0; x < pieces.Length; x++)
        {
            if (x != 3)
            {
                int index = FullPieceIndexFinder(x);
                inputs[index] = 1;
                if (x == 2)
                {
                    //Debug.Log("2index: " + index);
                }
            }
        }
        return inputs;
    }
    public int FullPieceIndexFinder(int index)
    {
        //given index is the true placement
        int findex = FindPiece(index);
        int windex;
        if (findex > 2)
        {
            windex = findex + 1;
        }
        else
        {
            windex = findex;
        }

        int bg = PieceOrientation(windex);//1-3 based on what side the blue or green on a corner is on 
        int adder = 3 * findex + bg;
        if (index > 3)
            index--;
        int final = index * 21 + adder;
        return final;
    }

    public int WhatPiece(int index)
    {
        // determines the adjusted index of what piece is at true index
        int c1 = pieces[index].GetColor(1);
        int c2 = pieces[index].GetColor(2);
        int c3 = pieces[index].GetColor(3);

        int piece = 0;

        if (c1 == 2 || c2 == 2 || c3 == 2)
        {
            piece += 4;
        }
        if (c1 == 4 || c2 == 4 || c3 == 4)
        {
            piece += 2;
        }
        if (c1 == 6 || c2 == 6 || c3 == 6)
        {
            piece += 1;
        }
        if (piece > 3)
            piece--;
        return piece;
    }
    public int FindPiece(int piece)
    {
        //finds the adjusted index of piece that belongs in the given normal index
        int x;
        for (x = 0; x < 8; x++)
        {
            if (x != 3)
            {
                int maybe = WhatPiece(x);
                if (maybe > 2)
                    maybe++;
                if (piece == maybe)
                {
                    if (x > 3)
                        return x - 1;
                    return x;
                }
            }
        }
        //this will never happen but the compiler is unhappy
        return -1;
    }

    public int PieceOrientation(int index)
    {
        //gives orientation of given true index
        //bg on bg = 0 : bg on ro = 1 : bg on wy = 2

        Piece piece = pieces[index];
        if (piece.GetColorFromMid(1) == 1 || piece.GetColorFromMid(2) == 1 ||
            piece.GetColorFromMid(1) == 2 || piece.GetColorFromMid(2) == 2)
        {
            return 0;
        }
        if (piece.GetColorFromMid(3) == 1 || piece.GetColorFromMid(4) == 1 ||
            piece.GetColorFromMid(3) == 2 || piece.GetColorFromMid(4) == 2)
        {
            return 1;
        }
        if (piece.GetColorFromMid(5) == 1 || piece.GetColorFromMid(6) == 1 ||
            piece.GetColorFromMid(5) == 2 || piece.GetColorFromMid(6) == 2)
        {
            return 2;
        }
        return 90;
    }

    public float[] GetInputsFullTile()
    {
        float[] inputs = new float[126];
        //goy does not move so is not included those tiles in inputs
        //each tile has 6 options
        return inputs;
    }
}
