using KR_Lib.Statements;
using System;
using System.Collections.Generic;

namespace KR_Lib.Descriptions
{
    interface IDescription
    {
        /// <summary>
        /// Dodanie nowego zdania do domeny.
        /// </summary>
        /// <param name="statement">Nowe zdanie.</param>
        /// <returns>Identyfikator nowego zdania.</returns>
        Guid AddStatement(IStatement statement);
        void DeleteStatement(Guid guid);
    }
    public class Description : IDescription
    {
        public List<IStatement> statements;

        public Description()
        {}

        #region Public Methods
        /// <summary>
        /// Dodanie nowego zdania do domeny.
        /// </summary>
        /// <param name="statement">Nowe zdanie.</param>
        /// <returns>Identyfikator nowego zdania.</returns>
        public Guid AddStatement(IStatement statement)
        {
            statements.Add(statement);
            return statement.Id;
        }

        public void DeleteStatement(Guid guid)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
