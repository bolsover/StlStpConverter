using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;


namespace Bolsover.Converter
{
    public class Csys3D : Entity
    {
        private Direction Direction1 { get; set; }
        private Direction Direction2 { get; set; }
        private Point Point { get; set; }

        // Constructors
        public Csys3D(List<Entity> entityList) : base(entityList)
        {
        }

        public Csys3D(List<Entity> entityList, Direction direction1, Direction direction2, Point point) : base(entityList)
        {
            Direction1 = direction1;
            Direction2 = direction2;
            Point = point;
        }

        // Serialize method
        public override void Serialize(StreamWriter writer)
        {
            writer.WriteLine($"#{Id} = AXIS2_PLACEMENT_3D('{Label}',#{Point.Id},#{Direction1.Id},#{Direction2.Id});");
        }

        // ParseArgs using LINQ
        public override void ParseArgs(Dictionary<int, Entity> entityMap, string args)
        {
            var ids = ParseEntityIds(args);
            if (ids.Count >= 3)
            {
                Point = TryGetEntityById<Point>(entityMap, ids[0]);
                Direction1 = TryGetEntityById<Direction>(entityMap, ids[1]);
                Direction2 = TryGetEntityById<Direction>(entityMap, ids[2]);
            }
        }

        private static List<int> ParseEntityIds(string args)
        {
            var start = args.IndexOf(',');
            if (start == -1) return new List<int>();

            var argStr = args.Substring(start + 1)
                .Replace(",", " ")
                .Replace("#", " ");

            return argStr
                .Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(s => int.TryParse(s, out var val) ? val : 0)
                .ToList();
        }

        private static T TryGetEntityById<T>(Dictionary<int, Entity> entityMap, int id) where T : Entity
        {
            return entityMap.TryGetValue(id, out var entity) ? entity as T : null;
        }
    }
}