public class Color 
{
    public Color(string hex, int r, int g, int b)
    {
        red = r;
        green = g;
        blue = b;
        hexCode = hex;
        if (hexCode.Length >= 7)
        {
            hexCode = hexCode.Replace("#", "");
            red = Convert.ToInt32($"0x{hexCode[0]}{hexCode[1]}", 16);
            green = Convert.ToInt32($"0x{hexCode[2]}{hexCode[3]}", 16);
            blue = Convert.ToInt32($"0x{hexCode[4]}{hexCode[5]}", 16);
        }
    }

    public int red { get; set; }
    public int green { get; set; }
    public int blue { get; set; }
    public string hexCode { get; set; }

    override public string ToString()
    {
        return $"\x1b[38;2;{red};{green};{blue}m";
    }
}

public class Pixel
{
    public Pixel(char psym, string pcolorString, int pposX, int pposY)
    {
        sym = psym;
        colorString = pcolorString;
        posX = pposX;
        posY = pposY;
    }

    public char sym { get; set; }
    public string colorString { get; set; }
    public int posX { get; set; }
    public int posY { get; set; }

    override public string ToString()
    {
        return $"{colorString}\x1b[{posY};{posX}H{sym}";
    }
    public void draw()
    {
        Console.Write($"{colorString}\x1b[{posY};{posX}H{sym}");
    }
}

public class Rectangle
{
    public Rectangle(char rsym, string rcolorString, int rposX, int rposY, int rgeomX, int rgeomY)
    {
        sym = rsym;
        colorString = rcolorString;
        posX = rposX;
        posY = rposY;
        geomX = rgeomX;
        geomY = rgeomY;
    }

    public char sym { get; set; }
    public string colorString { get; set; }
    public int posX { get; set; }
    public int posY { get; set; }
    public int geomX { get; set; }
    public int geomY { get; set; }

    public void draw()
    {
        string line = new string(sym, geomX);
        string rect = colorString;
        for (int gy = 0; gy < geomY; gy++)
        {
            rect += $"\x1b[{posY + gy};{posX}H{line}";
        }
        Console.Write(rect);
    }
    override public string ToString()
    {
        string line = new string(sym, geomX);
        string rect = colorString;
        for (int gy = 0; gy < geomY; gy++)
        {
            rect += $"\x1b[{posY + gy};{posX}H{line}";
        }
        return rect;
    }
}

public class Ellipse
{
    public Ellipse(char esym, string ecolorString, int eposX, int eposY, int eradX, int eradY, bool efill)
    {
        sym = esym;
        colorString = ecolorString;
        posX = eposX;
        posY = eposY;
        radX = eradX;
        radY = eradY;
        fill = efill;
        ellipse = "";
        double dx, dy, d1, d2, x, y, powRadX, powRadY;
        powRadX = radX * radX;
        powRadY = radY * radY;
        
        if (fill) {
            for(y=-radY; y<=radY; y++) {
                for(x=-radX; x<=radX; x++) {
                    if (x*x*radY*radY+y*y*radX*radX <= radY*radY*radX*radX) {
                        ellipse += new Pixel(sym, colorString, (int)(posX + x), (int)(posY + y));
                    }
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
            ellipse += new Pixel(sym, colorString, (int)(x + posX), (int)(y + posY));
            ellipse += new Pixel(sym, colorString, (int)(-x + posX), (int)(y + posY));
            ellipse += new Pixel(sym, colorString, (int)(x + posX), (int)(-y + posY));
            ellipse += new Pixel(sym, colorString, (int)(-x + posX), (int)(-y + posY));
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
            ellipse += new Pixel(sym, colorString, (int)(x + posX), (int)(y + posY));
            ellipse += new Pixel(sym, colorString, (int)(-x + posX), (int)(y + posY));
            ellipse += new Pixel(sym, colorString, (int)(x + posX), (int)(-y + posY));
            ellipse += new Pixel(sym, colorString, (int)(-x + posX), (int)(-y + posY));
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

    public char sym { get; set; }
    public string colorString { get; set; }
    public int posX { get; set; }
    public int posY { get; set; }
    public int radX { get; set; }
    public int radY { get; set; }
    public bool fill { get; set; }
    public string ellipse;
    public void draw()
    {
        Console.Write(ellipse);
    }
    override public string ToString()
    {
        return ellipse;
    }
}

public class Line
{
    public Line(char lsym, string lcolorString, int lx0, int ly0, int lx1, int ly1)
    {
        sym = lsym;
        colorString = lcolorString;
        x0 = lx0;
        y0 = ly0;
        x1 = lx1;
        y1 = ly1;
    }

    public char sym { get; set; }
    public string colorString { get; set; }
    public int x0 { get; set; }
    public int y0 { get; set; }
    public int x1 { get; set; }
    public int y1 { get; set; }

    override public string ToString()
    {
        int x, y, dX, dY, error, steepY;
        bool steep;
        string line = "";
        steep = Math.Abs(y1 - y0) > Math.Abs(x1 - x0);
        if (steep) 
        {
            var temp = x0;
            x0 = y0;
            y0 = temp;
            temp = x1;
            x1 = y0;
            y0 = temp;
        }
        if (x0 > x1) 
        {
            var temp = x0;
            x0 = y0;
            y0 = temp;
            temp = x1;
            x1 = y0;
            y0 = temp;
        }
        dX = x1 - x0;
        dY = Math.Abs(y1 - y0);
        steepY = y0 < y1 ? 1 : -1;
        y = y0;
        error = dX / 2;
        for (x = x0; x <= x1; x++)
        {
            line += new Pixel(sym, colorString, steep ? y : x, steep ? x : y);
            error -= dY;
            if (error < 0)
            {
                y += steepY;
                error += dX;
            }
        }
        return line;
    }
    public void draw()
    {
        int x, y, dX, dY, error, steepY;
        bool steep;
        Pixel pix = new Pixel(sym, colorString, 0,0);
        string line = "";
        steep = Math.Abs(y1 - y0) > Math.Abs(x1 - x0);
        if (steep) 
        {
            var temp = x0;
            x0 = y0;
            y0 = temp;
            temp = x1;
            x1 = y0;
            y0 = temp;
        }
        if (x0 > x1) 
        {
            var temp = x0;
            x0 = y0;
            y0 = temp;
            temp = x1;
            x1 = y0;
            y0 = temp;
        }
        dX = x1 - x0;
        dY = Math.Abs(y1 - y0);
        steepY = y0 < y1 ? 1 : -1;
        y = y0;
        error = dX / 2;
        for (x = x0; x <= x1; x++)
        {
            pix.posX = steep ? y : x;
            pix.posY = steep ? x : y;
            line += pix;
            error -= dY;
            if (error < 0)
            {
                y += steepY;
                error += dX;
            }
        }
    Console.Write(line);
    }
}