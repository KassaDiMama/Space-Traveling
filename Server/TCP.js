const net = require('net');
const port = 33333;
const host = '0.0.0.0';
const messages = require("./Messages");
const DatabaseQueue = require("./Utils/DatabaseQueu");

const server = net.createServer();
server.listen(port, host, () => {
    console.log('TCP Server is running on port ' + port + '.');
});

let users = [];
var dataBaseClient;
var MongoClient = require('mongodb').MongoClient
MongoClient.connect("mongodb://localhost:27017/", (err, db) => {
    dataBaseClient = db;
    server.on('connection', function (sock) {


        console.log('CONNECTED: ' + sock.remoteAddress + ':' + sock.remotePort);
        users.push(sock);
        sock.databaseQueu = new DatabaseQueue(dataBaseClient);

        sock.on('data', function (data) {
            //console.log('DATA ' + sock.remoteAddress + ': ' + data);
            // Write the data back to all the connected, the client will receive it as data from the server
            var messageObject = messages.Message.Deserialize(data);
            if (messageObject) {
                console.log(data.toString())
                console.log(messageObject);
                messageObject.socket = sock;
                messageObject.db = sock.databaseQueu.db;
                if (sock.key == messageObject.key || sock.key == null) {
                    messageObject.onReceive();
                }

            } else {
                console.warn("Failed to deserialize message object: " + data);
            }

        });
        sock.on("close", function () {
            console.log('DISCONNECTED: ' + sock.remoteAddress + ':' + sock.remotePort);
            var id = sock.remoteAddress + ':' + sock.remotePort;
            users.splice(users.indexOf(sock))
        })
    });
})