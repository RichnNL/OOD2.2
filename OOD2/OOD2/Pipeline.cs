using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOD2
{
    class Pipeline : Item
    {
        public double safetyLimit { get; set; }
        public Component input;
        public Component output;

        public Pipeline(Component input, Component output, double safteyLimit)
        {
            this.input = input;
            this.output = output;
            this.safetyLimit = safteyLimit;
            if(input.GetType() == typeof(Splitter))
            {
                ((Splitter)input).addInput(this);
            }
            else if (input.GetType() == typeof(Merger))
            {
                ((Merger)input).addInput(this);
            }
            else if (input.GetType() == typeof(Pump))
            {
                ((Pump)input).addOutput(this);
            }
            output.addOutput(this);
        }
        public override void DrawSelf()
        {
            throw new NotImplementedException();
        }

        public override bool GetFlowFromInput()
        {
            throw new NotImplementedException();
        }

        public override bool SetOutputFlow()
        {
            throw new NotImplementedException();
        }
    }
}
