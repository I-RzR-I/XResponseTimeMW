using Microsoft.AspNetCore.Http;
using RzR.Web.Middleware.ResponseTime.Internals;

namespace XResponseTimeMW.Tests.Internals
{
    [TestClass]
    public class ServerTimingHeaderTests
    {
        [TestMethod]
        public void Append_Writes_Metric_With_One_Decimal_When_Header_Missing_Test()
        {
            var ctx = new DefaultHttpContext();

            ServerTimingHeader.Append(ctx.Response, "total", 12);

            Assert.AreEqual("total;dur=12.0", ctx.Response.Headers["Server-Timing"].ToString());
        }

        [TestMethod]
        public void Append_Preserves_Existing_Metrics_Via_StringValues_Concat_Test()
        {
            var ctx = new DefaultHttpContext();
            ctx.Response.Headers["Server-Timing"] = "db;dur=4.2";

            ServerTimingHeader.Append(ctx.Response, "action", 8);

            var values = (string[])ctx.Response.Headers["Server-Timing"]!;
            CollectionAssert.AreEqual(new[] { "db;dur=4.2", "action;dur=8.0" }, values);
        }

        [TestMethod]
        public void Append_Uses_Invariant_Culture_For_Decimal_Separator_Test()
        {
            var previous = System.Threading.Thread.CurrentThread.CurrentCulture;
            try
            {
                System.Threading.Thread.CurrentThread.CurrentCulture =
                    new System.Globalization.CultureInfo("de-DE");

                var ctx = new DefaultHttpContext();
                ServerTimingHeader.Append(ctx.Response, "action", 7);

                // de-DE would render "7,0" — must be "7.0"
                StringAssert.Contains(ctx.Response.Headers["Server-Timing"].ToString(), "7.0");
            }
            finally
            {
                System.Threading.Thread.CurrentThread.CurrentCulture = previous;
            }
        }

        [TestMethod]
        public void Append_With_Description_Quotes_The_Text_Test()
        {
            var ctx = new DefaultHttpContext();

            ServerTimingHeader.Append(ctx.Response, "db", 3.5, "primary read replica");

            Assert.AreEqual(
                "db;dur=3.5;desc=\"primary read replica\"",
                ctx.Response.Headers["Server-Timing"].ToString());
        }

        [TestMethod]
        public void Append_Is_Noop_For_Null_Response_Test()
        {
            // Should not throw.
            ServerTimingHeader.Append(null, "x", 1);
        }
    }
}


