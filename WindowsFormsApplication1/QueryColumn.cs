using System;


namespace GCC
{
    public class QueryColumn
    {
        private readonly string fHeaderText;
        private readonly Type fColumnType;

        /// <summary>
        /// Initiates a new instance of a QueryColumn object
        /// </summary>
        /// <param name="headerText">Sets the Header Text of the Query Column</param>
        /// <param name="columnType">Sets the System.Type of the Query Column</param>
        public QueryColumn(string headerText, Type columnType)
        {
            fHeaderText = headerText;
            fColumnType = columnType;
        }

        /// <summary>
        /// Gets the header text of the column
        /// </summary>
        public string HeaderText
        {
            get
            {
                return fHeaderText;
            }
        }

        /// <summary>
        /// Gets the System.Type of the column
        /// </summary>
        public Type ColumnType
        {
            get
            {
                return fColumnType;
            }
        }
    }
}