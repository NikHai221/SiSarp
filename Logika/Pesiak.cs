namespace com.example.Logika
{
    public class Pesiak : Figurka
    {
        private readonly int prvaPoziciaX;
        private readonly int prvaPoziciaY;

        public Pesiak(int x, int y, Strana strana) : base(TypFigurky.PESIAK, x, y, strana)
        {
            this.prvaPoziciaX = x;
            this.prvaPoziciaY = y;
        }

        public override bool validnyPohyb(int x, int y, Plocha plocha)
        {
            if (x < 0 || x > 7 || y < 0 || y > 7)
            {
                return false;
            }

            if (this.getStrana() == Strana.BIELA)
            {
                if (y == this.getY() && x == this.getX() + 1 && plocha.getFigurka(x, y) == null)
                {
                    this.setPozicia(x, y);
                    return true;
                }
                else if (y == this.getY() && x == this.getX() + 2
                    && (this.prvaPoziciaX == this.getX() && this.prvaPoziciaY == this.getY()) && plocha.getFigurka(x, y) == null && plocha.getFigurka(x - 1, y) == null)
                {
                    this.setPozicia(x, y);
                    return true;
                }
                else if (y == this.getY() + 1 && x == this.getX() + 1 && plocha.getFigurka(x, y) != null)
                {
                    this.setPozicia(x, y);
                    return true;
                }
                else if (y == this.getY() - 1 && x == this.getX() + 1 && plocha.getFigurka(x, y) != null)
                {
                    this.setPozicia(x, y);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                if (y == this.getY() && x == this.getX() - 1 && plocha.getFigurka(x, y) == null)
                {
                    this.setPozicia(x, y);
                    return true;
                }
                else if (y == this.getY() && x == this.getX() - 2
                    && (this.prvaPoziciaX == this.getX() && this.prvaPoziciaY == this.getY()) && plocha.getFigurka(x, y) == null && plocha.getFigurka(x + 1, y) == null)
                {
                    this.setPozicia(x, y);
                    return true;
                }
                else if (y == this.getY() + 1 && x == this.getX() - 1 && plocha.getFigurka(x, y) != null)
                {
                    this.setPozicia(x, y);
                    return true;
                }
                else if (y == this.getY() - 1 && x == this.getX() - 1 && plocha.getFigurka(x, y) != null)
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
}