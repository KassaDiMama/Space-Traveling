const Friend = require("./Friend");
class FriendsList {
    constructor() {
        this.friends = [];
    }
    addFriend(friend) {
        if (this.findFriend(friend.username) == null) {
            this.friends.push(friend);
        }

    }
    findFriend(username) {
        this.friends.forEach(friend => {
            if (friend.username = username) {
                return friend;
            }
        });
        return null;
    }
    removeFriend(username) {
        var index = this.friends.indexOf(this.findFriend(username));
        if (index >= 0) {
            this.friends.slice(index);
        } else {
            console.log("Friend doesn't exist");
        }

    }
    Serialize() {
        return JSON.stringify(this);
    }
    static Deserialize(jsonString) {
        var json = JSON.parse(jsonString);
        var friendsList = new FriendsList();
        json.friends.forEach(friendJson => {
            var friend = new Friend();
            friend.username = friendJson;
            friendsList.friends.push(friend);
        });
        return friendsList;
    }

}

module.exports = FriendsList;