using System;

namespace Greggs.Products.Api.Customs
{
    public class CustomException : Exception
    {
        public CustomException(string message) : base(message)
        {
        }
    }

    public class ForbiddenException : CustomException
    {
        public ForbiddenException(string message) : base(message)
        {
        }
    }

    public class NotFoundException : CustomException
    {
        public NotFoundException(string message) : base(message)
        {
        }
    }

    public class BadRequestException : CustomException
    {
        public BadRequestException(string message) : base(message)
        {
        }
    }
}
