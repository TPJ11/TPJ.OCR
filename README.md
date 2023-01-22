# TPJ.OCR
Simple OCR library to make OCRing images easy!

# Thanks
TPJ.OCR uses a number of other open source packages so special thanks to:

- [Tesseract](https://github.com/tesseract-ocr/tesseract)
- [Charlesw Tesseract Wrapper](https://github.com/charlesw/tesseract/)
- [IRON Drawing](https://ironsoftware.com/open-source/csharp/drawing/docs/)

# How to use TPJ.OCR
First copy the `tessdata`, `x64`, and `x86` folders from the example app `TPJ.TestOCR` from github, set each of the files from within those folders to `copy if newer`. This is the tesseract engine that will perform the OCR.

You must convert the image you wish to OCR into a `AnyBitmap` file, if you are loading it from disk then this is simple done like so:

```
AnyBitmap.FromFile(filePath);
```
`AnyBitmap` has a number of loading methods will should cover any way you need.

Next call one of the OCR extension method on the `AnyBitmap` object `bitmap.OCR()` - See examples.

## Content Type
OCR is very hard and it can often produce incorrect characters such as 1 where it should be an I or a 3 where it should be an S. If you know the data you are OCR is going to be only digits or only alphabetic we can change the common issues with the correct characters. 

There are three content types you can pick from: 

1. `Digits` - Only digits (0 - 9) characters
2. `Alphabetic` - Only alphebtic (Aa - Zz) characters
3. `Any` - This is the default any means no changes are made from the result of the OCR

## OCRArea, OCRRectangle, Location
There are three classes within TPJ.OCR `OCRArea`, `OCRRectangle`, `Location`.

### Location
This contents the start and end pixel for the rectangle area that will be OCRed. If you have the image you are going to OCR you can simply open the image in paint and note down the X and Y for the top left of the rectangle and the bottom right.

### OCRRectangle
This holds the X and Y location and a method to create the rectangle object based off the X and Y location.

### OCRArea
This holds your rectangle location and allows you to set the content type (`Digits`, `Alphabetic`, `Any`) and set a name for the area, the name is useful when you are OCRing a single image for mutiple areas as the result will contain the name given in the `OCRArea` object plus the result of the OCR allowing you to confidently pick out the result for the area.

# Examples
Check out the examples in github

## Full image OCR
```
AnyBitmap.FromFile(@"./ExampleImage.png").OCR();
```

## Single Area Digits Only
```
AnyBitmap.FromFile(@"./ExampleImage.png")
    .OCR(ContentType.Digits, 
    new OCRRectangle()
    {
        X = new Location()
        {
            StartPixel = 460,
            EndPixel = 700,
        },
        Y = new Location()
        {
            StartPixel = 220,
            EndPixel = 320,
        }
    });
```

## Multiple Areas
```
var result = AnyBitmap.FromFile(@"./ExampleImage.png")
    .OCR(new List<OCRArea>() 
    {
        new OCRArea()
        {
            Name = "Name",
            ContentType = ContentType.Alphabetic,
            Location = new OCRRectangle()
            {
                X = new Location()
                {
                    StartPixel = 300,
                    EndPixel = 700,
                },
                Y = new Location()
                {
                    StartPixel = 40,
                    EndPixel = 140,
                }
            }
        },
        new OCRArea()
        {
            Name = "Payroll Name",
            ContentType = ContentType.Alphabetic,
            Location = new OCRRectangle()
            {
                X = new Location()
                {
                    StartPixel = 1350,
                    EndPixel = 1700,
                },
                Y = new Location()
                {
                    StartPixel = 40,
                    EndPixel = 140,
                }
            }
        },
        new OCRArea()
        {
            Name = "HRMS No",
            ContentType = ContentType.Digits,
            Location = new OCRRectangle()
            {
                X = new Location()
                {
                    StartPixel = 460,
                    EndPixel = 700,
                },
                Y = new Location()
                {
                    StartPixel = 220,
                    EndPixel = 320,
                }
            }
        },
        new OCRArea()
        {
            Name = "Pay Date",
            ContentType = ContentType.Any,
            Location = new OCRRectangle()
            {
                X = new Location()
                {
                    StartPixel = 1170,
                    EndPixel = 1710,
                },
                Y = new Location()
                {
                    StartPixel = 220,
                    EndPixel = 320,
                }
            }
        }
    });
```