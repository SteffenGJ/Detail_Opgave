using System.Text.RegularExpressions;

namespace Opgave_House4IT;

//Class based on the data from CSV file
class Product
{
    private long item;
    private string description;
    private string unit;
    private double kostpris;
    private int priceUnit;
    private int priceGroup;
    private string? dateOfIssuance;
    private double sellingPrice;
    private string descriptionDK;
    private double kostprisDKK;
    private double salgsPrisDKK;

    public Product(long item, string description, string unit, double kostpris, int priceUnit, int priceGroup, string? dateOfIssuance = null, EuroDKKConverter? converter = null)
    {
        this.item = item;
        this.description = description;
        this.unit = unit;
        this.kostpris = kostpris;
        this.priceUnit = priceUnit;
        this.priceGroup = priceGroup;
        this.dateOfIssuance = dateOfIssuance;
        converter ??= new EuroDKKConverter();
        sellingPrice = DetermineSellingPrice();
        descriptionDK = ReplaceBlack();
        kostprisDKK = converter.ToDKK(kostpris);
        salgsPrisDKK = converter.ToDKK(sellingPrice);
    }

    private double DetermineSellingPrice()
    {
        //Determines the selling price of the product based on the price group
        if (priceGroup == 21)
        {
            //Adds 30% to kostpris and rounds to two decimals 
            return Math.Round(kostpris * 1.3, 2);
        }
        else if (priceGroup == 22 || priceGroup == 25)
        {
            //Adds 50% to kostpris and rounds to two decimals 
            return Math.Round(kostpris * 1.5, 2);
        }
        else if (priceGroup == 23 || priceGroup == 24)
        {
            //Adds 40% to kostpris and rounds to two decimals 
            return Math.Round(kostpris * 1.4, 2);
        }
        else
        {
            throw new Exception("Invalid pricegroup");
        }
    }

    public string ReplaceBlack()
    {
        //Replaces the word black with sort
        //Regex checking for all variations of the word black
        var pattern = new Regex("(?i)\bblack\b|black");

        if (pattern.IsMatch(description))
        {
            return pattern.Replace(description, "sort");
        }
        else
        {
            return description;
        }
    }

    public string ToCSVPrice()
    {
        //Extracts values from specified fields and makes a string suitable for a CSV file
        string vareNummer = item.ToString();
        string navn = descriptionDK;
        string kostprisDK = string.Join(" ", [kostprisDKK.ToString(), "DKK"]);
        string salgsPrisDK = string.Join(" ", [salgsPrisDKK.ToString(), "DKK"]);

        return string.Join(";", [vareNummer, navn, kostprisDK, salgsPrisDK]);
    }
}