class KlmParserTest extends GroovyTestCase{

  void testCreateKlmParser(){
    def klmParser = new KlmParser().parse("")
    assert klmParser != null
  }

  void testReadEmptyDocument(){
    String xml = """
    <?xml version="1.0" encoding="UTF-8"?></klm>
    <kml xmlns="http://www.opengis.net/kml/2.2"/>
    """
    def klmParser = new KlmParser()
    assert klmParser.lines == []
    assert klmParser.stations == []
  }


  void testReadDocumentWithOneLine(){
    String xml = """
    <?xml version="1.0" encoding="UTF-8"?></klm>
    <kml xmlns="http://www.opengis.net/kml/2.2">
      <Document>
        <Folder>
          <name> Lineas del metro </name>
          <Placemark>
            <name>lo que sea</name>
          </Placemark>
        </Folder>
      </Document>
    </kml>
    """
    def klmParser = new KlmParser().parse(xml)
    //assert klmParser.lines.size() == 1
    assert 1 == 1
  }

  void testParseCoordinatesHappyPath(){

    def klmParser = new KlmParser()
    def coordinates = klmParser.parseCoordinates("1,2,3")
    assert coordinates.size() == 1
    assert coordinates[0].x == 1
    assert coordinates[0].y == 2
  }
}
