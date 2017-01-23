using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOD2
{
    class Pipeline : Item
    {
        public decimal safetyLimit { get; set; }
        private Component input;
        private Component output;

        public Pipeline(Component input, Component output, decimal safteyLimit)
        {
            this.input = input;
            this.output = output;
            if(input.GetType() == typeof(Splitter))
            {
                ((Splitter)input).addOutput(this);
            }
            else
            {
                input.addOutput(this);
            }
            this.safetyLimit = safteyLimit;
            this.flow = input.getFlow();
            if(output.GetType() == typeof(Splitter))
            {
                ((Splitter)output).addInputPipeline(this);
            }
            else if (output.GetType() == typeof(Merger))
            {
                ((Merger)output).addInputPipeline(this);
            }
            else if (output.GetType() == typeof(Sink))
            {
                ((Sink)output).addInputPipeline(this);
            }
           
        }

        public override Component getNextComponent()
        {
            if(output != null)
            {
                return output;
            }
            else
            {
                return null;
            }
        }

        public override Pipeline getNextPipeline()
        {
            if(output != null)
            {
                return output.getNextPipeline();
            }
            else
            {
                return null;
            }
        }
        public Component getInput()
        {
            if(input != null)
            {
                return input;
            }
            else
            {
                return null;
            }
        }
    }
}
