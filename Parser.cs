using System.Text.RegularExpressions;

namespace Opgave_House4IT;

class Parser
{
    public T Parse<T>(string value)
    {
        try
        {
            //Converts the string value to the specified type T
            return (T)Convert.ChangeType(value, typeof(T));
        }
        catch (Exception)
        {
            throw new Exception($"Cannot convert the value {value} to {typeof(T)}");
        }
    }

    // A method for parsing prices
    public double ParsePrice(string price)
    {
        //Regex pattern for checking if price contains any of the characters specified in the []
        Regex currencyPattern = new Regex("[€$¢£]");
        //Regex pattern for checking if price contains a comma
        Regex commaPattern = new Regex(",");
        double parsedPrice = 0;

        if (currencyPattern.IsMatch(price))
        {
            //Removes the currency character if there is one
            string newPrice = currencyPattern.Replace(price, "");
  
            //Replaces , with .
            if (commaPattern.IsMatch(newPrice))
            {
                newPrice = commaPattern.Replace(newPrice, ".");
            }

            //Double is parsed where . is always the decimal seperator because of the InvariantCulture
            parsedPrice = double.Parse(newPrice, System.Globalization.CultureInfo.InvariantCulture);
        }

        return parsedPrice;
    }
}