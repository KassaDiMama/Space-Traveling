const Message = require("./Message");
class StartInformationMessage extends Message {
    constructor() {
        super();
        this.command = "StartInformationMessage";
        this.baseData = "";
        this.inventoryData = "";
        this.friendsList = "";
        this.outgoingRockets = "";
    }
}

module.exports = StartInformationMessage;