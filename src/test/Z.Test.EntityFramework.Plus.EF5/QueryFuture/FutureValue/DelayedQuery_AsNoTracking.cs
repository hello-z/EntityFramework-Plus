﻿// Description: EF Bulk Operations & Utilities | Bulk Insert, Update, Delete, Merge from database.
// Website & Documentation: https://github.com/zzzprojects/Entity-Framework-Plus
// Forum: https://github.com/zzzprojects/EntityFramework-Plus/issues
// License: http://www.zzzprojects.com/license-agreement/
// More projects: http://www.zzzprojects.com/
// Copyright (c) 2015 ZZZ Projects. All rights reserved.

using System.Data.Entity;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Z.EntityFramework.Plus;

namespace Z.Test.EntityFramework.Plus
{
    public partial class QueryFuture_FutureValue
    {
        [TestMethod]
        public void DelayedQuery_AsNoTracking()
        {
            TestContext.DeleteAll(x => x.Entity_Basics);
            TestContext.Insert(x => x.Entity_Basics, 10);

            using (var ctx = new TestContext())
            {
                // BEFORE
                var futureValue1 = ctx.Entity_Basics.Where(x => x.ColumnInt < 5).AsNoTracking().DeferredCount().FutureValue();
                var futureValue2 = ctx.Entity_Basics.Where(x => x.ColumnInt >= 5).AsNoTracking().DeferredCount().FutureValue();

                // TEST: The batch contains 2 queries
                Assert.AreEqual(2, QueryFutureManager.AddOrGetBatch(ctx.GetObjectContext()).Queries.Count);

                var value = futureValue1.Value;

                // AFTER

                // TEST: The batch contains 0 queries
                Assert.AreEqual(0, QueryFutureManager.AddOrGetBatch(ctx.GetObjectContext()).Queries.Count);

                // TEST: The futureValue1 has a value and the count equal 5
                Assert.IsTrue(futureValue1.HasValue);
                Assert.AreEqual(5, value);

                // TEST: The futureValue2 has a value and the count equal 5
                Assert.IsTrue(futureValue2.HasValue);
                Assert.AreEqual(5, futureValue2.Value);

                // TEST: No entries has been loaded in the change tracker (A value is returned, not an entity)
                Assert.AreEqual(0, ctx.ChangeTracker.Entries().Count());
            }
        }
    }
}