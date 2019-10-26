const Mongo = require("mongodb");

class Database {
    constructor(url) {
        this.url = url;
    }
    AddUser(username, baseData) {
        var MongoClient = Mongo.MongoClient
        MongoClient.connect(this.url, function (err, db) {
            if (err) throw err;
            var database = db.db("SpaceTravelGame");
            var obj = {
                "username": username,
                "baseData": baseData
            }
            database.collection("UserData").insertOne(obj, function (err, res) {
                if (err) throw err;
                console.log("WOOHOO USER ADDED");
            })
        })
    }
}

module.exports = Database;