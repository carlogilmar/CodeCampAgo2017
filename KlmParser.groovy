class KlmParser{

  def lines = []
  def stations = []
  String xml

  def KlmParser(){}

  def parse(String xml){
    this.xml = xml
    def mapa = new XlmSlurper().parseText(this.xml)
  }

}

class Line{

  String name
  ArrayList points

}
