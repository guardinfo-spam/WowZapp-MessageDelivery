using LOLCodeLibrary.ErrorsManagement;
using LOLMessageDelivery;
using LOLCodeLibrary;

namespace LOLMessageDelivery.Classes.ErrorsMgmt
{
    /// <summary>
    /// wrapper class for the General.Error struct
    /// </summary>
    public static class ErrorManagement
    {        
        /// <summary>
        /// creates and returns a new General.Error object
        /// </summary>
        /// <param name="errorMessage"></param>
        /// <param name="methodName"></param>
        /// <returns></returns>
        public static General.Error CreateErrorObject(SystemTypes.ErrorMessage errorMessage, string methodName)
        {
            General.Error errorObject = new General.Error();
            errorObject.ErrorDescription = errorMessage.ToString();
            errorObject.ErrorNumber = ((int)errorMessage).ToString();
            errorObject.ErrorLocation = methodName;
            errorObject.ErrorType = errorMessage;
            errorObject.ErrorTitle = methodName;
            return errorObject;
        }

        /// <summary>
        /// Use as a generic method to set an existing General.Error object
        /// </summary>
        /// <param name="errorMessage"></param>
        /// <param name="errorObject"></param>
        public static void SetErrorObject(SystemTypes.ErrorMessage errorMessage,ref General.Error errorObject, string methodName)
        {
            errorObject.ErrorDescription = errorMessage.ToString();
            errorObject.ErrorNumber = ((int)errorMessage).ToString();
            errorObject.ErrorLocation = methodName;
            errorObject.ErrorType = errorMessage;
        }

        /// <summary>
        /// checks if the properties of a General.Error object are set
        /// </summary>
        /// <param name="errorObject"></param>
        /// <returns></returns>
        public static bool IsErrorObjectValid(General.Error errorObject)
        {
            return !string.IsNullOrEmpty(errorObject.ErrorDescription) && !string.IsNullOrEmpty(errorObject.ErrorLocation) && !string.IsNullOrEmpty(errorObject.ErrorNumber);
        }

        public static General.Error CopyFrom(General.Error error)
        {
            General.Error result = new General.Error();
            result.ErrorDescription = error.ErrorDescription;
            result.ErrorLocation = error.ErrorLocation;
            result.ErrorNumber = error.ErrorNumber;
            result.ErrorTitle = error.ErrorTitle;
            result.ErrorType = error.ErrorType;

            return result;
        }

    }
}