
public class ChatService
{
    public string GetGroupName(string userId1, string userId2)
    {
        var stringCompare = string.Compare(userId1, userId2);
        return stringCompare < 0
            ? $"{userId1}-{userId2}"
            : $"{userId2}-{userId1}";
    }
}
