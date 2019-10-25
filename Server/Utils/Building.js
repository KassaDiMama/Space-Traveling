class Building {
    constructor(x, y, width, height) {
        this.type = "Building";
        this.x = x;
        this.y = y;
        this.width = width;
        this.height = height;
        this.grid = null;
        this.usingTiles = [];
    }
}

module.exports = Building;