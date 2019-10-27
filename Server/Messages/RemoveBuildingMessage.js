const Message = require("./Message");
const Mongo = require("mongodb");
const IsometricGrid = require("../Utils/IsometricGrid");
const Inventory = require("../Utils/Inventory");

class RemoveBuildingMessage extends Message {
    constructor() {
        super();
        this.command = "RemoveBuildingMessage";
        this.username = null;
        this.buildingData = null;
    }
    onReceive() {
        console.log("Base Requested by: " + this.username);
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
                    //console.log(this.buildingData);
                    var grid = IsometricGrid.Deserialize(result.baseData);
                    var building = grid.grid[this.buildingData.x][this.buildingData.y].building;
                    grid.removeBuilding(building);
                    var inventory = Inventory.Deserialize(result.inventory);
                    inventory.addItem(this.buildingData.type, 1);
                    var query = {
                        "username": this.username
                    };
                    var update = {
                        $set: {
                            "inventory": inventory.Serialize(),
                            "baseData": grid.Serialize()
                        }
                    }
                    database.collection("UserData").update(query, update, (err, res) => {
                        if (err) throw err;
                        console.log("Removed building")
                        db.close();
                        resolve(this.socket.databaseQueu);
                    })
                })
            })
        }
        this.socket.databaseQueu.addFunction(queuFunc);




    }
}

module.exports = RemoveBuildingMessage;