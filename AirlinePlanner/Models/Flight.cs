using System;
using System.Collections.Generic;
using AirlinePlanner.Models;

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
    public static List<Flight> GetAllFlights()
    {
      list<Flight> allFlights = new List<Flight>{};
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
  }
}
