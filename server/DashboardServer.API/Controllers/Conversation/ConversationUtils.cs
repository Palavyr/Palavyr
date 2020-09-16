// using Server.Domain;
//
// namespace DashboardServer.API.Controllers
// {
//     public static class ConversationUtils
//     {
//         public static ConversationNode UpdateNodeProperties(ConversationNode oldNode, ConversationNode newNode)
//         {
//             if (newNode.Id != null)
//             {
//                 oldNode.Id = newNode.Id;
//             }
//             oldNode.Fallback = newNode.Fallback;
//             oldNode.Text = newNode.Text;
//             oldNode.AreaIdentifier = newNode.AreaIdentifier;
//             oldNode.IsRoot = newNode.IsRoot;
//             oldNode.NodeChildrenString = newNode.NodeChildrenString;
//             oldNode.NodeId = newNode.NodeId;
//             oldNode.NodeType = newNode.NodeType;
//             oldNode.OptionPath = newNode.OptionPath;
//             oldNode.IsCritical = newNode.IsCritical;
//             oldNode.AccountId = newNode.AccountId;
//
//             return oldNode;
//         }
//     }
// }