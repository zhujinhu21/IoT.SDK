using System;
using System.Collections.Generic;
using System.Text;

namespace DiYi.IoT.SDK
{
    public class IoTOptions
    {

        public IoTOptions()
        {
           
            FailedRetryInterval = 60;
        
            Extensions = new List<IIoTOptionsExtension>();
           
        }

        internal IList<IIoTOptionsExtension> Extensions { get; }

        public int FailedRetryInterval { get; set; }

        /// <summary>
        /// The number of consumer thread connections.
        /// Default is 1
        /// </summary>
        public int ConsumerThreadCount { get; set; } = 1;

        /// <summary>
        /// The number of producer thread connections.
        /// Default is 1
        /// </summary>
        public int ProducerThreadCount { get; set; } = 1;

        /// <summary>
        /// Registers an extension that will be executed when building services.
        /// </summary>
        /// <param name="extension"></param>
        public void RegisterExtension(IIoTOptionsExtension extension)
        {
            if (extension == null)
            {
                throw new ArgumentNullException(nameof(extension));
            }

            Extensions.Add(extension);
        }
    }
}
