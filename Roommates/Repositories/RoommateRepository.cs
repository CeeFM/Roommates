using Microsoft.Data.SqlClient;
using Roommates.Models;
using System.Collections.Generic;

namespace Roommates.Repositories
{
    public class RoommateRepository : BaseRepository
    {
        private const string CONNECTION_STRING = @"server=localhost\SQLExpress;database=Roommates;integrated security=true; TrustServerCertificate=True;";
        public RoommateRepository(string connectionString) : base(connectionString) { }

        public Roommate GetById(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT r.FirstName, r.RentPortion, r.LastName, r.MoveInDate, r.RoomId, ro.Name, ro.Id, ro.MaxOccupancy FROM Roommate r JOIN Room ro ON r.RoomId = ro.Id WHERE r.Id = @id";
                    cmd.Parameters.AddWithValue("@id", id);
                    SqlDataReader reader = cmd.ExecuteReader();
                    int Id = reader.GetOrdinal("Id");
                    Roommate roommate = null;

                    if (reader.Read())
                    {
                        roommate = new Roommate
                        {
                            Id = id,
                            FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                            LastName = reader.GetString(reader.GetOrdinal("LastName")),
                            RentPortion = reader.GetInt32(reader.GetOrdinal("RentPortion")),
                            MoveInDate = reader.GetDateTime(reader.GetOrdinal("MoveInDate")),
                            RoomName = reader.GetString(reader.GetOrdinal("Name")),
                            Room = new Room
                            {
                                Name = reader.GetString(reader.GetOrdinal("Name")),
                                MaxOccupancy = reader.GetInt32(reader.GetOrdinal("MaxOccupancy")),
                                Id = Id

                            }
                        };
                    }

                    reader.Close();

                    return roommate;
                }
            }
        }

        public List<Roommate> GetAll()
        {
            using (SqlConnection conn = Connection) 
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand ())
                {
                    cmd.CommandText = "SELECT * FROM Roommate";
                    SqlDataReader reader = cmd.ExecuteReader();
                    List<Roommate> roommates = new List<Roommate>();
                    while (reader.Read())
                    {
                        int idColumnPosition = reader.GetOrdinal("Id");
                        int idValue = reader.GetInt32(idColumnPosition);
                        int firstNameColumnPosition = reader.GetOrdinal("FirstName");
                        string firstNameValue = reader.GetString(firstNameColumnPosition);
                        int lastNameColumnPosition = reader.GetOrdinal("LastName");
                        string lastNameValue = reader.GetString(lastNameColumnPosition);
                        int rentColumnPosition = reader.GetOrdinal("RentPortion");
                        int rentValue = reader.GetInt32(rentColumnPosition);
                        int moveDateColumnPosition = reader.GetOrdinal("MoveInDate");
                        DateTime moveDateValue = reader.GetDateTime(moveDateColumnPosition);

                        Roommate roommate = new Roommate
                        {
                            Id = idValue,
                            FirstName = firstNameValue,
                            LastName = lastNameValue,
                            RentPortion = rentValue,
                            MoveInDate = moveDateValue,
                            Room = null
                        };

                        roommates.Add(roommate);

                    }

                    reader.Close();

                    return roommates;

                }
            }
        }

        public List<Roommate> GetRoommatesByRoomId(int roomId)
        {

            RoomRepository roomRepo = new RoomRepository(CONNECTION_STRING);

            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {

                    cmd.CommandText = "SELECT r.FirstName, r.LastName, r.RentPortion, r.MoveInDate, r.Id, ro.Name FROM Roommate r JOIN Room ro ON r.RoomId = ro.Id WHERE ro.Id = @roomId";
                    cmd.Parameters.AddWithValue("@roomId", roomId);
                    Room thisRoom = roomRepo.GetById(roomId);
                    SqlDataReader reader = cmd.ExecuteReader();

                    Roommate roommate = null;
                    List<Roommate> roommates = new List<Roommate>();

                    if (reader.Read())
                    {
                        roommate = new Roommate
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                            LastName = reader.GetString(reader.GetOrdinal("LastName")),
                            RentPortion = reader.GetInt32(reader.GetOrdinal("RentPortion")),
                            MoveInDate = reader.GetDateTime(reader.GetOrdinal("MoveInDate")),
                            Room = thisRoom,
                            RoomName = reader.GetString(reader.GetOrdinal("Name"))
                        };
                        roommates.Add(roommate);
                    }

                    reader.Close();

                    return roommates;
                }
            }
        }

        public void Delete(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                { 
                    cmd.CommandText = $"DELETE FROM Roommate WHERE Id = @id";
                    cmd.Parameters.AddWithValue("@id", id);
                    SqlDataReader reader = cmd.ExecuteReader();
                }
            }
        }

        public void Insert(Roommate roommate)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open ();
                using (SqlCommand cmd = conn.CreateCommand ())
                {
                    cmd.CommandText = @"INSERT INTO Roommate (firstname, lastname, rentportion, moveindate, roomid)
                                        OUTPUT INSERTED.Id
                                        VALUES (@firstname, @lastname, @rentportion, @moveindate, @roomid)";
                    cmd.Parameters.AddWithValue("@firstname", roommate.FirstName);
                    cmd.Parameters.AddWithValue("@lastname", roommate.LastName);
                    cmd.Parameters.AddWithValue("@rentportion", roommate.RentPortion);
                    cmd.Parameters.AddWithValue("@moveindate", roommate.MoveInDate);
                    cmd.Parameters.AddWithValue("@roomid", roommate.Room.Id);
                    int id = (int)cmd.ExecuteScalar();
                    roommate.Id = id;

                }
            }
        }

        public void Update(Roommate roommate)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open ();
                using (SqlCommand cmd = conn.CreateCommand ())
                {
                    cmd.CommandText = @"UPDATE Roommate 
                                         SET FirstName = @FirstName,
                                             LastName = @LastName,
                                             RentPortion = @RentPortion,
                                             MoveInDate = @MoveInDate,
                                             RoomId = @RoomId
                                         WHERE Id = @Id";
                    cmd.Parameters.AddWithValue("@FirstName", roommate.FirstName);
                    cmd.Parameters.AddWithValue("@LastName", roommate.LastName);
                    cmd.Parameters.AddWithValue("@RentPortion", roommate.RentPortion);
                    cmd.Parameters.AddWithValue("@MoveInDate", roommate.MoveInDate);
                    cmd.Parameters.AddWithValue("@RoomId", roommate.Room.Id);
                    cmd.Parameters.AddWithValue("@Id", roommate.Id);
                    SqlDataReader reader = cmd.ExecuteReader();

                }
            }
        }
    }
}
