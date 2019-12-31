const Message = require("./Message");
const FriendBaseDataMessage = require("./FriendBaseDataMessage")

class RequestFriendsBaseMessage extends Message {
    constructor() {
        super();
        this.command = "RequestFriendsBaseMessage";
        this.friendName = "";
    }
    onReceive() {
        var RequestFriendsBaseFunction = (resolve, reject) => {
            try {
                var database = this.db.db("SpaceTravelGame")
                var searchObj = {
                    username: this.friendName
                }

                database.collection("UserData").findOne(searchObj, (err, result) => {
                    if (err) {
                        console.log("No such user")
                        resolve()
                    }
                    if (result != null) {
                        //registers user
                        var baseData = result.baseData;
                        var message = new FriendBaseDataMessage();
                        message.baseData = baseData;
                        this.socket.write(message.Serialize());
                        console.log("Send friend base")
                        resolve(this.socket.databaseQueu);



                    } else {
                        console.log("No such user");
                        resolve(this.socket.databaseQueu);
                    }


                })
            } catch (e) {
                console.error(e);
                resolve(this.socket.databaseQueu)
            }


        }
        this.socket.databaseQueu.addFunction(RequestFriendsBaseFunction);
    }
}

module.exports = RequestFriendsBaseMessage;