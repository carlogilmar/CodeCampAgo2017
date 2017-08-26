class KlmParser{

  def lines = []
  def stations = []
  String xml
  

  def KlmParser(){}

  def parse(String xml){
    this.xml = xml
    def datos = new XmlSlurper().parseText(xml)
    def placemark = datos.Document.Folder.Placemark
    def coords = placemark.LineString.coordinates.toString()
    def line = new Line(name: placemark.name, coordinates: parseCoordinates(coords))
    lines.add(line)
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
