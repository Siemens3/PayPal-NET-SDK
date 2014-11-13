﻿using System;
using System.Web;
using PayPal;
using PayPal.Api;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

// ##Reauthorization Sample
// Sample showing how to do a reauthorization
namespace PayPal.Sample
{
    public partial class Reauthorization : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            HttpContext CurrContext = HttpContext.Current;
            Authorization authorization = null;
            try
            {
                // ### Api Context
                // Pass in a `APIContext` object to authenticate 
                // the call and to send a unique request id 
                // (that ensures idempotency). The SDK generates
                // a request id if you do not pass one explicitly. 
                 // See [Configuration.cs](/Source/Configuration.html) to know more about APIContext..
                APIContext apiContext = Configuration.GetAPIContext();

                // ###Reauthorization
                // Make a authorized payment using `PayPal Account Payments` with intent
                // as `authorize`. You can reauthorize a payment only once 4 to 29
                // days after 3-day honor period for the original authorization
                // expires.
                authorization = Authorization.Get(apiContext, "8HD57954KS1107638");
                Amount reauthorizeAmount = new Amount();
                reauthorizeAmount.currency = "USD";
                reauthorizeAmount.total = "1";
                authorization.amount = reauthorizeAmount;
                Authorization reauthorization = authorization.Reauthorize(apiContext);
                CurrContext.Items.Add("ResponseJson", Common.FormatJsonString(reauthorization.ConvertToJson()));
            }
            catch (PayPalException ex)
            {
                CurrContext.Items.Add("Error", ex.Message);
            }
            CurrContext.Items.Add("RequestJson", Common.FormatJsonString(authorization.ConvertToJson()));
            Server.Transfer("~/Response.aspx");
        }
    }
}