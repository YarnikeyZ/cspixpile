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

    public int red { get; }
    public int green { get; }
    public int blue { get; }
    public string hexCode { get; }

    override public string ToString()
    {
        return $"\x1b[38;2;{red};{green};{blue}m";
    }
}

public class Pixel
{
    public Pixel(char sym, string colorSring, int posX, int posY)
    {
        psym = sym;
        pcolorString = colorSring;
        pposX = posX;
        pposY = posY;
    }

    public char psym { get; }
    public string pcolorString { get; }
    public int pposX { get; }
    public int pposY { get; }

    public void draw()
    {
        Console.Write($"{pcolorString}\x1b[{pposY};{pposX}H{psym}");
    }
}

public class Rectangle
{
    public Rectangle(char sym, string colorString, int posX, int posY, int geomX, int geomY)
    {
        rsym = sym;
        rcolorString = colorString;
        rposX = posX;
        rposY = posY;
        rgeomX = geomX;
        rgeomY = geomY;
    }

    public char rsym { get; }
    public string rcolorString { get; }
    public int rposX { get; }
    public int rposY { get; }
    public int rgeomX { get; }
    public int rgeomY { get; }

    public void draw()
    {
        string line = new string(rsym, rgeomX);
        string rect = rcolorString;
        for (int gy = 0; gy < rgeomY; gy++)
        {
            rect += $"\x1b[{rposY + gy};{rposX}H{line}";
        }
        Console.Write(rect);
    }
}