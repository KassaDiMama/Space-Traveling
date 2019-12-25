const Message = require("./Message");
const IsometricGrid = require("../Utils/IsometricGrid")
const Rocket = require("../Utils/Rocket")
const Inventory = require("../Utils/Inventory");
class LoginMessage extends Message {
    constructor() {
        super();
        this.command = "LoginMessage";
        this.username
        this.password = null;
    }
    onReceive() {
        var LoginFunction = (resolve, reject) => {

            console.log(this.username);
            var database = this.db.db("SpaceTravelGame")
            var searchObj = {
                username: this.username
            }

            database.collection("UserData").findOne(searchObj, (err, result) => {
                if (err) {
                    console.log("No such user")
                    resolve()
                }
                var password = result.password;
                if (password = this.password) {
                    this.socket.key = getRandomKey();
                    //send back key TODO
                }

            })

        }
        this.socket.databaseQueu.addFunction(LoginFunction);
    }
    getRandomKey() {
        var alphabet = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890"
        var length = 15;
        var key = "";
        for (let index = 0; index < length; index++) {
            var randomChar = alphabet[Math.floor(Math.random() * alphabet.length)];
            key.padEnd(randomChar);
        }
        return key;
    }
}

module.exports = LoginMessage;