const Message = require("./Message");
const IsometricGrid = require("../Utils/IsometricGrid")
const Rocket = require("../Utils/Rocket")
const Inventory = require("../Utils/Inventory");
class AddRocketOnBuildingMessage extends Message {
    constructor() {
        super();
        this.command = "BaseInformationMessage";
        this.buildingX = null;
        this.buildingY = null;
        this.type = null;
        this.key = null;
        this.rocketKey = null;
    }
    onReceive() {
        var AddRocketOnBuildingFunction = (resolve, reject) => {
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
                    searchObj = {
                        type: building.type
                    }
                    database.collection("Buildings").findOne(searchObj, (err, result2) => {
                        if (result2.isRocketHolder) {
                            var rocket = new Rocket(this.type, this.rocketKey, this.destination);
                            var inventory = Inventory.Deserialize(result.inventory);
                            if (inventory.itemPresent(rocket.type)) {
                                inventory.removeItem(rocket.type);
                                building.placeRocket(rocket);
                                var query = {
                                    username: this.socket.username
                                };
                                var changes = {
                                    $set: {
                                        "inventory": inventory.Serialize(),
                                        "baseData": grid.Serialize()
                                    }
                                }
                                database.collection("UserData").updateOne(query, changes, (err, result3) => {
                                    if (err) throw err;
                                    console.log("Added rocket");

                                    resolve(this.socket.databaseQueu);
                                })
                            } else {
                                console.warn("This rocket not in yo inventory");
                            }

                        }
                    })

                }


            })

        }
        this.socket.databaseQueu.addFunction(AddRocketOnBuildingFunction);
    }
}

module.exports = AddRocketOnBuildingMessage;