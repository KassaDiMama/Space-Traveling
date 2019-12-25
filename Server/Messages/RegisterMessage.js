const Message = require("./Message");
const IsometricGrid = require("../Utils/IsometricGrid")
const Rocket = require("../Utils/Rocket")
const Inventory = require("../Utils/Inventory");
const KeyMessage = require("../Messages/KeyMessage");
const FriendsList = require("../Utils/FriendsList");

const starterBaseData = "{\"width\":20,\"length\":10,\"buildings\":[{\"x\":14,\"y\":6,\"width\":2,\"height\":3,\"type\":\"Ground3x2\"},{\"x\":4,\"y\":3,\"width\":2,\"height\":3,\"type\":\"Ground3x2\"},{\"x\":5,\"y\":7,\"width\":2,\"height\":3,\"type\":\"TestStation\",\"rocket\":{\"type\":\"Rocket1\"}},{\"x\":11,\"y\":3,\"width\":2,\"height\":3,\"type\":\"TestStation\",\"rocket\":{\"type\":\"Rocket1\"}},{\"x\":15,\"y\":0,\"width\":2,\"height\":3,\"type\":\"TestStation\",\"rocket\":{\"type\":\"Rocket1\"}},{\"x\":1,\"y\":2,\"width\":2,\"height\":3,\"type\":\"TestStation\",\"rocket\":{\"type\":\"Rocket1\"}}]}"

class RegisterMessage extends Message {
    constructor() {
        super();
        this.command = "RegisterMessage";
        this.username = null;
        this.password = null;
    }
    onReceive() {
        var RegisterFunction = (resolve, reject) => {

            console.log("username: " + this.username);
            var database = this.db.db("SpaceTravelGame")
            var searchObj = {
                username: this.username
            }

            database.collection("UserData").findOne(searchObj, (err, result) => {
                if (err) {
                    console.log("No such user")
                    resolve()
                }
                console.log(result);
                if (result == null) {
                    //registers user
                    var newInventory = new Inventory();
                    var newFriendsList = new FriendsList();
                    var newUserData = {
                        "username": this.username,
                        "password": this.password,
                        "baseData": starterBaseData,
                        "inventory": newInventory.Serialize(),
                        "friendsList": newFriendsList.Serialize()
                    }
                    database.collection("UserData").insertOne(newUserData, (err, res) => {
                        if (err) throw err;
                        console.log("WOOHOO USER ADDED");
                        this.socket.key = this.getRandomKey();
                        console.log(this.socket.key)
                        this.socket.username = this.username;
                        var keyMessage = new KeyMessage();
                        keyMessage.key = this.socket.key;
                        this.socket.write(keyMessage.Serialize());
                        console.log("resolve")
                        resolve(this.socket.databaseQueu);
                    });



                } else {
                    console.log("yo this guy already exists yoooo");
                }


            })

        }
        this.socket.databaseQueu.addFunction(RegisterFunction);
    }
    getRandomKey() {
        var alphabet = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890"
        var length = 15;
        var key = "";
        for (let index = 0; index < length; index++) {
            var randomChar = alphabet[Math.floor(Math.random() * alphabet.length)];
            key += randomChar;
        }

        return key;
    }
}

module.exports = RegisterMessage;