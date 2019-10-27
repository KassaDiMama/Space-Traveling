const Message = require("./Message");
const Building = require("../Utils/Building");
const Inventory = require("../Utils/Inventory");
const IsometricGrid = require("../Utils/IsometricGrid");
const Mongo = require("mongodb");
class ChangeBuildingPositionMessage extends Message {
    constructor() {
        super();
        this.command = "ChangeBuildingPositionMessage";
        this.username = null;
        this.fromX = null;
        this.fromY = null;
        this.toX = null;
        this.toY = null;

        this.toWidth = null;
        this.toLength = null;
        this.buildingType = null;

    }
    onReceive() {
        var queuFunc = (resolve, reject) => {
            var MongoClient = Mongo.MongoClient;
            MongoClient.connect("mongodb://localhost:27017/", (err, db) => {
                if (err) throw err;
                console.log(this.username);
                var database = db.db("SpaceTravelGame")
                var searchObj = {
                    username: this.username
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
                            db.close();
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
                        username: this.username
                    };
                    var changes = {
                        $set: {
                            "baseData": newBaseData
                        }
                    }
                    database.collection("UserData").updateOne(query, changes, (err, res) => {
                        if (err) throw err;
                        console.log("approved and updated userdata DUUUDEEEE");
                        db.close();
                        resolve(this.socket.databaseQueu);
                    })
                })
            })
        }
        this.socket.databaseQueu.addFunction(queuFunc);
    }
}

module.exports = ChangeBuildingPositionMessage;