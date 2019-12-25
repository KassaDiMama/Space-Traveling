const Message = require("./Message");
const Building = require("../Utils/Building");
const Inventory = require("../Utils/Inventory");
const IsometricGrid = require("../Utils/IsometricGrid");
const Mongo = require("mongodb");
class ChangeBuildingPositionMessage extends Message {
    constructor() {
        super();
        this.command = "ChangeBuildingPositionMessage";
        this.fromX = null;
        this.fromY = null;
        this.toX = null;
        this.toY = null;

        this.toWidth = null;
        this.toLength = null;
        this.buildingType = null;

    }
    onReceive() {
        var ChangeBuildingPositionFunction = (resolve, reject) => {

            console.log(this.socket.username);
            var database = this.db.db("SpaceTravelGame")
            var searchObj = {
                username: this.socket.username
            }
            database.collection("UserData").findOne(searchObj, (err, result) => {
                if (err) throw err;
                var baseData = result.baseData;
                var grid = IsometricGrid.Deserialize(baseData);
                if (this.fromX != -1 && this.fromY != -1) {
                    var building = grid.grid[this.fromX][this.fromY].building;
                } else { //building just got placed from inventory
                    var inventory = Inventory.Deserialize(result.inventory)
                    // console.log(this.buildingType)
                    // console.log(inventory)
                    // console.log(inventory.itemPresent(this.buildingType));
                    if (inventory.itemPresent(this.buildingType)) {
                        inventory.removeItem(this.buildingType);
                        var building = new Building(this.toX, this.toY, this.toWidth, this.toLength);
                        building.type = this.buildingType;
                        grid.placeBuilding(building);
                        var query = {
                            username: "Kassa"
                        }
                        var update = {
                            $set: {
                                "inventory": inventory.Serialize()
                            }
                        }
                        database.collection("UserData").updateOne(query, update, (err, res) => {
                            if (err) throw err;
                            console.log("Updated inventory")
                        })
                    } else {
                        console.log("Item not in inventory");
                        resolve(this.socket.databaseQueu);
                        return null;
                    }

                }

                building.x = this.toX;
                building.y = this.toY;
                building.width = this.toWidth;
                building.height = this.toHeight;
                grid.updatePosition(building)
                var newBaseData = grid.Serialize();
                var query = {
                    username: this.socket.username
                };
                var changes = {
                    $set: {
                        "baseData": newBaseData
                    }
                }
                database.collection("UserData").updateOne(query, changes, (err, res) => {
                    if (err) throw err;
                    console.log("approved and updated userdata DUUUDEEEE");

                    resolve(this.socket.databaseQueu);
                })
            })

        }
        this.socket.databaseQueu.addFunction(ChangeBuildingPositionFunction);
    }
}

module.exports = ChangeBuildingPositionMessage;