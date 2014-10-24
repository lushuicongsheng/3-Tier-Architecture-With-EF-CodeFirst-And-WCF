using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EasyEF.WCFClientProxy.HomeService;
using EasyEF.Common;
using System.ServiceModel;
using System.Xml;
using System.ServiceModel.Description;

namespace EasyEF.WCFClientProxy
{
    public class HomeServiceClass
    {
        public static Product CreateProduct(Product item)
        {
            HomeService.ServiceClient client = new HomeService.ServiceClient();
            return client.CreateProduct(item);
        }

        private const int maxReceivedMessageSize = 2147483647;

        public static PagedListOfProduct6Lx8ofhX GetProducts(int pageSize, int pageIndex, int categoryId = 0)
        {
            string uri = "http://localhost:4000/Service.svc";
            IService client = GetServiceClient(uri);
            return client.GetProducts(pageSize, pageIndex, categoryId);
        }

        private static IService GetServiceClient(string uri)
        {
            var binding = new BasicHttpBinding();
            binding.MaxReceivedMessageSize = maxReceivedMessageSize;
            binding.ReaderQuotas = new XmlDictionaryReaderQuotas();
            binding.ReaderQuotas.MaxStringContentLength = maxReceivedMessageSize;
            binding.ReaderQuotas.MaxArrayLength = maxReceivedMessageSize;
            binding.ReaderQuotas.MaxBytesPerRead = maxReceivedMessageSize;

            ChannelFactory<HomeService.IService> chan = new ChannelFactory<HomeService.IService>(binding, new EndpointAddress(uri));
            chan.Endpoint.Behaviors.Add(new EasyEF.WCFExtension.InspectorBehavior());
            foreach (var op in chan.Endpoint.Contract.Operations)
            {
                var dataContractBehavior = op.Behaviors.Find<DataContractSerializerOperationBehavior>() as DataContractSerializerOperationBehavior;
                if (dataContractBehavior != null)
                    dataContractBehavior.MaxItemsInObjectGraph = int.MaxValue;
            }
            chan.Open();
            var service = chan.CreateChannel();
            return service;
        }
    }
}
