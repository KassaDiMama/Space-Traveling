const Message = require("./Message");
const InventoryInformationMessage = require("./InventoryInformationMessage");
const Mongo = require("mongodb");

class RequestInventoryMessage extends Message {
    constructor() {
        super();
        this.command = "RequestInventoryMessage";
        this.username = "";
    }
    onReceive() {
        console.log("Inventory Requested by: " + this.username);
        // var messageObject = new BaseInformation();
        // messageObject.baseData = "{}";
        // var messageString = messageObject.Serialize();
        // this.socket.write(messageString);
        var queuFunc = (resolve, reject) => {
            var MongoClient = Mongo.MongoClient;
            MongoClient.connect("mongodb://localhost:27017/", (err, db) => {
                if (err) throw err;
                var database = db.db("SpaceTravelGame")
                var searchObj = {
                    username: this.username
                }
                database.collection("UserData").findOne(searchObj, (err, result) => {
                    var messageObject = new InventoryInformationMessage(); //{\"width\":20,\"length\":10,\"buildings\":[{\"x\":3,\"y\":3,\"width\":2,\"height\":3,\"type\":\"Ground3x2\"},{\"x\":4,\"y\":0,\"width\":2,\"height\":3,\"type":"Ground3x2"}]}
                    //messageObject.baseData = "{\"width\":20,\"length\":10,\"buildings\":[{\"x\":3,\"y\":3,\"width\":2,\"height\":3,\"type\":\"Ground3x2\"},{\"x\":4,\"y\":0,\"width\":2,\"height\":3,\"type\":\"Ground3x2\"}]}";
                    messageObject.inventoryData = result.inventory;
                    var messageString = messageObject.Serialize();
                    this.socket.write(messageString);
                    db.close();
                    resolve(this.socket.databaseQueu)
                    console.log("resolved inventory request")
                })
            })
        }
        this.socket.databaseQueu.addFunction(queuFunc);




    }
}

module.exports = RequestInventoryMessage;