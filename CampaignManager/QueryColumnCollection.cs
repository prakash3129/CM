
using System.Collections;

namespace GCC
{
    public class QueryColumnCollection : CollectionBase
    {
        /// <summary>
        /// Add new column tot he QueryCollumnCollection object
        /// </summary>
        /// <param name="Column">The QueryCollumn object to be added to the collection</param>
        public void AddColumn(QueryColumn Column)
        {
            List.Add(Column);
        }
    }
}
