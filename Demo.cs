using static CSpixpile;

void showDemo(double fps = 25) {
    // Terminal size
    (int X, int Y) term = (Console.WindowWidth, Console.WindowHeight);

    // Colors
    Color clrBlack  = new ("#000000", (0, 0, 0));
    Color clrRed    = new ("#ff0000", (0, 0, 0));
    Color clrLBlue  = new ("#13a2e9", (0, 0, 0));
    Color clrDBlue  = new ("#0000ff", (0, 0, 0));
    Color clrGreen  = new ("#0cd205", (0, 0, 0));
    Color clrOrange = new ("#de8500", (0, 0, 0));
    Color clrYellow = new ("#ffff00", (0, 0, 0));

    // Picture
    Console.Write(DrawGlobals.CLR);
    string pic = "";

    pic += new Rectangle('@', clrLBlue , clrLBlue , (0, 0), (term.X, term.Y), true);
    pic += new Rectangle('g', clrGreen , clrGreen , (0, term.Y-7), (term.X, 7), true);

    pic += new Rectangle('h', clrOrange, clrOrange, (term.X-23, term.Y-13), (21, 10), true);
    pic += new Rectangle('d', clrRed   , clrRed   , (term.X-21, term.Y-9 ), (5 , 6 ), true);
    pic += new Rectangle('h', clrYellow, clrYellow, (term.X-18, term.Y-7 ), (1 , 2 ), true);
    pic += new Rectangle('w', clrYellow, clrYellow, (term.X-14, term.Y-10), (10, 5 ), true);
    pic += new Rectangle('f', clrOrange, clrOrange, (term.X-10, term.Y-10), (2 , 5 ), true);
    pic += new Rectangle('f', clrOrange, clrOrange, (term.X-14, term.Y-8 ), (10, 1 ), true);

    pic += new Line     ('r', clrRed   , clrRed   , (term.X-24, term.Y-12), (term.X-13, term.Y-18));
    pic += new Line     ('r', clrRed   , clrRed   , (term.X-13, term.Y-18), (term.X-2 , term.Y-12));
    pic += new Line     ('r', clrRed   , clrRed   , (term.X-24, term.Y-12), (term.X-2 , term.Y-12));
    pic += new Line     ('r', clrRed   , clrRed   , (term.X-21, term.Y-13), (term.X-5 , term.Y-13));
    pic += new Line     ('r', clrRed   , clrRed   , (term.X-19, term.Y-14), (term.X-7 , term.Y-14));
    pic += new Line     ('r', clrRed   , clrRed   , (term.X-17, term.Y-15), (term.X-9 , term.Y-15));
    pic += new Line     ('r', clrRed   , clrRed   , (term.X-15, term.Y-16), (term.X-11, term.Y-16));

    pic += new Pixel    ('r', clrRed   , clrRed   , (term.X-13, term.Y-17));

    pic += new Ellipse  ('l', clrDBlue , clrDBlue , (20, term.Y-4), (15, 2), true);
    pic += new Ellipse  ('s', clrYellow, clrYellow, (16, 7), (10, 5), true);
    
    pic += DrawGlobals.CCLR+MoveCursor(term);
    
    // Output
    if (fps > 0)
    {
        while (fps>0) { Console.Write(pic); Thread.Sleep((int)(1000/fps)); }
    }
    else
    {
        Console.Write(pic);
    }
}

showDemo(0);