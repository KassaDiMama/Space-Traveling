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

server.on('connection', function (sock) {
    console.log('CONNECTED: ' + sock.remoteAddress + ':' + sock.remotePort);
    users.push(sock);
    sock.databaseQueu = new DatabaseQueue();

    sock.on('data', function (data) {
        //console.log('DATA ' + sock.remoteAddress + ': ' + data);
        // Write the data back to all the connected, the client will receive it as data from the server
        var messageObject = messages.Message.Deserialize(data);
        messageObject.socket = sock;
        messageObject.onReceive();
    });
    sock.on("close", function () {
        console.log('DISCONNECTED: ' + sock.remoteAddress + ':' + sock.remotePort);
        var id = sock.remoteAddress + ':' + sock.remotePort;
        users.splice(users.indexOf(sock))
    })
});