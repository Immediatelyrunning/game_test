namespace game_test
{
    /// <summary>
    /// 静态实体管理类
    /// </summary>
    public class Static_graphic_manager
    {
        /// <summary>
        /// 绑定的窗口
        /// </summary>
        Form Form;
        /// <summary>
        /// 静态实体管理线程
        /// </summary>
        Thread Thread;
        /// <summary>
        /// 管理静态实体，先绑定窗口
        /// </summary>
        /// <param name="form"></param>
        public Static_graphic_manager(Form form)
        {
            Form = form;
            Play.Load_finished+=(sender, e) =>
            {
                Thread = new(Main);
                Thread.Start();
            };
        }
        void Main()
        {

        }
    }
}