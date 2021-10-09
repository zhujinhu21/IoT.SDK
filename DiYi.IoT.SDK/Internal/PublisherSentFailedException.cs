﻿using System;

namespace DiYi.IoT.SDK.Internal
{
    public class PublisherSentFailedException : Exception
    {
        public PublisherSentFailedException(string message) : base(message)
        {
        }

        public PublisherSentFailedException(string message, Exception ex) : base(message, ex)
        {
        }
    }
}