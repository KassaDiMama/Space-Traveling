const Message = require("./Message");
const Mongo = require("mongodb");
const IsometricGrid = require("../Utils/IsometricGrid");
const Inventory = require("../Utils/Inventory");

class RemoveBuildingMessage extends Message {
    constructor() {
        super();
        this.command = "RemoveBuildingMessage";
        this.buildingData = null;
    }
    onReceive() {
        console.log("Base Requested by: " + this.socket.username);
        // var messageObject = new BaseInformation();
        // messageObject.baseData = "{}";
        // var messageString = messageObject.Serialize();
        // this.socket.write(messageString);
        var RemoveBuildingFunction = (resolve, reject) => {
            var db = this.socket.databaseQueu.db;
            var database = db.db("SpaceTravelGame")
            var searchObj = {
                username: this.socket.username
            }
            database.collection("UserData").findOne(searchObj, (err, result) => {
                //console.log(this.buildingData);
                var grid = IsometricGrid.Deserialize(result.baseData);
                console.log("Coords: (" + this.buildingData.x + "," + this.buildingData.y + ")")
                console.log(grid.grid[this.buildingData.x])
                var building = grid.grid[this.buildingData.x][this.buildingData.y].building;
                if (building.rocket == null) {
                    grid.removeBuilding(building);
                    var inventory = Inventory.Deserialize(result.inventory);
                    inventory.addItem(this.buildingData.type, 1, "Building");
                    var query = {
                        "username": this.socket.username
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

                        resolve(this.socket.databaseQueu);
                    })
                } else {
                    console.log("There is a rocket on this building");
                }

            })
        }
        this.socket.databaseQueu.addFunction(RemoveBuildingFunction);




    }
}

module.exports = RemoveBuildingMessage;