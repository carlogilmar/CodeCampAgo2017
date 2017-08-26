class KlmParser{

  def lines = []
  def stations = []
  String xml

  def KlmParser(){}

  def parse(String xml){
    this.xml = xml
  }

}

class Line{

  String name
  ArrayList points

}
