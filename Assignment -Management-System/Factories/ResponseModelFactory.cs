using Assignment__Management_System.DataLayer;

namespace Assignment__Management_System.Factories
{
    public class ResponseModelFactory
    {
        public ResponseModel<T> CreateResponseModel<T>(bool issuccess, string message, T result)
        {
            return new ResponseModel<T>
            {
                IsSuccess = issuccess,
                Message = message,
                Result = result
            };
        }
    }
}
