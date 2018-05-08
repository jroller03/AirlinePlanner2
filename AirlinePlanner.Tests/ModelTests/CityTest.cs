using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System;
using AirlinePlanner.Models;
using AirlinePlanner;
using MySql.Data.MySqlClient;


namespace AirlinePlanner.Tests
{

   [TestClass]
   public class CityTests : IDisposable
   {
       public CityTests()
       {
           DBConfiguration.ConnectionString = "server=localhost;user id=root;password=root;port=8889;database=airline_planner;";
       }
        public void Dispose()
        {
          City.DeleteAll();
          Flight.DeleteAll();
        }
         [TestMethod]
         public void Save_SavesCityToDatabase_CityList()
         {
           City testCity = new City("Seattle", 1);
           testCity.Save();

           List<City> testResult = City.GetAllCities();
           List<City> allCities = new List<City> {testCity};

           CollectionAssert.AreEqual(testResult, allCities);
         }
         [TestMethod]
         public void Save_DatabaseAssignsIdToObject_Id()
         {
           //Arrange
           City testCity = new City("Seattle", 1);
           testCity.Save();

           //Act
           City savedCity = City.GetAllCities()[0];

           int result = savedCity.GetCityId();
           int testId = testCity.GetCityId();

           //Assert
           Assert.AreEqual(testId, result);
         }

         [TestMethod]
         public void Equals_OverrideTrueForSameName_City()
         {
           //Arrange, Act
           City firstCity = new City("Seattle", 1);
           City secondCity = new City("Seattle", 1);

           //Assert
           Assert.AreEqual(firstCity, secondCity);
         }

         [TestMethod]
         public void Find_FindsCityInDatabase_City()
         {
           //Arrange
           City testCity = new City("Seattle", 1);
           testCity.Save();

           //Act
           City foundCity = City.Find(testCity.GetCityId());

           //Assert
           Assert.AreEqual(testCity, foundCity);
         }
         [TestMethod]
          public void GetFlights_ReturnsAllCityFlights_FlightList()
          {
            //Arrange
            City testCity = new City("Seattle");
            testCity.Save();

            Flight testFlight1 = new Flight("H1212", "2018-05-09", "Portland", "New-York", "Delayed");
            testFlight1.Save();

            Flight testFlight2 = new Flight("DELTA323", "2018-06-09", "Los-Angeles", "Miami", "On Time");
            testFlight2.Save();

            //Act
            testCity.AddFlight(testFlight1);
            testCity.AddFlight(testFlight2);
            List<Flight> result = testCity.GetFlights();
            List<Flight> testList = new List<Flight> {testFlight1, testFlight2};

              // Console.WriteLine to see what elements in the List
              foreach(var flight in testList)
              {
                Console.WriteLine(flight.GetFlightName());
              }
              Console.WriteLine();

            //Assert
            CollectionAssert.AreEqual(testList, result);
          }


    }
}
