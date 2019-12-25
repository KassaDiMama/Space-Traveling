using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
class FriendsList
{
    public List<Friend> friends = new List<Friend>();
    public FriendsList()
    {

    }
    public void addFriend(Friend friend)
    {
        this.friends.Add(friend);
    }
    public string Serialize()
    {
        return JsonConvert.SerializeObject(this);
    }
    public static FriendsList Deserialize(string jsonString)
    {
        FriendsList friendsList = new FriendsList();
        JObject json = JObject.Parse(jsonString);
        foreach (var friend in json["friends"])
        {
            Friend newFriend = new Friend();
            newFriend.username = friend["username"].Value<string>();
            friendsList.addFriend(newFriend);
        }
        return friendsList;
    }
}