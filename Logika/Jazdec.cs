namespace com.example.Logika
{
    public class Jazdec : Figurka
    {
        public Jazdec(int x, int y, Strana strana) : base(TypFigurky.JAZDEC, x, y, strana)
        {
        }

        public override bool validnyPohyb(int x, int y, Plocha p)
        {
            if (x < 0 || x > 7 || y < 0 || y > 7)
            {
                return false;
            }
            if ((x == this.getX() + 2 && y == this.getY() + 1) || (x == this.getX() + 2 && y == this.getY() - 1)
                || (x == this.getX() - 2 && y == this.getY() + 1) || (x == this.getX() - 2 && y == this.getY() - 1)
                || (x == this.getX() + 1 && y == this.getY() + 2) || (x == this.getX() + 1 && y == this.getY() - 2)
                || (x == this.getX() - 1 && y == this.getY() + 2) || (x == this.getX() - 1 && y == this.getY() - 2))
            {
                this.setPozicia(x, y);
                return true;
            }
            else
            {
                return false;
            }
        }

        public override bool preskakujeFigurky(int novyX, int novyY, Plocha p)
        {
            return false;
        }
    }
}
