const Message = require("./Message");
const InventoryInformationMessage = require("./InventoryInformationMessage");
const Mongo = require("mongodb");
const FriendsList = require("../Utils/FriendsList");
const Friend = require("../Utils/Friend");
const AcceptFriendMessage = require("./AcceptFriendMessage")

class AddFriendMessage extends Message {
    constructor() {
        super();
        this.command = "AddFriendMessage";
        this.friendName = "";
    }
    onReceive() {
        console.log("Friend request sent by: " + this.socket.username);
        // var messageObject = new BaseInformation();
        // messageObject.baseData = "{}";
        // var messageString = messageObject.Serialize();
        // this.socket.write(messageString);
        var AddFriendFunction = (resolve, reject) => {
            var db = this.socket.databaseQueu.db;
            var database = db.db("SpaceTravelGame")
            var searchObj = {
                username: this.friendName
            }
            console.log("Looking for friend: " + this.friendName);
            database.collection("UserData").findOne(searchObj, (err, result) => {
                if (result != null) {
                    console.log("friend found")
                    searchObj = {
                        username: this.socket.username
                    }
                    database.collection("UserData").findOne(searchObj, (err, result2) => {
                        var friendsList = FriendsList.Deserialize(result2.friendsList);
                        var friend = new Friend(this.friendName);
                        friendsList.addFriend(friend);
                        var query = {
                            "username": this.socket.username
                        };
                        var update = {
                            $set: {
                                "friendsList": friendsList.Serialize()
                            }
                        }
                        database.collection("UserData").updateOne(query, update, (err, res) => {
                            var message = new AcceptFriendMessage();
                            message.friendsList = friendsList.Serialize()
                            this.socket.write(message.Serialize());
                            console.log("Added Friend")
                        })
                    })
                }


                resolve(this.socket.databaseQueu)
            })
        }
        this.socket.databaseQueu.addFunction(AddFriendFunction);




    }
}

module.exports = AddFriendMessage;