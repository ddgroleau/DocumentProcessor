using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UglyToad.PdfPig;
using UglyToad.PdfPig.DocumentLayoutAnalysis.PageSegmenter;
using UglyToad.PdfPig.DocumentLayoutAnalysis.TextExtractor;

namespace LetterTesting
{
    class Program
    {
        static void Main(string[] args)
        {
            string filePath1 = "../../Test/TestLetter1.pdf";
            string filePath2 = "../../Test/TestLetter2.pdf";
            using (var pdf = PdfDocument.Open(filePath1))
            {
                foreach (var page in pdf.GetPages())
                {
                    // Gets text by extracting based on order in the underlying document with new lines and spaces.
                    var text = ContentOrderTextExtractor.GetText(page);

                    // Gets text based on grouping letters into words.
                    var otherText = string.Join(" ", page.GetWords());

                    // Gets raw text of the page's content stream.
                    var rawText = page.Text;

                    var imgs = page.NumberOfImages;

                    // Gets page dimensions
                    var box = page.MediaBox.Bounds;

                    var letters = page.Letters.Count;
                }
            }

            using (var document = PdfDocument.Open(filePath2))
            {
                for (var i = 0; i < document.NumberOfPages; i++)
                {
                    var page = document.GetPage(i + 1);
                    var words = page.GetWords();

                    var blocks = RecursiveXYCut.Instance.GetBlocks(words);

                    // This allows you to specify how RecursiveXYCut separates document elements
                    var optionsBlocks = RecursiveXYCut.Instance.GetBlocks(words, new RecursiveXYCut.RecursiveXYCutOptions()
                    {
                        MinimumWidth = page.Width / 2,
                    }); ;

                    foreach (var block in blocks)
                    {
                        Console.WriteLine($"Line Count: {block.TextLines.Count()}");
                        Console.WriteLine(block.Text);
                        Console.WriteLine("////////////////////////////      BLOCK BREAK     //////////////////////////////////");
                    }
                }
            }

        }
        
    }
}
