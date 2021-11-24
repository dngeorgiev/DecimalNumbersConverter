namespace NumberSystemConverter 
{
    using System;
    using System.Text.RegularExpressions;

    public class Program
    {
        private static int[] AVAILABLE_NUMBER_SYSTEMS = new int[] { 2, 16, 8, 5, 10 };
        private static string ALLOWED_DIGITS = "0123456789ABCDEF";
        private static int FRACTION_PART_MAX_DIGITS = 10;

        public static void Main()
        {
            try
            {
                Console.WriteLine("===*** Number System Converter ***===");
                Console.WriteLine("------->>>> The following program converts all found numbers from a text <<<<-------");
                
                Console.WriteLine(Environment.NewLine);

                Console.WriteLine("------ Enter a text with decimal numbers (with DOT as a decimal separator) ------");

                string? userInput = Console.ReadLine();

                if (string.IsNullOrEmpty(userInput))
                {
                    throw new Exception("The input text cannot be empty.");
                }

                Console.WriteLine(Environment.NewLine);

                Console.WriteLine("------ Choose a number system to which you want all the numbers found to be converted to ------");
                Console.WriteLine($"----------- Available number systems: {string.Join(", ", AVAILABLE_NUMBER_SYSTEMS)} -----------");
                
                if (!int.TryParse(Console.ReadLine(), out int chosenNumberSystem))
                {
                    throw new Exception("Unable to parse the input to integer.");
                }
                else
                {
                    if (!AVAILABLE_NUMBER_SYSTEMS.Contains(chosenNumberSystem))
                    {
                        throw new Exception("Invalid number system chosen.");
                    }
                }
                
                string result = Solve(userInput, chosenNumberSystem);

                Console.WriteLine(Environment.NewLine);
                Console.WriteLine("-----****----- RESULT -----****-----");
                Console.WriteLine(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error has occured. Exception message: " + ex.Message);
            }
        }

        private static string Solve(string input, int chosenNumberSystem)
        {
            if (!AVAILABLE_NUMBER_SYSTEMS.Contains(chosenNumberSystem))
            {
                throw new Exception("Invalid number system chosen.");
            }

            var regexPattern = @"(\d+(?:[\.]\d{1,}))";

            var result = Regex.Replace(input, regexPattern, (match) => ConvertToNumberSystem(match.Value, chosenNumberSystem));
            
            return result;
        }

        private static string ConvertToNumberSystem(string numberString, int chosenNumberSystem)
        {
            if (!AVAILABLE_NUMBER_SYSTEMS.Contains(chosenNumberSystem))
            {
                throw new Exception("Invalid number system chosen.");
            }
            if (!double.TryParse(numberString, out double number))
            {
                throw new Exception("Error while trying to parse match from the text. Please, try again with another input.");
            }

            string convertNumberToString = "";
            switch (chosenNumberSystem)
            {
                case 2:
                case 16:
                case 8:
                case 5:
                    convertNumberToString = ConvertFloatToKNumeralSystem(number, chosenNumberSystem);
                    break;
                case 10:
                default:
                    convertNumberToString = numberString;
                    break;
            }

            return convertNumberToString;
        }

        private static string ConvertFloatToKNumeralSystem(double number, int myBase)
        {
            if (!AVAILABLE_NUMBER_SYSTEMS.Contains(myBase))
            {
                throw new Exception("Invalid number system chosen.");
            }

            int integerPart = (int) Math.Truncate(number); 
            double fractionPart = number - Math.Truncate(number);

            int remainder = 0;
            string binary = string.Empty;
            while (integerPart > 0)
            {
                remainder = integerPart % myBase;
                integerPart /= myBase;
                binary = remainder.ToString() + binary;
            }

            if (binary == string.Empty)
            {
                binary = "0";
            }

            string fractionPartBinary = string.Empty;
            while (fractionPart > 0 && fractionPartBinary.Length <= FRACTION_PART_MAX_DIGITS)
            {
                fractionPart *= myBase;
                fractionPartBinary += string.Format("{0}", ALLOWED_DIGITS[(int)fractionPart]);
                fractionPart -= (int) fractionPart; 
            }


            return fractionPartBinary != string.Empty ? binary + "." + fractionPartBinary : binary;
        }
    }
}