using System;
using System.Collections.Generic;
using System.Text;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;
using Yarn;

namespace YarnSpinner
{
    class KeyValuePairTypeConverter : IYamlTypeConverter
    {
        public bool Accepts(Type type)
        {
            return type == typeof(KeyValuePair<Line, string>);
        }

        public object ReadYaml(IParser parser, Type type)
        {
            parser.Consume<MappingStart>();

            parser.Consume<Scalar>();
            parser.Consume<MappingStart>();

            parser.Consume<Scalar>();
            string id = parser.Consume<Scalar>().Value;

            parser.Consume<Scalar>();
            parser.Consume<SequenceStart>();

            List<string> substs = new List<string>();
            while (parser.Accept<Scalar>(out _))
            {
                parser.Consume<Scalar>();
                substs.Add(parser.Consume<Scalar>().Value);
            }

            parser.Consume<SequenceEnd>();
            parser.Consume<MappingEnd>();

            parser.Consume<Scalar>();
            string value = parser.Consume<Scalar>().Value;

            parser.Consume<MappingEnd>();

            var line = new Line(id);
            line.Substitutions = substs.ToArray();
            return new KeyValuePair<Line, string>(line, value);
        }

        public void WriteYaml(IEmitter emitter, object value, Type type)
        {
            KeyValuePair<Line, string> pair = (KeyValuePair<Line, string>)value;
            emitter.Emit(new MappingStart());

            emitter.Emit(new Scalar("key"));
            emitter.Emit(new MappingStart());

            emitter.Emit(new Scalar("ID"));
            emitter.Emit(new Scalar(pair.Key.ID));

            emitter.Emit(new Scalar("Substitutions"));
            emitter.Emit(new SequenceStart(null, null, false, SequenceStyle.Block));

            foreach (var subst in pair.Key.Substitutions)
            {
                emitter.Emit(new Scalar(subst));
            }

            emitter.Emit(new SequenceEnd());
            emitter.Emit(new MappingEnd());

            emitter.Emit(new Scalar("Value"));
            emitter.Emit(new Scalar(pair.Value));

            emitter.Emit(new MappingEnd());
        }
    }
}
