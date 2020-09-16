using System.Collections.Generic;
using Server.Domain.DynamicTables;

namespace DashboardServer.API.receiverTypes
{
    public class DynamicTable
    {
        public List<SelectOneFlat> SelectOneFlat { get; set; }
        // add new Dynamic Table type rows properties
        public string TableTag { get; set; }
    }
}