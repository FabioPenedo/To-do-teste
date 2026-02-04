namespace To_do_teste.src.Context
{
    public class ToDoContext
    {
        private static readonly AsyncLocal<TenantContextData?> _toDoData = new();

        public static int? UserId => _toDoData.Value?.UserId;
        public static int? TaskId => _toDoData.Value?.TaskId;
        public static string? RequestId => _toDoData.Value?.RequestId;

        public static void Set(int? userId, int? taskId, string? requestId)
        {
            _toDoData.Value = new TenantContextData
            {
                UserId = userId,
                TaskId = taskId,
                RequestId = requestId
            };
        }

        public static void Clear()
        {
            _toDoData.Value = null;
        }

        private class TenantContextData
        {
            public int? UserId { get; set; }
            public int? TaskId { get; set; }
            public string? RequestId { get; set; }
        }
    }
}
