class Destination {
    constructor() {
        this.toPlanet = false;
        this.toPlayer = false;
        this.planetName = null;
        this.playerName = null;
    }
    static Deserialize(jsonString) {
        var newDestination = new Destination();
        var parsedJson = JSON.parse(jsonString);
        Object.keys(parsedJson).forEach(key => {
            newDestination[key] = parsedJson[key]
        });
        return newDestination;
    }
}
module.exports = Destination