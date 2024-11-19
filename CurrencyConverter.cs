namespace Opgave_House4IT;

class EuroDKKConverter
{
    //A simplified currency converter
    public double ToDKK(double price)
    {
        //Converts a price in EUR to DKK
        return Math.Round(price * 7.5, 2);
    }
}