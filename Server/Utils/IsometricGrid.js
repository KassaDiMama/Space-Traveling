const IsometricTile = require("./IsometricTile");
const Building = require("./Building");
class IsometricGrid {
    constructor(width, length) {
        this.width = width;
        this.length = length;
        this.grid = [];
        this.buildings = [];
        for (let x = 0; x < width; x++) {
            var row = [];
            for (let y = 0; y < length; y++) {
                row.push(new IsometricTile(x, y));

            }
            this.grid.push(row);

        }

    }
    isOnBoard(building) {
        for (let x = building.x; x < building.x + building.width; x++) {
            for (let y = building.y; y < building.y + building.height; y++) {
                if (x < 0 || this.width <= x || 0 > y || y >= this.length) {
                    return false;
                }
            }
        }
        return true;
    }
    isOnBuilding(building) {
        for (let x = building.x; x < building.x + building.width; x++) {
            for (let y = building.y; y < building.y + building.height; y++) {
                if (this.grid[x][y].building != null && this.grid[x][y].building != building) {
                    return true;
                }
            }
        }
        return false;
    }
    placeBuilding(building) {
        if (this.isOnBoard(building) && !this.isOnBuilding(building)) {
            //console.log("Possible change");
            this.buildings.push(building);
            building.grid = this;
            this.updatePosition(building);
        } else {
            console.log("Nah man this dude is hacking like crazy yo");
        }
    }
    updatePosition(building) {
        building.usingTiles.forEach(tile => {
            tile.building = null;
        });
        building.usingTiles = [];
        for (let x = building.x; x < building.x + building.width; x++) {
            for (let y = building.y; y < building.y + building.height; y++) {
                this.grid[x][y].building = building;
                building.usingTiles.push(this.grid[x][y])
            }
        }
    }
    removeBuilding(building) {

        building.usingTiles.forEach(tile => {
            tile.building = null;
        });
        building.usingTiles = [];
        var index = this.buildings.findIndex((b) => {
            return ((b.x == building.x) && (b.y == building.y))
        });
        this.buildings.splice(index, 1);
        //console.log(index)
    }
    Serialize() {
        var dict = {};
        dict.width = this.width;
        dict.length = this.length;
        dict.buildings = []
        this.buildings.forEach(building => {
            var buildingDict = {};
            buildingDict.x = building.x;
            buildingDict.y = building.y;
            buildingDict.width = building.width;
            buildingDict.height = building.height;
            buildingDict.type = building.type;
            dict.buildings.push(buildingDict);
        });
        // console.log(dict);
        // console.log(JSON.stringify(dict));
        return JSON.stringify(dict);
    }
    static Deserialize(jsonString) {
        var dict = JSON.parse(jsonString);
        var newGrid = new IsometricGrid(dict.width, dict.length);
        dict.buildings.forEach(buildingDict => {
            var buildingClass = require("./Building"); //require("./" + buildingDict.type);
            var building = new buildingClass(buildingDict.x, buildingDict.y, buildingDict.width, buildingDict.height);
            building.type = buildingDict.type;
            newGrid.placeBuilding(building);
        });
        return newGrid;

    }
}
module.exports = IsometricGrid;