static partial class Draw
{
    public static string doColor(int r = 255, int g = 255, int b = 255, string hex = "")
    {
        if (hex.Length >= 7)
        {
            hex = hex.Replace("#", "");
            r = Convert.ToInt32($"0x{hex[0]}{hex[1]}", 16);
            g = Convert.ToInt32($"0x{hex[2]}{hex[3]}", 16);
            b = Convert.ToInt32($"0x{hex[4]}{hex[5]}", 16);
        }
        return $"\x1b[38;2;{r};{g};{b}m";
    }
    public static void drawPixel(char sym, string colorString, int posX, int posY)
    {
        Console.Write($"{colorString}\x1b[{posY};{posX}H{sym}");
    }

    public static void drawRectangle(char sym, string colorString, int posX, int posY, int geomX, int geomY)
    {
        string line = new string(sym, geomX);
        string rect = colorString;
        for (int gy = 0; gy < geomY; gy++)
        {
            rect += $"\x1b[{posY + gy};{posX}H{line}"; 
        }
        Console.Write(rect);
    }

    public static void drawEllipse(char sym, string colorString, int posX, int posY, int radX, int radY, bool fill)
    {
        double dx, dy, d1, d2, x, y, powRadX, powRadY;
        powRadX = radX * radX;
        powRadY = radY * radY;
        
        if (fill)
        {
            for(y=-radY; y<=radY; y++) {
                for(x=-radX; x<=radX; x++) {
                    if(x*x*radY*radY+y*y*radX*radX <= radY*radY*radX*radX)
                        drawPixel(sym, colorString, (int)(posX + x), (int)(posY + y));
                }
            }
        }
        
        x = 0;
        y = radY;
        d1 = powRadY - powRadX * radY + 0.25f * powRadX;
        dx = 2 * powRadY * x;
        dy = 2 * powRadX * y;

        while (dx < dy)
        {
            drawPixel(sym, colorString, (int)(x + posX), (int)(y + posY));
            drawPixel(sym, colorString, (int)(-x + posX), (int)(y + posY));
            drawPixel(sym, colorString, (int)(x + posX), (int)(-y + posY));
            drawPixel(sym, colorString, (int)(-x + posX), (int)(-y + posY));
            if (d1 < 0)
            {
                x++;
                dx = dx + 2 * powRadY;
                d1 = d1 + dx + powRadY;
            }
            else
            {
                x++;
                y--;
                dx = dx + 2 * powRadY;
                dy = dy - 2 * powRadX;
                d1 = d1 + dx - dy + powRadY;
            }
        }
        d2 = powRadY * Math.Pow(x + 0.5f, 2) - powRadX * Math.Pow(y - 1, 2) - powRadX * powRadY;
        while (y >= 0)
        {
            drawPixel(sym, colorString, (int)(x + posX), (int)(y + posY));
            drawPixel(sym, colorString, (int)(-x + posX), (int)(y + posY));
            drawPixel(sym, colorString, (int)(x + posX), (int)(-y + posY));
            drawPixel(sym, colorString, (int)(-x + posX), (int)(-y + posY));
            if (d2 > 0)
            {
                y--;
                dy = dy - 2 * powRadX;
                d2 = d2 + powRadX - dy;
            }
            else
            {
                y--;
                x++;
                dx = dx + 2 * powRadY;
                dy = dy - 2 * powRadX;
                d2 = d2 + dx - dy + powRadX;
            }
        }
    }

    public static void drawLine(char sym, string colorString, int x0, int y0, int x1, int y1)
    {
        int x, y, dX, dY, error, steepY;
        bool steep;
        void Swap<T>(ref T Lhs, ref T Rhs)
        {
            T temp = Lhs;
            Lhs = Rhs;
            Rhs = temp;
        }
        steep = Math.Abs(y1 - y0) > Math.Abs(x1 - x0);
        if (steep) { Swap(ref x0, ref y0); Swap(ref x1, ref y1); }
        if (x0 > x1) { Swap(ref x0, ref x1); Swap(ref y0, ref y1); }
        dX = x1 - x0;
        dY = Math.Abs(y1 - y0);
        steepY = y0 < y1 ? 1 : -1;
        y = y0;
        error = dX / 2;
        for (x = x0; x <= x1; x++)
        {
            drawPixel(sym, colorString, steep ? y : x, steep ? x : y);
            error -= dY;
            if (error < 0)
            {
                y += steepY;
                error += dX;
            }
        }
    }
}