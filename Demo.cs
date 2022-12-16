void showDemo(double fps = 25) {
    // Terminal size
    int termX = Console.WindowWidth;
    int termY = Console.WindowHeight;

    // Colors
    string clrRed    = new Color("#ff0000", 0,0,0).ToString();
    string clrLBlue  = new Color("#13a2e9", 0,0,0).ToString();
    string clrDBlue  = new Color("#0000ff", 0,0,0).ToString();
    string clrGreen  = new Color("#0cd205", 0,0,0).ToString();
    string clrOrange = new Color("#de8500", 0,0,0).ToString();
    string clrYellow = new Color("#ffff00", 0,0,0).ToString();

    // Picture
    Console.Write("\x1b[0;0m\x1b[H\x1b[2J\x1b[3J");
    while (true)
    {
        Draw.drawRectangle('@', clrLBlue , 0, 0, termX, termY);
        Draw.drawRectangle('g', clrGreen , 0, termY-7, termX, 7);
        Draw.drawRectangle('h', clrOrange, termX-23, termY-13, 21, 10);
        Draw.drawRectangle('d', clrRed   , termX-21, termY-9, 5, 6);
        Draw.drawRectangle('h', clrYellow, termX-18, termY-7, 1, 2);
        Draw.drawRectangle('w', clrYellow, termX-14, termY-10, 10, 5);
        Draw.drawRectangle('f', clrOrange, termX-10, termY-10, 2, 5);
        Draw.drawRectangle('f', clrOrange, termX-14, termY-8, 10, 1);
        Draw.drawLine     ('r', clrRed   , termX-24, termY-12, termX-13, termY-18);
        Draw.drawLine     ('r', clrRed   , termX-13, termY-18, termX-2, termY-12);
        Draw.drawLine     ('r', clrRed   , termX-24, termY-12, termX-2, termY-12);
        Draw.drawLine     ('r', clrRed   , termX-21, termY-13, termX-5, termY-13);
        Draw.drawLine     ('r', clrRed   , termX-19, termY-14, termX-7, termY-14);
        Draw.drawLine     ('r', clrRed   , termX-17, termY-15, termX-9, termY-15);
        Draw.drawLine     ('r', clrRed   , termX-15, termY-16, termX-11, termY-16);
        Draw.drawPixel    ('r', clrRed   , termX-13, termY-17);
        Draw.drawEllipse  ('l', clrDBlue , 20, termY-4, 15, 2, true);
        Draw.drawEllipse  ('s', clrYellow, 16, 7, 10, 5, true);
        Thread.Sleep((int)(1000/fps));
    }
}

showDemo();