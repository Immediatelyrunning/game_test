using game_test.Map;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace game_test.Event
{

    internal class Block_changed_event
    {
        public EventHandler Block_changed;
        Block block_real;
        public Block Block
        {
            get
            {
                return block_real;
            }
            set
            {
                if (!value.Equals(block_real))
                {
                    Block_changed.Invoke(this,EventArgs.Empty);
                    block_real = value;
                }
            }
        }
    }
}
