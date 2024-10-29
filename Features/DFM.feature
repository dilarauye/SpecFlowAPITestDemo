Feature: DFM
DFM Post API tests

#Scenario: DFM post API test, response as a string
#	When user sends post request with '<payload>'
#	Then response should match with '<expectedResponse>'
#	Examples:
#	| payload       | expectedResponse       |
#	| payload1.json | expectedResponse1.json |
#	| payload2.json | expectedResponse2.json |
#	| payload3.json | expectedResponse3.json |


Scenario: DFM post API test, response as an object
	When user sends post request with '<payload>' response should match with '<expectedResponse>' object
	Examples:
	| payload       | expectedResponse       |
	| payload1.json | expectedResponse1.json |
	| payload2.json | expectedResponse2.json |
	| payload3.json | expectedResponse3.json |