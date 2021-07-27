using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RestSharp;
using Serilog;
using Simple.OData.Client;

namespace ERPNextIntegration.QBO
{
    public class QuickbooksClient: ODataClient
    {
        private const string ClientId = "ABHtOGyc4wj6iUoNEDixurC6suN7dP5k9nNNIZlb2CayO3HkHg";
        private const string ClientSecret = "EZHWhIn6Za30koQ5IpyqvThE5Uv3k5LRqGCkr3wf";
        private const string RenewUrl = "https://oauth.platform.intuit.com/oauth2/v1/tokens/bearer";
        private const string RealmId = "4620816365039357720";
        public string RefreshToken = "AB11636126009DZyXc7vkv6pSdIZCeymDBSoQccKc8Lwz5wtzD";
        public const string BasicAuthorization =
            "QUJIdE9HeWM0d2o2aVVvTkVEaXh1ckM2c3VON2RQNWs5bk5OSVpsYjJDYXlPM0hrSGc6RVpIV2hJbjZaYTMwa29RNUlweXF2VGhFNVV2M2s1TFJxR0NrcjN3Zg==";
        public string AccessToken =
            "eyJlbmMiOiJBMTI4Q0JDLUhTMjU2IiwiYWxnIjoiZGlyIn0..5_fsVNEYsJF5Nu7uLByVdQ.a_qNfjK0fuJcU7rxsnP--mk7Pd-sl_IkYFDM7lMNyvwDwsxKqqDmA3BpbEaMPxLZJt8RoO_sPVoplx4dJm0rdegnrlZ_OqOsKlucngRY0rY9cTfLxT4lCsxJSVILJh5xu1pdbQbh_jmwoCA2kcBZNSUdkDc5BOiILrmtnmen5EWHNuvFvha6cQupFyc3VC_u8gNdWe6KWNZChN2GTDfcjZ_zmq8Db5tKPdgbERoJdfoFZxkFNmROIeEAU7hhpZUIgWfOgo2xAViYWzunQcTOsjlqf1gpbLsjh3IX5BPhOg24dRuQ_5c1LazYEVOZoMDwXVFhm04JOFD6iRuC154nTN5Ehc8MnT4mhAKN0_l_ykNQGpIAfxchk-ppWcyqzfgk7U48Ziw_Q5EMJmQoN4CVODpnhqXVvCjO1ZAShgPJnxUbaJPGrAvwb6_3lCCQMf-2CuAk3gyJDL2IkG_8WpwrSEVVqWwjysqMZWj-vhEUO_ItL0E2ajZIq24LEze-mPXrVjfNDKNLr6P5mOTEIbX3Dc4AFFJRbhQyjRfm9OnuNLpzc4lbIaLopa4yuR4fahGXkYc2McotNMwxx1M3XSQevdVb21UVVTcuQHtwR5VHPlgoydYeB8hPn_p5Vm-io3sNliM0tEbx4UxQvnal3V8ZbwZYzYfD47K2Z2QW3rCNx1cIcDyZULS1nYBuK4R4SLemb77XoQ1RncSvm0vPiJBc2Yd09jeNjOq6zTPp7h3k8GgVWU7IwSEgLBHCEaYxn7v5.yUocbBa9AtgDyWm__jp-gg";
        public DateTime LastQboLogin = DateTime.MinValue;
        public QuickbooksClient(ODataClientSettings clientSettings) : base(clientSettings)
        {

        }
    }
}