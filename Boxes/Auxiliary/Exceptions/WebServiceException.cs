using System;

namespace Boxes.Auxiliary.Exceptions
{
    /// <summary>
    ///     Exception levée lorsqu'une erreur survient concernant le web service.
    /// </summary>
    class WebServiceException : Exception
    {
        /// <summary>
        ///     Constructeur non paramétré avec message par défaut.
        /// </summary>
        public WebServiceException() : base("An error occured and your request couldn't be completed. Please try again.")
        { }
    }
}
