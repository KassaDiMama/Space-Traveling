const Message = require("./Message");
const Mongo = require("mongodb");
const IsometricGrid = require("../Utils/IsometricGrid");
const Inventory = require("../Utils/Inventory");
const RocketTransaction = require("../Utils/RocketTransaction");
const OutgoingRockets = require("../Utils/OutgoingRockets");
const Destination = require("../Utils/Destination");

class SendRocketMessage extends Message {
    constructor() {
        super();
        this.command = "SendRocketMessage";
        this.rocketKey = null;
    }
    onReceive() {

        var SendRocketFucntion = (resolve, reject) => {
            var db = this.socket.databaseQueu.db;
            var database = db.db("SpaceTravelGame")
            var searchObj = {
                username: this.socket.username
            }
            database.collection("UserData").findOne(searchObj, (err, result) => {
                //console.log(this.buildingData);
                var grid = IsometricGrid.Deserialize(result.baseData);
                var building = grid.getBuildingByRocketKey(this.rocketKey);
                if (building != null) {
                    if (building.rocket != null) {
                        var rocket = building.rocket;
                        var destination = Destination.Deserialize(rocket.destination);
                        var rocketTransaction = new RocketTransaction(rocket.type, this.socket.username, destination, new Date().getMilliseconds())
                        var outgoingRockets = OutgoingRockets.Deserialize(result.outgoingRockets);
                        outgoingRockets.addTransaction(rocketTransaction);
                        building.rocket = null;
                        var query = {
                            "username": this.socket.username
                        };
                        var update = {
                            $set: {
                                "baseData": grid.Serialize(),
                                "outgoingRockets": outgoingRockets.Serialize()
                            }
                        }
                        database.collection("UserData").update(query, update, (err, res) => {
                            if (err) throw err;
                            console.log("Rocket sent!")

                            resolve(this.socket.databaseQueu);
                        })
                    }
                } else {
                    console.log("Building of rocket doesn't exist so can't send");
                    resolve(this.socket.databaseQueu);
                }


            })
        }
        this.socket.databaseQueu.addFunction(SendRocketFucntion);




    }
}

module.exports = SendRocketMessage;