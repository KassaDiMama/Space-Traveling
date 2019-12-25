const Message = require("./Message");
const IsometricGrid = require("../Utils/IsometricGrid")
class KeyMessage extends Message {
    constructor() {
        super();
        this.command = "KeyMessage";
        this.key = "";
    }

}

module.exports = KeyMessage;