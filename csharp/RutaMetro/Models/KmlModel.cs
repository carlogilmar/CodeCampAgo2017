namespace CodeCamp.RutaMetro
{
    using System.Collections.Generic;
    using System.Xml.Serialization;

    public static class Xmlns
    {
        public const string Klm = "http://www.opengis.net/kml/2.2";
    }

    [XmlRoot(Namespace = Xmlns.Klm)]
    public class Icon
    {
        [XmlElement(ElementName = "href", Namespace = Xmlns.Klm)]
        public string Href { get; set; }
    }

    [XmlRoot(Namespace = Xmlns.Klm)]
    public class IconStyle
    {
        [XmlElement(ElementName = "scale", Namespace = Xmlns.Klm)]
        public string Scale { get; set; }
        [XmlElement(Namespace = Xmlns.Klm)]
        public Icon Icon { get; set; }
    }

    [XmlRoot(Namespace = Xmlns.Klm)]
    public class LabelStyle
    {
        [XmlElement(ElementName = "scale", Namespace = Xmlns.Klm)]
        public string Scale { get; set; }
    }

    [XmlRoot(Namespace = Xmlns.Klm)]
    public class Style
    {
        [XmlElement(Namespace = Xmlns.Klm)]
        public IconStyle IconStyle { get; set; }
        [XmlElement(Namespace = Xmlns.Klm)]
        public LabelStyle LabelStyle { get; set; }
        [XmlAttribute(AttributeName = "id")]
        public string Id { get; set; }
        [XmlElement(Namespace = Xmlns.Klm)]
        public LineStyle LineStyle { get; set; }
        [XmlElement(Namespace = Xmlns.Klm)]
        public BalloonStyle BalloonStyle { get; set; }
    }

    [XmlRoot(Namespace = Xmlns.Klm)]
    public class Pair
    {
        [XmlElement(ElementName = "key", Namespace = Xmlns.Klm)]
        public string Key { get; set; }
        [XmlElement(ElementName = "styleUrl", Namespace = Xmlns.Klm)]
        public string StyleUrl { get; set; }
    }

    [XmlRoot(Namespace = Xmlns.Klm)]
    public class StyleMap
    {
        [XmlElement(Namespace = Xmlns.Klm)]
        public List<Pair> Pair { get; set; }
        [XmlAttribute(AttributeName = "id")]
        public string Id { get; set; }
    }

    [XmlRoot(Namespace = Xmlns.Klm)]
    public class LineStyle
    {
        [XmlElement(ElementName = "color", Namespace = Xmlns.Klm)]
        public string Color { get; set; }
        [XmlElement(ElementName = "width", Namespace = Xmlns.Klm)]
        public string Width { get; set; }
    }

    [XmlRoot(Namespace = Xmlns.Klm)]
    public class BalloonStyle
    {
        [XmlElement(ElementName = "text", Namespace = Xmlns.Klm)]
        public string Text { get; set; }
    }

    [XmlRoot(Namespace = Xmlns.Klm)]
    public class LineString
    {
        [XmlElement(ElementName = "tessellate", Namespace = Xmlns.Klm)]
        public string Tessellate { get; set; }
        [XmlElement(ElementName = "coordinates", Namespace = Xmlns.Klm)]
        public string Coordinates { get; set; }
    }

    [XmlRoot(Namespace = Xmlns.Klm)]
    public class Placemark
    {
        [XmlElement(ElementName = "name", Namespace = Xmlns.Klm)]
        public string Name { get; set; }
        [XmlElement(ElementName = "styleUrl", Namespace = Xmlns.Klm)]
        public string StyleUrl { get; set; }
        [XmlElement(Namespace = Xmlns.Klm)]
        public LineString LineString { get; set; }
        [XmlElement(ElementName = "description", Namespace = Xmlns.Klm)]
        public string Description { get; set; }
        [XmlElement(Namespace = Xmlns.Klm)]
        public Point Point { get; set; }
    }

    [XmlRoot(Namespace = Xmlns.Klm)]
    public class Folder
    {
        [XmlElement(ElementName = "name", Namespace = Xmlns.Klm)]
        public string Name { get; set; }
        [XmlElement(ElementName = "Placemark", Namespace = Xmlns.Klm)]
        public List<Placemark> Placemarks { get; set; }
    }

    [XmlRoot(Namespace = Xmlns.Klm)]
    public class Point
    {
        [XmlElement(ElementName = "coordinates", Namespace = Xmlns.Klm)]
        public string Coordinates { get; set; }
    }

    [XmlRoot(Namespace = Xmlns.Klm)]
    public class Document
    {
        [XmlElement(ElementName = "name", Namespace = Xmlns.Klm)]
        public string Name { get; set; }
        [XmlElement(ElementName = "description", Namespace = Xmlns.Klm)]
        public string Description { get; set; }
        [XmlElement(Namespace = Xmlns.Klm)]
        public List<Style> Style { get; set; }
        [XmlElement(Namespace = Xmlns.Klm)]
        public List<StyleMap> StyleMap { get; set; }
        [XmlElement(ElementName = "Folder", Namespace = Xmlns.Klm)]
        public List<Folder> Folders { get; set; }
    }

    [XmlRoot(ElementName = "kml", Namespace = Xmlns.Klm)]
    public class Kml
    {
        [XmlElement(ElementName = "Document", Namespace = Xmlns.Klm)]
        public Document Document { get; set; }
    }
}