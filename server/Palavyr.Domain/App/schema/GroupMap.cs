using System.ComponentModel.DataAnnotations;

namespace Server.Domain
{
    public class GroupMap
    {
        private static GroupMap CreateInstance(string groupId, string parentGroup, string groupName, string accountId)
        {
            return new GroupMap(groupId, parentGroup, groupName, accountId);
        }

        [Key] public int Id { get; set; }
        public string GroupId { get; set; }
        private string ParentId { get; set; }
        public string GroupName { get; set; }
        public string AccountId { get; set; }

        private GroupMap(string groupId, string parentId, string groupName, string accountId)
        {
            GroupId = groupId;
            ParentId = parentId;
            GroupName = groupName;
            AccountId = AccountId;
        }
        
        public static GroupMap CreateGroupMap(string groupId, string parentGroup, string groupName, string accountId)
        {
            return CreateInstance(groupId, parentGroup, groupName, accountId);
        }
    }
}