namespace BLL.Services
{
    public static class Extensions
    {
        public static int ToInt(this bool value) => value ? 1 : 0;
    }
}
