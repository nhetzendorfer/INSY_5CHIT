using System.Xml;
using System.Xml.Linq;

namespace XMLÜbungConsole;

class Program
{
    static void Main(string[] args)
    {
        /*XmlReader xr = new XmlTextReader("C:\\Users\\Nico\\RiderProjects\\XMLÜbung\\data.xml");
        Console.WriteLine(xr.Read());
        while (xr.Read())
        {
            //Console.Write("---"+xr.Name+": "+xr.Value+" "+xr.NodeType+"\n");
            Console.WriteLine(xr.LocalName+" "+xr.Name);
        }*/
        _1XMLDocument();
    }

    static void _1XMLDocument()
    {
        XmlDocument doc = new XmlDocument();
        
        //XmlReader xr = new XmlTextReader("C:\\Users\\Nico\\RiderProjects\\XMLÜbung\\data.xml");
        //doc.Load("C:\\Users\\Nico\\RiderProjects\\XMLÜbung\\data.xml");
        //Console.WriteLine(doc.InnerXml);
       
       // XmlElement rootElement = tree.CreateElement("Wuascht");
        //root.AppendChild(rootElement);
        
        
       /* DirectoryInfo di = new DirectoryInfo("C:\\Users\\Nico\\RiderProjects\\XMLÜbung\\XMLÜbungConsole");

        XmlDocument tree= new XmlDocument();
        tree.LoadXml("<tree/>");
        var root= tree.DocumentElement;
        StartPrintShit(ref tree,root,di);
        tree.Save("./tree.xml");*/
       _XDocument();
    }
    
    static void StartPrintShit(ref XmlDocument tree,XmlElement parent,DirectoryInfo di)
    {
        var dirs = di.GetDirectories();
        var files = di.GetFiles();
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("       "+di.Name);
        Console.ForegroundColor = ConsoleColor.White;

        foreach (var file in files)
        {
            var element = tree.CreateElement("file");
            element.SetAttribute("name", file.Name);
            parent.AppendChild(element);
            
            Console.WriteLine(file.Name); 
        }

        foreach (var dir in dirs)
        {
            var direl = tree.CreateElement(dir.Name);
            parent.AppendChild(direl);
            StartPrintShit(ref tree,direl,dir);
        }
    }

    static void _XDocument()
    {
        XDocument doc = XDocument.Load("C:\\Users\\Nico\\RiderProjects\\XMLÜbung\\data.xml");
        Console.WriteLine(doc.ToString());
        RecursiveOutput(doc.Root);
        DirectoryInfo di = new DirectoryInfo("C:\\Users\\Nico\\RiderProjects\\XMLÜbung\\XMLÜbungConsole");
        XStartPrintShit(doc.Root,di);
    }

    static void RecursiveOutput(XElement doc)
    {
        foreach (var element in doc.Elements())
        {
            Console.WriteLine("Line--- "+element.NodeType+" "+element.Name+" "+element.Value);
        }
    }

    static void XStartPrintShit(XElement parent, DirectoryInfo di)
    {
        
        var dirs = di.GetDirectories();
        var files = di.GetFiles();

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("       " + di.Name);
        Console.ForegroundColor = ConsoleColor.White;

        // Add files
        foreach (var file in files)
        {
            var fileElement = new XElement("file");
            fileElement.SetAttributeValue("name", file.Name);
            parent.Add(fileElement);

            Console.WriteLine(file.Name);
        }

        // Recurse into directories
        foreach (var dir in dirs)
        {
            var dirElement = new XElement(dir.Name);
            parent.Add(dirElement);

            XStartPrintShit(dirElement, dir);
        }
    }

}
