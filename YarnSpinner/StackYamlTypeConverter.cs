using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;
using Yarn;

namespace YarnSpinner
{
    class StackYamlTypeConverter : IYamlTypeConverter
    {
        public bool Accepts(Type type)
        {
            return type == typeof(Stack<Value>);
        }

        public object ReadYaml(IParser parser, Type type)
        {
            Stack<Value> result = new Stack<Value>();
            parser.Consume<SequenceStart>();

            while (parser.Accept<MappingStart>(out _))
            {
                result.Push(ReadValue(parser));
            }

            parser.Consume<SequenceEnd>();
            return result;
        }

        public void WriteYaml(IEmitter emitter, object value, Type type)
        {
            Stack<Value> stack = (Stack<Value>)value;
            emitter.Emit(new SequenceStart(null, null, false, SequenceStyle.Block));

            foreach (var item in stack.Reverse())
            {
                WriteValue(emitter, item);
            }

            emitter.Emit(new SequenceEnd());
        }

        void WriteValue(IEmitter emitter, Value value)
        {
            emitter.Emit(new MappingStart());

            emitter.Emit(new Scalar("Type"));
            emitter.Emit(new Scalar(value.type.ToString()));

            emitter.Emit(new Scalar("Value"));
            emitter.Emit(new Scalar(value.AsString));

            emitter.Emit(new MappingEnd());
        }

        Value ReadValue(IParser parser)
        {
            parser.Consume<MappingStart>();

            parser.Consume<Scalar>();
            var typeScalar = parser.Consume<Scalar>();
            
            parser.Consume<Scalar>();
            var valScalar = parser.Consume<Scalar>();

            parser.Consume<MappingEnd>();

            Value.Type valType = (Value.Type)Enum.Parse(typeof(Value.Type), typeScalar.Value);
            object val;
            switch (valType)
            {
                case Value.Type.Bool:
                    val = bool.Parse(valScalar.Value);
                    break;
                case Value.Type.Null:
                    val = null;
                    break;
                case Value.Type.Number:
                    if (valScalar.Value == "NaN")
                        val = float.NaN;
                    else
                        val = float.Parse(valScalar.Value);
                    break;
                case Value.Type.String:
                    val = valScalar.Value;
                    break;
                case Value.Type.Variable:
                    val = valScalar.Value;
                    break;
                default:
                    throw new ArgumentException("Unknown value type");
            }
            return new Value(val);
        }
    }
}
