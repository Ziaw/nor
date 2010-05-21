using Spark.Parser.Code;
using System.Linq;

namespace Spark.Nemerle.ChunkVisitors
{
    public static class TypeHelper
    {
        public static Snippets CorrectType(Snippets type)
        {
            foreach (var s in type)
                s.Value = s.Value.Replace('<', '[').Replace('>', ']');
            return type;
        }
    }
}