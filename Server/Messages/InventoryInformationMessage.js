const Message = require("./Message");
const IsometricGrid = require("../Utils/IsometricGrid")
class InventoryInformationMessage extends Message {
    constructor() {
        super();
        this.command = "InventoryInformationMessage";
        this.inventoryData = "";
    }

}

module.exports = InventoryInformationMessage;