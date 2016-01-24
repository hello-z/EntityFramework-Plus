﻿// Description: EF Bulk Operations & Utilities | Bulk Insert, Update, Delete, Merge from database.
// Website & Documentation: https://github.com/zzzprojects/Entity-Framework-Plus
// Forum: https://github.com/zzzprojects/EntityFramework-Plus/issues
// License: http://www.zzzprojects.com/license-agreement/
// More projects: http://www.zzzprojects.com/
// Copyright (c) 2015 ZZZ Projects. All rights reserved.

using System.Data.Common;
using System.Reflection;
#if EF5
using System.Data.EntityClient;

#elif EF6
using System.Data.Entity.Core.EntityClient;

#endif

namespace Z.EntityFramework.Plus
{
    public static partial class EntityConnectionExtensions
    {
        /// <summary>
        ///     A Database extension method that gets entity transaction.
        /// </summary>
        /// <param name="entityConnection">The @this to act on.</param>
        /// <returns>The entity transaction.</returns>
        public static DbTransaction GetStoreTransaction(this EntityConnection entityConnection)
        {
            var entityTransaction = entityConnection.GetEntityTransaction();

            if (entityTransaction == null)
            {
                return null;
            }

            // todo: Use field instead?
            var storeTransactionProperty = entityTransaction.GetType().GetProperty("StoreTransaction", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            return (DbTransaction) storeTransactionProperty.GetValue(entityTransaction, null);
        }
    }
}