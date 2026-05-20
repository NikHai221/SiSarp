namespace com.example.Logika
{
    public class Dama : Figurka
    {
        public Dama(int x, int y, Strana strana) : base(TypFigurky.DAMA, x, y, strana)
        {
        }

        public override bool validnyPohyb(int x, int y, Plocha p)
        {
            if (x < 0 || x > 7 || y < 0 || y > 7)
            {
                return false;
            }
            if (x == this.getX() || y == this.getY() || x - this.getX() == y - this.getY() || x - this.getX() == this.getY() - y)
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