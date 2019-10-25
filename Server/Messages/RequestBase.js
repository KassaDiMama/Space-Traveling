const Message = require("./Message");
const BaseInformation = require("./BaseInformation");

class RequestBase extends Message {
    constructor() {
        super();
        this.command = "RequestBase";
        this.username = "";
    }
    onReceive() {
        console.log("Base Requested by: " + this.username);
        var messageObject = new BaseInformation();
        messageObject.baseData = "TEST DUDEEE";
        var messageString = messageObject.Serialize();
        this.socket.write(messageString);
    }
}

module.exports = RequestBase;