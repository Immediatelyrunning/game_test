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
        Block block_real=new(0,0);
        public Block Block
        {
            get
            {
                return block_real;
            }
            set
            {
                if (value==null)
                {
                    return;
                }
                else if (!value.Equals(block_real))
                {
                    Block_changed.Invoke(this,EventArgs.Empty);
                    block_real = value;
                }
            }
        }
    }
}
