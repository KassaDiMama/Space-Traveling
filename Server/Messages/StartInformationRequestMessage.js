const Message = require("./Message");
const StartInformationMessage = require("./StartInformationMessage");
const Mongo = require("mongodb");

class StartInformationRequestMessage extends Message {
    constructor() {
        super();
        this.command = "StartInformationRequestMessage";

    }
    onReceive() {
        console.log("Base Requested by: " + this.socket.username);
        // var messageObject = new BaseInformation();
        // messageObject.baseData = "{}";
        // var messageString = messageObject.Serialize();
        // this.socket.write(messageString);
        var StartInformationRequestMessageFunction = (resolve, reject) => {
            try {
                var db = this.socket.databaseQueu.db;
                var database = db.db("SpaceTravelGame")
                var searchObj = {
                    username: this.socket.username
                }
                database.collection("UserData").findOne(searchObj, (err, result) => {
                    if (err) throw err
                    var messageObject = new StartInformationMessage();
                    messageObject.baseData = result.baseData;
                    messageObject.inventoryData = result.inventory;
                    messageObject.friendsList = result.friendsList;
                    messageObject.outgoingRockets = result.outgoingRockets;
                    var messageString = messageObject.Serialize();
                    this.socket.write(messageString);
                    resolve(this.socket.databaseQueu);
                    console.log("resolved start information request")
                })
            } catch (e) {
                console.error(e);
                resolve(this.socket.databaseQueu)
            }



        }
        this.socket.databaseQueu.addFunction(StartInformationRequestMessageFunction);



    }
}


module.exports = StartInformationRequestMessage;