class KlmParser{

  def lines = []
  def stations = []
  String xml
  ArrayList<Coord> coordinatesAssociate = []

  def KlmParser(){}

  def parse(String xml){
    this.xml = xml
  }

  def  parseCoordinates(String cadena){
    //ArrayList<Coord> coords 
    def coordinateLine = cadena.split(',')
    def coord = new Coord(coordinateLine[0], coordinateLine[1])
    coordinatesAssociate.add(coord)
    coordinatesAssociate
  }
}


class Coord {
  def x
  def y

  def Coord(def x, def y){
    this.x = new BigDecimal(x)
    this.y = new BigDecimal(y)
  }
}
