void showDemo(double fps = 25) {
    // Terminal size
    (int X, int Y) term = (Console.WindowWidth, Console.WindowHeight);

    // Colors
    string clrRed    = new Color("#ff0000", (0,0,0)).ToString();
    string clrLBlue  = new Color("#13a2e9", (0,0,0)).ToString();
    string clrDBlue  = new Color("#0000ff", (0,0,0)).ToString();
    string clrGreen  = new Color("#0cd205", (0,0,0)).ToString();
    string clrOrange = new Color("#de8500", (0,0,0)).ToString();
    string clrYellow = new Color("#ffff00", (0,0,0)).ToString();

    // Picture
    Console.Write(DrawGlobals.CLR);
    string pic = "";

    pic += new Rectangle('@', clrLBlue , (0, 0), (term.X, term.Y), true);
    pic += new Rectangle('g', clrGreen , (0, term.Y-7), (term.X, 7), true);

    pic += new Rectangle('h', clrOrange, (term.X-23, term.Y-13), (21, 10), true);
    pic += new Rectangle('d', clrRed   , (term.X-21, term.Y-9 ), (5 , 6 ), true);
    pic += new Rectangle('h', clrYellow, (term.X-18, term.Y-7 ), (1 , 2 ), true);
    pic += new Rectangle('w', clrYellow, (term.X-14, term.Y-10), (10, 5 ), true);
    pic += new Rectangle('f', clrOrange, (term.X-10, term.Y-10), (2 , 5 ), true);
    pic += new Rectangle('f', clrOrange, (term.X-14, term.Y-8 ), (10, 1 ), true);

    pic += new Line     ('r', clrRed   , (term.X-24, term.Y-12), (term.X-13, term.Y-18));
    pic += new Line     ('r', clrRed   , (term.X-13, term.Y-18), (term.X-2 , term.Y-12));
    pic += new Line     ('r', clrRed   , (term.X-24, term.Y-12), (term.X-2 , term.Y-12));
    pic += new Line     ('r', clrRed   , (term.X-21, term.Y-13), (term.X-5 , term.Y-13));
    pic += new Line     ('r', clrRed   , (term.X-19, term.Y-14), (term.X-7 , term.Y-14));
    pic += new Line     ('r', clrRed   , (term.X-17, term.Y-15), (term.X-9 , term.Y-15));
    pic += new Line     ('r', clrRed   , (term.X-15, term.Y-16), (term.X-11, term.Y-16));

    pic += new Pixel    ('r', clrRed   , (term.X-13, term.Y-17));

    pic += new Ellipse  ('l', clrDBlue , (20, term.Y-4), (15, 2), true);
    pic += new Ellipse  ('s', clrYellow, (16, 7), (10, 5), true);

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