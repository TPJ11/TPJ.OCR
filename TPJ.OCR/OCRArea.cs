using System.Drawing;
using TPJ.OCR.Enums;

namespace TPJ.OCR;

public class OCRArea
{
    /// <summary>
    /// Name of the area e.g. Staff Name
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Start and end location for the pixels for X and Y of the rectangle to draw
    /// </summary>
    public OCRRectangle Location { get; set; }

    /// <summary>
    /// The expected content type of the OCR result
    /// </summary>
    public ContentType ContentType { get; set; } = ContentType.Any;
}

public class OCRRectangle
{
    /// <summary>
    /// The start and end location on the X axis
    /// </summary>
    public Location X { get; set; } = new Location();

    /// <summary>
    /// The start and end location on the Y axis
    /// </summary>
    public Location Y { get; set; } = new Location();

    /// <summary>
    /// The rectangle created by using the X and Y locations
    /// </summary>
    /// <returns>Rectangle of the area using the X and Y locations</returns>
    public Rectangle GetRectangle()
    {
        var width = X.EndPixel - X.StartPixel;
        var height = Y.EndPixel - Y.StartPixel;
        return new Rectangle(X.StartPixel, Y.StartPixel, width, height);
    }
}

public class Location
{
    /// <summary>
    /// Start pixel of the line
    /// Tip - use paint to easily find the pixel
    /// </summary>
    public int StartPixel { get; set; }

    /// <summary>
    /// End pixel of the line
    /// Tip - use paint to easily find the pixel
    /// </summary>
    public int EndPixel { get; set; }
}
