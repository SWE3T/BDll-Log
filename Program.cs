using System;
using System.Data;
using System.Diagnostics;
using Microsoft.VisualBasic.FileIO;
using Npgsql;

namespace REDO
{
    static class Program
    {
        static void Main()
        {
            showTable();
        }

        private static void showTable()
        {
            // Specify connection options and open an connection
            NpgsqlConnection conn = new NpgsqlConnection(
                "Host=localhost;Username=usuario;Password=123456789;Database=trabalho2"
            );
            conn.Open();

            // Define a query
            NpgsqlCommand cmd = new NpgsqlCommand("select id, value from log", conn);

            // Execute a query
            NpgsqlDataReader dr = cmd.ExecuteReader();

            // Read all rows and output the first column in each row
            while (dr.Read())
                Console.Write("{0:D}, {1:D}\n", dr[0], dr[1]);

            // Close connection
            conn.Close();
        }
    }
}
