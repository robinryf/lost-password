namespace RobinBird.Utilities.Unity.Editor.Helper
{
    public struct GuiSwitch<T>
    {
        public T Data { get; set; }

        public bool Active { get; set; }

        public void Activate()
        {
            Set(true);
        }
        
        public void Activate(T data)
        {
            Data = data;
            Set(true);
        }

        public void Deactivate()
        {
            Set(false);
        }

        public void Set(bool active)
        {
            Active = active;
        }

        public bool Use()
        {
            if (Active == false)
            {
                return false;
            }
            Deactivate();
            return true;
        }
    }
}