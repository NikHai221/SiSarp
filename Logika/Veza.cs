namespace com.example.Logika
{
    public class Veza : Figurka
    {
        public Veza(int x, int y, Strana strana) : base(TypFigurky.VEZA, x, y, strana)
        {
        }

        public override bool validnyPohyb(int x, int y, Plocha p)
        {
            if (x < 0 || x > 7 || y < 0 || y > 7)
            {
                return false;
            }
            if (x == this.getX() || y == this.getY())
            {
                this.setPozicia(x, y);
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}