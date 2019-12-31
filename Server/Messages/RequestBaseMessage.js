const Message = require("./Message");
const BaseInformationMessage = require("./BaseInformationMessage");
const Mongo = require("mongodb");

class RequestBaseMessage extends Message {
    constructor() {
        super();
        this.command = "RequestBaseMessage";

    }
    onReceive() {

        console.log("dis far")
        console.log("Base Requested by: " + this.socket.username);
        // var messageObject = new BaseInformation();
        // messageObject.baseData = "{}";
        // var messageString = messageObject.Serialize();
        // this.socket.write(messageString);
        console.log("dis far")
        var RequestBaseFunction = (resolve, reject) => {
            try {
                console.log("dis far")
                var db = this.socket.databaseQueu.db;
                console.log("dis far")
                var database = db.db("SpaceTravelGame")
                var searchObj = {
                    username: this.socket.username
                }
                console.log("did you get this far tho??")
                database.collection("UserData").findOne(searchObj, (err, result) => {
                    if (err) throw err
                    console.log("if this worked scream plz")
                    var messageObject = new BaseInformationMessage(); //{\"width\":20,\"length\":10,\"buildings\":[{\"x\":3,\"y\":3,\"width\":2,\"height\":3,\"type\":\"Ground3x2\"},{\"x\":4,\"y\":0,\"width\":2,\"height\":3,\"type":"Ground3x2"}]}
                    //messageObject.baseData = "{\"width\":20,\"length\":10,\"buildings\":[{\"x\":3,\"y\":3,\"width\":2,\"height\":3,\"type\":\"Ground3x2\"},{\"x\":4,\"y\":0,\"width\":2,\"height\":3,\"type\":\"Ground3x2\"}]}";
                    messageObject.baseData = result.baseData;
                    var messageString = messageObject.Serialize();
                    this.socket.write(messageString);
                    console.log("test2")

                    console.log("test")
                    resolve(this.socket.databaseQueu);
                    console.log("resolved base request")
                })
            } catch (e) {
                console.error(e);
                resolve(this.socket.databaseQueu)
            }



        }
        this.socket.databaseQueu.addFunction(RequestBaseFunction);



    }
}


module.exports = RequestBaseMessage;