class RocketTransaction {
    constructor(rocketType, senderName, destination, time) {
        this.rocketType = rocketType;
        this.destination = senderName;
        this.destination = destination;
        this.time = time;
        this.landed = false;
        this.canLand = false;
    }
}

module.exports = RocketTransaction;