class Building {
    constructor(x, y, width, height) {
        this.type = "Building";
        this.x = x;
        this.y = y;
        this.width = width;
        this.height = height;
        this.grid = null;
        this.usingTiles = [];
        this.rocket = null;
    }
    placeRocket(rocket) {
        if (this.rocket == null) {
            this.rocket = rocket;
        } else {
            console.warn("Can't place rocket here.")
        }
    }
}

module.exports = Building;