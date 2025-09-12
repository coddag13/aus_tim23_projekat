using Common;

namespace dCom.ViewModel
{
    internal abstract class DigitalBase : BasePointItem, IDigitalPoint
    {
		private DState state;
		private DigitalOutputState digitalOutputState;

		public DigitalBase(IConfigItem c, IProcessingManager processingManager, IStateUpdater stateUpdater, IConfiguration configuration, int i) 
			: base(c, processingManager, stateUpdater, configuration, i)
		{
        }

		public DState State
		{
			get
			{
				return state;
			}

			set
			{
				state = value;
				OnPropertyChanged("State");
				OnPropertyChanged("DisplayValue");
			}
		}
        public DigitalOutputState DigitalOutputState
        {
            get
            {
                return digitalOutputState;
            }

            set
            {
                digitalOutputState = value;
                OnPropertyChanged("DigitalOutputState");
                OnPropertyChanged("DisplayValue");
            }
        }

        public override string DisplayValue
		{
			get
			{
                if (ConfigItem.RegistryType == PointType.DIGITAL_OUTPUT && ConfigItem.StartAddress==2000)
                    return DigitalOutputState.ToString();
                else
                    return State.ToString();
            }
		}

        protected override bool WriteCommand_CanExecute(object obj)
        {
            return false;
        }
    }
}
