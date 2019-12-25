const Message = require("./Message");
const InventoryInformationMessage = require("./InventoryInformationMessage");
const Mongo = require("mongodb");

class RequestInventoryMessage extends Message {
    constructor() {
        super();
        this.command = "RequestInventoryMessage";
    }
    onReceive() {
        console.log("Inventory Requested by: " + this.socket.username);
        // var messageObject = new BaseInformation();
        // messageObject.baseData = "{}";
        // var messageString = messageObject.Serialize();
        // this.socket.write(messageString);
        var RequestInventoryFunction = (resolve, reject) => {
            var db = this.socket.databaseQueu.db;
            var database = db.db("SpaceTravelGame")
            var searchObj = {
                username: this.socket.username
            }
            database.collection("UserData").findOne(searchObj, (err, result) => {
                var messageObject = new InventoryInformationMessage(); //{\"width\":20,\"length\":10,\"buildings\":[{\"x\":3,\"y\":3,\"width\":2,\"height\":3,\"type\":\"Ground3x2\"},{\"x\":4,\"y\":0,\"width\":2,\"height\":3,\"type":"Ground3x2"}]}
                //messageObject.baseData = "{\"width\":20,\"length\":10,\"buildings\":[{\"x\":3,\"y\":3,\"width\":2,\"height\":3,\"type\":\"Ground3x2\"},{\"x\":4,\"y\":0,\"width\":2,\"height\":3,\"type\":\"Ground3x2\"}]}";
                messageObject.inventoryData = result.inventory;
                var messageString = messageObject.Serialize();
                this.socket.write(messageString);

                resolve(this.socket.databaseQueu)
                console.log("resolved inventory request")
            })
        }
        this.socket.databaseQueu.addFunction(RequestInventoryFunction);




    }
}

module.exports = RequestInventoryMessage;