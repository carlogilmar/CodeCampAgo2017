class KlmParser{

  def lines = []
  def stations = []
  String xml
  

  def KlmParser(){}

  def parse(String xml){
    this.xml = xml
    def datos = new XmlSlurper().parseText(xml)
    def placemarks = datos.Document.Folder[0].Placemark
    placemarks.each{ placemark ->
      def coords = placemark.LineString.coordinates.toString()
      def line = new Line(name: placemark.name, coordinates: parseCoordinates(coords))
      lines.add(line)  
    }

    def xmlStations = datos.Document.Folder[1].Placemark
    xmlStations.each{ 
      def station = new Station()
      station.name = it.name.toString()
      def coords = it.Point.coordinates.toString()
      station.coordinate = parseCoordinates(coords)[0]  
      stations.add(station)
    }

  }

  def parseCoordinates(String cadena){
    ArrayList<Coord> coordinatesAssociate = []
      cadena.eachLine{ line ->
        if(line.trim()){
          coordinatesAssociate.add(parseSingleCoordinate(line))  
        } 
      }
      coordinatesAssociate   
  }

  def parseSingleCoordinate(String cadena){
    def coordinateLine = cadena.split(',')
    new Coord(coordinateLine[0].trim(), coordinateLine[1].trim())
  }

}

class Line{
  String name
  ArrayList<Coord> coordinates

}

class Station{
  String name
  Coord coordinate
  ArrayList<Line> lines
}


class Coord {
  def x
  def y

  def Coord(def x, def y){
    this.x = new BigDecimal(x)
    this.y = new BigDecimal(y)
  }

  def matches(def x, def y){
    this.x == x && this.y == y
  }
}
