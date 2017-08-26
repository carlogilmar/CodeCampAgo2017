class KlmParserTest extends GroovyTestCase{

  void testCreateKlmParser(){
    def klmParser = new KlmParser().parse("<kml></kml>")
    assert klmParser != null
  }

  void testReadEmptyDocument(){
    String xml = """
    <?xml version="1.0" encoding="UTF-8"?>
    <kml xmlns="http://www.opengis.net/kml/2.2"/>
    """
    def klmParser = new KlmParser()
    assert klmParser.lines == []
    assert klmParser.stations == []
  }


  void testReadDocumentWithOneLine(){
    String xml = """<?xml version="1.0" encoding="UTF-8"?>
    <kml xmlns="http://www.opengis.net/kml/2.2">
      <Document>
        <Folder>
          <name>Líneas del metro </name>
          <Placemark>
            <name>lo que sea</name>
          </Placemark>
        </Folder>
      </Document>
    </kml>
    """
    def klmParser = new KlmParser() //.parse(xml)
    klmParser.parse(xml)
    println klmParser.lines
    assert klmParser.lines.size() == 1
  }

  void testParseCoordinatesHappyPath(){
    def klmParser = new KlmParser()
    def coordinates = klmParser.parseCoordinates("1,2,3")
    assert coordinates.size() == 1
    assert coordinates[0].x == 1
    assert coordinates[0].y == 2
  }

  void testParseEmptyCoordinate(){
    def klmParser = new KlmParser()
    def coordinates = klmParser.parseCoordinates("")
    assert coordinates.size() == 0
    assert coordinates.size() != 1
  }

  void testParseCoordinatesWithSpaces(){
    def klmParser = new KlmParser()
    def coordinates = klmParser.parseCoordinates("  1,   2,3   ")
    assert coordinates.size() == 1
    assert coordinates[0].x == 1
    assert coordinates[0].y == 2
  }

  void testParseCoordinatesWithEmptyLine(){
    def klmParser = new KlmParser()
    def coordinates = klmParser.parseCoordinates("""
              1,2,3
              """)
    assert coordinates.size() == 1
    assert coordinates[0].x == 1
    assert coordinates[0].y == 2
  }

  void testParseCoordinatesWithManyLines(){
    def klmParser = new KlmParser()
    def coordinates = klmParser.parseCoordinates("1,2,3\n4,5,6\n7,8,9.0")
    assert coordinates.size() == 3
    assert coordinates[0].matches(1,2)
    assert coordinates[1].matches(4,5)
    assert coordinates[2].matches(7,8)
  }

  void testReadLineWithName(){
   String xml = """
    <kml xmlns="http://www.opengis.net/kml/2.2">
      <Document>
        <Folder>
          <name>Líneas del metro </name>
          <Placemark>
           <name>Línea 7</name>
           <styleUrl>#line-F8971B-10300-nodesc</styleUrl>
            <LineString>
             <tessellate>1</tessellate>
              <coordinates>
              -99.189586,19.3607037,0
              -99.1878051,19.3761645,0
              -99.1863245,19.3847724,0
              -99.1860509,19.3912798,0
              -99.187097,19.4031605,0
              -99.1912866,19.4119136,0
              -99.1919732,19.4255433,0
              -99.1910237,19.4335161,0
              -99.1914367675781,19.4468455104825,0
              -99.1870825,19.4586984,0
              -99.1905999,19.4700922,0
              -99.1905784606934,19.4806724417591,0
              -99.1948807000001,19.4905036,0
              -99.2000628,19.5046121,0
            </coordinates>
          </LineString>
        </Placemark>
        </Folder>
      </Document>
    </kml>
    """
    def klmParser = new KlmParser()
    klmParser.parse(xml)
    def firstLine = klmParser.lines[0]
    def coords = firstLine.coordinates
    assert firstLine.name == "Línea 7"
    assert coords.size() == 14
    assert coords[0].matches(-99.189586,19.3607037)
    assert coords[7].matches(-99.1910237,19.4335161)
    assert coords[13].matches(-99.2000628,19.5046121)

  }

  void testReadManyLines(){
   String xml = """
    <kml xmlns="http://www.opengis.net/kml/2.2">
      <Document>
        <Folder>
          <name>Líneas del metro </name>
          <Placemark>
           <name>Línea 7</name>
           <styleUrl>#line-F8971B-10300-nodesc</styleUrl>
            <LineString>
             <tessellate>1</tessellate>
              <coordinates>
              -99.189586,19.3607037,0
              -99.1878051,19.3761645,0
              -99.1863245,19.3847724,0
              -99.1860509,19.3912798,0
              -99.187097,19.4031605,0
              -99.1912866,19.4119136,0
              -99.1919732,19.4255433,0
              -99.1910237,19.4335161,0
              -99.1914367675781,19.4468455104825,0
              -99.1870825,19.4586984,0
              -99.1905999,19.4700922,0
              -99.1905784606934,19.4806724417591,0
              -99.1948807000001,19.4905036,0
              -99.2000628,19.5046121,0
            </coordinates>
          </LineString>
        </Placemark>
        <Placemark>
           <name>Línea 8</name>
           <styleUrl>#line-F8971B-10300-nodesc</styleUrl>
            <LineString>
             <tessellate>1</tessellate>
              <coordinates>
              -99.189586,19.3607037,0
              -99.1878051,19.3761645,0
              -99.1863245,19.3847724,0
              -99.1860509,19.3912798,0
              -99.187097,19.4031605,0
              -99.1912866,19.4119136,0
              -99.1919732,19.4255433,0
              -99.1910237,19.4335161,0
              -99.1914367675781,19.4468455104825,0
              -99.1870825,19.4586984,0
              -99.1905999,19.4700922,0
              -99.1905784606934,19.4806724417591,0
              -99.1948807000001,19.4905036,0
              -99.2000628,19.5046121,0
            </coordinates>
          </LineString>
        </Placemark>
        </Folder>
      </Document>
    </kml>
    """
    def klmParser = new KlmParser()
    klmParser.parse(xml)
    assert klmParser.lines[0].name == "Línea 7"
    assert klmParser.lines[0].coordinates.size() == 14
    assert klmParser.lines[1].name == "Línea 8"
    assert klmParser.lines[1].coordinates.size() == 14
  }

  void testReadAllLines(){
   String xml = leerXML("/Users/carlogilmar/Desktop/codeCamp/lineas.xml")
    def klmParser = new KlmParser()
    klmParser.parse(xml)
    assert klmParser.lines[0].name == "Línea 1"
    assert klmParser.lines[0].coordinates.size() == 20
    assert klmParser.lines[7].name == "Línea 8"
    assert klmParser.lines[7].coordinates.size() == 19
  }

  void testReadOneStation(){
   String xml = leerXML("/Users/carlogilmar/Desktop/codeCamp/lineas.xml")
    def klmParser = new KlmParser()
    klmParser.parse(xml)
    assert klmParser.stations.size() > 0
    assert klmParser.stations[0].name == "Acatitla"
    assert klmParser.stations[3].coordinate.y == 19.4356521 
  }

  void testBindStationsAndLines(){
   String xml = leerXML("/Users/carlogilmar/Desktop/codeCamp/lineas.xml")
    def klmParser = new KlmParser()
    klmParser.parse(xml)
    assert klmParser.stations[0].lines.size() > 0
  }



  def leerXML(String name){
    def file = new File(name)
    file.text
  }


}
