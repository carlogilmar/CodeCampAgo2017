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
    String xml = """
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
            <name>lo que sea</name>
            <LineString>
              <coordinates>
              1,2,3
              </coordinates>
            </LineString>
          </Placemark>
        </Folder>
      </Document>
    </kml>
    """
    def klmParser = new KlmParser() 
    klmParser.parse(xml)
    assert klmParser.lines[0].name == "lo que sea"
    assert klmParser.lines[0].coordinates.size() == 1 
  }

}
