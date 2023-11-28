using System;
using System.Threading;
using static cspixpile.Draw;
using PInvoke;

namespace Demo
{
    internal class Demo
    {

        static void showPicture(int X, int Y, Color sky, Color grass, Color houseWalls, Color houseDoor, Color houseDoorHandle, Color houseWindow, Color houseRoof, Color lake, Color sun)
        {
            Console.Write(DrawGlobals.CCLR + DrawGlobals.CLR + MoveCursor((X, Y)));

            Console.Write(new Rectangle('@', sky            , sky            , (0, 0    )      , (X, Y  ), true));
            Console.Write(new Rectangle('g', grass          , grass          , (0, Y - 7)      , (X, 7  ), true));

            Console.Write(new Rectangle('h', houseWalls     , houseWalls     , (X - 23, Y - 13), (21, 10), true));
            Console.Write(new Rectangle('d', houseDoor      , houseDoor      , (X - 21, Y - 9 ), (5 , 6 ), true));
            Console.Write(new Rectangle('h', houseDoorHandle, houseDoorHandle, (X - 18, Y - 7 ), (1 , 2 ), true));
            Console.Write(new Rectangle('w', houseWindow    , houseWindow    , (X - 14, Y - 10), (10, 5 ), true));
            Console.Write(new Rectangle('f', houseWalls     , houseWalls     , (X - 10, Y - 10), (2 , 5 ), true));
            Console.Write(new Rectangle('f', houseWalls     , houseWalls     , (X - 14, Y - 8 ), (10, 1 ), true));

            Console.Write(new Line     ('r', houseRoof      , houseRoof      , (X - 24, Y - 12), (X - 13, Y - 18)));
            Console.Write(new Line     ('r', houseRoof      , houseRoof      , (X - 13, Y - 18), (X - 2 , Y - 12)));
            Console.Write(new Line     ('r', houseRoof      , houseRoof      , (X - 24, Y - 12), (X - 2 , Y - 12)));
            Console.Write(new Line     ('r', houseRoof      , houseRoof      , (X - 21, Y - 13), (X - 5 , Y - 13)));
            Console.Write(new Line     ('r', houseRoof      , houseRoof      , (X - 19, Y - 14), (X - 7 , Y - 14)));
            Console.Write(new Line     ('r', houseRoof      , houseRoof      , (X - 17, Y - 15), (X - 9 , Y - 15)));
            Console.Write(new Line     ('r', houseRoof      , houseRoof      , (X - 15, Y - 16), (X - 11, Y - 16)));

            Console.Write(new Pixel    ('r', houseRoof      , houseRoof      , (X - 13, Y - 17)));

            Console.Write(new Ellipse  ('l', lake           , lake           , (20, Y - 4), (15, 2), true));
            Console.Write(new Ellipse  ('s', sun            , sun            , (16, 7    ), (10, 5), true));

            Thread.Sleep(5000);
        }

        static void Main(string[] args)
        {
            //Run the library
            TryEnableAnsiCodesForHandle(Kernel32.StdHandle.STD_OUTPUT_HANDLE);

            // Terminal size
            (int X, int Y) = (Console.WindowWidth, Console.WindowHeight);

            // Colors
            Color clrBlack  = new Color("#000000", (0, 0, 0), false);
            Color clrRed    = new Color("#ff0000", (0, 0, 0), false);
            Color clrLBlue  = new Color("#13a2e9", (0, 0, 0), false);
            Color clrDBlue  = new Color("#0000ff", (0, 0, 0), false);
            Color clrGreen  = new Color("#0cd205", (0, 0, 0), false);
            Color clrOrange = new Color("#de8500", (0, 0, 0), false);
            Color clrYellow = new Color("#ffff00", (0, 0, 0), false);

            showPicture(X, Y, clrLBlue, clrGreen, clrOrange, clrRed, clrYellow, clrYellow, clrRed, clrDBlue, clrYellow);

            Console.ReadKey();
        }
    }
}
