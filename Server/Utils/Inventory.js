const InventoryItem = require("./InventoryItem");
class Inventory {
    constructor() {
        this.inventoryList = [];
    }

    itemPresent(name) {
        if (this.getItem(name) != null) {
            return true;
        } else {
            return false;
        }
    }
    getItem(name) {
        this.inventoryList.forEach(inventoryItem => {
            if (InventoryItem.prefabName == name) {
                return inventoryItem;
            }
        });
        return null;
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
        item = getItem(name)
        item.amount -= amount;
        if (item.amount <= 0) {
            this.inventoryList.slice(this.inventoryList.indexOf(item));
        }
    }
    Serialize() {
        return JSON.stringify(this);
    }
    static Deserialize(jsonString) {
        var newInv = new InventoryItem();
        newInv.inventoryList = JSON.parse(jsonString).inventoryList;
        return newInv;
    }
}

module.exports = Inventory;