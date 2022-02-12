import { createServer } from 'http';

const hostname = '127.0.0.1';
const port = 3000;

const server = createServer((req, res) => {
  res.statusCode = 200;
  res.setHeader('Content-Type', 'text/plain');
  res.end('Hello, World!\n');
});

server.listen(port, hostname, () => {
  console.log(`Server running at http://${hostname}:${port}/`);
});



//var pg = require('pg');
//var connectionString = "postgres://usuario:@trabalho2/ip:25281/log";
const { Client } = require('pg')
const client = new Client()
await client.connect()
const { Client } = require('pg')

const query = async (connectionString) => {

  // create connection
  const connection = new Client(connectionString);
  connection.connect();

  // show tables in the postgres database
  const tables = await connection.query('SELECT * FROM log;');
  console.log(tables.rows);

  // close connection
  connection.end();
}

const server = 'trabalho2';
const user = 'usuario';
const password = '123465789';
const database = 'trabalho2';
const connectionString = `postgres://${user}@${server}:${password}@${server}.localhost/${database}`;

query(connectionString)
  .then(() => console.log('done'))
  .catch((err) => console.log(err));