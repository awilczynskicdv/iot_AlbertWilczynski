using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace lab1
{
    public class DatabaseContext
    {
        private readonly string connectionString;
        private const string Query = "Select * from people";

        public DatabaseContext(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public IEnumerable<Person> GetPeople()
        {
            var people = new List<Person>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(Query, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    people.Add(new Person
                    {
                        PersonId = Convert.ToInt32(reader["PersonId"]),
                        FirstName = reader["FirstName"].ToString(),
                        LastName = reader["LastName"].ToString(),
                        Phonenumber = reader["Phonenumber"].ToString()

                    });
                }
                reader.Close();
            }

            return people;
        }
        public void AddPerson(string fName, string lName, string phonenumber){

            string QueryAdd = $"Insert into People values('{fName}', '{lName}', '{phonenumber}')";
            Int32 rows;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(QueryAdd, connection);
                connection.Open();     
                command.ExecuteNonQuery();        
            }
        }

        public Person GetPerson(int personId){

            string QueryGet = $"Select * from People where PersonId = {personId}";
            var person = new Person();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(QueryGet, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if(reader.Read()){
                    person.PersonId = Convert.ToInt32(reader["PersonId"]);
                    person.FirstName = reader["FirstName"].ToString();
                    person.LastName = reader["LastName"].ToString();
                    person.Phonenumber = reader["Phonenumber"].ToString();
                }              
                reader.Close();      
            }
            return person;
        }

        public void RemovePerson(int personId){

            string QueryDelete = $"Delete from People where PersonId = {personId}";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(QueryDelete, connection);
                connection.Open(); 
                command.ExecuteNonQuery();
            }
        }

    }
}