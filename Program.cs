void drawPixel(char sym, int colorId, int posX, int posY) {
    Console.SetCursorPosition(posY, posX);
    Console.Write($"\x1b[38;5;{colorId}m\x1b[{posY};{posX}H{sym}");
}

void drawRectangle(char sym, int colorId, int posX, int posY, int geomX, int geomY) {
    string line = new string(sym, geomX);
    string rect = $"\x1b[38;5;{colorId}m";
    for (int gy = 0; gy < geomY; gy++) {
        rect += $"\x1b[{posY+gy};{posX}H{line}";
    }
    Console.Write(rect);
}

void drawEllipse(char sym, int colorId, int posX, int posY, int radX, int radY) {
    double dx, dy, d1, d2, x, y, powRadX, powRadY;
    powRadX = radX * radX;
    powRadY = radY * radY;
    x = 0;
    y = radY;
    d1 = (powRadY) - (powRadX * radY) + (0.25f * powRadX);
    dx = 2 * powRadY * x;
    dy = 2 * powRadX * y;
    while (dx < dy) {
        drawPixel(sym, colorId, (int)(x+posX), (int)(y+posY));
        drawPixel(sym, colorId, (int)(-x+posX), (int)(y+posY));
        drawPixel(sym, colorId, (int)(x+posX), (int)(-y+posY));
        drawPixel(sym, colorId, (int)(-x+posX), (int)(-y+posY));
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
        drawPixel(sym, colorId, (int)(x+posX), (int)(y+posY));
        drawPixel(sym, colorId, (int)(-x+posX), (int)(y+posY));
        drawPixel(sym, colorId, (int)(x+posX), (int)(-y+posY));
        drawPixel(sym, colorId, (int)(-x+posX), (int)(-y+posY));
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

// void drawLine(char sym, int colorId, int x0, int y0, int x1, int y1) {
//     double x, y, dX, dY, dDX, dDY, length;
//     dX = x1 - x0;
//     dY = y1 - y0;
//     if (Math.Abs(dX) >= Math.Abs(dY)) {
//         length = Math.Abs(dX);
//     }
//     else {
//         length = Math.Abs(dY);
//     }
//     dDX = dX / length;
//     dDY = dY / length;
//     x = x0;
//     y = y0;
//     drawPixel(sym, colorId, (int)x, (int)y);
//     for (int i = 0; i < length; i++) {
//         x += dDX;
//         y += dDY;
//         drawPixel(sym, colorId, (int)x, (int)y);
//     }
// }


void drawLine(char sym, int colorId, int x0, int y0, int x1, int y1) {
    double dX, dY, error, error2;
    int signX, signY;
    dX = Math.Abs(x1 - x0);
    dY = Math.Abs(x1 - x0);
    signX = x0 < x1 ? 1 : -1;
    signY = y0 < y1 ? 1 : -1;
    error = dX - dY;
    drawPixel(sym, colorId, (int)x1, (int)y1);
    while (x0 != x1 && y0 != y1) {
        drawPixel(sym, colorId, (int)x0, (int)y0);
        error2 = error * 2;
        if (error2 > -dY) {
            error -= dY;
            x0 += signX;
        }
        if (error2 < dX) {
            error += dX;
            y0 += signY;
        }
    }
}



void showDemo(double fps = 25) {
    int canvasX, canvasY;
    string clr = "\x1b[0;0m\x1b[H\x1b[2J\x1b[3J";
    canvasX = Console.WindowWidth;
    canvasY = Console.WindowHeight;
    while (true) {
        Console.Write(clr);
        drawRectangle('@', 117, 0, 0, canvasX, canvasY);
        drawRectangle('g', 40, 0, canvasY-7, canvasX, 7);
        drawRectangle('h', 208, canvasX-23, canvasY-13, 21, 10);
        drawRectangle('h', 208, canvasX-23, canvasY-13, 21, 10);
        drawRectangle('d', 9, canvasX-21, canvasY-9, 5, 6);
        drawRectangle('w', 11, canvasX-14, canvasY-10, 10, 5);
        drawRectangle('f', 208, canvasX-10, canvasY-10, 2, 5);
        drawRectangle('f', 208, canvasX-14, canvasY-8, 10, 1);
        drawRectangle('h', 11, canvasX-18, canvasY-7, 1, 2);
        drawEllipse('s', 11, 16, 7, 10, 5);
        drawEllipse('s', 11, 16, 7, 9, 5);
        drawEllipse('s', 11, 16, 7, 8, 5);
        drawRectangle('s', 11, 9, 3, 15, 9);
        drawEllipse('l', 21, 20, canvasY-4, 15, 2);
        drawRectangle('l', 21, 6, canvasY-5, 29, 3);
        drawLine('r', 9, canvasX-24, canvasY-12, canvasX-13, canvasY-18);
        drawLine('r', 9, canvasX-13, canvasY-18, canvasX-2, canvasY-12);
        drawLine('r', 9, canvasX-24, canvasY-12, canvasX-2, canvasY-12);
        drawLine('r', 9, canvasX-21, canvasY-13, canvasX-5, canvasY-13);
        drawLine('r', 9, canvasX-19, canvasY-14, canvasX-7, canvasY-14);
        drawLine('r', 9, canvasX-17, canvasY-15, canvasX-9, canvasY-15);
        drawLine('r', 9, canvasX-15, canvasY-16, canvasX-11, canvasY-16);
        Thread.Sleep((int)(1000/fps));
    }
}

showDemo(0.0001);