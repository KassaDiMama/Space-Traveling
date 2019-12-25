const Message = require("./Message");
const IsometricGrid = require("../Utils/IsometricGrid")
const Rocket = require("../Utils/Rocket")
const Inventory = require("../Utils/Inventory");
class RemoveRocketOnBuildingMessage extends Message {
    constructor() {
        super();
        this.command = "BaseInformationMessage";
        this.buildingX = null;
        this.buildingY = null;
        this.type = null;
    }
    onReceive() {
        var RemoveRocketOnBuildingFunction = (resolve, reject) => {

            console.log(this.socket.username);
            var database = this.db.db("SpaceTravelGame")
            var searchObj = {
                username: this.socket.username
            }

            database.collection("UserData").findOne(searchObj, (err, result) => {
                if (err) throw err;
                var baseData = result.baseData;
                var grid = IsometricGrid.Deserialize(baseData);
                if (this.buildingX != -1 && this.buildingY != -1) {
                    var building = grid.grid[this.buildingX][this.buildingY].building;

                    if (building.rocket != null) {
                        var inventory = Inventory.Deserialize(result.inventory);
                        inventory.addItem(building.rocket.type, 1, "Rocket");
                        building.rocket = null;
                        var query = {
                            username: this.socket.username
                        };
                        var update = {
                            $set: {
                                "inventory": inventory.Serialize(),
                                "baseData": grid.Serialize()
                            }
                        }
                        database.collection("UserData").updateOne(query, update, (err, res) => {
                            if (err) throw err;
                            console.log("Removed rocket");

                            resolve(this.socket.databaseQueu);
                        })
                    } else {
                        resolve()
                    }




                } else {
                    resolve()
                }


            })

        }
        this.socket.databaseQueu.addFunction(RemoveRocketOnBuildingFunction);
    }
}

module.exports = RemoveRocketOnBuildingMessage;