using System;
using System.Collections.Generic;
using AirlinePlanner.Models;
using MySql.Data.MySqlClient;

namespace AirlinePlanner
{
  public class Flight
  {
    private int _id;
    private string _flightName;
    private string _date;
    private string _deptCity;
    private string _arrivalCity;
    private string _status;

    public Flight(string flightName, string date, string departureCity, string arrivalCity, string status, int id = 0)
    {
      _flightName = flightName;
      _date = date;
      _deptCity = departureCity;
      _arrivalCity = arrivalCity;
      _status = status;
      _id = id;
    }
    public string GetFlightName()
    {
      return _flightName;
    }
    public string GetDate()
    {
      return _date;
    }
    public string GetDepartureCity()
    {
      return _deptCity;
    }
    public string GetArrivalCity()
    {
      return _arrivalCity;
    }
    public string GetStatus()
    {
      return _status;
    }
    public int GetId()
    {
      return _id;
    }
    public override bool Equals(System.Object otherFlight)
    {
      if (!(otherFlight is Flight))
      {
        return false;
      }
      else
      {
         Flight newFlight = (Flight) otherFlight;
         bool idEquality = this.GetId() == newFlight.GetId();
         bool nameEquality = this.GetFlightName() == newFlight.GetFlightName();
         bool dateEquality = this.GetDate() == newFlight.GetDate();
         bool departureEquality = this.GetDepartureCity() == newFlight.GetDepartureCity();
         bool arrivalEquality = this.GetArrivalCity() == newFlight.GetArrivalCity();
         bool statusEquality = this.GetStatus() == newFlight.GetStatus();
         return (idEquality && nameEquality);
       }
    }
    public override int GetHashCode()
    {
         return this.GetFlightName().GetHashCode();
    }

    public static List<Flight> GetAllFlights()
    {
      List<Flight> allFlights = new List<Flight>{};
      MySqlConnection conn = DB.Connection();
      conn.Open();
      MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM flights";
      MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;

      while(rdr.Read())
      {
        int id = rdr.GetInt32(0);
        string flightName = rdr.GetString(1);
        string date = rdr.GetString(2);
        string departureCity = rdr.GetString(3);
        string arrivalCity = rdr.GetString(4);
        string status = rdr.GetString(5);
        Flight newFlight = new Flight(flightName, date, departureCity, arrivalCity, status, id);
        allFlights.Add(newFlight);
      }
      conn.Close();
      if (conn != null)
      {
          conn.Dispose();
      }
      return allFlights;

    }
    public void Save()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"INSERT INTO flights (flight_name, flight_date, departure_city, arrival_city, flight_status) VALUES (@thisName, @thisDate, @thisDeparture, @thisArrival, @thisStatus);";

      cmd.Parameters.Add(new MySqlParameter("@thisName", _flightName));
      cmd.Parameters.Add(new MySqlParameter("@thisDate", _date));
      cmd.Parameters.Add(new MySqlParameter("@thisDeparture", _deptCity));
      cmd.Parameters.Add(new MySqlParameter("@thisArrival", _arrivalCity));
      cmd.Parameters.Add(new MySqlParameter("@thisStatus", _status));

      cmd.ExecuteNonQuery();
      _id = (int) cmd.LastInsertedId;

      conn.Close();
      if (conn != null)
      {
          conn.Dispose();
      }
    }

    public static Flight Find (int id)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;

      cmd.CommandText= @"SELECT * FROM flights WHERE id = (@searchId);";

      MySqlParameter searchId = new MySqlParameter();
      searchId.ParameterName = "@searchId";
      searchId.Value = id;
      cmd.Parameters.Add(searchId);

      var rdr = cmd.ExecuteReader() as MySqlDataReader;
      int flightId = 0;
      string flightName = "";
      string flightDate = "";
      string flightDeparture = "";
      string flightArrival = "";
      string flightStatus = "";

      while(rdr.Read())
      {
        flightId = rdr.GetInt32(0);
        flightName = rdr.GetString(1);
        flightDate = rdr.GetString(2);
        flightDeparture = rdr.GetString(3);
        flightArrival = rdr.GetString(4);
        flightStatus = rdr.GetString(5);
      }
      Flight newFlight = new Flight(flightName, flightDate, flightDeparture, flightArrival, flightStatus, flightId);

      conn.Close();
      if (conn != null)
      {
          conn.Dispose();
      }
      return newFlight;
    }

    public void AddCity(City newCity)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"INSERT INTO flights_cities (flight_id, city_id) VALUES (@FlightId, @CityId);";

      MySqlParameter flight_id = new MySqlParameter();
      flight_id.ParameterName = "@FlightId";
      flight_id.Value = _id;
      cmd.Parameters.Add(flight_id);

      MySqlParameter city_id = new MySqlParameter();
      city_id.ParameterName = "@CityId";
      city_id.Value = newCity.GetCityId();
      cmd.Parameters.Add(city_id);

      cmd.ExecuteNonQuery();
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }

    public List<City> GetCities()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT cities.* FROM flights
      JOIN flights_cities ON (flight_id = flights_cities.flight_id)
      JOIN cities ON (flights_cities.city_id=cities.id)
      WHERE flight_id = @FlightId;";

      MySqlParameter flightIdParameter = new MySqlParameter();
      flightIdParameter.ParameterName = "@FlightId";
      flightIdParameter.Value = _id;
      cmd.Parameters.Add(flightIdParameter);

      var rdr = cmd.ExecuteReader() as MySqlDataReader;

      List<City> cities = new List<City> {};
      while(rdr.Read())
      {
        int cityId = rdr.GetInt32(0);
        string cityName = rdr.GetString(1);
        City newCity = new City(cityName, cityId);
        cities.Add(newCity);
      }
      rdr.Dispose();

      conn.Close();
      if (conn != null)
      {
         conn.Dispose();
      }
      return cities;
    }

    public static void DeleteAll()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"DELETE FROM flights;";
      cmd.ExecuteNonQuery();
      conn.Close();
      if (conn != null)
      {
          conn.Dispose();
      }
    }
  }
}
