var RocketTransaction = require("./RocketTransaction");
class OutgoingRockets {
    constructor() {
        this.transactions = [];
    }
    addTransaction(transaction) {
        this.transactions.push(transaction);
    }
    Serialize() {
        return JSON.stringify(this);
    }
    static Deserialize(jsonString) {
        var parsedJson = JSON.parse(jsonString);
        var outgoingRockets = new OutgoingRockets();
        parsedJson.transactions.forEach(transaction => {
            var newTransaction = new RocketTransaction();
            Object.keys(transaction).forEach(key => {
                newTransaction[key] = transaction[key]
            });
            outgoingRockets.transactions.push(newTransaction);
        });
        return outgoingRockets;
    }
}

module.exports = OutgoingRockets;