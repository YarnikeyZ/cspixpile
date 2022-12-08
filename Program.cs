string doColor(int r=255, int g=255, int b=255, string hex="") {
    if (hex.Length >= 7) { 
        hex = hex.Replace("#", "");
        r = Convert.ToInt32($"0x{hex[0]}{hex[1]}", 16);
        g = Convert.ToInt32($"0x{hex[2]}{hex[3]}", 16);
        b = Convert.ToInt32($"0x{hex[4]}{hex[5]}", 16);
    }
    return $"\x1b[38;2;{r};{g};{b}m";
}

void drawPixel(char sym, string colorString, int posX, int posY) {
    Console.Write($"{colorString}\x1b[{posY};{posX}H{sym}");
}

void drawRectangle(char sym, string colorString, int posX, int posY, int geomX, int geomY) {
    string line = new string(sym, geomX);
    string rect = colorString;
    for (int gy = 0; gy < geomY; gy++) {
        rect += $"\x1b[{posY+gy};{posX}H{line}";
    }
    Console.Write(rect);
}

void drawEllipse(char sym, string colorString, int posX, int posY, int radX, int radY) {
    double dx, dy, d1, d2, x, y, powRadX, powRadY;
    powRadX = radX * radX;
    powRadY = radY * radY;
    x = 0;
    y = radY;
    d1 = (powRadY) - (powRadX * radY) + (0.25f * powRadX);
    dx = 2 * powRadY * x;
    dy = 2 * powRadX * y;
    while (dx < dy) {
        drawPixel(sym, colorString, (int)(x+posX), (int)(y+posY));
        drawPixel(sym, colorString, (int)(-x+posX), (int)(y+posY));
        drawPixel(sym, colorString, (int)(x+posX), (int)(-y+posY));
        drawPixel(sym, colorString, (int)(-x+posX), (int)(-y+posY));
        if (d1 < 0) {
            x++;
            dx = dx + (2 * powRadY);
            d1 = d1 + dx + (powRadY);
        }
        else {
            x++;
            y--;
            dx = dx + (2 * powRadY);
            dy = dy - (2 * powRadX);
            d1 = d1 + dx - dy + (powRadY);
        }
    }
    d2 = (powRadY * Math.Pow(x + 0.5f, 2)) - (powRadX * Math.Pow(y - 1, 2)) - (powRadX * powRadY);
    while (y >= 0) {
        drawPixel(sym, colorString, (int)(x+posX), (int)(y+posY));
        drawPixel(sym, colorString, (int)(-x+posX), (int)(y+posY));
        drawPixel(sym, colorString, (int)(x+posX), (int)(-y+posY));
        drawPixel(sym, colorString, (int)(-x+posX), (int)(-y+posY));
        if (d2 > 0) {
            y--;
            dy = dy - (2 * powRadX);
            d2 = d2 + (powRadX) - dy;
        }
        else {
            y--;
            x++;
            dx = dx + (2 * powRadY);
            dy = dy - (2 * powRadX);
            d2 = d2 + dx - dy + (powRadX);
        }
    }
}

void drawLine(char sym, string colorString, int x0, int y0, int x1, int y1) {
    int x, y, dX, dY, error, steepY;
    bool steep;
    void Swap<T>(ref T Lhs, ref T Rhs) {
        T temp = Lhs;
        Lhs = Rhs;
        Rhs = temp;
    }
    steep = Math.Abs(y1 - y0) > Math.Abs(x1 - x0);
    if (steep)   { Swap(ref x0, ref y0); Swap(ref x1, ref y1); }
    if (x0 > x1) { Swap(ref x0, ref x1); Swap(ref y0, ref y1); }
    dX = x1 - x0;
    dY = Math.Abs(y1 - y0);
    steepY = (y0 < y1) ? 1 : -1;
    y = y0;
    error = dX / 2;
    for (x = x0; x <= x1; x++) {
        drawPixel(sym, colorString, steep ? y : x, steep ? x : y);
        error -= dY;
        if (error < 0) {
            y += steepY;
            error += dX;
        }
    }
}

void showDemo(double fps = 25) {
    int termX, termY;
    string clr = "\x1b[0;0m\x1b[H\x1b[2J\x1b[3J";
    termX = Console.WindowWidth;
    termY = Console.WindowHeight;
    while (true) {
        Console.Write(clr);
        drawRectangle('@', doColor(0,0,0, "#13a2e9"), 0, 0, termX, termY);
        drawRectangle('g', doColor(0,0,0, "#0cd205"), 0, termY-7, termX, 7);
        drawRectangle('h', doColor(0,0,0, "#de8500"), termX-23, termY-13, 21, 10);
        drawRectangle('h', doColor(0,0,0, "#de8500"), termX-23, termY-13, 21, 10);
        drawRectangle('d', doColor(0,0,0, "#ff0000"), termX-21, termY-9, 5, 6);
        drawRectangle('w', doColor(0,0,0, "#ffff00"), termX-14, termY-10, 10, 5);
        drawRectangle('f', doColor(0,0,0, "#de8500"), termX-10, termY-10, 2, 5);
        drawRectangle('f', doColor(0,0,0, "#de8500"), termX-14, termY-8, 10, 1);
        drawRectangle('h', doColor(0,0,0, "#ffff00"), termX-18, termY-7, 1, 2);
        drawEllipse  ('s', doColor(0,0,0, "#ffff00"), 16, 7, 10, 5);
        drawEllipse  ('s', doColor(0,0,0, "#ffff00"), 16, 7, 9, 5);
        drawEllipse  ('s', doColor(0,0,0, "#ffff00"), 16, 7, 8, 5);
        drawRectangle('s', doColor(0,0,0, "#ffff00"), 9, 3, 15, 9);
        drawEllipse  ('l', doColor(0,0,0, "#0000ff"), 20, termY-4, 15, 2);
        drawRectangle('l', doColor(0,0,0, "#0000ff"), 6, termY-5, 29, 3);
        drawLine     ('r', doColor(0,0,0, "#ff0000"), termX-24, termY-12, termX-13, termY-18);
        drawLine     ('r', doColor(0,0,0, "#ff0000"), termX-13, termY-18, termX-2, termY-12);
        drawLine     ('r', doColor(0,0,0, "#ff0000"), termX-24, termY-12, termX-2, termY-12);
        drawLine     ('r', doColor(0,0,0, "#ff0000"), termX-21, termY-13, termX-5, termY-13);
        drawLine     ('r', doColor(0,0,0, "#ff0000"), termX-19, termY-14, termX-7, termY-14);
        drawLine     ('r', doColor(0,0,0, "#ff0000"), termX-17, termY-15, termX-9, termY-15);
        drawLine     ('r', doColor(0,0,0, "#ff0000"), termX-15, termY-16, termX-11, termY-16);
        drawPixel    ('r', doColor(0,0,0, "#ff0000"), termX-13, termY-17);
        Thread.Sleep((int)(1000/fps));
    }
}

showDemo();