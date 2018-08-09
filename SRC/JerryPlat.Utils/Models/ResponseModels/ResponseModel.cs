using JerryPlat.Utils.Helpers;

namespace JerryPlat.Utils.Models
{
    public class ResponseModel<T>
    {
        public static ResponseModel<T> Ok(T data, string Message = "")
        {
            if (data == null)
            {
                return NotFound();
            }

            return new ResponseModel<T>
            {
                Status = ConstantHelper.Ok,
                Message = Message,
                Data = data
            };
        }

        public static ResponseModel<T> Error(string errorMessage)
        {
            return new ResponseModel<T>
            {
                Status = ConstantHelper.Error,
                Message = errorMessage,
                Data = default(T)
            };
        }

        public static ResponseModel<T> Invalid(string errorMessage)
        {
            return new ResponseModel<T>
            {
                Status = ConstantHelper.Invalid,
                Message = errorMessage,
                Data = default(T)
            };
        }

        public static ResponseModel<T> NotFound()
        {
            return new ResponseModel<T>
            {
                Status = ConstantHelper.NotFound,
                Message = string.Empty,
                Data = default(T)
            };
        }

        public static ResponseModel<T> Existed()
        {
            return new ResponseModel<T>
            {
                Status = ConstantHelper.Existed,
                Message = string.Empty,
                Data = default(T)
            };
        }

        public static ResponseModel<T> Logout(T data)
        {
            return new ResponseModel<T>
            {
                Status = ConstantHelper.Logout,
                Message = string.Empty,
                Data = data
            };
        }

        public string Status { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
    }
}