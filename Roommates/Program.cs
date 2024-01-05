using Roommates.Models;
using Roommates.Repositories;
using System;
using System.Collections.Generic;

namespace Roommates
{
    class Program
    {
        //  This is the address of the database.
        //  We define it here as a constant since it will never change.
        private const string CONNECTION_STRING = @"server=localhost\SQLExpress;database=Roommates;integrated security=true; TrustServerCertificate=True;";

        static void Main(string[] args)
        {
            RoomRepository roomRepo = new RoomRepository(CONNECTION_STRING);
            ChoreRepository choreRepo = new ChoreRepository(CONNECTION_STRING);
            RoommateRepository roommateRepo = new RoommateRepository(CONNECTION_STRING);
            bool runProgram = true;
            while (runProgram)
            {
                string selection = GetMenuSelection();

                switch (selection)
                {
                    case ("Show all rooms"):
                        List<Room> rooms = roomRepo.GetAll();
                        foreach (Room r in rooms)
                        {
                            Console.WriteLine($"{r.Name} has an Id of {r.Id} and a max occupancy of {r.MaxOccupancy}");
                        }
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case ("Show all chores"):
                        List<Chore> chores = choreRepo.GetAll();
                        foreach (Chore c in chores)
                        {
                            Console.WriteLine($"{c.Name} has an Id of {c.Id}");
                        }
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case ("Show all unassigned chores"):
                        chores = choreRepo.GetUnassignedChores();
                        foreach (Chore c in chores)
                        {
                            Console.WriteLine($"Unassigned Chore: {c.Name}, Chore ID# {c.Id}");
                        }
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case ("Show all roommates"):
                        List<Roommate> roommates = roommateRepo.GetAll();
                        foreach (Roommate r in roommates)
                        {
                            Console.WriteLine($"Roommate #{r.Id}: {r.FirstName} {r.LastName} - they pay {r.RentPortion}% of the total rent.");
                        }
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case ("Search for room"):
                        Console.Write("Room Id: ");
                        int id = int.Parse(Console.ReadLine());

                        Room room = roomRepo.GetById(id);

                        Console.WriteLine($"{room.Id} - {room.Name} Max Occupancy({room.MaxOccupancy})");
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case ("Search for chore"):
                        Console.Write("Chore Id: ");
                        id = int.Parse(Console.ReadLine());

                        Chore chore = choreRepo.GetById(id);

                        Console.WriteLine($"{chore.Id} - {chore.Name}");
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case ("Search for roommate"):
                        Console.Write("Roommate Id: ");
                        id = int.Parse(Console.ReadLine());

                        Roommate roommate = roommateRepo.GetById(id);

                        Console.WriteLine($"Roommate #{roommate.Id} - Name: {roommate.FirstName} - {roommate.RentPortion}% Rent Portion - Lives in: {roommate.RoomName}");
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case ("Search roommate by room"):
                        Console.Write("Room Id: ");
                        int roomId = int.Parse(Console.ReadLine());

                        List <Roommate> roommatesByRoom = roommateRepo.GetRoommatesByRoomId(roomId);

                        foreach (Roommate r in roommatesByRoom)
                        {
                            Console.WriteLine($"Room #{r.Room.Id}: {r.Room.Name} - {r.FirstName} {r.LastName} lives in here and pays {r.RentPortion}% of the rent.");
                        }
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case ("Add a room"):
                        Console.Write("Room name: ");
                        string name = Console.ReadLine();

                        Console.Write("Max occupancy: ");
                        int max = int.Parse(Console.ReadLine());

                        Room roomToAdd = new Room()
                        {
                            Name = name,
                            MaxOccupancy = max
                        };

                        roomRepo.Insert(roomToAdd);

                        Console.WriteLine($"{roomToAdd.Name} has been added and assigned an Id of {roomToAdd.Id}");
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case ("Add a chore"):
                        Console.Write("Chore name: ");
                        name = Console.ReadLine();

                        Chore choreToAdd = new Chore()
                        {
                            Name = name
                        };

                        choreRepo.Insert(choreToAdd);

                        Console.WriteLine($"{choreToAdd.Name} has been added and assigned an Id of {choreToAdd.Id}");
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case ("Add a roommate"):
                        Console.Write("New roommate first name: ");
                        string firstName = Console.ReadLine();
                        Console.Write("New roommate last name: ");
                        string lastName = Console.ReadLine();
                        Console.Write("New roommate's rent portion: ");
                        int rentPortion = int.Parse(Console.ReadLine());
                        Console.Write("New roommate's Room ID: ");
                        int roommateRoomId = int.Parse(Console.ReadLine());

                        Roommate roommateToAdd = new Roommate()
                        {
                            FirstName = firstName,
                            LastName = lastName,
                            RentPortion = rentPortion,
                            Room = roomRepo.GetById(roommateRoomId),
                            MoveInDate = DateTime.Today
                        };

                        roommateRepo.Insert(roommateToAdd);

                        Console.WriteLine($"{roommateToAdd.FirstName} {roommateToAdd.LastName} has been added! They live in the {roommateToAdd.Room.Name} now. STAY OUT OF THERE!");
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case ("Delete a roommate"):
                        List<Roommate> deleteRoommates = roommateRepo.GetAll();
                        foreach (Roommate r in deleteRoommates)
                        {
                            Console.WriteLine($"Roommate ID# {r.Id}: {r.FirstName} {r.LastName}");
                        }
                        Console.WriteLine("Enter the roommate's ID who you're coldly deleting, you cold MF: ");
                        int roommateID = int.Parse(Console.ReadLine());
                        Roommate deletedRoommate = roommateRepo.GetById(roommateID);
                        Console.WriteLine($"{deletedRoommate.FirstName} is getting kicked out of the house. They out here messy crying and stuff, but you know it had to be done.");
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        roommateRepo.Delete(roommateID);
                        break;
                    case ("Update a roommate"):
                        List<Roommate> updateRoommates = roommateRepo.GetAll();
                        foreach (Roommate r in updateRoommates)
                        {
                            Console.WriteLine($"Roommate ID# {r.Id}: {r.FirstName} {r.LastName}");
                        }
                        Console.WriteLine("Enter the roommate's ID who you're updating, hopefully with their consent: ");
                        roommateID = int.Parse(Console.ReadLine());
                        Roommate updatedRoommate = roommateRepo.GetById(roommateID);
                        Console.WriteLine($"OK Here's the person you want to update: {updatedRoommate.FirstName} {updatedRoommate.LastName}");
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        bool updateLoop = true;
                        while (updateLoop)
                        {
                          Console.Clear();
                          Console.WriteLine("What field do you want to update?");
                          Console.WriteLine();
                          Console.WriteLine(@"**Press 1 for First Name**
**Press 2 for Last Name**
**Press 3 for Rent Portion**
**Press 4 for Move In Date**
**Press 5 for Room**
**Press 6 to Stop Updating**");
                          int userUpdateID = int.Parse(Console.ReadLine());
                          switch (userUpdateID)
                            {
                                case (1):
                                    Console.WriteLine($"Enter new first name. (Current first name -- {updatedRoommate.FirstName}):  ");
                                    string newFirstName = Console.ReadLine();
                                    updatedRoommate.FirstName = newFirstName;
                                    Console.WriteLine($"OK, great! First name has been updated to {updatedRoommate.FirstName}");
                                    Console.Write("Press any key to continue");
                                    Console.ReadKey();
                                    break;
                                case (2):
                                    Console.WriteLine($"Enter new last name. (Current first name -- {updatedRoommate.LastName}):  ");
                                    string newLastName = Console.ReadLine();
                                    updatedRoommate.LastName = newLastName;
                                    Console.WriteLine($"OK, great! Last name has been updated to {updatedRoommate.LastName}");
                                    Console.Write("Press any key to continue");
                                    Console.ReadKey();
                                    break;
                                case (3):
                                    Console.WriteLine($"Enter new rent portion. (Current rent portion -- {updatedRoommate.RentPortion}%):  ");
                                    int newRentPortion = int.Parse(Console.ReadLine());
                                    updatedRoommate.RentPortion = newRentPortion;
                                    Console.WriteLine($"OK, great! Rent portion has been updated to {updatedRoommate.RentPortion}%");
                                    Console.Write("Press any key to continue");
                                    Console.ReadKey();
                                    break;
                                case (4):
                                    Console.WriteLine($"Enter new move in date - formatted as YYYY-MM-DD. (Current move in date -- {updatedRoommate.MoveInDate}):  ");
                                    DateTime newMoveDate = DateTime.Parse(Console.ReadLine());
                                    updatedRoommate.MoveInDate = newMoveDate;
                                    Console.WriteLine($"OK, great! Move in date has been updated to {updatedRoommate.MoveInDate}");
                                    Console.Write("Press any key to continue");
                                    Console.ReadKey();
                                    break;
                                case (5):
                                    Console.WriteLine($"Enter new room ID for {updatedRoommate.FirstName}. (Current room Id -- {updatedRoommate.Room.Id}, {updatedRoommate.Room.Name}):  ");
                                    int newRoomId = int.Parse(Console.ReadLine());
                                    updatedRoommate.Room = roomRepo.GetById(newRoomId);
                                    Console.WriteLine($"OK, great! {updatedRoommate.FirstName}'s room has been updated to {updatedRoommate.Room.Name}, which is room #{updatedRoommate.Room.Id}");
                                    Console.Write("Press any key to continue");
                                    Console.ReadKey();
                                    break;
                                case (6):
                                    roommateRepo.Update(updatedRoommate);
                                    updateLoop = false;
                                    break;
                            }
                          
                        }
                        break;
                    case ("Exit"):
                        runProgram = false;
                        break;
                }
            }

            static string GetMenuSelection()
            {
                Console.Clear();

                List<string> options = new List<string>()
            {
                "Show all rooms",
                "Search for room",
                "Add a room",
                "Show all chores",
                "Show all unassigned chores",
                "Search for chore",
                "Add a chore",
                "Show all roommates",
                "Search for roommate",
                "Search roommate by room",
                "Add a roommate",
                "Delete a roommate",
                "Update a roommate",
                "Exit"
            };

                for (int i = 0; i < options.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {options[i]}");
                }

                while (true)
                {
                    try
                    {
                        Console.WriteLine();
                        Console.Write("Select an option > ");

                        string input = Console.ReadLine();
                        int index = int.Parse(input) - 1;
                        return options[index];
                    }
                    catch (Exception)
                    {
                        continue;
                    }
                }
            }
        }
    }
}