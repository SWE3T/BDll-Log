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
            readFile();
            showTable();
        }

        private static void readFile()
        {
            NpgsqlConnection con = new NpgsqlConnection(
                "Host=localhost; Username=usuario; Password=123456789; Database=trabalho2"
            );

            con.Open();
            
            string filename = @"entradaLog.txt";
            
            var lines = File.ReadAllLines(filename);
            
            // read each line from file
            foreach (var line in lines)
            {
                Console.WriteLine(line);
                if (line.Contains("="))
                {
                    Console.WriteLine("A linha contém '=';");

                    string[] result = line.Split(',', '=');
                    // Console.WriteLine(result[0]);
                    // Console.WriteLine(result[1]);
                    // Console.WriteLine(result[2]);
                    //id A B
                    var sql =
                        "INSERT INTO log (id, " +
                        '"' + result[0] + '"' +") VALUES ('"
                        + result[1] + "', '" + result[2]
                        + "') ON CONFLICT (id) DO UPDATE SET " 
                        +'"' + result[0] + '"' + " =  " + result[2] + ";";
                    Console.WriteLine(sql);

                    using var cmd = new NpgsqlCommand(sql, con);
                    cmd.ExecuteNonQuery();
                    //INSERT INTO
                    //   log (id, "A")
                    // VALUES
                    //   ('1', '20') 
                    // ON CONFLICT (id) DO UPDATE 
                    //   SET "A" = 20;

                }
            }
            // read target line directly
            Console.WriteLine(targetLine);
        }

        private static void showTable()
        {
            NpgsqlConnection conn = new NpgsqlConnection(
                "Host=localhost;Username=usuario;Password=123456789;Database=trabalho2"
            );
            conn.Open();

            NpgsqlCommand cmd = new NpgsqlCommand("select * from log", conn);

            NpgsqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
                Console.Write("ID: {0:D}, A: {1:D}, B: {2:D}\n", dr[0], dr[1], dr[2]);

            conn.Close();
        }
    }
}
