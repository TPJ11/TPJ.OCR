namespace TPJ.OCR.Enums;

public enum ContentType
{
    /// <summary>
    /// Only Digits are expected this will fix common issues in OCR like
    /// I = 1
    /// </summary>
    Digits,

    /// <summary>
    /// Only Alphabetic characters are expected this will fix common issues in OCR like
    /// 3 = S
    /// </summary>
    Alphabetic,

    /// <summary>
    /// Any type of character is allowed, no changes to the result of the OCR is made
    /// </summary>
    Any
}