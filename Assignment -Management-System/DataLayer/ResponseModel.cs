namespace Assignment__Management_System.DataLayer
{
    public class ResponseModel<T>
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public T Result { get; set; }

    }
}
