class Message {
    constructor() {
        this.command = "Message";
    }
    Serialize() {
        return JSON.stringify(this);
    }
    static Deserialize(jsonString) {
        var parsedJson = JSON.parse(jsonString);
        var messageClass = require("./" + parsedJson.command);
        var message = new messageClass();
        Object.keys(parsedJson).forEach(key => {
            message[key] = parsedJson[key]
        });
        return message;
    }
    onReceive() {

    }
}
module.exports = Message;