const Message = require("./Message");
const IsometricGrid = require("../Utils/IsometricGrid")
class BaseInformation extends Message {
    constructor() {
        super();
        this.command = "BaseInformation";
        this.baseData = "";
    }
    onReceive() {
        console.log(this.baseData)
        console.log(IsometricGrid.Deserialize(this.baseData));
    }
}

module.exports = BaseInformation;