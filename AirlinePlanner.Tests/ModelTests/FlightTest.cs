using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System;
using AirlinePlanner.Models;
using AirlinePlanner;
using MySql.Data.MySqlClient;


namespace AirlinePlanner.Tests
{

   [TestClass]
   public class FlightTests : IDisposable
   {
     public FlightTests()
     {
         DBConfiguration.ConnectionString = "server=localhost;user id=root;password=root;port=8889;database=airline_planner;";
     }
      public void Dispose()
      {
        City.DeleteAll();
        Flight.DeleteAll();
      }
      [TestMethod]
      public void Save_SavesFlightToDatabase_FlightList()
      {
        Flight testFlight = new Flight("H23344", "2018-05-09", "Portland", "New-York", "Delayed",  1);
        testFlight.Save();

        List<Flight> testResult = Flight.GetAllFlights();
        List<Flight> allFlights = new List<Flight> {testFlight};

        CollectionAssert.AreEqual(testResult, allFlights);
      }
      [TestMethod]
      public void Save_DatabaseAssignsIdToObject_Id()
      {
        //Arrange
        Flight testFlight = new Flight("H23344", "2018-05-09", "Portland", "New-York", "Delayed",  1);
        testFlight.Save();

        //Act
        Flight savedFlight = Flight.GetAllFlights()[0];

        int result = savedFlight.GetId();
        int testId = testFlight.GetId();

        //Assert
        Assert.AreEqual(testId, result);
      }
      [TestMethod]
      public void Equals_OverrideTrueForSameName_Flight()
      {
        //Arrange, Act
        Flight firstFlight = new Flight("H23344", "2018-05-09", "Portland", "New-York", "Delayed",  1);
        Flight secondFlight = new Flight("H23344", "2018-05-09", "Portland", "New-York", "Delayed",  1);

        //Assert
        Assert.AreEqual(firstFlight, secondFlight);
      }
      [TestMethod]
      public void Find_FindsFlightInDatabase_Flight()
      {
        //Arrange
        Flight testFlight = new Flight("H23344", "2018-05-09", "Portland", "New-York", "Delayed",  1);
        testFlight.Save();

        //Act
        Flight foundFlight = Flight.Find(testFlight.GetId());

        //Assert
        Assert.AreEqual(testFlight, foundFlight);
      }
      [TestMethod]
       public void GetCities_ReturnsAllCityFlights_CityList()
       {
         //Arrange
         Flight testFlight = new Flight("H1212", "2018-05-09", "Portland", "New-York", "Delayed");
         testFlight.Save();

         City testCity1 = new City("Portland");
         testCity1.Save();

         City testCity2 = new City("Los-Angeles");
         testCity2.Save();

         //Act
         testFlight.AddCity(testCity1);
         testFlight.AddCity(testCity2);
         List<City> result = testFlight.GetCities();
         List<City> testList = new List<City> {testCity1, testCity2};

         //Assert
         CollectionAssert.AreEqual(testList, result);
       }
    }
}
