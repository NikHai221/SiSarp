using System;

namespace com.example.Logika
{
    public class Kral : Figurka
    {
        public Kral(int x, int y, Strana strana) : base(TypFigurky.KRAL, x, y, strana)
        {
        }

        public override bool validnyPohyb(int x, int y, Plocha plocha)
        {
            if (x < 0 || x > 7 || y < 0 || y > 7)
            {
                return false;
            }
            if (Math.Abs(x - this.getX()) <= 1 && Math.Abs(y - this.getY()) <= 1)
            {
                this.setPozicia(x, y);
                return true;
            }
            return false;
        }
    }
}