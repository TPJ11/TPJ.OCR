
using IronSoftware.Drawing;
using Tesseract;
using TPJ.OCR.Enums;

namespace TPJ.OCR;

public static class BitmapOCRExtension
{
    /// <summary>
    /// OCR the passed in image
    /// </summary>
    /// <param name="image">Image to OCR</param>
    /// <param name="type">
    /// If the data type is known then attempts to fix common OCR 
    /// issues are made on that type e.g. if its a number and OCR 
    /// picks up I then it will change the I to a 1
    /// If null no fixed type will be used so no attempt will be made to change the result from the OCR
    /// </param>
    /// <param name="location">The area of the image to OCR, if null the full image will be OCR</param>
    /// <returns>Data found in the area of the image</returns>
    public static string? OCR(this AnyBitmap image, ContentType type = ContentType.Any, OCRRectangle ? location = null)
    {
        return OCR(image, new List<OCRArea>() { new OCRArea()
        {
            Name = "1",
            ContentType = type,
            Location = location ?? 
                new OCRRectangle()
                {
                    X = new Location()
                    {
                        StartPixel = 0,
                        EndPixel = image.Width,
                    },
                    Y = new Location()
                    {
                        StartPixel = 0,
                        EndPixel = image.Height,
                    }
                }
        } }).FirstOrDefault().Value;
    }

    /// <summary>
    /// OCR the image for each of the given areas retuning a 
    /// list of area name and OCR value
    /// </summary>
    /// <param name="image">Image to OCR</param>
    /// <param name="areas">Areas of the image to OCR</param>
    /// <returns>List of OCR results</returns>
    public static List<(string AreaName, string? Value)> OCR(this AnyBitmap image, IEnumerable<OCRArea> areas)
    {     
        var result = new List<(string AreaName, string? value)>();

        using var engine = new TesseractEngine(@"./tessdata", "eng", EngineMode.Default);

        foreach (var area in areas)
        {
            var croppedImage = image.Clone(area.Location.GetRectangle())
                .ExportBytes(AnyBitmap.ImageFormat.Tiff);          
            
            using var pix = Pix.LoadTiffFromMemory(croppedImage);
            using var page = engine.Process(pix);

            var value = page.GetText();

            switch (area.ContentType)
            {
                case ContentType.Digits:
                    value = FixStringDigits(RemoveNewLine(value));
                    break;
                case ContentType.Alphabetic:
                    value = FixDigitText(RemoveNewLine(value));
                    break;
            }

            result.Add((area.Name, value));
        }

        return result;
    }

    /// <summary>
    /// Removes all \n from a string
    /// </summary>
    /// <param name="text">The text.</param>
    /// <returns>text without \n</returns>
    private static string RemoveNewLine(string text) => text.Replace("\n", "");

    /// <summary>
    ///  Check each char in the given value is a letter
    ///  if its not try and convert it to a letter
    /// </summary>
    /// <param name="value">OCR value</param>
    /// <returns>Checked and fixed text</returns>
    private static string FixDigitText(string value)
    {
        // Check for any char in the string not to be a letter
        if (value.Any(c => !char.IsLetter(c)))
        {
            string fixedText = string.Empty;

            // Rebuild the new name fixing the mistake
            foreach (var c in value.ToCharArray())
            {
                // If a digit is found then change to letter
                if (char.IsDigit(c))
                {
                    if (int.TryParse(c.ToString(), out var digit))
                        fixedText += IntToLetter(digit);
                }
                // If a none letter is found which is not a digit 
                // then it may be a symbol
                else if (!char.IsLetter(c))                
                    fixedText += SymbolsToLetter(c.ToString());                
                else                
                    fixedText += c;
            }

            value = fixedText;
        }

        return value;
    }

    /// <summary>
    /// Check each char of a string to make sure 
    /// its a number if its not try and convert to
    /// the most likely number equivalent
    /// </summary>
    /// <param name="value">Text from the OCR program</param>
    /// <returns>value after</returns>
    private static string FixStringDigits(string value)
    {
        // Check for any char in the string is not a number
        // if there is a non number in the string then we need to
        // try and fix it.
        if (value.Any(c => !char.IsDigit(c)))
        {
            string fixedNumber = string.Empty;
            
            foreach (var c in value.ToCharArray())
                fixedNumber += char.IsLetter(c) ? LetterToInt(c.ToString()) : c.ToString();

            value = fixedNumber;
        }

        return value;
    }

    /// <summary>
    /// Changes letter to int.
    /// </summary>
    /// <param name="letter">The letter.</param>
    /// <returns>Int</returns>
    private static string LetterToInt(string letter) =>
        letter.ToLower() switch
        {
            "q" => "9",
            "e" => "6",
            "r" => "2",
            "t" => "7",
            "y" => "4",
            "i" => "1",
            "o" => "0",
            "p" => "9",
            "a" => "8",
            "s" => "3",
            "d" => "0",
            "f" => "7",
            "g" => "8",
            "j" => "1",
            "l" => "1",
            "z" => "3",
            "b" => "8",
            _ => string.Empty,
        };    

    /// <summary>
    /// Changes int to a letter.
    /// </summary>
    /// <param name="digit">The digit.</param>
    /// <returns>Letter</returns>
    private static string IntToLetter(int digit) =>
        digit switch
        {
            0 => "o",
            1 => "l",
            2 => "s",
            3 => "s",
            4 => "g",
            5 => "s",
            6 => "g",
            7 => "t",
            8 => "o",
            9 => "g",
            _ => string.Empty,
        };
    
    /// <summary>
    /// Changes a symbolses to a letter.
    /// </summary>
    /// <param name="symbol">The symbol.</param>
    /// <returns>Letter</returns>
    private static string SymbolsToLetter(string symbol) => 
        symbol switch
        {
            "@" => "e",
            _ => string.Empty,
        };   

}
