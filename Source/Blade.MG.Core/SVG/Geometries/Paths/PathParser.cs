using Microsoft.Xna.Framework;

namespace Blade.MG.SVG.Geometries.Paths
{
    //TODO: Handle Special Numerical Values
    // Instead of a standard numerical value, you can also use the following special values. These values are case-sensitive.
    //  Infinity : Represents Double.PositiveInfinity.
    // -Infinity : Represents Double.NegativeInfinity.
    // NaN       : Represents Double.NaN.
    internal static class PathParser
    {
        private enum ParserTokenType
        {
            Unknown,
            Command,
            Number,
            Vector
        }

        // https://docs.microsoft.com/en-us/dotnet/framework/wpf/graphics-multimedia/path-markup-syntax
        public static List<PathCommand> ParsePath(string path)
        {
            List<PathCommand> pathCommands = new List<PathCommand>();

            if (path == null)
            {
                throw new ArgumentNullException("path");
            }

            var pathSpan = path.AsSpan();

            ReadOnlySpan<char> token;
            ParserTokenType tokenType;
            Vector2 vector;

            int index = 0;
            while (GetNextToken(pathSpan, ref index, out token, out vector, out tokenType))
            {
                if (tokenType != ParserTokenType.Command)
                {
                    throw new Exception("Invalid Path at " + index);
                }

                char command = token[0];
                char commandUpper = Char.ToUpper(command);
                bool isRelative = !Char.IsUpper(command);

                if (commandUpper == 'M')
                {
                    // Move <point> [<point>] ...
                    RequireVectorToken(pathSpan, ref index, out token, out vector, out tokenType);

                    pathCommands.Add(new PathMove { IsRelative = isRelative, StartPoint = vector });


                    // Peek at the next token, if it's a point then treat it as a relative line command 'l'
                    if (PeekNextToken(pathSpan, index, out token, out vector, out tokenType))
                    {
                        if (tokenType != ParserTokenType.Command)
                        {
                            // Handle line command
                            command = isRelative ? 'l' : 'L';
                            commandUpper = 'L';
                            //isRelative = Same as Move Command;
                        }
                    }
                }

                if (commandUpper == 'M')
                {
                }
                else if (commandUpper == 'L')
                {
                    // Line <point> [<point>] ...
                    RequireVectorToken(pathSpan, ref index, out token, out vector, out tokenType);

                    var lineCommand = new PathLine { IsRelative = isRelative };
                    pathCommands.Add(lineCommand);

                    lineCommand.EndPoints.Add(vector);

                    // Peek at the next token, if it's a point then add to the list
                    while (TakeIfVectorToken(pathSpan, ref index, out token, out vector, out tokenType))
                    {
                        lineCommand.EndPoints.Add(vector);
                    }

                }
                else if (commandUpper == 'H')
                {
                    // Horizontal Line <number>
                    RequireNumberToken(pathSpan, ref index, out token, out vector, out tokenType);

                    pathCommands.Add(new PathHLine { IsRelative = isRelative, EndPointX = vector.X });
                }
                else if (commandUpper == 'V')
                {
                    // Verical Line <number>
                    RequireNumberToken(pathSpan, ref index, out token, out vector, out tokenType);

                    pathCommands.Add(new PathVLine { IsRelative = isRelative, EndPointY = vector.X });
                }
                else if (commandUpper == 'C')
                {
                    // Cubic Bezier <control point 1> <control point 2> <endPoint>
                    RequireVectorToken(pathSpan, ref index, out token, out Vector2 controlPoint1, out tokenType);
                    RequireVectorToken(pathSpan, ref index, out token, out Vector2 controlPoint2, out tokenType);
                    RequireVectorToken(pathSpan, ref index, out token, out Vector2 endPoint, out tokenType);

                    pathCommands.Add(new PathBezierCubic(isRelative, controlPoint1, controlPoint2, endPoint));
                }
                else if (commandUpper == 'Q')
                {
                    // Quadratic Bezier <control point> <endPoint>
                    RequireVectorToken(pathSpan, ref index, out token, out Vector2 controlPoint, out tokenType);
                    RequireVectorToken(pathSpan, ref index, out token, out Vector2 endPoint, out tokenType);

                    pathCommands.Add(new PathBezierQuadratic(isRelative, controlPoint, endPoint));
                }
                else if (commandUpper == 'S')
                {
                    // Smooth Cubic Bezier <control point 2> [<control point 3>] ... <endPoint>
                    // {control Point 1 is taken as the last control point in the previous Bezier}

                    var bezier = new PathBezierSmoothCubic();
                    bezier.IsRelative = isRelative;

                    RequireVectorToken(pathSpan, ref index, out token, out Vector2 controlPoint2, out tokenType);
                    bezier.ControlPoints.Add(controlPoint2);

                    RequireVectorToken(pathSpan, ref index, out token, out Vector2 endPoint, out tokenType);

                    while (TakeIfVectorToken(pathSpan, ref index, out token, out Vector2 controlPointNext, out tokenType))
                    {
                        // This isn't actually the endPoint, add it as a control point
                        bezier.ControlPoints.Add(endPoint);
                        // Use the new point as the endPoint
                        endPoint = controlPointNext;
                    }
                    bezier.EndPoint = endPoint;

                    pathCommands.Add(bezier);
                }
                else if (commandUpper == 'T')
                {
                    // Smooth Quadratic Bezier <control point> <endPoint>
                    var bezier = new PathBezierSmoothQuadratic();
                    bezier.IsRelative = isRelative;

                    RequireVectorToken(pathSpan, ref index, out token, out Vector2 endPoint, out tokenType);

                    while (TakeIfVectorToken(pathSpan, ref index, out token, out Vector2 controlPointNext, out tokenType))
                    {
                        // This isn't actually the endPoint, add it as a control point
                        bezier.ControlPoints.Add(endPoint);
                        // Use the new point as the endPoint
                        endPoint = controlPointNext;
                    }
                    bezier.EndPoint = endPoint;

                    pathCommands.Add(bezier);
                }
                else if (commandUpper == 'A')
                {
                    // Elliptic Arc <size> <rotationAngle> <isLargeArcFlag> <sweepDirectionFlag> <endPoint>
                    RequireVectorToken(pathSpan, ref index, out token, out Vector2 size, out tokenType);
                    RequireNumberToken(pathSpan, ref index, out token, out Vector2 rotationAngle, out tokenType);
                    RequireNumberToken(pathSpan, ref index, out token, out Vector2 isLargeArcFlag, out tokenType);
                    RequireNumberToken(pathSpan, ref index, out token, out Vector2 sweepDirectionFlag, out tokenType);
                    RequireVectorToken(pathSpan, ref index, out token, out Vector2 endPoint, out tokenType);

                    pathCommands.Add(new PathEllipticArc { IsRelative = isRelative, Size = size, RotationAngle = rotationAngle.X, IsLargeArc = isLargeArcFlag.X >= 0.999f, SweepDirection = sweepDirectionFlag.X >= 0.999f, EndPoint = endPoint });
                }
                else if (commandUpper == 'Z')
                {
                    // Close
                    pathCommands.Add(new PathClose { IsRelative = false });
                }
                else
                {
                    throw new Exception("Invalid Path at " + index);
                }
            }

            return pathCommands;
        }

        private static void RequireVectorToken(ReadOnlySpan<char> pathSpan, scoped ref int index, out ReadOnlySpan<char> token, out Vector2 vector, out ParserTokenType tokenType)
        {
            if (!TakeIfVectorToken(pathSpan, ref index, out token, out vector, out tokenType))
            {
                throw new Exception("Invalid Path at " + index);
            }
        }

        private static void RequireNumberToken(ReadOnlySpan<char> pathSpan, scoped ref int index, out ReadOnlySpan<char> token, out Vector2 vector, out ParserTokenType tokenType)
        {
            if (!TakeIfNumberToken(pathSpan, ref index, out token, out vector, out tokenType))
            {
                throw new Exception("Invalid Path at " + index);
            }
        }

        private static bool TakeIfVectorToken(ReadOnlySpan<char> pathSpan, scoped ref int index, out ReadOnlySpan<char> token, out Vector2 vector, out ParserTokenType tokenType)
        {
            int saveIndex = index;

            if (!GetNextToken(pathSpan, ref index, out token, out vector, out tokenType))
            {
                index = saveIndex;
                return false;
            }

            if (tokenType == ParserTokenType.Number)
            {
                if (TakeIfNumberToken(pathSpan, ref index, out ReadOnlySpan<char> token2, out Vector2 vector2, out tokenType))
                {
                    vector.Y = vector2.X;
                    return true;
                }
            }

            index = saveIndex;
            return false;
        }

        private static bool TakeIfNumberToken(ReadOnlySpan<char> pathSpan, scoped ref int index, out ReadOnlySpan<char> token, out Vector2 vector, out ParserTokenType tokenType)
        {
            int saveIndex = index;

            if (!GetNextToken(pathSpan, ref index, out token, out vector, out tokenType))
            {
                index = saveIndex;
                return false;
            }

            if (tokenType != ParserTokenType.Number)
            {
                index = saveIndex;
                return false;
            }

            return true;
        }

        private static bool PeekNextToken(ReadOnlySpan<char> pathSpan, int index, out ReadOnlySpan<char> token, out Vector2 vector, out ParserTokenType tokenType)
        {
            return GetNextToken(pathSpan, ref index, out token, out vector, out tokenType);
        }

        private static bool GetNextToken(ReadOnlySpan<char> pathSpan, scoped ref int index, out ReadOnlySpan<char> token, out Vector2 vector, out ParserTokenType tokenType)
        {
            token = null;
            vector = new Vector2();
            tokenType = ParserTokenType.Unknown;

            while (true)
            {
                if (index >= pathSpan.Length)
                {
                    return false;
                }

                char c = pathSpan[index];

                if (Char.IsWhiteSpace(c) || c == ',')
                {
                    // Skip spaces and separators
                    index++;
                    continue;
                }
                else if (Char.IsLetter(c))
                {
                    // Command
                    token = ParseCommand(pathSpan, ref index);
                    tokenType = ParserTokenType.Command;
                }
                else if (Char.IsDigit(c) || c == '.' || c == '+' || c == '-')
                {
                    // Numeric
                    token = ParseNumber(pathSpan, ref index);
                    tokenType = ParserTokenType.Number;

                    while (index < pathSpan.Length && Char.IsWhiteSpace(pathSpan[index]))
                    {
                        index++;
                    }

                    vector = new Vector2(float.Parse(token, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture), 0f);
                }
                else
                {
                    // Unkown Command
                    throw new Exception("Unknown Path Markup at " + index);
                }

                if (token != null)
                {
                    break;
                }
            }

            return true;
        }

        private static ReadOnlySpan<char> ParseCommand(ReadOnlySpan<char> pathSpan, scoped ref int index)
        {
            int startIndex = index;
            index++;

            return pathSpan.Slice(startIndex, index - startIndex);
        }

        private static ReadOnlySpan<char> ParseNumber(ReadOnlySpan<char> pathSpan, scoped ref int index)
        {
            bool seenDecimalSeparator = false;
            bool seenExponent = false;

            int startIndex = index;

            while (index < pathSpan.Length && (Char.IsDigit(pathSpan[index]) || pathSpan[index] == '.' || pathSpan[index] == '+' || pathSpan[index] == '-' || pathSpan[index] == 'E' || pathSpan[index] == 'e'))
            {
                if ((pathSpan[index] == '+' || pathSpan[index] == '-') && (index > startIndex))
                {
                    break;
                }

                if (pathSpan[index] == '.')
                {
                    if (seenDecimalSeparator)
                    {
                        break;
                    }
                    seenDecimalSeparator = true;
                }

                if (pathSpan[index] == 'E' || pathSpan[index] == 'e')
                {
                    if (seenExponent)
                    {
                        break;
                    }
                    seenExponent = true;
                }

                index++;
            }

            return pathSpan.Slice(startIndex, index - startIndex);
        }

    }
}
