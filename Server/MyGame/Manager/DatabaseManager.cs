using MyGame.Utils;

namespace MyGame
{
    internal class DatabaseManager : Singleton<DatabaseManager>
    {

        public MyGameEntities Entities { get; private set; }

        private DatabaseManager()
        {

        }

        public void Init()
        {
            Entities = new MyGameEntities();
            try
            {
                Entities.Database.Initialize(true);
            }
            catch (System.Exception e)
            {
                Log.Error(e);
            }
           
        }

        public void Save(bool isAsync = false)
        {
            if(isAsync)
            {
                Entities.SaveChangesAsync();
            }
            else
            {
                Entities.SaveChanges();
            }
        }
    }
}
