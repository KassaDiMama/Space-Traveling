class Message {
    constructor() {
        this.command = "Message";
        this.socket = null;
        this.db = null;
        this.key = null;
    }
    Serialize() {
        return JSON.stringify(this);
    }
    static Deserialize(jsonString) {
        try {
            var parsedJson = JSON.parse(jsonString);
            var messageClass = require("./" + parsedJson.command);
            var message = new messageClass();
            Object.keys(parsedJson).forEach(key => {
                message[key] = parsedJson[key]
            });
            return message;
        } catch (error) {
            console.log(error)
            console.warn("No such message: " + parsedJson.command);
        }

    }
    onReceive() {

    }
}
module.exports = Message;