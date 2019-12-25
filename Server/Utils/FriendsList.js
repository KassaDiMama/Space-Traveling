class FriendsList {
    constructor() {
        this.friends = [];
    }
    addFriend(friend) {
        this.friends.push(friend);
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

}

module.exports = FriendsList;