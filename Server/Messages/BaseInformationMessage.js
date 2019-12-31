const Message = require("./Message");
const IsometricGrid = require("../Utils/IsometricGrid")
class BaseInformationMessage extends Message {
    constructor() {
        super();
        this.command = "BaseInformationMessage";
        this.baseData = "";
    }
    onReceive() {
        var InformationFunction = (resolve, reject) => {
            try {
                //console.log(this.baseData)
                var messageObject = new BaseInformationMessage();
                messageObject.baseData = "{\"width\":20,\"length\":10,\"buildings\":[{\"x\":3,\"y\":3,\"width\":2,\"height\":3,\"type\":\"Ground3x2\"},{\"x\":4,\"y\":0,\"width\":2,\"height\":3,\"type\":\"Ground3x2\"}]}";
                var messageString = messageObject.Serialize();
                this.socket.write(messageString);
                resolve(this.socket.databaseQueu)
            } catch (e) {
                console.error(e);
                resolve(this.socket.databaseQueu)
            }


        }
        this.socket.databaseQueu.addFunction(InformationFunction);
    }
}

module.exports = BaseInformationMessage;