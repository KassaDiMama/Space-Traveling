const Message = require("./Message");
const Building = require("../Utils/Building");
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
                if (this.fromX != -1 && this.fromY != -1) { //building just got placed from inventory
                    if (this.type)
                        var building = grid.grid[this.fromX][this.fromY].building;
                } else {
                    var building = new Building(this.toX, this.toY, this.toWidth, this.toLength);
                    building.type = this.buildingType;
                    grid.placeBuilding(building);
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
                database.collection("UserData").updateOne(query, changes, function (err, res) {
                    if (err) throw err;
                    console.log("approved and updated userdata DUUUDEEEE");
                    db.close();
                })
            })
        })
    }
}

module.exports = ChangeBuildingPositionMessage;