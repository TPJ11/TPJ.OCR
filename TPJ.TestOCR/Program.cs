using IronSoftware.Drawing;
using TPJ.OCR;
using TPJ.OCR.Enums;

namespace TPJ.TestOCR
{
    internal class Program
    {
        static void Main(string[] args)
        {
            FullImage();
            SingleArea();
            MultipleAreas();
        }

        private static void FullImage()
        {
            Console.WriteLine("Full Image");
            Console.WriteLine(AnyBitmap.FromFile(@"./ExampleImage.png").OCR());
            Console.WriteLine();
        }

        private static void SingleArea()
        {
            Console.WriteLine("Single Area");
            Console.WriteLine(AnyBitmap.FromFile(@"./ExampleImage.png")
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
                }));
            Console.WriteLine();
        }

        private static void MultipleAreas()
        {
            Console.WriteLine("Multiple Areas");
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

            foreach (var item in result)
                Console.WriteLine($"{item.AreaName} = {item.Value}");

            Console.WriteLine();
        }
    }
}