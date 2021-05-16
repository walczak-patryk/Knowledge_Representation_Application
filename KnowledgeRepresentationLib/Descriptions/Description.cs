using KR_Lib.Statements;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KR_Lib.Descriptions
{
    public interface IDescription
    {
        /// <summary>
        /// Dodanie nowego zdania do domeny.
        /// </summary>
        /// <param name="statement">Nowe zdanie.</param>
        /// <returns>Identyfikator nowego zdania.</returns>
        Guid AddStatement(IStatement statement);
        /// <summary>
        /// Usunięcie zdania z domeny.
        /// </summary>
        /// <param name="guid">Identyfikator zdania do usunięcia.</param>
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
            return statement.GetId();
        }

        /// <summary>
        /// Usunięcie zdania z domeny.
        /// </summary>
        /// <param name="guid">Identyfikator zdania do usunięcia.</param>
        public void DeleteStatement(Guid guid)
        {
            var statementToRemove = statements.SingleOrDefault(statement => statement.GetId() == guid);
            if (statementToRemove != null)
                statements.Remove(statementToRemove);
        }

        #endregion
    }
}
