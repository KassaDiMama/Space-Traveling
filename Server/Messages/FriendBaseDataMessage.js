const Message = require("./Message");
class FriendBaseDataMessage extends Message {
    constructor() {
        super();
        this.command = "FriendBaseDataMessage";
        this.baseData = "";
    }
}

module.exports = FriendBaseDataMessage;