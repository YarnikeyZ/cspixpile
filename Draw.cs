using PInvoke;
using System;

namespace cspixpile
{
    public class Draw
    {
        public static string ColorThem(Color foreground, Color background, bool draw = true)
        {
            string them = "";
            if (!foreground.noColor) { them += $"{DrawGlobals.ESC}[38;2;{foreground.rgb.R};{foreground.rgb.G};{foreground.rgb.B}m"; }
            if (!background.noColor) { them += $"{DrawGlobals.ESC}[48;2;{background.rgb.R};{background.rgb.G};{background.rgb.B}m"; }
            if (draw) { Console.Write(them); }
            return them;
        }

        public static string ColorIt(string text, Color foreground, Color background, bool draw = true)
        {
            string it = "";
            if (!foreground.noColor) { it += $"{DrawGlobals.ESC}[38;2;{foreground.rgb.R};{foreground.rgb.G};{foreground.rgb.B}m"; if (background.noColor) { it += $"{text}{DrawGlobals.CCLR}"; } }
            if (!background.noColor) { it += $"{DrawGlobals.ESC}[48;2;{background.rgb.R};{background.rgb.G};{background.rgb.B}m{text}{DrawGlobals.CCLR}"; }
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
            if (rect1.pos.Y >= rect2.pos.Y) { collision.top = true; }
            if (rect1.pos.Y + rect1.size.Y >= rect2.pos.Y + rect2.size.Y) { collision.bottom = true; }
            if (rect1.pos.X >= rect2.pos.X) { collision.left = true; }
            if (rect1.pos.X + rect1.size.X >= rect2.pos.X + rect2.size.X) { collision.bottom = true; }
            return collision;
        }

        public static bool TryEnableAnsiCodesForHandle(Kernel32.StdHandle stdHandle)
        {
            var consoleHandle = Kernel32.GetStdHandle(stdHandle);
            if (Kernel32.GetConsoleMode(consoleHandle, out var consoleBufferModes) &&
                consoleBufferModes.HasFlag(Kernel32.ConsoleBufferModes.ENABLE_VIRTUAL_TERMINAL_PROCESSING))
                return true;

            consoleBufferModes |= Kernel32.ConsoleBufferModes.ENABLE_VIRTUAL_TERMINAL_PROCESSING;
            return Kernel32.SetConsoleMode(consoleHandle, consoleBufferModes);
        }

        public static class DrawGlobals
        {
            public static string ESC = "\x1b";
            public static string CCLR = $"{ESC}[0;0m";
            public static string CLR = $"{CCLR}{ESC}[H{ESC}[2J{ESC}[3J";
        }

        public class Color
        {
            public Color(string _hex, (int R, int G, int B) _rgb, bool _noColor = false)
            {
                rgb = _rgb;
                hex = _hex;
                noColor = _noColor;

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
            public bool noColor { get; }

            override public string ToString()
            {
                return $"R: {rgb.R}; G: {rgb.G}; B: {rgb.B};";
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
            }

            public char sym { get; set; }
            public Color colorForeground { get; set; }
            public Color colorBackground { get; set; }
            public (int X, int Y) pos { get; set; }
            public string pixel;

            override public string ToString()
            {
                return ColorIt($"{MoveCursor((pos.X, pos.Y), false)}{sym}", colorForeground, colorBackground, false);
            }

            public void draw()
            {
                Console.Write(ToString());
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
            }

            public char sym { get; set; }
            public Color colorForeground { get; set; }
            public Color colorBackground { get; set; }
            private (int X, int Y) beforepos;
            public (int X, int Y) pos
            {
                get => beforepos;
                set { if (value != beforepos) { beforepos = value; top = pos.Y; bottom = pos.Y + size.Y; right = pos.X + size.X; left = pos.X; } }
            }
            private (int X, int Y) beforesize;
            public (int X, int Y) size
            {
                get => beforesize; 
                set { if (value != beforesize) { beforesize = value; top = pos.Y; bottom = pos.Y + size.Y; right = pos.X + size.X; left = pos.X; } }
            }
            public bool fill { get; set; }
            public int top { get; set; }
            public int bottom { get; set; }
            public int left { get; set; }
            public int right { get; set; }
            public string rect;

            override public string ToString()
            {
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
                return rect;
            }

            public void draw()
            {
                Console.Write(ToString());
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
            }

            public char sym { get; set; }
            public Color colorForeground { get; set; }
            public Color colorBackground { get; set; }
            public (int X, int Y) pos { get; set; }
            public (int X, int Y) rad { get; set; }
            public bool fill { get; set; }
            public string ellipse;

            override public string ToString()
            {
                ellipse = ColorThem(colorForeground, colorBackground, false);
                int posX = pos.X;
                int posY = pos.Y;
                int radX = rad.X;
                int radY = rad.Y;
                int x, y;
                double dx, dy, d1, d2, powRadX, powRadY;
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
                                ellipse += $"{MoveCursor((posX + x, posY + y), false)}{sym}";
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
                    ellipse += $"{MoveCursor((x + posX, y + posY), false)}{sym}";
                    ellipse += $"{MoveCursor((-x + posX, y + posY), false)}{sym}";
                    ellipse += $"{MoveCursor((x + posX, -y + posY), false)}{sym}";
                    ellipse += $"{MoveCursor((-x + posX, -y + posY), false)}{sym}";
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
                    ellipse += $"{MoveCursor((x + posX, y + posY), false)}{sym}";
                    ellipse += $"{MoveCursor((-x + posX, y + posY), false)}{sym}";
                    ellipse += $"{MoveCursor((x + posX, -y + posY), false)}{sym}";
                    ellipse += $"{MoveCursor((-x + posX, -y + posY), false)}{sym}";
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
                return ellipse;
            }

            public void draw()
            {
                Console.Write(ToString());
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
            }

            public char sym { get; set; }
            public Color colorForeground { get; set; }
            public Color colorBackground { get; set; }
            public (int X, int Y) pos1 { get; set; }
            public (int X, int Y) pos2 { get; set; }
            public string line;

            override public string ToString()
            {
                line = "";

                int pos1X = pos1.X;
                int pos1Y = pos1.Y;
                int pos2X = pos2.X;
                int pos2Y = pos2.Y;
                int x, y, dX, dY, error, steepY;
                bool steep;
                Pixel pixel = new Pixel(sym, colorForeground, colorBackground, (0, 0));

                steep = Math.Abs(pos2Y - pos1Y) > Math.Abs(pos2X - pos1X);
                if (steep || pos1X > pos2X)
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
                    pixel.pos = (steep ? y : x, steep ? x : y);
                    line += pixel;
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
                Console.Write(ToString());
            }
        }

    }
}
