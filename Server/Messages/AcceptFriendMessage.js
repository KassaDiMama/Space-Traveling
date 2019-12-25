const Message = require("./Message");
class AcceptFriendMessage extends Message {
    constructor() {
        super();
        this.command = "AcceptFriendMessage";
        this.friendsList = {};
    }
}

module.exports = AcceptFriendMessage;