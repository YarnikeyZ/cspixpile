public class CSpixpile
{
    public static string ColorThem(Color foreground, Color background, bool draw = true)
    {
        string them = $"{DrawGlobals.ESC}[38;2;{foreground.rgb.R};{foreground.rgb.G};{foreground.rgb.B}m{DrawGlobals.ESC}[48;2;{background.rgb.R};{background.rgb.G};{background.rgb.B}m";
        if (draw) { Console.Write(them); }
        return them;
    }

    public static string ColorIt(string text, Color foreground, Color background, bool draw = true)
    {
        string it = $"{DrawGlobals.ESC}[38;2;{foreground.rgb.R};{foreground.rgb.G};{foreground.rgb.B}m{DrawGlobals.ESC}[48;2;{background.rgb.R};{background.rgb.G};{background.rgb.B}m{text}{DrawGlobals.CCLR}";
        if (draw) { Console.Write(it); }
        return it;
    }

    public static string MoveCursor((int X, int Y) pos, bool draw = true)
    {
        string cursor = $"{DrawGlobals.ESC}[{pos.Y};{pos.X}H";
        if (draw) { Console.Write(cursor); }
        return cursor;
    }

    public static (bool top, bool bottom, bool left, bool right) CheckCollision(Rectangle rect1, Rectangle rect2)
    {
        (bool top, bool bottom, bool left, bool right) collision = (false, false, false, false);
        if (rect1.pos.Y >= rect2.pos.Y)                               { collision.top    = true; }
        if (rect1.pos.Y + rect1.size.Y >= rect2.pos.Y + rect2.size.Y) { collision.bottom = true; }
        if (rect1.pos.X >= rect2.pos.X)                               { collision.left   = true; }
        if (rect1.pos.X + rect1.size.X >= rect2.pos.X + rect2.size.X) { collision.bottom = true; }
        return collision;
    }

    public static class DrawGlobals
    {
        public static string ESC = "\x1b";
        public static string CCLR = $"{ESC}[0;0m";
        public static string CLR = $"{CCLR}{ESC}[H{ESC}[2J{ESC}[3J";
    }

    public class Color
    {
        public Color(string _hex, (int R, int G, int B) _rgb)
        {
            rgb = _rgb;
            hex = _hex;
            color = "";

            int R = _rgb.R;
            int G = _rgb.G;
            int B = _rgb.B;
            if (hex.Length >= 7)
            {
                hex = hex.Replace("#", "");
                R = Convert.ToInt32($"0x{hex[0]}{hex[1]}", 16);
                G = Convert.ToInt32($"0x{hex[2]}{hex[3]}", 16);
                B = Convert.ToInt32($"0x{hex[4]}{hex[5]}", 16);
            }
            rgb = (R, G, B);
        }


        public (int R, int G, int B) rgb { get; set; }
        public string hex { get; set; }
        public string color;

        override public string ToString()
        {
            return color;
        }
    }

    public class Pixel
    {
        public Pixel(char _sym, Color _colorForeground, Color _colorBackground, (int _X, int _Y) _pos)
        {
            sym = _sym;
            colorForeground = _colorForeground;
            colorBackground = _colorBackground;
            pos = _pos;
            pixel = ColorIt($"{MoveCursor((pos.X, pos.Y), false)}{sym}", colorForeground, colorBackground, false);
        }

        public char sym { get; set; }
        public Color colorForeground { get; set; }
        public Color colorBackground { get; set; }
        public (int X, int Y) pos { get; set; }
        public string pixel;

        public void draw()
        {
            Console.Write(pixel);
        }

        override public string ToString()
        {
            return pixel;
        }
    }

    public class Rectangle
    {
        public Rectangle(char _sym, Color _colorForeground, Color _colorBackground, (int _X, int _Y) _pos, (int _X, int _Y) _size, bool _fill)
        {
            sym = _sym;
            colorForeground = _colorForeground;
            colorBackground = _colorBackground;
            pos = _pos;
            size = _size;
            fill = _fill;

            rect = ColorThem(colorForeground, colorBackground, false);
            string line = new string(sym, size.X);

            if (fill)
            {
                for (int gy = 0; gy < size.Y; gy++)
                {
                    rect += $"{MoveCursor((pos.X, pos.Y + gy), false)}{line}";
                }
            }
            else
            {
                rect += $"{MoveCursor((pos.X, pos.Y), false)}{line}";
                for (int gy = 0; gy < size.Y; gy++)
                {
                    rect += $"{MoveCursor((pos.X, pos.Y + gy), false)}{sym}{MoveCursor((pos.X + size.X - 1, pos.Y + gy), false)}{sym}";
                }
                rect += $"{MoveCursor((pos.X, pos.Y + size.Y - 1), false)}{line}";
            }
        }

        public char sym { get; set; }
        public Color colorForeground { get; set; }
        public Color colorBackground { get; set; }
        public (int X, int Y) pos { get; set; }
        public (int X, int Y) size { get; set; }
        public bool fill { get; set; }
        public string rect;

        public void draw()
        {
            Console.Write(rect);
        }

        override public string ToString()
        {
            return rect;
        }
    }

    public class Ellipse
    {
        public Ellipse(char _sym, Color _colorForeground, Color _colorBackground, (int _X, int _Y) _pos, (int _X, int _Y) _rad, bool _fill)
        {
            sym = _sym;
            colorForeground = _colorForeground;
            colorBackground = _colorBackground;
            pos = _pos;
            rad = _rad;
            fill = _fill;
            ellipse = "";
            int posX = pos.X;
            int posY = pos.Y;
            int radX = rad.X;
            int radY = rad.Y;
            double dx, dy, d1, d2, x, y, powRadX, powRadY;
            powRadX = radX * radX;
            powRadY = radY * radY;

            if (fill)
            {
                for (y = -radY; y <= radY; y++)
                {
                    for (x = -radX; x <= radX; x++)
                    {
                        if (x * x * radY * radY + y * y * radX * radX <= radY * radY * radX * radX)
                        {
                            ellipse += new Pixel(sym, colorForeground, colorBackground, ((int)(posX + x), (int)(posY + y)));
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
                ellipse += new Pixel(sym, colorForeground, colorBackground, ((int)(x + posX), (int)(y + posY)));
                ellipse += new Pixel(sym, colorForeground, colorBackground, ((int)(-x + posX), (int)(y + posY)));
                ellipse += new Pixel(sym, colorForeground, colorBackground, ((int)(x + posX), (int)(-y + posY)));
                ellipse += new Pixel(sym, colorForeground, colorBackground, ((int)(-x + posX), (int)(-y + posY)));
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
                ellipse += new Pixel(sym, colorForeground, colorBackground, ((int)(x + posX), (int)(y + posY)));
                ellipse += new Pixel(sym, colorForeground, colorBackground, ((int)(-x + posX), (int)(y + posY)));
                ellipse += new Pixel(sym, colorForeground, colorBackground, ((int)(x + posX), (int)(-y + posY)));
                ellipse += new Pixel(sym, colorForeground, colorBackground, ((int)(-x + posX), (int)(-y + posY)));
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
        public Color colorForeground { get; set; }
        public Color colorBackground { get; set; }
        public (int X, int Y) pos { get; set; }
        public (int X, int Y) rad { get; set; }
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
        public Line(char _sym, Color _colorForeground, Color _colorBackground, (int _X, int _Y) _pos1, (int _X, int _Y) _pos2)
        {
            sym = _sym;
            colorForeground = _colorForeground;
            colorBackground = _colorBackground;
            pos1 = _pos1;
            pos2 = _pos2;
            line = "";

            int pos1X = pos1.X;
            int pos1Y = pos1.Y;
            int pos2X = pos2.X;
            int pos2Y = pos2.Y;
            int x, y, dX, dY, error, steepY;
            bool steep;

            steep = Math.Abs(pos2Y - pos1Y) > Math.Abs(pos2X - pos1X);
            if (steep)
            {
                var temp = pos1X;
                pos1X = pos1Y;
                pos1Y = temp;
                temp = pos2X;
                pos2X = pos1Y;
                pos1Y = temp;
            }
            if (pos1X > pos2X)
            {
                var temp = pos1X;
                pos1X = pos1Y;
                pos1Y = temp;
                temp = pos2X;
                pos2X = pos1Y;
                pos1Y = temp;
            }
            dX = pos2X - pos1X;
            dY = Math.Abs(pos2Y - pos1Y);
            steepY = pos1Y < pos2Y ? 1 : -1;
            y = pos1Y;
            error = dX / 2;
            for (x = pos1X; x <= pos2X; x++)
            {
                line += new Pixel(sym, colorForeground, colorBackground, (steep ? y : x, steep ? x : y));
                error -= dY;
                if (error < 0)
                {
                    y += steepY;
                    error += dX;
                }
            }
        }

        public char sym { get; set; }
        public Color colorForeground { get; set; }
        public Color colorBackground { get; set; }
        public (int X, int Y) pos1 { get; set; }
        public (int X, int Y) pos2 { get; set; }
        public string line;

        public void draw()
        {
            Console.Write(line);
        }

        override public string ToString()
        {
            return line;
        }
    }
}