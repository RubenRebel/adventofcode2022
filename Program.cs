using AdventofCode2022;
using AdventofCode2022.Solutions;

var reader = new FileReader();
var stringUtils = new StringUtilities();
var calculationUtils = new CalculationUtilities();

var day1 = new Day1(reader, stringUtils, calculationUtils);
var day2 = new Day2(reader, stringUtils, calculationUtils);
var day3 = new Day3(reader, stringUtils, calculationUtils);


Console.ReadKey();