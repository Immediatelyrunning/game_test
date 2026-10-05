namespace game_test
{
    /*
        在winforms中往右下角是增加坐标，然后一个对象的初始点在对象的左上角
     */
    public class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            ApplicationConfiguration.Initialize();
            Application.Run(new Main_form());
        }
    }
}