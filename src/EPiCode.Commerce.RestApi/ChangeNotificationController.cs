using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using EPiServer.Events.ChangeNotification;
using EPiServer.ServiceLocation;

namespace EPiCode.Commerce.RestService
{
    public class ChangeNotificationController : SecuredApiController
    {
        private readonly IChangeNotificationManager _changeNotificationManager;

        public ChangeNotificationController()
            : this(ServiceLocator.Current.GetInstance<IChangeNotificationManager>())
        {
            
        }
        public ChangeNotificationController(IChangeNotificationManager changeNotificationManager)
        {
            _changeNotificationManager = changeNotificationManager;
        }

        [HttpGet]
        public IEnumerable<IChangeProcessorInfo> GetChangeProcessors()
        {
            // IChangeNotificationManager changeNotificationManager = ServiceLocator.Current.GetInstance<IChangeNotificationManager>();
            IEnumerable<IChangeProcessorInfo> processorInfos = _changeNotificationManager.GetProcessorInfo();
            return processorInfos;
        }

        [HttpGet]
        public IChangeProcessorInfo GetChangeProcessor(string name)
        {
            IEnumerable<IChangeProcessorInfo> processorInfos = _changeNotificationManager.GetProcessorInfo();
            IChangeProcessorInfo processorInfo = processorInfos.FirstOrDefault(c => c.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase));
            return processorInfo;
        }


        [HttpGet]
        public HttpResponseMessage Start()
        {
            _changeNotificationManager.Start();
            return CreateResponseMessage(HttpStatusCode.OK, "Change Notification Started");

        }

        [HttpGet]
        public HttpResponseMessage StartRecovery()
        {
            _changeNotificationManager.TryStartRecoveryAll();
            return CreateResponseMessage(HttpStatusCode.OK, "Started Recovery");

        }

        [HttpGet]
        public HttpResponseMessage Stop()
        {
            _changeNotificationManager.Stop();
            return CreateResponseMessage(HttpStatusCode.OK, "Change Notification Stopped");

        }


    }
}
