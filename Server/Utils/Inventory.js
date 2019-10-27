const InventoryItem = require("./InventoryItem");
class Inventory {
    constructor() {
        this.inventoryList = [];
    }

    itemPresent(name) {
        //console.log(this.getItem(name))
        if (this.getItem(name) != null) {
            return true;
        } else {
            return false;
        }
    }
    getItem(name) {
        var item = null;
        this.inventoryList.forEach(inventoryItem => {
            if (inventoryItem.prefabName == name) {
                item = inventoryItem;
            }
        });
        return item
    }
    addItem(name, amount) {
        if (this.itemPresent(name)) {
            this.getItem(name).amount += amount;
        } else {
            var newItem = new InventoryItem(name, amount);
            this.inventoryList.push(newItem);
        }
    }
    removeItem(name, amount = 1) {
        var item = this.getItem(name)
        item.amount -= amount;
        if (item.amount <= 0) {
            this.inventoryList.splice(this.inventoryList.indexOf(item), 1);
        }
    }
    Serialize() {
        return JSON.stringify(this);
    }
    static Deserialize(jsonString) {
        var newInv = new Inventory();
        newInv.inventoryList = JSON.parse(jsonString).inventoryList;
        return newInv;
    }
}

module.exports = Inventory;