using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Wordprocessing;
using Syncfusion.Drawing;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;
using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using Run = DocumentFormat.OpenXml.Wordprocessing.Run;
using Text = DocumentFormat.OpenXml.Spreadsheet.Text;

namespace Xml2Txt
{
    class Program
    {
        static void Main(string[] args)
        {
            do
            {
                try
                {
                    //  Read from console, how many files at what approximate size.
                    int pagesToCreate = 0;
                    int saveFilesize = 0;
                    Console.WriteLine("How many files?");
                    if (Int32.TryParse(Console.ReadLine(), out pagesToCreate))
                    {
                        Console.WriteLine("Min Filesize in MB?");
                        if (Int32.TryParse(Console.ReadLine(), out saveFilesize))
                        {
                            saveFilesize *= (1024 * 1024);
                        }
                        else
                        {
                            Console.WriteLine("Invalid input!");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Invalid input!");
                    }

                    // Keep track of running totals
                    int filesize = 0;
                    int pagesProcessed = 0;
                    int filesCreated = 0;

                    Console.WriteLine("Source file?");
                    StringBuilder sb = new StringBuilder("", 5000);
                    string path = Path.Combine(Environment.CurrentDirectory, Console.ReadLine());


                    Console.WriteLine("Enter the name for this data set...");
                    string dirName = Console.ReadLine();

                    XmlReader reader = XmlReader.Create(path);
                    reader.ReadToDescendant("page");
                    if (pagesToCreate > 0)
                    {
                        if (!Directory.Exists(dirName))
                        {
                            Directory.CreateDirectory(dirName);
                        }
                        do
                        {
                            XmlDocument doc = new XmlDocument();
                            doc.LoadXml(reader.ReadOuterXml());

                            XmlNode pageNode = doc.DocumentElement;
                            if (pageNode != null)
                            {
                                pagesProcessed++;
                                foreach (XmlNode revisonNode in pageNode.ChildNodes)
                                {
                                    if (revisonNode.Name == "revision")
                                    {
                                        foreach (XmlNode textNode in revisonNode.ChildNodes)
                                        {
                                            if (textNode.Name == "text")
                                            {
                                                int thisFileSize = Int32.Parse(textNode.Attributes["bytes"].Value);
                                                filesize += thisFileSize;
                                                sb.Append(textNode.InnerText);
                                                if (filesize >= saveFilesize)
                                                {
                                                    string outputFile = dirName + "_" + filesCreated++;
                                                    CreateTxtFile(dirName, outputFile, sb.ToString(), (filesize / 1024 / 1024));
                                                    sb = new StringBuilder("", 5000);
                                                    filesize = 0;
                                                    if (filesCreated >= pagesToCreate)
                                                    {
                                                        goto LoopEnd;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        while (reader.ReadToNextSibling("page"));

                        if (filesize > 0)
                        {
                            // if anything left write that to a file.
                            string outputFile = dirName + "_" + filesCreated++;
                            CreateTxtFile(dirName, outputFile, sb.ToString(), (filesize / 1024 / 1024));
                        }
                    }
                LoopEnd:
                    reader.Close();
                    if (pagesProcessed > 0)
                    {
                        Console.WriteLine("Processed:" + pagesProcessed + " wikipedia pages");
                        Console.WriteLine("Saved:" + filesCreated + " files in " + dirName);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);

                }
                Console.WriteLine("q to quit, return to run again");

            }
            while (Console.ReadLine() != "q");

            static void CreateTxtFile(string directory, string outputFile, string content, int size)
            {
                using (FileStream fs = File.Create(directory + "/" + outputFile + ".txt"))
                {
                    byte[] info = new UTF8Encoding(true).GetBytes(content);
                    // Add some information to the file.
                    fs.Write(info, 0, info.Length);
                }

                Console.WriteLine("Created:" + outputFile + ".txt" + " Size:" + size + "MB");
            }
        }
    }
}


