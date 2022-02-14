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
            //showTable();
        }

        private static void readFile()
        {
            NpgsqlConnection con = new NpgsqlConnection(
                "Host=localhost; Username=usuario; Password=123456789; Database=trabalho2"
            );

            con.Open();

            string filename = @"entradaLog.txt";

            var lines = File.ReadAllLines(filename);

            int i = 0;

            foreach (var line in lines)
            {
                lines[i] = lines[i].Replace(">", "");
                lines[i] = lines[i].Replace("<", "");
                //Console.WriteLine(lines[i]);
                i++;

                //Console.WriteLine(line);
                if (line.Contains("=")) //atribuição
                {
                    string[] result = line.Split(',', '=');

                    var sql =
                        "INSERT INTO log (id, "
                        + '"'
                        + result[0]
                        + '"'
                        + ") VALUES ('"
                        + result[1]
                        + "', '"
                        + result[2]
                        + "') ON CONFLICT (id) DO UPDATE SET "
                        + '"'
                        + result[0]
                        + '"'
                        + " =  "
                        + result[2]
                        + ";";
                    //Console.WriteLine(sql);

                    using var cmd = new NpgsqlCommand(sql, con);
                    cmd.ExecuteNonQuery();
                }
            }

            int indexCKPT = 0;
            String[] transAbertas = new string[7];

            for (var a = lines.Length - 1; a > 0; a--) //acha o ult check
            {
                if (lines[a].Contains("Start CKPT"))
                {
                    lines[a] = lines[a].Replace("Start CKPT", "");
                    lines[a] = lines[a].Replace("(", "");
                    lines[a] = lines[a].Replace(")", "");
                    transAbertas = lines[a].Split(',');
                    indexCKPT = a + 1;
                    break;
                }
            }

            Console.WriteLine("linha do CKPT: {0:D}", indexCKPT);
            //Console.WriteLine(transAbertas[0]);

            for (var t = 0; t < transAbertas.Length; t++)
            {
                transAbertas[t] = transAbertas[t] + " NÃO sofreu REDO.";
            }

            for (var b = indexCKPT - 1; b < lines.Length; b++) //checa os commits <commit T4>
            {
                if (lines[b].Contains("commit"))
                {
                    string[] commit = lines[b].Split(" ");
                    string value = commit[1] + " NÃO sofreu REDO.";

                    int index = Array.IndexOf(transAbertas, value);
                    if (index > -1)
                    {
                        transAbertas[index] = commit[1] + " sofreu REDO.";
                    }

                    // Console.WriteLine("A transação " + commit[1] + " sofreu REDO.");
                    //refaz o caminho

                }
            }
            //refaz se precisar, se não segue pro prox
            Console.WriteLine("Valores iniciais: ");
            showTable();
            foreach (var item in transAbertas)
            {
                Console.WriteLine(item);
            }
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
//verifica se tem end ->
//o que está em aberto com commit o que foi aberto e commitado durante o start end deve ser refeito
